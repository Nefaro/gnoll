using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using GnollModLoader.Lua;
using GnollModLoader.Integration.MoonSharp.Loaders;
using GnollModLoader.Lua.Proxy;

namespace GnollModLoader
{
    public class LuaManager
    {
        private readonly static string SCRIPT_DIR_NAME = "Scripts";
        private readonly static string CUSTOM_INIT_SCRIPT_PATH = GnomanEmpire.SaveFolderPath($"Gnoll\\{SCRIPT_DIR_NAME}\\");
        private readonly static string CUSTOM_INIT_SCRIPT_NAME = "CustomInit.lua";

        private readonly static string LUA_SUPPORT_MOD_NAME = "LuaSupport";

        private readonly static CoreModules DEFAULT_CORE_MODULES = CoreModules.Preset_HardSandbox |
                CoreModules.Json |
                CoreModules.ErrorHandling |
                CoreModules.Coroutine |
                CoreModules.OS_Time |
                CoreModules.LoadMethods |
                CoreModules.Metatables;
        // Stricter rule, we need nothing more than to read the file and verify it's a mod
        private readonly static CoreModules MODINFO_CORE_MODULES = CoreModules.Preset_HardSandbox |
                CoreModules.LoadMethods;

        // {mod.Name -> {script.path, script}
        private readonly Dictionary<string, Tuple<string, Script>> _scriptRegistry = new Dictionary<string, Tuple<string, Script>> ();
        private readonly Dictionary<string, object> _globalTables = new Dictionary<string, object>();

        private readonly HookManager _hookManager;
        private readonly SaveGameManager _saveGameManager;
        private readonly LuaHookManager _luaHookManager;

        private string _luaSupportInitScript;
        private bool _initDone = false;

        public LuaManager(HookManager hookManager, SaveGameManager saveGameManager) 
        {
            this._hookManager = hookManager;
            this._saveGameManager = saveGameManager;
            this._luaHookManager = new LuaHookManager(hookManager, this);
        }

        /**
         * Run script Lua function referenced with the function name. 
         * Can include optional arguments that are expected by the Lua function
         */
        public void RunLuaFunction(string functionName, params object[] args)
        {
            this.findAndRunLuaFunctionWithArguments(functionName, args);
        }

        internal void RegisterMod(IGnollMod mod, string modAbsolutePath)
        {
            var initScript = this.generatePathForMod(modAbsolutePath, SCRIPT_DIR_NAME) + "\\ModInit.lua";

            // XXX: DEBUG line for local testing; leaving it in for now
            //var initScript = Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\ModInit.lua";

            if (LUA_SUPPORT_MOD_NAME == mod.Name)
            {
                // LuaSupport mod gets registered only if it's enabled
                _luaSupportInitScript = initScript;
            }
            else
            {
                this._scriptRegistry[mod.Name] = new Tuple<string, Script>(initScript, null);
            }
            var luaMod = mod as IHasLuaScripts;
            luaMod.AttachScriptRunner(this.ScriptRunnerForMod(mod.Name));
        }

        internal LuaScriptRunner ScriptRunnerForMod(string modName)
        {
            var runner = new LuaScriptRunner();
            runner.RunnerDelegate = (functionName, args) =>
            {
                var script = this._scriptRegistry[modName].Item2;
                
                runLuaFunction(modName, script, functionName, args);
            };
            return runner;
        }

        internal void RunInitScripts()
        {
            if (!VerifyLuaIntegrationEnabled())
            {
                Logger.Log("Lua Support DISABLED");
                return;
            }

            if (File.Exists(this.getCustomInitScriptLocation()))
            {
                this._scriptRegistry["CustomInit"] = new Tuple<string, Script>(this.getCustomInitScriptLocation(), null);
            }

            foreach (var modName in new List<string>(this._scriptRegistry.Keys))
            {
                var initScript = this._scriptRegistry[modName].Item1;
                try
                {
                    Logger.Log($"Trying to load Lua init script: {initScript}");
                    if (File.Exists(initScript))
                    {
                        var script = this.loadAndGetScript(modName, initScript);
                        this._scriptRegistry[modName] = new Tuple<string, Script>(initScript, script);
                        if (script != null)
                        {
                            Logger.Log($"-- Init script for '{modName}' loaded successfully");
                        }
                        else
                        {
                            Logger.Error($"Init script for mod '{modName}' failed");
                        }
                    }
                    else
                    {
                        Logger.Warn($"Mod registered for script loading but init script missing: {initScript}");
                    }
                }
                catch (InterpreterException ex)
                {
                    Logger.Error($"Init script with mod '{modName}' failed");
                    Logger.Error($"-- {ex.DecoratedMessage}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Init script for mod '{modName}' failed");
                    Logger.Error($"-- {ex}");
                }
            }
        }

        internal void UnloadMod(IGnollMod mod)
        {
            try
            {
                Logger.Log($"-- Unloading mod '{mod.Name}' from script handler");
                this._scriptRegistry.Remove(mod.Name);
            }
            catch (Exception ex)
            {
                Logger.Error($"Unloading mod '{mod.Name}' failed");
                Logger.Error($"-- {ex}");
            }
        }
        internal IGnollMod LoadModInfo(string filePath)
        {
            if (!VerifyLuaIntegrationEnabled())
            {
                Logger.Log("Lua Support DISABLED");
                return null;
            }
            var modInfoFilename = Path.GetFullPath(filePath);

            Script script = new Script(MODINFO_CORE_MODULES);

            ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[] {
                Path.GetDirectoryName(modInfoFilename) + $"\\{SCRIPT_DIR_NAME}\\?",
                Path.GetDirectoryName(modInfoFilename) + $"\\{SCRIPT_DIR_NAME}\\?.lua",
            };

            Logger.Log("Module paths: ");
            foreach (string path in ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths)
            {
                Logger.Log($"-- {path}");
            }

            // Add all global tables
            foreach (KeyValuePair<string, object> entry in _globalTables)
            {
                script.Globals[entry.Key] = entry.Value;
            }
            Table modinfoTable = script.DoFile(modInfoFilename).Table;
            LuaModInfo modInfo = this.mapFromTable(script, modinfoTable);

            script.Options.DebugPrint = s => { LuaLogger.Log(modInfo.Name, s); };

            return modInfo;
        }

        public bool VerifyLuaIntegrationEnabled()
        {
            // verify that the Lua ingeration should be enabled
            // for this check if "LuaSuppot" mod is enabled
            if ( _luaSupportInitScript == null )
            {
                return false;
            }
            if ( _initDone ) 
            {
                return true;
            }
            // Lua support is enabled, init the subsystem
            this.init();

            // Debug hooks
            this._hookManager.InGameHUDInit += this.hookInGameHudInit;

            // Real hooks
            this._luaHookManager.AttachHooks();

            // some special hooks that require to be last to initialize
            this._hookManager.AfterGameLoaded += this.hookLuaOnGameLoaded;
            this._hookManager.AfterGameSaved += this.hookLuaOnGameSave;
            
            return true;
        }

        // Init after it's clear if the Lua subsystem is enabled or not
        private void init()
        {
            Logger.Log("Lua Support ENABLED, initializing ...");
            this.setDefaultOptions();
            this.registerTypes();

            // Custom Lua Global Table
            // Available in every script
            this._globalTables[GnomoriaGlobalTable.GLOBAL_TABLE_LUA_NAME] = new GnomoriaGlobalTable();
            this._globalTables[JobBoardGlobalTable.GLOBAL_TABLE_LUA_NAME] = new JobBoardGlobalTable();
            this._globalTables[GuiHelperGlobalTable.GLOBAL_TABLE_LUA_NAME] = new GuiHelperGlobalTable();

            // Force Table to always be dictionary. Helps with game saving
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(object),
                dyntype =>
                {
                    var dict = new Dictionary<object, object>();
                    var table = dyntype.Table;
                    foreach (var pair in table.Pairs)
                    {
                        dict[pair.Key.ToObject()] = pair.Value.ToObject();
                    }
                    return dict;
                });
            Logger.Log("Lua Support initialization ... DONE");
            this._initDone = true;
        }

        private void registerTypes()
        {
            // Game Defs
            GameDefsProxyRegistry.RegisterTypes();

            // Game Entities
            EntityProxyRegistry.RegisterTypes();
            // Game GUI stuff
            GuiProxyRegistry.RegisterTypes();
            // Jobs
            JobsProxyRegistry.RegisterTypes();

            // C# Objects
            UserData.RegisterType<Vector4>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<Color>();

            // Pseudo Global Table, can contain various "helpers"
            UserData.RegisterType<GnomoriaGlobalTable>();
            UserData.RegisterType<JobBoardGlobalTable>();
            UserData.RegisterType<GuiHelperGlobalTable>();

            // hiding the internal functionality
            UserData.RegisterProxyType<SaverProxy, SaveGameManager.Saver>(t => new SaverProxy(t));
            UserData.RegisterProxyType<LoaderProxy, SaveGameManager.Loader>(t => new LoaderProxy(t));

            // TODO: Those need to be proxied as well
            //UserData.RegisterType<Game.Character>();
            //UserData.RegisterType<Game.GameEntity>();
        }
        private void setDefaultOptions()
        {
            // Insert our own implementation
            Script.DefaultOptions.ScriptLoader = new FilesystemScriptLoader();
            // Redirect lua "print" to our log console
            Script.DefaultOptions.DebugPrint = s => { LuaLogger.Log("", s ); };
        }

        private void runLuaFunction(string modName, Script script, string functionName, params object[] args)
        {
            if (script == null)
            {
                return;
            }

            try
            {
                var func = script.Globals[functionName];
                if (func != null)
                {
                    script.Call(func, args);
                }
            }
            catch (InterpreterException ex)
            {
                Logger.Error($"LUA Error: {ex.DecoratedMessage}");
            }
            catch (Exception ex)
            {
                Logger.Error($"LUA Error: {ex}");
            }
        }

        private void runValidationScripts()
        {
            foreach (var entry in this._scriptRegistry)
            {
                var script = entry.Value.Item2;
                Logger.Log($"-- Calling Lua function: [{entry.Key}] OnRunScriptValidation");
                runLuaFunction(entry.Key, script, "OnRunScriptValidation");
            }
        }

        private void reloadAllScripts()
        {
            foreach (string key in this._scriptRegistry.Keys.ToList())
            {
                var value = this._scriptRegistry[key];
                try
                {
                    var initScriptPath = value.Item1;
                    Logger.Log($"-- Trying to reload Lua script: {initScriptPath}");
                    var script = loadAndGetScript(key, initScriptPath);
                    if ( script != null )
                    {
                        this._scriptRegistry[key] = new Tuple<string, Script>(initScriptPath, script);
                    }
                }
                catch (Exception ex)
                {
                    this._scriptRegistry[key] = new Tuple<string, Script>(value.Item1, null);
                    Logger.Error($"Reloading script for '{key}' failed");
                    Logger.Error($"-- {ex}");
                }
            }
        }

        private Script loadAndGetScript(string modName, string scriptPath)
        {
            try {
                Script script = new Script(DEFAULT_CORE_MODULES);

                script.Options.DebugPrint = s => { LuaLogger.Log(modName, s); };
                script.Globals["Color"] = UserData.CreateStatic<Color>();

                ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[]
                {
                    Path.GetDirectoryName(scriptPath) + "\\?",
                    Path.GetDirectoryName(scriptPath) + "\\?.lua",
                    Path.GetDirectoryName(_luaSupportInitScript) + "\\?",
                    Path.GetDirectoryName(_luaSupportInitScript) + "\\?.lua"
                };

                Logger.Log("Module paths: ");
                foreach (string path in ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths)
                {
                    Logger.Log($"-- {path}");
                }

                // Add all global tables
                foreach (KeyValuePair<string, object> entry in _globalTables)
                {
                    script.Globals[entry.Key] = entry.Value;
                }
                script.DoFile(scriptPath);
                return script;
            }
            catch (InterpreterException ex)
            {
                Logger.Error($"Init script with path '{scriptPath}' failed");
                Logger.Error($"-- {ex.DecoratedMessage}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"Init script with path '{scriptPath}' failed");
                Logger.Error($"-- {ex}");
                return null;
            }
        }

        private void hookInGameHudInit(InGameHUD inGameHUD, Manager manager)
        {
            // if debug, hook up Lua debug buttons
            if (GnollMain.Debug)
            {
                Logger.Log("Attaching Lua buttons");
                this.attachIngameUI(inGameHUD, manager);
            }
        }

        private void hookLuaOnGameLoaded()
        {
            this.findAndRunLuaFunctionForEntry("OnSaveGameLoaded", entry => _saveGameManager.LoaderForMod(entry.Key));
        }

        private void hookLuaOnGameSave()
        {
            this.findAndRunLuaFunctionForEntry("OnGameSave", entry => _saveGameManager.SaverForMod(entry.Key));
        }

        private void findAndRunLuaFunctionWithArguments(string functionName, params object[] args)
        {
            foreach (var entry in this._scriptRegistry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(entry.Key, script, functionName, args);
            }
        }

        private void findAndRunLuaFunctionForEntry(string functionName, Func<KeyValuePair<string, Tuple<string, Script>>, object> entryProcessor)
        {
            foreach (var entry in this._scriptRegistry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(entry.Key, script, functionName, entryProcessor(entry));
            }
        }

        private void attachIngameUI(InGameHUD inGameHUD, Manager manager)
        {
            GClass0 glass = inGameHUD.GClass0_0;
            Button button7 = glass.method_40("Lua Reload", "Reload Lua scripts", new Game.GUI.Controls.EventHandler((sender, events) =>
            {
                this.reloadAllScripts();
                Logger.Log($"-- -- Lua Scripts reloaded -- -- ");
            }));
            glass.panel_0.Add(button7);
            button7.Left = glass.panel_0.Width + button7.Margins.Left;
            glass.panel_0.Width = button7.Left + button7.Width;
            glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;

            Button button8 = glass.method_40("Lua Validate", "Run Lua validation scripts", new Game.GUI.Controls.EventHandler((sender, events) =>
            {
                this.runValidationScripts();
                Logger.Log($"-- -- Lua Validation done -- -- ");
            }));
            glass.panel_0.Add(button8);
            button8.Left = glass.panel_0.Width + button8.Margins.Left;
            glass.panel_0.Width = button8.Left + button8.Width;
            glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;
        }

        private string generatePathForMod(string modAbsolutePath, string subDirectory)
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GNOLL_WORKSPACE")))
            {
                // For debugging
                return genrateOverridePathForScript(modAbsolutePath);
            }
            return modAbsolutePath + "\\" + subDirectory;
        }

        // For debugging
        private string genrateOverridePathForScript(string modAbsolutePath)
        {
            Logger.Log($"Abs path {modAbsolutePath}");
            Logger.Warn("Overriding Lua script location!");
            var path = Path.GetFileName(Path.GetDirectoryName(modAbsolutePath + "\\"));
            path = Path.Combine(Environment.GetEnvironmentVariable("GNOLL_WORKSPACE"), GnollMain.MODS_DIR, path, path, SCRIPT_DIR_NAME);
            Logger.Log($"Path {path}");
            return path;
        }

        private string getCustomInitScriptLocation()
        {
            return CUSTOM_INIT_SCRIPT_PATH + CUSTOM_INIT_SCRIPT_NAME;
        }

        private LuaModInfo mapFromTable(Script script, Table table) 
        { 
            LuaModInfo modInfo = new LuaModInfo();

            foreach (var prop in typeof(LuaModInfo).GetProperties())
            {
                var attr = prop.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault() as RequiredAttribute;

                var luaVal = table.Get(prop.Name);
                if (luaVal.IsNil() && attr != null)
                    throw new ArgumentNullException($"Expected mandatory property '{prop.Name}', got null value");

                if (prop.PropertyType == typeof(string) && luaVal.Type == DataType.String) {
                    string value = (luaVal.IsNil() ? "" : luaVal.String);
                    // Limit the string length
                    if ( attr != null && attr.MaxLength != -1 && attr.MaxLength < value.Length)
                    {
                        value = value.Substring(0, attr.MaxLength);
                    }
                    prop.SetValue(modInfo, value, null);
                }
                else if (prop.PropertyType == typeof(int) && luaVal.Type == DataType.Number)
                    prop.SetValue(modInfo, luaVal.IsNil() ? -1 : (int)luaVal.Number, null);
                else if (prop.PropertyType == typeof(bool) && luaVal.Type == DataType.Boolean)
                    prop.SetValue(modInfo, !luaVal.IsNil() && luaVal.Boolean, null);
            }

            DynValue onEnable = table.Get("OnEnable");
            if (onEnable.Type == DataType.Function)
            {
                // Map the Lua function to a C# delegate
                modInfo.OnEnableDelegate = () =>
                {
                    // Call the Lua function; pass the table as 'self' + argument
                    script.Call(onEnable, table);
                };
            }

            DynValue onDisable = table.Get("OnDisable");
            if (onEnable.Type == DataType.Function)
            {
                // Map the Lua function to a C# delegate
                modInfo.OnDisableDelegate = () =>
                {
                    // Call the Lua function; pass the table as 'self' + argument
                    script.Call(onDisable, table);
                };
            }

            return modInfo;
        }

        // Lua ModInfo support
        [AttributeUsage(AttributeTargets.Property)]
        private class RequiredAttribute : Attribute 
        {
            public int MaxLength { get; set; }
            public RequiredAttribute() { }
        }

        private class LuaModInfo : IGnollMod, IHasLuaScripts
        {
            // 36 chars max
            [RequiredAttribute(MaxLength=36)] public string Name { get; set; }
            // 134 chars max
            [RequiredAttribute(MaxLength=134)] public string Description { get; set; }
            [RequiredAttribute] public int RequireMinPatchVersion { get; set; }

            [RequiredAttribute(MaxLength = 12)] public string BuiltWithLoaderVersion { get; set; }

            public bool IsDefaultEnabled { get; set; } = false;

            public bool NeedsRestartOnToggle { get; set; } = false;

            public Action OnEnableDelegate { get; set; }

            public Action OnDisableDelegate { get; set; }
            public void OnEnable(HookManager hookManager)
            {
                OnEnableDelegate?.Invoke();
            }

            public void OnDisable(HookManager hookManager)
            {
                OnDisableDelegate?.Invoke();
            }

            bool IGnollMod.IsDefaultEnabled()
            {
                return IsDefaultEnabled;
            }

            bool IGnollMod.NeedsRestartOnToggle()
            {
                return NeedsRestartOnToggle;
            }

            public void AttachScriptRunner(LuaScriptRunner scriptRunner)
            {
                // no-op; scripts are run natively
            }
        }
    }
}

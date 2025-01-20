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
        // {mod.Name -> {script.path, script}
        private readonly Dictionary<string, Tuple<string, Script>> _scriptRegistry = new Dictionary<string, Tuple<string, Script>> ();
        private readonly Dictionary<string, object> _globalTables = new Dictionary<string, object>();

        private readonly HookManager _hookManager;
        private readonly SaveGameManager _saveGameManager;
        private readonly LuaHookManager _luaHookManager;

        private string _luaSupportInitScript;
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

        internal void RegisterMod(IGnollMod mod, Assembly modAssembly)
        {
            var initScript = this.generatePathForMod(modAssembly, SCRIPT_DIR_NAME) + "\\ModInit.lua";

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
        }

        internal void RunInitScripts()
        {
            if (!verifyLuaIntegrationEnabled())
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

        private bool verifyLuaIntegrationEnabled()
        {
            // verify that the Lua ingeration should be enabled
            // for this check if "LuaSuppot" mod is enabled
            if ( _luaSupportInitScript == null )
            {
                return false;
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
            
            Logger.Log("Lua Support initialization ... DONE");
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
                    Logger.Log($"-- Calling Lua function: [{modName}] {functionName}");
                    script.Call(func, args);
                }
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

                ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = [
                    Path.GetDirectoryName(scriptPath) + "\\?",
                    Path.GetDirectoryName(scriptPath) + "\\?.lua",
                    Path.GetDirectoryName(_luaSupportInitScript) + "\\?",
                    Path.GetDirectoryName(_luaSupportInitScript) + "\\?.lua"
                ];

                // XXX: DEBUG lines for local testing; leaving them in for now
                /*
                ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = [
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\?",
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\?.lua",
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\LuaSupport\\LuaSupport\\Scripts\\?",
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\LuaSupport\\LuaSupport\\Scripts\\?.lua"
                ];
                */

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
            catch(Exception ex)
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
                this.AttachIngameUI(inGameHUD, manager);
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

        private void AttachIngameUI(InGameHUD inGameHUD, Manager manager)
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

        private string generatePathForMod(Assembly modAssembly, string subDirectory)
        {
            string assembly = Path.GetDirectoryName(modAssembly.Location);
            string dll = modAssembly.GetName().Name;
            string folder = assembly + "\\" + dll + "\\" + subDirectory;
            return folder;
        }

        private string getCustomInitScriptLocation()
        {
            return CUSTOM_INIT_SCRIPT_PATH + CUSTOM_INIT_SCRIPT_NAME;
        }
    }

}

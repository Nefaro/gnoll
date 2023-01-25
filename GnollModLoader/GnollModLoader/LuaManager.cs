using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Game;
using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using GameLibrary;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Linq.Expressions;

namespace GnollModLoader
{
    public class LuaManager
    {
        private readonly static string SCRIPT_DIR_NAME = "Scripts";

        private readonly Dictionary<String, Tuple<String,Script>> _registry = new Dictionary<String, Tuple<String, Script>> ();

        private HookManager _hookManager;
        public LuaManager(HookManager hookManager) 
        {
            this._hookManager = hookManager;
            this.init();
        }

        public void init()
        {

            var loader = Script.DefaultOptions.ScriptLoader;
            Script.DefaultOptions.DebugPrint = s => { LuaLogger.Log(Newtonsoft.Json.JsonConvert.ToString(s)); };

            //((ScriptLoaderBase)loader).IgnoreLuaPathGlobal = true;
            // ((ScriptLoaderBase)loader).ModulePaths = ScriptLoaderBase.UnpackStringPaths("C:/Users/svenson/workspace/gnoll/GnollModLoader/GnollModLoader/Scripts/?;" +
            //    "C:/Users/svenson/workspace/gnoll/GnollModLoader/GnollModLoader/Scripts/?.lua");

            /*
            var script = this.loadAndGetScript();

            if (script == null)
            {
                Logger.Error("Script null");
                return;
            }*/

            UserData.RegisterType<Game.Item>();
            UserData.RegisterType<Game.Character>();
            UserData.RegisterType<Game.GameEntity>();
            UserData.RegisterType<Game.GameDefs>();
            UserData.RegisterType<GameLibrary.ItemDef>();
            UserData.RegisterType<GameLibrary.PlantDef>();
            UserData.RegisterType<GameLibrary.MaterialProperty>();
            UserData.RegisterType<GameLibrary.PlantSettings>();

            this._hookManager.OnEntitySpawn += (Game.GameEntity entity) =>
            {
                //Logger.Log($"Entity type {entity.Name()}");
                /*
                var func = script.Globals["OnEntitySpawn"];
                if (func != null )
                {
                    //Logger.Log($"Calling OnEntitySpawn for: {entity.Name()}");
                    script.Call(func, entity);
                }*/
            };

            this._hookManager.BeforeStartNewGameAfterReadDefs += (Game.CreateWorldOptions options) =>
            {
                foreach (var entry in this._registry)
                {
                    var script = entry.Value.Item2;
                    runLuaFunction(script, "OnGameDefsLoaded", GnomanEmpire.Instance.GameDefs);
                }
            };
        }

        private static void runLuaFunction(Script script, string functionName, params object[] args)
        {
            try
            {
                var func = script.Globals[functionName];
                if (func != null)
                {
                    Logger.Log($"Call {functionName}");
                    script.Call(func, args);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"LUA Error: {ex}");
            }
        }

        public void HookInGameHudInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            if (GnollMain.Debug)
            {
                Logger.Log("Attaching LUA button");
                this.AttachIngameUI(inGameHUD, manager);
            }
        }

        private void reloadAllscripts()
        {
            foreach (string key in this._registry.Keys.ToList())
            {
                var value = this._registry[key];
                try
                {
                    var initScriptPath = value.Item1;
                    Logger.Log($"-- Trying to reloading Lua script: {initScriptPath}");
                    var script = loadAndGetScript(initScriptPath);
                    if ( script != null )
                    {
                        this._registry[key] = new Tuple<string, Script>(initScriptPath, script);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Reloading script for '{key}' failed");
                    Logger.Error($"-- {ex}");
                }
            }
        }

        private Script loadAndGetScript(string scriptPath)
        {
            Script script = new Script(
                CoreModules.Preset_HardSandbox | 
                CoreModules.Json | 
                CoreModules.ErrorHandling | 
                CoreModules.Coroutine | 
                CoreModules.OS_Time | 
                CoreModules.LoadMethods |
                CoreModules.Metatables
                );
            ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[] {
                Path.GetDirectoryName(scriptPath) + "\\?",
                Path.GetDirectoryName(scriptPath) + "\\?.lua" };
            script.DoFile(scriptPath);
            return script;
        }

        private void AttachIngameUI(InGameHUD inGameHUD, Manager manager)
        {
            GClass0 glass = inGameHUD.GClass0_0;
            Button button7 = glass.method_40("LUA Reload", "LUA", new Game.GUI.Controls.EventHandler((sender, events) =>
            {
                this.reloadAllscripts();
                Logger.Log($"-- -- LUA Scripts reloaded -- -- ");
            }));
            glass.panel_0.Add(button7);
            button7.Left = glass.panel_0.Width + button7.Margins.Left;
            glass.panel_0.Width = button7.Left + button7.Width;
            glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;
        }

        internal void RegisterMod(IGnollMod mod, Assembly modAssembly)
        {
            try
            {
                var initScript = this.generatePathForMod(modAssembly, SCRIPT_DIR_NAME) + "\\OnModInit.lua";
                Logger.Log($"-- Trying to load Lua init script: {initScript}");
                if (System.IO.File.Exists(initScript))
                {
                    var script = this.loadAndGetScript(initScript);
                    if (script != null)
                    {
                        this._registry[mod.Name] = new Tuple<string, Script>(initScript, script);
                        Logger.Log($"-- Init script for '{mod.Name}' loaded successfully");
                    }
                }
                else
                {
                    Logger.Warn($"Mod registered for script loading but init script missing: {initScript}");
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"Init script for mod '{mod.Name}' failed");
                Logger.Error($"-- {ex}");
            }
        }
        internal void UnloadMod(IGnollMod mod)
        {
            try
            {
                Logger.Log($"-- Unloading mod '{mod.Name}' from script handler");
                this._registry.Remove(mod.Name);
            }
            catch(Exception ex)
            {
                Logger.Error($"Unloading mod '{mod.Name}' failed");
                Logger.Error($"-- {ex}");
            }
        }

        private string generatePathForMod(Assembly modAssembly, string subDirectory)
        {
            string assembly = Path.GetDirectoryName(modAssembly.Location);
            string dll = modAssembly.GetName().Name;
            string folder = assembly + "\\" + dll + "\\" + subDirectory;
            return folder;
        }
    }

}

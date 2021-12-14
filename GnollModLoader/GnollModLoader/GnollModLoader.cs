using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Game;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollModLoader
{
    public class GnollModLoader
    {
        private readonly HookManager _hookManager;
        private readonly List<IGnollMod> _modsList = new List<IGnollMod>();

        public List<IGnollMod> Mods { get { return _modsList; } }

        public GnollModLoader(HookManager hookManager)
        {
            this._hookManager = hookManager;
        }

        public void LoadModsFrom(string dir)
        {
            String mask = "*.dll";
            if (!Directory.Exists(dir))
            {
                Logger.Log("-- Mod directory missing; no mods loaded");
                return;
            }
            foreach (String filename in Directory.EnumerateFiles(dir, mask, SearchOption.AllDirectories))
            {
                LoadMod(filename);
            }
            this._hookManager.RegisterMods(this.Mods);

            if (GnollMain._debug)
            {
                this._hookManager.InGameHUDInit += DEBUG_HookManager_InGameHUDInit;
                this._hookManager.UpdateInGame += DEBUG_HookManager_UpdateInGame;
                this._hookManager.OnJobComplete += DEBUG_HookManager_OnJobComplete;
                this._hookManager.InGameShowWindow += DEBUG_HookManager_InGameShowWindow;
                this._hookManager.BeforeEntitySpawn += DEBUG_HookManager_BeforeEntitySpawn;
                this._hookManager.OnEntitySpawn += DEBUG_HookManager_OnEntitySpawn;
                this._hookManager.MainMenuGuiInit += DEBUG_HookManager_MainMenuGuiInit;
                this._hookManager.BeforeStartNewGame += DEBUG_HookManager_BeforeStartNewGame;
                this._hookManager.BeforeStartNewGameAfterReadDefs += DEBUG_HookManager_BeforeStartNewGameAfterReadDefs;
            }
        }

        public bool LoadMod(string path)
        {
            Logger.Log("-- Checking {0} for mods ... ", path);
            Assembly assembly = Assembly.LoadFrom(path);

            try
            {
                // Only load classes that implement the given interface
                var searchType = typeof(IGnollMod);

                foreach (var type in this.GetAssemblyTypes(assembly))
                {
                    if (searchType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IGnollMod mod = (IGnollMod)Activator.CreateInstance(type);
                        if ( GnollMain.PATCH_VERSION >= mod.RequireMinPatchVersion )
                        {
                            Logger.Log("-- ++ Instantiating mod: " + mod.Name);

                            _modsList.Add(mod);
                            mod.OnLoad(_hookManager);
                            return true;
                        }
                        else
                        {
                            Logger.Log("-- -- Validation failed for mod: " + mod.Name);
                            Logger.Log("-- -- Current patch version {0}, mod required patch version {1} or higher  ", GnollMain.PATCH_VERSION, mod.RequireMinPatchVersion);
                            return false;
                        }
                    }
                }
            }
            catch (System.TypeLoadException e)
            {
                Logger.Log("-- -- Trying to load mod from '{0}' failed with exception");
                Logger.Log("-- -- {0}", e);
                return false;
            }
            Logger.Log("-- -- No mods found from '{0}'", path);
            return false;
        }

        private IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        private void DEBUG_HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
        }

        private void DEBUG_HookManager_UpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
        }

        private void DEBUG_HookManager_OnJobComplete(Game.Job job, Game.Character character)
        {
        }       

        private void DEBUG_HookManager_InGameShowWindow(Window window)
        {
        }

        private void DEBUG_HookManager_BeforeEntitySpawn(GameEntity entity)
        {
        }

        private void DEBUG_HookManager_OnEntitySpawn(GameEntity entity)
        { 
        }

        private void DEBUG_HookManager_MainMenuGuiInit(MainMenuWindow window, Manager manager)
        {
        }

        private void DEBUG_HookManager_BeforeStartNewGame(CreateWorldOptions worldOptions)
        {
        }

        private void DEBUG_HookManager_BeforeStartNewGameAfterReadDefs(CreateWorldOptions worldOptions)
        {
        }
    }
}

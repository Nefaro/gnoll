using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GameLibrary;
using GnollModLoader.GUI;

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
                System.Console.WriteLine("-- Mod directory missing; no mods loaded");
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
            }
        }

        public void LoadMod(string path)
        {
            System.Console.WriteLine("-- Loading mod from: '" + path + "'");
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
                        System.Console.WriteLine("-- Instantiating mod: " + mod.Name);

                        _modsList.Add(mod);
                        mod.OnLoad(_hookManager);
                    }
                }
            }
            catch (System.TypeLoadException)
            {
                System.Console.WriteLine("-- Trying to load mod failed; Maybe not a Gnoll compatible nod?");
            }
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollModLoader
{
    public class GnollModLoader
    {
        private readonly HookManager _hookManager;
        private readonly List<IMod> _modsList = new List<IMod>();

        public List<IMod> Mods { get { return _modsList;  }}

        public GnollModLoader(HookManager hookManager)
        {
            this._hookManager = hookManager;
        }

        public void LoadMod(string path)
        {
            System.Console.WriteLine("-- Loading mod from: '" + path + "'");
            Assembly assembly = Assembly.LoadFrom(path);

            try
            {
                Type type = assembly.GetType("Mod.ModMain");

                if (type != null)
                {
                    IMod mod = (IMod)Activator.CreateInstance(type);
                    System.Console.WriteLine("-- Instantiating mod: " + mod.Name);

                    _modsList.Add(mod);
                    mod.OnLoad(_hookManager);
                }
            }
            catch(System.TypeLoadException)
            {
                System.Console.WriteLine("-- Trying to load mod failed; Maybe not a Gnoll compatible nod?");
            }
        }

        public void LoadModsFrom(string dir)
        {
            String mask = "*.dll";
            if( !Directory.Exists(dir) )
            {
                System.Console.WriteLine("-- Mod directory missing; no mods loaded");
                return;
            }
            foreach (String filename in Directory.EnumerateFiles(dir, mask, SearchOption.AllDirectories))
            {
                LoadMod(filename);
            }
            this._hookManager.RegisterMods(this.Mods);
        }
    }
}

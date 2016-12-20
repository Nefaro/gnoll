using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModLoader
{
    public class ModManager
    {
        private HookManager hookManager;
        List<IMod> mods = new List<IMod>();

        public ModManager(HookManager hookManager)
        {
            this.hookManager = hookManager;
        }

        public void LoadMod(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);

            Type type = assembly.GetType("Mod.ModMain");

            IMod mod = (IMod) Activator.CreateInstance(type);
            mods.Add(mod);

            mod.OnLoad(hookManager);
        }

        public void ScanDirectory(string dir)
        {
            String mask = "*.dll";

            foreach (String filename in Directory.EnumerateFiles(dir, mask, SearchOption.AllDirectories))
            {
                LoadMod(filename);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace GnollModLoader
{
    /**
     * Responsible for finding, validating and loading the mods from .dll files
     */
    public class ModLoader
    {
        private readonly ModManager _modManager;

        public ModLoader(ModManager modManager)
        {
            this._modManager = modManager;
        }

        public void LoadModsFrom(string dir)
        {
            String mask = "*.dll";
            if (!Directory.Exists(dir))
            {
                Logger.Error("Mod directory missing; no mods loaded");
                return;
            }
            foreach (String filename in Directory.EnumerateFiles(dir, mask, SearchOption.AllDirectories))
            {
                LoadMod(filename);
            }
            Logger.Log("Loading mods ... DONE");
        }

        public bool LoadMod(string path)
        {
            Logger.Log("Checking '{0}' for mods ... ", path);
            Assembly assembly = Assembly.LoadFrom(path);

            try
            {
                // Only load classes that implement the given interface
                var searchType = typeof(IGnollMod);

                foreach (var type in this.getAssemblyTypes(assembly))
                {
                    if (searchType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IGnollMod mod = (IGnollMod)Activator.CreateInstance(type);
                        if ( GnollMain.PATCH_VERSION >= mod.RequireMinPatchVersion )
                        {
                            Logger.Log("++ Instantiating mod: " + mod.Name);
                            this._modManager.RegisterMod(mod, assembly);
                            return true;
                        }
                        else
                        {
                            Logger.Log("-- Validation failed for mod: " + mod.Name);
                            Logger.Log("-- Current patch version {0}, mod required patch version {1} or higher  ", GnollMain.PATCH_VERSION, mod.RequireMinPatchVersion);
                            return false;
                        }
                    }
                }
            }
            catch (System.TypeLoadException e)
            {
                Logger.Error("Trying to load mod from '{0}' failed with exception");
                Logger.Error("{0}", e);
                return false;
            }
            Logger.Log("-- No mods found from '{0}'", path);
            return false;
        }

        private IEnumerable<Type> getAssemblyTypes(Assembly assembly)
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
    }
}

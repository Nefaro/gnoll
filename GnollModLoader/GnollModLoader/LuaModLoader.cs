using System;
using System.IO;

namespace GnollModLoader
{
    internal class LuaModLoader
    {
        private readonly ModManager _modManager;
        private readonly LuaManager _luaManager;

        public LuaModLoader(ModManager modManager, LuaManager luaManager)
        {
            this._modManager = modManager;
            this._luaManager = luaManager;
        }

        internal void LoadModsFrom(string dir)
        {
            Logger.Log("Loading Lua mods ...");

            if (!_luaManager.VerifyLuaIntegrationEnabled())
                return;

            string mask = "ModInfo.lua";
            if (!Directory.Exists(dir))
            {
                Logger.Error("Mod directory missing; no mods loaded");
                return;
            }
            foreach (string filename in Directory.EnumerateFiles(dir, mask, SearchOption.AllDirectories))
            {
                _loadMod(filename);
            }
            Logger.Log("Loading Lua mods ... DONE");
        }

        private void _loadMod(string path)
        {
            Logger.Log("Checking '{0}' for mods ... ", path);
            try
            {
                // Only load classes that implement the given interface
                IGnollMod mod = this._luaManager.LoadModInfo(path);
                if (GnollMain.PATCH_VERSION >= mod.RequireMinPatchVersion)
                {
                    Logger.Log("++ Instantiating mod: " + mod.Name);
                    this._modManager.RegisterMod(mod, Path.GetFullPath(Path.GetDirectoryName(path)));
                    return;
                }
                else
                {
                    Logger.Log("-- Validation failed for mod: " + mod.Name);
                    Logger.Log("-- Current patch version {0}, mod required patch version {1} or higher  ", GnollMain.PATCH_VERSION, mod.RequireMinPatchVersion);
                    return;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Trying to load mod from '{0}' failed with exception", path);
                Logger.Error("{0}", e);
            }
            Logger.Log("-- Cannot load mods from '{0}'", path);
        }
    }
}

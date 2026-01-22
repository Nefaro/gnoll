using System;
using Game;
using GnollModLoader;

namespace GnollMods.WildernessTrees
{
    public class ModMain : IGnollMod, IHasDirectPatch, IHasLuaScripts
    {
        public static ModsLogger Logger { get; set; }
        public static LuaScriptRunner Runner { get; set; }
        public string Name => "WildernessTrees";

        public string Description => "An experimental mod adjusting Tree growth";

        public string BuiltWithLoaderVersion => "G1.15.0";

        public int RequireMinPatchVersion => 15;

        public void ApplyPatch(Patcher patcher)
        {
            var orig = typeof(Tree).GetMethod(nameof(Tree.Update));
            var prefixPatch = typeof(Patch_Tree_Update).GetMethod(nameof(Patch_Tree_Update.Prefix));
            patcher.ApplyDirectPatch(orig, prefixPatch: prefixPatch);
        }

        public void AttachScriptRunner(LuaScriptRunner scriptRunner)
        {
            Runner = scriptRunner;
        }

        public bool IsDefaultEnabled()
        {
            return false;
        }

        public bool NeedsRestartOnToggle()
        {
            return true;
        }

        public void OnDisable(HookManager hookManager)
        {
            
        }

        public void OnEnable(HookManager hookManager)
        {
            
        }
    }
    
    internal class Patch_Tree_Update
    {
        public static bool Prefix(ref Tree __instance, float dt)
        {
            ModMain.Runner.RunFunction("OnTreeUpdate", __instance, dt);
            return Patcher.CONTINUE_CHAIN;
        }
    }
}

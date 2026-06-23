using System.Collections.Generic;
using Game;
using GnollModLoader;

namespace GnollMods.FixResourcePileDeath
{
    public class ModMain : IGnollMod, IHasDirectPatch
    {
        public static ModsLogger Logger { get; set; }

        public string Name => "FixResourcePileDeath";

        public string Description => "Fixes a game crash caused by resource pile dying";

        public string BuiltWithLoaderVersion => "G1.15.3";

        public int RequireMinPatchVersion => 15;

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        public void OnDisable(HookManager hookManager)
        {
            //do nothing
        }

        public void OnEnable(HookManager hookManager)
        {
            //do nothing
        }

        public void ApplyPatch(Patcher patcher)
        {
            var orig = typeof(GameEntityManager).GetMethod(nameof(GameEntityManager.method_7));
            var prefixPatch = typeof(Patch_GameEntityManager).GetMethod(nameof(Patch_GameEntityManager.Method7_Prefix));
            patcher.ApplyDirectPatch(orig, prefixPatch: prefixPatch);
        }

        public ModMain()
        {
            Logger = ModsLogger.getLogger(this);
        }
    }

    internal class Patch_GameEntityManager
    {

        public static bool Method7_Prefix(ref GameEntityManager __instance)
        {
            // make a defencive copy
            List<GameEntity> list = new List<GameEntity>(__instance.list_5);
            __instance.list_5.Clear();
            foreach (GameEntity gameEntity_ in list)
            {
                __instance.method_9(gameEntity_);
            }

            return Patcher.SKIP_CHAIN;
        }
    }
}

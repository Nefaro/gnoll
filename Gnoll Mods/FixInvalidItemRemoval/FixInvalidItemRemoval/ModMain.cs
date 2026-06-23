using System;
using Game;
using GameLibrary;
using GnollModLoader;

namespace FixInvalidItemRemoval
{
    public class ModMain : IGnollMod, IHasDirectPatch
    {
        public string Name => "FixInvalidItemRemoval";

        public string Description => "Fixes a game crash caused by invalid item definition";

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
            var orig = typeof(Fortress).GetMethod(nameof(Fortress.method_2));
            var prefixPatch = typeof(Patch_Fortress).GetMethod(nameof(Patch_Fortress.Method2_Prefix));
            patcher.ApplyDirectPatch(orig, prefixPatch: prefixPatch);
        }
    }

    internal class Patch_Fortress
    {
        public static bool Method2_Prefix(ref Fortress __instance, Item item_0)
        {
            ItemDef itemDef = GnomanEmpire.Instance.GameDefs.ItemDef(item_0.ItemID);
            if (itemDef != null)
            {
                __instance.method_4(item_0, itemDef.Effects);
            }
            if (__instance.stockManager_0.IsItemInStocks(item_0))
            {
                __instance.uint_0 -= Math.Min(item_0.Value(), __instance.uint_0);
            }
            return Patcher.SKIP_CHAIN;
        }
    }
}

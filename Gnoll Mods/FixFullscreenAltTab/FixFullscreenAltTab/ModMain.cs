using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using GnollModLoader;

namespace GnollMods.FixFullscreenAltTab
{
    internal class ModsMain : IGnollMod, IHasDirectPatch
    {
        public string Name { get { return "FixFullscreenAltTab"; } }
        public string Description { get { return "This mod trys to fix the alt-tab crashing issue. Disable this, if you don't use fullscreen."; } }
        public string BuiltWithLoaderVersion { get { return "G1.13"; } }
        public int RequireMinPatchVersion { get { return 13; } }


        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return true;
        }

        public void OnDisable(HookManager hookManager)
        {
            // do nothing
        }

        public void OnEnable(HookManager hookManager)
        {
            // do nothing
        }


        public void ApplyPatch(Patcher patcher)
        {
            var orig = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.Draw));
            var prefixPatch = typeof(Patch_GnomanEmpire).GetMethod(nameof(Patch_GnomanEmpire.Draw_prefix));
            var postfixPatch = typeof(Patch_GnomanEmpire).GetMethod(nameof(Patch_GnomanEmpire.Draw_postfix));
            patcher.ApplyDirectPatch(orig, prefixPatch: prefixPatch, postfixPatch: postfixPatch);
        }
    }

    internal class Patch_GnomanEmpire
    {
        static bool drawing = false;

        public static bool Draw_prefix()
        {
            if (drawing)
            {
                return Patcher.SKIP_CHAIN;
            }
            drawing = true;
            return Patcher.CONTINUE_CHAIN;
        }

        public static void Draw_postfix()
        {
            drawing = false;
        }
    }
}

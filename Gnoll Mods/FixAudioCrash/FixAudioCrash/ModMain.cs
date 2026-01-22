using System;
using Game;
using GnollModLoader;

namespace GnollMods.FixAudioCrash
{
    public class ModMain : IGnollMod, IHasDirectPatch
    {
        public static ModsLogger Logger { get; set; }

        public string Name => "FixAudioCrash";

        public string Description => "A fix for audio crash preventing save game loading. Disable this mod if you have no trouble loading save games.";

        public string BuiltWithLoaderVersion => "G1.15.0";

        public int RequireMinPatchVersion => 15;

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
            //do nothing
        }

        public void OnEnable(HookManager hookManager)
        {
            //do nothing
        }

        public ModMain()
        {
            Logger = ModsLogger.getLogger(this);
        }

        public void ApplyPatch(Patcher patcher)
        {
            var orig = typeof(AudioManager).GetMethod(nameof(AudioManager.Update));
            var finalizer = typeof(Patch_AudioManager).GetMethod(nameof(Patch_AudioManager.Finalizer));
            var prefixPatch = typeof(Patch_AudioManager).GetMethod(nameof(Patch_AudioManager.Prefix));
            patcher.ApplyDirectPatch(orig, prefixPatch: prefixPatch, finalizer: finalizer);
        }
    }

    internal class Patch_AudioManager
    {
        private static readonly int LIMIT = 500;
        private static int counter = 0;

        public static bool Prefix()
        {
            if ( counter >= LIMIT)
            {
                return Patcher.SKIP_CHAIN;
            }
            return Patcher.CONTINUE_CHAIN;
        }

        public static Exception Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                ModMain.Logger.Error("Exception in Audio Manager ( count = {0} ) :", counter++);
                ModMain.Logger.Error("{0}", __exception);
                if ( counter >= LIMIT )
                {
                    ModMain.Logger.Warn("Audio Manager exception limit triggered; skipping further Audio Manager updates");
                }
            }
            return null; // Return null if no exception and no new one to throw
        }
    }
}

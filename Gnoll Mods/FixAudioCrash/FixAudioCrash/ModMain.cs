using System;
using Game;
using GnollModLoader;

namespace GnollMods.FixAudioCrash
{
    public class ModMain : IGnollMod, IHasDirectPatch
    {
        public static ModsLogger Logger { get; set; }

        public string Name => "FixAudioCrash";

        public string Description => "There is an issue of audio manager crashing on a game world loading. Trying to prevent this.";

        public string BuiltWithLoaderVersion => "G1.15.0";

        public int RequireMinPatchVersion => 15;

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
            var orig = typeof(GnomanEmpire).GetMethod(nameof(AudioManager.Update));
            var finalizer = typeof(Patch_AudioManager).GetMethod(nameof(Patch_AudioManager.Finalizer));
            patcher.ApplyDirectPatch(orig, finalizer: finalizer);
        }
    }

    internal class Patch_AudioManager
    {
        public static Exception Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                ModMain.Logger.Error("Exception in Audio Manager:");
                ModMain.Logger.Error("{0}", __exception);
            }
            return null; // Return null if no exception and no new one to throw
        }
    }
}

using System;
using Game;
using HarmonyLib;

namespace GnollModLoader
{
    /**
     * This patcher is global to the whole game.
     * It will be executed as soon as possible.
     */
    public class GlobalPatcher
    {
        private readonly Harmony harmony = new Harmony("com.github.nefaro.gnoll");
        private readonly HookManager hookManager;

        public GlobalPatcher(HookManager hookManager)
        {
            this.hookManager = hookManager;
        }

        public void PerformPatching()
        {
            Logger.Log("-- Applying patches ...");
            this.ApplyPatch_GnomanEmpire_PlayGame();
            this.ApplyPatch_GameSettings_ApplyDisplayChanges();
            Logger.Log("-- Applying patches ... DONE");
        }

        private void ApplyPatch_GnomanEmpire_PlayGame()
        {
            var playGame = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.PlayGame));
            var prefixPatch = typeof(Patch_GnomanEmpire_PlayGame).GetMethod(nameof(Patch_GnomanEmpire_PlayGame.Prefix));
            this.ApplyPatchImpl(playGame, prefixPatch:prefixPatch);
        }

        private void ApplyPatch_GameSettings_ApplyDisplayChanges()
        {
            var playGame = typeof(GameSettings).GetMethod(nameof(GameSettings.ApplyDisplayChanges));
            var prefixPatch = typeof(Patch_GameSettings_ApplyDisplayChanges).GetMethod(nameof(Patch_GnomanEmpire_PlayGame.Prefix));
            this.ApplyPatchImpl(playGame, prefixPatch: prefixPatch);
        }

        private void ApplyPatchImpl(System.Reflection.MethodInfo original, System.Reflection.MethodInfo prefixPatch = null, System.Reflection.MethodInfo postfixPatch = null)
        {
            if ( prefixPatch == null && postfixPatch == null )
            {
                Logger.Error($"-- Failed to apply patch: Prefix and Postfix patch are 'null'");
            }
            var patchName = (prefixPatch ?? postfixPatch).DeclaringType.Name;
            try
            {
                if (original == null)
                {
                    Logger.Error($"-- Failed to apply patch '{patchName}': Cannot find original method");
                }
                harmony.Patch(original, (prefixPatch != null ? new HarmonyMethod(prefixPatch) : null), (postfixPatch != null ? new HarmonyMethod(postfixPatch) : null));
            }
            catch (Exception e)
            {
                Logger.Error($"-- Failed to apply patch '{patchName}': {e}");
            }
        }
    }

    internal class Patch_GnomanEmpire_PlayGame
    {
        public static void Prefix()
        {
            // Add a hook before ingame HUD is built
            HookManager.HookInGameHUDInit_before();
        }
    }

    internal class Patch_GameSettings_ApplyDisplayChanges
    {
        public static void Prefix()
        {
            GnollMain.HookPostInit();
        }
    }

}

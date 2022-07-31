using System;
using System.Collections.Generic;
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

            List<Action> patches = new List<Action> { 
                ApplyPatch_GnomanEmpire_PlayGame,
                ApplyPatch_GnomanEmpire_LoadGame,
                ApplyPatch_GnomanEmpire_SaveGame,
                ApplyPatch_GameSettings_ApplyDisplayChanges
             };

            foreach (Action d in patches)
            {
                d.Invoke();
            }            
            Logger.Log("-- Applying patches ... DONE");
        }

        private void ApplyPatch_GnomanEmpire_PlayGame()
        {
            var playGame = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.PlayGame));
            var prefixPatch = typeof(Patch_GnomanEmpire_PlayGame).GetMethod(nameof(Patch_GnomanEmpire_PlayGame.Prefix));
            this.ApplyPatchImpl(playGame, prefixPatch: prefixPatch);
        }

        private void ApplyPatch_GnomanEmpire_LoadGame()
        {
            var loadGame = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.LoadGame));
            var postfixPatch = typeof(Patch_GnomanEmpire_LoadGame).GetMethod(nameof(Patch_GnomanEmpire_LoadGame.Postfix));
            this.ApplyPatchImpl(loadGame, postfixPatch: postfixPatch);
        }

        private void ApplyPatch_GnomanEmpire_SaveGame()
        {
            // SaveGame is a threaded method, "method_5" is the real, single threaded, save game method
            var saveGame = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.method_5));
            var prefixPatch = typeof(Patch_GnomanEmpire_SaveGame).GetMethod(nameof(Patch_GnomanEmpire_SaveGame.Prefix));
            var postfixPatch = typeof(Patch_GnomanEmpire_SaveGame).GetMethod(nameof(Patch_GnomanEmpire_SaveGame.Postfix));
            this.ApplyPatchImpl(saveGame, prefixPatch: prefixPatch, postfixPatch: postfixPatch);
        }

        private void ApplyPatch_GameSettings_ApplyDisplayChanges()
        {
            var orig = typeof(GameSettings).GetMethod(nameof(GameSettings.ApplyDisplayChanges));
            var prefixPatch = typeof(Patch_GameSettings_ApplyDisplayChanges).GetMethod(nameof(Patch_GameSettings_ApplyDisplayChanges.Prefix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch);
        }

        /* 
        // Experimental
        private void ApplyPatch_Faction_PlayerSpawnStrength()
        {
            var orig = typeof(Faction).GetMethod(nameof(Faction.PlayerSpawnStrength));
            var prefixPatch = typeof(Patch_Faction_PlayerSpawnStrength).GetMethod(nameof(Patch_Faction_PlayerSpawnStrength.Prefix));
            var postfixPatch = typeof(Patch_Faction_PlayerSpawnStrength).GetMethod(nameof(Patch_Faction_PlayerSpawnStrength.Postfix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch, postfixPatch: postfixPatch);
        }
        */

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
                Logger.Log($"-- Patched {original.FullDescription()}");
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

    internal class Patch_GnomanEmpire_LoadGame
    {
        public static void Postfix()
        {
            HookManager.HookLoadGame_after();
        }
    }

    internal class Patch_GnomanEmpire_SaveGame
    {
        public static void Prefix()
        {
            HookManager.HookSaveGame_before();
        }

        public static void Postfix()
        {
            HookManager.HookSaveGame_after();
        }
    }

    // Experimental population amount override
    // Leaving in for future work ...
    /*
    internal class Patch_Faction_PlayerSpawnStrength
    {
        public static void Prefix()
        {
            
        }

        public static void Postfix(ref float __result, bool modified, bool applyVariance)
        {
            if (modified)
            {
                Logger.Log($"-- Original strength {__result}");
                __result = __result + 57;
                Logger.Log($"-- Calculated strength {__result}");
            }
        }
    }*/
}

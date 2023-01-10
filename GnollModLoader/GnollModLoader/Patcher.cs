using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Game;
using Game.Common;
using Game.GUI;
using Game.GUI.Controls;
using GameLibrary;
using HarmonyLib;
using LibNoise.Xna.Operator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SevenZip;
using static HarmonyLib.Code;

namespace GnollModLoader
{
    /**
     * This patcher runs Harmony patches.
     * It will be executed as soon as possible.
     */
    public class Patcher
    {
        public static readonly Boolean SKIP_CHAIN = false;
        public static readonly Boolean CONTINUE_CHAIN = true;

        private readonly Harmony harmony = new Harmony("com.github.nefaro.gnoll");
        private readonly HookManager hookManager;

        public Patcher(HookManager hookManager)
        {
            this.hookManager = hookManager;
        }

        public void PerformPatching()
        {
            Logger.Log("Applying patches ...");

            List<Action> patches = new List<Action> {
                // Experimental
                //ApplyPatch_Faction_PlayerSpawnStrength,

                // Debug

                // Real patches
                ApplyPatch_GnomanEmpire_PlayGame,
                ApplyPatch_GnomanEmpire_LoadGame,
                ApplyPatch_GnomanEmpire_SaveGame,
                ApplyPatch_GameSettings_ApplyDisplayChanges,
                ApplyPatch_MainMenuWindow
            };

            foreach (Action d in patches)
            {
                d.Invoke();
            }
            Logger.Log("Applying patches ... DONE");
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
            // SaveGame is a threaded method, "method_5" is the real, single threaded save game method
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

        private void ApplyPatch_MainMenuWindow()
        {
            var orig = typeof(MainMenuWindow).GetConstructor(new Type[] { typeof(Manager) });
            var postfixPatch = typeof(Patch_MainMenuWindow).GetMethod(nameof(Patch_MainMenuWindow.Ctor_Postfix));
            this.ApplyPatchImpl(orig, postfixPatch: postfixPatch);
        }

        /*
        private void ApplyPatch_Button()
        {
            var orig = typeof(Button).GetConstructor(new Type[] {typeof(Manager)});
            var postfixPatch = typeof(Patch_Button).GetMethod(nameof(Patch_Button.Postfix_ctor));
            this.ApplyPatchImpl(orig, postfixPatch: postfixPatch);
        }*/

        // Experimental
        /*
        private void ApplyPatch_Faction_PlayerSpawnStrength()
        {
            var orig = typeof(Faction).GetMethod(nameof(Faction.PlayerSpawnStrength));
            var prefixPatch = typeof(Patch_Faction_PlayerSpawnStrength).GetMethod(nameof(Patch_Faction_PlayerSpawnStrength.Prefix));
            var postfixPatch = typeof(Patch_Faction_PlayerSpawnStrength).GetMethod(nameof(Patch_Faction_PlayerSpawnStrength.Postfix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch, postfixPatch: postfixPatch);
        }
        */


        // Debugging

        public void ApplyDirectPatch(System.Reflection.MethodBase original,
            System.Reflection.MethodInfo prefixPatch = null,
            System.Reflection.MethodInfo postfixPatch = null,
            System.Reflection.MethodInfo finalizer = null)
        {
            // Currently exactly the same signature
            // Need to think if it needs a bit more validation
            this.ApplyPatchImpl(original, prefixPatch, postfixPatch, finalizer);
        }

        private void ApplyPatchImpl(System.Reflection.MethodBase original, 
            System.Reflection.MethodInfo prefixPatch = null, 
            System.Reflection.MethodInfo postfixPatch = null,
            System.Reflection.MethodInfo finalizer = null)
        {
            if (prefixPatch == null && postfixPatch == null && finalizer == null)
            {
                Logger.Error($"!! Failed to apply patch: Prefix and Postfix patch are 'null'");
            }
            var patchName = (prefixPatch ?? postfixPatch ?? finalizer).DeclaringType.Name;
            try
            {
                if (original == null)
                {
                    Logger.Error($"!! Failed to apply patch '{patchName}': Cannot find original method");
                }
                harmony.Patch(original, 
                    (prefixPatch != null ? new HarmonyMethod(prefixPatch) : null), 
                    (postfixPatch != null ? new HarmonyMethod(postfixPatch) : null),
                    null,
                    (finalizer != null ?  new HarmonyMethod(finalizer) : null)
                    );

                Logger.Log($"-- Patched {original.FullDescription()}");
            }
            catch (Exception e)
            {
                Logger.Error($"!! Failed to apply patch '{patchName}': {e}");
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

    internal class Patch_MainMenuWindow
    {
        public static void Ctor_Postfix(ref MainMenuWindow __instance)
        {
            GnollMain.HookGnollMainMenu_after(__instance);
        }
    }

    // Experimental population amount override
    // Leaving in for future work ...
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
                __result = 81;
                Logger.Log($"-- Calculated strength {__result}");
            }
        }
    }
}

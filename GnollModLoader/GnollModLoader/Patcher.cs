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
                //ApplyPatch_Camera_method_0,
                //ApplyPatch_Camera_method_1,
                //ApplyPatch_GnomanEmpire_FinishLoading,
                //ApplyPatch_Region_LoadRegion,
                //ApplyPatch_World_LoadWorld,
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
            var prefixPatch = typeof(Patch_GnomanEmpire_LoadGame).GetMethod(nameof(Patch_GnomanEmpire_LoadGame.Prefix));
            var postfixPatch = typeof(Patch_GnomanEmpire_LoadGame).GetMethod(nameof(Patch_GnomanEmpire_LoadGame.Postfix));
            this.ApplyPatchImpl(loadGame, prefixPatch: prefixPatch, postfixPatch: postfixPatch);
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

        private void ApplyPatch_Camera_method_0()
        {
            var orig = typeof(Camera).GetMethod(nameof(Camera.method_0));
            var prefixPatch = typeof(Patch_Camera_method_0).GetMethod(nameof(Patch_Camera_method_0.Prefix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch);
        }

        private void ApplyPatch_Camera_method_1()
        {
            var orig = typeof(Camera).GetMethod(nameof(Camera.method_1));
            var prefixPatch = typeof(Patch_Camera_method_1).GetMethod(nameof(Patch_Camera_method_1.Prefix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch);
        }

        private void ApplyPatch_World_LoadWorld()
        {
            var orig = typeof(World).GetMethod(nameof(World.LoadWorld));
            var prefixPatch = typeof(Patch_World_LoadWorld).GetMethod(nameof(Patch_World_LoadWorld.Prefix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch);
        }

        private void ApplyPatch_GnomanEmpire_FinishLoading()
        {
            var orig = typeof(GnomanEmpire).GetMethod(nameof(GnomanEmpire.FinishLoadingGame));
            var prefixPatch = typeof(Patch_GnomanEmpire_FinishLoading).GetMethod(nameof(Patch_GnomanEmpire_FinishLoading.Prefix));
            this.ApplyPatchImpl(orig, prefixPatch: prefixPatch);
        }

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

        public static bool Prefix(ref GnomanEmpire __instance, string fileName, bool fallenKingdom = false)
        {
            Logger.Log($"Load Game");
            __instance.string_0 = fileName;
            FileStream fileStream = null;
            string str = fallenKingdom ? GnomanEmpire.SaveFolderPath("OldWorlds\\") : GnomanEmpire.SaveFolderPath("Worlds\\");
            try
            {
                fileStream = new FileStream(str + fileName, FileMode.Open, FileAccess.Read);
                using (FileStream fileStream2 = new FileStream(str + "temp", FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryReader reader = new BinaryReader(fileStream);
                    GameSaveHeader gameSaveHeader = new GameSaveHeader(reader);
                    __instance.uint_0 = gameSaveHeader.SaveFileVersion;
                    foreach(ModFolder folder in gameSaveHeader.ModFolders)
                    {
                        //folder.Folder = folder.Folder.Replace("F:\\SteamGames\\", "H:\\Steam\\");
                        Logger.Log($"{folder.Folder}"); 
                    }
                    __instance.gameDefs_0.SetModFolders(gameSaveHeader.ModFolders);
                    SevenZipExtractor sevenZipExtractor = new SevenZipExtractor(fileStream);
                    sevenZipExtractor.ExtractFile(0, fileStream2);
                    fileStream2.Position = 0L;
                    BinaryReader reader2 = new BinaryReader(fileStream2);
                    if (__instance.task_0 != null && !__instance.task_0.IsCompleted)
                    {
                        __instance.task_0.Wait();
                    }
                    __instance.method_4();
                    __instance.gameDefs_0.ReadDefs(false);
                    __instance.camera_0 = new Camera(reader2);
                    __instance.gameEntityManager_0 = new GameEntityManager(reader2);
                    __instance.world_0.LoadWorld(reader2);
                    __instance.gameEntityManager_0.OnSerializationComplete();
                    __instance.world_0.OnSerializationComplete();
                    __instance.method_7();
                    __instance.uint_0 = 0U;
                }
            }
            catch(Exception ex) 
            {
                Logger.Error($"{ex}");
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            File.Delete(str + "temp");
            Logger.Log($"GAM Instance {GnomanEmpire.Instance.GetHashCode()}");
            Logger.Log($"GAM Region {GnomanEmpire.Instance.Region.GetHashCode()}");
            Logger.Log($"Region TileSelectionManager {GnomanEmpire.Instance.Region.TileSelectionManager}");
            return Patcher.SKIP_CHAIN;
        }

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
    internal class Patch_World_LoadWorld
    {
        public static bool Prefix(ref World __instance, BinaryReader reader)
        {
            Logger.Log($"LoadWorld patch");
            try { 
                __instance.aidirector_0 = new AIDirector(reader);
                __instance.region_0 = new Region();
                __instance.region_0.LoadRegion(reader, new TileSet(GnomanEmpire.Instance.GameDefs.Tilesheet, new Vector2(32f, 16f)));

                Logger.Log($"Region {__instance.region_0}");
                Logger.Log($"LW Instance {GnomanEmpire.Instance.GetHashCode()}");
                Logger.Log($"LW Region {GnomanEmpire.Instance.Region.GetHashCode()}");
                Logger.Log($"Region TileSelectionManager {GnomanEmpire.Instance.Region.TileSelectionManager}");

                __instance.notificationManager_0 = new NotificationManager(reader);
            if (GnomanEmpire.Instance.LoadingSaveVersion < 21U)
            {
                GameMode gameMode;
                if (GnomanEmpire.Instance.LoadingSaveVersion >= 2U)
                {
                    gameMode = (GameMode)reader.ReadInt32();
                }
                else
                {
                    gameMode = GameMode.Normal;
                }
                __instance.difficultySetting_0 = new DifficultySetting(gameMode);
                return Patcher.SKIP_CHAIN;
            }
            __instance.difficultySetting_0 = new DifficultySetting(reader);

            return Patcher.SKIP_CHAIN;
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }
            return Patcher.SKIP_CHAIN;
        }
    }

    internal class Patch_Camera_method_1
    {
        public static bool Prefix(ref Camera __instance, float float_2)
        {
            Logger.Log($"Camera patch methdo1");

            try
            {
                Map map = GnomanEmpire.Instance.Map;
                float num = __instance.method_9() - __instance.float_0;
                if (Math.Abs(num) < 0.05f)
                {
                    __instance.float_0 = __instance.method_9();
                }
                else
                {
                    __instance.method_9();
                    if (num > 0f)
                    {
                        __instance.float_0 = MathHelper.Clamp(__instance.float_0 + num * float_2 * 10f, __instance.float_1[__instance.int_0 - 1], __instance.method_9());
                    }
                    else
                    {
                        __instance.float_0 = MathHelper.Clamp(__instance.float_0 + num * float_2 * 10f, __instance.method_9(), __instance.float_1[__instance.int_0 + 1]);
                    }
                }
                Vector2 value = __instance.method_8();
                if (__instance.gameEntity_0 != null)
                {
                    __instance.MoveTo(__instance.gameEntity_0.Position, false, true);
                }
                else
                {
                    Vector2 mTileSize = map.TileSet.mTileSize;
                    float num2 = (float)map.mMapWidth * mTileSize.X * 0.5f;
                    __instance.vector2_0.X = MathHelper.Clamp(__instance.vector2_0.X, -num2, num2);
                    __instance.vector2_0.Y = MathHelper.Clamp(__instance.vector2_0.Y, 0f, (float)map.mMapHeight * mTileSize.Y);
                    __instance.gameProperty_0.Value = (int)MathHelper.Clamp((float)__instance.gameProperty_0, 0f, (float)(map.mMapDepth - 1));
                }
                if (__instance.float_0 == 1f)
                {
                    __instance.vector2_0.X = (float)((int)__instance.Position.X);
                    __instance.vector2_0.Y = (float)((int)__instance.Position.Y);
                }
                __instance.matrix_0 = Matrix.CreateTranslation(new Vector3(-__instance.Position + value, 0f)) * Matrix.CreateScale(new Vector3(__instance.float_0, __instance.float_0, 1f));
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex}");
            }
            return Patcher.SKIP_CHAIN;
        }
    }

    internal class Patch_Camera_method_0
    {
        public static void Prefix(ref Camera __instance, float float_2)
        {
            Logger.Log($"Running Camera.method_o patch");
            Logger.Log($"CAM Instance {GnomanEmpire.Instance.GetHashCode()}");
            Logger.Log($"CAM Region {GnomanEmpire.Instance.Region.GetHashCode()}");
            Logger.Log($"Region TileSelectionManager {GnomanEmpire.Instance.Region.TileSelectionManager}");
            if (GnomanEmpire.Instance.GuiManager.MouseOver)
            {
                return;
            }
            InputManager inputManager = GnomanEmpire.Instance.InputManager;
            GameSettings settings = GnomanEmpire.Instance.Settings;
            Vector2 value = __instance.vector2_0;
            if (inputManager.IsActionFired(KeyboardActions.PanCameraRight))
            {
                __instance.vector2_0.X = __instance.vector2_0.X + float_2 * 500f / __instance.float_0;
                __instance.gameEntity_0 = null;
            }
            else if (inputManager.IsActionFired(KeyboardActions.PanCameraLeft))
            {
                __instance.vector2_0.X = __instance.vector2_0.X - float_2 * 500f / __instance.float_0;
                __instance.gameEntity_0 = null;
            }
            if (inputManager.IsActionFired(KeyboardActions.PanCameraUp))
            {
                __instance.vector2_0.Y = __instance.vector2_0.Y - float_2 * 500f / __instance.float_0;
                __instance.gameEntity_0 = null;
            }
            else if (inputManager.IsActionFired(KeyboardActions.PanCameraDown))
            {
                __instance.vector2_0.Y = __instance.vector2_0.Y + float_2 * 500f / __instance.float_0;
                __instance.gameEntity_0 = null;
            }
            Map map = GnomanEmpire.Instance.Map;
            if (inputManager.IsPressed(MouseButtons.Left))
            {
                Vector2 vector = __instance.ScreenCoordsToIsoMapIndex(inputManager.MousePosition);
                Logger.Log("MousePos: ({0},{1}) Tile Index: ({2},{3})", new object[]
                {
                    inputManager.MouseX,
                    inputManager.MouseY,
                    vector.X,
                    vector.Y
                });
            }
            if (GnomanEmpire.Instance.Region.TileSelectionManager.AutoScrollWindow())
            {
                Vector2 mousePosition = inputManager.MousePosition;
                Rectangle rectangle = new Rectangle(0, 0, settings.ScreenWidth, settings.ScreenHeight);
                float num = (float)rectangle.Width * 0.1f;
                float num2 = (float)rectangle.Height * 0.1f;
                if (mousePosition.X > (float)rectangle.Right - num)
                {
                    __instance.vector2_0.X = __instance.vector2_0.X + float_2 * 500f / __instance.float_0;
                    __instance.gameEntity_0 = null;
                }
                else if (mousePosition.X < (float)rectangle.Left + num)
                {
                    __instance.vector2_0.X = __instance.vector2_0.X - float_2 * 500f / __instance.float_0;
                    __instance.gameEntity_0 = null;
                }
                if (mousePosition.Y < (float)rectangle.Top + num2)
                {
                    __instance.vector2_0.Y = __instance.vector2_0.Y - float_2 * 500f / __instance.float_0;
                    __instance.gameEntity_0 = null;
                }
                else if (mousePosition.Y > (float)rectangle.Bottom - num2)
                {
                    __instance.vector2_0.Y = __instance.vector2_0.Y + float_2 * 500f / __instance.float_0;
                    __instance.gameEntity_0 = null;
                }
            }
            else if (!GnomanEmpire.Instance.Region.TileSelectionManager.HasFocus && (inputManager.IsDown(MouseButtons.Left) || inputManager.IsDown(MouseButtons.Right)))
            {
                Vector2 mouseDelta = inputManager.MouseDelta;
                __instance.vector2_0.X = __instance.vector2_0.X - mouseDelta.X / __instance.float_0;
                __instance.vector2_0.Y = __instance.vector2_0.Y - mouseDelta.Y / __instance.float_0;
                __instance.gameEntity_0 = null;
            }
            GuiManager guiManager = GnomanEmpire.Instance.GuiManager;
            if (value != __instance.vector2_0 && guiManager.InGameHUD_0 != null)
            {
                guiManager.InGameHUD_0.CloseWindow();
            }
            if (inputManager.IsActionFired(KeyboardActions.ZoomIn))
            {
                __instance.int_0 = (int)MathHelper.Clamp((float)(__instance.int_0 + 1), 0f, (float)(__instance.float_1.Length - 1));
            }
            else if (inputManager.IsActionFired(KeyboardActions.ZoomOut))
            {
                __instance.int_0 = (int)MathHelper.Clamp((float)(__instance.int_0 - 1), 0f, (float)(__instance.float_1.Length - 1));
            }
            else if (inputManager.IsActionFired(KeyboardActions.IncreaseDepth))
            {
                __instance.gameProperty_0.Value++;
                __instance.gameEntity_0 = null;
            }
            else if (inputManager.IsActionFired(KeyboardActions.DecreaseDepth))
            {
                __instance.gameProperty_0.Value--;
                __instance.gameEntity_0 = null;
            }
            if (inputManager.IsDown(Keys.LeftControl))
            {
                if (inputManager.MouseWheelDelta > 0f)
                {
                    __instance.int_0 = (int)MathHelper.Clamp((float)(__instance.int_0 + 1), 0f, (float)(__instance.float_1.Length - 1));
                }
                else if (inputManager.MouseWheelDelta < 0f)
                {
                    __instance.int_0 = (int)MathHelper.Clamp((float)(__instance.int_0 - 1), 0f, (float)(__instance.float_1.Length - 1));
                }
            }
            else if (inputManager.MouseWheelDelta < 0f)
            {
                __instance.gameProperty_0.Value++;
                __instance.gameEntity_0 = null;
            }
            else if (inputManager.MouseWheelDelta > 0f)
            {
                __instance.gameProperty_0.Value--;
                __instance.gameEntity_0 = null;
            }
            if (inputManager.IsActionFired(KeyboardActions.RotateCameraRight))
            {
                __instance.method_7(true);
                return;
            }
            if (inputManager.IsActionFired(KeyboardActions.RotateCameraLeft))
            {
                __instance.method_7(false);
            }



        }
    }

    internal class Patch_GnomanEmpire_FinishLoading
    {
        public static void Prefix()
        {
            Logger.Log($"FinihsLoading patch");
            Logger.Log($"FIN Instance {GnomanEmpire.Instance.GetHashCode()}");
            Logger.Log($"FIN Region {GnomanEmpire.Instance.Region.GetHashCode()}");
            Logger.Log($"Region TileSelectionManager {GnomanEmpire.Instance.Region.TileSelectionManager}");
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

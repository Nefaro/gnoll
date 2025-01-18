﻿using Game;

namespace GnollModLoader
{
    public class HookManager
    {
        public delegate Game.GUI.Controls.Button AddButton(string label);

        // Called on when export menu has initialized
        public delegate void ExportMenuListInitHandler(Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager, AddButton context);
        // Called when the ingame HUD (this is the gamefield, NOT the main menu) is complete 
        public delegate void InGameHUDInitHandler(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager);
        // Called before ingame HUD is constructed (this means world is constructed, UI is missing)
        public delegate void BeforeInGameHUDInitHandler();
        // Called after the ingame HUD opens a window
        public delegate void InGameHUDShowWindowHandler(Game.GUI.Controls.Window window);
        // Called on every update, after the game's update has run
        public delegate void UpdateInGameHandler(float realTimeDelta, float gameTimeDelta);
        // Called when a character has finished a job (after the ingame processing has finished)
        public delegate void OnJobCompleteHandler(Game.Job job, Game.Character character);
        // Called after a new entity has been spawned
        public delegate void OnEntitySpawnHandler(Game.GameEntity entity);
        // Called before a new entity has been spawned
        public delegate void BeforeEntitySpawnHandler(Game.GameEntity entity);
        // Called after main menu init, before "Exit" button is attached
        public delegate void MainMenuGuiInitHandler(Game.GUI.MainMenuWindow window, Game.GUI.Controls.Manager manager);
        // Called after new game has configured but before any generation has happened
        public delegate void BeforeStartNewGameHandler(CreateWorldOptions worldOptions);
        // Called after new game has configured and after mod defs have been read in, but before any generation
        public delegate void BeforeStartNewGameAfterReadDefsHandler(CreateWorldOptions worldOptions);
        // Called after new game has configured, world created and starting faction placed
        public delegate void BeforeStartNewGameAfterEmbarkHandler();
        // Called after a savegame has been loaded
        public delegate void AfterGameLoadedHandler();
        // Called before a game is being saved
        public delegate void BeforeGameSavedHandler();
        // Called after a game is being saved
        public delegate void AfterGameSavedHandler();

        public HookManager( )
        {
            instance = this;
        }

        public static int HookImportExportListInit(int Y, Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager)
        {
            AddButton addButton = (string label) =>
            {
                var button = new Game.GUI.Controls.Button(importExportMenu.Manager);
                button.Init();
                Y += button.Margins.Top;
                button.Width = 200;
                button.Top = Y;
                button.Text = label;
                importExportMenu.panel_0.Add(button);
                Y += button.Height + button.Margins.Bottom;
                return button;
            };

            if (instance.ExportMenuListInit != null)
            {
                Logger.Log("-- Hook Import/Export list");
                instance.ExportMenuListInit(importExportMenu, manager, addButton);
            }

            return Y;
        }

        public static void HookInGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            Logger.Log($"++ Using savegame '{GnomanEmpire.Instance.CurrentWorld}'");
            if (instance.InGameHUDInit != null)
            {
                Logger.Log("-- Hook In Game HUD Init");
                instance.InGameHUDInit(inGameHUD, manager);
            }
        }

        public static void HookUpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
            if (instance.UpdateInGame != null)
            {
                float timeElapsedInGame = Game.GnomanEmpire.Instance.world_0.Paused ? 0.0f : gameTimeDelta;
                instance.UpdateInGame(realTimeDelta, timeElapsedInGame);
            }
        }
        
        public static void HookMainMenuGuiInit(Game.GUI.MainMenuWindow window, Game.GUI.Controls.Manager manager)
        {
            Logger.Log("-- Hook Main Menu Init");
            if (instance.MainMenuGuiInit != null)
            {
                instance.MainMenuGuiInit(window, manager);
            }
        }

        public static void HookIngameHudShowWindow_after(Game.GUI.Controls.Window window)
        {
            if (instance.InGameShowWindow != null)
            {
                instance.InGameShowWindow(window);
            }
        }

        public static void HookOnEntitySpawn_after(Game.GameEntity entity)
        {
            if ( instance.OnEntitySpawn != null)
            {
                instance.OnEntitySpawn(entity);
            }
        }

        public static void HookOnEntitySpawn_before(Game.GameEntity entity)
        {
            if (instance.BeforeEntitySpawn != null)
            {
                instance.BeforeEntitySpawn(entity);
            }
        }

        public static void HookOnJobComplete_after(Game.Job job, Game.Character character)
        {
            if (instance.OnJobComplete != null)
            {
                instance.OnJobComplete(job, character);
            }
        }

        public static void HookOnStartNewGame_before(CreateWorldOptions worldOptions)
        {
            if (instance.BeforeStartNewGame != null)
            {
                instance.BeforeStartNewGame(worldOptions);
            }
        }

        public static void HookOnStartNewGame_afterReadDefs(CreateWorldOptions worldOptions)
        {
            if (instance.BeforeStartNewGameAfterReadDefs != null)
            {
                instance.BeforeStartNewGameAfterReadDefs(worldOptions);
            }
        }

        public static void HookOnStartNewGame_afterEmbark() 
        { 
            if ( instance.BeforeStartNewGameAfterEmbark != null )
            {
                instance.BeforeStartNewGameAfterEmbark();
            }
        }

        public static void HookInGameHUDInit_before()
        {
            if (instance.BeforeInGameHudInit != null)
            {
                instance.BeforeInGameHudInit();
            }
        }

        public static void HookLoadGame_after()
        {
            if (instance.AfterGameLoaded != null)
            {
                instance.AfterGameLoaded();
            }
        }
        internal static void HookSaveGame_before()
        {
            if (instance.BeforeGameSaved != null)
            {
                instance.BeforeGameSaved();
            }
        }
        internal static void HookSaveGame_after()
        {
            if (instance.AfterGameSaved != null)
            {
                instance.AfterGameSaved();
            }
        }

        public event ExportMenuListInitHandler ExportMenuListInit;
        public event InGameHUDInitHandler InGameHUDInit;
        public event BeforeInGameHUDInitHandler BeforeInGameHudInit;
        public event InGameHUDShowWindowHandler InGameShowWindow;
        public event UpdateInGameHandler UpdateInGame;
        public event OnJobCompleteHandler OnJobComplete;
        public event OnEntitySpawnHandler OnEntitySpawn;
        public event BeforeEntitySpawnHandler BeforeEntitySpawn;
        public event MainMenuGuiInitHandler MainMenuGuiInit;
        public event BeforeStartNewGameHandler BeforeStartNewGame;
        public event BeforeStartNewGameAfterReadDefsHandler BeforeStartNewGameAfterReadDefs;
        public event BeforeStartNewGameAfterEmbarkHandler BeforeStartNewGameAfterEmbark;
        public event AfterGameLoadedHandler AfterGameLoaded;
        public event BeforeGameSavedHandler BeforeGameSaved;
        public event AfterGameSavedHandler AfterGameSaved;

        private static HookManager instance;
    }
}

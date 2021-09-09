using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GnollModLoader.GUI;

namespace GnollModLoader
{
    public class HookManager
    {
        public delegate Game.GUI.Controls.Button AddButton(string label);

        // Called on when export menu has initialized
        public delegate void ExportMenuListInitHandler(Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager, AddButton context);
        // Called when the ingame HUD (this is the gamefield, NOT the main menu) is complete 
        public delegate void InGameHUDInitHandler(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager);
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

        private List<IGnollMod> _listOfMods;

        public HookManager()
        {
            instance = this;
        }

        public void RegisterMods(List<IGnollMod> listOfMods)
        {
            this._listOfMods = listOfMods;
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
                Console.WriteLine("-- Hook Import/Export list");
                instance.ExportMenuListInit(importExportMenu, manager, addButton);
            }

            return Y;
        }

        public static void HookInGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            if (instance.InGameHUDInit != null)
            {
                Console.WriteLine("-- Hook In Game HUD Init");
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
            Console.WriteLine("-- Hook Main Menu Init");
            Game.GUI.Controls.Button modButton = window.method_39(manager, GnollMain.NAME);
            modButton.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PushWindow(new ModLoaderMenu(Game.GnomanEmpire.Instance.GuiManager.Manager, instance._listOfMods));
            };
            window.panel_0.Add(modButton);
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

        public event ExportMenuListInitHandler ExportMenuListInit;
        public event InGameHUDInitHandler InGameHUDInit;
        public event InGameHUDShowWindowHandler InGameShowWindow;
        public event UpdateInGameHandler UpdateInGame;
        public event OnJobCompleteHandler OnJobComplete;
        public event OnEntitySpawnHandler OnEntitySpawn;
        public event BeforeEntitySpawnHandler BeforeEntitySpawn;

        private static HookManager instance;
    }
}

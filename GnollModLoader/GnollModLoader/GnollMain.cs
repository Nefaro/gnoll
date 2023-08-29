using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader.GUI;

namespace GnollModLoader
{
    public class GnollMain
    {
        private const string MAJOR_VERSION = "1";
        private const string BUGFIX_VERSION = ".0";

        // for easier validation
        public const uint PATCH_VERSION = 14;

        public const string NAME = "Gnoll Mod Loader";
        public const string SHORT_NAME = "Gnoll";
        public const string APP_URL = "https://github.com/Nefaro/gnoll";
        public const string ORIGINAL_URL = "https://github.com/minexew/gnomodkit";

        public const string MODS_DIR = "Gnoll Mods\\enabled";

        private static bool debug = false;
        public static bool Debug => debug;
        public static string VERSION
        {
            get { return MAJOR_VERSION + "." + PATCH_VERSION + BUGFIX_VERSION; }
        }

        static HookManager hookManager;
        static ModLoader modLoader;
        static ModManager modManager;
        static LuaManager luaManager;
        static SaveGameManager saveGameManager;

        private GnollMain()
        {
        }

        public static void HookInit()
        {
            // System Console for debugging
            // should probably put behind conf or button press ...
            ConsoleWindow.ShowConsoleWindow();
            if (!Debug)
                ConsoleWindow.HideConsoleWindow();

            Logger.Log("== {0} {1} == ", NAME, VERSION);

            hookManager = new HookManager();
            // Apply patches before mods are loaded
            var patcher = new Patcher(hookManager);
            patcher.PerformPatching();

            // Attach debug hooks before mods are loaded
            tryAttachDebugHooks(hookManager);

            saveGameManager = new SaveGameManager();

            // hook save game load event
            // NOTE: SaveGameManager needs to be first one on the "load" path, 
            // since other mods depend on the data being already available
            // Needs to be before Lua manager, since that also has its own hooks
            hookManager.AfterGameLoaded += saveGameManager.HookAfterGameLoaded;

            luaManager = new LuaManager(hookManager, saveGameManager);

            // Mod manager runs AFTER patches have been applied
            modManager = new ModManager(hookManager, patcher, luaManager);
            modLoader = new ModLoader(modManager);
            modLoader.LoadModsFrom(MODS_DIR);
            luaManager.RunInitScripts();

            // hook up Gnoll main menu
            hookManager.MainMenuGuiInit += HookGnollMainMenu;
            // hook game saving event
            // NOTE: SaveGameManager needs to be last one on the "save" path
            hookManager.AfterGameSaved += saveGameManager.HookAfterGameSaved;
            
        }

        // Called after the game has initialized and game settings have been read in,
        // but before the settings have been applied to the game
        public static void HookPostInit()
        {
            if (Game.GnomanEmpire.Instance.Settings.DebugMode)
            {
                Logger.Log("-- Enabling debug mode; override from Game");
                debug = true;
                ConsoleWindow.ShowConsoleWindow();
            }
        }

        public static void HookGnollMainMenu(MainMenuWindow window, Manager manager)
        {
            Game.GUI.Controls.Button modButton = window.method_39(manager, GnollMain.NAME);
            modButton.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PushWindow(new ModLoaderMenu(Game.GnomanEmpire.Instance.GuiManager.Manager, modManager));
            };
            window.panel_0.Add(modButton);
        }

        public static void HookGnollMainMenu_after(MainMenuWindow window)
        {
            window.label_0.Text = window.label_0.Text + getMainPageLabel();
        }

        private static string getMainPageLabel()
        {
            return $" {GnollMain.SHORT_NAME} {GnollMain.VERSION}";
        }

        private static void tryAttachDebugHooks(HookManager hookManager)
        {
            if (GnollMain.Debug)
            {
                Logger.Log("Attaching DEBUG hooks");
                // Attach debug events before anything else
                hookManager.InGameHUDInit += DEBUG_HookManager_InGameHUDInit;
                hookManager.UpdateInGame += DEBUG_HookManager_UpdateInGame;
                hookManager.OnJobComplete += DEBUG_HookManager_OnJobComplete;
                hookManager.InGameShowWindow += DEBUG_HookManager_InGameShowWindow;
                hookManager.BeforeInGameHudInit += DEBUG_HookManager_BeforeInGameHudInit;
                hookManager.BeforeEntitySpawn += DEBUG_HookManager_BeforeEntitySpawn;
                hookManager.OnEntitySpawn += DEBUG_HookManager_OnEntitySpawn;
                hookManager.MainMenuGuiInit += DEBUG_HookManager_MainMenuGuiInit;
                hookManager.BeforeStartNewGame += DEBUG_HookManager_BeforeStartNewGame;
                hookManager.BeforeStartNewGameAfterReadDefs += DEBUG_HookManager_BeforeStartNewGameAfterReadDefs;
                Logger.Log("Attaching DEBUG hooks ... DONE");
            }
        }

        private static void DEBUG_HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
        }

        private static void DEBUG_HookManager_BeforeInGameHudInit()
        {
            //Logger.Log("-- Override initial spawn");
            //GnomanEmpire.Instance.World.AIDirector.PlayerFaction.mReadyToSpawn = true;
            //GnomanEmpire.Instance.World.AIDirector.PlayerFaction.mNextSpawnTime = -1f;
        }

        private static void DEBUG_HookManager_UpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
        }

        private static void DEBUG_HookManager_OnJobComplete(Game.Job job, Game.Character character)
        {
        }

        private static void DEBUG_HookManager_InGameShowWindow(Window window)
        {
        }

        private static void DEBUG_HookManager_BeforeEntitySpawn(GameEntity entity)
        {
        }

        private static void DEBUG_HookManager_OnEntitySpawn(GameEntity entity)
        {
        }

        private static void DEBUG_HookManager_MainMenuGuiInit(MainMenuWindow window, Manager manager)
        {
        }

        private static void DEBUG_HookManager_BeforeStartNewGame(CreateWorldOptions worldOptions)
        {
        }

        private static void DEBUG_HookManager_BeforeStartNewGameAfterReadDefs(CreateWorldOptions worldOptions)
        {
        }
    }
}

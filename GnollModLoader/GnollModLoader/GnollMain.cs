using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.GUI.Controls;
using GnollModLoader.GUI;

namespace GnollModLoader
{
    public class GnollMain
    {
        private const string MAJOR_VERSION = "G1";

        // for easier validation
        public const uint PATCH_VERSION = 10;

        public const string NAME = "Gnoll Mod Loader";
        public const string APP_URL = "https://github.com/Nefaro/gnoll";
        public const string ORIGINAL_URL = "https://github.com/minexew/gnomodkit";

        public const string MODS_DIR = "Gnoll Mods\\enabled";

        private static bool debug = false;
        public static bool Debug => debug;
        public static string VERSION
        {
            get { return MAJOR_VERSION + "." + PATCH_VERSION; }
        }

        static HookManager hookManager;
        static GnollModLoader modManager;

        private GnollMain()
        {
        }

        public static void HookInit()
        {
            // System Console for debugging
            // should probably put behind conf or button press ...
            ConsoleWindow.ShowConsoleWindow();
            if ( !Debug )
                ConsoleWindow.HideConsoleWindow();

            Logger.Log("Gnomodkit {0} {1}", NAME, VERSION);

            hookManager = new HookManager();
            // Apply patches before mods are loaded
            var patcher = new GlobalPatcher(hookManager);
            patcher.PerformPatching();

            modManager = new GnollModLoader(hookManager);
            modManager.LoadModsFrom(MODS_DIR);
        }

        // Called after the game has initialized and game settings have been read in,
        // but befpre the settings have been applied to the game
        public static void HookPostInit()
        {
            if (Game.GnomanEmpire.Instance.Settings.DebugMode)
            {
                Logger.Log("-- Enabling debug mode; override from Game");
                debug = true;
                ConsoleWindow.ShowConsoleWindow();
            }
        }
    }
}

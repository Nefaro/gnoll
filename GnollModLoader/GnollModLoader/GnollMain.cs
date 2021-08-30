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
        public const string NAME = "Gnoll Mod Loader";
        public const string VERSION_STRING = "G1.1";
        public const string APP_URL = "https://github.com/Nefaro/gnoll";
        public const string ORIGINAL_URL = "https://github.com/minexew/gnomodkit";

        public const string MODS_DIR = "Gnoll Mods\\enabled";

        static HookManager hookManager;
        static GnollModLoader modManager;

        private GnollMain()
        {
        }

        public static void HookInit()
        {
            // System Console for debugging
            // should probably put behind conf or button press ...
            // ConsoleWindow.ShowConsoleWindow();

            System.Console.WriteLine(String.Format("Gnomodkit {0} {1}", NAME, VERSION_STRING));

            hookManager = new HookManager();

            modManager = new GnollModLoader(hookManager);
            modManager.LoadModsFrom(MODS_DIR);
        }
    }
}

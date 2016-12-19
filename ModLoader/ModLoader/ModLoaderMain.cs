using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader
{
    public class ModLoaderMain
    {
        public const string name = "ModLoader";
        public const string versionString = "v1.0";
        public const string url = "https://github.com/minexew/gnomodkit";

        public static void HookInit()
        {
            ConsoleWindow.ShowConsoleWindow();

            Console.WriteLine(String.Format("gnomodkit {0} {1}", name, versionString));
        }

        public static void HookMainMenuGuiInit(Game.GUI.MainMenuWindow window, Game.GUI.Controls.Manager manager)
        {
            Game.GUI.Controls.Button aboutButton = window.method_39(manager, "About " + name);

            aboutButton.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                var dialog = new GUI.AboutModLoaderDialog(Game.GnomanEmpire.Instance.GuiManager.Manager);
                dialog.Init();
                dialog.ShowModal();
                Game.GnomanEmpire.Instance.GuiManager.Add(dialog);
            };

            window.panel_0.Add(aboutButton);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModLoader
{
    public class HookManager
    {
        public delegate void ExportMenuGuiInitHandler(Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager);

        public HookManager()
        {
            instance = this;
        }

        public static void HookImportExportMenuGuiInit(Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager)
        {
            if (instance.ExportMenuGuiInit != null)
                instance.ExportMenuGuiInit(importExportMenu, manager);
        }

        public event ExportMenuGuiInitHandler ExportMenuGuiInit;

        private static HookManager instance;
    }
}

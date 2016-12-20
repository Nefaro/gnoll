using Game.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader.GUI
{
    public class AboutModLoaderDialog : MessageDialog
    {
        public AboutModLoaderDialog(Manager manager) : base(
                manager,
                ModLoaderMain.name,
                String.Format("{0}\n{1}\n\n{2}", ModLoaderMain.name, ModLoaderMain.versionString, ModLoaderMain.url)
            )
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace GnollMods.MarketStallFasterScroll
{
    class ModMain : IGnollMod
    {
        public string Name { get { return "MarketStallFasterScroll"; } }

        public string Description { get { return "Speeds up the mouse wheel scroll on the market stall trade panels"; } }

        public string BuiltWithLoaderVersion { get { return "G1.13"; } }

        public int RequireMinPatchVersion { get { return 13; } }
        public ModMain()
        {
        }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameShowWindow += HookManager_InGameShowWindow;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameShowWindow -= HookManager_InGameShowWindow;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }
        private void HookManager_InGameShowWindow(Window window)
        {
            if (window.GetType() == typeof(Game.GUI.MarketStallUI))
            {
                foreach (Game.GUI.TabbedWindowPanel panel in ((Game.GUI.TabbedWindow)window).list_0)
                {
                    if (panel.GetType() == typeof(Game.GUI.MarketStallTradeUI))
                    {
                        foreach (var subPanel in panel.clipBox_0.ControlsList)
                        {
                            if (subPanel.GetType() == typeof(Game.GUI.TradePanelUI))
                            {
                                foreach (var control in subPanel.Controls)
                                {
                                    if (control.GetType() == typeof(Game.GUI.Controls.ScrollBar))
                                    {
                                        ((Game.GUI.Controls.ScrollBar)control).StepSize = 16 * 3;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

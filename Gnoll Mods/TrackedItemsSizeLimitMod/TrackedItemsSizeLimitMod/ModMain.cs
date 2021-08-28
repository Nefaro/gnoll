using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace Mod
{
    class ModMain : IMod
    {
        public static ModMain instance;

        public string Name { get { return "TrackedItemsSizeLimitMod"; } }

        public string Description { get { return "Increases the limit of how many items can be tracked on the main HUD"; } }

        public ModMain()
        {
            instance = this;
        }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            foreach (var control in inGameHUD.gclass0_0.panel_0.clipBox_0.ControlsList)
            {
                if (control.Text.ToLower() == "stocks")
                {
                    control.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
                    {
                        System.Console.WriteLine("-- Stocks clicked");
                        System.Console.WriteLine("-- Active Window: " + inGameHUD.ActiveWindow.Text);
                        foreach (Game.GUI.TabbedWindowPanel panel in ((Game.GUI.TabbedWindow)inGameHUD.ActiveWindow).list_0)
                        {
                            if (panel.Text.ToLower() == "trackeditemsui")
                            {
                                System.Console.WriteLine("-- Tabbed Window Text: " + panel.Text);
                                foreach (var panelControl in panel.clipBox_0.ControlsList)
                                {
                                    if (panelControl.Text.ToLower() == "new")
                                    {
                                        System.Console.WriteLine("-- Panel Control Name: " + panelControl.Name);
                                        System.Console.WriteLine("-- Panel Control Text: " + panelControl.Text);
                                        panel.clipBox_0.Remove(panelControl);

                                        panel.Add(this.Reimplement(panelControl, manager, ((TrackedItemsUI)panel).listBox_0));

                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    };
                    break;
                }
            }
        }

        private Button Reimplement(Control original, Manager manager, ListBox listBox)
        {
            Button clone = new Button(manager);
            clone.Init();
            clone.Left = original.Left;
            clone.Top = original.Top;
            clone.Width = original.Width;

            clone.Text = original.Text;
            clone.Anchor = original.Anchor;
            clone.Click += ((sender, e) =>
            {
                System.Console.WriteLine(" -- Custom TrackedItem event handler");
                TrackedItemGroup itemGroup = new TrackedItemGroup();
                GnomanEmpire.Instance.Fortress.StockManager.TrackedItemGroups.Add(itemGroup);

                listBox.Prop_0.Add("New Group");
                listBox.ItemIndex = listBox.Prop_0.Count - 1;
                GnomanEmpire.Instance.GuiManager.InGameHUD_0.GClass0_0.AddStockTrackLabel(itemGroup);
            });
            return clone;
        }
    }
}

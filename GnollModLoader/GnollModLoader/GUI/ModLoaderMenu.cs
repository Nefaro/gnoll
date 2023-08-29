using Game;
using Game.GUI;
using Game.GUI.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GnollModLoader.GUI
{
    // Gnoll Mods menu at the main menu
    public class ModLoaderMenu : Panel
    {
        private readonly ModManager _modManager;

        private TabControl _tabControl;
        private List<TabbedWindowPanel> _tabbedWindowList;

        public ModLoaderMenu(Manager manager, ModManager modManager) : base(manager)
        {
            this._modManager = modManager;

            this.Init();
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
            this.Color = Color.Transparent;
            this._tabControl = new TabControl(this.Manager);
            this._tabControl.Init();
            this._tabControl.Width = 480;
            this._tabControl.Height = 360;
            this._tabControl.PageChanged += new Game.GUI.Controls.EventHandler(this.OnPanelChanged);
            this.Add(this._tabControl);
            this._tabbedWindowList = new List<TabbedWindowPanel>();

            GnomanEmpire.Instance.GuiManager.Add(this);
            this.CenterTabControl();
            GnomanEmpire.Instance.Graphics.DeviceReset += new System.EventHandler<System.EventArgs>(this.OnDeviceReset);

            this.AddTabPanel("Mods", new ModsMenuPanel(this.Manager, this._modManager));
            this.AddTabPanel("About", new AboutMenuPanel(this.Manager));
        }

        private void OnPanelChanged(object sender, Game.GUI.Controls.EventArgs e)
        {
            if (this._tabControl.SelectedIndex >= this._tabbedWindowList.Count)
                return;
            this._tabbedWindowList[this._tabControl.SelectedIndex].OnActivate();
        }
        private void CenterTabControl()
        {
            this._tabControl.Left = (this.Width - this._tabControl.Width) / 2;
            this._tabControl.Top = (this.Height - this._tabControl.Height) / 2;
        }

        private void OnDeviceReset(object sender, System.EventArgs e)
        {
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
            this.CenterTabControl();
        }

        private void AddTabPanel(string name, TabbedWindowPanel panel)
        {
            TabPage tabPage = this._tabControl.AddPage(name);
            this.InitTabPanel(ref panel);
            tabPage.Add((Control)panel);
            panel.SetupPanel();
            this._tabbedWindowList.Add(panel);
        }

        private void InitTabPanel(ref TabbedWindowPanel panel)
        {
            panel.Init();
            panel.AutoScroll = true;
            panel.Passive = false;
            panel.CanFocus = true;
            panel.Color = Color.Transparent;

            panel.Top = panel.Margins.Top;
            panel.Left = panel.Margins.Left;
            panel.Anchor = Anchors.All;
            panel.Width = this._tabControl.ClientWidth - panel.Margins.Horizontal;
            panel.Height = this._tabControl.ClientHeight - panel.Margins.Vertical;
        }

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            GnomanEmpire.Instance.Graphics.DeviceReset -= new System.EventHandler<System.EventArgs>(this.OnDeviceReset);
            this._tabControl = null;
            this._tabbedWindowList.Clear();
        }
    }
}

using Game;
using Game.GUI;
using Game.GUI.Controls;
using System;
using System.Collections.Generic;

namespace Mod
{
    // Code based on ImportExport*Dialog from retail game

    internal class ImportExportTrackedItemsDialog : Panel
    {
        public ImportExportTrackedItemsDialog(Manager manager) : base(manager)
		{
            this.Init();
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
            this.Color = Microsoft.Xna.Framework.Color.Transparent;

            this.tabControl = new TabControl(this.Manager);
            this.tabControl.Init();
            this.tabControl.Width = 480;
            this.tabControl.Height = 200;
            this.tabControl.PageChanged += new Game.GUI.Controls.EventHandler(this.PageChanged);
            this.Add(this.tabControl);

            this.panels = new List<TabbedWindowPanel>();
            GnomanEmpire.Instance.GuiManager.Add(this);
            this.CenterPanel();
            GnomanEmpire.Instance.Graphics.DeviceReset += new EventHandler<System.EventArgs>(this.Graphics_DeviceReset);

            this.AddPage("Import", new ImportTrackedItemsDialog(this.Manager));
            this.AddPage("Export", new ExportTrackedItemsDialog(this.Manager));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                GnomanEmpire.Instance.Graphics.DeviceReset -= new EventHandler<System.EventArgs>(this.Graphics_DeviceReset);
                this.tabControl = null;
                this.panels.Clear();
            }
        }

        private void PageChanged(object sender, Game.GUI.Controls.EventArgs e)
        {
            if (this.tabControl.SelectedIndex < this.panels.Count)
            {
                this.panels[this.tabControl.SelectedIndex].OnActivate();
            }
        }

        protected void AddPage(string name, TabbedWindowPanel panel)
        {
            TabPage tabPage = this.tabControl.AddPage(name);
            this.SetupTabPanel(ref panel);
            tabPage.Add(panel);
            panel.SetupPanel();
            this.panels.Add(panel);
        }

        protected TabbedWindowPanel CreateTabPanel()
        {
            TabbedWindowPanel result = new TabbedWindowPanel(this.Manager);
            this.SetupTabPanel(ref result);
            return result;
        }

        protected void SetupTabPanel(ref TabbedWindowPanel panel)
        {
            panel.Init();
            panel.AutoScroll = true;
            panel.Passive = false;
            panel.CanFocus = true;
            panel.Top = panel.Margins.Top;
            panel.Left = panel.Margins.Left;
            panel.Width = this.tabControl.ClientWidth - panel.Margins.Horizontal;
            panel.Height = this.tabControl.ClientHeight - panel.Margins.Vertical;
            panel.Anchor = Anchors.All;
            panel.Color = Microsoft.Xna.Framework.Color.Transparent;
        }

        private void CenterPanel()
        {
            this.tabControl.Left = (this.Width - this.tabControl.Width) / 2;
            this.tabControl.Top = (this.Height - this.tabControl.Height) / 2;
        }

        private void Graphics_DeviceReset(object sender, System.EventArgs e)
        {
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
            this.CenterPanel();
        }

        protected TabControl tabControl;
        private List<TabbedWindowPanel> panels;
    }
}

using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GnollMods.Challenges.Gui
{
    public class ChallengesMenu : Panel
    {
        private TabControl _tabControl;
        private List<TabbedWindowPanel> _tabbedWindowList;

        public ChallengesMenu(Manager manager, ChallengesScores scores) : base(manager)
        {
            this.Init();
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
            this.Color = Microsoft.Xna.Framework.Color.Transparent;

            this._tabControl = new TabControl(this.Manager);
            this._tabControl.Init();
            this._tabControl.Width = 640;
            this._tabControl.Height = 360;
            this._tabControl.PageChanged += new Game.GUI.Controls.EventHandler(this.OnPanelChanged);
            this.Add(this._tabControl);
            this._tabbedWindowList = new List<TabbedWindowPanel>();

            GnomanEmpire.Instance.GuiManager.Add(this);
            this.CenterTabControl();
            GnomanEmpire.Instance.Graphics.DeviceReset += new System.EventHandler<System.EventArgs>(this.OnDeviceReset);

            this.AddTabPanel("Challenges", new ChallengesSelectMenuPanel(manager));
            this.AddTabPanel("Scoring (no mods)", new ChallengesScoringMenuPanel(manager, scores.VanillaScores));
            this.AddTabPanel("Scoring (with mods)", new ChallengesScoringMenuPanel(manager, scores.ModdedScores));
        }

        private void OnDeviceReset(object sender, System.EventArgs e)
        {
            this.Width = this.Manager.ScreenWidth;
            this.Height = this.Manager.ScreenHeight;
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

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;
            GnomanEmpire.Instance.Graphics.DeviceReset -= new System.EventHandler<System.EventArgs>(this.OnDeviceReset);
        }

        private void AddTabPanel(string name, TabbedWindowPanel panel)
        {
            TabPage tabPage = this._tabControl.AddPage(name);
            this.InitTabPanel(ref panel);
            tabPage.Add(panel);
            panel.SetupPanel();
            this._tabbedWindowList.Add(panel);
        }

        private void InitTabPanel(ref TabbedWindowPanel panel)
        {
            panel.Init();
            panel.AutoScroll = true;
            panel.Passive = false;
            panel.CanFocus = true;
            panel.Color = Microsoft.Xna.Framework.Color.Transparent;

            panel.Top = panel.Margins.Top;
            panel.Left = panel.Margins.Left;
            panel.Anchor = Anchors.All;
            panel.Height = this._tabControl.ClientHeight - panel.Margins.Vertical;
        }
    }
}

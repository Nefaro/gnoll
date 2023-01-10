using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using GnollMods.Challenges.Gui;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Gui
{
    class ChallengesScoringMenuPanel : AbstractTabbedWindowPanel
    {
        private TabControl _tabControl;
        private List<TabbedWindowPanel> _tabbedWindowList;

        private ScoreTablePanel _tiny;
        private ScoreTablePanel _small;
        private ScoreTablePanel _standard;
        private ScoreTablePanel _large;
        private ScoreTablePanel _huge;

        public ChallengesScoringMenuPanel(Manager manager, Dictionary<String, ChallengesPerChallengeScores> scores) : base(manager)
        {
            this.Width = 630;
            this.Height = 320;

            ListBox listBox = new ListBox(manager);
            listBox.Init();
            listBox.Top = listBox.Margins.Top;
            listBox.Left = listBox.Margins.Left;
            listBox.Width = 160 - listBox.Margins.Vertical;
            listBox.HideSelection = false;
            listBox.Height = 320;

            foreach (IChallenge challenge in ModMain.Instance.ChallengeManager.Challenges)
            {
                listBox.Prop_0.Add(challenge.ChallengeName());
            }

            this.Add(listBox);

            this._tabControl = new TabControl(this.Manager);
            this._tabControl.Init();
            this._tabControl.PageChanged += new Game.GUI.Controls.EventHandler(this.OnPanelChanged);

            this.Add(this._tabControl);
            this._tabbedWindowList = new List<TabbedWindowPanel>();

            _tiny = new ScoreTablePanel(this.Manager);
            _small = new ScoreTablePanel(this.Manager);
            _standard = new ScoreTablePanel(this.Manager);
            _large = new ScoreTablePanel(this.Manager);
            _huge = new ScoreTablePanel(this.Manager);

            this.AddTabPanel("Tiny", _tiny);
            this.AddTabPanel("Small", _small);
            this.AddTabPanel("Standard", _standard);
            this.AddTabPanel("Large", _large);
            this.AddTabPanel("Huge", _huge);

            Button back = CreateBackButton(manager);
            this.Add(back);
            listBox.Height = this.Height - listBox.Margins.Vertical - listBox.Top - back.Height - back.Margins.Vertical - back.Margins.Top;

            this._tabControl.Left = listBox.Left + listBox.Width + listBox.Margins.Left;
            this._tabControl.Width = this.Width - listBox.Width - listBox.Left - listBox.Margins.Horizontal - this._tabControl.Margins.Horizontal - 4;
            this._tabControl.Height = listBox.Height;
            this._tabControl.Top = listBox.Top;

            _tabControl.VerticalScrollBarEnabled = false;
            _tabControl.VerticalScrollBarShow = false;

            listBox.ItemIndexChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                RemoveScoresFromPanel();
                if (listBox.ItemIndex <= ModMain.Instance.ChallengeManager.Challenges.Count)
                {
                    ChallengesPerChallengeScores challengeScores;
                    var challengeName = ModMain.Instance.ChallengeManager.Challenges[listBox.ItemIndex].ChallengeName();
                    scores.TryGetValue(challengeName, out challengeScores);
                    if (challengeScores != null)
                    {
                        ApplyScoresToPanel(challengeScores);
                    }
                }
            };

            if (ModMain.Instance.ChallengeManager.Challenges.Count > 0)
            {
                listBox.ItemIndex = 0;
            }

        }
        private Button CreateBackButton(Manager manager)
        {
            Button back = CreateButton(manager, "Back");
            back.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            return back;
        }

        private Button CreateButton(Manager manager, string text)
        {
            Button button = new Button(manager);
            button.Init();
            button.Margins = new Margins(4, 4, 4, 4);
            button.Text = text;
            button.Left = button.Margins.Left;
            button.Anchor = Anchors.Left | Anchors.Bottom;
            button.Top = this.Height - this.ClientMargins.Vertical - button.Height - button.Margins.Bottom;
            return button;
        }
        private string SpliceText(string text, int max)
        {
            return String.Join(Environment.NewLine,
                TextBox.WrapText(text, (double)max, Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource));
        }

        private void OnPanelChanged(object sender, Game.GUI.Controls.EventArgs e)
        {
            if (this._tabControl.SelectedIndex >= this._tabbedWindowList.Count)
                return;
            this._tabbedWindowList[this._tabControl.SelectedIndex].OnActivate();
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
            panel.Height = _tabControl.ClientHeight;
            panel.Width = this.Width;

            panel.VerticalScrollBarEnabled = false;
            panel.VerticalScrollBarShow = false;
        }

        private void ApplyScoresToPanel(ChallengesPerChallengeScores challengeScores)
        {
            var sizeToPanelMapping = new Dictionary<int, ScoreTablePanel>()
            {
                { 64, _tiny},
                { 96, _small},
                { 128, _standard},
                { 160, _large},
                { 192, _huge},
            };

            List<ChallengesScoreRecord> scores;
            foreach (var panelEntry in sizeToPanelMapping)
            {
                scores = null;
                challengeScores.Scores.TryGetValue(ModMain.Instance.ChallengeManager.SizeMap[panelEntry.Key], out scores);
                if (scores != null)
                {
                    panelEntry.Value.ApplyScores(scores);
                }
            }
        }

        private void RemoveScoresFromPanel()
        {
            _tiny.ClearScores();
            _small.ClearScores();
            _standard.ClearScores();
            _large.ClearScores();
            _huge.ClearScores();
        }
    }
}

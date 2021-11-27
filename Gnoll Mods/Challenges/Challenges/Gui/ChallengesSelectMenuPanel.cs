using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using GnollMods.Challenges.Challenge;
using Microsoft.Xna.Framework.Graphics;

namespace GnollMods.Challenges.Gui
{
    class ChallengesSelectMenuPanel : AbstractTabbedWindowPanel
    {
        public ChallengesSelectMenuPanel(HookManager hookManager, Manager manager) : base(manager)
        {
            this.Width = 630;
            this.Height = 320;
            ListBox listBox = new ListBox(manager);
            listBox.Init();
            listBox.Top = listBox.Margins.Top;
            listBox.Left = listBox.Margins.Left;
            listBox.Width = 160 - listBox.Margins.Vertical;
            listBox.HideSelection = false;

            var challenges = ModMain.Instance.ChallengeManager.Challenges;

            foreach (IChallenge challenge in challenges)
            {
                listBox.Prop_0.Add(challenge.ChallengeName());
            }

            this.Add(listBox);

            Button back = CreateBackButton(manager);
            this.Add(back);

            Button start = CreateButton(manager, "Start");
            this.Add(start);

            listBox.Height = this.Height - listBox.Margins.Vertical - listBox.Top - back.Height - back.Margins.Vertical - back.Margins.Top;
            start.Left = this.Width - start.Width - 4 * 4;

            LoweredPanel descPanel = new LoweredPanel(manager);
            descPanel.Init();
            descPanel.Height = listBox.Height;
            descPanel.Width = this.Width - listBox.Width - listBox.Left - listBox.Margins.Horizontal - descPanel.Margins.Horizontal - 4;
            descPanel.Left = listBox.Left + listBox.Width + listBox.Margins.Left;
            descPanel.Top = listBox.Top;
            this.Add(descPanel);
            Label description = new Label(manager);
            description.Text = "";
            description.Height = descPanel.Height - descPanel.Margins.Vertical;
            description.Width = descPanel.Width - descPanel.Margins.Horizontal;
            description.Top = 0;
            descPanel.Add(description);

            Label objectives = new Label(manager);
            objectives.Text = "";
            objectives.Height = descPanel.Height - descPanel.Margins.Vertical;
            objectives.Width = descPanel.Width - descPanel.Margins.Horizontal;
            objectives.Top = 0;
            objectives.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            descPanel.Add(objectives);

            Label timeframe = new Label(manager);
            timeframe.Text = "";
            timeframe.Height = descPanel.Height - descPanel.Margins.Vertical;
            timeframe.Width = descPanel.Width - descPanel.Margins.Horizontal;
            timeframe.Top = 0;
            timeframe.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            descPanel.Add(timeframe);

            Label rules = new Label(manager);
            rules.Text = "";
            rules.Height = descPanel.Height - descPanel.Margins.Vertical;
            rules.Width = descPanel.Width - descPanel.Margins.Horizontal;
            rules.Top = 0;
            rules.TextColor = Microsoft.Xna.Framework.Color.Orange;
            descPanel.Add(rules);

            listBox.ItemIndexChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                var font = Manager.Skin.Controls["Label"].Layers[0].Text.Font;
                var fontResource = font.Resource;
                if (listBox.ItemIndex < challenges.Count)
                {
                    rules.Hide();
                    var chall = challenges[listBox.ItemIndex];
                    description.Text = this.SpliceText(chall.ChallengeDescription(), 48 * 9);
                    description.Text += "\n\n===== ===== ===== ===== ===== ===== =====\n";
                    var size = fontResource.MeasureString(description.Text);
                    description.Height = (int)size.Y;

                    objectives.Top = description.Height;
                    objectives.Text = "Objective: " + this.SpliceText(chall.ChallengeObjective(), 40 * 9);
                    size = fontResource.MeasureString(objectives.Text);
                    objectives.Height = (int)size.Y;

                    timeframe.Top = objectives.Top + objectives.Height; ;
                    timeframe.Text = "Timeframe: " + this.SpliceText(chall.ChallengeTimeframe(), 40 * 9);
                    size = fontResource.MeasureString(timeframe.Text);
                    timeframe.Height = (int)size.Y;

                    if (!String.IsNullOrEmpty(chall.AdditionalRules()))
                    {
                        rules.Top = timeframe.Top + timeframe.Height; ;
                        rules.Text = "\nSpecial Rules: " + this.SpliceText(chall.AdditionalRules(), 40 * 9);
                        size = fontResource.MeasureString(rules.Text);
                        rules.Height = (int)size.Y;
                        rules.Show();
                    }
                }
            };
            if (challenges.Count > 0)
            {
                listBox.ItemIndex = 0;
            }

            start.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                if (listBox.ItemIndex < challenges.Count)
                {
                    ModMain.Instance.ChallengeManager.StartChallenge(listBox.ItemIndex);
                    GnomanEmpire.Instance.GuiManager.MenuStack.PushWindow(new NewGameWindow(GnomanEmpire.Instance.GuiManager.Manager));
                }
            };
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
            SpriteFont font = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource;
            string[] textArray = text.Split('\n');
            for(int i = 0; i < textArray.Count(); i++)
            {
                if ( font.MeasureString(textArray[i]).X > max )
                {
                    textArray[i] = String.Join(Environment.NewLine, 
                        TextBox.WrapText(textArray[i], (double)max, font));
                }
            }
            return String.Join(Environment.NewLine, textArray);
        }
    }
}

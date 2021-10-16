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

            listBox.ItemIndexChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                if (listBox.ItemIndex < challenges.Count)
                {
                    description.Text = this.SpliceText(challenges[listBox.ItemIndex].ChallengeDescription(), 48 * 9);
                    description.Text += "\n\n===== ===== ===== ===== ===== ===== =====\n";
                    description.Text += "\nObjective: " + this.SpliceText(challenges[listBox.ItemIndex].ChallengeObjective(), 40 * 9);
                    description.Text += "\nTimeframe: " + this.SpliceText(challenges[listBox.ItemIndex].ChallengeTimeframe(), 40 * 9);
                    var size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(description.Text);
                    description.Height = (int)size.Y;
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

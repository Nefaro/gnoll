using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;
using Microsoft.Xna.Framework.Graphics;

namespace GnollMods.Challenges.Gui
{
    public class ChallengesEndDialogPanel : TabbedWindowPanel
    {
        private int _rowHeight = 18;
        private int _topMargin = 2;

        public ChallengesEndDialogPanel(Manager manager, IChallenge challenge) : base(manager)
        {
            LoweredPanel desc = new LoweredPanel(manager);
            desc.Init();
            desc.Top = 10;
            desc.Left = 10;
            desc.Width = 435;
            desc.Height = 150;

            this.Add(desc);

            Label endword = new Label(manager);
            endword.Init();
            endword.Text = this.SpliceText(challenge.ChallengeEndMessage(), 48 * 9);
            endword.Height = _rowHeight;
            endword.Top = 0;
            endword.Width = 430;

            var size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(endword.Text);
            endword.Height = (int)size.Y;

            desc.Add(endword);

            Label separator = new Label(manager);
            separator.Init();
            separator.Text = "==== ==== ==== ====";
            separator.Height = _rowHeight;
            separator.Top = endword.Height + 2 * _topMargin;
            separator.Width = 430;
            desc.Add(separator);

            Label score = new Label(manager);
            score.Init();
            score.Text = "FINAL SCORE: " + challenge.CalculateScore();
            score.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            score.Height = _rowHeight;
            score.Top = separator.Top + separator.Height + 2 * _topMargin;
            score.Width = 430;
            desc.Add(score);
        }

        private string SpliceText(string text, int max)
        {
            SpriteFont font = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource;
            string[] textArray = text.Split('\n');
            for (int i = 0; i < textArray.Count(); i++)
            {
                if (font.MeasureString(textArray[i]).X > max)
                {
                    textArray[i] = String.Join(Environment.NewLine,
                        TextBox.WrapText(textArray[i], (double)max, font));
                }
            }
            return String.Join(Environment.NewLine, textArray);
        }
    }

    public class ChallengesEndDialog : TabbedWindow
    {
        public ChallengesEndDialog(Manager manager, IChallenge challenge) : base(manager)
        {
			this.Text = "Challenge Fulfilled";
			this.Width = 510;
			this.Height = 290;
			this.Resizable = false;
			this.Center();

            base.AddPage("Score", new ChallengesEndDialogPanel(manager, challenge));
        }
    }
}

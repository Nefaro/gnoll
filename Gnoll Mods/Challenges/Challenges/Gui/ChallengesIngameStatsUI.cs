using System;
using System.Collections.Generic;
using System.Linq;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Gui
{
    internal class ChallengesIngameStatsUI : TabbedWindowPanel
    {
        private int _rowHeight = 18;
        private int _topMargin = 2;

        public ChallengesIngameStatsUI(Manager manager, ChallengesScoreRecord record, IChallenge challenge) : base(manager)
        {
            LoweredPanel desc = new LoweredPanel(manager);
            desc.Init();
            desc.Top = 10;
            desc.Left = 10;
            desc.Width = 435;
            desc.Height = 150;
            desc.AutoScroll = true;
            desc.HorizontalScrollBarEnabled = false;

            this.Add(desc);

            var stats = challenge.CalculateStats();
            int line = 0;
            foreach(var item in stats)
            {
                AddStatsLine(manager, desc, item, line++);
            }
            Label separator = new Label(manager);
            separator.Init();
            separator.Text = "===== ===== ===== ===== ===== ======";
            separator.Height = _rowHeight;
            separator.Top = (line++) * (_rowHeight + _topMargin);
            separator.Width = 450;
            desc.Add(separator);

            Label total = new Label(manager);
            total.Init();
            total.Text = challenge.CalculateScore();
            total.Height = _rowHeight;
            total.Top = (line++) * (_rowHeight + _topMargin);
            total.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            total.BackColor = Microsoft.Xna.Framework.Color.BurlyWood;
            total.Left = 250 + 3 * total.Margins.Horizontal - 3;
            total.Alignment = Alignment.MiddleRight;
            total.Width = 60;
            desc.Add(total);
        }

        private void AddStatsLine(Manager manager, LoweredPanel panel, StatsItem item, int line)
        {
            Label name = new Label(manager);
            name.Init();
            name.Text = item.ItemName + " : ";
            name.Height = _rowHeight;
            name.Top = line * (_rowHeight + _topMargin);
            name.Width = 150;
            panel.Add(name);

            Label count = new Label(manager);
            count.Init();
            count.Text = item.ItemCount + " ";
            count.Height = _rowHeight;
            count.Top = line * (_rowHeight + _topMargin);
            count.Left = name.Left + name.Width + count.Margins.Horizontal;
            count.Width = 50;
            count.Alignment = Alignment.MiddleRight;
            panel.Add(count);

            Label multy = new Label(manager);
            multy.Init();
            multy.Text = " x" +item.ScoreMultiplyer;
            multy.Height = _rowHeight;
            multy.Top = line * (_rowHeight + _topMargin);
            multy.Left = count.Left + count.Width + multy.Margins.Horizontal - 3;
            multy.Width = 50;
            panel.Add(multy);

            Label sum = new Label(manager);
            sum.Init();
            sum.Text = "" + Math.Round(item.ScoreMultiplyer * item.ItemCount);
            sum.Height = _rowHeight;
            sum.Top = line * (_rowHeight + _topMargin);
            sum.Left = multy.Left + multy.Width + multy.Margins.Horizontal;
            sum.Width = 60;
            sum.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            sum.Alignment = Alignment.MiddleRight;
            panel.Add(sum);
        }
    }
}
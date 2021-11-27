using System;
using System.Collections.Generic;
using System.Linq;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Gui
{
    internal class ChallengesIngameScoreUI : TabbedWindowPanel
    {
        private int _rowHeight = 18;
        private int _topMargin = 2;

        public ChallengesIngameScoreUI(Manager manager, ChallengesScoreRecord record, IChallenge challenge) : base(manager)
        {
            LoweredPanel desc = new LoweredPanel(manager);
            desc.Init();
            desc.Top = 10;
            desc.Left = 10;
            desc.Width = 435;
            desc.Height = 150;

            this.Add(desc);

            int line = 0;

            Label score = new Label(manager);
            score.Init();
            score.Text = "";
            score.Height = _rowHeight;
            score.Top = line * (_rowHeight + _topMargin);
            score.Width = 430;
            score.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            desc.Add(score);

            Label separator = new Label(manager);
            separator.Init();
            separator.Text = "==== ==== ==== ====";
            separator.Height = _rowHeight;
            separator.Top = ++line * (_rowHeight + _topMargin);
            separator.Width = 430;
            desc.Add(separator);

            Label kingdom = new Label(manager);
            kingdom.Init();
            kingdom.Text = "";
            kingdom.Height = _rowHeight;
            kingdom.Top = ++line * (_rowHeight + _topMargin);
            kingdom.Width = 430;
            desc.Add(kingdom);

            Label difficulty = new Label(manager);
            difficulty.Init();
            difficulty.Text = "";
            difficulty.Height = _rowHeight;
            difficulty.Top = ++line * (_rowHeight + _topMargin);
            difficulty.Width = 430;
            desc.Add(difficulty);

            Label metal = new Label(manager);
            metal.Init();
            metal.Text = "";
            metal.Height = _rowHeight;
            metal.Top = ++line * (_rowHeight + _topMargin);
            metal.Width = 430;
            desc.Add(metal);

            Label enemy = new Label(manager);
            enemy.Init();
            enemy.Text = "";
            enemy.Height = _rowHeight;
            enemy.Top = ++line * (_rowHeight + _topMargin);
            enemy.Width = 430;
            desc.Add(enemy);

            Label enemy2 = new Label(manager);
            enemy2.Init();
            enemy2.Text = "";
            enemy2.Height = _rowHeight;
            enemy2.Top = ++line * (_rowHeight + _topMargin);
            enemy2.Width = 430;
            enemy2.TextColor = Microsoft.Xna.Framework.Color.Orange;
            desc.Add(enemy2);

            kingdom.Text = string.Format("{0} ({1})",
                record.KingdomName,
                record.KingdomSeed);

            var isScoringEnded = challenge.IsEndConditionsMet();

            score.Text = string.Format("CURRENT SCORE: {0}" +(isScoringEnded?" (scoring has ended)":"") ,
                record.Score);

            difficulty.Text = string.Format("Difficulty: {0}",
                record.Difficulty);

            metal.Text = string.Format("{0} amount of metal in {1} depth",
                record.MetalAmount,
                record.MetalDepth);

            // Filter out our tags
            List<string> filtered = record.EnemyList
                .FindAll(item => !item.StartsWith(ChallengesManager.TAG_START))
                .ToList();

            if (ChallengesManager.GAME_MODE_PEACEFUL.Equals(record.Difficulty, StringComparison.OrdinalIgnoreCase))
            {
                enemy.Text = String.Format("Peaceful, enemies disabled");
                enemy.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
                enemy2.Text = "";
            }
            else if (filtered.Count() > 0)
            {
                enemy.Text = string.Format("Enemy strength {0} ({1} growth)",
                    record.EnemyStrength,
                    (record.EnemyStrengthGrowth ? "with" : "without"));
                enemy.TextColor = Microsoft.Xna.Framework.Color.Orange;

                enemy2.Text = string.Format("Attack rate {0} and size {1} by {2} type(s) of enemies",
                    record.EnemyAttackRate,
                    record.EnemyAttackSize,
                    filtered.Count());
            }
            else
            {
                enemy.Text = String.Format("No enemies selected");
                enemy.TextColor = Microsoft.Xna.Framework.Color.LimeGreen;
                enemy2.Text = "";
            }
        }
    }
}
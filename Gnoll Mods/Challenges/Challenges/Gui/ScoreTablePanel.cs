using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Gui
{
    class Row
    {
        public Label rank;
        public Button kingdom;
        public Label score;

        public Row(Label rank, Button kingdom, Label score)
        {
            this.rank = rank;
            this.kingdom = kingdom;
            this.score = score;
            this.Hide();
        }

        public void Hide()
        {
            this.rank.Hide();
            this.kingdom.Hide();
            this.score.Hide();
        }

        public void Reset()
        {
            this.kingdom.Text = "";
            this.score.Text = "";
            this.Hide();
        }

        public void Show()
        {
            this.rank.Show();
            this.kingdom.Show();
            this.score.Show();
        }

        public void SetNameAndScore(string name, string score)
        {
            this.kingdom.Text = name;
            this.score.Text = score;
        }
    }

    class Description
    {
        private LoweredPanel _panel;
        private Label _score;
        private Label _kingdom;
        private Label _difficulty;
        private Label _metal;
        private Label _enemy;
        private Label _enemy2;

        public Description(LoweredPanel panel, Label score, Label kingdom, Label difficulty, Label metal, Label enemy, Label enemy2)
        {
            this._panel = panel;
            this._score = score;
            this._kingdom = kingdom;
            this._difficulty = difficulty;
            this._metal = metal;
            this._enemy = enemy;
            this._enemy2 = enemy2;
            this.Hide();
        }

        public void Hide()
        {
            this._panel.Hide();
        }

        public void Show()
        {
            this._panel.Show();
        }

        public void FillData(ChallengesScoreRecord data)
        {
            _score.Text = String.Format("SCORE: {0} on date {1}",
                data.Score,
                data.Date);

            _kingdom.Text = String.Format("{0} ({1})", 
                data.KingdomName,
                data.KingdomSeed);

            _difficulty.Text = String.Format("Difficulty: {0} , Base Mod: {1}",
                data.Difficulty,
                data.BaseMod);

            _metal.Text = String.Format("{0} amount of metal in {1} depth",
                data.MetalAmount,
                data.MetalDepth);

            // Filter out our tags
            List<string> filtered = data.EnemyList
                .FindAll(item => !item.StartsWith(ChallengesManager.TAG_START))
                .ToList();

            if (ChallengesManager.GAME_MODE_PEACEFUL.Equals(data.Difficulty, StringComparison.OrdinalIgnoreCase))
            {
                _enemy.Text = String.Format("Peaceful, enemies disabled");
                _enemy2.Text = "";
            }
            else if ( filtered.Count() > 0 )
            {
                _enemy.Text = String.Format("Enemy strength {0} ({1} growth)",
                    data.EnemyStrength,
                    (data.EnemyStrengthGrowth ? "with" : "without"));

                _enemy2.Text = String.Format("Enemy attack rate {0}, size {1} and {2} enemy types",
                    data.EnemyAttackRate,
                    data.EnemyAttackSize,
                    filtered.Count());
            }
            else
            {
                _enemy.Text = String.Format("No enemies selected");
                _enemy2.Text = "";
            }

            this.Show();
        }

    }
    class ScoreTablePanel : TabbedWindowPanel
    {
        private const int LINES_LIMIT = 5;

        private int _rowHeight = 18;
        private int _topMargin = 2;
        private List<ChallengesScoreRecord> _scoreRecords = new List<ChallengesScoreRecord>();
        private List<Row> _rows = new List<Row>();
        private Label _noScores;
        private Description _desc;

        public ScoreTablePanel(Manager manager) : base(manager)
        {
            _rows.Add(BuildRow(manager, 0, "", ""));
            _rows.Add(BuildRow(manager, 1, "", ""));
            _rows.Add(BuildRow(manager, 2, "", ""));
            _rows.Add(BuildRow(manager, 3, "", ""));
            _rows.Add(BuildRow(manager, 4, "", ""));

            _noScores = new Label(manager);
            _noScores.Init();
            _noScores.Text = "No scores to display";
            _noScores.Height = _rowHeight;
            _noScores.Width = 360;
            _noScores.Top = 0;

            this.Add(_noScores);

            this.AddDescriptionPanel(manager);
        }

        public void ApplyScores(List<ChallengesScoreRecord> Scores)
        {
            HideDescription();
            if ( Scores != null )
            {
                var orderedScores = Scores.OrderByDescending(sc => sc.Score).ToList();
                this._noScores.Hide();
                for ( int i = 0; (i < _rows.Count && i < LINES_LIMIT) ; i++)
                {
                    _rows[i].Hide();
                    if ( i < orderedScores.Count)
                    {
                        var data = orderedScores[i];
                        _rows[i].SetNameAndScore(data.KingdomName, data.Score);
                        _rows[i].kingdom.Click += (sender, e) =>
                        {
                            ShowDescription(data);
                        };
                        _rows[i].Show();

                        if ( i == 0)
                        {
                            ShowDescription(data);
                        }
                    }
                }
            }
            else
            {
                this._noScores.Show();
            }
        }

        public void ClearScores()
        {
            this._noScores.Show();
            HideDescription();
            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i].Reset();
            }
        }

        private Row BuildRow(Manager manager, int idx, string kingdom, string score)
        {
            Label rank = new Label(manager);
            rank.Init();
            rank.Text = string.Format("#{0}", (idx+1));
            rank.Height = _rowHeight;
            rank.Width = 30;
            rank.Top = idx * (_rowHeight + _topMargin);
            this.Add(rank);

            Button name = new Button(manager);
            name.Init();
            name.Text = kingdom;
            name.Width = 240;
            name.Height = _rowHeight;
            name.Top =  idx * (_rowHeight + _topMargin);
            name.Left = rank.Width;
            this.Add(name);

            Label label1 = new Label(manager);
            label1.Init();
            label1.Text = score;
            label1.Width = 120;
            label1.Height = _rowHeight;
            label1.Top = idx * (_rowHeight + _topMargin);
            label1.Left = label1.Margins.Left + name.Width + name.Margins.Horizontal + name.Left;
            this.Add(label1);

            return new Row(rank, name, label1);
        }

        private void AddDescriptionPanel(Manager manager)
        {
            LoweredPanel desc = new LoweredPanel(manager);
            desc.Init();
            desc.Top = 5 * (_rowHeight + _topMargin);
            desc.Width = 435;
            desc.Height = 130;

            this.Add(desc);
            var idx = 0;

            Label score = new Label(manager);
            score.Init();
            score.Text = "";
            score.Height = _rowHeight;
            score.Top = idx++ * (_rowHeight + _topMargin);
            score.Width = 430;
            score.TextColor = Microsoft.Xna.Framework.Color.Yellow;
            desc.Add(score);

            Label kingdom = new Label(manager);
            kingdom.Init();
            kingdom.Text = "";
            kingdom.Height = _rowHeight;
            kingdom.Top = idx++ * (_rowHeight + _topMargin);
            kingdom.Width = 430;
            desc.Add(kingdom);

            Label difficulty = new Label(manager);
            difficulty.Init();
            difficulty.Text = "";
            difficulty.Height = _rowHeight;
            difficulty.Top = idx++ * (_rowHeight + _topMargin);
            difficulty.Width = 430;
            desc.Add(difficulty);

            Label metal = new Label(manager);
            metal.Init();
            metal.Text = "";
            metal.Height = _rowHeight;
            metal.Top = idx++ * (_rowHeight + _topMargin);
            metal.Width = 430;
            desc.Add(metal);

            Label enemy = new Label(manager);
            enemy.Init();
            enemy.Text = "";
            enemy.Height = _rowHeight;
            enemy.Top = idx++ * (_rowHeight + _topMargin);
            enemy.Width = 430;
            desc.Add(enemy);

            Label enemy2 = new Label(manager);
            enemy2.Init();
            enemy2.Text = "";
            enemy2.Height = _rowHeight;
            enemy2.Top = idx++ * (_rowHeight + _topMargin);
            enemy2.Width = 430;
            desc.Add(enemy2);

            this._desc = new Description(desc, score,  kingdom, difficulty, metal, enemy, enemy2);
        }

        private void ShowDescription(ChallengesScoreRecord record)
        {
            this._desc.FillData(record);
        }

        private void HideDescription()
        {
            this._desc.Hide();
        }
    }
}

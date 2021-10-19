using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Gui;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges
{
    internal class ChallengesManager
    {
        private readonly static string SCORE_FILE = GnomanEmpire.SaveFolderPath("GnollChallenges") + "\\{0}_scores.json";

        public readonly static string TAG_START = "tag:";
        public readonly static string GAME_MODE_PEACEFUL = "Peaceful";

        private readonly static string START_TAG_PATTERN = TAG_START + "{0}:start";
        private readonly static string END_TAG_PATTERN = TAG_START + "{0}:end";

        private static readonly Dictionary<int, string> _sizeMap = new Dictionary<int, string>()
        {
            { 64, "Tiny"},
            { 96, "Small"},
            { 128, "Standard"},
            { 160, "Large"},
            { 192, "Huge"},
        };

        public Dictionary<int, string> SizeMap { get { return _sizeMap; } }

        private static readonly List<string> _metalDepth = new List<string>()
        {
            "Shallow",
            "Normal",
            "Deep"
        };

        private static readonly List<string> _metalAmount = new List<string>()
        {
            "Scarce",
            "Normal",
            "Abundant"
        };

        private readonly IList<IChallenge> _challenges = new List<IChallenge>()
        {
            new LumberjackChallenge(),
            new OrchardChallenge()
        };

        public IList<IChallenge> Challenges { get { return _challenges; } }

        public IChallenge ActiveChallenge { get; set; }

        private bool _isNewStart = false;
        internal void StartChallenge(int challengeIdx)
        {
            if (challengeIdx < Challenges.Count)
            {
                this.ActiveChallenge = Challenges[challengeIdx];
                this.ActiveChallenge.OnStart();
                this._isNewStart = true;
            }
        }

        internal void Reset()
        {
            this.ActiveChallenge = null;
            this._isNewStart = false;
        }

        private void OnDayStart(object sender, System.EventArgs e)
        {
            // check if challenge end condition is met
            var finished = this.IsChallengeFinished();
            if ( finished )
            {
                System.Console.WriteLine(" -- Challenge end reached!");
                var inGameHud = GnomanEmpire.Instance.GuiManager.InGameHUD_0;
                var manager = GnomanEmpire.Instance.GuiManager.Manager;

                bool flag = !(inGameHud.ActiveWindow is ChallengesIngameUI);
                inGameHud.CloseWindow();
                if (flag)
                {
                    inGameHud.ShowWindow(new ChallengesEndDialog(manager, this.ActiveChallenge), true);
                }
            }
        }

        private bool IsChallengeFinished()
        {
            if (this.ActiveChallenge != null && this.ActiveChallenge.IsEndConditionsMet())
            {
                HashSet<string> settings = Game.GnomanEmpire.Instance.World.DifficultySettings.hashSet_0;
                string endTag = FormatEndTag(this.ActiveChallenge);
                settings.Add(endTag);
                SaveScore();
                Game.GnomanEmpire.Instance.Region.OnDayStart -= OnDayStart;
                return true;
            }
            return false;
        }

        internal void HookManager_InGameHUDInit(InGameHUD inGameHUD, Manager manager)
        {
            // need to check if the current game has any tags
            // need to find which challenge is tagged
            HashSet<string> settings = Game.GnomanEmpire.Instance.World.DifficultySettings.hashSet_0;
            var hasTags = false;
            if ( !this._isNewStart )
            {
                foreach (string item in settings)
                {
                    if (item.StartsWith(TAG_START))
                    {
                        hasTags = true;
                        break;
                    }
                }
                // No tag = not a challenge
                if (!hasTags)
                {
                    System.Console.WriteLine("-- Found no tags; challenges disabled");
                    this.ActiveChallenge = null;
                    return;
                }
            }

            if (this.ActiveChallenge == null)
            {
                foreach (var challenge in this.Challenges)
                {
                    if (settings.Contains(FormatStartTag(challenge)))
                    {
                        this.ActiveChallenge = challenge;
                        System.Console.WriteLine("-- Found current challenge: {0}", this.ActiveChallenge);
                        break;
                    }
                }
            }
            var startTag = this.FormatStartTag(this.ActiveChallenge);
            var endTag = this.FormatEndTag(this.ActiveChallenge);
            if (this._isNewStart)
            {
                settings.Add(startTag);
            }

            if ( settings.Contains(startTag))
            {
                this.AttachIngameUI(inGameHUD, manager);
                if (!settings.Contains(endTag))
                {
                    // Active challenge
                    // Day start = sunrise
                    System.Console.WriteLine(" -- Day start event handler attached");
                    Game.GnomanEmpire.Instance.Region.OnDayStart += OnDayStart;
                    // just in case the challenge has already finished
                    this.IsChallengeFinished();
                }
            }
        }

        private void AttachIngameUI(InGameHUD inGameHUD, Manager manager)
        {
            if (this.ActiveChallenge != null)
            {
                GClass0 glass = inGameHUD.GClass0_0;

                Button button7 = glass.method_40("Challenges", "Challenges", new Game.GUI.Controls.EventHandler((sender, events) =>
                {
                    bool flag = !(inGameHUD.ActiveWindow is ChallengesIngameUI);
                    inGameHUD.CloseWindow();
                    if (flag)
                    {
                        string mapSize = this.SizeMap[GnomanEmpire.Instance.World.Region.Map.MapWidth];
                        ChallengesScoreRecord record = this.BuildScoreRecord(this.ActiveChallenge, mapSize);
                        inGameHUD.ShowWindow(new ChallengesIngameUI(manager, record, this.ActiveChallenge), true);
                    }
                }));
                glass.panel_0.Add(button7);
                button7.Left = glass.panel_0.Width + button7.Margins.Left;
                glass.panel_0.Width = button7.Left + button7.Width;
                glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;
            }
        }

        public void SaveScore()
        {
            if ( this.ActiveChallenge == null)
                return;

            IChallenge challenge = this.ActiveChallenge;

            System.Console.WriteLine(" -- Save scores called");

            ChallengesScores scoreData = this.LoadScore();
            List<ModFolder> modList = new List<ModFolder>(GnomanEmpire.Instance.GameDefs.ModFolders);

            if (modList.Count == 1 && modList[0].SteamWorkshopItemID == 0)
            {
                System.Console.WriteLine("-- -- Saving vanilla scores");
                scoreData.VanillaScores = this.BuildScores(challenge, scoreData.VanillaScores);
            }
            else
            {
                System.Console.WriteLine("-- -- Saving modded scores");
                scoreData.ModdedScores = this.BuildScores(challenge, scoreData.ModdedScores);
            }

            scoreData.KeepTop10();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(scoreData, Newtonsoft.Json.Formatting.Indented);
            string filename = string.Format(SCORE_FILE, challenge.ChallengeName().ToLower());
            System.Console.WriteLine(" -- -- saving scores to {0}", filename);
            System.IO.File.WriteAllText(filename, json);
        }

        private Dictionary<String, ChallengesPerChallengeScores> BuildScores(IChallenge challenge, Dictionary<String, ChallengesPerChallengeScores> scoresDictionary)
        {
            ChallengesPerChallengeScores perChallengeScore;

            if (!scoresDictionary.TryGetValue(challenge.ChallengeName(), out perChallengeScore))
            {
                perChallengeScore = new ChallengesPerChallengeScores();
                scoresDictionary.Add(challenge.ChallengeName(), perChallengeScore);
            }

            string mapSize = this.SizeMap[GnomanEmpire.Instance.World.Region.Map.MapWidth];
            ChallengesScoreRecord record = this.BuildScoreRecord(challenge, mapSize);

            List<ChallengesScoreRecord> recordList;
            if (!perChallengeScore.Scores.TryGetValue(mapSize, out recordList))
            {
                recordList = new List<ChallengesScoreRecord>();
                perChallengeScore.Scores.Add(mapSize, recordList);
            }
            recordList.Add(record);
            return scoresDictionary;
        }

        private ChallengesScoreRecord BuildScoreRecord(IChallenge challenge, string size)
        {
            ChallengesScoreRecord record = new ChallengesScoreRecord();
            record.KingdomName = GnomanEmpire.Instance.World.AIDirector.PlayerFaction.Name;
            record.KingdomSize = size;
            record.KingdomSeed = "" + GnomanEmpire.Instance.World.Region.Map.WorldSeed;
            record.Score = challenge.CalculateScore();
            record.Difficulty = GnomanEmpire.Instance.World.DifficultySettings.gameMode_0.ToString();
            record.BaseMod = "" + GnomanEmpire.Instance.GameDefs.BaseModFolder.SteamWorkshopItemID;
            record.ModList =
                new List<ModFolder>(GnomanEmpire.Instance.GameDefs.ModFolders)
                .Select(mod => mod.Folder)
                .ToList();
            record.Date = DateTime.Now.ToString();
            if (GnomanEmpire.Instance.World.DifficultySettings.int_0 < _metalDepth.Count)
                record.MetalDepth = _metalDepth[GnomanEmpire.Instance.World.DifficultySettings.int_0];
            else
                record.MetalDepth = _metalDepth[1];

            if (GnomanEmpire.Instance.World.DifficultySettings.int_1 < _metalAmount.Count)
                record.MetalAmount = _metalAmount[GnomanEmpire.Instance.World.DifficultySettings.int_1];
            else
                record.MetalAmount = _metalAmount[1];

            record.EnemyList = GnomanEmpire.Instance.World.DifficultySettings.hashSet_0.ToList();
            record.EnemyStrength = "" + GnomanEmpire.Instance.World.DifficultySettings.RawEnemyStrength;
            record.EnemyStrengthGrowth = GnomanEmpire.Instance.World.DifficultySettings.IncreaseOverTime;
            record.EnemyAttackRate = "" + GnomanEmpire.Instance.World.DifficultySettings.AttackRate;
            record.EnemyAttackSize = "" + GnomanEmpire.Instance.World.DifficultySettings.AttackSize;
            return record;
        }

        public ChallengesScores LoadScore()
        {
            System.Console.WriteLine(" -- Loading scores");
            ChallengesScores scores = new ChallengesScores();
            foreach (var challenge in _challenges)
            {
                string filename = string.Format(SCORE_FILE, challenge.ChallengeName().ToLower());
                if (System.IO.File.Exists(filename))
                {
                    string json = System.IO.File.ReadAllText(filename);
                    ChallengesScores challengeScores = Newtonsoft.Json.JsonConvert.DeserializeObject<ChallengesScores>(json);

                    foreach (var entry in challengeScores.ModdedScores)
                        scores.ModdedScores[entry.Key] = entry.Value;

                    foreach (var entry in challengeScores.VanillaScores)
                        scores.VanillaScores[entry.Key] = entry.Value;
                }
            }
            return scores;
        }

        private string FormatEndTag(IChallenge challenge)
        {
            return string.Format(END_TAG_PATTERN, challenge.ChallengeName()).ToLower();
        }

        private string FormatStartTag(IChallenge challenge)
        {
            return string.Format(START_TAG_PATTERN, challenge.ChallengeName()).ToLower();
        }
    }
}

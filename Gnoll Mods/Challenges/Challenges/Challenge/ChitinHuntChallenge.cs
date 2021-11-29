using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Challenge
{
    class ChitinHuntChallenge : AbstractItemBasedChallenge
    {
        // 12 days per season, 4 seasons per year, 3 years per challenge
        private const int TIMELIMIT_DAYS = 12 * 4 * 3;

        private const string ITEM_ID = "BugCiv_RawChitin";
        private const string ITEM_NAME_PART = "chitin";
        private const string ENEMIES_MANTS_PART = "mant";

        public override string ChallengeDescription()
        {
            return "Hasty greetings, Governor! \n\nWe desperately need your help! \nBecause of ... reasons, we have a desperate need for some chitin. Yes, chitin. " +
                "This means we need you to do some hunting. Any type of chitin is welcome, as long as it's raw chitin! " +
                "\nWe depend on you, please do not let us down!";
        }

        public override string ChallengeEndMessage()
        {
            return "Joyful greetings, Governor!\n\n We have received your shipment of your produce. This will definitely help us with future plans. " +
                "Great many thanks to you, the motherland is in your dept!";
        }

        public override string ChallengeName()
        {
            return "ChitinHunt";
        }

        public override string ChallengeObjective()
        {
            return "Collect and store chitin";
        }

        public override string AdditionalRules()
        {
            return "Mants will be force enabled";
        }

        public override string ChallengeTimeframe()
        {
            return "3 years";
        }
        public override void OnNewGameStart(CreateWorldOptions worldOptions)
        {
            var newGameSettings = GnomanEmpire.Instance.GameDefs.NewGameSettings;
            foreach (var enemy in newGameSettings.EnemyRaceOptions)
            {
                if (enemy.Name.ToLower().Contains(ENEMIES_MANTS_PART))
                {
                    foreach (string raceID in enemy.RaceIDs)
                    {
                        worldOptions.DifficultySettings.AllowRace(raceID, true);
                    }
                }
            }
        }

        public override bool IsEndConditionsMet()
        {
            // Check if it's the next day or correct day after sunrise
            return Game.GnomanEmpire.Instance.Region.TotalTime() >= (TIMELIMIT_DAYS + 1) ||
                (Game.GnomanEmpire.Instance.Region.TotalTime() >= TIMELIMIT_DAYS &&
                Game.GnomanEmpire.Instance.Region.Time.Value > Game.GnomanEmpire.Instance.Region.Sunrise());
        }

        protected override Dictionary<string, List<Item>> GetFilteredItems()
        {
            StockManager stockManager = GnomanEmpire.Instance.Fortress.StockManager;

            Dictionary<string, List<Item>> dict = stockManager.ItemsByItemID(ITEM_ID);
            return dict.Where(i => i.Key.ToLower().Contains(ITEM_NAME_PART))
                .ToDictionary(i => i.Key, i => i.Value);
        }
    }
}

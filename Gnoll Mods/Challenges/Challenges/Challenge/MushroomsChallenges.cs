using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;

namespace GnollMods.Challenges.Challenge
{
    class MushroomsChallenge : IChallenge
    {
        // 12 days per season, 4 seasons per year, 3 years per challenge
        private const int TIMELIMIT_DAYS = 12 * 4 * 3;

        // start at this
        private const double SCORE_BASE = 1.0;
        // for each extra type, add this coef for the score calc
        private const double SCORE_INC = 0.25;
        private const string ITEM_ID = "Agriculture_Vegetable";
        private const string ITEM_NAME_PART = "mushroom";

        public string ChallengeDescription()
        {
            return "Hasty greetings, Governor! \n\nWe desperately need your help! \nThere is a big party coming and you are invited! ... Provided you can get us mushrooms, " + 
                "like we need a lot of mushrooms. Our own stocks are empty. Not even enough for Sunday evening tea." +
                "We need your settlement to help us out and grow as much mushroom as you can. \nWe depend on you, please do not let us down!";
        }

        public string ChallengeEndMessage()
        {
            return "Joyful greetings, Governor!\n\n We have received your shipment of the produce. This will be defenitly enough for the party! You are invited, of course!. " +
                "Great many thanks to you, the motherland is in your dept!";
        }

        public string ChallengeName()
        {
            return "Mushrooms";
        }

        public string ChallengeObjective()
        {
            return "Collect and store fruit";
        }

        public string ChallengeTimeframe()
        {
            return "3 years";
        }

        public void OnPreStart()
        {

        }
        public void OnNewGameStart(CreateWorldOptions worldOptions)
        {
        }

        public string CalculateScore()
        {
            StockManager stockManager = GnomanEmpire.Instance.Fortress.StockManager;
            Dictionary<string, List<Item>> dict = stockManager.ItemsByItemID(ITEM_ID);
            double score = 0;
            var idx = 0;
            if (dict != null)
            {
                foreach (var item in dict.OrderByDescending(i => i.Value.Count()))
                {
                    if (item.Key.ToLower().Contains(ITEM_NAME_PART))
                    {
                        score += (SCORE_BASE + SCORE_INC * idx) * item.Value.Count();
                        idx++;
                    }
                }
            }
            return "" + (int)score;
        }

        public bool IsEndConditionsMet()
        {
            // Check if it's the next day or correct day after sunrise
            return Game.GnomanEmpire.Instance.Region.TotalTime() >= (TIMELIMIT_DAYS + 1) ||
                (Game.GnomanEmpire.Instance.Region.TotalTime() >= TIMELIMIT_DAYS &&
                Game.GnomanEmpire.Instance.Region.Time.Value > Game.GnomanEmpire.Instance.Region.Sunrise());
        }

        public string AdditionalRules()
        {
            return "";
        }
    }
}

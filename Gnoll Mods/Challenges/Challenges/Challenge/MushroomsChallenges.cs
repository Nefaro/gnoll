using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Challenge
{
    class MushroomsChallenge : AbstractItemBasedChallenge
    {
        // 12 days per season, 4 seasons per year, 3 years per challenge
        private const int TIMELIMIT_DAYS = 12 * 4 * 3;

        private const string ITEM_ID = "Agriculture_Vegetable";
        private const string ITEM_NAME_PART = "mushroom";

        public override string ChallengeDescription()
        {
            return "Hasty greetings, Governor! \n\nWe desperately need your help! \nThere is a big party coming and you are invited! ... Provided you can get us mushrooms, " + 
                "like we need a lot of mushrooms. Our own stocks are empty. Not even enough for Sunday evening tea." +
                "We need your settlement to help us out and grow as much mushroom as you can. \nWe depend on you, please do not let us down!";
        }

        public override string ChallengeEndMessage()
        {
            return "Joyful greetings, Governor!\n\n We have received your shipment of the produce. This will be defenitly enough for the party! You are invited, of course!. " +
                "Great many thanks to you, the motherland is in your dept!";
        }

        public override string ChallengeName()
        {
            return "Mushrooms";
        }

        public override string ChallengeObjective()
        {
            return "Collect and store fruit";
        }

        public override string ChallengeTimeframe()
        {
            return "3 years";
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

using System.Collections.Generic;
using Game;

namespace GnollMods.Challenges.Challenge
{
    class LumberjackChallenge : AbstractItemBasedChallenge
    {
        private readonly static string NAME = "Lumberjack";

        // 12 days per season, 4 seasons per year, 3 years per challenge
        private const int TIMELIMIT_DAYS = 12 * 4 * 3;

        private const string ITEM_ID = "RawWood";

        public override string ChallengeDescription()
        {
            return "Hasty greetings, Governor! \n\nWe desperately need your help! \nThe weather has put us in a situation, where we are in danger or running out of lumber in near future. " +
                "We need your settlement to help us out and collect as much wood as you can. \nWe depend on you, please do not let us down!";
        }
        public override string ChallengeEndMessage()
        {
            return "Joyful greetings, Governor!\n\n We have received your shipment of wood. This will definitely help us in keeping us warm during the winter. " +
                "Great many thanks to you, the motherland is in your dept!";
        }

        public override string ChallengeName()
        {
            return NAME;
        }

        public override string ChallengeObjective()
        {
            return "Collect and store logs.";
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
            return stockManager.ItemsByItemID(ITEM_ID);
        }
    }
}

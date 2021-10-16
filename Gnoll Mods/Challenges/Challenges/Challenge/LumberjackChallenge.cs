using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using GnollMods.Challenges.Gui;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Challenge
{
    class LumberjackChallenge : IChallenge
    {
        private readonly static string NAME = "Lumberjack";

        private const int TIMELIMIT_DAYS = 3 * 12;
        // start at this
        private const double SCORE_BASE = 1.0;
        // for each extra type, add this coef for the score calc
        private const double SCORE_INC = 0.1;
        private const string ITEM_ID = "RawWood";

        public string ChallengeDescription()
        {
            return "Hasty greetings, Governor! \n\nWe desperately need your help! \nThe weather has put us in a situation, where we are in danger or running out of lumber in near future. " +
                "We need your settlement to help us out and collect as much wood as you can. \nWe depend on you, please do not let us down!";
        }

        public string ChallengeName()
        {
            return NAME;
        }

        public string ChallengeObjective()
        {
            return "Collect and store logs.";
        }

        public string ChallengeTimeframe()
        {
            return "3 years";
        }

        public void OnStart()
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
                    score += (SCORE_BASE + SCORE_INC * idx) * item.Value.Count();
                    idx++;
                }
            }
            return "" + (int)score;
        }

        public bool IsEndConditionsMet()
        {
            // Check the days since beginning
            return Game.GnomanEmpire.Instance.Region.TotalTime() >= TIMELIMIT_DAYS;
        }
    }
}

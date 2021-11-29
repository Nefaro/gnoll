using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Challenge
{
    /**
     * Abstract class for challenges that track item amounts.
     */
    abstract public class AbstractItemBasedChallenge : IChallenge
    {
        // start at this
        private const double SCORE_BASE = 1.0;
        // for each extra type, add this coef for the score calc
        private const double SCORE_INC = 0.25;

        /**
         * The score base, given for single item type.
         * Default = 1;
         */
        protected virtual double ScoreBase { get { return SCORE_BASE; } }
        /**
         * The score increment, given for any extra type present
         * Default = 0.25
         */
        protected virtual double ScoreIncrement { get { return SCORE_INC; } }

        /**
         * Get and filter the items tracked for the given Challenge
         */
        protected abstract Dictionary<string, List<Item>> GetFilteredItems();

        protected virtual double CalculateScoreImpl(Dictionary<string, List<Item>> itemDictionary)
        {
            double score = 0;
            var idx = 0;
            if (itemDictionary != null)
            {
                foreach (var item in itemDictionary.OrderByDescending(i => i.Value.Count()))
                {
                    score += (ScoreBase+ ScoreIncrement* idx) * item.Value.Count();
                    idx++;
                }
            }
            return score;
        }

        public virtual string CalculateScore()
        {
            Dictionary<string, List<Item>> dict = GetFilteredItems();
            return "" + (int)CalculateScoreImpl(dict);
        }

        protected virtual List<StatsItem> CalculateStatsImpl(Dictionary<string, List<Item>> itemDictionary)
        {
            List<StatsItem> result = new List<StatsItem>();
            GameDefs gameDefs = GnomanEmpire.Instance.GameDefs;
            var idx = 0;
            if (itemDictionary != null)
            {
                foreach (var item in itemDictionary.OrderByDescending(i => i.Value.Count()))
                {
                    result.Add(new StatsItem(gameDefs.MaterialDef(item.Key).Name, item.Value.Count(), ScoreBase+ ScoreIncrement* idx));
                    idx++;
                }
            }
            return result;
        }

        public virtual List<StatsItem> CalculateStats()
        {
            return CalculateStatsImpl(GetFilteredItems());
        }

        public virtual string AdditionalRules()
        {
            return "";
        }

        public abstract string ChallengeDescription();

        public abstract string ChallengeEndMessage();

        public abstract string ChallengeName();

        public abstract string ChallengeObjective();

        public abstract string ChallengeTimeframe();

        public abstract bool IsEndConditionsMet();

        public virtual void OnNewGameStart(CreateWorldOptions worldOptions)
        {

        }

        public virtual void OnPreStart()
        {

        }

    }
}

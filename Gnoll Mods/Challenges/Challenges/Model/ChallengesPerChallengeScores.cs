using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollMods.Challenges.Model
{
    public class ChallengesPerChallengeScores
    {
        private readonly int TOP_LIMIT = 10;

        // map.size -> scores
        public Dictionary<String, List<ChallengesScoreRecord>> Scores { get; set; }

        public ChallengesPerChallengeScores()
        {
            Scores = new Dictionary<string, List<ChallengesScoreRecord>>();
        }

        public void KeepTop10()
        {
            var result = new Dictionary<string, List<ChallengesScoreRecord>>();
            foreach(var scoresItem in Scores)
            {
                if ( scoresItem.Value.Count > TOP_LIMIT)
                    result.Add(scoresItem.Key, new List<ChallengesScoreRecord>(scoresItem.Value.OrderByDescending(sc => sc.Score)).GetRange(0, TOP_LIMIT));
                else
                    result.Add(scoresItem.Key, scoresItem.Value);
            }
            this.Scores = result;
        }
    }
}

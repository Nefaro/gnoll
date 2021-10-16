using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollMods.Challenges.Model
{
    public class ChallengesScores
    {
        // challenge.Name => challenge.ScoreRecords
        public Dictionary<String, ChallengesPerChallengeScores> ModdedScores { get; set; }
        public Dictionary<String, ChallengesPerChallengeScores> VanillaScores { get; set; }

        public ChallengesScores()
        {
            ModdedScores = new Dictionary<string, ChallengesPerChallengeScores>();

            VanillaScores = new Dictionary<string, ChallengesPerChallengeScores>();
        }

        public void KeepTop10()
        {
            foreach(var item in ModdedScores)
            {
                item.Value.KeepTop10();
            }

            foreach (var item in VanillaScores)
            {
                item.Value.KeepTop10();
            }
        }
    }
}

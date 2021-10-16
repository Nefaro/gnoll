using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollMods.Challenges.Model
{
    public class ChallengesScoreRecord
    {
        public string KingdomName {  get; set; }
        public string KingdomSize {  get; set; }
        public string Score { get; set;  }
        public string KingdomSeed { get; set; }
        public string BaseMod { get; set; }
        public List<string> ModList { get; set;  }
        public string Difficulty {  get; set; }
        public string Date { get; set; }
        public string MetalAmount { get; set; }
        public string MetalDepth { get; set; }
        public List<string> EnemyList { get; set; }

        public string EnemyStrength { get; set; }
        public Boolean EnemyStrengthGrowth { get; set; }
        public string EnemyAttackRate { get; set; }
        public string EnemyAttackSize { get; set; }

    }
}

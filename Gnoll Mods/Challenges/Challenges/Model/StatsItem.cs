using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollMods.Challenges.Model
{
    public class StatsItem
    {
        public string ItemName { get; private set;  }
        public int ItemCount { get; private set; }
        public double ScoreMultiplyer { get; private set;  }

        public StatsItem(string itemName, int itemCount, double scoreMultiplyer)
        {
            ItemName = itemName;
            ItemCount = itemCount;
            ScoreMultiplyer = scoreMultiplyer;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GnollModLoader;

namespace GnollMods.Challenges.Challenge
{

    public interface IChallenge
    {
        string ChallengeName();

        string ChallengeDescription();

        string ChallengeObjective();

        string ChallengeTimeframe();

        void OnStart();

        string CalculateScore();
        bool IsEndConditionsMet();
    }

}

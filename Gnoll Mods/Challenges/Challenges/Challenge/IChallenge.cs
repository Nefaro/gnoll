using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;

namespace GnollMods.Challenges.Challenge
{

    public interface IChallenge
    {
        string ChallengeName();

        string ChallengeDescription();

        string ChallengeObjective();

        string ChallengeTimeframe();

        string ChallengeEndMessage();

        void OnPreStart();
        void OnNewGameStart(CreateWorldOptions worldOptions);

        string CalculateScore();
        bool IsEndConditionsMet();

        string AdditionalRules();
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;
using GnollMods.Challenges.Model;

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
        List<StatsItem> CalculateStats();

        bool IsEndConditionsMet();

        string AdditionalRules();

    }

}

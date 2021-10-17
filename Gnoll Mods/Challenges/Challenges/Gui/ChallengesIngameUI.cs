using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges.Gui
{
    public class ChallengesIngameUI : TabbedWindow
    {
        public ChallengesIngameUI(Manager manager, ChallengesScoreRecord record, IChallenge challenge) : base(manager)
        {
			this.Text = "Challenges";
			this.Width = 510;
			this.Height = 290;
			this.Resizable = false;
			this.Center();
			base.AddPage("Score", new ChallengesIngameScoreUI(manager, record, challenge));
		}
	}
}

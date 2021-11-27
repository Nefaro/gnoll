using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using GnollMods.Challenges.Challenge;
using GnollMods.Challenges.Gui;
using GnollMods.Challenges.Model;

namespace GnollMods.Challenges 
{
    internal class ModMain : IGnollMod
    {
        private static ModMain _instance;
        public static ModMain Instance { get { return _instance; } }
        public string Name { get { return "Challenges"; } }
        public string Description { get { return "Introduces 'Challenges' game mode: a game with objectives."; } }
        public string BuiltWithLoaderVersion { get { return "G1.8"; } }
        public int RequireMinPatchVersion { get { return 8; } }

        private HookManager _hookManager;

        private ChallengesManager _challengesManager;
        public ChallengesManager ChallengeManager {  get { return _challengesManager; } }

        public ModMain()
        {
            _instance = this;
            _challengesManager = new ChallengesManager();
        }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.MainMenuGuiInit += HookManager_MainMenuGuiInit;
            hookManager.InGameHUDInit += _challengesManager.HookManager_InGameHUDInit;
            hookManager.BeforeStartNewGame += _challengesManager.HookManager_BeforeStartNewGame;
            this._hookManager = hookManager;
        }

        private void HookManager_MainMenuGuiInit(MainMenuWindow window, Manager manager)
        {
            Game.GUI.Controls.Button modButton = window.method_39(manager, "Gnoll Challenges");
            modButton.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                _challengesManager.Reset();
                var scores = _challengesManager.LoadScore();
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PushWindow(new ChallengesMenu(_hookManager, Game.GnomanEmpire.Instance.GuiManager.Manager, scores));
            };
            window.panel_0.Add(modButton);
        }
    }
}

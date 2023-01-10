using System.Collections.Generic;

using GnollModLoader;
using Game;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollMods.IdleTrackerMod
{
    public class ModMain : IGnollMod
    {
        public static ModMain instance;

        private const float _updatePeriod = 1.0f;
        private GClass0 _hudPanel;
        private IdleTrackerPanel _idleTrackerPanel;
        private float _timeSinceLastUpdate = 0.0f;

        public string Name { get { return "IdleTrackerMod"; } }

        public string Description { get { return "Tracks idling workers and makes them visible"; } }

        public string BuiltWithLoaderVersion { get { return "G1.13"; } }

        public int RequireMinPatchVersion { get { return 13; } }

        public ModMain()
        {
            instance = this;
        }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
            hookManager.UpdateInGame += HookManager_UpdateInGame;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameHUDInit -= HookManager_InGameHUDInit;
            hookManager.UpdateInGame -= HookManager_UpdateInGame;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        private void HookManager_UpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
            _timeSinceLastUpdate += gameTimeDelta;

            while (_timeSinceLastUpdate > _updatePeriod)
            {
                _timeSinceLastUpdate -= _updatePeriod;

                Update();
            }
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            this._hudPanel = inGameHUD.gclass0_0;
            this._idleTrackerPanel = new IdleTrackerPanel(manager);
            _idleTrackerPanel.Visible = false;
            _hudPanel.Add(_idleTrackerPanel);
        }

        private void Update()
        {
            Faction playerFaction = GnomanEmpire.Instance.World.AIDirector.PlayerFaction;

            int numIdle = 0;

            _idleTrackerPanel.ClearContents();

            foreach (KeyValuePair<uint, Character> gnome in playerFaction.Members)
            {
                //if (gnome.Value.Job == null && !gnome.Value.Body.IsSleeping)
                if (gnome.Value.StatusText() == "Idle")
                {
                    numIdle++;

                    var button = _idleTrackerPanel.AddButton(gnome.Value.NameAndTitle());

                    button.ToolTip.Text = gnome.Value.FormattedTopSkills(5);
                    button.Click += (sender, e) =>
                    {
                        GnomanEmpire.Instance.Camera.MoveTo(gnome.Value.Position, true, true);
                    };
                }
            }

            if (numIdle == 0)
            {
                _idleTrackerPanel.Visible = false;
                return;
            }

            _idleTrackerPanel.AddLabel(numIdle + " idle gnomes");

            _idleTrackerPanel.SetPosition(
                _hudPanel.Width - _idleTrackerPanel.Width - _idleTrackerPanel.margins_0.Right,
                _hudPanel.Height - _idleTrackerPanel.Height - _idleTrackerPanel.margins_0.Bottom);

            _idleTrackerPanel.Visible = true;
        }

    }
}

using System.Collections.Generic;

using ModLoader;
using Game;
using Game.GUI;

namespace Mod
{
    public class ModMain : IMod
    {
        public static ModMain instance;

        private const string name = "IdleTrackerMod";
        private const float updatePeriod = 1.0f;
        private GClass0 hudPanel;
        private IdleTrackerPanel idleTrackerPanel;
        private float timeSinceLastUpdate = 0.0f;

        public ModMain()
        {
            instance = this;
        }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
            hookManager.UpdateInGame += HookManager_UpdateInGame;
        }

        private void HookManager_UpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
            timeSinceLastUpdate += gameTimeDelta;

            while (timeSinceLastUpdate > updatePeriod)
            {
                timeSinceLastUpdate -= updatePeriod;

                Update();
            }
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            this.hudPanel = inGameHUD.gclass0_0;

            this.idleTrackerPanel = new IdleTrackerPanel(manager);
            idleTrackerPanel.Visible = false;
            hudPanel.Add(idleTrackerPanel);
        }

        private void Update()
        {
            Faction playerFaction = GnomanEmpire.Instance.World.AIDirector.PlayerFaction;

            int numIdle = 0;

            idleTrackerPanel.ClearContents();

            foreach (KeyValuePair<uint, Character> gnome in playerFaction.Members)
            {
                //if (gnome.Value.Job == null && !gnome.Value.Body.IsSleeping)
                if (gnome.Value.StatusText() == "Idle")
                {
                    numIdle++;

                    var button = idleTrackerPanel.AddButton(gnome.Value.NameAndTitle());

                    button.ToolTip.Text = gnome.Value.FormattedTopSkills(5);
                    button.Click += (sender, e) => {
                        GnomanEmpire.Instance.Camera.MoveTo(gnome.Value.Position, true, true);
                    };
                }
            }

            if (numIdle == 0)
            {
                idleTrackerPanel.Visible = false;
                return;
            }

            idleTrackerPanel.AddLabel(numIdle + " idle gnomes");

            idleTrackerPanel.SetPosition(
                hudPanel.Width - idleTrackerPanel.Width - idleTrackerPanel.margins_0.Right,
                hudPanel.Height - idleTrackerPanel.Height - idleTrackerPanel.margins_0.Bottom);

            idleTrackerPanel.Visible = true;
        }
    }
}

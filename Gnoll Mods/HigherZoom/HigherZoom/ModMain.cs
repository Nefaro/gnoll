using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;

namespace GnollMods.HigherZoom
{
    public class ModMain : IGnollMod
    {
        public string Name { get { return "HigherZoom"; } }

        public string Description { get { return "Provides 2 more levels for zooming out."; } }

        public string BuiltWithLoaderVersion { get { return "G1.11"; } }

        public int RequireMinPatchVersion { get { return 11; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.AfterGameLoaded += HookManager_AfterGameLoaded;
            hookManager.BeforeInGameHudInit += HookManager_BeforeInGameHudInit;
            hookManager.BeforeGameSaved += HookManager_BeforeGameSaved;
        }

        private void HookManager_BeforeGameSaved()
        {
            // We need to defuse the zoome level
            // Just in case someone loads the game without the given mod

            // Set it to a safe level
            GnomanEmpire.Instance.Camera.int_0 = 1;
        }

        private void HookManager_BeforeInGameHudInit()
        {
            EnhanceZoomLevel();
        }

        private void HookManager_AfterGameLoaded()
        {
            EnhanceZoomLevel();
        }

        private void EnhanceZoomLevel()
        {
            // Reset the camera 
            GnomanEmpire.Instance.Camera.float_1 = new float[]
            {
                0.25f,
                0.5f,
                1f,
                2f,
                4f
            };
            GnomanEmpire.Instance.Camera.int_0 = 3;
        }
    }
}

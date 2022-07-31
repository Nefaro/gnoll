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

        public string Description { get { return "Provides 3 more levels for zooming out."; } }

        public string BuiltWithLoaderVersion { get { return "G1.12"; } }

        public int RequireMinPatchVersion { get { return 12; } }

        private int _tempCameraSelection;
        private float _tempCameraZoom;

        public void OnLoad(HookManager hookManager)
        {
            hookManager.AfterGameLoaded += HookManager_AfterGameLoaded;
            hookManager.BeforeInGameHudInit += HookManager_BeforeInGameHudInit;
            hookManager.BeforeGameSaved += HookManager_BeforeGameSaved;
            hookManager.AfterGameSaved += HookManager_AfterGameSaved;
        }

        private void HookManager_BeforeGameSaved()
        {
            // We need to defuse the zoome level
            // Just in case someone loads the game without the given mod

            // Temp save the camera zoom;
            _tempCameraSelection = GnomanEmpire.Instance.Camera.int_0;
            _tempCameraZoom = GnomanEmpire.Instance.Camera.float_0;

            // Set it to a safe level
            if ( GnomanEmpire.Instance.Camera.int_0 > 2 || GnomanEmpire.Instance.Camera.int_0 == 0)
            {
                GnomanEmpire.Instance.Camera.int_0 = 1;
                GnomanEmpire.Instance.Camera.float_0 = 2f;
            }
        } 
        private void HookManager_AfterGameSaved()
        {
            // Restore the camera to the higher zoom level
            GnomanEmpire.Instance.Camera.int_0 = _tempCameraSelection;
            GnomanEmpire.Instance.Camera.float_0 = _tempCameraZoom;
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
                0.8f,
                1f,
                2f,
                4f
            };
            GnomanEmpire.Instance.Camera.int_0 = 4;
        }
    }
}

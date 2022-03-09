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
        }
        private void HookManager_AfterGameLoaded()
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

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

        public string BuiltWithLoaderVersion { get { return "G1.10"; } }

        public int RequireMinPatchVersion { get { return 10; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.BeforeInGameHudInit += HookManager_BeforeInGameHudInit;
        }
        private void HookManager_BeforeInGameHudInit()
        {
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

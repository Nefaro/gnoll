using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GnollModLoader;

namespace RaidingParty
{
    internal class ModMain : IGnollMod, IHasLuaScripts
    {
        public string Name { get { return "RaidingParty"; } }
        public string Description { get { return "A mod that adds raiding friendly or enemy kingdoms"; } }
        public string BuiltWithLoaderVersion { get { return "1.14"; } }
        public int RequireMinPatchVersion { get { return 14; } }

        public bool IsDefaultEnabled()
        {
            return false;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        public void OnDisable(HookManager hookManager)
        {

        }

        public void OnEnable(HookManager hookManager)
        {

        }
    }
}

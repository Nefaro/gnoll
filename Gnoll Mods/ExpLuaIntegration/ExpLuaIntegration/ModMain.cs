using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GnollModLoader;

namespace ExpLuaIntegration
{
    internal class ModMain : IGnollMod, IHasLuaScripts
    {
        public string Name { get { return "__ExpLuaIntegration"; } }
        public string Description { get { return "This is an experimental mod, that shows how to use the experimental Lua integration."; } }
        public string BuiltWithLoaderVersion { get { return "G1.14"; } }
        public int RequireMinPatchVersion { get { return 14; } }

        public bool IsDefaultEnabled()
        {
            return false;
        }

        public bool NeedsRestartOnToggle()
        {
            return true;
        }

        public void OnDisable(HookManager hookManager)
        {
            
        }

        public void OnEnable(HookManager hookManager)
        {
            
        }
    }
}

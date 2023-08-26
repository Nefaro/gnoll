using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GnollModLoader;

namespace LuaSupport
{
    public class ModMain : IGnollMod, IHasLuaScripts
    {
        public string Name => "LuaSupport";
        public string Description => "A 'toggle' mod. Enable this mod to enable Lua integration";
        public string BuiltWithLoaderVersion => "1.14";
        public int RequireMinPatchVersion => 14;

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
            // Nothing to do
        }

        public void OnEnable(HookManager hookManager)
        {
            // Nothing to do
        }
    }
}

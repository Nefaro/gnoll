using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class LiquidSettingsProxy
    {
        private LiquidSettings _target;

        [MoonSharpHidden]
        public LiquidSettingsProxy(LiquidSettings target)
        {
            this._target = target;
        }

        public Dictionary<string, string> MaterialIDToLiquidIDs => _target.MaterialIDToLiquidIDs;
    }
}

using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class GolemSettingsProxy
    { 
        private GolemSettings _target;

        [MoonSharpHidden]
        public GolemSettingsProxy(GolemSettings target)
        {
            this._target = target;
        }

        public string CoreItemID { get => _target.CoreItemID; set => _target.CoreItemID = value; }
        public List<GolemSpawnDef> GolemSpawnDefs => _target.GolemSpawnDefs;
    }
}

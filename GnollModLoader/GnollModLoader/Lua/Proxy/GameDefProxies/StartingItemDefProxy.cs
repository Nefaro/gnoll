using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class StartingItemDefProxy
    {
        private StartingItemDef _target;

        [MoonSharpHidden]
        public StartingItemDefProxy(StartingItemDef target)
        {
            this._target = target;
        }

        public List<StartingItem> IDs => _target.IDs;
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
    }
}

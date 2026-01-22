using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class MilitaryProxy
    {
        private readonly Military _target;

        [MoonSharpHidden]
        public MilitaryProxy(Military target)
        {
            this._target = target;
        }

        public List<Squad> Squads => _target.Squads;
        
    }
}

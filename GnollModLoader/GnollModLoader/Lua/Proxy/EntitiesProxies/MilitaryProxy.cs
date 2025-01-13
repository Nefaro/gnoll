using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiesProxies
{
    internal class MilitaryProxy
    {
        private Military _target;

        [MoonSharpHidden]
        public MilitaryProxy(Military target)
        {
            this._target = target;
        }

        public List<Squad> Squads => _target.Squads;
        
    }
}
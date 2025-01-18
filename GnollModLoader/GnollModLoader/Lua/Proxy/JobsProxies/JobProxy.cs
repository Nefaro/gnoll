using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class JobProxy
    {
        private readonly Job _target;

        [MoonSharpHidden]
        public JobProxy(Job target)
        {
            this._target = target;
        }
    }
}

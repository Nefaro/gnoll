using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class EnvoyProxy
    {
        private readonly Envoy _target;

        [MoonSharpHidden]
        public EnvoyProxy(Envoy target)
        {
            this._target = target;
        }

    }
}

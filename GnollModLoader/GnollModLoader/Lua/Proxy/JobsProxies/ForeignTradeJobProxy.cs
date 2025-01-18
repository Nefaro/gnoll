using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class ForeignTradeJobProxy
    {
        private readonly ForeignTradeJob _target;

        [MoonSharpHidden]
        public ForeignTradeJobProxy(ForeignTradeJob target)
        {
            this._target = target;
        }


    }
}

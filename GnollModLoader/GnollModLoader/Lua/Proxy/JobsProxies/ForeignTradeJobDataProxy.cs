using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.JobsProxies
{
    internal class ForeignTradeJobDataProxy
    {
        private readonly ForeignTradeJobData _target;

        [MoonSharpHidden]
        public ForeignTradeJobDataProxy(ForeignTradeJobData target)
        {
            this._target = target;
        }
    }
}

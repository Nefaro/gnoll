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

        public Envoy Envoy => _target.envoy_0;

        public string ClassName => typeof(ForeignTradeJob).Name;

        public Faction? GetTargetFaction()
        {
            var data = _target.Data as ForeignTradeJobData;
            if (data == null)
            {
                return null;
            }
            return GnomanEmpire.Instance.World.AIDirector.Factions[data.FactionID];
        }
    }
}

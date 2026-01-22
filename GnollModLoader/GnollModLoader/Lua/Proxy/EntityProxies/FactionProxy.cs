using System.Collections.Generic;
using Game;
using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class FactionProxy
    {
        private readonly Faction _target;

        [MoonSharpHidden]
        public FactionProxy(Faction target)
        {
            this._target = target;
        }

        public uint ID => _target.UInt32_0;
        public string Name => _target.Name;
        public FactionDef FactionDef => _target.FactionDef;
        public FactionType FactionType => _target.FactionDef.Type;
        public MerchantGoods FactionGoods => _target.FactionGoods;
        public Dictionary<uint, Character> Members => _target.Members;
        public string SubType => _target.SubType();
        public Character PlayerEnvoy { get => _target.PlayerEnvoy; set => _target.PlayerEnvoy = value; }

        public bool IsHostile(uint factionID) => _target.IsHostile(factionID);

        public bool IsHostileToPlayer() 
        {
            return _target.IsHostile(Faction.PlayerFactionID);
        }

        // We need to do some conversion for Lua
        // "Vector3.One" is a hard sell ...
        public Vector3? FindRegionExitPosition(Character chr) {
            var position = _target.FindRegionExitPosition(chr);
            if (position == -Vector3.One) 
                return null;

            return position;
        }

        public Envoy? CreateMerchantEnvoyForSquadJob(Job job, Squad squad)
        {
            var tradeJob = job as ForeignTradeJob;

            if (tradeJob == null) {
                Logger.Error($"Job {job.GetType()} does not support Envoys");
                return null;
            }

            var envoy = new Envoy(EnvoyType.Merchant, GnomanEmpire.Instance.Region.TotalTime() + _target.Distance, 0f, EnvoyState.Departing);
            // Add all squad members as envoys
            foreach(Character member in squad.Members) 
            {
                envoy.AddMember(member.UInt32_0);
            }
            tradeJob.envoy_0 = envoy;
            return envoy;
        }

    }
}

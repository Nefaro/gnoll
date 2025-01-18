using System;
using System.IO;
using Game;
using GameLibrary;
using Microsoft.Xna.Framework;

namespace GnollModLoader.Lua
{
    internal class JobBoardGlobalTable
    {
        public readonly static string GLOBAL_TABLE_LUA_NAME = "_JOBS";
        public Job? CreateCustomRaidingJobForSquad(Faction faction, Squad squad)
        {
            if (faction == null)
            {
                Logger.Error($"Cannot create raiding job, faction is null");
                return null;
            }

            if (squad == null)
            {
                Logger.Error($"Cannot create raiding job, squad is null");
                return null;
            }

            var leader = squad.Members[0];
            var egress = faction.FindRegionExitPosition(leader);
            if (egress == -Vector3.One)
            {
                Logger.Warn("Squad cannot find exit position");
                GnomanEmpire.Instance.World.NotificationManager.AddNotification("Squad cannot find a way out to offworld");
                return null;
            }

            var job = new CustomRaidJob(egress, new ForeignTradeJobData(faction.UInt32_0));
            var arrival = GnomanEmpire.Instance.Region.TotalTime() + 0.007f;
            // var arrival = GnomanEmpire.Instance.Region.TotalTime() + faction.Distance
            leader.TakeJob(job);
            faction.PlayerEnvoy = leader;
            leader.TravelOffMap();
            job.envoy_0 = new Envoy(EnvoyType.Raid, arrival, 0f, EnvoyState.Departing);
            job.envoy_0.AddMember(leader.UInt32_0);

            return job;
        }
        public void AddSpawningItems(Job job, string materialID, string itemID, int value = 1, int quantity = 1)
        {
            Logger.Log($"Adding item {materialID} {itemID} with value {value} and quantity {quantity}");
            this.AddSpawningGoods(job, new AvailableGood(itemID, materialID, ItemQuality.Average, quantity, (uint)value), quantity);
         }

        public void AddSpawningGoods(Job job, AvailableGood goods, int quantityOverride = 1)
        {
            if (job == null) 
            {
                Logger.Error($"Cannot add goods, job is null");
                return;
            }
            try
            {
                ForeignTradeJob raidJob = job as ForeignTradeJob;
                if (raidJob != null)
                {
                    var factionId = raidJob.method_0().FactionID;
                    Faction faction = GnomanEmpire.Instance.World.AIDirector.Faction(factionId);

                    if (quantityOverride == 0)
                        quantityOverride = 1;

                    goods.mQuantity = quantityOverride;
                    faction.FactionGoods.OfferedGoods.Add(goods);
                }
                else
                {
                    Logger.Warn($"Job {job.GetType()} does not support adding spawning goods");
                }
            }
            catch(Exception e)
            {
                Logger.Error("Couldn't add");
                Logger.Error("{0}", e);
            }
        }

    }

    internal class CustomRaidJob : ForeignTradeJob
    {
        public CustomRaidJob(BinaryReader reader) : base(reader)
        {
        }

        public CustomRaidJob(Vector3 position, ForeignTradeJobData data) : base(position, data)
        {
        }
        private bool returning = false;
        public override bool Progress(Character character, float dt)
        {
            //Logger.Log($"Job status: {this.envoy_0.State} ");

            //float num = GnomanEmpire.Instance.Region.TotalTime();
            //Logger.Log($"Envoy num: {num} > float {this.envoy_0.float_0} ");

            switch (this.envoy_0.State)
            {
                case EnvoyState.OnLocation:
                    this.mDifficulty = 0.007f;
                    break;
            }

            if (!returning && this.envoy_0.State == EnvoyState.Returning)
            {
                envoy_0.float_0 = 0.007F + GnomanEmpire.Instance.Region.TotalTime();
                returning = true;
            }
            return base.Progress(character, dt);
        }
    }

}

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
            try
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

                var egress = faction.FindRegionExitPosition(squad.Members[0]);
                if (egress == -Vector3.One)
                {
                    Logger.Warn("Squad cannot find exit position");
                    GnomanEmpire.Instance.World.NotificationManager.AddNotification("Squad cannot find a way out to offworld");
                    return null;
                }

                ForeignTradeJob leaderJob = null;
                foreach (Character member in squad.Members)
                {
                    if ( member == null )
                    {
                        // Not all squad spots are taken
                        continue;
                    }

                    // each member gets it's own job object
                    var job = new ForeignTradeJob(egress, new ForeignTradeJobData(faction.UInt32_0));
                    var arrival = GnomanEmpire.Instance.Region.TotalTime() + faction.Distance;
                    member.TakeJob(job);
                    // A bit cruel but ...
                    member.Body.WakeUp();

                    faction.PlayerEnvoy = member;
                    member.TravelOffMap();
                    job.envoy_0 = new Envoy(EnvoyType.Raid, arrival, 0f, EnvoyState.Departing);
                    job.envoy_0.AddMember(member.UInt32_0);
                    if (leaderJob == null)
                    {
                        // First member is leader
                        leaderJob = job;
                    }
                }
                // We are intereste only in the leader job, since this is the one we will operate with
                return leaderJob;
            }
            catch (Exception ex)
            {
                Logger.Error("Cannot create Job posting: " + ex.ToString());
            }
            return null;
        }

        public void AddSpawningItems(Job job, string materialID, string itemID, int value = 1, int quantity = 1)
        {
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
}

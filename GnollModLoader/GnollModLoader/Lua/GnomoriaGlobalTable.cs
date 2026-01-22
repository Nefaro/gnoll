using System.Collections.Generic;
using Game;
using GameLibrary;

namespace GnollModLoader.Lua
{
    /**
     * Represents the global Gnomoria table in Lua context
     */
    internal class GnomoriaGlobalTable
    {
        public readonly static string GLOBAL_TABLE_LUA_NAME = "_GNOMORIA";

        // Get the game definitions
        public GameDefs GetGameDefs()
        {
            return GnomanEmpire.Instance.GameDefs;
        }
        public GameEntityManager GetEntityManager()
        {
            return GnomanEmpire.Instance.EntityManager;
        }

        // Expose Game built-in random
        public int RandomInt(int max)
        {
            return GnomanEmpire.Instance.Rand.Next(max);
        }

        public float RandomFloat(float max)
        {
            return (float)GnomanEmpire.Instance.Rand.NextDouble() * max;
        }
        public float RandomIntInRange(float max, float min)
        {
            return GnomanEmpire.Instance.RandomInRange(max, min);
        }

        public bool RandomBoolean()
        {
            return (GnomanEmpire.Instance.Rand.Next(2) == 1);
        }

        public Military GetMilitary()
        {
            return GnomanEmpire.Instance.Fortress.Military;
        }

        public List<Faction> GetDiplomaticFactions()
        {
            var factions = new List<Faction>();

            DifficultySetting difficultySettings = GnomanEmpire.Instance.World.DifficultySettings;
            foreach (KeyValuePair<uint, Faction> keyValuePair in GnomanEmpire.Instance.World.AIDirector.Factions)
            {
                Faction faction = keyValuePair.Value;
                // Same condition as on the Diplomacy UI 
                if ((faction.FactionDef.Type == FactionType.FriendlyCiv || faction.FactionDef.Type == FactionType.EnemyCiv) && 
                    (faction.FactionDef.Type != FactionType.EnemyCiv 
                        || (difficultySettings.Difficulty != GameMode.Peaceful && faction.IsAnyRaceAllowed() && GnomanEmpire.Instance.Region.Day > 12U)))
                {
                    factions.Add(faction);
                }
            }
            return factions;
        }

        public Faction GetPlayerFaction() => GnomanEmpire.Instance.World.AIDirector.PlayerFaction;

        public bool IsRaceAllowedBySettings(string raceID)
        {
            var settings = GnomanEmpire.Instance.World.DifficultySettings;
            return settings.IsRaceAllowed(raceID);
        }

        public void Notify(string message)
        {
            GnomanEmpire.Instance.World.NotificationManager.AddNotification(message);
        }

        // Get the season index: [0..3] = [spring, summer, fall, winter]
        public Season? GetCurrentSeason()
        {
            if ( GnomanEmpire.Instance.World != null )
            { 
                return GnomanEmpire.Instance.Region.Season();
            }
            return null;
        }

        // Get current day number
        public uint? GetCurrentDay()
        {
            if (GnomanEmpire.Instance.World != null)
            {
                return GnomanEmpire.Instance.Region.Day;
            }
            return null;
        }
        public float? GetOutsideLightLevel()
        {
            if (GnomanEmpire.Instance.World != null)
            {
                return GnomanEmpire.Instance.Region.OutsideLight;
            }
            return -1;
        }
    }
}

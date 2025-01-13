using Game;

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

        public Military GetMilitary()
        {
            return GnomanEmpire.Instance.Fortress.Military;
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
    }
}

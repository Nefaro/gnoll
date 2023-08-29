using Game;

namespace GnollModLoader.Lua
{
    /**
     * Represents the global Gnomoria table in Lua context
     */
    internal class GnomoriaGlobalTable
    {
        public readonly static string GNOMORIA_GLOBAL_TABLE_NAME = "_GNOMORIA";

        // Get the game definitions
        public GameDefs getGameDefs()
        {
            return GnomanEmpire.Instance.GameDefs;
        }

        // Get the season index: [0..3] = [spring, summer, fall, winter]
        public Season? getCurrentSeason()
        {
            if ( GnomanEmpire.Instance.World != null ) 
            { 
                return GnomanEmpire.Instance.Region.Season();
            }
            return null;
        }

        // Get current day number
        public uint? getCurrentDay()
        {
            if (GnomanEmpire.Instance.World != null)
            {
                return GnomanEmpire.Instance.Region.Day;
            }
            return null;
        }
    }
}

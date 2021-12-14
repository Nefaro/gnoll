using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;

namespace GnollMods.FixDisableNewGameModdedMobs
{
    internal class ModMain : IGnollMod
    {
        public string Name { get { return "FixDisableNewGameModdedMobs"; } }
        public string Description { get { return "Fixes disabling modded enemies on new game start"; } }
        public string BuiltWithLoaderVersion { get { return "G1.8"; } }
        public int RequireMinPatchVersion { get { return 8; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.BeforeStartNewGameAfterReadDefs += HookManager_BeforeStartNewGame;
        }

        private void HookManager_BeforeStartNewGame(Game.CreateWorldOptions worldOptions)
        {
            // Blacklist of mobs, ie mobs, that should NOT spawn
            var blacklist = worldOptions.DifficultySettings.hashSet_0;
            // All possible enemies (vanilla + modded)
            var raceOptions = GnomanEmpire.Instance.GameDefs.NewGameSettings.EnemyRaceOptions;
            // map raceID => group.Name
            var raceIDtoGroupNameMap = new Dictionary<String, String>();
            foreach (var group in raceOptions)
            {
                foreach (var raceID in group.RaceIDs)
                {
                    raceIDtoGroupNameMap.Add(raceID, group.Name);
                }
            }
            var blacklistGroups = new HashSet<String>();
            // find the group names of blacklisted mobs 
            foreach (var raceID in blacklist)
            {
                string group;
                if (raceIDtoGroupNameMap.TryGetValue(raceID, out group))
                {
                    blacklistGroups.Add(group);
                }
            }
            // increment over all existing groups and blacklist the races
            // we found in the previous filter
            foreach (var group in raceOptions)
            {
                if ( blacklistGroups.Contains(group.Name) )
                {
                    foreach(var raceID in group.RaceIDs)
                    {
                        // existing once are not toched
                        // new ones are added
                        worldOptions.DifficultySettings.hashSet_0.Add(raceID);
                    }
                }
            }
        }
    }
}

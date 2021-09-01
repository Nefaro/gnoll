using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader;

namespace GnollMods.VeinMiner
{
    class ModMain : IGnollMod
    {
        public static ModMain instance;
        public string Name { get { return  "VeinMiner"; } }
        public string Description { get { return  "Allows the miner to mine out a discovered vein of ore or gems."; } }
        public string BuiltWithLoaderVersion { get { return  "G1.2"; } }

        public ModMain()
        {
            instance = this;
        }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.OnJobComplete += HookManager_OnJobComplete;
        }

        private void HookManager_OnJobComplete(Game.Job job, Game.Character character)
        {
            if (job.GetType() == typeof(Game.MineJob))
            {
                Map map = GnomanEmpire.Instance.Map;
                // check around the current position and register any cell that has minerals in the  wall
                this.RegisterMiningJob(map, job.Position.X + 1, job.Position.Y, job.Position.Z);
                this.RegisterMiningJob(map, job.Position.X - 1, job.Position.Y, job.Position.Z);
                this.RegisterMiningJob(map, job.Position.X, job.Position.Y + 1, job.Position.Z);
                this.RegisterMiningJob(map, job.Position.X, job.Position.Y - 1, job.Position.Z);
            }
        }

        private void RegisterMiningJob(Map map, float col, float row, float level)
        {
            if (!map.InBounds((int)level, (int)row, (int)col))
                return;

            MapCell cell = map.GetCell((int)level, (int)row, (int)col);
            if (cell.HasEmbeddedWall())
            {
                Mineral mineral = cell.EmbeddedWall as Mineral;
                if (mineral != null)
                {
                    // This wall segment has minerals/gems
                    GnomanEmpire.Instance.Fortress.JobBoard.AddJob(new MineJob(new Microsoft.Xna.Framework.Vector3(col, row, level)));
                }
            }
        }
    }
}

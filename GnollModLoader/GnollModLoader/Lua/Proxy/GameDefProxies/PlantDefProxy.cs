using MoonSharp.Interpreter;
using static GameLibrary.PlantDef;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class PlantDefProxy
    {
        private GameLibrary.PlantDef _target;

        [MoonSharpHidden]
        public PlantDefProxy(GameLibrary.PlantDef target)
        {
            this._target = target;
        }

        public float FruitGrowTimeMax { get => _target.FruitGrowTimeMax; set => _target.FruitGrowTimeMax = value; }
        public float FruitGrowTimeMin { get => _target.FruitGrowTimeMin; set => _target.FruitGrowTimeMin = value; }
        public string FruitID { get => _target.FruitID; set => _target.FruitID = value; }
        public float GrowTimeMax { get => _target.GrowTimeMax; set => _target.GrowTimeMax = value; }
        public float GrowTimeMin { get => _target.GrowTimeMin; set => _target.GrowTimeMin = value; }
        public HarvestedItem[] HarvestedItems { get => _target.HarvestedItems; set => _target.HarvestedItems = value; }
        public string PlantID { get => _target.PlantID; set => _target.PlantID = value; }
        public string SeedItemID { get => _target.SeedItemID; set => _target.SeedItemID = value; }
        public bool Underground { get => _target.Underground; set => _target.Underground = value; }
    }
}

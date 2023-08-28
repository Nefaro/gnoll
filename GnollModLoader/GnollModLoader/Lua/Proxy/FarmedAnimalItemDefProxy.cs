using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class FarmedAnimalItemDefProxy
    {
        private FarmedAnimalItemDef _target;

        [MoonSharpHidden]
        public FarmedAnimalItemDefProxy(FarmedAnimalItemDef target)
        {
            this._target = target;
        }

        public GenderType[] Genders { get => _target.Genders; set => _target.Genders = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public int Quantity { get => _target.Quantity; set => _target.Quantity = value; }
        public float RegrowRateMax { get => _target.RegrowRateMax; set => _target.RegrowRateMax = value; }
        public float RegrowRateMin { get => _target.RegrowRateMin; set => _target.RegrowRateMin = value; }
    }
}

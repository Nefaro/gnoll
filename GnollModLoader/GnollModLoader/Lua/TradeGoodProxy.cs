using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class TradeGoodProxy
    {
        private TradeGood _target;

        [MoonSharpHidden]
        public TradeGoodProxy(TradeGood target)
        {
            this._target = target;
        }

        public float Chance { get => _target.Chance; set => _target.Chance = value; }
        public uint FixedValue { get => _target.FixedValue; set => _target.FixedValue = value; }
        public GenderType Gender { get => _target.Gender; set => _target.Gender = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public int Max { get => _target.Max; set => _target.Max = value; }
        public int Min { get => _target.Min; set => _target.Min = value; }
        public ItemQuality Quality { get => _target.Quality; set => _target.Quality = value; }
        public string RaceID { get => _target.RaceID; set => _target.RaceID = value; }
    }
}

using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class TradeModifierProxy
    {
        private TradeModifier _target;

        [MoonSharpHidden]
        public TradeModifierProxy(TradeModifier target)
        {
            this._target = target;
        }

        public float BuyModifier { get => _target.BuyModifier; set => _target.BuyModifier = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public float SellModifier { get => _target.SellModifier; set => _target.SellModifier = value; }
    }
}

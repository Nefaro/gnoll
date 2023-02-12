using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string ItemID => _target.ItemID;
        public float BuyModifier => _target.BuyModifier;
        public float SellModifier => _target.SellModifier;
    }
}

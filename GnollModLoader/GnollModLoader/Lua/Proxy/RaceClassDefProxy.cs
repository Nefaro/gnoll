using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class RaceClassDefProxy
    {
        private RaceClassDef _target;

        [MoonSharpHidden]
        public RaceClassDefProxy(RaceClassDef target)
        {
            this._target = target;
        }

        public List<AttributeDef> Attributes => _target.Attributes;
        public float BaseCombatValue { get => _target.BaseCombatValue; set => _target.BaseCombatValue = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string RaceID { get => _target.RaceID; set => _target.RaceID = value; }
        public List<StartingItemDef> StartingItems  => _target.StartingItems;
        public string[] TemplateMaterialIDs { get => _target.TemplateMaterialIDs; set => _target.TemplateMaterialIDs = value; }
        public TradeGood[] TradeGoods { get => _target.TradeGoods; set => _target.TradeGoods = value; }
        public TradeModifier[] TradeModifiers { get => _target.TradeModifiers; set => _target.TradeModifiers = value; }
    }
}

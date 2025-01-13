using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ItemComponentProxy
    {
        private ItemComponent _target;

        [MoonSharpHidden]
        public ItemComponentProxy(ItemComponent target)
        {
            this._target = target;
        }

        public List<string> AllowedMaterials => _target.AllowedMaterials;
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public uint Quantity { get => _target.Quantity; set => _target.Quantity = value; }
        public List<string> RestrictedMaterials => _target.RestrictedMaterials;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
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

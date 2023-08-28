using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class WornEquipmentDefProxy
    {
        private WornEquipmentDef _target;

        [MoonSharpHidden]
        public WornEquipmentDefProxy(WornEquipmentDef target)
        {
            this._target = target;
        }

        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public string SmeltIntoItemID { get => _target.SmeltIntoItemID; set => _target.SmeltIntoItemID = value; }
        public uint SmeltAmount { get => _target.SmeltAmount; set => _target.SmeltAmount = value; }
    }
}

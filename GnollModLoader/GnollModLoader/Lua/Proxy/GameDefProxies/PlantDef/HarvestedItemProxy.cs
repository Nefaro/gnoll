﻿using MoonSharp.Interpreter;
using static GameLibrary.PlantDef;

namespace GnollModLoader.Lua.Proxy.GameDefProxies.PlantDef
{
    internal class HarvestedItemProxy
    {
        private HarvestedItem _target;

        [MoonSharpHidden]
        public HarvestedItemProxy(HarvestedItem target)
        {
            _target = target;
        }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
    }
}

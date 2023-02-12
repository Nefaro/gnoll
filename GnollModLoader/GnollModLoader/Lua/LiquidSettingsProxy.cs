using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class LiquidSettingsProxy
    {
        private LiquidSettings _target;

        [MoonSharpHidden]
        public LiquidSettingsProxy(LiquidSettings target)
        {
            this._target = target;
        }

        public Dictionary<string, string> MaterialIDToLiquidIDs => _target.MaterialIDToLiquidIDs;
    }
}

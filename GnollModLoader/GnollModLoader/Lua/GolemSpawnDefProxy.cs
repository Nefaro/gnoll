using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class GolemSpawnDefProxy
    {
        private GolemSpawnDef _target;

        [MoonSharpHidden]
        public GolemSpawnDefProxy(GolemSpawnDef target)
        {
            this._target = target;
        }

        public string ItemID => _target.ItemID;

        public uint Amount => _target.Amount;
    }
}

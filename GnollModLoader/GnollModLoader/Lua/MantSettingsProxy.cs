using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class MantSettingsProxy
    {
        private MantSettings _target;

        [MoonSharpHidden]
        public MantSettingsProxy(MantSettings target)
        {
            this._target = target;
        }

        public List<string> DesirableItemIDs => _target.DesirableItemIDs;
    }
}

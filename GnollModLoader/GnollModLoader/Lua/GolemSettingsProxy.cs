using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class GolemSettingsProxy
    {
        private GolemSettings _target;

        [MoonSharpHidden]
        public GolemSettingsProxy(GolemSettings target)
        {
            this._target = target;
        }

        public string CoreItemID => _target.CoreItemID;

        public List<GolemSpawnDef> GolemSpawnDefs => _target.GolemSpawnDefs;
    }
}

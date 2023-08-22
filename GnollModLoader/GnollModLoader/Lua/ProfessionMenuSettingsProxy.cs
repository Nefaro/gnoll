using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class ProfessionMenuSettingsProxy
    {
        private ProfessionMenuSettings _target;

        [MoonSharpHidden]
        public ProfessionMenuSettingsProxy(ProfessionMenuSettings target)
        {
            this._target = target;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class GrassSettingsProxy
    {
        private GrassSettings _target;

        [MoonSharpHidden]
        public GrassSettingsProxy(GrassSettings target)
        {
            this._target = target;
        }
        public string GrassMaterialID => _target.GrassMaterialID;

        public string GrownOnMaterialID => _target.GrownOnMaterialID;
    }
}

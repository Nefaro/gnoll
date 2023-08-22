using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class ConstructionPropertiesProxy
    {
        private ConstructionProperties _target;

        [MoonSharpHidden]
        public ConstructionPropertiesProxy(ConstructionProperties target)
        {
            this._target = target;
        }
        public bool HasFlag(ConstructionProperty flag) => _target.HasFlag(flag);
        public ConstructionProperty Flags { get => _target.Flags; set => _target.Flags = value; }
    }
}

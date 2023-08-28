using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class LiquidDefProxy
    {
        private LiquidDef _target;

        [MoonSharpHidden]
        public LiquidDefProxy(LiquidDef target)
        {
            this._target = target;
        }

        public float Compression { get => _target.Compression; set => _target.Compression = value; }
        public string Description { get => _target.Description; set => _target.Description = value; }
        public string LiquidID { get => _target.LiquidID; set => _target.LiquidID = value; }
        public float Viscosity { get => _target.Viscosity; set => _target.Viscosity = value; }
    }
}

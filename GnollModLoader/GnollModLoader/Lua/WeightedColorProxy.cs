using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class WeightedColorProxy
    {
        private WeightedColor _target;

        [MoonSharpHidden]
        public WeightedColorProxy(WeightedColor target)
        {
            this._target = target;
        }

        public Vector4 Color { get => _target.Color; set => _target.Color = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

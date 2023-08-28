using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class WeightedMaterialProxy
    {
        private WeightedMaterial _target;

        [MoonSharpHidden]
        public WeightedMaterialProxy(WeightedMaterial target)
        {
            this._target = target;
        }

        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

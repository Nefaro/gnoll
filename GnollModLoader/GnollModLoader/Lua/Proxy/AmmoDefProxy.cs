using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class AmmoDefProxy
    {
        private AmmoDef _target;

        [MoonSharpHidden]
        public AmmoDefProxy(AmmoDef target)
        {
            _target = target;
        }
        public string AmmoID { get => _target.AmmoID; set => _target.AmmoID = value; }
        public float BluntModifier { get => _target.BluntModifier; set => _target.BluntModifier = value; }
        public DamageType[] DamageTypes { get => _target.DamageTypes; set => _target.DamageTypes = value; }
        public float Edge { get => _target.Edge; set => _target.Edge = value; }
        public float Point { get => _target.Point; set => _target.Point = value; }
        public float VelocityModifier { get => _target.VelocityModifier; set => _target.VelocityModifier = value; }
        public float WeaponSize { get => _target.WeaponSize; set => _target.WeaponSize = value; }
    }
}

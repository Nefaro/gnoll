using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class NaturalWeaponDefProxy
    {
        private NaturalWeaponDef _target;

        [MoonSharpHidden]
        public NaturalWeaponDefProxy(NaturalWeaponDef target)
        {
            this._target = target;
        }

        public NaturalWeaponDef GroundWeapon { get => NaturalWeaponDef.GroundWeapon; set => NaturalWeaponDef.GroundWeapon = value; }
        public NaturalWeaponDef LavaWeapon { get => NaturalWeaponDef.LavaWeapon; set => NaturalWeaponDef.LavaWeapon = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public float Size { get => _target.Size; set => _target.Size = value; }
        public int TemplateMaterialIndex { get => _target.TemplateMaterialIndex; set => _target.TemplateMaterialIndex = value; }
        public WeaponDef WeaponDef { get => _target.WeaponDef; set => _target.WeaponDef = value; }
    }
}

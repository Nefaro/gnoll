using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class TargetedAttackDefProxy
    {
        private TargetedAttackDef _target;

        [MoonSharpHidden]
        public TargetedAttackDefProxy(TargetedAttackDef target)
        {
            this._target = target;
        }
        public AttackDef AttackDef { get => _target.AttackDef; set => _target.AttackDef = value; }
        public List<MaterialType> TargetedMaterials => _target.TargetedMaterials;
    }
}

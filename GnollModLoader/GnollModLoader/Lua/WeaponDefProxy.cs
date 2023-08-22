using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class WeaponDefProxy
    {
        private WeaponDef _target;

        [MoonSharpHidden]
        public WeaponDefProxy(WeaponDef target)
        {
            this._target = target;
        }

        public string AmmoItemID { get => _target.AmmoItemID; set => _target.AmmoItemID = value; }
        public AttackDef[] AttackMoves { get => _target.AttackMoves; set => _target.AttackMoves = value; }
        public float BluntModifier { get => _target.BluntModifier; set => _target.BluntModifier = value; }
        public DefendDef[] DefendMoves { get => _target.DefendMoves; set => _target.DefendMoves = value; }
        public float Edge { get => _target.Edge; set => _target.Edge = value; }
        public float KnockbackModifier { get => _target.KnockbackModifier; set => _target.KnockbackModifier = value; }
        public float PenetrationDepth { get => _target.PenetrationDepth; set => _target.PenetrationDepth = value; }
        public float Point { get => _target.Point; set => _target.Point = value; }
        public float ProjectileModifier { get => _target.ProjectileModifier; set => _target.ProjectileModifier = value; }
        public string Skill { get => _target.Skill; set => _target.Skill = value; }
        public WeaponStatusEffectType StatusEffect { get => _target.StatusEffect; set => _target.StatusEffect = value; }
        public TargetedAttackDef[] TargetedAttackMoves { get => _target.TargetedAttackMoves; set => _target.TargetedAttackMoves = value; }
        public float VelocityModifier { get => _target.VelocityModifier; set => _target.VelocityModifier = value; }
    }
}

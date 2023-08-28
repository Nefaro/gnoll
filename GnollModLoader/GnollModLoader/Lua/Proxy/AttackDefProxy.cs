using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class AttackDefProxy
    {
        private AttackDef _target;

        [MoonSharpHidden]
        public AttackDefProxy(AttackDef target)
        {
            this._target = target;
        }
        public bool IsPhysicalAttack() => _target.IsPhysicalAttack();
        public bool IsRangedAttack() => _target.IsRangedAttack();

        public string DamageString(DamageType damageType) => AttackDef.DamageString(damageType);
        public string DestroyString(DamageType damageType) => AttackDef.DestroyString(damageType);

        public Vector2 AttackRange { get => _target.AttackRange; set => _target.AttackRange = value; }
        public float AttackTime { get => _target.AttackTime; set => _target.AttackTime = value; }
        public AttackType AttackType { get => _target.AttackType; set => _target.AttackType = value; }
        public float DamageScale { get => _target.DamageScale; set => _target.DamageScale = value; }
        public DamageType[] DamageTypes { get => _target.DamageTypes; set => _target.DamageTypes = value; }
        public int MinimumSkillLevel { get => _target.MinimumSkillLevel; set => _target.MinimumSkillLevel = value; }
        public bool RequiresAmmo { get => _target.RequiresAmmo; set => _target.RequiresAmmo = value; }
        public string SFXEventName { get => _target.SFXEventName; set => _target.SFXEventName = value; }
        public string Verb { get => _target.Verb; set => _target.Verb = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

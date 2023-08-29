using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
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

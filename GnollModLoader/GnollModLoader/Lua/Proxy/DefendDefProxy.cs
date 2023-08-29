using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class DefendDefProxy
    {
        private DefendDef _target;

        [MoonSharpHidden]
        public DefendDefProxy(DefendDef target)
        {
            this._target = target;
        }
        public bool DefendsAgainst(AttackType attackType) => _target.DefendsAgainst(attackType);

        public float AttackDelay { get => _target.AttackDelay; set => _target.AttackDelay = value; }
        public float DefendTime { get => _target.DefendTime; set => _target.DefendTime = value; }
        public AttackType[] DefendTypes { get => _target.DefendTypes; set => _target.DefendTypes = value; }
        public string SFXEventName { get => _target.SFXEventName; set => _target.SFXEventName = value; }
        public string Verb { get => _target.Verb; set => _target.Verb = value; }


    }
}

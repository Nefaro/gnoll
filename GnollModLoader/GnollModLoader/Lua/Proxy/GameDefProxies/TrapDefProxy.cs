using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class TrapDefProxy
    {
        private TrapDef _target;

        [MoonSharpHidden]
        public TrapDefProxy(TrapDef target)
        {
            this._target = target;
        }

        public float AttackSpeed { get => _target.AttackSpeed; set => _target.AttackSpeed = value; }
        public string TrapID { get => _target.TrapID; set => _target.TrapID = value; }
        public WeaponDef WeaponDef { get => _target.WeaponDef; set => _target.WeaponDef = value; }
        public float WeaponSize { get => _target.WeaponSize; set => _target.WeaponSize = value; }
    }
}

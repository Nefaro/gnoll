using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class GenderDefProxy
    {
        private GenderDef _target;

        [MoonSharpHidden]
        public GenderDefProxy(GenderDef target)
        {
            this._target = target;
        }

        public GenderType Gender { get => _target.Gender; set => _target.Gender = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public float RandomWeight { get => _target.RandomWeight; set => _target.RandomWeight = value; }
    }
}

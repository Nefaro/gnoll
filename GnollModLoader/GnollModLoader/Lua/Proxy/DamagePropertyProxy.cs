using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class DamagePropertyProxy
    {
        private DamageProperty _target;

        [MoonSharpHidden]
        public DamagePropertyProxy(DamageProperty target)
        {
            this._target = target;
        }

        public float Blocks { get => _target.Blocks; set => _target.Blocks = value; }
        public float Break { get => _target.Break; set => _target.Break = value; }
        public float PercentReceived { get => _target.PercentReceived; set => _target.PercentReceived = value; }
        public float PercentTransfered { get => _target.PercentTransfered; set => _target.PercentTransfered = value; }
    }
}

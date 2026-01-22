using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class TreeProxy
    {
        private readonly Tree _target;

        [MoonSharpHidden]
        public TreeProxy(Tree target)
        {
            this._target = target;
        }

        public int TypeID => _target.TypeID();
        public string MaterialID => _target.MaterialID;

        public bool HasClipping => _target.HasClipping;

        public bool HasFruit => _target.HasFruit;

        public bool IsOutside { get => _target.Cell().Outside; }

        public float TimeToGrow { get => _target.float_0; set => _target.float_0 = value; }

        public void GrowClippings() => _target.method_1();

        public void GrowFruit() => _target.method_2();
    }
}

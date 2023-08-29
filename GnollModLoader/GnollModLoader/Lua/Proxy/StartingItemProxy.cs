using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class StartingItemProxy
    {
        private StartingItem _target;

        [MoonSharpHidden]
        public StartingItemProxy(StartingItem target)
        {
            this._target = target;
        }

        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

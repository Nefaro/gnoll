using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class WeightedItemProxy
    {
        private WeightedItem _target;

        [MoonSharpHidden]
        public WeightedItemProxy(WeightedItem target)
        {
            this._target = target;
        }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

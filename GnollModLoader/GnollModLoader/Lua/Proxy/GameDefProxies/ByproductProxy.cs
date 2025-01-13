using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ByproductProxy
    {
        private Byproduct _target;

        [MoonSharpHidden]
        public ByproductProxy(Byproduct target)
        {
            this._target = target;
        }

        public string ConversionMaterial { get => _target.ConversionMaterial; set => _target.ConversionMaterial = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public int MaterialIndex { get => _target.MaterialIndex; set => _target.MaterialIndex = value; }
        public uint Quantity { get => _target.Quantity; set => _target.Quantity = value; }
    }
}

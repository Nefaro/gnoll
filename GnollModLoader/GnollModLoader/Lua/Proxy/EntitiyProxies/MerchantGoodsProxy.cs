using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class MerchantGoodsProxy
    {
        private readonly MerchantGoods _target;

        [MoonSharpHidden]
        public MerchantGoodsProxy(MerchantGoods target)
        {
            this._target = target;
        }

        public List<AvailableGood> AvailableGoods => _target.AvailableGoods;

    }
}

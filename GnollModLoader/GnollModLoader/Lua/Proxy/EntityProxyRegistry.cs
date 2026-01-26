using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiyProxies;
using GnollModLoader.Lua.Proxy.EntityProxies;

namespace GnollModLoader.Lua.Proxy
{
    internal class EntityProxyRegistry
    {
        internal static void RegisterTypes()
        {
            UserData.RegisterProxyType<GameEntityManagerProxy, GameEntityManager>(t => new GameEntityManagerProxy(t));

            UserData.RegisterProxyType<AvailableGoodProxy, AvailableGood>(t => new AvailableGoodProxy(t));
            UserData.RegisterProxyType<CharacterProxy, Character>(t => new CharacterProxy(t));
            UserData.RegisterProxyType<EnvoyProxy, Envoy>(t => new EnvoyProxy(t));
            UserData.RegisterProxyType<FactionProxy, Faction>(t => new FactionProxy(t));
            UserData.RegisterProxyType<MerchantGoodsProxy, MerchantGoods>(t => new MerchantGoodsProxy(t));
            UserData.RegisterProxyType<MilitaryProxy, Military>(t => new MilitaryProxy(t));
            UserData.RegisterProxyType<SquadProxy, Squad>(t => new SquadProxy(t));
            UserData.RegisterProxyType<TreeProxy, Tree>(t => new TreeProxy(t));
            UserData.RegisterProxyType<WorldProxy, World>(t => new WorldProxy(t));
        }
    }
}

using Game;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class EntitiesProxyRegistry
    {
        internal static void RegisterTypes()
        {
            UserData.RegisterProxyType<WorldProxy, World>(t => new WorldProxy(t));
            UserData.RegisterProxyType<MilitaryProxy, Military>(t => new MilitaryProxy(t));
            UserData.RegisterProxyType<SquadProxy, Squad>(t => new SquadProxy(t));
        }
    }
}

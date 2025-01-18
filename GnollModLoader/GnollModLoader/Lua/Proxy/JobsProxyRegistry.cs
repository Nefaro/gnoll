using Game;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiyProxies;
using GnollModLoader.Lua.Proxy.JobsProxies;

namespace GnollModLoader.Lua.Proxy
{
    internal class JobsProxyRegistry
    {
        internal static void RegisterTypes()
        {
            UserData.RegisterProxyType<ForeignTradeJobProxy, ForeignTradeJob>(t => new ForeignTradeJobProxy(t));
            UserData.RegisterProxyType<ForeignTradeJobDataProxy, ForeignTradeJobData>(t => new ForeignTradeJobDataProxy(t));
            UserData.RegisterProxyType<JobProxy, Job>(t => new JobProxy(t));
        }
    }
}

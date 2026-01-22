using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class WorldProxy
    {
        private readonly World _target;

        [MoonSharpHidden]
        public WorldProxy(World target)
        {
            this._target = target;
        }

        public NotificationManager NotificationManager { get => _target.NotificationManager; }

    }
}

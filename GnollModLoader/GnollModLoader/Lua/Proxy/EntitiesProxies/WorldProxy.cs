using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiesProxies
{
    internal class WorldProxy
    {
        private World _target;

        [MoonSharpHidden]
        public WorldProxy(World target)
        {
            this._target = target;
        }

        public NotificationManager NotificationManager { get => _target.NotificationManager; }

    }
}
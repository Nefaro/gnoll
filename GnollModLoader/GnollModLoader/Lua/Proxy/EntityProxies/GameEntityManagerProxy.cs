using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntityProxies
{
    internal class GameEntityManagerProxy
    {
        private readonly GameEntityManager _target;

        [MoonSharpHidden]
        public GameEntityManagerProxy(GameEntityManager target)
        {
            this._target = target;
        }

        public void RemoveFromUpdateList(GameEntity gameEnt) => _target.RemoveFromUpdateList(gameEnt);
    }
}

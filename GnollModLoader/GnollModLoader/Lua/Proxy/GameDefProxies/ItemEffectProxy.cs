﻿using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ItemEffectProxy
    {
        private ItemEffect _target;

        [MoonSharpHidden]
        public ItemEffectProxy(ItemEffect target)
        {
            this._target = target;
        }

        public float Amount { get => _target.Amount; set => _target.Amount = value; }
        public ItemEffectType Effect { get => _target.Effect; set => _target.Effect = value; }
    }
}

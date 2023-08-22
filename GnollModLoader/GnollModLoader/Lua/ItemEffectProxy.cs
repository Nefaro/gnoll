using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
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

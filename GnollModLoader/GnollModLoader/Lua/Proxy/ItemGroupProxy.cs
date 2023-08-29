using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class ItemGroupProxy
    {
        private ItemGroup _target;

        [MoonSharpHidden]
        public ItemGroupProxy(ItemGroup target)
        {
            this._target = target;
        }

        public HashSet<string> AllowedItems => _target.AllowedItems;
    }
}

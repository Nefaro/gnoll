using System.Collections.Generic;
using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using GameLibrary;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class AvailableGoodProxy
    {
        private readonly AvailableGood _target;

        [MoonSharpHidden]
        public AvailableGoodProxy(AvailableGood target)
        {
            this._target = target;
        }

        public string ItemID => _target.ItemID;

        public string MaterialID => _target.MaterialID;

        public ItemQuality Quality => _target.Quality;

        public string Name => _target.Name();

        public uint Value => _target.Value();
    }
}

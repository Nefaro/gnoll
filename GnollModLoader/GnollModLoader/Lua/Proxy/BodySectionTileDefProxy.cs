using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class BodySectionTileDefProxy
    {
        private BodySectionTileDef _target;

        [MoonSharpHidden]
        public BodySectionTileDefProxy(BodySectionTileDef target)
        {
            this._target = target;
        }

        public List<BodySectionTileDetails> TileDetails => _target.TileDetails;
    }
}

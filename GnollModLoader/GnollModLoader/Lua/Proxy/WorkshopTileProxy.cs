using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class WorkshopTileProxy
    {
        private WorkshopTile _target;

        [MoonSharpHidden]
        public WorkshopTileProxy(WorkshopTile target)
        {
            this._target = target;
        }

        public Vector2 Position { get => _target.Position; set => _target.Position = value; }
        public WorkshopTilePart[] TileParts { get => _target.TileParts; set => _target.TileParts = value; }
    }
}

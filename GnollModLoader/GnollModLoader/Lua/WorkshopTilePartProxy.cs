using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class WorkshopTilePartProxy
    {
        private WorkshopTilePart _target;

        [MoonSharpHidden]
        public WorkshopTilePartProxy(WorkshopTilePart target)
        {
            this._target = target;
        }

        public Vector4 FixedMaterialColor { get => _target.FixedMaterialColor; set => _target.FixedMaterialColor = value; }
        public int MaterialIndex { get => _target.MaterialIndex; set => _target.MaterialIndex = value; }
        public Vector2 Offset { get => _target.Offset; set => _target.Offset = value; }
        public CameraOrientation Orientation { get => _target.Orientation; set => _target.Orientation = value; }
        public string SpriteID { get => _target.SpriteID; set => _target.SpriteID = value; }
        public Dictionary<string, string> SpriteIDByMaterialID => _target.SpriteIDByMaterialID;
    }
}

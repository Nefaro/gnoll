using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using System.Runtime.InteropServices;

namespace GnollModLoader.Lua
{
    internal class BodySectionTileDetailsProxy
    {
        private BodySectionTileDetails _target;

        [MoonSharpHidden]
        public BodySectionTileDetailsProxy(BodySectionTileDetails target)
        {
            this._target = target;
        }

        public List<WeightedColor> Colors => _target.Colors;
        public string SpriteID { get => _target.SpriteID; set => _target.SpriteID = value; }
        public bool UseHairColor { get => _target.UseHairColor; set => _target.UseHairColor = value; }
        public bool UseSkinColor { get => _target.UseSkinColor; set => _target.UseSkinColor = value; }
        public float Weight { get => _target.Weight; set => _target.Weight = value; }
    }
}

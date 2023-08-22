using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class BodySectionTilesDefProxy
    {
        private BodySectionTilesDef _target;

        [MoonSharpHidden]
        public BodySectionTilesDefProxy(BodySectionTilesDef target)
        {
            this._target = target;
        }

        public BodySectionTileDef BodyTiles { get => _target.BodyTiles; set => _target.BodyTiles = value; }
        public List<BodySectionTileDef> FemaleDecorations => _target.FemaleDecorations;
        public List<BodySectionTileDef> MaleDecorations  => _target.MaleDecorations; 
        public List<BodySectionTileDef> NeuterDecorations => _target.NeuterDecorations; 
    }
}

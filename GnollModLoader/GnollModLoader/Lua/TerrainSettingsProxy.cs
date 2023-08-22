using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using static GameLibrary.TerrainSettings;

namespace GnollModLoader.Lua
{
    internal class TerrainSettingsProxy
    {
        private Game.TerrainSettings _target;

        [MoonSharpHidden]
        public TerrainSettingsProxy(Game.TerrainSettings target)
        {
            this._target = target;
        }

        public string AirMaterialID { get => _target.string_12; set => _target.string_12 = value; }
        public string BottomLeftBorderID { get => _target.string_10; set => _target.string_10 = value; }
        public string BottomRightBorderID { get => _target.string_11; set => _target.string_11 = value; }
        public string ClayMaterialID { get => _target.string_18; set => _target.string_18 = value; }
        public Color DarknessIndicatorColor { get => _target.color_1; set => _target.color_1 = value; }
        public string DarknessIndicatorSpriteID { get => _target.string_1; set => _target.string_1 = value; }
        public string DirtMaterialID { get => _target.string_17; set => _target.string_17 = value; }
        public Color HiddenCellColor { get => _target.color_0; set => _target.color_0 = value; }
        public string HiddenCellSpriteID { get => _target.string_0; set => _target.string_0 = value; }
        public string LavaMaterialID { get => _target.string_15; set => _target.string_15 = value; }
        public string LeftBorderID { get => _target.string_6; set => _target.string_6 = value; }
        public string LeftWallBorderID { get => _target.string_8; set => _target.string_8 = value; }
        public List<GrowthSettings> Minerals => _target.Minerals;
        public string MudMaterialID { get => _target.string_19; set => _target.string_19 = value; }
        public string ObsidianMaterialID { get => _target.string_16; set => _target.string_16 = value; }
        public List<GrowthSettings> Plants => _target.Plants;
        public string RainMaterialID { get => _target.string_14; set => _target.string_14 = value; }
        public string RightBorderID { get => _target.string_7; set => _target.string_7 = value; }
        public string RightWallBorderID { get => _target.string_9; set => _target.string_9 = value; }
        public List<string> StoneMaterialIDs => _target.StoneMaterialIDs;
        public string TopLeftBorderID { get => _target.string_2; set => _target.string_2 = value; }
        public string TopLeftWallBorderID { get => _target.string_4; set => _target.string_4 = value; }
        public string TopRightBorderID { get => _target.string_3; set => _target.string_3 = value; }
        public string TopRightWallBorderID { get => _target.string_5; set => _target.string_5 = value; }
        public List<GrowthSettings> Trees => _target.Trees;
        public string WaterMaterialID { get => _target.string_13; set => _target.string_13 = value; }
    }
}

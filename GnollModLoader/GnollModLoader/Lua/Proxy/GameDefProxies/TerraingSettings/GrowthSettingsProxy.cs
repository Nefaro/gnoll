using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;
using static GameLibrary.TerrainSettings;

namespace GnollModLoader.Lua.Proxy.GameDefProxies.TerraingSettings
{
    internal class GrowthSettingsProxy
    {
        private GrowthSettings _target;

        [MoonSharpHidden]
        public GrowthSettingsProxy(GrowthSettings target)
        {
            _target = target;
        }

        public int EndDepth { get => _target.EndDepth; set => _target.EndDepth = value; }
        public int MaxQuantity { get => _target.MaxQuantity; set => _target.MaxQuantity = value; }
        public int MaxSize { get => _target.MaxSize; set => _target.MaxSize = value; }
        public int MinQuantity { get => _target.MinQuantity; set => _target.MinQuantity = value; }
        public int MinSize { get => _target.MinSize; set => _target.MinSize = value; }
        public int StartDepth { get => _target.StartDepth; set => _target.StartDepth = value; }
        public GrowthType Type { get => _target.Type; set => _target.Type = value; }

        public List<WeightedMaterial> WeightedMaterialIDs => _target.WeightedMaterialIDs;
    }
}

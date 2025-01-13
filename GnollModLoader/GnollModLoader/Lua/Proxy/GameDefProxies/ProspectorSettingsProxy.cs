using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ProspectorSettingsProxy
    {
        private ProspectorSettings _target;

        [MoonSharpHidden]
        public ProspectorSettingsProxy(ProspectorSettings target)
        {
            this._target = target;
        }

        public string MetalSliverItemID { get => _target.MetalSliverItemID; set => _target.MetalSliverItemID = value; }
        public string SilicaItemID { get => _target.SilicaItemID; set => _target.SilicaItemID = value; }
        public string SilicaMaterialID { get => _target.SilicaMaterialID; set => _target.SilicaMaterialID = value; }
        public Dictionary<MaterialType, List<WeightedMaterial>> WeightedMaterialIDsByMaterialType => _target.WeightedMaterialIDsByMaterialType;
    }
}

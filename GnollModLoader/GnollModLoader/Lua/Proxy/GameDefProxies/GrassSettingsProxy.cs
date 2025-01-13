using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class GrassSettingsProxy
    {
        private GrassSettings _target;

        [MoonSharpHidden]
        public GrassSettingsProxy(GrassSettings target)
        {
            this._target = target;
        }

        public string GrassMaterialID { get => _target.GrassMaterialID; set => _target.GrassMaterialID = value; }
        public string GrownOnMaterialID { get => _target.GrownOnMaterialID; set => _target.GrownOnMaterialID = value; }
    }
}

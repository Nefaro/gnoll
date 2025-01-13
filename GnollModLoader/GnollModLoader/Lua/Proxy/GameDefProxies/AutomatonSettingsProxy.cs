using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class AutomatonSettingsProxy
    {
        private AutomatonSettings _target;

        [MoonSharpHidden]
        public AutomatonSettingsProxy(AutomatonSettings target)
        {
            this._target = target;
        }

        public string AutomatonItemID { get => _target.AutomatonItemID; set => _target.AutomatonItemID = value; }
        public string CoreItemID { get => _target.CoreItemID; set => _target.CoreItemID = value; }
        public string FuelItemID { get => _target.FuelItemID; set => _target.FuelItemID = value; }
        public string FuelMaterialID { get => _target.FuelMaterialID; set => _target.FuelMaterialID = value; }
        public string RepairWorkshopID { get => _target.RepairWorkshopID; set => _target.RepairWorkshopID = value; }
    }
}

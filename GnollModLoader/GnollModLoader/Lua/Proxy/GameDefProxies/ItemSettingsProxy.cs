using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ItemSettingsProxy
    {
        private Game.ItemSettings _target;

        [MoonSharpHidden]
        public ItemSettingsProxy(Game.ItemSettings target)
        {
            this._target = target;
        }
        public HashSet<string> Armor => _target.Armor;
        public List<string> ArtificialLimbMaterialIDs => _target.ArtificialLimbMaterialIDs;
        public string CorpseItemID { get => _target.string_0; set => _target.string_0 = value; }
        public WeaponDef DefaultWeapon { get => _target.weaponDef_0; set => _target.weaponDef_0 = value; }
        public HashSet<string> Drinks => _target.Drinks;
        public HashSet<string> Food => _target.Food;
        public HashSet<string> Furniture => _target.Furniture;
        public string LimbItemID { get => _target.string_1; set => _target.string_1 = value; }
        public Dictionary<string, string> StorageIDs => _target.StorageIDs;
        public HashSet<string> TransportContainers => _target.TransportContainers;
        public HashSet<string> Weapons => _target.Weapons;
        public Dictionary<string, WornEquipmentDef> WornEquipment => _target.WornEquipment;
        public Dictionary<string, ToolSettings> Tools => _target.dictionary_1;
        public Dictionary<string, string> ItemIDToPileItemID => _target.dictionary_2;
        public Dictionary<MaterialType, string> MaterialTypeToItemID => _target.dictionary_4;
        public Dictionary<string, string> ItemIDToAmmoID => _target.dictionary_5;
        public Dictionary<string, List<string>> AmmoContainersByAmmoItemID => _target.dictionary_6;
        public Dictionary<string, string> BodyPartIDToItemID => _target.dictionary_7;
        public Dictionary<string, string> ItemIDToArtificialLimbBodyPartID => _target.dictionary_8;
    }
}

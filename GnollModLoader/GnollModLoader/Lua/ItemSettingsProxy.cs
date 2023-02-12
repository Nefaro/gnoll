using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
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
        public string CorpseItemID => _target.CorpseItemID;
        public WeaponDef DefaultWeapon => _target.DefaultWeapon;
        public HashSet<string> Drinks => _target.Drinks;

        public HashSet<string> Food => _target.Food;
        public HashSet<string> Furniture => _target.Furniture;
        public string LimbItemID => _target.LimbItemID;
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

using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ItemDefProxy
    {
        private ItemDef _target;

        [MoonSharpHidden]
        public ItemDefProxy(ItemDef target)
        {
            this._target = target;
        }
        public float BaseValue { get => _target.BaseValue; set => _target.BaseValue = value; }
        public float CombatRatingModifier { get => _target.CombatRatingModifier; set => _target.CombatRatingModifier = value; }
        public string Description { get => _target.Description; set => _target.Description = value; }
        public List<ItemEffect> Effects => _target.Effects;
        public float EquippedJobPenalty { get => _target.EquippedJobPenalty; set => _target.EquippedJobPenalty = value; }
        public float EquippedMovePenalty { get => _target.EquippedMovePenalty; set => _target.EquippedMovePenalty = value; }
        public EquipmentType EquipSlot { get => _target.EquipSlot; set => _target.EquipSlot = value; }
        public string GroupName { get => _target.GroupName; set => _target.GroupName = value; }
        public bool HasQuality { get => _target.HasQuality; set => _target.HasQuality = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string ObtainDescription { get => _target.ObtainDescription; set => _target.ObtainDescription = value; }
        public string Prefix { get => _target.Prefix; set => _target.Prefix = value; }
        public string Suffix { get => _target.Suffix; set => _target.Suffix = value; }
        public float Thickness { get => _target.Thickness; set => _target.Thickness = value; }
        public bool TwoHanded { get => _target.TwoHanded; set => _target.TwoHanded = value; }
        public float Value { get => _target.Value; set => _target.Value = value; }
        public WeaponDef WeaponDef { get => _target.WeaponDef; set => _target.WeaponDef = value; }
        public float WeaponSize { get => _target.WeaponSize; set => _target.WeaponSize = value; }
    }
}

using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class CraftableItemProxy
    {
        private CraftableItem _target;

        [MoonSharpHidden]
        public CraftableItemProxy(CraftableItem target)
        {
            this._target = target;
        }

        public CharacterAttributeType[] AttributeUsed { get => _target.AttributeUsed; set => _target.AttributeUsed = value; }
        public string BlueprintID { get => _target.BlueprintID; set => _target.BlueprintID = value; }
        public List<Byproduct> Byproducts => _target.Byproducts;
        public ItemComponent[] Components { get => _target.Components; set => _target.Components = value; }
        public string ConversionMaterial { get => _target.ConversionMaterial; set => _target.ConversionMaterial = value; }
        public float Difficulty { get => _target.Difficulty; set => _target.Difficulty = value; }
        public bool DisallowAutoGen { get => _target.DisallowAutoGen; set => _target.DisallowAutoGen = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
        public uint Quantity { get => _target.Quantity; set => _target.Quantity = value; }
        public int RequiredSkillLevel { get => _target.RequiredSkillLevel; set => _target.RequiredSkillLevel = value; }
        public string SkillUsed { get => _target.SkillUsed; set => _target.SkillUsed = value; }
    }
}

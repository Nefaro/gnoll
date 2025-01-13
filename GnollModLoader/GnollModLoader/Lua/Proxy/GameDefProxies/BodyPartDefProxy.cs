using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class BodyPartDefProxy
    {
        private BodyPartDef _target;

        [MoonSharpHidden]
        public BodyPartDefProxy(BodyPartDef target)
        {
            this._target = target;
        }

        public BodyFunction BodyFunction { get => _target.BodyFunction; set => _target.BodyFunction = value; }
        public BodyPartProperty BodyProperties { get => _target.BodyProperties; set => _target.BodyProperties = value; }
        public List<BodyPartDef> ContainedParts => _target.ContainedParts;
        public string HarvestedItem { get => _target.HarvestedItem; set => _target.HarvestedItem = value; }
        public int HarvestedQuantity { get => _target.HarvestedQuantity; set => _target.HarvestedQuantity = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public NaturalWeaponDef NaturalWeapon { get => _target.NaturalWeapon; set => _target.NaturalWeapon = value; }
        public bool Symmetrical { get => _target.Symmetrical; set => _target.Symmetrical = value; }
        public int TemplateMaterialIndex { get => _target.TemplateMaterialIndex; set => _target.TemplateMaterialIndex = value; }
        public float Thickness { get => _target.Thickness; set => _target.Thickness = value; }
        public float ToHitWeight { get => _target.ToHitWeight; set => _target.ToHitWeight = value; }
    }
}

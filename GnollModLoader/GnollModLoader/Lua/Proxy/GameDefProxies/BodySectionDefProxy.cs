using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class BodySectionDefProxy
    {
        private BodySectionDef _target;

        [MoonSharpHidden]
        public BodySectionDefProxy(BodySectionDef target)
        {
            this._target = target;
        }

        public string BodyPartID { get => _target.BodyPartID; set => _target.BodyPartID = value; }
        public List<BodySectionDef> ConnectedSections  => _target.ConnectedSections;
        public bool Connection { get => _target.Connection; set => _target.Connection = value; }
        public EquipmentType EquipType { get => _target.EquipType; set => _target.EquipType = value; }
        public bool Hand { get => _target.Hand; set => _target.Hand = value; }
        public bool Limb { get => _target.Limb; set => _target.Limb = value; }
        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public bool Symmetrical { get => _target.Symmetrical; set => _target.Symmetrical = value; }
        public int TemplateMaterialIndex { get => _target.TemplateMaterialIndex; set => _target.TemplateMaterialIndex = value; }
        public uint TemplatePartsQuantity { get => _target.TemplatePartsQuantity; set => _target.TemplatePartsQuantity = value; }
        public BodySectionTilesDef Tile { get => _target.Tile; set => _target.Tile = value; }
        public float ToHitWeight { get => _target.ToHitWeight; set => _target.ToHitWeight = value; }
    }
}

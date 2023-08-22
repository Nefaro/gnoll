﻿using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class BodyDefProxy
    {
        private BodyDef _target;

        [MoonSharpHidden]
        public BodyDefProxy(BodyDef target)
        {
            this._target = target;
        }

        public List<EquipmentType> AdditionalEquipmentSlots => _target.AdditionalEquipmentSlots;
        public List<FarmedAnimalItemDef> FarmedItems => _target.FarmedItems;
        public string ID { get => _target.ID; set => _target.ID = value; }
        public BodySectionDef MainBody { get => _target.MainBody; set => _target.MainBody = value; }
        public bool ShowEquipment { get => _target.ShowEquipment; set => _target.ShowEquipment = value; }
    }
}

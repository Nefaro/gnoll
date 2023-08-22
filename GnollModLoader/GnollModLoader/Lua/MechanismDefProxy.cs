using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class MechanismDefProxy
    {
        private MechanismDef _target;

        [MoonSharpHidden]
        public MechanismDefProxy(MechanismDef target)
        {
            this._target = target;
        }

        public bool RequiresAir() => _target.RequiresAir();

        public MechanismDef.BuildSlot BuildsIn { get => _target.BuildsIn; set => _target.BuildsIn = value; }
        public Vector3[] ConnectionCells { get => _target.ConnectionCells; set => _target.ConnectionCells = value; }
        public string ConstructionID { get => _target.ConstructionID; set => _target.ConstructionID = value; }
        public string FuelItemID { get => _target.FuelItemID; set => _target.FuelItemID = value; }
        public string MechanismID { get => _target.MechanismID; set => _target.MechanismID = value; }
        public float Power { get => _target.Power; set => _target.Power = value; }
        public bool PressureSwitch { get => _target.PressureSwitch; set => _target.PressureSwitch = value; }
        public bool RequiresFloor { get => _target.RequiresFloor; set => _target.RequiresFloor = value; }

    }
}

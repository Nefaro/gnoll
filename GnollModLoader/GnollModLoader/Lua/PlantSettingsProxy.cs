using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class PlantSettingsProxy
    {
        private PlantSettings _target;

        [MoonSharpHidden]
        public PlantSettingsProxy(PlantSettings target)
        {
            this._target = target;
        }

        public Dictionary<string, string> MaterialIDToPlantIDs => _target.MaterialIDToPlantIDs;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class MechanismSettingsProxy
    {
        private Game.MechanismSettings _target;

        [MoonSharpHidden]
        public MechanismSettingsProxy(Game.MechanismSettings target)
        {
            this._target = target;
        }

        public HashSet<string> Axles => _target.hashSet_0;
        public HashSet<string> Switches => _target.hashSet_1;
        public HashSet<string> Traps => _target.hashSet_2;
        public HashSet<string> MechanicalWalls => _target.hashSet_3;
        public HashSet<string> Hatches => _target.hashSet_4;
        public HashSet<string> Pumps => _target.hashSet_5;
        public HashSet<string> Handcranks => _target.hashSet_6;
        public HashSet<string> SteamEngines => _target.hashSet_7;
        public HashSet<string> Windmills => _target.hashSet_8;
        public Dictionary<string, string> ConstructionIDToMechanismID => _target.dictionary_0;
        public Dictionary<string, string> MechanismIDToTrapID => _target.dictionary_1;
    }
}

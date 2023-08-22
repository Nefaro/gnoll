using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class SquadDefProxy
    {
        private SquadDef _target;

        [MoonSharpHidden]
        public SquadDefProxy(SquadDef target)
        {
            this._target = target;
        }

        public bool AllClasses { get => _target.AllClasses; set => _target.AllClasses = value; }
        public float BaseCombatValue { get => _target.BaseCombatValue; set => _target.BaseCombatValue = value; }
        public List<RaceClassDef> Classes => _target.Classes;
        public SquadType Type { get => _target.Type; set => _target.Type = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Game;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class CharacterSettingsProxy
    {
        private CharacterSettings _target;

        [MoonSharpHidden]
        public CharacterSettingsProxy(CharacterSettings target)
        {
            this._target = target;
        }

        public List<string> CombatSkills => _target.CombatSkills;
        public List<string> LaborSkills => _target.LaborSkills;
        public List<string> DefaultAllowedSkills => _target.DefaultAllowedSkills;
        public string DodgeSkill => _target.DodgeSkill;
        public string ZombieRaceID => _target.ZombieRaceID;
        public TradeModifier[] DefaultTradeModifiers => _target.DefaultTradeModifiers;
    }
}

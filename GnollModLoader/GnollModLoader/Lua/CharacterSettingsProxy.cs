using System.Collections.Generic;
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
        public List<string> DefaultAllowedSkills => _target.DefaultAllowedSkills;
        public TradeModifier[] DefaultTradeModifiers { get => _target.DefaultTradeModifiers; set => _target.DefaultTradeModifiers = value; }
        public string DodgeSkill { get => _target.DodgeSkill; set => _target.DodgeSkill = value; }
        public List<string> LaborSkills => _target.LaborSkills;
        public string ZombieRaceID { get => _target.ZombieRaceID; set => _target.ZombieRaceID = value; }
    }
}

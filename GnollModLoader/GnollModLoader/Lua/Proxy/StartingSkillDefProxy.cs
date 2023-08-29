using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class StartingSkillDefProxy
    {
        private StartingSkillDef _target;

        [MoonSharpHidden]
        public StartingSkillDefProxy(StartingSkillDef target)
        {
            this._target = target;
        }

        public int Max { get => _target.Max; set => _target.Max = value; }
        public int Mean { get => _target.Mean; set => _target.Mean = value; }
        public int Min { get => _target.Min; set => _target.Min = value; }
        public string Skill { get => _target.Skill; set => _target.Skill = value; }
    }
}

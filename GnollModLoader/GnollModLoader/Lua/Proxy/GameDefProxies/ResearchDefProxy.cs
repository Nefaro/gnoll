using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ResearchDefProxy
    {
        private ResearchDef _target;

        [MoonSharpHidden]
        public ResearchDefProxy(ResearchDef target)
        {
            this._target = target;
        }

        public float BaseDifficulty { get => _target.BaseDifficulty; set => _target.BaseDifficulty = value; }
        public float PerLevelDifficulty { get => _target.PerLevelDifficulty; set => _target.PerLevelDifficulty = value; }
        public string ResearchID { get => _target.ResearchID; set => _target.ResearchID = value; }
        public string SkillID { get => _target.SkillID; set => _target.SkillID = value; }
    }
}

using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ScaledSkillProxy
    {
        private ScaledSkill _target;

        [MoonSharpHidden]
        public ScaledSkillProxy(ScaledSkill target)
        {
            this._target = target;
        }

        public string SkillID { get => _target.SkillID; set => _target.SkillID = value; }
        public float SkillIncreaseScale { get => _target.SkillIncreaseScale; set => _target.SkillIncreaseScale = value; }
    }
}

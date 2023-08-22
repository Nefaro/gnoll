using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class JobSettingProxy
    {
        private JobSetting _target;

        [MoonSharpHidden]
        public JobSettingProxy(JobSetting target)
        {
            this._target = target;
        }

        public List<ScaledSkill> AdditionalSkillIDs => _target.AdditionalSkillIDs;
        public List<string> ConstructionIDs => _target.ConstructionIDs;
        public JobType Type { get => _target.Type; set => _target.Type = value; }
        public int RequiredSkillLevel { get => _target.RequiredSkillLevel; set => _target.RequiredSkillLevel = value; }
        public string RequiredToolItemID { get => _target.RequiredToolItemID; set => _target.RequiredToolItemID = value; }
        public string SkillID { get => _target.SkillID; set => _target.SkillID = value; }
        public float SkillIncreaseScale { get => _target.SkillIncreaseScale; set => _target.SkillIncreaseScale = value; }
    }
}

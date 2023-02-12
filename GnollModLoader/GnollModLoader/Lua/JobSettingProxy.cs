using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using GnollModLoader.Model;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class JobSettingProxy
    {
        private GameLibrary.JobSetting _target;

        [MoonSharpHidden]
        public JobSettingProxy(GameLibrary.JobSetting target)
        {
            this._target = target;
        }

        public JobType JobType { get { return _target.Type; } }

        public string SkillID => _target.SkillID;

        public int RequiredSkillLevel => _target.RequiredSkillLevel;
        public float SkillIncreaseScale => _target.SkillIncreaseScale;
        public List<ScaledSkill> AdditionalSkillIDs => _target.AdditionalSkillIDs;
        public string RequiredToolItemID => _target.RequiredToolItemID;
        public List<string> ConstructionIDs => _target.ConstructionIDs;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class JobSettingsProxy
    {
        private Game.JobSettings _target;

        [MoonSharpHidden]
        public JobSettingsProxy(Game.JobSettings target)
        {
            this._target = target;
        }

        public string DefaultJobSkillID => _target.DefaultJobSkillID;
        public Color HeightIndicatorColor =>_target.HeightIndicatorColor;
        public Dictionary<JobType, JobSettingProxy> JobSettingPerJobType => _target.dictionary_0;
    }
}

using System.Collections.Generic;
using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class JobSettingsProxy
    {
        private Game.JobSettings _target;

        [MoonSharpHidden]
        public JobSettingsProxy(Game.JobSettings target)
        {
            this._target = target;
        }

        public string DefaultJobSkillID { get => _target.string_0; set => _target.string_0 = value; }
        public Color HeightIndicatorColor { get => _target.color_0; set => _target.color_0 = value; }
        public Dictionary<JobType, JobSetting> JobSettingPerJobType => _target.dictionary_0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class ScaledSkillProxy
    {
        private ScaledSkill _target;

        [MoonSharpHidden]
        public ScaledSkillProxy(ScaledSkill target)
        {
            this._target = target;
        }
        public string SkillID => _target.SkillID;
        public float SkillIncreaseScale => _target.SkillIncreaseScale;
    }
}

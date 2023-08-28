using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class ToolSettingsProxy
    {
        private ToolSettings _target;

        [MoonSharpHidden]
        public ToolSettingsProxy(ToolSettings target)
        {
            this._target = target;
        }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }

        public List<string> RelatedSkills => _target.RelatedSkills;
    }
}

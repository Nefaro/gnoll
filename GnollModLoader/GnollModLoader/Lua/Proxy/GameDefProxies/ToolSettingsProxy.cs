using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
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

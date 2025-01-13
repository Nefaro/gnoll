using System.Collections.Generic;
using MoonSharp.Interpreter;
using DefaultProfession = GameLibrary.NewGameSettings.DefaultProfession;

namespace GnollModLoader.Lua.Proxy.GameDefProxies.NewGameSettings
{
    internal class DefaultProfessionProxy
    {
        private DefaultProfession _target;

        [MoonSharpHidden]
        public DefaultProfessionProxy(DefaultProfession target)
        {
            this._target = target;
        }

        public string Name { get => _target.Name; set => _target.Name = value; }
        public List<string> SkillIDs => _target.SkillIDs;
    }
}

using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class MantSettingsProxy
    {
        private MantSettings _target;

        [MoonSharpHidden]
        public MantSettingsProxy(MantSettings target)
        {
            this._target = target;
        }

        public List<string> DesirableItemIDs => _target.DesirableItemIDs;
    }
}

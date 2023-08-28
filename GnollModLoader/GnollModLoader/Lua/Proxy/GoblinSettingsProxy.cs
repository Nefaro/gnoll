using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class GoblinSettingsProxy
    {
        private GoblinSettings _target;

        [MoonSharpHidden]
        public GoblinSettingsProxy(GoblinSettings target)
        {
            this._target = target;
        }

        public List<string> DesirableItemIDs => _target.DesirableItemIDs;
        public Dictionary<string, uint> InsultAmounts => _target.InsultAmounts;
        public List<string> MaterialIDs => _target.MaterialIDs;
    }
}

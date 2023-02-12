using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
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
        public List<string> MaterialIDs => _target.MaterialIDs;
        public Dictionary<string, uint> InsultAmounts => _target.InsultAmounts;
    }
}

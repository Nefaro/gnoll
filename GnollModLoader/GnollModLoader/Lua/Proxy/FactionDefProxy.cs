using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class FactionDefProxy
    {
        private FactionDef _target;

        [MoonSharpHidden]
        public FactionDefProxy(FactionDef target)
        {
            this._target = target;
        }
        public string Description { get => _target.Description; set => _target.Description = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string LanguageID { get => _target.LanguageID; set => _target.LanguageID = value; }
        public List<SquadDef> Squads => _target.Squads; 
        public string SubType { get => _target.SubType; set => _target.SubType = value; }
        public FactionType Type { get => _target.Type; set => _target.Type = value; }
    }
}

using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class BlueprintDefProxy
    {
        private BlueprintDef _target;

        [MoonSharpHidden]
        public BlueprintDefProxy(BlueprintDef target)
        {
            this._target = target;
        }

        public string BlueprintID { get => _target.BlueprintID; set => _target.BlueprintID = value; }
        public int Difficulty { get => _target.Difficulty; set => _target.Difficulty = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string ResearchID { get => _target.ResearchID; set => _target.ResearchID = value; }
    }
}

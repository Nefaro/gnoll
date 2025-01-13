using System.Collections.Generic;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.GameDefProxies.NewGameSettings
{
    internal class EnemyRaceGroupProxy
    {
        private EnemyRaceGroup _target;

        [MoonSharpHidden]
        public EnemyRaceGroupProxy(EnemyRaceGroup target)
        {
            this._target = target;
        }

        public string Name { get => _target.Name; set => _target.Name = value; }
        public List<string> RaceIDs => _target.RaceIDs;
    }
}

using System.Collections.Generic;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class NewGameSettingsProxy
    {
        private GameLibrary.NewGameSettings _target;

        [MoonSharpHidden]
        public NewGameSettingsProxy(GameLibrary.NewGameSettings target)
        {
            this._target = target;
        }

        public List<EnemyRaceGroup> EnemyRaceOptions => _target.EnemyRaceOptions;
        public List<DefaultProfession> DefaultProfessions => _target.DefaultProfessions;
        public List<Settler> Settlers => _target.Settlers;
        public List<FarmAnimal> FarmAnimals => _target.FarmAnimals;
        public List<ItemGenSettings> Items => _target.Items;
        public List<ContainerGenSettings> Containers => _target.Containers;
        public List<ItemGenSettings> Piles => _target.Piles;
    }
}

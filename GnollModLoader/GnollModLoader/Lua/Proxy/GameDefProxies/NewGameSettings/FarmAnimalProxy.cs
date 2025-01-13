using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.GameDefProxies.NewGameSettings
{
    internal class FarmAnimalProxy
    {
        private FarmAnimal _target;

        [MoonSharpHidden]
        public FarmAnimalProxy(FarmAnimal target)
        {
            this._target = target;
        }

        public GenderType Gender { get => _target.Gender; set => _target.Gender = value; }
        public Vector2 Offset { get => _target.Offset; set => _target.Offset = value; }
        public string RaceID { get => _target.RaceID; set => _target.RaceID = value; }
    }
}

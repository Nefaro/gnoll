using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.NewGameSettings
{
    internal class SettlerProxy
    {
        private Settler _target;

        [MoonSharpHidden]
        public SettlerProxy(Settler target)
        {
            this._target = target;
        }

        public List<ItemSettings> HeldItems => _target.HeldItems;
        public Vector2 Offset { get => _target.Offset; set => _target.Offset = value; }
        public string Profession { get => _target.Profession; set => _target.Profession = value; }
        public string RaceID { get => _target.RaceID; set => _target.RaceID = value; }
        public string TemplateMaterialID { get => _target.TemplateMaterialID; set => _target.TemplateMaterialID = value; }
    }
}

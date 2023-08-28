using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.NewGameSettings
{
    internal class ItemGenSettingsProxy
    {
        private ItemGenSettings _target;

        [MoonSharpHidden]
        public ItemGenSettingsProxy(ItemGenSettings target)
        {
            this._target = target;
        }

        public Vector2 Offset { get => _target.Offset; set => _target.Offset = value; }
    }
}

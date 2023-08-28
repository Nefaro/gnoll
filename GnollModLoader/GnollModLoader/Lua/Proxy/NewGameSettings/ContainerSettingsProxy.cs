using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;
using static GameLibrary.NewGameSettings;

namespace GnollModLoader.Lua.Proxy.NewGameSettings
{
    internal class ContainerSettingsProxy
    {
        private ContainerGenSettings _target;

        [MoonSharpHidden]
        public ContainerSettingsProxy(ContainerGenSettings target)
        {
            _target = target;
        }

        public Vector2 Offset { get => _target.Offset; set => _target.Offset = value; }
        public List<ItemSettings> Contents => _target.Contents;
    }
}

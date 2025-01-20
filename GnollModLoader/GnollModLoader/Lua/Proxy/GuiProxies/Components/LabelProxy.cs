using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using Microsoft.Xna.Framework;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class LabelProxy
    {
        private readonly Label _target;

        [MoonSharpHidden]
        public LabelProxy(Label target)
        {
            this._target = target;
        }

        public Color TextColor { get => _target.TextColor; set => _target.TextColor = value; }
    }
}

using Game.GUI.Controls;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class EventArgsProxy
    {
        private readonly EventArgs _target;

        [MoonSharpHidden]
        public EventArgsProxy(EventArgs target)
        {
            this._target = target;
        }

        public bool Handled => _target.Handled;
    }
}

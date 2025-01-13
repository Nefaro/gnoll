using Game.GUI.Controls;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class ButtonProxy
    {
        private Button _target;

        [MoonSharpHidden]
        public ButtonProxy(Button target)
        {
            this._target = target;
        }

        public event EventHandler Click
        {
            add
            {
                _target.Click += value;
            }
            remove
            {
                _target.Click -= value;
            }
        }
    }
}

using Game.GUI.Controls;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class ButtonProxy
    {
        private readonly Button _target;

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

        public void Disable() => _target.Enabled = false;

        public void Enable() => _target.Enabled = true;
        
        public bool IsEnabled() => _target.Enabled;

        public Color TextColor { get => _target.TextColor; set => _target.TextColor = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
    
    }
}

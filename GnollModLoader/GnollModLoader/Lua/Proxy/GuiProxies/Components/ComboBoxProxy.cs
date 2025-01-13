using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class ComboBoxProxy
    {
        private ComboBox _target;

        [MoonSharpHidden]
        public ComboBoxProxy(ComboBox target)
        {
            this._target = target;
        }

        public object SelectedItem()
        {
            if (this._target.ItemIndex >= 0 && this._target.Items.Count > this._target.ItemIndex)
                return this._target.Items[this._target.ItemIndex];

            return null;
        }

        public event EventHandler ItemIndexChanged
        {
            add
            {
                _target.ItemIndexChanged += value;
                _target.ItemIndex = 0; // Triggers event
            }
            remove
            {
                _target.ItemIndexChanged -= value;
            }
        }
    }
}

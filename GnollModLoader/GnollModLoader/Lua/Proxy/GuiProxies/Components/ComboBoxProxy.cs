﻿using Game.GUI.Controls;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class ComboBoxProxy
    {
        private readonly ComboBox _target;

        [MoonSharpHidden]
        public ComboBoxProxy(ComboBox target)
        {
            this._target = target;
        }

        public void Disable() => _target.Enabled = false;

        public void Enable() => _target.Enabled = true;
        public bool IsEnabled() => _target.Enabled;

        public int ItemIndex { get => _target.ItemIndex; set => _target.ItemIndex = value; }

        public object SelectedItem()
        {
            if (this._target.ItemIndex >= 0 && this._target.Items.Count > this._target.ItemIndex)
                return this._target.Items[this._target.ItemIndex];

            return null;
        }

        public int SelectedItemIndex() => _target.ItemIndex;

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

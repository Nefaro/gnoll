using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.GuiProxies.Components
{
    internal class LabelProxy
    {
        private Label _target;

        [MoonSharpHidden]
        public LabelProxy(Label target)
        {
            this._target = target;
        }
    }
}

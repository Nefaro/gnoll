using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiesProxies
{
    internal class TabbedWindowPanelProxy
    {
        private TabbedWindowPanel _target;

        [MoonSharpHidden]
        public TabbedWindowPanelProxy(TabbedWindowPanel target)
        {
            this._target = target;
        }

        public void Add(Control control) => _target.Add(control);
        

        public void AddDownFrom(Control add, Control from)
        {
            var control = _target.GetControl(from.Name);
            if (control != null)
            {
                add.Top = control.Top + control.Height + control.Margins.Horizontal;
            }

            this.Add(add);
        }
    }
}
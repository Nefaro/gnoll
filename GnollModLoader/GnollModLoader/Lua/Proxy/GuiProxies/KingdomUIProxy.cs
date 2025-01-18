using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GuiProxies
{
    internal class KingdomUIProxy
    {
        private readonly KingdomUI _target;

        [MoonSharpHidden]
        public KingdomUIProxy(KingdomUI target)
        {
            this._target = target;
        }

        public void AddPage(string pageName, TabbedWindowPanel panel) => _target.AddPage(pageName, panel);
    }
}

using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader.Lua.Proxy.EntitiesProxies;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.GuiProxies.Components;
using GnollModLoader.Lua.Proxy.GuiProxies;

namespace GnollModLoader.Lua.Proxy
{
    internal class GuiProxyRegistry
    {
        internal static void RegisterTypes()
        {
            UserData.RegisterProxyType<EventArgsProxy, EventArgs>(t => new EventArgsProxy(t));
            UserData.RegisterProxyType<MouseEventArgsProxy, MouseEventArgs>(t => new MouseEventArgsProxy(t));

            UserData.RegisterProxyType<ButtonProxy, Button>(t => new ButtonProxy(t));
            UserData.RegisterProxyType<ComboBoxProxy, ComboBox>(t => new ComboBoxProxy(t));
            UserData.RegisterProxyType<LabelProxy, Label>(t => new LabelProxy(t));
            UserData.RegisterProxyType<TabbedWindowPanelProxy, TabbedWindowPanel>(t => new TabbedWindowPanelProxy(t));

            UserData.RegisterProxyType<KingdomUIProxy, KingdomUI>(t => new KingdomUIProxy(t));
        }
    }
}

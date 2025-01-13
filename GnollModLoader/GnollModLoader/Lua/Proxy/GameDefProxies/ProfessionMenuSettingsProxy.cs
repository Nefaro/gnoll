using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ProfessionMenuSettingsProxy
    {
        private ProfessionMenuSettings _target;

        [MoonSharpHidden]
        public ProfessionMenuSettingsProxy(ProfessionMenuSettings target)
        {
            this._target = target;
        }
    }
}

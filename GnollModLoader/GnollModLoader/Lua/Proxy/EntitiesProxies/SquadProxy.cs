using Game;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.EntitiesProxies
{
    internal class SquadProxy
    {
        private Squad _target;

        [MoonSharpHidden]
        public SquadProxy(Squad target)
        {
            this._target = target;
        }

        public string Name => _target.Name;
    }
}
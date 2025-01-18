using Game;
using MoonSharp.Interpreter;
using GnollModLoader.Lua.Proxy.EntitiesProxies;

namespace GnollModLoader.Lua.Proxy.EntitiyProxies
{
    internal class SquadProxy
    {
        private readonly Squad _target;

        [MoonSharpHidden]
        public SquadProxy(Squad target)
        {
            this._target = target;
        }

        public string Name => _target.Name;

        public Character[] Members => _target.Members;

        public override  string ToString() => _target.ToString();
    }
}

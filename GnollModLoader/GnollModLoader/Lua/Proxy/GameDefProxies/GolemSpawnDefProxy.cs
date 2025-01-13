using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class GolemSpawnDefProxy
    {
        private GolemSpawnDef _target;

        [MoonSharpHidden]
        public GolemSpawnDefProxy(GolemSpawnDef target)
        {
            this._target = target;
        }

        public uint Amount { get => _target.Amount; set => _target.Amount = value; }
        public string ItemID { get => _target.ItemID; set => _target.ItemID = value; }
    }
}

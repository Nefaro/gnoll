using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class ConstructionPropertiesProxy
    {
        private ConstructionProperties _target;

        [MoonSharpHidden]
        public ConstructionPropertiesProxy(ConstructionProperties target)
        {
            this._target = target;
        }
        public bool HasFlag(ConstructionProperty flag) => _target.HasFlag(flag);
        public ConstructionProperty Flags { get => _target.Flags; set => _target.Flags = value; }
    }
}

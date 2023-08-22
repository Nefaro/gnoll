using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class AttributeDefProxy
    {
        private AttributeDef _target;

        [MoonSharpHidden]
        public AttributeDefProxy(AttributeDef target)
        {
            this._target = target;
        }

        public CharacterAttributeType Attribute { get => _target.Attribute; set => _target.Attribute = value; }
        public int Max { get => _target.Max; set => _target.Max = value; }
        public int Mean { get => _target.Mean; set => _target.Mean = value; }
        public int Min { get => _target.Min; set => _target.Min = value; }
    }
}

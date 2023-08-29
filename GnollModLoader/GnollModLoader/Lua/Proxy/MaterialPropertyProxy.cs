using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class MaterialPropertyProxy
    {
        private MaterialProperty _target;

        [MoonSharpHidden]
        public MaterialPropertyProxy(MaterialProperty target)
        {
            this._target = target;
        }
        public string GroupNameOrName() => _target.GroupNameOrName();

        public float BluntModifier { get => _target.BluntModifier; set => _target.BluntModifier = value; }
        public Vector4 Color { get => _target.Color; set => _target.Color = value; }
        public DamageProperty[] DamageProperties { get => _target.DamageProperties; set => _target.DamageProperties = value; }
        public string GroupName { get => _target.GroupName; set => _target.GroupName = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public bool OnlyAddUniqueName { get => _target.OnlyAddUniqueName; set => _target.OnlyAddUniqueName = value; }
        public float PierceModifier { get => _target.PierceModifier; set => _target.PierceModifier = value; }
        public float SlashModifier { get => _target.SlashModifier; set => _target.SlashModifier = value; }
        public float Strength { get => _target.Strength; set => _target.Strength = value; }
        public float Sustains { get => _target.Sustains; set => _target.Sustains = value; }
        public MaterialType Type { get => _target.Type; set => _target.Type = value; }
        public float Value { get => _target.Value; set => _target.Value = value; }
    }
}

using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class SkillDefProxy
    {
        private SkillDef _target;

        [MoonSharpHidden]
        public SkillDefProxy(SkillDef target)
        {
            this._target = target;
        }

        public string ID { get => _target.ID; set => _target.ID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string Title { get => _target.Title; set => _target.Title = value; }
    }
}

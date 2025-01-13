using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class UniformSettingsProxy
    {
        private Game.UniformSettings _target;

        [MoonSharpHidden]
        public UniformSettingsProxy(Game.UniformSettings target)
        {
            this._target = target;
        }

        public List<GameLibrary.UniformSettings.Uniform> DefaultUniforms => _target.DefaultUniforms;
        public Dictionary<string, List<string>> AllowedMaterialsPerItemIDs => _target.AllowedMaterialsPerItemIDs;
        public HashSet<string> AllWeapons => _target.AllWeapons;
        public HashSet<string> TwoHanded => _target.hashSet_1;
        public HashSet<string> Shield => _target.hashSet_2;
        public HashSet<string> Armor => _target.Armor;
        public List<List<string>> Any1HWeapon => _target.Any1HWeapon;
        public List<List<string>> Any2HWeapon => _target.Any2HWeapon;
    }
}

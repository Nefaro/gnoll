using System.Collections.Generic;
using MoonSharp.Interpreter;
using static GnollModLoader.SaveGameManager;

namespace GnollModLoader.Lua
{
    internal class LoaderProxy
    {
        private Loader _target;

        [MoonSharpHidden]
        public LoaderProxy(Loader target)
        {
            _target = target;
        }

        public Dictionary<string, Dictionary<object, object>> Load() => _target.Load();
    }

}

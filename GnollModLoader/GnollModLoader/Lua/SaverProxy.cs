using System.Collections.Generic;
using MoonSharp.Interpreter;
using static GnollModLoader.SaveGameManager;

namespace GnollModLoader.Lua
{
    internal class SaverProxy
    {
        private Saver _target;

        [MoonSharpHidden]
        public SaverProxy(Saver target)
        {
            _target = target;
        }

        public void Save(Dictionary<object, object> obj) => _target.Save(obj);
    }

}

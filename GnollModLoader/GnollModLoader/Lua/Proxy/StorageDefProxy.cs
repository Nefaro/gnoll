using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class StorageDefProxy
    {
        private StorageDef _target;

        [MoonSharpHidden]
        public StorageDefProxy(StorageDef target)
        {
            this._target = target;
        }
        public ItemGroup AllowedItems { get => _target.AllowedItems; set => _target.AllowedItems = value; }
        public bool RequireSame { get => _target.RequireSame; set => _target.RequireSame = value; }
        public string StorageID { get => _target.StorageID; set => _target.StorageID = value; }
    }
}

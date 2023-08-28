using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.NewGameSettings
{
    internal class ItemSettingsProxy
    {
        private GameLibrary.NewGameSettings.ItemSettings _target;

        [MoonSharpHidden]
        public ItemSettingsProxy(GameLibrary.NewGameSettings.ItemSettings target)
        {
            this._target = target;
        }

        public List<WeightedItem> Items => _target.Items;

        public string MaterialID { get => _target.MaterialID; set => _target.MaterialID = value; }
        public int Quantity { get => _target.Quantity; set => _target.Quantity = value; }
    }
}

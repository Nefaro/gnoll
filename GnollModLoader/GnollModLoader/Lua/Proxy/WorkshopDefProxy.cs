using GameLibrary;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class WorkshopDefProxy
    {
        private WorkshopDef _target;

        [MoonSharpHidden]
        public WorkshopDefProxy(WorkshopDef target)
        {
            this._target = target;
        }

        public string AudioEventClick { get => _target.AudioEventClick; set => _target.AudioEventClick = value; }
        public Rectangle Bounds { get => _target.Bounds; set => _target.Bounds = value; }
        public Vector2 ComponentsPosition { get => _target.ComponentsPosition; set => _target.ComponentsPosition = value; }
        public string ConstructionID { get => _target.ConstructionID; set => _target.ConstructionID = value; }
        public CraftableItem[] CraftableItems { get => _target.CraftableItems; set => _target.CraftableItems = value; }
        public Vector2 CraftPosition { get => _target.CraftPosition; set => _target.CraftPosition = value; }
        public string Description { get => _target.Description; set => _target.Description = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public int MaxCapacity { get => _target.MaxCapacity; set => _target.MaxCapacity = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string ResearchID { get => _target.ResearchID; set => _target.ResearchID = value; }
        public Vector2 StockPosition { get => _target.StockPosition; set => _target.StockPosition = value; }
        public WorkshopTile[] Tiles { get => _target.Tiles; set => _target.Tiles = value; }
        public float Value { get => _target.Value; set => _target.Value = value; }
    }
}

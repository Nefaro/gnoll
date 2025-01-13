using System.Collections.Generic;
using GameLibrary;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy.GameDefProxies
{
    internal class ConstructionDefProxy
    {
        private ConstructionDef _target;

        [MoonSharpHidden]
        public ConstructionDefProxy(ConstructionDef target)
        {
            this._target = target;
        }

        public string BlueprintID { get => _target.BlueprintID; set => _target.BlueprintID = value; }
        public bool BuildAdjacent { get => _target.BuildAdjacent; set => _target.BuildAdjacent = value; }
        public ItemComponent[] Components { get => _target.Components; set => _target.Components = value; }
        public string Description { get => _target.Description; set => _target.Description = value; }
        public bool Destructible { get => _target.Destructible; set => _target.Destructible = value; }
        public List<ItemEffect> Effects => _target.Effects;
        public string GroupName { get => _target.GroupName; set => _target.GroupName = value; }
        public string ID { get => _target.ID; set => _target.ID = value; }
        public string Name { get => _target.Name; set => _target.Name = value; }
        public string Prefix { get => _target.Prefix; set => _target.Prefix = value; }
        public ConstructionProperties Properties { get => _target.Properties; set => _target.Properties = value; }
        public bool RequiresFloor { get => _target.RequiresFloor; set => _target.RequiresFloor = value; }
        public bool RequiresSupportWall { get => _target.RequiresSupportWall; set => _target.RequiresSupportWall = value; }
        public bool Rotates { get => _target.Rotates; set => _target.Rotates = value; }
        public float Thickness { get => _target.Thickness; set => _target.Thickness = value; }
        public string ToolTip { get => _target.ToolTip; set => _target.ToolTip = value; }
        public float Value { get => _target.Value; set => _target.Value = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using Microsoft.Xna.Framework.Content;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua
{
    internal class WorkshopSettingsProxy
    {
        private WorkshopSettings _target;

        [MoonSharpHidden]
        public WorkshopSettingsProxy(WorkshopSettings target)
        {
            this._target = target;
        }

        public string ButcherShopID { get => _target.ButcherShopID; set => _target.ButcherShopID = value; }
        public Dictionary<string, string> ConstructionIDToWorkshopID => _target.ConstructionIDToWorkshopID;
        public Dictionary<string, List<WeightedMaterial>> CoreItemIDsToMaterialIDs => _target.CoreItemIDsToMaterialIDs;
        public string MarketStallID { get => _target.MarketStallID; set => _target.MarketStallID = value; }
        public string ProspectorID { get => _target.ProspectorID; set => _target.ProspectorID = value; }
        public string SmelterID { get => _target.SmelterID; set => _target.SmelterID = value; }
        public string SmeltItemID { get => _target.SmeltItemID; set => _target.SmeltItemID = value; }
        public string TinkerBenchID { get => _target.TinkerBenchID; set => _target.TinkerBenchID = value; }
        public string TrainingGroundsID { get => _target.TrainingGroundsID; set => _target.TrainingGroundsID = value; }
    }
}

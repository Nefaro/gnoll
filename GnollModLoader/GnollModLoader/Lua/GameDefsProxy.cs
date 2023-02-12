using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GameLibrary;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace GnollModLoader.Lua
{
    internal class GameDefsProxy
    {
        private GameDefs _target;

        [MoonSharpHidden]
        public GameDefsProxy(GameDefs target) 
        { 
            this._target = target;
        }
        public Dictionary<string, AmmoDef> AmmoDefs => _target.dictionary_9;
        public Dictionary<string, BlueprintDef> BlueprintDefs => _target.BlueprintDefs;
        public Dictionary<string, BodyDef> BodyDefs => _target.BodyDefs;
        public Dictionary<string, FactionDef> FactionDefs => _target.FactionDefs;
        public Dictionary<string, ItemDef> ItemDefs => _target.ItemDefs;
        public Dictionary<string, LiquidDef> LiquidDefs => _target.LiquidDefs;
        public Dictionary<string, RaceDef> RaceDefs => _target.RaceDefs;
        public Dictionary<string, ResearchDef> ResearchDefs => _target.ResearchDefs;
        public Dictionary<string, SkillDef> SkillDefs => _target.SkillDefs;
        public Dictionary<string, WorkshopDef> WorkshopDefs => _target.WorkshopDefs;

        public Dictionary<string, MaterialProperty> Materials => _target.Materials;

        public AutomatonSettings AutomatonSettings => _target.AutomatonSettings;
        public CharacterSettings CharacterSettings =>_target.CharacterSettings;
        public GoblinSettings GoblinSettings => _target.GoblinSettings;
        public GolemSettings GolemSettings => _target.GolemSettings;
        public GrassSettings GrassSettings => _target.GrassSettings;
        public Game.ItemSettings ItemSettings => _target.ItemSettings;
        public Game.JobSettings JobSettings => _target.JobSettings;
        public LiquidSettings LiquidSettings => _target.LiquidSettings;
        public MantSettings MantSettings =>  _target.MantSettings;
        public Game.MechanismSettings MechanismSettings => _target.MechanismSettings;
        public NewGameSettings NewGameSettings => _target.NewGameSettings;
        public PlantSettings PlantSettings => _target.PlantSettings;
        public ProfessionMenuSettings ProfessionMenuSettin => _target.ProfessionMenuSettings;
        public ProspectorSettings ProspectorSettings => _target.ProspectorSettings;
        public RightClickMenuSettings RightClickMenuSettings => _target.RightClickMenuSettings;
        public StockMenuSettings StockMenuSettings =>_target.StockMenuSettings;
        public Game.TerrainSettings TerrainSettings => _target.TerrainSettings;
        public Game.UniformSettings UniformSettings => _target.UniformSettings;
        public WorkshopSettings WorkshopSettings => _target.WorkshopSettings;
    }
}

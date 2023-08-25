using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.GUI.Controls;
using Game.GUI;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using GameLibrary;
using System.Reflection;
using System.IO;
using GnollModLoader.Lua;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using static GameLibrary.PlantDef;
using static GnollModLoader.SaveGameManager;

namespace GnollModLoader
{
    internal class SaverProxy
    {
        private Saver _target;

        [MoonSharpHidden]
        public SaverProxy(Saver target)
        {
            this._target = target;
        }

        public void Save(Dictionary<object, object> obj) => _target.Save(obj);
    }

    internal class LoaderProxy
    {
        private Loader _target;

        [MoonSharpHidden]
        public LoaderProxy(Loader target)
        {
            this._target = target;
        }

        public Dictionary<object, object> Load() => _target.Load();
    }

    public class LuaManager
    {
        private readonly static string SCRIPT_DIR_NAME = "Scripts";
        private readonly static CoreModules DEFAULT_CORE_MODULES = CoreModules.Preset_HardSandbox |
                CoreModules.Json |
                CoreModules.ErrorHandling |
                CoreModules.Coroutine |
                CoreModules.OS_Time |
                CoreModules.LoadMethods |
                CoreModules.Metatables;

        private readonly Dictionary<String, Tuple<String,Script>> _registry = new Dictionary<String, Tuple<String, Script>> ();

        private HookManager _hookManager;
        private SaveGameManager _saveGameManager;
        public LuaManager(HookManager hookManager, SaveGameManager saveGameManager) 
        {
            this._hookManager = hookManager;
            this._saveGameManager = saveGameManager;
            this.init();
        }

        public void init()
        {
            var loader = Script.DefaultOptions.ScriptLoader;
            // Redirect lua "print" to our log console
            Script.DefaultOptions.DebugPrint = s => { LuaLogger.Log(Newtonsoft.Json.JsonConvert.ToString(s)); };

            UserData.RegisterProxyType<GameDefsProxy, GameDefs>(t => new GameDefsProxy(t));

            UserData.RegisterProxyType<AmmoDefProxy, AmmoDef>(t => new AmmoDefProxy(t));
            UserData.RegisterProxyType<AttributeDefProxy, AttributeDef>(t => new AttributeDefProxy(t));
            UserData.RegisterProxyType<AttackDefProxy, AttackDef>(t => new AttackDefProxy(t));
            UserData.RegisterProxyType<AutomatonSettingsProxy, AutomatonSettings>(t => new AutomatonSettingsProxy(t));
            UserData.RegisterProxyType<BlueprintDefProxy, BlueprintDef>(t => new BlueprintDefProxy(t));
            UserData.RegisterProxyType<BodyDefProxy, BodyDef>(t => new BodyDefProxy(t));
            UserData.RegisterProxyType<BodyPartDefProxy, BodyPartDef>(t => new BodyPartDefProxy(t));
            UserData.RegisterProxyType<BodySectionDefProxy, BodySectionDef>(t => new BodySectionDefProxy(t));
            UserData.RegisterProxyType<BodySectionTilesDefProxy, BodySectionTilesDef>(t => new BodySectionTilesDefProxy(t));
            UserData.RegisterProxyType<BodySectionTileDetailsProxy, BodySectionTileDetails>(t => new BodySectionTileDetailsProxy(t));
            UserData.RegisterProxyType<BodySectionTileDefProxy, BodySectionTileDef>(t => new BodySectionTileDefProxy(t));
            UserData.RegisterProxyType<ByproductProxy, Byproduct>(t => new ByproductProxy(t));
            UserData.RegisterProxyType<CharacterSettingsProxy, CharacterSettings>(t => new CharacterSettingsProxy(t));
            UserData.RegisterProxyType<ConstructionDefProxy, ConstructionDef>(t => new ConstructionDefProxy(t));
            UserData.RegisterProxyType<ConstructionPropertiesProxy, ConstructionProperties>(t => new ConstructionPropertiesProxy(t));
            UserData.RegisterProxyType<CraftableItemProxy, CraftableItem>(t => new CraftableItemProxy(t));
            UserData.RegisterProxyType<DamagePropertyProxy, DamageProperty>(t => new DamagePropertyProxy(t));
            UserData.RegisterProxyType<DefendDefProxy, DefendDef>(t => new DefendDefProxy(t));
            UserData.RegisterProxyType<FactionDefProxy, FactionDef>(t => new FactionDefProxy(t));
            UserData.RegisterProxyType<FarmedAnimalItemDefProxy, FarmedAnimalItemDef>(t => new FarmedAnimalItemDefProxy(t));
            UserData.RegisterProxyType<GenderDefProxy, GenderDef>(t => new GenderDefProxy(t));
            UserData.RegisterProxyType<GoblinSettingsProxy, GoblinSettings>(t => new GoblinSettingsProxy(t));
            UserData.RegisterProxyType<GolemSettingsProxy, GolemSettings>(t => new GolemSettingsProxy(t));
            UserData.RegisterProxyType<GolemSpawnDefProxy, GolemSpawnDef>(t => new GolemSpawnDefProxy(t));
            UserData.RegisterProxyType<GrassSettingsProxy, GrassSettings>(t => new GrassSettingsProxy(t));
            UserData.RegisterProxyType<ItemComponentProxy, ItemComponent>(t => new ItemComponentProxy(t));
            UserData.RegisterProxyType<ItemDefProxy, ItemDef>(t => new ItemDefProxy(t));
            UserData.RegisterProxyType<ItemGroupProxy, ItemGroup>(t => new ItemGroupProxy(t));
            UserData.RegisterProxyType<ItemSettingsProxy, Game.ItemSettings>(t => new ItemSettingsProxy(t));
            UserData.RegisterProxyType<JobSettingsProxy, Game.JobSettings>(t => new JobSettingsProxy(t));
            UserData.RegisterProxyType<JobSettingProxy, GameLibrary.JobSetting>(t => new JobSettingProxy(t));
            UserData.RegisterProxyType<LiquidDefProxy, LiquidDef>(t => new LiquidDefProxy(t));
            UserData.RegisterProxyType<LiquidSettingsProxy, LiquidSettings>(t => new LiquidSettingsProxy(t));
            UserData.RegisterProxyType<MantSettingsProxy, MantSettings>(t => new MantSettingsProxy(t));
            UserData.RegisterProxyType<MaterialPropertyProxy, MaterialProperty>(t => new MaterialPropertyProxy(t));
            UserData.RegisterProxyType<MechanismDefProxy, MechanismDef>(t => new MechanismDefProxy(t));
            UserData.RegisterProxyType<MechanismSettingsProxy, Game.MechanismSettings>(t => new MechanismSettingsProxy(t));
            UserData.RegisterProxyType<NewGameSettingsProxy, NewGameSettings>(t => new NewGameSettingsProxy(t));
            UserData.RegisterProxyType<NaturalWeaponDefProxy, NaturalWeaponDef>(t => new NaturalWeaponDefProxy(t));
            UserData.RegisterProxyType<PlantDefProxy, PlantDef>(t => new PlantDefProxy(t));
            UserData.RegisterProxyType<PlantSettingsProxy, PlantSettings>(t => new PlantSettingsProxy(t));
            UserData.RegisterProxyType<ProfessionMenuSettingsProxy, ProfessionMenuSettings>(t => new ProfessionMenuSettingsProxy(t));
            UserData.RegisterProxyType<ProspectorSettingsProxy, ProspectorSettings>(t => new ProspectorSettingsProxy(t));
            UserData.RegisterProxyType<RaceClassDefProxy, RaceClassDef>(t => new RaceClassDefProxy(t));
            UserData.RegisterProxyType<RaceDefProxy, RaceDef>(t => new RaceDefProxy(t));
            UserData.RegisterProxyType<ResearchDefProxy, ResearchDef>(t => new ResearchDefProxy(t));
            UserData.RegisterProxyType<ScaledSkillProxy, ScaledSkill>(t => new ScaledSkillProxy(t));
            UserData.RegisterProxyType<SkillDefProxy, SkillDef>(t => new SkillDefProxy(t));
            UserData.RegisterProxyType<SquadDefProxy, SquadDef>(t => new SquadDefProxy(t));
            UserData.RegisterProxyType<StartingItemDefProxy, StartingItemDef>(t => new StartingItemDefProxy(t));
            UserData.RegisterProxyType<StartingItemProxy, StartingItem>(t => new StartingItemProxy(t));
            UserData.RegisterProxyType<StartingSkillDefProxy, StartingSkillDef>(t => new StartingSkillDefProxy(t));
            UserData.RegisterProxyType<StorageDefProxy, StorageDef>(t => new StorageDefProxy(t));
            UserData.RegisterProxyType<TargetedAttackDefProxy, TargetedAttackDef>(t => new TargetedAttackDefProxy(t));
            UserData.RegisterProxyType<TerrainSettingsProxy, Game.TerrainSettings>(t => new TerrainSettingsProxy(t));
            UserData.RegisterProxyType<ToolSettingsProxy,ToolSettings>(t => new ToolSettingsProxy(t));
            UserData.RegisterProxyType<TradeGoodProxy, TradeGood>(t => new TradeGoodProxy(t));
            UserData.RegisterProxyType<TradeModifierProxy, TradeModifier>(t => new TradeModifierProxy(t));
            UserData.RegisterProxyType<TrapDefProxy, TrapDef>(t => new TrapDefProxy(t));
            UserData.RegisterProxyType<UniformSettingsProxy, Game.UniformSettings>(t => new UniformSettingsProxy(t));
            UserData.RegisterProxyType<WeightedColorProxy, WeightedColor>(t => new WeightedColorProxy(t));
            UserData.RegisterProxyType<WeightedItemProxy, WeightedItem>(t => new WeightedItemProxy(t));
            UserData.RegisterProxyType<WeightedMaterialProxy, WeightedMaterial>(t => new WeightedMaterialProxy(t));
            UserData.RegisterProxyType<WeaponDefProxy, WeaponDef>(t => new WeaponDefProxy(t));
            UserData.RegisterProxyType<WornEquipmentDefProxy, WornEquipmentDef>(t => new WornEquipmentDefProxy(t));
            UserData.RegisterProxyType<WorkshopDefProxy, WorkshopDef>(t => new WorkshopDefProxy(t));
            UserData.RegisterProxyType<WorkshopSettingsProxy, WorkshopSettings>(t => new WorkshopSettingsProxy(t));
            UserData.RegisterProxyType<WorkshopTilePartProxy, WorkshopTilePart>(t => new WorkshopTilePartProxy(t));
            UserData.RegisterProxyType<WorkshopTileProxy, WorkshopTile>(t => new WorkshopTileProxy(t));

            UserData.RegisterProxyType<Lua.NewGameSettings.ContainerSettingsProxy, NewGameSettings.ContainerGenSettings>(t => new Lua.NewGameSettings.ContainerSettingsProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.DefaultProfessionProxy, NewGameSettings.DefaultProfession>(t => new Lua.NewGameSettings.DefaultProfessionProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.EnemyRaceGroupProxy, NewGameSettings.EnemyRaceGroup>(t => new Lua.NewGameSettings.EnemyRaceGroupProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.FarmAnimalProxy, NewGameSettings.FarmAnimal>(t => new Lua.NewGameSettings.FarmAnimalProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.ItemGenSettingsProxy, NewGameSettings.ItemGenSettings>(t => new Lua.NewGameSettings.ItemGenSettingsProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.ItemSettingsProxy, NewGameSettings.ItemSettings>(t => new Lua.NewGameSettings.ItemSettingsProxy(t));
            UserData.RegisterProxyType<Lua.NewGameSettings.SettlerProxy, NewGameSettings.Settler>(t => new Lua.NewGameSettings.SettlerProxy(t));

            UserData.RegisterProxyType<Lua.PlantDef.HarvestedItemProxy, PlantDef.HarvestedItem>(t => new Lua.PlantDef.HarvestedItemProxy(t));

            UserData.RegisterProxyType<Lua.TerraingSettings.GrowthSettingsProxy, GameLibrary.TerrainSettings.GrowthSettings>(t => new Lua.TerraingSettings.GrowthSettingsProxy(t));

            UserData.RegisterProxyType<Lua.UniformSettings.UniformProxy, GameLibrary.UniformSettings.Uniform>(t => new Lua.UniformSettings.UniformProxy(t));

            UserData.RegisterType<Vector4>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<Vector2>();

            UserData.RegisterType<Game.Character>();
            UserData.RegisterType<Game.GameEntity>();

            // hiding the internal functionality
            UserData.RegisterProxyType<SaverProxy, Saver>(t => new SaverProxy(t));
            UserData.RegisterProxyType<LoaderProxy, Loader>(t => new LoaderProxy(t));

            this._hookManager.OnEntitySpawn += (Game.GameEntity entity) =>
            {
                //Logger.Log($"Entity type {entity.Name()}");
                /*
                var func = script.Globals["OnEntitySpawn"];
                if (func != null )
                {
                    //Logger.Log($"Calling OnEntitySpawn for: {entity.Name()}");
                    script.Call(func, entity);
                }*/
            };

            this._hookManager.BeforeStartNewGameAfterReadDefs += this.hookLuaOnGameDefsLoaded;
            this._hookManager.AfterGameLoaded += this.hookLuaOnGameLoad;
            this._hookManager.AfterGameSaved += this.hookLuaOnGameSave;
        }


        public void HookInGameHudInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            if (GnollMain.Debug)
            {
                Logger.Log("Attaching Lua buttons");
                this.AttachIngameUI(inGameHUD, manager);
            }
        }

        private void hookLuaOnGameDefsLoaded(Game.CreateWorldOptions options)
        {
            Logger.Log("Hooking: hookLuaOnGameDefsLoaded");
            foreach (var entry in this._registry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(script, "OnGameDefinitionsInitialized", GnomanEmpire.Instance.GameDefs);
            }
        }
        private void hookLuaOnGameLoad()
        {
            Logger.Log("Hooking: After save game loaded");
            foreach (var entry in this._registry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(script, "OnSaveGameLoaded", _saveGameManager.LoaderForMod(entry.Key));
            }
            Logger.Log("Hooking: After save game definitions loaded");
            foreach (var entry in this._registry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(script, "OnDefinitionsLoaded", GnomanEmpire.Instance.GameDefs);
            }
        }

        private void hookLuaOnGameSave()
        {
            foreach (var entry in this._registry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(script, "OnGameSave", _saveGameManager.SaverForMod(entry.Key));
            }
        }

        private void runValidationScripts()
        {
            foreach (var entry in this._registry)
            {
                var script = entry.Value.Item2;
                runLuaFunction(script, "OnRunScriptValidation", GnomanEmpire.Instance.GameDefs);
            }
        }

        private static void runLuaFunction(Script script, string functionName, params object[] args)
        {
            if (script == null)
            {
                return;
            }

            try
            {
                var func = script.Globals[functionName];
                if (func != null)
                {
                    Logger.Log($"Call {functionName}");
                    script.Call(func, args);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"LUA Error: {ex}");
            }
        }

        private void reloadAllScripts()
        {
            foreach (string key in this._registry.Keys.ToList())
            {
                var value = this._registry[key];
                try
                {
                    var initScriptPath = value.Item1;
                    Logger.Log($"-- Trying to reload Lua script: {initScriptPath}");
                    var script = loadAndGetScript(initScriptPath);
                    if ( script != null )
                    {
                        this._registry[key] = new Tuple<string, Script>(initScriptPath, script);
                    }
                }
                catch (Exception ex)
                {
                    this._registry[key] = new Tuple<string, Script>(value.Item1, null);
                    Logger.Error($"Reloading script for '{key}' failed");
                    Logger.Error($"-- {ex}");
                }
            }
        }

        private Script loadAndGetScript(string scriptPath)
        {
            try { 
                Script script = new Script(DEFAULT_CORE_MODULES);
                /*
                ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[] {
                    Path.GetDirectoryName(scriptPath) + "\\?",
                    Path.GetDirectoryName(scriptPath) + "\\?.lua" };
                */
                ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = new string[] {
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\?",
                    Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\?.lua" };                

                Logger.Log("Module paths: ");
                foreach (string path in ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths)
                {
                    Logger.Log($" -- {path}");
                }

                script.DoFile(scriptPath);
                return script;
            }
            catch(Exception ex)
            {
                Logger.Error($"Init script with path '{scriptPath}' failed");
                Logger.Error($"-- {ex}");
                return null;
            }
        }

        private void AttachIngameUI(InGameHUD inGameHUD, Manager manager)
        {
            GClass0 glass = inGameHUD.GClass0_0;
            Button button7 = glass.method_40("Lua Reload", "Reload Lua scripts", new Game.GUI.Controls.EventHandler((sender, events) =>
            {
                this.reloadAllScripts();
                Logger.Log($"-- -- Lua Scripts reloaded -- -- ");
            }));
            glass.panel_0.Add(button7);
            button7.Left = glass.panel_0.Width + button7.Margins.Left;
            glass.panel_0.Width = button7.Left + button7.Width;
            glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;

            Button button8 = glass.method_40("Lua Validate", "Run Lua validation scripts", new Game.GUI.Controls.EventHandler((sender, events) =>
            {
                this.runValidationScripts();
                Logger.Log($"-- -- Lua Validation done -- -- ");
            }));
            glass.panel_0.Add(button8);
            button8.Left = glass.panel_0.Width + button8.Margins.Left;
            glass.panel_0.Width = button8.Left + button8.Width;
            glass.panel_0.Left = (glass.Width - glass.panel_0.Width) / 2;
        }

        internal void RegisterMod(IGnollMod mod, Assembly modAssembly)
        {
            try
            {
                //var initScript = this.generatePathForMod(modAssembly, SCRIPT_DIR_NAME) + "\\OnModInit.lua";
                var initScript = Environment.GetEnvironmentVariable("GNOLL_WORKSPACE") + "\\Gnoll Mods\\ExpLuaIntegration\\ExpLuaIntegration\\Scripts\\OnModInit.lua";
                Logger.Log($"-- Trying to load Lua init script: {initScript}");
                if (System.IO.File.Exists(initScript))
                {
                    var script = this.loadAndGetScript(initScript);
                    this._registry[mod.Name] = new Tuple<string, Script>(initScript, script);
                    if (script != null)
                    {
                        Logger.Log($"-- Init script for '{mod.Name}' loaded successfully");
                    }
                    else
                    {
                        Logger.Error($"Init script for mod '{mod.Name}' failed");
                    }
                }
                else
                {
                    Logger.Warn($"Mod registered for script loading but init script missing: {initScript}");
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"Init script for mod '{mod.Name}' failed");
                Logger.Error($"-- {ex}");
            }
        }
        internal void UnloadMod(IGnollMod mod)
        {
            try
            {
                Logger.Log($"-- Unloading mod '{mod.Name}' from script handler");
                this._registry.Remove(mod.Name);
            }
            catch(Exception ex)
            {
                Logger.Error($"Unloading mod '{mod.Name}' failed");
                Logger.Error($"-- {ex}");
            }
        }

        private string generatePathForMod(Assembly modAssembly, string subDirectory)
        {
            string assembly = Path.GetDirectoryName(modAssembly.Location);
            string dll = modAssembly.GetName().Name;
            string folder = assembly + "\\" + dll + "\\" + subDirectory;
            return folder;
        }
    }

}

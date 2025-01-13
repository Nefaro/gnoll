using Game;
using GameLibrary;
using GnollModLoader.Lua.Proxy.GameDefProxies;
using MoonSharp.Interpreter;

namespace GnollModLoader.Lua.Proxy
{
    internal class GameDefsProxyRegistry
    {
        internal static void RegisterTypes()
        {
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
            UserData.RegisterProxyType<JobSettingProxy, JobSetting>(t => new JobSettingProxy(t));
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
            UserData.RegisterProxyType<ToolSettingsProxy, ToolSettings>(t => new ToolSettingsProxy(t));
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

            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.ContainerSettingsProxy, NewGameSettings.ContainerGenSettings>(t => new GameDefProxies.NewGameSettings.ContainerSettingsProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.DefaultProfessionProxy, NewGameSettings.DefaultProfession>(t => new GameDefProxies.NewGameSettings.DefaultProfessionProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.EnemyRaceGroupProxy, NewGameSettings.EnemyRaceGroup>(t => new GameDefProxies.NewGameSettings.EnemyRaceGroupProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.FarmAnimalProxy, NewGameSettings.FarmAnimal>(t => new GameDefProxies.NewGameSettings.FarmAnimalProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.ItemGenSettingsProxy, NewGameSettings.ItemGenSettings>(t => new GameDefProxies.NewGameSettings.ItemGenSettingsProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.ItemSettingsProxy, NewGameSettings.ItemSettings>(t => new GameDefProxies.NewGameSettings.ItemSettingsProxy(t));
            UserData.RegisterProxyType<GameDefProxies.NewGameSettings.SettlerProxy, NewGameSettings.Settler>(t => new GameDefProxies.NewGameSettings.SettlerProxy(t));

            UserData.RegisterProxyType<GameDefProxies.PlantDef.HarvestedItemProxy, PlantDef.HarvestedItem>(t => new GameDefProxies.PlantDef.HarvestedItemProxy(t));

            UserData.RegisterProxyType<GameDefProxies.TerraingSettings.GrowthSettingsProxy, GameLibrary.TerrainSettings.GrowthSettings>(t => new GameDefProxies.TerraingSettings.GrowthSettingsProxy(t));

            UserData.RegisterProxyType<GameDefProxies.UniformSettings.UniformProxy, GameLibrary.UniformSettings.Uniform>(t => new GameDefProxies.UniformSettings.UniformProxy(t));

            UserData.RegisterProxyType<CreateWorldOptionsProxy, CreateWorldOptions>(t => new CreateWorldOptionsProxy(t));
        }
    }
}

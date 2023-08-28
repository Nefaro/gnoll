print("Hello from ScriptValidation IMPORT")

function OnRunScriptValidation()
    print "Running Validation Script"
    
    if ( _GNOMORIA == nil ) then
        print("!! Validation failure: _GNOMORIA table is missing; cannot continue")
        return
    end
    
    gameDefs = _GNOMORIA.getGameDefs()
    if ( gameDefs == nil ) then
        print("!! Validation failure: GameDefs in _GNOMORIA table is missing; cannot continue")
        return
    end    
    
    print "Validating 'gameDefs' members ... "
    local tbl = nil;
    tbl = gameDefs.AmmoDefs;
    tbl = gameDefs.BlueprintDefs;
    tbl = gameDefs.BodyDefs;
    tbl = gameDefs.BodyPartDefs;
    tbl = gameDefs.ConstructionDef;
    tbl = gameDefs.FactionDefs;
    tbl = gameDefs.ItemDefs;
    tbl = gameDefs.LiquidDefs;
    tbl = gameDefs.MechanismDefs;
    tbl = gameDefs.PlantDefs;
    tbl = gameDefs.RaceDefs;
    tbl = gameDefs.ResearchDefs;
    tbl = gameDefs.SkillDefs;
    tbl = gameDefs.StorageDefs;
    tbl = gameDefs.TrapDefs;
    tbl = gameDefs.WorkshopDefs;

    tbl = gameDefs.Materials;
    tbl = gameDefs.AutomatonSettings;
    tbl = gameDefs.CharacterSettings;
    tbl = gameDefs.GoblinSettings;
    tbl = gameDefs.GolemSettings;
    tbl = gameDefs.GrassSettings;
    tbl = gameDefs.ItemSettings;
    tbl = gameDefs.JobSettings;
    tbl = gameDefs.LiquidSettings;
    tbl = gameDefs.MantSettings;
    tbl = gameDefs.MechanismSettings;
    tbl = gameDefs.NewGameSettings;
    tbl = gameDefs.PlantSettings;
    tbl = gameDefs.ProspectorSettings;
    tbl = gameDefs.TerrainSettings;
    tbl = gameDefs.UniformSettings;
    tbl = gameDefs.WorkshopSettings;
    
    print "Validating other definition members ... "
    
    tbl = gameDefs.BodyDefs;
    for k, v in pairs(tbl) do
        tbl = v.AdditionalEquipmentSlots;
        tbl = v.AdditionalEquipmentSlots[1];
        tbl = v.FarmedItems;
        tbl = v.FarmedItems[1];
        tbl = v.MainBody;
        tbl = v.MainBody.ConnectedSections;
        if ( tbl ~= nil ) then 
            tbl = v.MainBody.ConnectedSections[1];
        end
            
        tbl = v.MainBody.Tile;
        if ( tbl ~= nil ) then
            tbl = v.MainBody.Tile.BodyTiles;
            tbl = v.MainBody.Tile.FemaleDecorations;
            tbl = v.MainBody.Tile.FemaleDecorations[1];
            tbl = v.MainBody.Tile.MaleDecorations;
            tbl = v.MainBody.Tile.MaleDecorations[1];        
            tbl = v.MainBody.Tile.NeuterDecorations;
            tbl = v.MainBody.Tile.NeuterDecorations[1];
            if ( tbl ~= nil ) then 
                tbl = v.MainBody.Tile.NeuterDecorations[1].TileDetails;
                if ( tbl ~= nil ) then
                    tbl = v.MainBody.Tile.NeuterDecorations[1].TileDetails[1];
                    if ( tbl ~=  nil ) then 
                        tbl = v.MainBody.Tile.NeuterDecorations[1].TileDetails[1].Colors;
                        tbl = v.MainBody.Tile.NeuterDecorations[1].TileDetails[1].Colors[1];
                    end
                end
            end
        end
    end
        
    tbl = gameDefs.BodyPartDefs;
    for k, v in pairs(tbl) do
        if ( v ~= nil ) then 
            tbl = v.NaturalWeapon;
            if ( tbl ~= nil ) then
                tbl = v.NaturalWeapon.WeaponDef;
                tbl = v.NaturalWeapon.WeaponDef.AttackMoves;
                tbl = v.NaturalWeapon.WeaponDef.AttackMoves[1];
                tbl = v.NaturalWeapon.WeaponDef.DefendMoves;
                tbl = v.NaturalWeapon.WeaponDef.DefendMoves[1];
                tbl = v.NaturalWeapon.WeaponDef.TargetedAttackMoves;
                tbl = v.NaturalWeapon.WeaponDef.TargetedAttackMoves[1];
            end
        end
    end

    tbl = gameDefs.ConstructionDef;
    for k, v in pairs(tbl) do
        tbl = v.Components;
        tbl = v.Components[1];
        tbl = v.Properties;
    end        

    tbl = gameDefs.FactionDefs;
    for k, v in pairs(tbl) do
        tbl = v.Squads;
        tbl = v.Squads[1];
    end  
    
    tbl = gameDefs.PlantDefs;
    for k, v in pairs(tbl) do
        tbl = v.HarvestedItems;
        tbl = v.HarvestedItems[1];
    end          
    
    tbl = gameDefs.RaceDefs;
    for k, v in pairs(tbl) do
        tbl = v.Attributes;
        tbl = v.Attributes[1];            
        tbl = v.Genders;
        tbl = v.Genders[1];
    end     
    
    tbl = gameDefs.StorageDefs;
    for k, v in pairs(tbl) do
        tbl = v.AllowedItems;
    end
    
    tbl = gameDefs.WorkshopDefs;
    for k, v in pairs(tbl) do
        tbl = v.CraftableItems;
        tbl = v.CraftableItems.Byproducts;
        if ( tbl ~= nil ) then 
            tbl = v.CraftableItems.Byproducts[1];
        end
        tbl = v.CraftableItems.Components;
        if ( tbl ~= nil ) then 
            tbl = v.CraftableItems.Components[1];
        end            
        tbl = v.Tiles;
        tbl = v.Tiles[1];
        tbl = v.Tiles[1].TileParts;
        tbl = v.Tiles[1].TileParts[1];
    end
    
    tbl = gameDefs.Materials;
    for k, v in pairs(tbl) do
        tbl = v.DamageProperties;
        tbl = v.DamageProperties[1];
    end
    
    tbl = gameDefs.CharacterSettings;
    tbl = gameDefs.CharacterSettings.DefaultTradeModifiers;
    tbl = gameDefs.CharacterSettings.DefaultTradeModifiers[1];
 
    tbl = gameDefs.GolemSettings;
    tbl = gameDefs.GolemSettings.GolemSpawnDefs;
    tbl = gameDefs.GolemSettings.GolemSpawnDefs[1];
 
    tbl = gameDefs.ItemSettings;
    tbl = gameDefs.ItemSettings.DefaultWeapon;
    tbl = gameDefs.ItemSettings.WornEquipment;
    tbl = gameDefs.ItemSettings.Tools;

    tbl = gameDefs.JobSettings;
    tbl = gameDefs.JobSettings.JobSettingPerJobType;
    for k, v in pairs(tbl) do
        tbl = v.AdditionalSkillIDs;
        tbl = v.AdditionalSkillIDs[1];
    end
 
    tbl = gameDefs.NewGameSettings.EnemyRaceOptions;
    tbl = gameDefs.NewGameSettings.EnemyRaceOptions[1];
    tbl = gameDefs.NewGameSettings.DefaultProfessions;
    tbl = gameDefs.NewGameSettings.DefaultProfessions[1];
    tbl = gameDefs.NewGameSettings.Settlers;
    tbl = gameDefs.NewGameSettings.Settlers[1];
    tbl = gameDefs.NewGameSettings.Settlers[1].HeldItems;
    tbl = gameDefs.NewGameSettings.Settlers[1].HeldItems[1];
    tbl = gameDefs.NewGameSettings.Settlers[1].HeldItems[1].Items;
    tbl = gameDefs.NewGameSettings.Settlers[1].HeldItems[1].Items[1];
    tbl = gameDefs.NewGameSettings.FarmAnimals;
    tbl = gameDefs.NewGameSettings.FarmAnimals[1];
    tbl = gameDefs.NewGameSettings.Items;
    tbl = gameDefs.NewGameSettings.Items[1];
    tbl = gameDefs.NewGameSettings.Containers;
    tbl = gameDefs.NewGameSettings.Containers[1];
    tbl = gameDefs.NewGameSettings.Containers[1].Contents;
    tbl = gameDefs.NewGameSettings.Containers[1].Contents[1];
 
    tbl = gameDefs.ProspectorSettings.WeightedMaterialIDsByMaterialType;
    for k, v in pairs(tbl) do
        tbl = v;
        tbl = v[1];
    end
    
    tbl = gameDefs.TerrainSettings;
    tbl = gameDefs.TerrainSettings.Minerals;
    tbl = gameDefs.TerrainSettings.Minerals[1];
    tbl = gameDefs.TerrainSettings.Minerals[1].WeightedMaterialIDs;
    tbl = gameDefs.TerrainSettings.Minerals[1].WeightedMaterialIDs[1];
    tbl = gameDefs.TerrainSettings.Plants;
    tbl = gameDefs.TerrainSettings.Plants[1];
    tbl = gameDefs.TerrainSettings.Trees;
    tbl = gameDefs.TerrainSettings.Trees[1];
 
    tbl = gameDefs.UniformSettings;
    tbl = gameDefs.UniformSettings.DefaultUniforms;
    for k, v in pairs(tbl) do
        tbl = v;
    end

    tbl = gameDefs.WorkshopSettings;
    tbl = gameDefs.WorkshopSettings.CoreItemIDsToMaterialIDs;
    for k, v in pairs(tbl) do
        tbl = v;
        tbl = v[1];
    end   

    print "Validating Lua 'enum' definitions ... "

    runAttackTypeEnumValidation(gameDefs);
         
    print "Running Validation Script ... DONE"
end

function runAttackTypeEnumValidation(gameDefs)
    require "AttackType"
    tbl = gameDefs.BodyPartDefs;
    for k, v in pairs(tbl) do
        if ( v ~= nil ) then 
            tbl = v.NaturalWeapon;
            if ( tbl ~= nil ) then
                for idx, attMove in ipairs(tbl.WeaponDef.AttackMoves) do
                    if ( attMove.AttackType == AttackType.HandToHand) then 
                        print(" -- HandToHand attack type")
                        return
                    end
                end
            end
        end
    end
end
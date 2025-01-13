require "GnollLuaValidationScript"

function OnEntitySpawn(entity)
end

g_valueMapping = {}

local _G = _GNOMORIA;
if ( _G ~= nil and _G.getCurrentSeason()  != null ) then
    local Season = require "Season"
    for season, idx in pairs(Season) do
        if ( idx == _G.getCurrentSeason()  ) then
            print("Current season " .. season)
        end
    end
    _G.notify(("Current season " .. season)    
elseif ( _G == nil ) then
    print("_GNOMORIA table is missing")
end

function OnNewGameStarted()
    gameDefs = _G.getGameDefs()
    if ( gameDefs == nil ) then
        print("GameDefs is null")
    end
    
    _generateNewValues(gameDefs)
    _assignNewValues(gameDefs)    
end

function _generateNewValues(gameDefs) 
    require "MaterialType"
    math.randomseed(os.time());
    for k, v in pairs(gameDefs.Materials) do
        local rndValue = math.random(1, 10);
        if ( v.Type == MaterialType.Wood )  then
            formatting = k .. " => "            
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[k] ~= nil) then 
                -- This is wood
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                print(" -- Name: " .. v.Name)
                print(" -- Current Value: " .. v.Value)
                print(" -- New Value: " .. rndValue)
                g_valueMapping[v.Name] = rndValue
            end
        end
    end
end

function _assignNewValues(gameDefs)
    require "MaterialType"
    print("_assignNewValues");
    for k, v in pairs(gameDefs.Materials) do
        if ( v.Type == MaterialType.Wood )  then
            formatting = k .. " => "            
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[k] ~= nil and g_valueMapping[v.Name] ~= nil) then 
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                print(" -- Name: " .. v.Name)
                print(" -- Current Value: " .. v.Value)
                v.Value = g_valueMapping[v.Name]                
                print(" -- New Value: " .. v.Value)                
            end
        end
    end
end

function OnGameSave(saver)
    saver.Save(g_valueMapping)
end

function OnSaveGameLoaded(loader)
    g_valueMapping = loader.load()
    print(" -- Loaded save game data ")
    for k, v in pairs(g_valueMapping) do
        print(" -- Value: " .. v)
        print(" -- Name: " .. k)
    end
    gameDefs = _G.getGameDefs()    
    _assignNewValues(gameDefs)    
    _G.notify("Mod data loaded successfully!")
end

function OnSeasonChange(season)
    local Season = require "Season"
    for ses, idx in pairs(Season) do
        if ( idx == _GNOMORIA.getCurrentSeason() ) then
            print("Current season " .. ses)
        end
    end
    
    for ses, idx in pairs(Season) do
        if ( idx == season ) then
            print("Season changed to " .. ses)
        end
    end
end
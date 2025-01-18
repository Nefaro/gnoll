g_valueMapping = {}

local _GN = _GNOMORIA;

if ( _GN ~= nil and _GN.getCurrentSeason()  != null ) then
    local Season = require "Season"
    for season, idx in pairs(Season) do
        if ( idx == _GN.getCurrentSeason()  ) then
            print("Current season " .. season)
            _GN.notify("Current season: " .. season)    
        end
    end
    
elseif ( _GN == nil ) then
    print("_GNOMORIA table is missing")
end

function OnNewGameStarted()
    local gameDefs = _GN.getGameDefs()
    if ( gameDefs == nil ) then
        print("GameDefs is null")
    end
    
    _GenerateNewValues(gameDefs)
    _assignNewValues(gameDefs)    
end

function _GenerateNewValues(gameDefs) 
    require "MaterialType"
    for k, v in pairs(gameDefs.Materials) do
        local rndValue = _GN.RandomInt(10);
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
    gameDefs = _GN.getGameDefs()    
    _assignNewValues(gameDefs)    
end

function OnSeasonChange(season)
    local Season = require "Season"
    for ses, idx in pairs(Season) do
        if ( idx == _GN.getCurrentSeason() ) then
            print("Current season " .. ses)
        end
    end
    
    for ses, idx in pairs(Season) do
        if ( idx == season ) then
            print("Season changed to " .. ses)
        end
    end
end
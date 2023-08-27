require "OnScriptValidation"

function OnEntitySpawn(entity)
end

g_valueMapping = {}

if ( _GNOMORIA ~= nil and _GNOMORIA['CurrentSeason']) then
    local Season = require "Season"
    for season, idx in pairs(Season) do
        if ( idx == _GNOMORIA['CurrentSeason'] ) then
            print("Current season " .. season)
        end
    end
else
    print("_GNOMORIA table is missing")
end


function OnNewGameStarted()
    gameDefs = _GNOMORIA['GameDefs']
    if ( gameDefs == nil ) then
        print("GameDefs is null")
    end
    
    _generateNewValues(gameDefs)
    _assignNewValues(gameDefs)    
end

function _generateNewValues(gameDefs) 
    local idx = 1;
    for k, v in pairs(gameDefs.Materials) do
        if ( v.Type == 4 )  then
            formatting = k .. " => "            
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[k] ~= nil) then 
                -- This is wood
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                print(" -- Name: " .. v.Name)
                print(" -- Current Value: " .. v.Value)
                print(" -- New Value: " .. v.Value)
                g_valueMapping[v.Name] = idx
            end
            idx = idx + 1
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
    gameDefs = _GNOMORIA['GameDefs']    
    _assignNewValues(gameDefs)    
end

function OnSeasonChange(season)
    local Season = require "Season"
    for ses, idx in pairs(Season) do
        if ( idx == season ) then
            print("Season changed to " .. ses)
        end
    end
end
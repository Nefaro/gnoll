require "OnScriptValidation"

function OnEntitySpawn(entity)
end

g_valueMapping = {}

function OnGameDefinitionsInitialized(gameDefs)
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
    print("_assignNewValues");
    for k, v in pairs(gameDefs.Materials) do
        if ( v.Type == 4 )  then
            formatting = k .. " => "            
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[k] ~= nil and g_valueMapping[v.Name] ~= nil) then 
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                print(" -- Name: " .. v.Name)
                print(" -- Current Value: " .. v.Value)
                v.Value = g_valueMapping[v.Name];                
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
end

function OnDefinitionsLoaded(gameDefs)
    print("OnDefinitionsLoaded");
    _assignNewValues(gameDefs)
end
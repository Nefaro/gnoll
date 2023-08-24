print("Hello from Lua init script")

require "OnScriptValidation"

function OnEntitySpawn(entity)

end

g_valueMapping = {}

function OnGameDefsLoaded(gameDefs)
    print "Gamedefs is NOT nil"
    --local tbl = gameDefs.PlantSettings.MaterialIDToPlantIDs;
    local tbl = gameDefs.Materials;        
    local idx = 1;
    for k, v in pairs(tbl) do
        if ( v.Type == 4 )  then
            formatting = k .. " => "            
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[k] ~= nil) then 
                -- This is wood
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                v.Value = idx
                print(" -- Value: " .. v.Value)
                print(" -- Name: " .. v.Name)
                g_valueMapping[v.Name] = v.Value
            else
            end
            idx = idx + 1
        end
    end
end

function OnSave(saver)
    saver.Save(g_valueMapping)
end
local Tree = require "Tree"
    
function OnTreeUpdate(_treeInstance, delta) 
    -- Includes are invisible for the Lua engin.
    -- Need a wrapper to redirect
    Tree:update(_treeInstance, delta)
end

function OnNewGameStarted()
    Tree:generateNewData()
end

function OnGameSave(saver)
    Tree:SaveData(saver)
end

function OnSaveGameLoaded(loader)
    Tree:LoadData(loader)    
end
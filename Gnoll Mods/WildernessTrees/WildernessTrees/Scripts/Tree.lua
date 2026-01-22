local Season = require "Season"
local MaterialType = require "MaterialType"

local Tree = {}
g_gameData = {}
local KEY_SEASON_BONUSES = "seasonBonuses"
local KEY_WOOD_VALUE_MAP = "woodValues"

-- These global tables are provided by Gnoll
local _GN  = _GNOMORIA

local WILDERNESS_COEF = 5
local FAVORITE_COEF = 3
local DISLIKE_COEF = 0.1
local BONUS_BASE = 0.125
local VALUE_ROLL_COEF = 3
-- Spring, Summer, Fall, Winter
local SEASON_WEIGHTS = {5, 8, 5, 1} -- sum = 19

function Tree.update(_treeInstance, delta) 
    if (not _treeInstance.IsOutside) then
        -- If the tree has migrated inside, don't update it anymore
        _GN.GetEntityManage.RemoveFromUpdateList(_treeInstance)
    elseif(_GN.GetOutsideLightLevel() > 0.75) then
		local gameDefs = _GN.GetGameDefs()
        
		if (_treeInstance.TimeToGrow == -1.0) then
			local plantDef = gameDefs.PlantDefFromMaterial(_treeInstance.MaterialID())
			if (not _treeInstance.HasClipping) then
				_treeInstance.TimeToGrow = _GN.RandomInRange(plantDef.GrowTimeMin, plantDef.GrowTimeMax) * 600 * 0.5
			elseif (not _treeInstance.HasFruit and plantDef.HasFruit) then
				_treeInstance.TimeToGrow = _GN.RandomInRange(plantDef.FruitGrowTimeMin, plantDef.FruitGrowTimeMax) * 600
			end
		end
		_treeInstance.TimeToGrow = _treeInstance.TimeToGrow - delta
        
		if (_treeInstance.TimeToGrow < 0.0) then
			if (not _treeInstance.HasClipping) then
				_treeInstance.GrowClipping()
			elseif (not _treeInstance.HasFruit) then
				_treeInstance.GrowFruit()
			end
			_treeInstance.TimeToGrow = -1.0
			local plantDef = gameDefs.PlantDefFromMaterial(_treeInstance.MaterialID())
			if (_treeInstance.HasFruit or not plantDef.HasFruit) then
                _GN.GetEntityManage.RemoveFromUpdateList(_treeInstance)
            end
		end 
    end
end

-- Generate season specific growth data
function Tree.generateGrowthMap()
    g_gameData[KEY_SEASON_BONUSES] = {}
    g_gameData[KEY_WOOD_VALUE_MAP] = {}

    local gameDefs = _GN.GetGameDefs()
    for materialID, materialProperty in pairs(gameDefs.Materials) do
        if ( materialProperty.Type == MaterialType.Wood ) then
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID] ~= nil) then 
                -- This is wood
                print("WOOD: " .. materialID)

                local favorite = _findSeason()
                local disliked = _findSeason(favorite)
                local bonusMap = _calculateBonusGrowthForSeason(favorite, disliked)
                local newValue = _generateNewMaterialValue()

                g_gameData[KEY_WOOD_VALUE_MAP][materialID] = newValue
                g_gameData[KEY_SEASON_BONUSES][materialID] = bonusMap

                print(" -- Current Value: " .. materialProperty.Value)
                print(" -- New Value: " .. newValue)
            end
        end
    end
end

function Tree.SaveData(saver)
    saver.Save(g_gameData)
end

function Tree.LoadData(loader)
    g_gameData = loader.load()
    _assignNewValues()
end

-- Weighted probability, find one season
function _findSeason(except)    
    local weightsSum = _sumSeasonWeights(except)
    -- Starts from 0, max excluded
    local roll = _GN.RandomInt(weightsSum) + 1
    local sum = 0
    for ses, idx in pairs(Season) do
        if ( except == nil or except ~= ses ) then
            -- Lua array starts at 1
            sum = sum + SEASON_WEIGHTS[idx+1]
            if ( sum >= roll ) then
                return ses
            end
        end
    end
end

function _sumSeasonWeights(except)
    local sum = 0
    for ses, idx in pairs(Season) do
        if ( except == nil or except ~= ses ) then
            -- Lua array start from 1, C# types start from 0
            sum = sum + SEASON_WEIGHTS[idx+1]
        end
    end       
    return sum
end

-- Calculate the bonus growth, for each season. 
-- We are rolling a dice, with [1 .. WILDERNESS_COEF] sides, SEASON_WEIGHTS[season] times
-- We get the sum of the rolls and multiply by BONUS_BASE
-- Favorite gets bonus*3, dislike gets bonus*0.1, others are 1:1
function _calculateBonusGrowthForSeason(favorite, disliked)
    local bonusMap = {}
    print(" -- Prefer " .. favorite .. ", dislike " .. disliked)
    for ses, idx in pairs(Season) do
        local rollCount = SEASON_WEIGHTS[idx+1]
        local sum = 0
        for cnt = 1, rollCount do
            -- rnd starts from 0, adjust
            local roll = _GN.RandomInt(WILDERNESS_COEF) + 1
            sum = sum + roll
        end
        local bonus = BONUS_BASE * sum
        if ( ses == favorite ) then
            bonus = bonus * FAVORITE_COEF
        elseif( ses == disliked ) then 
            bonus = bonus * DISLIKE_COEF
        end
        print(" -- Bonus for " .. ses .. ": " .. bonus)
        bonusMap[ses] = bonus;
    end
    return bonusMap;
end

function _generateNewMaterialValue() 
    local newValue = 0
    for cnt = 1, VALUE_ROLL_COEF do
        -- rnd starts from 0, we want from 1, adjust
        newValue = newValue + _GN.RandomInt(WILDERNESS_COEF) + 1
    end
    -- Shift towards 0
    newValue = newValue - WILDERNESS_COEF;
    -- Normalize to be at least 1
    if ( newValue < 1 ) then
        newValue = 1
    end
    return newValue
end

function _assignNewValues()
    local gameDefs = _GN.GetGameDefs()
    if ( g_gameData == nil or g_gameData[KEY_WOOD_VALUE_MAP] == null ) then
        return
    end

    for materialID, v in pairs(gameDefs.Materials) do
        if ( v.Type == MaterialType.Wood )  then
            formatting = materialID .. " => "
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID] ~= nil and g_gameData[KEY_WOOD_VALUE_MAP][materialID] ~= nil) then 
                print("WOOD: " .. formatting .. tostring(v) .. " (TYPE = " .. v.Type ..")")
                print(" -- Name: " .. materialID)
                print(" -- Current Value: " .. v.Value)
                v.Value = g_gameData[KEY_WOOD_VALUE_MAP][materialID]                
                print(" -- New Value: " .. v.Value)                
            end
        end
    end
end

return Tree

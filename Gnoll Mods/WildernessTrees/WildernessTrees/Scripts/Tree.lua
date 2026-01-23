local Season = require "Season"
local SeasonHelper = require "SeasonHelper"
local MaterialType = require "MaterialType"

local Tree = {
    g_gameData = {}
}

local KEY_SEASON_BONUSES = "seasonBonuses"
local KEY_WOOD_VALUE_MAP = "woodValues"

-- These global tables are provided by Gnoll
local _GN  = _GNOMORIA

local WILDERNESS_COEF = 5 -- How wild
local FAVORITE_COEF = 3 -- Bonus for favorite season is multiplied by this
local DISLIKE_COEF = 0.1 -- Bonus for disliked season is multiploied by this
local BONUS_BASE = 0.125 -- The base bonus coef, currently 12.5%
local VALUE_ROLL_COEF = 5 -- For material value calculation, WILDERNESS_COEF sided die is rolled this amount
local SHIFT_COEF = 3 -- WILDERNESS_COEF times SHIFT_COEF is removed from the material value, shifting towards 0
local RAIN_BONUS = 1 + BONUS_BASE
-- Spring, Summer, Fall, Winter
local SEASON_WEIGHTS = {5, 8, 5, 1} -- sum = 19

function Tree:update(_treeInstance, delta)
    if ( _GN.IsGamePaused() ) then
        -- Nothing to process when the game is paused
        return
    end

    if (not _treeInstance.IsOutside) then
        -- If the tree has migrated inside, don't update it anymore
        _GN.GetEntityManager.RemoveFromUpdateList(_treeInstance)
    elseif(_GN.GetOutsideLightLevel() > 0.75) then
		local gameDefs = _GN.GetGameDefs()
        
		if (_treeInstance.TimeToGrow == -1.0) then
			local plantDef = gameDefs.PlantDefFromMaterial(_treeInstance.MaterialID)

			if (not _treeInstance.HasClipping) then
				_treeInstance.TimeToGrow = _GN.RandomInRange(plantDef.GrowTimeMin, plantDef.GrowTimeMax) * 600 * 0.5 *  WILDERNESS_COEF
			elseif (not _treeInstance.HasFruit and plantDef.HasFruit) then
				_treeInstance.TimeToGrow = _GN.RandomInRange(plantDef.FruitGrowTimeMin, plantDef.FruitGrowTimeMax) * 600 *  WILDERNESS_COEF
			end
		end
        local season = SeasonHelper.currentSeason()
        local plantID = gameDefs.PlantSettings.MaterialIDToPlantIDs[_treeInstance.MaterialID]
        local seasonBonus = self.g_gameData[KEY_SEASON_BONUSES][plantID][season]

        if ( _GN.IsRaining() ) then
            -- Rain good, except Winter
            if ( season ~= Season.Winter) then
                seasonBonus = seasonBonus * RAIN_BONUS
            end
        end
        -- Apply the bobus growth
		_treeInstance.TimeToGrow = _treeInstance.TimeToGrow - ( delta * seasonBonus )


		if (_treeInstance.TimeToGrow < 0.0) then
			if (not _treeInstance.HasClipping) then
				_treeInstance.GrowClipping()
			elseif (not _treeInstance.HasFruit) then
				_treeInstance.GrowFruit()
			end
			_treeInstance.TimeToGrow = -1.0
			local plantDef = gameDefs.PlantDefFromMaterial(_treeInstance.MaterialID)
			if (_treeInstance.HasFruit or not plantDef.HasFruit) then
                _GN.GetEntityManager.RemoveFromUpdateList(_treeInstance)
            end
		end 
    end
end

-- Generate season specific growth data and material values
function Tree:generateNewData()
    self.g_gameData[KEY_SEASON_BONUSES] = {}
    self.g_gameData[KEY_WOOD_VALUE_MAP] = {}

    local gameDefs = _GN.GetGameDefs()
    print("Generating new growth bonuses and material values ...")
    for materialID, materialProperty in pairs(gameDefs.Materials) do
        if ( materialProperty.Type == MaterialType.Wood ) then
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID] ~= nil) then 
                -- This is wood
                local plantID = gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID]
                print("++ " .. plantID)

                local favorite = _findSeason()
                local disliked = _findSeason(favorite)
                local bonusMap = _calculateBonusGrowthForSeason(favorite, disliked)
                local newValue = _generateNewMaterialValue()

                self.g_gameData[KEY_WOOD_VALUE_MAP][plantID] = newValue
                self.g_gameData[KEY_SEASON_BONUSES][plantID] = bonusMap

                print(" -- Current Value: " .. materialProperty.Value)
                print(" -- New Value: " .. newValue)
            end
        end
    end
    print("Generating new growth bonuses and material values ... DONE")
end

function Tree:SaveData(saver)
    saver.Save(self.g_gameData)
end

function Tree:LoadData(loader)
    self.g_gameData = loader.load()
    local seasonData = self.g_gameData[KEY_SEASON_BONUSES]
    if ( seasonData == nil ) then
        print("seasonData from load are null")
    end
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
    -- Shift towards 0; more likely to have value=1 than the higher values
    newValue = newValue - WILDERNESS_COEF * SHIFT_COEF;
    -- Normalize to be at least 1
    if ( newValue < 1 ) then
        newValue = 1
    end
    return newValue
end

function _assignNewValues()
    local gameDefs = _GN.GetGameDefs()
    if ( Tree.g_gameData == nil or Tree.g_gameData[KEY_WOOD_VALUE_MAP] == null ) then
        return
    end
    print("Applying material values from save file ...")
    for materialID, materialProps in pairs(gameDefs.Materials) do
        if (materialProps.Type == MaterialType.Wood)  then
            if (gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID] ~= nil) then 
                local plantID = gameDefs.PlantSettings.MaterialIDToPlantIDs[materialID]
                if (Tree.g_gameData[KEY_WOOD_VALUE_MAP][plantID] ~= nil) then 
                    print("++ " .. plantID)
                    print(" -- Current Value: " .. materialProps.Value)
                    materialProps.Value = Tree.g_gameData[KEY_WOOD_VALUE_MAP][plantID]                
                    print(" -- New Value: " .. materialProps.Value)                
                end
            end
        end
    end
    print("Applying material values from save file ... DONE")
end

return Tree

-- Don't name it _G, this table already exists in Lua
-- These global tables are provided by Gnoll
local _GN    = _GNOMORIA
local _U    = _GUI
local _J     = _JOBS

-- Will be used to save and load the state of this adventure
-- Need to track the "leader" of the raiding party and go from there
-- g_RaidParty is for saving/loading this mod specific stuff
local g_RaidParty = {}
local KEY_LEADER = "leader"
-- Leader of the current squad
local leader

-- Combobox selections
local selectedSquadIdx
local selectedKingdomIdx

-- Add page to the Kingdom main menu
function AddPageToKingdomMenu(kingdomMenu)
    if ( _U == nil ) then
        print("UI  Helper not loaded")
        return
    end
    
    local hasExistingRaidParty = false
    -- Figure out, if there is already an existing party en route
    -- "ForeignTradeJob" is the Gnomoria object we are (ab)using for this particular functionality
    if ( leader ~= nil and leader.Job ~= nil and leader.Job.ClassName == "ForeignTradeJob") then
        if ( leader.Job.Envoy ~= nil ) then
            require "EnvoyType"
            if ( leader.Job.Envoy.EnvoyType == EnvoyType.Raid ) then
                hasExistingRaidParty = true
            end
        end
    else 
        -- If any of the checks fails, assume... nothing. Release the leader
        leader = nil
    end    
    
    -- New Panel added to the Kingdom Menu
    local tabbedPanel = _U.CreateTabbedWindowPanel()
    
    local squadLabel = _U.CreateLabel("Select Squad to form a raiding party")
    tabbedPanel.Add(squadLabel)
    
    local squadSelection
    local canSend = true
    if ( #_GN.GetMilitary().Squads == 0 ) then
        squadSelection = _U.CreateLabel("No Squads to select from")
        tabbedPanel.AddDownFrom(squadSelection, squadLabel)
        canSend = false
    else
        squadSelection = _U.CreateSelect(_GN.GetMilitary().Squads)
        -- Add change handler, defined in Lua
        squadSelection.ItemIndexChanged.Add(squadSelectionHandler)
        tabbedPanel.AddDownFrom(squadSelection, squadLabel)
        
        if ( hasExistingRaidParty ) then 
            for i, sq in ipairs(_GN.GetMilitary().Squads)  do
                if ( sq.Name == leader.Squad.Name) then 
                    -- From Lua index to C# index
                    squadSelection.ItemIndex = (i - 1)
                    break
                end
            end
        end        
    end
    
    local kingdomLabel = _U.CreateLabel("Select Kingdom to raid")
    tabbedPanel.AddDownFrom(kingdomLabel, squadSelection)        
    
    local kingdomSelect
    if ( #_GN.GetDiplomaticFactions() == 0 ) then
        kingdomSelect = _U.CreateLabel("No Kingdoms to select from")
        tabbedPanel.AddDownFrom(kingdomSelect, kingdomLabel)
        canSend = false
    else
        kingdomSelect = _U.CreateSelect(_GN.GetDiplomaticFactions())
        -- Add change handler, defined in Lua
        kingdomSelect.ItemIndexChanged.Add(kingdomSelectionHandler)
        tabbedPanel.AddDownFrom(kingdomSelect, kingdomLabel)
        
        if ( hasExistingRaidParty ) then 
            for i, kng in ipairs(_GN.GetDiplomaticFactions())  do
                if ( kng.Name == leader.Job.GetTargetFaction().Name) then 
                    -- From Lua index to C# index, hence -1
                    kingdomSelect.ItemIndex = (i - 1)
                    break
                end
            end
        end
    end
    
    local button = _U.CreateButton("Send Raiding Party")
    tabbedPanel.AddDownFrom(button, kingdomSelect)
    kingdomMenu.AddPage("(G) Raiding", tabbedPanel)
        
    if ( hasExistingRaidParty ) then
        local raidLabel = _U.CreateLabel("Squad '"..leader.Squad.Name.."' en route to '".. leader.Job.GetTargetFaction().Name .."' ... ")
        -- Colors are defined by MS Xna Framework, loaded into Lua by Gnoll
        raidLabel.TextColor = Color.Orange
        tabbedPanel.AddDownFrom(raidLabel, button)
        -- Disable/Enable needs to be last, after the buttons have been added to the panel
        squadSelection.Disable()
        kingdomSelect.Disable()
        button.Disable()
    elseif ( canSend ) then
        -- Add Click handler defined in Lua
        button.Click.Add(buttonHandler)
    else  
        button.Disable()
    end    
end

-- Squad selection handler, need to know the index of the selection
-- Selected object would be better, but index is easier
function squadSelectionHandler(control)
    print("Squad selection changed! : '".. control.SelectedItemIndex() .."'")
    selectedSquadIdx = control.SelectedItemIndex()
    if (selectedSquadIdx ~= nil and selectedSquadIdx ~= -1) then
        -- Adjust for Lua indexing starting from 1
        selectedSquadIdx = selectedSquadIdx + 1
    end    
end

-- Kingdom selection handler
function kingdomSelectionHandler(control) 
    print("Kingdom selection changed! : '".. control.SelectedItemIndex() .."'")
    selectedKingdomIdx = control.SelectedItemIndex()
    if (selectedKingdomIdx ~= nil and selectedKingdomIdx ~= -1) then
        -- Adjust for Lua indexing starting from 1
        selectedKingdomIdx = selectedKingdomIdx + 1
    end     
end

-- Send button handler
function buttonHandler(button)
    -- If squad selection index and kingdom selection index are missing, we have completely messed up
    if (selectedSquadIdx == nil or selectedSquadIdx == -1) then
        print("Invalid Squad selection")
        return
    end
    
    if (selectedKingdomIdx == nil or selectedKingdomIdx == -1) then
        print("Invalid Kingdom selection")
        return
    end
    
    local squads = _GN.GetMilitary().Squads
    if ( squads == nil ) then
        print("No squads to select from")
        return
    end
    
    local kingdoms = _GN.GetDiplomaticFactions()
    if ( kingdoms == nil ) then
        print("No kingdoms to select from")
        return
    end
    
    local squad = squads[selectedSquadIdx]
    print("Selected squad! " .. squad.Name)
    
    local kingdom = kingdoms[selectedKingdomIdx]
    print("Selected Kingdom! " .. kingdom.Name)
    -- Prepare the raid
    sendSquadToRaid(squad, kingdom)
end

function sendSquadToRaid(squad, kingdom) 
    -- Promote the first member to leader
    -- We use the leader for tracking progress as well as for saving/loading game
    -- Remember, Lua indexing starts from 1    
    leader = squad.Members[1]
    -- Store the Character.ID for saving/loading
    g_RaidParty[KEY_LEADER] = leader.ID
    print("Promoted to leader! " .. leader.Name)
    
    if ( kingdom == nil ) then 
        print("Kingdom nil")
        return
    end
    -- Our hacked, custom Raiding job object
    -- Hides quite a bit of code for setting this up
    local raidJob = _J.CreateCustomRaidingJobForSquad(kingdom, squad)
    if ( raidJob == nil ) then
        print("No raiding job")
        return
    end
    
    require "CharacterAttributeType"
    
    local sumFitness = 0
    local sumCharm = 0
    local sumNimble = 0
    
    for _, member in ipairs(squad.Members) do
        -- Use Fitness to calculate the amount of loot we get from hostile factions
        sumFitness = sumFitness + member.AttributeLevel(CharacterAttributeType.Fitness)
        -- Use Charm to calculate the amount of loot we get from friendly factions
        sumCharm = sumCharm + member.AttributeLevel(CharacterAttributeType.Charm)
        -- Use Nimbleness to "find" some metal ingots and gems
        sumNimble = sumNimble + member.AttributeLevel(CharacterAttributeType.Nimbleness)        
    end 
    
    print ("Sum of Fitness " .. sumFitness)
    print ("Sum of Charm " .. sumCharm)
    print ("Sum of Nimbleness " .. sumNimble)    

    local lootItems = {}
    if ( kingdom.IsHostileToPlayer() ) then 
        local picksCount = math.floor(sumFitness / 10)
        print ("Item picks " .. picksCount)
        -- Generate item table for enemy faction
        lootItems = itemsFromEnemyFaction(picksCount, kingdom)
        -- Add all items to the job, for spawning on the map
        if ( lootItems ~= nil ) then 
            for _, v in pairs(lootItems) do
                -- Expecting {materialID, itemID, value}            
                _J.AddSpawningItems(raidJob, v[1], v[2], v[3])
            end
        end        
    else
        local picksCount = math.floor(sumCharm / 10)
        print ("Item picks " .. picksCount)
        -- Generate item table for friendly faction
        lootItems = itemsFromFriendlyFaction(picksCount, kingdom)
        -- Add all items to the job, for spawning on the map
        if ( lootItems ~= nil ) then 
            for _, v in pairs(lootItems) do
                -- Expecting {AvailableGood}
                _J.AddSpawningGoods(raidJob, v)
            end
        end         
    end
    
    -- Generate bonus items from Gnomoria definitions
    lootItems = itemsFromDictionary(math.floor(sumNimble / 100), math.floor(sumNimble / 250))
    if ( lootItems ~= nil ) then 
        for _, v in pairs(lootItems) do
            -- Expecting {materialID, itemID, value, quantity}            
            _J.AddSpawningItems(raidJob, v[1], v[2], v[3], v[4])
        end
    end     
    
    print("Squad '".. squad.Name .."' was sent to raid  '" .. kingdom.Name.."'")
    -- Send a notification to screen as well
    _GN.Notify("Squad '".. squad.Name .."' was sent to raid  '" .. kingdom.Name.."'")    
end

function itemsFromDictionary(picksCountMetal, picksCountGems) 
    local gameDefs = _GN.getGameDefs()
    require "MaterialType"
    
    -- Filtered metal items
    local metalSelection = {}    
    -- Filtered gem items
    local gemSelection = {}
    -- Resulting/picked loot items
    local lootItems = {}
    
    for _, matDef in pairs(gameDefs.Materials) do
        if ( matDef.Type == MaterialType.Metal )  then      
            print("Metal: " .. matDef.ID)
            table.insert(metalSelection, matDef)
        elseif ( matDef.Type == MaterialType.Gem ) then
            print("Gem: " .. matDef.ID)
            table.insert(gemSelection, matDef)        
        end
    end
    
    for idx = 1, picksCountMetal do
        local pick = _GN.RandomInt(#metalSelection)
        -- +1 since we are using it as index
        local def = metalSelection[pick+1]
        -- {materialID, itemID, value, quantity}
        -- Hardcoded itemID = Bar, since it's the bars that I want
        table.insert(lootItems, {def.ID, "Bar", def.Value, math.max(1, _GN.RandomInt(picksCountMetal))}) 
    end  
    
    for idx = 1, picksCountGems do
        local pick = _GN.RandomInt(#gemSelection)
        -- +1 since we are using it as index
        local def = gemSelection[pick+1]
        
        -- 50/50 uncut/cut gem
        local itemID = "RawGem"
        if (_GN.RandomBoolean() ) then
            itemID = "Gem"
        end
        
        -- {materialID, itemID, value, quantity}
        table.insert(lootItems, {def.ID, itemID, def.Value, math.max(1,_GN.RandomInt(picksCountGems))}) 
    end    
    
    return lootItems
end

function itemsFromFriendlyFaction(picksCount, kingdom)
    local selection = {}
    -- Throw random picks until limit
    for idx = 1, picksCount do
        local pick = _GN.RandomInt(#kingdom.FactionGoods.AvailableGoods)
        -- +1 since we are using it as index
        local item = kingdom.FactionGoods.AvailableGoods[pick+1]
        table.insert(selection, item) 
    end
    return selection
end

-- Select items from the staring equipment of enemy factions
-- Since the starting equipment has weights for picking, this will be a bit complex calculation
function itemsFromEnemyFaction(picksCount, kingdom)
    require "SquadType"
    
    local squadDefs = kingdom.FactionDef.Squads
    
    local totalWeight = 0
    -- Calculate the total weights of all the squad items
    for _, squadDef in ipairs(squadDefs) do        
        for _, cls in ipairs(squadDef.Classes) do
            if ( _GN.IsRaceAllowedBySettings(cls.RaceID) and cls.Name ~= '') then
                for _, itemDef in ipairs(cls.StartingItems) do
                    for _, startingItem in ipairs(itemDef.IDs) do
                        totalWeight = totalWeight + startingItem.Weight
                    end
                end
            end
        end
    end
    
    -- Throw random picks until limit
    local picks = {}
    for idx = 1, picksCount do
      table.insert(picks, _GN.RandomInt(totalWeight))
    end 
    table.sort(picks)
   
    local tracker = 0
    local i = 1
    local selection = {}
    -- Select the items according to random picks
    for _, squadDef in ipairs(squadDefs) do       
        for _, cls in ipairs(squadDef.Classes) do
            if ( _GN.IsRaceAllowedBySettings(cls.RaceID) and cls.Name ~= '') then
                for _, itemDef in ipairs(cls.StartingItems) do
                    for _, startingItem in ipairs(itemDef.IDs) do
                        if ("ITEMID_NONE" ~= startingItem.ItemID) then
                            tracker = tracker + startingItem.Weight
                            if ( tracker >= picks[i] ) then
                                -- {materialID, itemID, value}
                                table.insert(selection, {itemDef.MaterialID, startingItem.ItemID, 1}) 
                                i = i + 1
                                if ( i > picksCount ) then
                                    return selection
                                end
                            end
                        end
                    end
                end
            end
        end
    end
    return selection
end

-- Save the party leader ID, if we have it
function OnGameSave(saver)
    saver.Save(g_RaidParty)
end

-- Load the party leader ID, as well as find the object
function OnSaveGameLoaded(loader)
    g_RaidParty = loader.load()
    local leaderID = g_RaidParty[KEY_LEADER]
    if ( leaderID ~= nil ) then 
        print(" -- Loaded raid party leader " .. leaderID)
        -- Find the actual character
        local faction = _GN.GetPlayerFaction()
        local temp = faction.Members[leaderID]
        if ( temp == nil ) then
            print(" -- Raid party leader does not exist ... ")
            leader = nil
        else
            print(" -- Raid party leader: " .. temp.Name)
            leader = temp
        end
    else
        print(" -- No raid party leader found ")
    end
end

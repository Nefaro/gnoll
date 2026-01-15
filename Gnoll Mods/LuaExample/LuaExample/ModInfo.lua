Startup = require "Startup" -- Example of delegating OnEnable/OnDisable to another script. This script is searched from this mod's ./Script folder

ModInfo = {
    IsDefaultEnabled=false, -- Optional: Default 'false'
    NeedsRestartOnToggle=false, -- Optional: Default 'false'. Lua scripts/mods should not require restart on enable/disable.
    Name="LuaExample", -- Mandatory: Name is used as unique identifier and also shown ingame mod list. Current convention, same name as the mod-folder. Max 36 chars
    Description="This is an example of how to register a pure Lua mod. Other mods so far have required a C# component to be present. This one does not.", -- Mandatory: Also shown in the ingame mod list. Max 134 chars
    BuiltWithLoaderVersion="1.15.0", -- Mandatory: Mostly for information, the code does nothing with it, but useful for debugging. The current Gnoll version. Limited to 12 chars.
    RequireMinPatchVersion=15, -- Mandatory: This is checked just as a low effort validation to weed out version mismatch. Gnoll needs to have at least this patch version.
    OnEnable = function()  -- Optional: Default no nothing
        Startup.OnModEnable()
    end,
   OnDisable = function() -- Optional: Default no nothing
        Startup.OnModDisable()
    end
}
return ModInfo;
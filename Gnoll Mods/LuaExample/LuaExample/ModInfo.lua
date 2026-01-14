Startup = require "Startup" -- This is searched from this mods ./Script folder

ModInfo = {
    IsDefaultEnabled=false,
    NeedsRestartOnToggle=false,
    Name="LuaExample",
    Description="This is an example of how to register a pure Lua mod. Other mods so far have required a C# component to be present. This one does not",
    BuiltWithLoaderVersion="G1.14.3", -- Mostly for information, the code does nothing with it
    RequireMinPatchVersion=14, -- This is checked
    OnEnable = function() 
        Startup.OnModEnable()
    end,
   OnDisable = function()
        Startup.OnModDisable()
    end
}
return ModInfo;
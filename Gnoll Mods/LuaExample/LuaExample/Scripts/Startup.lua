Startup = {
    OnModEnable = function()
        print("Hello from Startup! Loading mod!")
    end,
    
    OnModDisable = function()
        print("Unloading mod! Bye from Startup!")
    end
}
return Startup
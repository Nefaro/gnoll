namespace GnollModLoader
{
    // Main interface for Gnoll Mods
    // The interface type will be used for
    // detecting Gnoll Mods
    public interface IGnollMod
    {
        // Called when the mod is enabled
        void OnEnable(HookManager hookManager);

        // Called when the mod is disabled
        void OnDisable(HookManager hookManager);

        // If the mod should be enabled by default
        bool IsDefaultEnabled();

        // If the mod requires the game to restart 
        // when toggling enable/disable
        bool NeedsRestartOnToggle();

        // Name of the mod, 1-liner, visible from the ui
        string Name { get; }

        // Short description, will be cut into pieces in the ui
        string Description { get; }

        // Version of the loader when the mod was built.
        // Keeping track of the version for debugging etc.
        string BuiltWithLoaderVersion { get; }

        // Min required patch version
        // Modloader should detect incompatible patch version to avoid trouble
        int RequireMinPatchVersion { get; }
    }
}

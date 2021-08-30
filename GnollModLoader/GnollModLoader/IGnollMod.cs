using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollModLoader
{
    // Main interface for Gnoll Mods
    // The interface type will be used for
    // detecting Gnoll Mods
    public interface IGnollMod
    {
        void OnLoad(HookManager hookManager);

        // Name of the mod, 1-liner, visible from the ui
        string Name { get; }

        // Short description, will be cut into pieces in the ui
        string Description { get; }

        // Version of the loader when the mod was built.
        // Keeping track of the version for debugging etc.
        string BuiltWithLoaderVersion { get;  }
    }
}

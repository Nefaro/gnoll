using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollModLoader
{
    public interface IMod
    {
        void OnLoad(HookManager hookManager);

        string Name { get; }

        string Description { get; }
    }
}

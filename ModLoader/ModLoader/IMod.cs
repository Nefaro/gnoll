using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModLoader
{
    public interface IMod
    {
        void OnLoad(HookManager hookManager);
    }
}

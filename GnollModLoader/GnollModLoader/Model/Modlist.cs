using System;
using System.Collections.Generic;

namespace GnollModLoader.Model
{
    internal class Modlist
    {
        public Modlist()
        {
            ModStatus = new Dictionary<string, bool>();
        }
        public Dictionary<String, bool> ModStatus{ get; set; }
    }
}

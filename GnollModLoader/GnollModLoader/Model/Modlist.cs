using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

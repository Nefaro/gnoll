using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnollModLoader
{
    // Encapsulate current output
    // Porbably need file-based logging down the line
    internal class Logger
    {
        private static readonly string PREFIX = "[Gnoll] ";
        public static void Log(string message, params object[] args)
        {
            System.Console.WriteLine(PREFIX + message, args);
        }
        public static void Log(string message, object arg)
        {
            System.Console.WriteLine(PREFIX + message, arg);
        }

    }
}

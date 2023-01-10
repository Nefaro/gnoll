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
        private static readonly string ERROR = "[ERROR] ";

        public static void Log(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) " + PREFIX + message, args);
        }
        public static void Log(string message, object arg)
        {
            System.Console.WriteLine($"({DateTime.Now}) " + PREFIX + message, arg);
        }

        public static void Error(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) " + PREFIX + ERROR + message, args);
        }

    }
}

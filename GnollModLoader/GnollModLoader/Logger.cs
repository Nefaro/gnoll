using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace GnollModLoader
{
    // Encapsulate current output
    // Porbably need file-based logging down the line
    internal class Logger
    {
        private static readonly string PREFIX = "[Gnoll]";
        private static readonly string ERROR = "[ERROR]";
        private static readonly string WARN = "[WARN]";

        public static void Log(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) {PREFIX} {message}", args);
        }
        public static void Log(string message, object arg)
        {
            System.Console.WriteLine($"({DateTime.Now}) {PREFIX} {message}", arg);
        }

        public static void Warn(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) {PREFIX} ?? {WARN} {message}", args);
        }

        public static void Error(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) {PREFIX} !! {ERROR} {message}", args);
        }

    }

    internal class LuaLogger
    {
        private static readonly string PREFIX = "LUA";

        public static void Log(string mod, string message, params object[] args)
        {
            System.Console.Write($"({DateTime.Now}) [{PREFIX}::{mod}] ", args);
            System.Console.WriteLine( process(message) );
        }

        private static string process(string message) => Newtonsoft.Json.JsonConvert.ToString(message);
    }

    public class ModsLogger
    {
        private static readonly string ERROR = "[ERROR]";
        private static readonly string WARN = "[WARN]";

        private static Dictionary<string, ModsLogger> _registry = new Dictionary<string, ModsLogger>();

        private readonly string _owner;

        private ModsLogger(string owner) { 
            this._owner = owner;
        }

        public static ModsLogger getLogger(IGnollMod forMod)
        {
            var key = "Default";
            if (forMod != null) {
                key = forMod.Name;
            }
            ModsLogger logger = null;
            if (_registry.TryGetValue(key, out logger) )
            {
                return logger;
            }
            logger = new ModsLogger(key);
            _registry.Add(key, logger);
            return logger;
        }

        public void Log(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) [{this.prefix()}] {message}", args);
        }

        public void Error(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) [{this.prefix()}] !! {ERROR} {message}", args);
        }
        public void Warn(string message, params object[] args)
        {
            System.Console.WriteLine($"({DateTime.Now}) [{this.prefix()}] ?? {WARN} {message}", args);
        }

        private string prefix()
        {
            return this._owner.ToString();
        }
    }
}

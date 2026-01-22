using System;

namespace GnollModLoader
{
    // Abstraction to run mod specific scripts from C#
    public class LuaScriptRunner
    {
        public Action<string, object[]>  RunnerDelegate { get; set; }

        public void RunFunction(string functionName, params object[] args)
        {
            RunnerDelegate?.Invoke(functionName, args);
        }
    }
}

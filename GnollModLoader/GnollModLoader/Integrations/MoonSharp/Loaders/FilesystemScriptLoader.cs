using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace GnollModLoader.Integration.MoonSharp.Loaders
{
    // Mostly copy from the Moonsharp project
    // "portable" library, which we use, does not support the built-in FileSystem loader
    // Gnoll, however, is limited to PC (Win/Linux-wine) and uses "portable" only for
    // working around a runtime issue

    internal class FilesystemScriptLoader : ScriptLoaderBase
    {
        public override object LoadFile(string file, Table globalContext)
        {
            return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public override bool ScriptFileExists(string name)
        {
            return File.Exists(name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InstallerCore
{
    public class FileUnblocker
    {
        private static readonly Logger _log = Logger.GetLogger;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        public static bool UnblockFile(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }

        public static void UnblockPath(string path)
        {
            string[] files = System.IO.Directory.GetFiles(path);
            string[] dirs = System.IO.Directory.GetDirectories(path);

            foreach (string file in files)
            {
                _log.WriteLine($"Unblocking file: {file}");
                bool result = UnblockFile(file);
                if ( !result )
                {
                    int error = Marshal.GetLastWin32Error();

                    _log.WriteLine($"Failed to unblock file: {file} with error {error}");
                }
            }

            foreach (string dir in dirs)
            {
                UnblockPath(dir);
            }
        }
    }
}

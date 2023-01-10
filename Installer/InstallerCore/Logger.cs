using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerCore
{
    // Poor man's logging framework
    // Encapsulates logging
    public class Logger
    {
        private static readonly string FILENAME = "GnollInstaller.log";
        private static readonly StreamWriter LOGFILE = new StreamWriter(FILENAME, append: true);
        private static readonly Logger _instance = new Logger();

        public static Logger GetLogger
        {
            get
            {
                LOGFILE.AutoFlush = true;
                return _instance;
            }
        }


        public void WriteLine(String msg)
        {
            LOGFILE.WriteLine(msg);
            LOGFILE.Flush();
        }

        public void Log(String msg)
        {
            this.WriteLine(msg);
        }

        public void Warn(String msg)
        {
            this.WriteLine("!! " + msg);
        }

        public void Error(String msg)
        {
            this.WriteLine("¤¤ " + msg);
        }
    }
}

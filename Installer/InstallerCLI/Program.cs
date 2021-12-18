using InstallerCore;
using System;
using System.IO;

namespace InstallerCLI
{
    class Program
    {
        static void AlmostMain(string[] args, StreamWriter logFile)
        {
            var modKitVersion = new ModKitVersion(1700, "G1.7");
            var gameDb = new GameDb();
            var patchDb = new PatchDatabase();

            string gameDir;

            if (args.Length > 0)
            {
                gameDir = args[0];
            }
            else
            {
                Console.Write("Game install dir> ");
                gameDir = Console.ReadLine();
            }

            var res = InstallerCore.InstallerCore.ScanGameInstall(gameDir, modKitVersion, gameDb, patchDb);

            Console.WriteLine($"Game version:            {res.GameVersion}");
            Console.WriteLine($"Installed gnoll version: {res.ModKitVersion}");

            if (!res.PatchAvailable)
            {
                Console.WriteLine($"WARNING: No patch available for this game version");
            }

            var actions = res.AvailableActions;

            for (int i = 0; i < actions.Length; i++)
            {
                Console.WriteLine($"[{i}] {actions[i]}");
            }

            if (actions.Length > 0)
            {
                Console.WriteLine();
                Console.Write("Choice> ");

                var choice = Console.ReadLine();

                if (choice != "")
                {
                    actions[int.Parse(choice)].Execute();
                    Console.WriteLine("Success.");
                }
            }
        }

        static void Main(string[] args)
        {
            using (var logFile = new StreamWriter("GnollInstaller.log", append: true))
            {
                try
                {
                    AlmostMain(args, logFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    logFile.WriteLine(ex.ToString());
                }
                finally
                {
                    Console.WriteLine();
                    Console.WriteLine("(Press Enter to exit)");
                    Console.ReadLine();
                }
            }
        }
    }
}

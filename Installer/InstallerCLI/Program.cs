using InstallerCore;
using System;
using System.IO;

namespace InstallerCLI
{
    class Program
    {
        private static readonly Logger _log = Logger.GetLogger;

        static void AlmostMain(string[] args)
        {
            var gameDb = new GamePatchDatabase(AppContext.BaseDirectory);

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

            var res = InstallerCore.InstallerCore.ScanGameInstall(gameDir, gameDb);

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
            try
            {
                AlmostMain(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _log.WriteLine(ex.ToString());
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

using System.IO;

namespace InstallerCore
{
    public abstract class Action
    {
        public abstract void Execute();
    }

    internal class InstallModKit : Action
    {
        public InstallModKit(string catalogPath, string outputPath, string backupPath, string vanillaMd5, ModKitVersion modKitVersion, Installable patch)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            BackupPath = backupPath;
            VanillaMd5 = vanillaMd5;
            ModKitVersion = modKitVersion;
            Patch = patch;
        }

        public override void Execute()
        {
            // back up destination
            File.Move(OutputPath, BackupPath);

            // apply patch
            Patch.Install(OutputPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);
            catalog.DefaultRecord = new InstallRecord(modKitBuildNumber: ModKitVersion.BuildNumber, vanillaMd5: VanillaMd5);
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Install {ModKitVersion.VersionString} to {OutputPath} using {Patch}";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
        public string VanillaMd5 { get; }
        public ModKitVersion ModKitVersion { get; }
        public Installable Patch { get; }
    }

    internal class InstallStandalone : Action
    {
        public InstallStandalone(string catalogPath, string outputPath, string vanillaMd5, ModKitVersion modKitVersion, Installable patch)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            VanillaMd5 = vanillaMd5;
            ModKitVersion = modKitVersion;
            Patch = patch;
        }

        public override void Execute()
        {
            // apply patch
            Patch.Install(OutputPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);
            catalog.Standalone.Add(new InstallRecord(modKitBuildNumber: ModKitVersion.BuildNumber, vanillaMd5: VanillaMd5));
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Install {ModKitVersion.VersionString} to {OutputPath} using {Patch}";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string VanillaMd5 { get; }
        public ModKitVersion ModKitVersion { get; }
        public Installable Patch { get; }
    }

    internal class UninstallModKit : Action
    {
        public UninstallModKit(string catalogPath, string outputPath, string backupPath, string vanillaMd5, ModKitVersion modKitVersion)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            BackupPath = backupPath;
            VanillaMd5 = vanillaMd5;
            ModKitVersion = modKitVersion;
        }

        public override void Execute()
        {
            // restore backup
            File.Replace(BackupPath, OutputPath, destinationBackupFileName: null);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);
            catalog.DefaultRecord = null;
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Un-mod {ModKitVersion.VersionString} ({OutputPath})";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
        public string VanillaMd5 { get; }
        public ModKitVersion ModKitVersion { get; }
    }

    internal class UninstallStandalone : Action
    {
        public UninstallStandalone(string catalogPath, string outputPath, string vanillaMd5, ModKitVersion modKitVersion)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            VanillaMd5 = vanillaMd5;
            ModKitVersion = modKitVersion;
        }

        public override void Execute()
        {
            // delete patched executable
            File.Delete(OutputPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);

            for (int i = catalog.Standalone.Count - 1; i >= 0; i--)
            {
                var entry = catalog.Standalone[i];
                if (ModKitVersion.BuildNumber == entry.ModKitBuildNumber && VanillaMd5 == entry.VanillaMd5)
                {
                    catalog.Standalone.RemoveAt(i);
                    continue;
                }
            }
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Uninstall {ModKitVersion.VersionString} ({OutputPath})";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string VanillaMd5 { get; }
        public ModKitVersion ModKitVersion { get; }
    }
}

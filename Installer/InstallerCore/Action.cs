using System;
using System.IO;

namespace InstallerCore
{
    public abstract class Action
    {
        public abstract void Execute();
    }

    public class InstallModKit : Action
    {
        public InstallModKit(string catalogPath, string outputPath, string backupPath, string vanillaMd5, string patchVersion, Installable patch)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            BackupPath = backupPath;
            VanillaMd5 = vanillaMd5;
            PatchVersion = patchVersion;
            Patch = patch;
        }

        public override void Execute()
        {
            // apply patch. do this to a temp file because the patch itself might be referring to the file being replaced! (e.g. Gnomoria.exe)
            string tmpOutputPath = OutputPath + ".tmp";
            Patch.Install(tmpOutputPath);

            File.Replace(tmpOutputPath, OutputPath, BackupPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);
            catalog.DefaultRecord = new InstallRecord(versionString: PatchVersion, vanillaMd5: VanillaMd5);
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Install {PatchVersion} to {OutputPath} using {Patch}";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
        public string VanillaMd5 { get; }
        public string PatchVersion { get; }
        public Installable Patch { get; }
    }

    public class InstallStandalone : Action
    {
        public InstallStandalone(string catalogPath, string outputPath, string vanillaMd5, string patchVersion, Installable patch)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            VanillaMd5 = vanillaMd5;
            PatchVersion = patchVersion;
            Patch = patch;
        }

        public override void Execute()
        {
            // apply patch
            Patch.Install(OutputPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);
            catalog.Standalone = new InstallRecord(versionString: PatchVersion, vanillaMd5: VanillaMd5);
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Install {PatchVersion} to {OutputPath} using {Patch}";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string VanillaMd5 { get; }
        public string PatchVersion { get; }
        public Installable Patch { get; }
    }

    public class UninstallModKit : Action
    {
        public UninstallModKit(string catalogPath, string outputPath, string backupPath, string vanillaMd5, string patchVersion)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            BackupPath = backupPath;
            VanillaMd5 = vanillaMd5;
            PatchVersion = patchVersion;
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
            return $"Un-mod {PatchVersion} ({OutputPath})";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
        public string VanillaMd5 { get; }
        public string PatchVersion { get; }
    }

    public class UninstallStandalone : Action
    {
        public UninstallStandalone(string catalogPath, string outputPath, string vanillaMd5, string patchVersion)
        {
            CatalogPath = catalogPath;
            OutputPath = outputPath;
            VanillaMd5 = vanillaMd5;
            PatchVersion = patchVersion;
        }

        public override void Execute()
        {
            // delete patched executable
            File.Delete(OutputPath);

            // update catalog
            var catalog = InstallDb.LoadOrEmpty(CatalogPath);

            catalog.Standalone = null;
            catalog.Save(CatalogPath);
        }

        public override string ToString()
        {
            return $"Uninstall {PatchVersion} ({OutputPath})";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string VanillaMd5 { get; }
        public string PatchVersion { get; }
    }

    public class InstallModLoader : Action
    {
        public InstallModLoader(string modLoaderPath, string outputPath)
        {
            ModloaderPath = modLoaderPath;
            OutputPath = outputPath;
            BackupPath = outputPath + ".bak";
        }

        public override void Execute()
        {
            if ( File.Exists(ModloaderPath) )
            {
                if ( File.Exists(OutputPath) )
                {
                    // For replace to work, all directories need to be in the same volume
                    // copy modloader from installer path to game directory (with a temp name)
                    string temp = Path.Combine(OutputPath + Guid.NewGuid().ToString());
                    File.Copy(ModloaderPath, temp);
                    // replace existing (with backup), if exists
                    File.Replace(temp, OutputPath, BackupPath);
                }
                else 
                {
                    // copy modloader from given path to output path
                    File.Copy(ModloaderPath, OutputPath);
                }
            }
        }

        public override string ToString()
        {
            return $"Install Mod Loader to {OutputPath}";
        }

        public string ModloaderPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
    }
}

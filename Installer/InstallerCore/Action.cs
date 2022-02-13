using System;
using System.IO;

namespace InstallerCore
{
    public abstract class Action
    {
        private static readonly Logger _log = Logger.GetLogger;

        public void Execute()
        {
            _log.WriteLine(this.ToString());
            this.ExecuteImpl();
        }

        protected abstract void ExecuteImpl();
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

        protected override void ExecuteImpl()
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
            return $"$$ Install {PatchVersion} to {OutputPath} using {Patch}";
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

        protected override void ExecuteImpl()
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
            return $"$$ Install {PatchVersion} to {OutputPath} using {Patch}";
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

        protected override void ExecuteImpl()
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
            return $"$$ Un-mod {PatchVersion} ({OutputPath})";
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

        protected override void ExecuteImpl()
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
            return $"$$ Uninstall {PatchVersion} ({OutputPath})";
        }

        public string CatalogPath { get; }
        public string OutputPath { get; }
        public string VanillaMd5 { get; }
        public string PatchVersion { get; }
    }

    public class InstallModLoaderDependency : Action
    {
        private static readonly Logger _log = Logger.GetLogger;
        public InstallModLoaderDependency(string modLoaderPath, string outputPath, bool withBackup)
        {
            ModloaderPath = modLoaderPath;
            OutputPath = outputPath;
            BackupPath = outputPath + ".bak";
            WithBackup = withBackup;
        }

        protected override void ExecuteImpl()
        {
            this.CopyFile(ModloaderPath, OutputPath, (WithBackup ? BackupPath : null));
        }

        private void CopyFile(string source, string target, string backup)
        {
            if (File.Exists(source))
            {
                if (File.Exists(target))
                {
                    // For replace to work, all directories need to be in the same volume
                    // copy file from installer path to game directory (with a temp name)
                    string temp = Path.Combine(target + Guid.NewGuid().ToString());
                    File.Copy(source, temp);
                    // replace existing (with backup), if exists
                    File.Replace(temp, target, backup);
                }
                else
                {
                    // copy file from given path to output path
                    File.Copy(source, target);
                }
            }
            else
            {
                _log.log($"Tasked with copying a file but source file is missing: {source}");
            }
        }

        public override string ToString()
        {
            return $"$$ Install Mod Loader Dependency to {OutputPath}";
        }

        public string ModloaderPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
        public bool WithBackup { get; }
    }

    public class CopyModsAction : Action
    {
        public CopyModsAction(string modsPath, string outputPath)
        {
            ModsPath = modsPath;
            OutputPath = outputPath;
            BackupPath = outputPath + ".bak";
        }

        protected override void ExecuteImpl()
        {
            if (Directory.Exists(ModsPath))
            {
                if (Directory.Exists(OutputPath))
                {
                    // Do backup of existing
                    CopyFilesRecursively(OutputPath, BackupPath);
                }
                // copy mods from given path to output path
                CopyFilesRecursively(ModsPath, OutputPath);
                // Unblock the files from the target
                FileUnblocker.UnblockPath(OutputPath);
            }
        }

        private void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public override string ToString()
        {
            return $"$$ Copying Mods to {OutputPath}";
        }

        public string ModsPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
    }

    public class DeleteModsAction : Action
    {
        public DeleteModsAction(string modsPath, string outputPath)
        {
            ModsPath = modsPath;
            OutputPath = outputPath;
            BackupPath = outputPath + ".bak";
        }

        protected override void ExecuteImpl()
        {
            if (Directory.Exists(BackupPath))
            {
                Directory.Delete(BackupPath, true);
            }
            if (Directory.Exists(OutputPath))
            {
                Directory.Delete(OutputPath, true);
            }
        }

        public override string ToString()
        {
            return $"$$ Cleaning up Mods from {OutputPath} and {BackupPath}";
        }

        public string ModsPath { get; }
        public string OutputPath { get; }
        public string BackupPath { get; }
    }

    public class UninstallModLoaderDependency : Action
    {
        public UninstallModLoaderDependency(string outputPath)
        {
            OutputPath = outputPath;
            BackupPath = outputPath + ".bak";
        }

        protected override void ExecuteImpl()
        {
            if (File.Exists(BackupPath))
            {
                File.Delete(BackupPath);
            }
            if (File.Exists(OutputPath))
            {
                File.Delete(OutputPath);
            }
        }

        public override string ToString()
        {
            return $"$$ Uninstall Mod Loader Dependency from {OutputPath} and {BackupPath}";
        }
        public string OutputPath { get; }
        public string BackupPath { get; }
    }
}

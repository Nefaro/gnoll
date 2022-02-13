using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace InstallerCore
{
    public class ScanResult
    {
        public ScanResult(string gnollVersion, string gameVersion, Action[] availableActions, 
            bool patchAvailable, string oldStandaloneVersion, string newStandaloneVersion, string patchVersion)
        {
            ModKitVersion = gnollVersion;
            GameVersion = gameVersion;
            AvailableActions = availableActions;
            PatchAvailable = patchAvailable;
            OldStandaloneVersion = oldStandaloneVersion;
            NewStandaloneVersion = newStandaloneVersion;
            PatchVersion = patchVersion;
        }

        public string ModKitVersion { get; }
        public string GameVersion { get; }
        public string OldStandaloneVersion { get; }
        public string NewStandaloneVersion { get; }
        public Action[] AvailableActions { get; }
        public bool PatchAvailable { get; }

        public string PatchVersion { get; }
    }

    public class InstallerCore
    {
        private static readonly Logger _log = Logger.GetLogger;
        private static readonly string _modLoaderFile = "GnollModLoader.dll";
        private static readonly string _modLoaderDependency = "0Harmony.dll";
        private static readonly string _modsDirectory = "Gnoll Mods";


        public static ScanResult ScanGameInstall(string installDir, GamePatchDatabase gameDb)
        {
            _log.WriteLine($"About to scan game directory {installDir}");

            string gameExePath = Path.Combine(installDir, "Gnomoria.exe");
            string backupExePath = Path.Combine(installDir, "Gnomoria.orig.exe");
            string modLoaderTargetPath = Path.Combine(installDir, _modLoaderFile);
            string modLoaderSourcePath = Path.Combine(gameDb.PatchFolder, _modLoaderFile);
            string modsSourcePath = Path.Combine(gameDb.PatchFolder, _modsDirectory);
            string modsTargetPath = Path.Combine(installDir, _modsDirectory);

            string dependencyTargetPath = Path.Combine(installDir, _modLoaderDependency);
            string dependencySourcePath = Path.Combine(gameDb.PatchFolder, _modLoaderDependency);


            // Load installation catalog (json file)

            string catalogPath = Path.Combine(installDir, "gnoll-version.json");
            _log.WriteLine($"{catalogPath} exists? => {File.Exists(catalogPath)}");
            var catalog = InstallDb.LoadOrEmpty(catalogPath);

            bool isExeModded = (catalog.DefaultRecord != null);
            string moddedVersion = (catalog.DefaultRecord != null ? catalog.DefaultRecord.VersionString : null);

            // Figure out game version

            string vanillaGameMd5;
            string vanillaGamePath;

            if (!isExeModded)
            {
                vanillaGamePath = gameExePath;

                // https://stackoverflow.com/a/10520086
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(vanillaGamePath))
                    {
                        var hash = md5.ComputeHash(stream);
                        vanillaGameMd5 = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                        _log.WriteLine($"Game not modded; calculated vanillaGameMd5 = {vanillaGameMd5}");
                    }
                }
            }
            else
            {
                vanillaGameMd5 = catalog.DefaultRecord.VanillaMd5;
                vanillaGamePath = backupExePath;
                _log.WriteLine($"Game IS modded; reported vanillaGameMd5 = {vanillaGameMd5}");
            }

            GamePatchDatabase.GameEntry gameVersion = gameDb.GetGameEntryMd5Hash(vanillaGameMd5);

            // Construct actions to offer
            var actions = new List<Action>();
            var installable = gameDb.GetInstallablePatchIfAvailable( gameVersion, vanillaGamePath);

            // Check if current modkit version has been installed as stand-alone exe (trust the catalog)
            bool isStandaloneInstallPresent = false;
            bool isStandaloneUpToDate = false;
            if (catalog.Standalone != null)
            {
                _log.WriteLine($"Detected stand-alone installation of build {catalog.Standalone.VersionString}");
                isStandaloneInstallPresent = true;
                if ( installable.PatchVersion.Equals(catalog.Standalone.VersionString) )
                {
                    isStandaloneUpToDate = true;
                }
            }

            // offer action InstallModKit if game unmodded and recognized (patch available)
            if (!isExeModded)
            {
                if (installable != null)
                {
                    _log.WriteLine($"Game not modded & patch available => propose InstallModKit");
                    actions.Add(new InstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, installable.PatchVersion, installable));
                    if ( File.Exists(modLoaderSourcePath) )
                    {
                        actions.Add(new InstallModLoaderDependency(modLoaderSourcePath, modLoaderTargetPath, true));
                        actions.Add(new InstallModLoaderDependency(dependencySourcePath, dependencyTargetPath, false));
                        actions.Add(new UninstallModLoaderDependency(modLoaderTargetPath));
                        actions.Add(new UninstallModLoaderDependency(dependencyTargetPath));
                    }
                    else
                    {
                        _log.WriteLine($"Warning: Mod Loader not found; not installing");
                    }
                }
                else
                {
                    _log.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            else
            {
                _log.WriteLine($"Game modded => propose UninstallModKit");
                actions.Add(new UninstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, moddedVersion));
            }

            string standaloneUpToDateFilename = (!String.IsNullOrEmpty(installable.PatchVersion) ? $"Gnoll-{installable.PatchVersion}-{vanillaGameMd5.Substring(0, 8)}.exe" : null);
            string standaloneOldFilename = (catalog.Standalone != null && !String.IsNullOrEmpty(catalog.Standalone.VersionString) ? $"Gnoll-{catalog.Standalone.VersionString}-{vanillaGameMd5.Substring(0, 8)}.exe" : null);

            // offer action InstallStandalone if current version not present and patch available
            if (!isStandaloneInstallPresent || !isStandaloneUpToDate)
            {
                if (installable != null && standaloneUpToDateFilename != null)
                {
                    string standalonePath = Path.Combine(installDir, standaloneUpToDateFilename);
                    _log.WriteLine($"Game not stand-alone modded & patch available => propose InstallStandalone");
                    actions.Add(new InstallStandalone(catalogPath, standalonePath, vanillaGameMd5, installable.PatchVersion, installable));
                    if (File.Exists(modLoaderSourcePath))
                    {
                        actions.Add(new InstallModLoaderDependency(modLoaderSourcePath, modLoaderTargetPath, true));
                        actions.Add(new InstallModLoaderDependency(dependencySourcePath, dependencyTargetPath, false));
                        actions.Add(new UninstallModLoaderDependency(modLoaderTargetPath));
                        actions.Add(new UninstallModLoaderDependency(dependencyTargetPath));
                    }
                    else
                    {
                        _log.WriteLine($"Warning: Mod Loader not found; not installing");
                    }
                }
                else
                {
                    _log.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            if(isStandaloneInstallPresent)
            {

                string standalonePath = Path.Combine(installDir, standaloneUpToDateFilename);
                _log.WriteLine($"Game stand-alone modded => propose UninstallStandalone");
                actions.Add(new UninstallStandalone(catalogPath, standalonePath, vanillaGameMd5, standaloneOldFilename));
            }
            if ( Directory.Exists(modsSourcePath) )
            {
                actions.Add(new CopyModsAction(modsSourcePath, modsTargetPath));
                actions.Add(new DeleteModsAction(modsSourcePath, modsTargetPath));
            }
            else
            {
                _log.WriteLine($"Warning: Mods not included in {modsSourcePath}");
            }
            return new ScanResult(moddedVersion, gameVersion.Name, actions.ToArray(), patchAvailable: (installable != null),
                standaloneOldFilename, standaloneUpToDateFilename,(installable!=null? installable.PatchVersion: null));
        }
    }
}

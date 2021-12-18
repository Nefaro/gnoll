using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace InstallerCore
{
    public class ScanResult
    {
        public ScanResult(string gnollVersion, string gameVersion, Action[] availableActions, bool patchAvailable)
        {
            ModKitVersion = gnollVersion;
            GameVersion = gameVersion;
            AvailableActions = availableActions;
            PatchAvailable = patchAvailable;
        }

        public string ModKitVersion { get; }
        public string GameVersion { get; }
        public Action[] AvailableActions { get; }
        public bool PatchAvailable { get; }
    }

    public class ModKitVersion
    {
        public ModKitVersion(int buildNumber, string versionString)
        {
            BuildNumber = buildNumber;
            VersionString = versionString;
        }

        public int BuildNumber { get; }
        public string VersionString { get; }
    }

    public class PatchDatabase
    {
        private static readonly Logger _log = Logger.GetLogger;
        public Installable GetInstallableIfAvailable(int modKitBuildNumber, string gameMd5, string vanillaExePath)
        {
            string fullResourceName = $"G{modKitBuildNumber}_{gameMd5.Substring(0, 8)}.exe";
            var fullBytes = (byte[])Properties.Resources.ResourceManager.GetObject(fullResourceName);

            if (fullBytes != null)
            {
                return new CopyInstallable(fullBytes);
            }

            string patchResourceName = $"G{modKitBuildNumber}_{gameMd5.Substring(0, 8)}.xdelta";
            _log.log($"Looking for patch {patchResourceName}");
            var patchBytes = (byte[])Properties.Resources.ResourceManager.GetObject(patchResourceName);

            if (patchBytes != null)
            {
                return new PatchInstallable(patchBytes, vanillaExePath);
            }
            else
            {
                return null;
            }
        }
    }

    public class InstallerCore
    {
        private static readonly Logger _log = Logger.GetLogger;

        public static string[] FindGameInstallDirectories()
        {
            throw new NotImplementedException();
        }

        public static ScanResult ScanGameInstall(string installDir, ModKitVersion modKitVersion, GameDb gameDb, PatchDatabase patchDb)
        {
            _log.WriteLine($"About to scan game directory {installDir}");

            string gameExePath = Path.Combine(installDir, "Gnomoria.exe");
            string backupExePath = Path.Combine(installDir, "Gnomoria.orig.exe");

            // Load installation catalog (json file)

            string catalogPath = Path.Combine(installDir, "gnoll-version.json");
            _log.WriteLine($"{catalogPath} exists? => {File.Exists(catalogPath)}");
            var catalog = InstallDb.LoadOrEmpty(catalogPath);

            bool isExeModded = (catalog.DefaultRecord != null);

            // Check if current modkit version has been installed as stand-alone exe (trust the catalog)

            bool isUpToDateStandaloneInstallPresent = false;

            foreach (var record in catalog.Standalone)
            {
                if (record.ModKitBuildNumber == modKitVersion.BuildNumber)
                {
                    _log.WriteLine($"Detected stand-alone installation of build {record.ModKitBuildNumber}");
                    isUpToDateStandaloneInstallPresent = true;
                }
            }

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

            // Construct actions to offer

            var actions = new List<Action>();

            var installable = patchDb.GetInstallableIfAvailable(modKitVersion.BuildNumber, vanillaGameMd5, vanillaGamePath);

            // offer action InstallModKit if game unmodded and recognized (patch available)
            if (!isExeModded)
            {
                if (installable != null)
                {
                    _log.WriteLine($"Game not modded & patch available => propose InstallModKit");
                    actions.Add(new InstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, modKitVersion, installable));
                }
                else
                {
                    _log.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            else
            {
                _log.WriteLine($"Game modded => propose UninstallModKit");
                actions.Add(new UninstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, modKitVersion));
            }

            // offer action InstallStandalone if current version not present and patch available
            string standalonePath = Path.Combine(installDir, $"Gnoll-{modKitVersion.BuildNumber}-{vanillaGameMd5.Substring(0, 8)}.exe");
            if (!isUpToDateStandaloneInstallPresent)
            {
                if (installable != null)
                {
                    _log.WriteLine($"Game not stand-alone modded & patch available => propose InstallStandalone");
                    actions.Add(new InstallStandalone(catalogPath, standalonePath, vanillaGameMd5, modKitVersion, installable));
                }
                else
                {
                    _log.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            else
            {
                _log.WriteLine($"Game stand-alone modded => propose UninstallStandalone");
                actions.Add(new UninstallStandalone(catalogPath, standalonePath, vanillaGameMd5, modKitVersion));
            }

            string gnollVersion = (catalog.DefaultRecord != null ? catalog.DefaultRecord.ModKitBuildNumber.ToString() : null);
            string gameVersionString = gameDb.GetGameVersionStringByMd5Hash(vanillaGameMd5);

            return new ScanResult(gnollVersion, gameVersionString, actions.ToArray(), patchAvailable: (installable != null));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace InstallerCore
{
    public class ScanResult
    {
        public ScanResult(string gnollVersion, string gameVersion, Action[] availableActions)
        {
            ModKitVersion = gnollVersion;
            GameVersion = gameVersion;
            AvailableActions = availableActions;
        }

        public string ModKitVersion { get; }
        public string GameVersion { get; }
        public Action[] AvailableActions { get; }
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
        public Installable GetInstallableIfAvailable(int modKitBuildNumber, string gameMd5)
        {
            // TODO: PatchInstallable
            //string patchResourceName = $"G{modKitBuildNumber}_{gameMd5.Substring(0, 8)}.xdelta";
            //var patchBytes = (byte[])Properties.Resources.ResourceManager.GetObject(patchResourceName);

            string fullResourceName = $"G{modKitBuildNumber}_{gameMd5.Substring(0, 8)}.exe";
            var fullBytes = (byte[])Properties.Resources.ResourceManager.GetObject(fullResourceName);

            if (fullBytes != null)
            {
                return new CopyInstallable(fullBytes);
            }
            else
            {
                return null;
            }
        }
    }

    public class InstallerCore
    {
        public static string[] FindGameInstallDirectories()
        {
            throw new NotImplementedException();
        }

        public static ScanResult ScanGameInstall(string installDir, ModKitVersion modKitVersion, GameDb gameDb, PatchDatabase patchDb)
        {
            string gameExePath = Path.Combine(installDir, "Gnomoria.exe");
            string backupExePath = Path.Combine(installDir, "Gnomoria.orig.exe");

            // Load installation catalog (json file)

            string catalogPath = Path.Combine(installDir, "gnoll-version.json");
            var catalog = InstallDb.LoadOrEmpty(catalogPath);

            bool isExeModded = (catalog.DefaultRecord != null);

            // Check if current modkit version has been installed as stand-alone exe (trust the catalog)

            bool isUpToDateStandaloneInstallPresent = false;

            foreach (var record in catalog.Standalone)
            {
                if (record.ModKitBuildNumber == modKitVersion.BuildNumber)
                {
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
                    }
                }
            }
            else
            {
                vanillaGameMd5 = catalog.DefaultRecord.VanillaMd5;
                vanillaGamePath = backupExePath;
            }

            // Construct actions to offer

            var actions = new List<Action>();

            var installable = patchDb.GetInstallableIfAvailable(modKitVersion.BuildNumber, vanillaGameMd5);

            // offer action InstallModKit if game unmodded and recognized (patch available)
            if (!isExeModded)
            {
                if (installable != null)
                {
                    actions.Add(new InstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, modKitVersion, installable));
                }
                else
                {
                    Console.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            else
            {
                actions.Add(new UninstallModKit(catalogPath, gameExePath, backupExePath, vanillaGameMd5, modKitVersion));
            }

            // offer action InstallStandalone if current version not present and patch available
            string standalonePath = Path.Combine(installDir, $"Gnoll-{modKitVersion.BuildNumber}-{vanillaGameMd5.Substring(0, 8)}.exe");
            if (!isUpToDateStandaloneInstallPresent)
            {
                if (installable != null)
                {
                    actions.Add(new InstallStandalone(catalogPath, standalonePath, vanillaGameMd5, modKitVersion, installable));
                }
                else
                {
                    Console.WriteLine($"Warning: no patch available for game version {vanillaGameMd5}");
                }
            }
            else
            {
                actions.Add(new UninstallStandalone(catalogPath, standalonePath, vanillaGameMd5, modKitVersion));
            }

            string gnollVersion = (catalog.DefaultRecord != null ? catalog.DefaultRecord.ModKitBuildNumber.ToString() : null);
            string gameVersionString = gameDb.GetGameVersionStringByMd5Hash(vanillaGameMd5);

            return new ScanResult(gnollVersion, gameVersionString, actions.ToArray());
        }
    }
}

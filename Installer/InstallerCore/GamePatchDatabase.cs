using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InstallerCore
{
    public class GamePatchDatabase
    {
        private static readonly Logger _log = Logger.GetLogger;
        private static readonly string _patchFolderName = "Payloads";
        private static readonly string _patchDatabaseName = "patchinfo.json";
        private GameEntry[] _entries;
        private readonly string _patchFolder;

        public string PatchFolder { get => _patchFolder; }

        public GamePatchDatabase(string appFolder)
        {
            // The overall patch directory
            _log.log($"App folder: {appFolder}");
            this._patchFolder = Path.Combine(appFolder, _patchFolderName);
            _log.log($"Patch folder: {_patchFolder}");
            // Load the patch info 
            string jsonString =  File.ReadAllText(Path.Combine(_patchFolder, _patchDatabaseName));
            this._entries = JsonSerializer.Deserialize<GameEntry[]>(jsonString);
            foreach(var ent in this._entries)
            {
                _log.log($"Entry: {ent.Name} - {ent.Md5sum}");
                _log.log($"Entry patches: {ent.Patches.Count}");
            }
        }

        public class GameEntry
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("md5sum")]
            public string Md5sum { get; set; }

            [JsonPropertyName("patches")]
            public List<PatchEntry> Patches { get; set; }

            [JsonPropertyName("latestVersion")]
            public string LatestPatch { get; set; }
        }

        public class PatchEntry
        {
            [JsonPropertyName("versionString")]
            public string VersionString { get; set; }

            [JsonPropertyName("version")]
            public int Version { get; set; }

            [JsonPropertyName("filename")]
            public string Filename { get; set; }
        }

        public GameEntry GetGameEntryMd5Hash(string exeMd5)
        {
            foreach (var entry in _entries)
            {
                if (exeMd5 == entry.Md5sum)
                {
                    return entry;
                }
            }

            throw new KeyNotFoundException($"Unknown game version (md5 {exeMd5})");
        }
        public PatchInstallable GetInstallablePatchIfAvailable(GameEntry gameEntry, string vanillaExePath)
        {
            // if no patches defined, skip out
            if (gameEntry.Patches.Count < 0)
                return null;

            var latestPatch = this.GetLatestPatch(gameEntry);
            if (latestPatch == null)
                return null;
            var patchFile = Path.Combine(_patchFolder,latestPatch.Filename); 
            if (patchFile == null)
                return null;

            if ( !File.Exists(patchFile) )
            {
                _log.log($"Error: Cannot find patch file ( { patchFile })");
                throw new FileNotFoundException($"Cannot find patch file '{Path.GetFileName(patchFile)}'. Is the database corrupt?");
            }

            var patchBytes = File.ReadAllBytes(patchFile);
            if (patchBytes != null)
                return new PatchInstallable(patchBytes, vanillaExePath, latestPatch.VersionString);

            return null;
        }

        public PatchEntry GetLatestPatch(GameEntry gameEntry)
        {
            _log.log($"Finding patch for: {gameEntry.Name} - {gameEntry.Md5sum}");
            // try to find the latest patch
            if (!string.IsNullOrEmpty(gameEntry.LatestPatch))
            {
                foreach (var patch in gameEntry.Patches)
                {
                    if (patch.VersionString == gameEntry.LatestPatch)
                    {
                        var patchFile = Path.Combine(this._patchFolder, patch.Filename);
                        if (File.Exists(patchFile))
                        {
                            _log.log($"Picked (latest) patch: {patch.VersionString}");
                            return patch;
                        }
                    }
                }
            }
            // finding latest failed, offer any patch
            if (gameEntry.Patches.Count > 0)
            {
                // Frist sort by patch int version
                gameEntry.Patches.Sort(
                    delegate (PatchEntry x, PatchEntry y) { return y.Version.CompareTo(x.Version); });

                _log.log("Sorted patches:");
                foreach (var patch in gameEntry.Patches)
                {
                    _log.log($" -- {patch.VersionString}");
                }

                return gameEntry.Patches[0];
            }

            return null;
        }
    }
}

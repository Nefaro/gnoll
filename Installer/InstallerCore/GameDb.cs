using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InstallerCore
{
    public class GameDb
    {
        private GameDbEntry[] _entries;

        public GameDb()
        {
            byte[] jsonString = Properties.Resources.GameVersions;

            this._entries = JsonSerializer.Deserialize<GameDbEntry[]>(jsonString);
        }

        private class GameDbEntry
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("md5sum")]
            public string Md5sum { get; set; }
        }

        public string GetGameVersionStringByMd5Hash(string exeMd5)
        {
            foreach (var entry in _entries)
            {
                if (exeMd5 == entry.Md5sum)
                {
                    return entry.Name;
                }
            }

            throw new KeyNotFoundException($"Unknown game version (md5 {exeMd5})");
        }
    }
}

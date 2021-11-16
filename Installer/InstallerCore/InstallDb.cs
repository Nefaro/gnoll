using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InstallerCore
{
    public class InstallRecord
    {
        [JsonPropertyName("modKitBuildNumber")]
        public int ModKitBuildNumber { get; }

        [JsonPropertyName("vanillaMd5")]
        public string VanillaMd5 { get; }

        public InstallRecord(int modKitBuildNumber, string vanillaMd5)
        {
            ModKitBuildNumber = modKitBuildNumber;
            VanillaMd5 = vanillaMd5;
        }
    }

    public class InstallDb
    {
        [JsonPropertyName("default")]
        public InstallRecord DefaultRecord { get; set; }

        [JsonPropertyName("standalone")]
        public List<InstallRecord> Standalone { get; set; }

        public InstallDb()
        {
            Standalone = new List<InstallRecord>();
        }

        public static InstallDb LoadOrEmpty(string fileName)
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<InstallDb>(jsonString);
            }
            catch (FileNotFoundException)
            {
                return new InstallDb();
            }
        }

        public void Save(string fileName)
        {
            string tmpFileName = fileName + ".tmp";

            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(this);
            File.WriteAllBytes(tmpFileName, jsonUtf8Bytes);

            File.Replace(tmpFileName, fileName, destinationBackupFileName: null);
        }
    }
}

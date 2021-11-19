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

        public void Save(string fileName, StreamWriter logFile)
        {
            string tmpFileName = fileName + ".tmp";

            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(this);
            File.WriteAllBytes(tmpFileName, jsonUtf8Bytes);

            logFile.WriteLine($"Updating {fileName}");
            File.Delete(fileName);      // If the file to be deleted does not exist, no exception is thrown.
            File.Move(tmpFileName, fileName);
        }
    }
}

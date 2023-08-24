using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game;

namespace GnollModLoader
{
    /**
     * Responsible for saving and loading a savegame companion file 
     * containing gnoll specific game data, ie stuff that mods need to save
     *
     */
    public class SaveGameManager
    {
        private readonly static string SAVEFILE_PATH = GnomanEmpire.SaveFolderPath("Gnoll\\Worlds\\");
        private readonly static string SAVEFILE_NAME_FORMAT = "{0}.gnoll.json";

        private static Dictionary<string, Saver> _modSavers = new Dictionary<string, Saver>();

        public SaveGameManager() { 

        }

        public Saver SaverForMod(string modName)
        {
            Saver saver;
            if (_modSavers.TryGetValue(modName, out saver))
            {
                return saver;
            }
            _modSavers[modName] = new Saver(modName);
            return _modSavers[modName];
        }

        internal void HookAfterGameSaved()
        {
            saveModlistFile();
        }

        private void saveModlistFile()
        {
            try
            {
                var modData = _modSavers.Values.ToDictionary(svr => svr.ModName, svr => svr.SaveData);
                if (_modSavers.Count == 0 || modData.Count == 0 ) 
                {
                    Logger.Log("Save data empty, skipping Gnoll save");
                    return;
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(modData, Newtonsoft.Json.Formatting.Indented);
                var fileName = createSaveFileName();
                Logger.Log("Creating Gnoll save: {0}", fileName);
                System.IO.File.WriteAllText(fileName, json);
            }
            catch (Exception e)
            {
                Logger.Error("Creating Gnoll save failed!");
                Logger.Error($"-- {e}");
            }
        }

        private string createSaveFileName()
        {
            return SAVEFILE_PATH + String.Format(SAVEFILE_NAME_FORMAT, GnomanEmpire.Instance.CurrentWorld);
        }

        public class Saver
        {
            public Dictionary<object, object> SaveData { get => _saveData; set => _saveData = value; }
            public string ModName => _modName;


            private Dictionary<object, object> _saveData = new Dictionary<object, object>();

            private readonly string _modName;
            public Saver(string forMod)
            {
                _modName = forMod;
            }

            public void Save(Dictionary<object, object> saveData)
            {
                _saveData = saveData;
            }
        }
    }

}

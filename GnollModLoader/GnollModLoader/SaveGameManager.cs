using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader.Model;

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
        private static Dictionary<string, Dictionary<object, object>> _saveData = new Dictionary<string, Dictionary<object, object>>();

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

        public Loader LoaderForMod(string modName)
        {
            return new Loader(modName);
        }

        internal void HookAfterGameSaved()
        {
            //The savegamemanager save hook *needs* to be called as the last one
            //else it will miss the save data that follows
            saveModDataFile();
            // Need to collect the data again on next cycle
            _saveData.Clear();
        }

        internal void HookAfterGameLoaded()
        {
            loadModDataFile();
        }

        private void saveModDataFile()
        {
            try
            {
                var fileName = createSaveFileName();
                if (_modSavers.Count == 0 || _saveData.Count == 0 ) 
                {
                    Logger.Log("Save data empty, skipping Gnoll save");
                    // clean up the "old" save, if it exists
                    if ( File.Exists(fileName) )
                    {
                        File.Delete(fileName);
                    }
                    return;
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(_saveData, Newtonsoft.Json.Formatting.Indented);
                Logger.Log("Creating Gnoll save: {0}", fileName);
                System.IO.File.WriteAllText(fileName, json);
            }
            catch (Exception e)
            {
                Logger.Error("Creating Gnoll save failed!");
                Logger.Error($"-- {e}");
            }
        }

        private void loadModDataFile()
        {
            try
            {
                var fileName = createSaveFileName();
                if (File.Exists(fileName) )
                {
                    Logger.Log("Loading mod data from {0}", fileName);
                    string json = File.ReadAllText(fileName);
                    _saveData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<object, object>>>(json);
                }
                else
                {
                    // if no data to load, reset
                    _saveData = new Dictionary<string, Dictionary<object, object>>();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Loading Gnoll save failed!");
                Logger.Error($"-- {e}");
            }
        }

        private string createSaveFileName()
        {
            return SAVEFILE_PATH + String.Format(SAVEFILE_NAME_FORMAT, GnomanEmpire.Instance.CurrentWorld);
        }

        public class Saver
        {
            private readonly string _modName;
            public Saver(string forMod)
            {
                _modName = forMod;
            }

            public void Save(Dictionary<object, object> saveData)
            {
                _saveData[this._modName] = saveData;
            }
        }

        public class Loader
        {
            private readonly string _modName;
            public Loader(string forMod)
            {
                _modName = forMod;
            }
            public Dictionary<object, object> Load()
            {
                return _saveData[this._modName];
            }
        }
    }

}

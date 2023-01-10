using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using GnollModLoader.Model;

namespace GnollModLoader
{
    /**
     * Responsible for managing the lifecycle and enable/disable events for mods
     * 
     */
    public class ModManager
    {
        private readonly static string MODLIST_FILE = GnomanEmpire.SaveFolderPath("Gnoll") + "\\modlist.json";

        private readonly List<IGnollMod> _modsList = new List<IGnollMod>();
        private readonly HookManager _hookManager;
        private readonly Modlist _modlist;
        private readonly ISet<String> _waitingRestart;
        private readonly Patcher _patcher;
        public List<IGnollMod> Mods { get { return _modsList; } }

        public ModManager(HookManager hookManager, Patcher patcher)
        {
            this._hookManager = hookManager;
            this._patcher = patcher;
            this._modlist = this.tryLoadModlistFile();
            this._waitingRestart = new HashSet<String>();
        }

        public void RegisterMod(IGnollMod mod)
        {
            _modsList.Add(mod);
            if ( this.IsModEnabled(mod))
            {
                mod.OnEnable(this._hookManager);
                if ( mod is IHasDirectPatch )
                {
                    Logger.Log("-- Mod has a direct patch");
                    var withPatch = mod as IHasDirectPatch;
                    withPatch.ApplyPatch(this._patcher);
                }
            }
        }

        public bool IsModEnabled(IGnollMod mod)
        {
            if ( mod == null )
            {
                return false;
            }
            if ( this._modlist.ModStatus.TryGetValue(mod.Name, out bool isEnabled) )
            {
                return isEnabled;
            }
            if ( mod.IsDefaultEnabled() )
            {
                this.storeModStatus(mod, true);
                return true;
            }
            return false;
        }

        public bool IsWaitingRestart(IGnollMod mod)
        {
            return this._waitingRestart.Contains(mod.Name);
        }

        public void EnableMod(IGnollMod mod)
        {
            try
            {
                Logger.Log($"-- Enabling mod {mod.Name} ...");
                mod.OnEnable(this._hookManager);
                Logger.Log($"-- Enabling mod {mod.Name} ... DONE");
                this.storeModStatus(mod, true);
                if ( mod.NeedsRestartOnToggle() )
                {
                    this._waitingRestart.Add(mod.Name);
                }
            }
            catch(Exception e)
            {
                Logger.Error($"!! Enabling mod {mod.Name} failed; try restarting the game");
                Logger.Error($"!! -- {e}");
            }
        }

        public void DisableMod(IGnollMod mod)
        {
            try
            {
                Logger.Log($"-- Disabling mod {mod.Name} ...");
                mod.OnDisable(this._hookManager);
                Logger.Log($"-- Disabling mod {mod.Name} ... DONE");
                this.storeModStatus(mod, false);
                if (mod.NeedsRestartOnToggle())
                {
                    this._waitingRestart.Add(mod.Name);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"!! Disabling mod {mod.Name} failed; try restarting the game");
                Logger.Error($"!! -- {e}");
            }
        }


        private Modlist tryLoadModlistFile()
        {
            if (System.IO.File.Exists(MODLIST_FILE))
            {
                Logger.Log("Loading mod states from {0}", MODLIST_FILE);
                string json = System.IO.File.ReadAllText(MODLIST_FILE);
                Modlist modlist = Newtonsoft.Json.JsonConvert.DeserializeObject<Modlist>(json);
                return modlist;
            }
            else
            {
                return new Modlist();
            }
        }

        private void storeModStatus(IGnollMod mod, bool enabled)
        {
            this._modlist.ModStatus[mod.Name] = enabled;
            this.saveModlistFile();
        }

        private void saveModlistFile()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this._modlist, Newtonsoft.Json.Formatting.Indented);
            Logger.Log("-- -- Saving modlist to {0}", MODLIST_FILE);
            System.IO.File.WriteAllText(MODLIST_FILE, json);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace GnollMods.SeasonalChange
{
    class ModMain : IGnollMod
    {
        private static readonly string WORKSHOP_PATH_PREFIX = "Gnoll";
        private static readonly string ASSEMBLY = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string DLL = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string DATA_PATH_PREFIX = "Data";
        private static readonly string DATA_FOLDER = Path.Combine(ASSEMBLY, DLL, DATA_PATH_PREFIX);
        public string Name { get { return "SeasonalChange"; } }
        public string Description { get { return "Changes the textures depending on the current season"; } }

        public string BuiltWithLoaderVersion { get { return "1.14.1"; } }

        public int RequireMinPatchVersion { get { return 13; } }

        private IList<string> _modsWithSeasonPaths;
        private ModsLogger _logger;

        public ModMain()
        {
            _logger = ModsLogger.getLogger(this);
            _modsWithSeasonPaths = new List<string>();
        }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHudInit;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameHUDInit -= HookManager_InGameHudInit;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        private void HookManager_InGameHudInit(InGameHUD inGameHUD, Manager manager)
        {
            this.scanWorkshopMods();
            // On first load, replace with current seasonal sprites
            this.switchSpritesToSeason(Game.GnomanEmpire.Instance.Region.Season().ToString().ToLower());

            if ( isNewGame() )
            {
                // Need to do some more steps when a new game is started
                // because the processing path is different and system is
                // in a different state
                this.callTextureRepack();
            }

            Game.GnomanEmpire.Instance.Region.OnSeasonChange += onSeasonChange;
        }

        private void scanWorkshopMods()
        {
            _logger.Log("Scanning mods for seasonal graphics");
            var modList = new List<ModFolder>(GnomanEmpire.Instance.GameDefs.ModFolders).Select(mod => mod.Folder).ToList();
            foreach (var mod in modList)
            {
                this.scanForSprites(mod);
                // Gog has a different format
                if (!Path.IsPathRooted(mod)) 
                {
                    this.scanForSprites(Path.Combine("Mods", mod)); 
                }
            }
        }

        private void scanForSprites(string mod)
        {
            var dataDirectory = Path.Combine(mod, WORKSHOP_PATH_PREFIX, DLL, DATA_PATH_PREFIX);
            if (Directory.Exists(dataDirectory))
            {
                _logger.Log($"++ Found seasonal graphics directory: {dataDirectory}");
                this._modsWithSeasonPaths.Add(dataDirectory);
            }
        }

        private bool isNewGame()
        {
            var loadScreen = Game.GnomanEmpire.Instance.loadScreen_0;
            return (loadScreen != null && 
                loadScreen.GetType() == typeof(Game.GUI.CreateWorldLoadScreen)&&
                Game.GnomanEmpire.Instance.Region.TotalTime() < 1 &&
                Game.GnomanEmpire.Instance.Region.Sunrise() > Game.GnomanEmpire.Instance.Region.Time.Value);
        }

        private void onSeasonChange(object sender, System.EventArgs e)
        {
            // good habbit to encapsulate your custom code
            // so it wont crash the whole game
            try
            {
                string season = Game.GnomanEmpire.Instance.Region.Season().ToString().ToLower();
                _logger.Log("++ Season change event called; new season: " + season);
                this.switchSpritesToSeason(season);
                // Need to call the repack manually to replace the ingame sprites
                this.callTextureRepack();
            }
            catch (Exception)
            {
                // If we reach thisfar, then exception should not happen... but
                // The game might be graphically in an undefined state, but not crashed
                _logger.Error("-- Couldn't switch sprites ");
            }
        }

        private void switchSpritesToSeason(string season)
        {
            // Repopulate the "gamedefs -> sprite" map
            Game.GnomanEmpire.Instance.gameDefs_0.method_12();

            // Load the (default) Sprites contained within our own mod
            loadCustomSprites(Path.Combine(DATA_FOLDER, season), "default.png");

            // Load the Sprites contained within workshop mods
            foreach (var dataFolder in this._modsWithSeasonPaths)
            {
                var seasonDirectory = Path.Combine(dataFolder, season);
                if ( Directory.Exists(seasonDirectory) ) 
                {
                    foreach (var spriteFile in Directory.GetFiles(seasonDirectory, "*.png") )
                    {
                        loadCustomSprites(seasonDirectory, Path.GetFileName(spriteFile));
                    }
                }
            }

        }

        private void loadCustomSprites(string spriteDirectory, string fileName)
        {
            var seasonalSprite = Path.Combine(spriteDirectory, fileName);
            // In case our own spritesheet is missing,
            // we can still reload everything
            // That way the original spritesheets are loaded and everything is still fine
            if (File.Exists(seasonalSprite))
            {
                _logger.Log($"++ Loading sprites: {seasonalSprite}");
                // Load our own custom spritesheet
                Game.GnomanEmpire.Instance.gameDefs_0.method_81(seasonalSprite);

                // Find all spritesheets that are name after the filename and replace them with our own
                foreach (var key in Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4.Keys.ToList())
                {
                    if (key.EndsWith(fileName) && !key.Equals(seasonalSprite))
                    {
                        Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4[key] =
                            Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4[seasonalSprite];
                    }
                }
            }
        }

        private void callTextureRepack()
        {
            // Do the internal book-keeping and texture-keeping
            Game.GnomanEmpire.Instance.gameDefs_0.PackTextures();
            Game.GnomanEmpire.Instance.world_0.Region.Map.TileSet.mTexture = Game.GnomanEmpire.Instance.gameDefs_0.Tilesheet;
        }

    }
}

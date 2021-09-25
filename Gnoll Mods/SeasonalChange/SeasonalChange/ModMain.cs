using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace GnollMods.SeasonalChange
{
    class ModMain : IGnollMod
    {
        private static readonly string assembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string dll = Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string dataFolder = assembly + "/" + dll + "/Data";

        public string Name { get { return "SeasonalChange"; } }
        public string Description { get { return "Changes the textures depending on the current season"; } }

        public string BuiltWithLoaderVersion { get { return "G1.1"; } }

        public int RequireMinPatchVersion { get { return 1; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHudInit;
        }

        private void HookManager_InGameHudInit(InGameHUD inGameHUD, Manager manager)
        {
            // On first load, replace with current seasonal sprites
            this.SwitchSpritesToSeason(Game.GnomanEmpire.Instance.Region.Season().ToString().ToLower());

            if ( IsNewGame() )
            {
                // Need to do some more steps when a new game is started
                // because the processing path is different and system is
                // in a different state
                this.CallTextureRepack();
            }

            Game.GnomanEmpire.Instance.Region.OnSeasonChange += OnSeasonChange;
        }

        private bool IsNewGame()
        {
            var loadScreen = Game.GnomanEmpire.Instance.loadScreen_0;
            return (loadScreen != null && 
                loadScreen.GetType() == typeof(Game.GUI.CreateWorldLoadScreen)&&
                Game.GnomanEmpire.Instance.Region.TotalTime() < 1 &&
                Game.GnomanEmpire.Instance.Region.Sunrise() > Game.GnomanEmpire.Instance.Region.Time.Value);
        }

        private void OnSeasonChange(object sender, System.EventArgs e)
        {
            // good habbit to encapsulate your custom code
            // so it wont crash the whole game
            try
            {
                string season = Game.GnomanEmpire.Instance.Region.Season().ToString().ToLower();
                System.Console.WriteLine("-- Season change event called; new season: " + season);
                this.SwitchSpritesToSeason(season);
                // Need to call the repack manually to replace the ingame sprites
                this.CallTextureRepack();
            }
            catch (Exception)
            {
                // If we reach thisfar, then exception should not happen... but
                // The game might be graphically in an undefined state, but not crashed
                System.Console.WriteLine("-- Couldn't switch sprites ");
            }
        }

        private void SwitchSpritesToSeason(string season)
        {
            string seasonalSprite = dataFolder + "/" + "default-" + season + ".png";
            // Repopulate the "gamedefs -> sprite" map
            Game.GnomanEmpire.Instance.gameDefs_0.method_12();

            // In case our own spritesheet is missing,
            // we can still reload everything
            // That way the original spritesheets are loaded and everything is still fine
            if (File.Exists(seasonalSprite))
            {
                System.Console.WriteLine("-- Switching season sprites to: " + season);

                // Load our own custom spritesheet
                Game.GnomanEmpire.Instance.gameDefs_0.method_81(seasonalSprite);

                // Find all spritesheets that are "default.png" and replace them with our own
                // Note: our do not end with "default.png" so we are safe from overwrite/rewrite
                foreach (var key in Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4.Keys.ToList())
                {
                    if (key.EndsWith("default.png"))
                    {                       
                        Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4[key] =
                            Game.GnomanEmpire.Instance.gameDefs_0.dictionary_4[seasonalSprite];
                    }
                }
            }
        }

        private void CallTextureRepack()
        {
            // Do the internal book-keeping and texture-keeping
            Game.GnomanEmpire.Instance.gameDefs_0.PackTextures();
            Game.GnomanEmpire.Instance.world_0.Region.Map.TileSet.mTexture = Game.GnomanEmpire.Instance.gameDefs_0.Tilesheet;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;
using Game.AudioManager;
using irrKlang.Net4;
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

        public string BuiltWithLoaderVersion { get { return "G1.13"; } }

        public int RequireMinPatchVersion { get { return 13; } }

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
namespace GnollMods.RainSFX
{
    class ModMain : IGnollMod
    {
        private static readonly string assembly = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static readonly string dll = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private static readonly string modFolder = Path.Combine(assembly, dll);
        private static readonly string dataFolder = Path.Combine(modFolder, "Data");
        private static readonly string rainSoundPath = Path.Combine(modFolder, "RainSFX", "rain03.wav");

        private bool isRaining = false;
        private ISoundEngine soundEngine;

        public string Name { get { return "RainSFX"; } }
        public string Description { get { return "Plays rain sound effects when it's raining in the game"; } }

        public string BuiltWithLoaderVersion { get { return "G1.14"; } }

        public int RequireMinPatchVersion { get { return 14; } }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.Update += HookManager_Update;

            // Initialize the sound engine
            soundEngine = new ISoundEngine();
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.Update -= HookManager_Update;

            // Release resources when disabling the mod
            soundEngine.Dispose();
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        private void HookManager_Update(double dt)
        {
            // Check if it's raining and play/stop the rain sound accordingly
            if (Game.GnomanEmpire.Instance.Region.Rain && !isRaining)
            {
                isRaining = true;
                PlayRainSound();
            }
            else if (!Game.GnomanEmpire.Instance.Region.Rain && isRaining)
            {
                isRaining = false;
                // Stop the rain sound if it's playing
                StopRainSound();
            }
        }

        private void PlayRainSound()
        {
            // Check if the rain sound file exists
            if (File.Exists(rainSoundPath))
            {
                Console.WriteLine("-- Playing rain sound");

                // Play the rain sound using irrklang.Net4
                soundEngine.Play2D(rainSoundPath, false, false);
            }
            else
            {
                Console.WriteLine("-- Rain sound file not found: " + rainSoundPath);
            }
        }

        private void StopRainSound()
        {
            Console.WriteLine("-- Stopping rain sound");

            // Stop all sounds
            soundEngine.StopAllSounds();
        }
    }
}

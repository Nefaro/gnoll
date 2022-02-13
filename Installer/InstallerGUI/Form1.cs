using InstallerCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InstallerGUI
{
    public partial class Form1 : Form
    {
        private InstallerCore.Action _installModkitAction, 
            _installStandaloneAction,
            _uninstallModkitAction, 
            _uninstallStandaloneAction,
            _copyModsAction,
            _deleteModsAction
            ;
        private List<InstallerCore.Action> _installModLoaderDependencies = new List<InstallerCore.Action>();
        private List<InstallerCore.Action> _uninstallModLoaderDependencies = new List<InstallerCore.Action>();

        private static readonly Logger _log = InstallerCore.Logger.GetLogger;
        private static readonly string _appName = $"Gnoll Installer (v1.10.0)";
        private readonly GamePatchDatabase _gameDb;

        public Form1()
        {
            _log.log("Running Gnoll Installer ...");
            try
            {
                InitializeComponent();
                DPI_Per_Monitor.TryEnableDPIAware(this, SetUserFonts);
                this.Text = _appName;
                versionLabel.Text = $"{_appName} by Minexew && Nefaro";
                _gameDb = new GamePatchDatabase(AppContext.BaseDirectory);
            }
            catch(Exception e)
            {
                _log.log("Running Gnoll Installer ... FAILED");
                _log.log(e.ToString());
                MessageBox.Show($"Cannot run Gnoll Installer: \r\nERROR: {e.Message}",
                    "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
        void SetUserFonts(float scaleFactorX, float scaleFactorY)
        {
            var OldFont = Font;
            Font = new Font(OldFont.FontFamily, 11f * scaleFactorX, OldFont.Style, GraphicsUnit.Pixel);
            OldFont.Dispose();
        }
        
        protected override void DefWndProc(ref Message m)
        {
            DPI_Per_Monitor.Check_WM_DPICHANGED_WM_NCCREATE(SetUserFonts, m, this.Handle);
            base.DefWndProc(ref m);
        }

        private void RescanGame()
        {
            try
            {
                installModkitButton.Enabled = false;
                installStandaloneButton.Enabled = false;
                uninstallModkitButton.Enabled = false;
                uninstallStandaloneButton.Enabled = false;
                copyModsButton.Enabled = false;
                gameVersionLabel.Text = "?";
                standaloneVersion.Text = "?";
                selectedPatchVersion.Text = "???";

                string gameDir = gamePathInput.Text;

                var res = InstallerCore.InstallerCore.ScanGameInstall(gameDir, _gameDb);

                _log.log($"Available patch {res.GameVersion}");

                string gameVersionStr = res.GameVersion;

                if (res.ModKitVersion != null)
                {
                    gameVersionStr += ", Gnoll " + res.ModKitVersion;
                }
                else
                {
                    gameVersionStr += " unmodded";
                }

                gameVersionLabel.Text = gameVersionStr;

                if (res.OldStandaloneVersion != null)
                {
                    standaloneVersion.Text = res.OldStandaloneVersion;
                }
                else
                {
                    standaloneVersion.Text = "Standalone executable not found";
                }

                if (!res.PatchAvailable)
                {
                    MessageBox.Show("No patch available for this game version", "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else 
                {
                    selectedPatchVersion.Text = $"Gnoll {res.PatchVersion}";
                }

                // Ugh, this code is a dumpster fire... at least it's straightforward

                foreach (var action in res.AvailableActions)
                {
                    if (action is InstallModKit)
                    {
                        installModkitButton.Enabled = true;
                        _installModkitAction = action;
                    }

                    if (action is UninstallModKit)
                    {
                        uninstallModkitButton.Enabled = true;
                        _uninstallModkitAction = action;
                    }

                    if (action is InstallStandalone)
                    {
                        installStandaloneButton.Enabled = true;
                        _installStandaloneAction = action;
                    }

                    if (action is UninstallStandalone)
                    {
                        uninstallStandaloneButton.Enabled = true;
                        _uninstallStandaloneAction = action;
                    }

                    if (action is InstallModLoaderDependency)
                    {
                        _installModLoaderDependencies.Add(action);
                    }
                    if (action is UninstallModLoaderDependency)
                    {
                        _uninstallModLoaderDependencies.Add(action);
                    }

                    if (action is CopyModsAction)
                    {
                        _copyModsAction = action;
                    }
                    if ( action is DeleteModsAction)
                    {
                        _deleteModsAction = action;
                    }
                }
                copyModsButton.Enabled = _copyModsAction != null && (uninstallStandaloneButton.Enabled || uninstallModkitButton.Enabled);
                uninstallAll.Enabled = !String.IsNullOrEmpty(gameDir);
            }
            catch(Exception e)
            {
                // bad stuff
                _log.log("Error: Could not identify game version");
                _log.log(e.ToString());
                MessageBox.Show($"Could not identify game version, no patch available: \r\nERROR: {e.Message}", 
                    "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowOk()
        {
            MessageBox.Show("Great Success!", "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void browseForGame_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                gamePathInput.Text = dialog.FileName;

                RescanGame();
            }
        }

        // DRY as fuck...

        private void installModkitButton_Click(object sender, EventArgs e)
        {
            try
            {
                _installModkitAction.Execute();
                if ( _installModLoaderDependencies.Count > 0 )
                {
                    _log.log("Installing modloader dependencies");
                    foreach(var action in _installModLoaderDependencies)
                    {
                        action.Execute();
                    }
                }
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void copyMods_Click(object sender, EventArgs e)
        {
            try
            {
                if (_copyModsAction != null)
                {
                    _log.log("Copying mods ...");
                    _copyModsAction.Execute();
                }
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void installStandaloneButton_Click(object sender, EventArgs e)
        {
            try
            {
                _installStandaloneAction.Execute();
                if (_installModLoaderDependencies.Count > 0)
                {
                    _log.log("Installing modloader dependencies");
                    foreach (var action in _installModLoaderDependencies)
                    {
                        action.Execute();
                    }
                }
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void uninstallModkitButton_Click(object sender, EventArgs e)
        {
            try
            {
                _uninstallModkitAction.Execute();
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void uninstallStandaloneButton_Click(object sender, EventArgs e)
        {
            try
            {
                _uninstallStandaloneAction.Execute();
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void uninstallAll_Click(object sender, EventArgs e)
        {
            try
            {
                _log.log("Uninstalling everything ...");
                if (_deleteModsAction != null)
                {
                    _deleteModsAction.Execute();
                }
                if (_uninstallModLoaderDependencies.Count > 0)
                {
                    foreach (var action in _uninstallModLoaderDependencies)
                    {
                        action.Execute();
                    }
                }
                if (_uninstallModkitAction != null)
                {
                    _uninstallModkitAction.Execute();
                }
                if (_uninstallStandaloneAction != null)
                {
                    _uninstallStandaloneAction.Execute();
                }
                ShowOk();
            }
            finally
            {
                RescanGame();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/Nefaro/gnoll");
        }
    }
}

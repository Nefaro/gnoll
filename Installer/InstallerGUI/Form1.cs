using InstallerCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InstallerGUI
{
    public partial class Form1 : Form
    {
        private static ModKitVersion modKitVersion = new ModKitVersion(1700, "G1.7");

        private InstallerCore.Action _installModkitAction, _installStandaloneAction,
            _uninstallModkitAction, _uninstallStandaloneAction;

        private readonly StreamWriter _logFile;

        public Form1()
        {
            InitializeComponent();
            DPI_Per_Monitor.TryEnableDPIAware(this, SetUserFonts);

            versionLabel.Text = $"Gnoll {modKitVersion.VersionString} by Nefaro && Minexew";

            _logFile = new StreamWriter("GnollInstaller.log", append: true);
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
            installModkitButton.Enabled = false;
            installStandaloneButton.Enabled = false;
            uninstallModkitButton.Enabled = false;
            uninstallStandaloneButton.Enabled = false;
            gameVersionLabel.Text = "?";

            var gameDb = new GameDb();
            var patchDb = new PatchDatabase();

            string gameDir = gamePathInput.Text;

            var res = InstallerCore.InstallerCore.ScanGameInstall(gameDir, modKitVersion, gameDb, patchDb, _logFile);

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

            if (!res.PatchAvailable)
            {
                MessageBox.Show("No patch available for this game version", "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
        }

        private void ShowOk()
        {
            MessageBox.Show("That went well...", "Gnoll", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                _installModkitAction.Execute(_logFile);
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
                _installStandaloneAction.Execute(_logFile);
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
                _uninstallModkitAction.Execute(_logFile);
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
                _uninstallStandaloneAction.Execute(_logFile);
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

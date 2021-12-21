
namespace InstallerGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gamePathInput = new System.Windows.Forms.TextBox();
            this.browseForGame = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gameVersionLabel = new System.Windows.Forms.TextBox();
            this.installModkitButton = new System.Windows.Forms.Button();
            this.installStandaloneButton = new System.Windows.Forms.Button();
            this.uninstallModkitButton = new System.Windows.Forms.Button();
            this.uninstallStandaloneButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.standaloneVersion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.selectedPatch = new System.Windows.Forms.Label();
            this.selectedPatchVersion = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.ForestGreen;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::InstallerGUI.Properties.Resources.banner;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 83);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game install path:";
            // 
            // gamePathInput
            // 
            this.gamePathInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamePathInput.Location = new System.Drawing.Point(3, 3);
            this.gamePathInput.Name = "gamePathInput";
            this.gamePathInput.ReadOnly = true;
            this.gamePathInput.Size = new System.Drawing.Size(259, 20);
            this.gamePathInput.TabIndex = 2;
            // 
            // browseForGame
            // 
            this.browseForGame.AutoSize = true;
            this.browseForGame.Dock = System.Windows.Forms.DockStyle.Right;
            this.browseForGame.Location = new System.Drawing.Point(282, 3);
            this.browseForGame.Name = "browseForGame";
            this.browseForGame.Size = new System.Drawing.Size(69, 24);
            this.browseForGame.TabIndex = 3;
            this.browseForGame.Text = "Browse...";
            this.browseForGame.UseVisualStyleBackColor = true;
            this.browseForGame.Click += new System.EventHandler(this.browseForGame_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Detected version:";
            // 
            // gameVersionLabel
            // 
            this.gameVersionLabel.Location = new System.Drawing.Point(119, 3);
            this.gameVersionLabel.Name = "gameVersionLabel";
            this.gameVersionLabel.ReadOnly = true;
            this.gameVersionLabel.Size = new System.Drawing.Size(232, 20);
            this.gameVersionLabel.TabIndex = 5;
            // 
            // installModkitButton
            // 
            this.installModkitButton.AutoSize = true;
            this.installModkitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.installModkitButton.Enabled = false;
            this.installModkitButton.Location = new System.Drawing.Point(3, 3);
            this.installModkitButton.Name = "installModkitButton";
            this.installModkitButton.Size = new System.Drawing.Size(145, 59);
            this.installModkitButton.TabIndex = 6;
            this.installModkitButton.Text = "Mod Gnomoria.exe";
            this.installModkitButton.UseVisualStyleBackColor = true;
            this.installModkitButton.Click += new System.EventHandler(this.installModkitButton_Click);
            // 
            // installStandaloneButton
            // 
            this.installStandaloneButton.AutoSize = true;
            this.installStandaloneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.installStandaloneButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.installStandaloneButton.Enabled = false;
            this.installStandaloneButton.Location = new System.Drawing.Point(3, 68);
            this.installStandaloneButton.Name = "installStandaloneButton";
            this.installStandaloneButton.Size = new System.Drawing.Size(145, 23);
            this.installStandaloneButton.TabIndex = 7;
            this.installStandaloneButton.Text = "Install stand-alone";
            this.installStandaloneButton.UseVisualStyleBackColor = true;
            this.installStandaloneButton.Click += new System.EventHandler(this.installStandaloneButton_Click);
            // 
            // uninstallModkitButton
            // 
            this.uninstallModkitButton.AutoSize = true;
            this.uninstallModkitButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uninstallModkitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uninstallModkitButton.Enabled = false;
            this.uninstallModkitButton.Location = new System.Drawing.Point(154, 3);
            this.uninstallModkitButton.Name = "uninstallModkitButton";
            this.uninstallModkitButton.Size = new System.Drawing.Size(197, 59);
            this.uninstallModkitButton.TabIndex = 8;
            this.uninstallModkitButton.Text = "Un-mod Gnomoria.exe";
            this.uninstallModkitButton.UseVisualStyleBackColor = true;
            this.uninstallModkitButton.Click += new System.EventHandler(this.uninstallModkitButton_Click);
            // 
            // uninstallStandaloneButton
            // 
            this.uninstallStandaloneButton.AutoSize = true;
            this.uninstallStandaloneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uninstallStandaloneButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.uninstallStandaloneButton.Enabled = false;
            this.uninstallStandaloneButton.Location = new System.Drawing.Point(154, 68);
            this.uninstallStandaloneButton.Name = "uninstallStandaloneButton";
            this.uninstallStandaloneButton.Size = new System.Drawing.Size(197, 23);
            this.uninstallStandaloneButton.TabIndex = 9;
            this.uninstallStandaloneButton.Text = "Uninstall stand-alone";
            this.uninstallStandaloneButton.UseVisualStyleBackColor = true;
            this.uninstallStandaloneButton.Click += new System.EventHandler(this.uninstallStandaloneButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel8, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(360, 356);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.browseForGame, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.gamePathInput, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 102);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(354, 30);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel3.Controls.Add(this.standaloneVersion, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 191);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(360, 29);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // standaloneVersion
            // 
            this.standaloneVersion.Location = new System.Drawing.Point(121, 3);
            this.standaloneVersion.Name = "standaloneVersion";
            this.standaloneVersion.ReadOnly = true;
            this.standaloneVersion.Size = new System.Drawing.Size(236, 20);
            this.standaloneVersion.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 23);
            this.label3.TabIndex = 16;
            this.label3.Text = "Detected stand-alone:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.installModkitButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.uninstallStandaloneButton, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.installStandaloneButton, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.uninstallModkitButton, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 229);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(354, 101);
            this.tableLayoutPanel4.TabIndex = 14;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.versionLabel, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.linkLabel1, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 336);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(354, 33);
            this.tableLayoutPanel5.TabIndex = 15;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.versionLabel.Location = new System.Drawing.Point(3, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(70, 33);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.Text = "Gnoll Installer";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.linkLabel1.Location = new System.Drawing.Point(311, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(40, 33);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "GitHub";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 223);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(354, 1);
            this.tableLayoutPanel6.TabIndex = 17;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.AutoSize = true;
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel7.Controls.Add(this.selectedPatch, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.selectedPatchVersion, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 138);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(354, 18);
            this.tableLayoutPanel7.TabIndex = 18;
            // 
            // selectedPatch
            // 
            this.selectedPatch.AutoSize = true;
            this.selectedPatch.Dock = System.Windows.Forms.DockStyle.Left;
            this.selectedPatch.Location = new System.Drawing.Point(3, 0);
            this.selectedPatch.Name = "selectedPatch";
            this.selectedPatch.Size = new System.Drawing.Size(82, 18);
            this.selectedPatch.TabIndex = 0;
            this.selectedPatch.Text = "Installing Patch:";
            // 
            // selectedPatchVersion
            // 
            this.selectedPatchVersion.AutoSize = true;
            this.selectedPatchVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedPatchVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.selectedPatchVersion.Location = new System.Drawing.Point(119, 0);
            this.selectedPatchVersion.Name = "selectedPatchVersion";
            this.selectedPatchVersion.Size = new System.Drawing.Size(232, 18);
            this.selectedPatchVersion.TabIndex = 1;
            this.selectedPatchVersion.Text = "???";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.AutoSize = true;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel8.Controls.Add(this.gameVersionLabel, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 162);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(354, 26);
            this.tableLayoutPanel8.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(360, 356);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(376, 360);
            this.Name = "Form1";
            this.Text = "Gnoll Installer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox gamePathInput;
        private System.Windows.Forms.Button browseForGame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gameVersionLabel;
        private System.Windows.Forms.Button installModkitButton;
        private System.Windows.Forms.Button installStandaloneButton;
        private System.Windows.Forms.Button uninstallModkitButton;
        private System.Windows.Forms.Button uninstallStandaloneButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TextBox standaloneVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label selectedPatch;
        private System.Windows.Forms.Label selectedPatchVersion;
    }
}


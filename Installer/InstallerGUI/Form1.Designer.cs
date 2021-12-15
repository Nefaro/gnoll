
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.ForestGreen;
            this.pictureBox1.Image = global::InstallerGUI.Properties.Resources.banner;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game install path:";
            // 
            // gamePathInput
            // 
            this.gamePathInput.Location = new System.Drawing.Point(12, 118);
            this.gamePathInput.Name = "gamePathInput";
            this.gamePathInput.ReadOnly = true;
            this.gamePathInput.Size = new System.Drawing.Size(274, 20);
            this.gamePathInput.TabIndex = 2;
            // 
            // browseForGame
            // 
            this.browseForGame.Location = new System.Drawing.Point(296, 116);
            this.browseForGame.Name = "browseForGame";
            this.browseForGame.Size = new System.Drawing.Size(75, 23);
            this.browseForGame.TabIndex = 3;
            this.browseForGame.Text = "Browse...";
            this.browseForGame.UseVisualStyleBackColor = true;
            this.browseForGame.Click += new System.EventHandler(this.browseForGame_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Detected version:";
            // 
            // gameVersionLabel
            // 
            this.gameVersionLabel.Location = new System.Drawing.Point(12, 162);
            this.gameVersionLabel.Name = "gameVersionLabel";
            this.gameVersionLabel.ReadOnly = true;
            this.gameVersionLabel.Size = new System.Drawing.Size(274, 20);
            this.gameVersionLabel.TabIndex = 5;
            // 
            // installModkitButton
            // 
            this.installModkitButton.Enabled = false;
            this.installModkitButton.Location = new System.Drawing.Point(12, 200);
            this.installModkitButton.Name = "installModkitButton";
            this.installModkitButton.Size = new System.Drawing.Size(175, 46);
            this.installModkitButton.TabIndex = 6;
            this.installModkitButton.Text = "Mod Gnomoria.exe";
            this.installModkitButton.UseVisualStyleBackColor = true;
            this.installModkitButton.Click += new System.EventHandler(this.installModkitButton_Click);
            // 
            // installStandaloneButton
            // 
            this.installStandaloneButton.Enabled = false;
            this.installStandaloneButton.Location = new System.Drawing.Point(12, 252);
            this.installStandaloneButton.Name = "installStandaloneButton";
            this.installStandaloneButton.Size = new System.Drawing.Size(175, 23);
            this.installStandaloneButton.TabIndex = 7;
            this.installStandaloneButton.Text = "Install stand-alone";
            this.installStandaloneButton.UseVisualStyleBackColor = true;
            this.installStandaloneButton.Click += new System.EventHandler(this.installStandaloneButton_Click);
            // 
            // uninstallModkitButton
            // 
            this.uninstallModkitButton.Enabled = false;
            this.uninstallModkitButton.Location = new System.Drawing.Point(199, 200);
            this.uninstallModkitButton.Name = "uninstallModkitButton";
            this.uninstallModkitButton.Size = new System.Drawing.Size(172, 46);
            this.uninstallModkitButton.TabIndex = 8;
            this.uninstallModkitButton.Text = "Un-mod Gnomoria.exe";
            this.uninstallModkitButton.UseVisualStyleBackColor = true;
            this.uninstallModkitButton.Click += new System.EventHandler(this.uninstallModkitButton_Click);
            // 
            // uninstallStandaloneButton
            // 
            this.uninstallStandaloneButton.Enabled = false;
            this.uninstallStandaloneButton.Location = new System.Drawing.Point(199, 252);
            this.uninstallStandaloneButton.Name = "uninstallStandaloneButton";
            this.uninstallStandaloneButton.Size = new System.Drawing.Size(172, 23);
            this.uninstallStandaloneButton.TabIndex = 9;
            this.uninstallStandaloneButton.Text = "Uninstall stand-alone";
            this.uninstallStandaloneButton.UseVisualStyleBackColor = true;
            this.uninstallStandaloneButton.Click += new System.EventHandler(this.uninstallStandaloneButton_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(12, 293);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(70, 13);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.Text = "Gnoll Installer";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(332, 293);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(40, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "GitHub";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 315);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.uninstallStandaloneButton);
            this.Controls.Add(this.uninstallModkitButton);
            this.Controls.Add(this.installStandaloneButton);
            this.Controls.Add(this.installModkitButton);
            this.Controls.Add(this.gameVersionLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browseForGame);
            this.Controls.Add(this.gamePathInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Gnoll Installer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollModLoader.GUI
{
    // Gnoll Mods panel, shows loaded/enabled mods
    class ModsMenuPanel : AbstractTabbedWindowPanel
    {
        private readonly List<IGnollMod> _listOfMods;
        private int _height;

        public ModsMenuPanel(Manager manager, List<IGnollMod> listOfMods) : base(manager)
        {
            if (listOfMods != null)
                this._listOfMods = listOfMods;
            else
                this._listOfMods = new List<IGnollMod>();
        }

        public override void SetupPanel()
        {
            this.AttachBackButton(this.Manager);
            this.AddLabel(this.Manager, "Enabled mods");
            ListBox listBox = new ListBox(this.Manager);
            listBox.Init();
            listBox.HideSelection = false;
            foreach (var mod in this._listOfMods)
            {
                listBox.Prop_0.Add(mod.Name);
            }
            listBox.Top = this._height + listBox.Margins.Top;
            listBox.Height = 120;
            this._height += listBox.Height + listBox.Margins.Bottom;
            listBox.Width = this.Parent.Width - listBox.Margins.Horizontal;

            this.Add(listBox);
            this.AddLabel(this.Manager, "Mod description");
            Panel descriptionPanel = new LoweredPanel(this.Manager);
            descriptionPanel.Init();
            descriptionPanel.Height = 90;
            descriptionPanel.Top = this._height + descriptionPanel.Margins.Top;
            descriptionPanel.Width = this.Parent.Width - descriptionPanel.Margins.Horizontal;
            this.Add(descriptionPanel);

            Label modNameLabel = new Label(this.Manager);
            modNameLabel.Init();
            modNameLabel.Width = 400;
            modNameLabel.Text = "";

            Label separatorLabel = new Label(this.Manager);
            separatorLabel.Init();
            separatorLabel.Width = 400;
            separatorLabel.Text = "";
            separatorLabel.Top += 15;

            Label modDescLabel= new Label(this.Manager);
            modDescLabel.Init();
            modDescLabel.Width = 400;
            modDescLabel.Text = "";
            var size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(modDescLabel.Text);
            modDescLabel.Height = (int)size.Y;
            modDescLabel.Top = modNameLabel.Height;

            descriptionPanel.Add(modNameLabel);
            descriptionPanel.Add(separatorLabel);
            descriptionPanel.Add(modDescLabel);

            listBox.ItemIndexChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                IGnollMod mod = this._listOfMods[listBox.ItemIndex];
                modNameLabel.Text = mod.Name;
                string multiline = this.SpliceText(mod.Description, 50);

                modDescLabel.Text = multiline;
                size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(modDescLabel.Text);
                modDescLabel.Height = (int)size.Y;
                modDescLabel.Top = modNameLabel.Height + modDescLabel.Margins.Top;

                separatorLabel.Text = "===== ===== ===== ===== =====";
            };

            if (this._listOfMods.Count > 0)
            {
                listBox.ItemIndex = 0;
            }
        }

        private void AddLabel(Manager manager, string text)
        {
            Label label = new Label(manager);
            label.Init();
            label.Text = text;

            label.Margins = new Margins(3, 3, 3, 3);
            label.Width = 400;
            label.Top = this._height + label.Margins.Top;
            label.Left = label.Margins.Left;
            this._height += label.Height + label.Margins.Bottom;
            this.Add(label);
        }

        private string SpliceText(string text, int max)
        {
            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(Environment.NewLine, lines.GroupBy(w => (charCount += w.Length + 1) / (max + 2))
                        .Select(g => string.Join(" ", g.ToArray())));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Game.GUI;
using Game.GUI.Controls;
using Microsoft.Xna.Framework;

namespace GnollModLoader.GUI
{
    // Gnoll Mods panel, shows loaded/enabled mods
    class ModsMenuPanel : AbstractTabbedWindowPanel
    {
        private readonly List<IGnollMod> _listOfMods;
        private readonly ModManager _modManager;
        private int _height;

        private static readonly Color STATUS_COLOR_DISABLED = Color.DarkRed;
        private static readonly Color STATUS_COLOR_ENABLED = Color.LimeGreen;
        private static readonly Color STATUS_COLOR_RESTART_REQUIRED = Color.Orange;

        public ModsMenuPanel(Manager manager, ModManager modManager) : base(manager)
        {
            this._modManager = modManager;
            this._listOfMods = modManager.Mods;
        }

        public override void SetupPanel()
        {
            this.Init();
            ListBox listBox = new ListBox(this.Manager);
            listBox.Init();

            Button back = this.AttachBackButton(this.Manager);

            Button enableButton = this.BuildButton(this.Manager, "Enable", (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Logger.Log("Enabled clicked");
                IGnollMod mod = this._listOfMods[listBox.ItemIndex];
                this._modManager.EnableMod(mod);
                listBox.ItemIndex = listBox.ItemIndex;
            });
            enableButton.Left = back.Left + back.Width + enableButton.Margins.Left;

            Button disableButton = this.BuildButton(this.Manager, "Disable", (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                IGnollMod mod = this._listOfMods[listBox.ItemIndex];
                this._modManager.DisableMod(mod);
                listBox.ItemIndex = listBox.ItemIndex;
            });
            disableButton.Left = enableButton.Left + enableButton.Width + disableButton.Margins.Left;
            var size = disableButton.Skin.Layers["Control"].Text.Font.Resource.MeasureString(disableButton.Text);
            disableButton.Width = (int)size.X + disableButton.Margins.Horizontal + disableButton.Margins.Left;

            this.AddLabel(this.Manager, "List of available mods");

            listBox.HideSelection = false;
            // Remove the original implementation
            listBox.clipBox_0.Draw -= listBox.clipBox_0_Draw;
            // Add our own custom draw 
            this.listboxDrawOverride(listBox);

            foreach (var mod in this._listOfMods)
            {
                Label label = new Label(this.Manager);
                label.Text = mod.Name;
                if ( this._modManager.IsWaitingRestart(mod))
                {
                    label.TextColor = STATUS_COLOR_RESTART_REQUIRED;
                }
                else if ( this._modManager.IsModEnabled(mod) )
                {
                    label.TextColor = STATUS_COLOR_ENABLED;
                }
                else
                {
                    label.TextColor = STATUS_COLOR_DISABLED;
                }
                listBox.Prop_0.Add(label);
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

            Label modStatusLabel = new Label(this.Manager);
            modStatusLabel.Init();
            modStatusLabel.Text = "";

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
            size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(modDescLabel.Text);
            modDescLabel.Height = (int)size.Y;
            modDescLabel.Top = modNameLabel.Height;

            descriptionPanel.Add(modStatusLabel);
            descriptionPanel.Add(modNameLabel);
            descriptionPanel.Add(separatorLabel);
            descriptionPanel.Add(modDescLabel);

            listBox.ItemIndexChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                IGnollMod mod = this._listOfMods[listBox.ItemIndex];
                Label item = (Label)listBox.Prop_0[listBox.ItemIndex];
                if (this._modManager.IsWaitingRestart(mod))
                {
                    modStatusLabel.Text = "(Restart Required)";
                    modStatusLabel.TextColor = STATUS_COLOR_RESTART_REQUIRED;
                    item.TextColor = STATUS_COLOR_RESTART_REQUIRED;
                    enableButton.Suspended = true;
                    disableButton.Suspended = true;
                }
                else if ( this._modManager.IsModEnabled(mod) )
                {
                    modStatusLabel.Text = "(Enabled)";
                    modStatusLabel.TextColor = STATUS_COLOR_ENABLED;
                    item.TextColor = STATUS_COLOR_ENABLED;
                    enableButton.Suspended = true;
                    disableButton.Suspended = false;
                }
                else
                {
                    modStatusLabel.Text = "(Disabled)";
                    modStatusLabel.TextColor = STATUS_COLOR_DISABLED;
                    item.TextColor = STATUS_COLOR_DISABLED;
                    enableButton.Suspended = false;
                    disableButton.Suspended = true;
                }

                size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(modStatusLabel.Text);
                modStatusLabel.Width = (int)size.X + modStatusLabel.Margins.Right;

                modNameLabel.Text = mod.Name;
                modNameLabel.Left = modStatusLabel.Width;
                string multiline = this.SpliceText(mod.Description, 47);

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

        // Reimplemented Drawing to use labels
        private void listboxDrawOverride(ListBox listBox)
        {
            SkinText skinText = listBox.Skin.Layers["Control"].Text;
            SkinLayer layer = listBox.Skin.Layers["ListBox.Selection"];
            listBox.clipBox_0.Draw += (object sender, DrawEventArgs e) =>
            {
                if (listBox.list_0 != null && listBox.list_0.Count > 0)
                {
                    int num = (int)skinText.Font.Resource.MeasureString(listBox.list_0[0].ToString()).Y - 1;
                    int num2 = listBox.scrollBar_0.Value / 10;
                    int num3 = listBox.scrollBar_0.PageSize / 10;
                    int num4 = (int)((float)(this.scrollBar_0.Value % 10) / 10f * (float)num);
                    int count = listBox.list_0.Count;
                    int num5 = listBox.int_15;
                    for (int i = num2; i <= num2 + num3 + 1; i++)
                    {
                        if (i < count)
                        {
                            Label listItem = (Label)listBox.list_0[i];
                            e.Renderer.DrawString(skinText.Font.Resource, listItem.Text, new Rectangle(e.Rectangle.Left, e.Rectangle.Top - num4 + (i - num2) * num, e.Rectangle.Width, num), 
                                listItem.TextColor, skinText.Alignment, skinText.OffsetX, skinText.OffsetY, true);
                        }
                    }
                    if (num5 >= 0 && num5 < count && (listBox.Focused || !listBox.bool_23))
                    {
                        int num6 = -num4 + (num5 - num2) * num;
                        if (num6 > -num && num6 < (num3 + 1) * num)
                        {
                            Label listItem = (Label)listBox.list_0[num5];
                            e.Renderer.DrawLayer(listBox, layer, new Rectangle(e.Rectangle.Left, e.Rectangle.Top + num6, e.Rectangle.Width, num));
                            e.Renderer.DrawString(listBox, layer, listItem.Text, new Rectangle(e.Rectangle.Left, e.Rectangle.Top + num6, e.Rectangle.Width, num), false);
                        }
                    }
                }
            };
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

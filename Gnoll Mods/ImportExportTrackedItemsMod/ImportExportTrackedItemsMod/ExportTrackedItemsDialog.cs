using Game;
using Game.GUI;
using Game.GUI.Controls;
using System.IO;
using System.Linq;

namespace GnollMods.ImportExportTrackedItemsMod
{
    // Code based on Export*Dialog from retail game
    // TODO: split into generic dialog + specific importer/exporter (low priority)

    internal class ExportTrackedItemsDialog : TabbedWindowPanel
    {
        public ExportTrackedItemsDialog(Manager manager) : base(manager)
        {
        }

        public override void SetupPanel()
        {
            Label label = new Label(this.Manager);
            label.Init();
            label.Top = label.Margins.Top;
            label.Left = label.Margins.Left;
            label.Text = "Save Tracked items as:";
            label.Width = this.ClientWidth - label.Margins.Horizontal;
            this.Add(label);

            TextBox nameBox = new TextBox(this.Manager);
            nameBox.Init();
            nameBox.Top = label.Top + label.Height;
            nameBox.Left = nameBox.Margins.Left;
            nameBox.Width = this.ClientWidth - nameBox.Left - nameBox.Margins.Right;
            nameBox.Text = "tracked";
            this.Add(nameBox);

            Button button = new Button(this.Manager);
            button.Top = nameBox.Top + nameBox.Height + nameBox.Margins.Bottom + button.Margins.Top + 10;
            button.Left = button.Margins.Left;
            button.Text = "Save";
            button.Click += delegate (object s, Game.GUI.Controls.EventArgs e)
            {
                string text = ExportTrackedItemsDialog.CleanFileName(nameBox.Text);
                if (nameBox.Text != text)
                {
                    nameBox.Text = text;
                    return;
                }
                if (nameBox.Text == "")
                {
                    return;
                }
                ModMain.instance.SaveTrackedItems(nameBox.Text);
                GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            this.Add(button);

            Button button2 = new Button(this.Manager);
            button2.Top = button.Top;
            button2.Left = button.Left + button.Width + button.Margins.Right + button2.Margins.Left;
            button2.Text = "Back";
            button2.Click += delegate (object s, Game.GUI.Controls.EventArgs e)
            {
                GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            this.Add(button2);
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (string current, char c) => current.Replace(c.ToString(), ""));
        }
    }
}
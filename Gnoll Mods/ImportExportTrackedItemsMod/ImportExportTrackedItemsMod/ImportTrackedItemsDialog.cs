using System.IO;
using Game;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollMods.ImportExportTrackedItemsMod
{
    // Code based on Import*Dialog from retail game
    // TODO: split into generic dialog + specific importer/exporter (low priority)

    internal class ImportTrackedItemsDialog : TabbedWindowPanel
    {
        public ImportTrackedItemsDialog(Manager manager) : base(manager)
        {
        }

        public override void SetupPanel()
        {
            Label label = new Label(this.Manager);
            label.Init();
            label.Top = label.Margins.Top;
            label.Left = label.Margins.Left;
            label.Text = "Import Tracked Items:";
            label.Width = this.ClientWidth - label.Margins.Horizontal;
            this.Add(label);

            ListBox gameSaveList = new ListBox(this.Manager);
            gameSaveList.Init();
            gameSaveList.Top = label.Top + label.Height;
            gameSaveList.Left = gameSaveList.Margins.Left;
            gameSaveList.Width = 300;
            gameSaveList.Height = this.ClientHeight - gameSaveList.Top - gameSaveList.Margins.Bottom;
            gameSaveList.HideSelection = false;

            string[] files = Directory.GetFiles(GnomanEmpire.SaveFolderPath("TrackedItems\\"), "*.json");

            foreach (string text in files)
            {
                gameSaveList.list_0.Add(text.Replace(GnomanEmpire.SaveFolderPath("TrackedItems\\"), "").Replace(".json", ""));
            }

            this.Add(gameSaveList);

            Button button = new Button(this.Manager);
            button.Top = gameSaveList.Top;
            button.Left = gameSaveList.Left + gameSaveList.Width + gameSaveList.Margins.Right + button.Margins.Left;
            button.Width = this.ClientWidth - button.Left - button.Margins.Right;
            button.Text = "Import";
            button.Click += delegate (object s, Game.GUI.Controls.EventArgs e)
            {
                if (gameSaveList.ItemIndex >= 0 && gameSaveList.ItemIndex < gameSaveList.list_0.Count)
                {
                    ModMain.instance.LoadTrackedItems(gameSaveList.list_0[gameSaveList.ItemIndex].ToString());
                }
                GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            this.Add(button);

            Button button2 = new Button(this.Manager);
            button2.Top = button.Top + button.Height + button.Margins.Bottom + button2.Margins.Top;
            button2.Left = button.Left;
            button2.Width = this.ClientWidth - button2.Left - button2.Margins.Right;
            button2.Text = "Back";
            button2.Click += delegate (object s, Game.GUI.Controls.EventArgs e)
            {
                GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            this.Add(button2);
        }
    }
}

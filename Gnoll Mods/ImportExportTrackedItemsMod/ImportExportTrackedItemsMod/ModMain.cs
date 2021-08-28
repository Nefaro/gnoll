using System.Collections.Generic;

using GnollModLoader;
using Game.GUI.Controls;
using Game;

namespace Mod
{
    public class ModMain : IMod
    {
        public static ModMain instance;

        public ModMain()
        {
            instance = this;
        }

        public string Name { get { return "ImportExportTrackedItemsMod"; } }

        public string Description { get { return "Imports and Exports tracked items"; } }

        public void OnLoad(HookManager hookManager)
        {
            hookManager.ExportMenuListInit += HookManager_ExportMenuListInit;
        }

        private void HookManager_ExportMenuListInit(Game.GUI.ImportExportMenu importExportMenu, Game.GUI.Controls.Manager manager, HookManager.AddButton addButton)
        {
            Button button2 = addButton("Tracked Items");

            button2.Click += delegate (object s, Game.GUI.Controls.EventArgs e)
            {
                GnomanEmpire.Instance.GuiManager.MenuStack.PushWindow(new ImportExportTrackedItemsDialog(manager));
            };
        }

        internal void LoadTrackedItems(string name)
        {
            string path = GnomanEmpire.SaveFolderPath("TrackedItems\\") + name.Trim() + ".json";
            string json = System.IO.File.ReadAllText(path);

            TrackedItemsModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<TrackedItemsModel>(json);

            var groups = GnomanEmpire.Instance.Region.Fortress.StockManager.TrackedItemGroups;
            var hud = GnomanEmpire.Instance.GuiManager.InGameHUD_0.GClass0_0;

            for (int i = groups.Count - 1; i >= 0; i--)
            {
                groups.RemoveAt(i);
                hud.RemoveStockTrackLabel(i);
            }

            foreach (TrackedItemsModel.Group groupModel in model.groups)
            {
                var group = new TrackedItemGroup();
                group.Name.Value = groupModel.name;

                // Import items

                foreach (string item in groupModel.items)
                {
                    group.AddAllowedItem(item);
                }

                // Import allowed materials

                foreach (TrackedItemsModel.AllowedMaterialsForItem allowedMaterials in groupModel.allowedMaterials)
                {
                    HashSet<string> materials = new HashSet<string>(allowedMaterials.materials);

                    group.dictionary_0.Add(allowedMaterials.item, materials);
                }

                groups.Add(group);

                hud.AddStockTrackLabel(group, false);
            }
        }

        internal void SaveTrackedItems(string name)
        {
            string path = GnomanEmpire.SaveFolderPath("TrackedItems\\") + name.Trim() + ".json";

            var groups = GnomanEmpire.Instance.Region.Fortress.StockManager.TrackedItemGroups;

            TrackedItemsModel model = new TrackedItemsModel();
            model.gameVersion = Game.Common.GameSaveHeader.CurrentVersion;

            foreach (TrackedItemGroup group in groups)
            {
                TrackedItemsModel.Group groupModel = new TrackedItemsModel.Group();
                groupModel.name = group.Name;

                // Export items
                foreach (string item in group.itemGroup_0.AllowedItems)
                {
                    groupModel.items.Add(item);
                }

                // Export allowed materials
                foreach (KeyValuePair<string, HashSet<string>> keyValuePair in group.dictionary_0)
                {
                    TrackedItemsModel.AllowedMaterialsForItem materialModel = new TrackedItemsModel.AllowedMaterialsForItem();

                    materialModel.item = keyValuePair.Key;

                    foreach (string material in keyValuePair.Value)
                    {
                        materialModel.materials.Add(material);
                    }

                    groupModel.allowedMaterials.Add(materialModel);
                }

                model.groups.Add(groupModel);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }


    }
}

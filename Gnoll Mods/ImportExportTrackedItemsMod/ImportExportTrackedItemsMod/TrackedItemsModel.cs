using System.Collections.Generic;

namespace GnollMods.ImportExportTrackedItemsMod
{
    internal class TrackedItemsModel
    {
        public uint gameVersion;
        public List<Group> groups = new List<Group>();

        public TrackedItemsModel()
        {
        }

        public class Group
        {
            public string name;
            public List<string> items = new List<string>();
            public List<AllowedMaterialsForItem> allowedMaterials = new List<AllowedMaterialsForItem>();

            public Group()
            {
            }
        }

        public class AllowedMaterialsForItem
        {
            public string item;
            public List<string> materials = new List<string>();

            public AllowedMaterialsForItem()
            {
            }
        }
    }
}
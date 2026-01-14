using System;
using System.Collections.Generic;
using System.Linq;
using Game.GUI;
using Game.GUI.Controls;
using GnollModLoader;

namespace GnollMods.BuildListCache
{
    class ModMain : IGnollMod
    {
        private BuildConstructionUI buildWindow;
        private Panel cachePanel;
        private readonly Dictionary<String, FixedSizedQueue<CacheItem>> cache = new Dictionary<string, FixedSizedQueue<CacheItem>>();

        public string Name { get { return "BuildListCache"; } }
        public string Description { get { return "This mod caches and displays the last used items in the construction window"; } }
        public string BuiltWithLoaderVersion { get { return "G1.13"; } }
        public int RequireMinPatchVersion { get { return 13; } }

        private void HookManager_InGameShowWindow(Window window)
        {
            this.buildWindow = window as Game.GUI.BuildConstructionUI;
            if (buildWindow != null)
            {
                cachePanel = new Game.GUI.LoweredPanel(buildWindow.Manager);
                cachePanel.Init();
                cachePanel.Left = buildWindow.constructionPanel_0.Left + 58;
                cachePanel.Top = 64 + 115 + 5;
                cachePanel.Width = buildWindow.constructionPanel_0.Width - 75;
                cachePanel.Height = 98;
                cachePanel.AutoScroll = true;
                cachePanel.HorizontalScrollBarEnabled = false;
                cachePanel.VerticalScrollBarShow = false;
                cachePanel.Passive = true;
                cachePanel.CanFocus = false;
                cachePanel.ScrollTo(0, 0);
                window.Add(cachePanel);

                UpdateCacheButtons(buildWindow.list_0[0]);

                foreach (var control in buildWindow.constructionPanel_0.Parent.controlsList_1)
                {
                    var button = control as Button;
                    if (button != null && button.Text == "Build")
                    {
                        button.Tag = buildWindow.list_0[0];
                        button.Click -= buildWindow.method_45;
                        button.Click += BuildClick;
                        button.Click += buildWindow.method_45;
                        break;
                    }
                }

            }

        }

        private void BuildClick(object sender, Game.GUI.Controls.EventArgs e)
        {
            var button = sender as Button;
            if (button != null && buildWindow != null && buildWindow.constructionPanel_0 != null)
            {
                string tag = button.Tag.ToString();
                var cache = GetCache(tag);
                cache.Enqueue(new CacheItem(buildWindow.constructionPanel_0.Construction, buildWindow.constructionPanel_0.ComponentMaterials()));
                UpdateCacheButtons(tag);
            }
        }

        private void UpdateCacheButtons(string tag)
        {
            var cache = GetCache(tag);
            int idx = cache.Count;
            foreach (var item in cache)
            {
                AddButton(item, buildWindow, cachePanel, --idx);
            }
        }

        private void AddButton(CacheItem item, BuildConstructionUI buildWindow, Panel cachePanel, int idx)
        {
            Button button_0 = new Button(buildWindow.Manager);
            button_0.Init();
            button_0.Margins = new Margins(0, 1, 0, 1);
            button_0.Anchor = (Anchors.Left | Anchors.Top | Anchors.Right);
            button_0.Text = item.ToString();
            button_0.Width = cachePanel.ClientWidth - button_0.Margins.Horizontal;
            button_0.Height = 20;
            button_0.Left = button_0.Margins.Left;
            button_0.Top = idx * (button_0.Height + button_0.Margins.Vertical);
            button_0.Click += UseCachedItem;
            button_0.Tag = item;
            cachePanel.Add(button_0);
        }

        private void UseCachedItem(object sender, Game.GUI.Controls.EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                CacheItem item = (CacheItem)button.Tag;
                System.Console.WriteLine(" -- Selected item {0}", item);
                Game.GnomanEmpire.Instance.Region.TileSelectionManager.ComponentsSelected(item.ConstructionID, item.MaterialIDs);
                buildWindow.Close();
            }
        }

        private FixedSizedQueue<CacheItem> GetCache(string tag)
        {
            FixedSizedQueue<CacheItem> queue;
            if (!cache.TryGetValue(tag, out queue))
            {
                queue = new FixedSizedQueue<CacheItem>(5);
                cache.Add(tag, queue);
            }
            return queue;
        }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameShowWindow += HookManager_InGameShowWindow;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameShowWindow -= HookManager_InGameShowWindow;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }
    }

    class CacheItem
    {
        private readonly string constructID;
        private readonly List<string> materialIDs;
        public string ConstructionID { get { return constructID; } }
        public List<string> MaterialIDs { get { return materialIDs; } }

        public CacheItem(string constructID, List<String> materialIDs)
        {
            this.constructID = constructID;
            this.materialIDs = materialIDs;
        }

        public override string ToString()
        {
            var constructionName = Game.Construction.GroupName(constructID);

            string materialName = "any";
            if (materialIDs.Count > 0 && Game.GnomanEmpire.Instance.GameDefs.MaterialDef(materialIDs[0]) != null )
            {
                materialName = Game.GnomanEmpire.Instance.GameDefs.MaterialDef(materialIDs[0]).Name;
            }
            return materialName + " " + constructionName;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(CacheItem) &&
                    constructID == ((CacheItem)obj).constructID &&
                    Enumerable.SequenceEqual(materialIDs, ((CacheItem)obj).materialIDs);
        }

        public override int GetHashCode()
        {
            int hashCode = -1843774847;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(constructID);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(materialIDs);
            return hashCode;
        }
    }

    // StackOverflow says this is a bad class,
    // but it's good enough for our case
    class FixedSizedQueue<T> : Queue<T>
    {
        private readonly object syncObject = new object();
        public int Size { get; private set; }
        public FixedSizedQueue(int size)
        {
            Size = size;
        }
        public new void Enqueue(T obj)
        {
            if (base.Contains(obj))
            {
                return;
            }

            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    base.Dequeue();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using GameLibrary;
using GnollModLoader;

namespace GnollMods.MarketStallSellFilters
{
    internal class ModMain: IGnollMod
    {
        public string Name { get { return "MarketStallSellFilters"; } }
        public string Description { get { return "Market stall gets value and name filters for player items"; } }
        public string BuiltWithLoaderVersion { get { return "G1.7"; } }
        public int RequireMinPatchVersion { get { return 7; } }

        private List<AvailableGood> goodsCache = new List<AvailableGood>();

        private Label labelFilterValue;
        private TextBox filterValue;
        private Label labelFilterName;
        private TextBox filterName;


        public void OnLoad(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
            hookManager.InGameShowWindow += HookManager_InGameShowWindow;
            hookManager.BeforeEntitySpawn += HookManager_BeforeEntitySpawn;
        }

        private void HookManager_BeforeEntitySpawn(Game.GameEntity entity)
        {
            // Give the merchants the faction names
            Merchant merc = entity as Merchant;
            if (merc != null)
            {
                Faction faction = GnomanEmpire.Instance.World.AIDirector.Faction(merc.FactionID);
                merc.characterHistory_0.SetName(faction.Name);
            }
        }

        private void HookManager_InGameShowWindow(Window window)
        {
            MarketStallUI marketStallUI = window as MarketStallUI;
            if (marketStallUI != null)
            {
                this.goodsCache = new List<AvailableGood>();
                foreach (TabbedWindowPanel panel in ((TabbedWindow)window).list_0)
                {
                    MarketStallTradeUI stallTraderUI = panel as MarketStallTradeUI;
                    // check, that we are opening the trader ui and that we have trade items view (which means, trader is at post)
                    if (stallTraderUI != null && stallTraderUI.tradePanelUI_2 != null)
                    {
                        foreach (var control in stallTraderUI.clipBox_0.ControlsList)
                        {
                            if (control is Label && control.Text.EndsWith("'s Stock"))
                            {
                                control.Text = "Merchant's Stock";
                                break;
                            }
                        }
                        System.Console.WriteLine($" Trade UI Width {stallTraderUI.tradePanelUI_2.Width}");

                        labelFilterValue = new Label(window.Manager);
                        labelFilterValue.Init();
                        labelFilterValue.Top = stallTraderUI.tradePanelUI_2.Top + stallTraderUI.tradePanelUI_2.Height + stallTraderUI.tradePanelUI_2.Margins.Bottom + labelFilterValue.Margins.Top; ;
                        labelFilterValue.Left = stallTraderUI.tradePanelUI_2.Left;
                        labelFilterValue.Width = (int)(stallTraderUI.tradePanelUI_2.Width/3);
                        labelFilterValue.Text = string.Format("Hide cheaper than:");
                        stallTraderUI.Add(labelFilterValue);

                        filterValue = new TextBox(window.Manager);
                        filterValue.Init();
                        filterValue.Top = labelFilterValue.Top;
                        filterValue.Left = labelFilterValue.Left + labelFilterValue.Width + filterValue.Margins.Horizontal;
                        filterValue.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 10);
                        stallTraderUI.Add(filterValue);

                        labelFilterName = new Label(window.Manager);
                        labelFilterName.Init();
                        labelFilterName.Top = labelFilterValue.Top;
                        labelFilterName.Left = filterValue.Left + filterValue.Width + labelFilterName.Margins.Horizontal;
                        labelFilterName.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 4);
                        labelFilterName.Text = string.Format("Filter by name:");
                        stallTraderUI.Add(labelFilterName);

                        filterName = new TextBox(window.Manager);
                        filterName.Init();
                        filterName.Top = labelFilterValue.Top;
                        filterName.Left = labelFilterName.Left + labelFilterName.Width + filterName.Margins.Horizontal;
                        filterName.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 5) - 10;
                        filterName.Text = "";
                        stallTraderUI.Add(filterName);

                        MarketStall marketStall = stallTraderUI.mWorkshop as MarketStall;
                        if (marketStall != null)
                        {
                            if (marketStall.HasMerchant)
                            {
                                Faction faction = GnomanEmpire.Instance.World.AIDirector.Faction(marketStall.Merchant.FactionID);
                            }
                        }
                        var merchant = marketStall.Merchant;
                        this.goodsCache = new List<AvailableGood>(stallTraderUI.list_0);

                        filterValue.TextChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
                        {
                            var textBox = sender as TextBox;
                            int num;
                            if (textBox != null && int.TryParse(textBox.Text, out num))
                            {
                                stallTraderUI.tradePanelUI_2.RemoveAllGoods();
                                if (num >= 0)
                                {
                                    var filteredItems = this.FilterGoods(merchant, num, filterName.Text);
                                    stallTraderUI.list_0 = filteredItems;
                                    stallTraderUI.tradePanelUI_2.BuildItems(filteredItems, merchant.TradeGoods, false);
                                    return;
                                }
                            }

                            if (textBox.Text != "")
                            {
                                textBox.Text = "1";
                            }
                        };
                        filterValue.Text = "1";

                        filterName.TextChanged += (object sender, Game.GUI.Controls.EventArgs e) =>
                        {
                            var textBox = sender as TextBox;
                            int num = 1;
                            if (filterValue.Text != null && int.TryParse(filterValue.Text, out num))
                            {
                                if (num < 0)
                                {
                                    num = 1;
                                }
                            }
                            stallTraderUI.tradePanelUI_2.RemoveAllGoods();
                            var filteredItems = this.FilterGoods(merchant, num, textBox.Text);
                            stallTraderUI.list_0 = filteredItems;
                            stallTraderUI.tradePanelUI_2.BuildItems(filteredItems, merchant.TradeGoods, false);
                        };

                        marketStallUI.Resize += MarketStallUI_Resize;

                        break;
                    }
                }
            }
        }

        private void MarketStallUI_Resize(object sender, ResizeEventArgs e)
        {
            MarketStallUI marketStallUI = sender as MarketStallUI;
            foreach (TabbedWindowPanel panel in ((TabbedWindow)marketStallUI).list_0)
            {
                MarketStallTradeUI stallTraderUI = panel as MarketStallTradeUI;
                // check, that we are opening the trader ui and that we have trade items view (which means, trader is at post)
                if (stallTraderUI != null && stallTraderUI.tradePanelUI_2 != null)
                {
                    labelFilterValue.Top = stallTraderUI.tradePanelUI_2.Top + stallTraderUI.tradePanelUI_2.Height + stallTraderUI.tradePanelUI_2.Margins.Bottom + labelFilterValue.Margins.Top; ;
                    labelFilterValue.Left = stallTraderUI.tradePanelUI_2.Left;
                    labelFilterValue.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 3);

                    filterValue.Top = labelFilterValue.Top;
                    filterValue.Left = labelFilterValue.Left + labelFilterValue.Width + filterValue.Margins.Horizontal;
                    filterValue.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 10);

                    labelFilterName.Top = labelFilterValue.Top;
                    labelFilterName.Left = filterValue.Left + filterValue.Width + labelFilterName.Margins.Horizontal;
                    labelFilterName.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 4);

                    filterName.Top = labelFilterValue.Top;
                    filterName.Left = labelFilterName.Left + labelFilterName.Width + filterName.Margins.Horizontal;
                    filterName.Width = (int)(stallTraderUI.tradePanelUI_2.Width / 5) - 10;
                }
            }
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            // Give the merchants the faction names
            foreach (var faction in GnomanEmpire.Instance.World.AIDirector.Factions.Values)
            {
                if (faction.FactionDef.Type == FactionType.FriendlyCiv)
                {
                    foreach (Character ch in faction.dictionary_1.Values)
                    {
                        if (ch is Merchant)
                        {
                            ch.characterHistory_0.SetName(faction.Name);
                        }
                    }
                }
            }
        }

        private List<AvailableGood> FilterGoods(Merchant merchant, int valueFilter, String nameFilter)
        {
            var filteredItems = new List<AvailableGood>();
            foreach (var good in this.goodsCache)
            {
                if (nameFilter != "" && !good.Name().Contains(nameFilter))
                    continue;

                if ((valueFilter == 0 || good.Value() > 0) && good.Value() * merchant.TradeGoods.BuyRate(good.ItemID) >= valueFilter)
                {
                    filteredItems.Add(good);
                }
            }
            return filteredItems;
        }
    }
}

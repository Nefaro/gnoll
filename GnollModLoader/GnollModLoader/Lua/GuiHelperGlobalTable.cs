using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GnollModLoader.Lua
{
    internal class GuiHelperGlobalTable
    {
        public readonly static string GLOBAL_TABLE_LUA_NAME = "_GUI";

        public TabbedWindowPanel CreateTabbedWindowPanel()
        {
            var manager = GnomanEmpire.Instance.GuiManager.Manager;
            var panel = new TabbedWindowPanel(manager);
            panel.Init();
            return panel;
        }

        public Label CreateLabel(string name)
        {
            var manager = GnomanEmpire.Instance.GuiManager.Manager;
            var label = new Label(manager);
            label.Init();
            label.Text = name;
            label.Name = this.generateUniqueName(label); // For uniqueness

            var textSize = this.textSize(manager, name);
            label.Width = (int)textSize.X;
            return label;
        }

        public Button CreateButton(string name)
        {
            var manager = GnomanEmpire.Instance.GuiManager.Manager;
            var button = new Button(manager);
            button.Init();
            button.Text = name;
            button.Name = this.generateUniqueName(button); // For uniqueness

            var textSize = this.textSize(manager, name);

            button.Width = (int)textSize.X + 2 * button.Margins.Horizontal;
            return button;
        }

        public ComboBox CreateSelect(List<object> data)
        {
            var manager = GnomanEmpire.Instance.GuiManager.Manager;
            ComboBox comboBox = new ComboBox(manager);
            comboBox.Init();
            comboBox.Name = this.generateUniqueName(comboBox); // For uniqueness

            foreach (object entity in data)
            {
                this.comboBoxAddItem(ref comboBox, entity); 
            }
            comboBox.ItemIndex = 0;

            comboBox.MinimumWidth = 400;
            comboBox.Width = comboBox.ClientWidth - comboBox.Left - comboBox.Margins.Right;

            return comboBox;
        }

        // We want to have a typed version and a general version of the item add
        private void comboBoxAddItem(ref ComboBox comboBox, object item )
        {
            if (item is Squad)
                comboBox.Items.Add((item as Squad).Name);
            else if (item is Character)
                comboBox.Items.Add((item as Character).Name());
            else if (item is Faction)
                comboBox.Items.Add($"{(item as Faction).Name} - {(item as Faction).SubType()}");
            else
                comboBox.Items.Add(item.ToString());
        }

        private string generateUniqueName(Control control)
        {
            return String.Format("{0:X}", control.GetHashCode());
        }

        private Vector2 textSize(Manager manager, string text)
        {
            return manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(text);
        }

        private string spliceText(Manager manager, string text, int max)
        {
            SpriteFont font = manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource;
            string[] textArray = text.Split('\n');
            for (int i = 0; i < textArray.Count(); i++)
            {
                if (font.MeasureString(textArray[i]).X > max)
                {
                    textArray[i] = String.Join(Environment.NewLine,
                        TextBox.WrapText(textArray[i], (double)max, font));
                }
            }
            return String.Join(Environment.NewLine, textArray);
        }
    }
}

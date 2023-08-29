using System;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollModLoader.GUI
{
    abstract class AbstractTabbedWindowPanel : TabbedWindowPanel
    {
        public AbstractTabbedWindowPanel(Manager manager) : base(manager)
        {
            
        }
        protected Button AttachBackButton(Manager manager)
        {
            return this.BuildButton(manager, "Back", (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            });
        }

        protected Button BuildButton(Manager manager, String text, Game.GUI.Controls.EventHandler clickEventHandler)
        {
            Button button = new Button(this.Manager);
            button.Init();
            button.Margins = new Margins(4, 4, 4, 4);
            button.Text = text;
            button.Left = button.Margins.Left;
            button.Anchor = Anchors.Left | Anchors.Bottom;
            button.Top = this.Height - this.ClientMargins.Vertical - button.Height - button.Margins.Bottom;
            button.Click += clickEventHandler;
            this.Add(button);
            return button;
        }
    }
}

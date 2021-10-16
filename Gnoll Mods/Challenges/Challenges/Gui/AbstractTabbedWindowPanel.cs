using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GUI;
using Game.GUI.Controls;

namespace GnollMods.Challenges.Gui
{
    abstract class AbstractTabbedWindowPanel : TabbedWindowPanel
    {
        public AbstractTabbedWindowPanel(Manager manager) : base(manager)
        {
            
        }
        protected void AttachBackButton(Manager manager)
        {
            Button back = new Button(this.Manager);
            back.Init();
            back.Margins = new Margins(4, 4, 4, 4);
            back.Text = "Back";
            back.Left = back.Margins.Left;
            back.Anchor = Anchors.Left | Anchors.Bottom;
            back.Top = this.Height - this.ClientMargins.Vertical - back.Height - back.Margins.Bottom;
            back.Click += (object sender, Game.GUI.Controls.EventArgs e) =>
            {
                Game.GnomanEmpire.Instance.GuiManager.MenuStack.PopWindow();
            };
            this.Add(back);
        }
    }
}

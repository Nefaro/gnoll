using System;
using Game.GUI.Controls;

namespace GnollModLoader.GUI
{
    class AboutMenuPanel : AbstractTabbedWindowPanel
    {
        private int _height;

        public AboutMenuPanel(Manager manager) : base(manager)
        {
            this.AttachBackButton(manager);

            this._height = 0;
            this.AddLabel(manager, "DLL mods loaded by \"" + GnollMain.NAME + "\"");
            this.AddLabel(manager, "Running version \"" + GnollMain.VERSION_STRING + "\"");
            this.AddLabel(manager, "More info and code at: ");
            this.AddLabel(manager, " -> " + GnollMain.APP_URL);
            this.AddLabel(manager, "Based on work done at: ");
            this.AddLabel(manager, " -> " + GnollMain.ORIGINAL_URL);
        }

        public void AddLabel(Manager manager, string text)
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
    }
}

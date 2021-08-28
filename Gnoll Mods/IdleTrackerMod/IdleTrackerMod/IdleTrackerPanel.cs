using System;
using Game.GUI.Controls;
using Microsoft.Xna.Framework;

namespace Mod
{
    internal class IdleTrackerPanel : Panel
    {
        public IdleTrackerPanel(Manager manager) : base(manager)
        {
            this.Init();
            this.Anchor = Anchors.Bottom | Anchors.Right;
            this.Color = new Color(0.0f, 0.0f, 0.0f, 0.8f);

            this.Height = 100;
            this.Width = 100;
        }

        public Button AddButton(string text)
        {
            var button = new Game.GUI.Controls.Button(this.manager_0);
            button.Init();
            button.Width = 200;
            button.Left = button.Margins.Left;
            button.Top = this.Height + button.Margins.Top;
            button.Text = text;
            this.Add(button, true);

            this.Width = Math.Max(this.Width, button.Margins.Left + button.Width + button.Margins.Right);
            this.Height += button.Margins.Top + button.Height + button.Margins.Bottom;

            return button;
        }

        public void AddLabel(string text)
        {
            var label = new Game.GUI.Controls.Label(this.manager_0);
            label.Init();
            label.Width = 200;
            label.Left = label.Margins.Left;
            label.Top = this.Height + label.Margins.Top;
            label.Text = text;
            label.Alignment = Alignment.TopRight;
            this.Add(label, true);

            this.Height += label.Margins.Top + label.Height + label.Margins.Bottom;
        }

        public void ClearContents()
        {
            this.ClientArea.controlsList_1.Clear();
            this.Height = 0;
        }
    }
}

using Game.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader.GUI
{
    public class MessageDialog : Dialog
    {
        public MessageDialog(Manager manager, string title, string text) : base(manager)
        {
            var size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(text);

            ClientWidth = (int)size.X + 16 + 16;
            ClientHeight = (int)size.Y + 120 - 48;
            TopPanel.Visible = false;

            IconVisible = true;
            Resizable = false;
            Text = title;
            Center();

            var label = new Label(this.Manager);
            label.Init();
            label.Left = 16;
            label.Top = 16;
            label.Width = this.ClientWidth - label.Left;
            label.Height = (int)size.Y;
            label.Alignment = Alignment.TopLeft;
            label.Text = text;

            var btnOk = new Button(this.Manager);
            btnOk.Init();
            btnOk.Left = base.BottomPanel.ClientWidth / 2 - btnOk.Width - 4;
            btnOk.Top = 8;
            btnOk.Text = "OK";
            btnOk.ModalResult = ModalResult.Ok;

            Add(label);
            BottomPanel.Add(btnOk);
            DefaultControl = btnOk;
        }
    }
}

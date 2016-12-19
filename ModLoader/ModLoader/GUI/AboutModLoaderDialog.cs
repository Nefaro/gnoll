using Game.GUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModLoader.GUI
{
    public class AboutModLoaderDialog : Dialog
    {
        public AboutModLoaderDialog(Manager manager) : base(manager)
        {
            string text = String.Format("{0}\n{1}\n\n{2}", ModLoaderMain.name, ModLoaderMain.versionString, ModLoaderMain.url);
            var size = Manager.Skin.Controls["Label"].Layers[0].Text.Font.Resource.MeasureString(text);

            ClientWidth = (int)size.X + 48 + 16 + 16 + 16;
            ClientHeight = (int)size.Y + 120 - 48;
            TopPanel.Visible = false;

            IconVisible = true;
            Resizable = false;
            Text = ModLoaderMain.name;
            Center();

            var label = new Label(this.Manager);
            label.Init();
            label.Left = 80;
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

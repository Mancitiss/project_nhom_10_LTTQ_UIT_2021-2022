using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Friend
{
    public partial class FormApplication : Form
    {
        public FormApplication()
        {
            InitializeComponent();
        }
        public void AddMessage(string message, bool stacktoleft)
        {
            var chatItem = new CustomControls.ChatItem(message, stacktoleft);
            chatItem.Name = "chatItem" + panelChat.Controls.Count;
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panelChat.BackColor;
            panelChat.Controls.Add(chatItem);
            chatItem.BringToFront();

            chatItem.ResizeBubbles((int)(panelChat.Width * 0.6));

            panelChat.ScrollControlIntoView(chatItem);
        }

        private void testForm_Resize(object sender, EventArgs e)
        {
            foreach (var control in panelChat.Controls)
            {
                if (control is CustomControls.ChatItem)
                {
                    (control as CustomControls.ChatItem).ResizeBubbles((int)(panelChat.Width * 0.6));
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts))
            {
                AddMessage(textboxWriting.Texts, false);
                textboxWriting.Texts = "";
            }
        }

        private void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textboxWriting.Texts))
                {
                    AddMessage(textboxWriting.Texts, false); 
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                }
            }
        }
    }
}

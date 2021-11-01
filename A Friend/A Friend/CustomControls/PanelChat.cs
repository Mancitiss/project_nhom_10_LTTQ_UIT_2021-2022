using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Friend.CustomControls
{
    public partial class PanelChat : UserControl
    {
        public PanelChat()
        {
            InitializeComponent();
        }

        public void AddMessage(string message, bool stacktoleft)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            panel_Chat.Controls.Add(chatItem);
            chatItem.BringToFront();
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
            panel_Chat.ScrollControlIntoView(chatItem);
        }

        private void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(textboxWriting.Texts))
                {
                    AddMessage(textboxWriting.Texts.Trim(), false);
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts))
            {
                AddMessage(textboxWriting.Texts, false);
                textboxWriting.Texts = "";
                AFriendClient.Send_to_id(AFriendClient.client, AFriendClient.user.id, AFriendClient.user.id, textboxWriting.Texts);
            }
        }
    }
}

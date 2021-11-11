using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace A_Friend.CustomControls
{
    public partial class PanelChat : UserControl
    {
        Account account;
        string id;
        byte state;
        Color stateColor = Color.Gainsboro;

        public PanelChat()
        {
            InitializeComponent();
            panel_Chat.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
        }

        private void panel_Chat_MouseWheel(object sender, EventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value==0)
            {
                LoadMessage();
            }    
        }

        public PanelChat(Account account)
        {
            InitializeComponent();
            this.account = account;
            this.DoubleBuffered = true;
            this.Name = "panelChat_" + account.id;
            labelFriendName.Text = account.name;
            textboxWriting.PlaceholderText = "to " + account.name;
            this.id = account.id;
            State = account.state;
        }

        public string ID
        {
            get { return this.id; }
        }

        public byte State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;

                    if (state == 0)
                    {
                        stateColor = Color.Gainsboro;
                        panelTopRight.Invalidate();
                    }
                    else if (state == 1)
                    {
                        stateColor = Color.SpringGreen;
                        panelTopRight.Invalidate();
                    }
                    else
                    {
                        stateColor = Color.Red;
                        panelTopRight.Invalidate();
                    }
                    this.Invalidate();
                }
            }
        }
        
        public void AddMessage(string message, bool stacktoleft)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            panel_Chat.Controls.Add(chatItem);
            chatItem.BringToFront();
            chatItem.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
            panel_Chat.ScrollControlIntoView(chatItem);
        }

        private void AddMessageToTop(string message, bool stacktoleft)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            chatItem.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            panel_Chat.Controls.Add(chatItem);
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
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
                    blockSending();
                    AFriendClient.Send_to_id(AFriendClient.client, AFriendClient.user.id, AFriendClient.user.id, textboxWriting.Texts);
                }
            }
        }
         
        private void blockSending()
        {
            textboxWriting.Enabled = false;
            buttonSend.Enabled = false;
            timerChat.Start();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts))
            {
                AddMessage(textboxWriting.Texts, false);
                textboxWriting.Texts = "";
                textboxWriting.RemovePlaceHolder();
                blockSending();
                AFriendClient.Send_to_id(AFriendClient.client, AFriendClient.user.id, AFriendClient.user.id, textboxWriting.Texts);
            }
        }

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(stateColor, 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(pen, friendPicture.Left - 1, friendPicture.Top - 1, friendPicture.Width + 2, friendPicture.Width + 2);
            }
        }

        private void panel_Chat_Click(object sender, EventArgs e)
        {
            panel_Chat.Focus();
        }

        public void LoadMessage()
        {
            panel_Chat.SuspendLayout();
            AddMessageToTop("Chào bạn", true);
            AddMessageToTop("Chào", false);
            AddMessageToTop("Chào Tạm biệt", true);
            AddMessageToTop("Tạm biệt", false);
            panel_Chat.ResumeLayout();
        }

        private void PanelChat_Load(object sender, EventArgs e)
        {
            //LoadMessage();
            textboxWriting.Focus();
            this.ActiveControl = textboxWriting;
        }

        public string GetLastMessage()
        {
            if (panel_Chat.Controls.Count == 0)
                return "";
            ChatItem2 message = panel_Chat.Controls[panel_Chat.Controls.Count - 1] as ChatItem2;
            return message.Texts; 
        }
        public string GetFirstMessage()
        {
            if (panel_Chat.Controls.Count == 0)
                return "";
            ChatItem2 message = panel_Chat.Controls[0] as ChatItem2;
            return message.Texts; 
        }

        public bool IsLastMessageFromYou()
        {
            if (panel_Chat.Controls.Count == 0)
                return true;
            ChatItem2 message = panel_Chat.Controls[panel_Chat.Controls.Count - 1] as ChatItem2;
            if (message.StackToLeft)
                return false;
            return true;
        }

        private void panel_Chat_ControlAdded(object sender, ControlEventArgs e)
        {
            this.OnControlAdded(e);
        }

        private void panel_Chat_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.OnControlRemoved(e);
        }

        private void timerChat_Tick(object sender, EventArgs e)
        {
            textboxWriting.Enabled = true;
            buttonSend.Enabled = true;
            textboxWriting.Focus();
            timerChat.Stop();
        }

        private void panel_Chat_Scroll(object sender, ScrollEventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value == 0)
            {
                LoadMessage();
            }
        }
    }
}

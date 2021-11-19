﻿using System;
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
        bool locking = false;
        List<CustomControls.ChatItem2> chatItems = new List<ChatItem2>();

        public delegate void AddMessageItem(string str, bool left);
        public AddMessageItem AddMessageDelegate;
        /*
        public delegate void ButtonSend_Click(object sender, EventArgs e);
        public ButtonSend_Click ButtonSend_Click_Delegate;
        */

        public PanelChat()
        {
            InitializeComponent();
            AddMessageDelegate = new AddMessageItem(AddMessage);
            //ButtonSend_Click_Delegate = new ButtonSend_Click(buttonSend_Click);
            panel_Chat.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            this.CreateControl();
            textboxWriting.dynamicMode = true;
            textboxWriting.SetMaximumTextLenght(2021);
        }

        private void panel_Chat_MouseWheel(object sender, EventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value == 0)
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
            AddMessageDelegate = new AddMessageItem(AddMessage);
            //ButtonSend_Click_Delegate = new ButtonSend_Click(buttonSend_Click);
            this.CreateControl();
            Console.WriteLine("Handler created");
            Console.WriteLine(this.id);
            textboxWriting.dynamicMode = true;
            textboxWriting.SetMaximumTextLenght(2021);
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

        public void RemoveChatItem(CustomControls.ChatItem2 chatItem)
        {
            chatItems.Remove(chatItem);
            panel_Chat.Controls.Remove(chatItem);
        }

        public void AddMessage(string message, bool stacktoleft)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            chatItems.Add(chatItem);
            panel_Chat.Controls.Add(chatItem);
            chatItem.BringToFront();
            chatItem.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
            panel_Chat.ScrollControlIntoView(chatItem);
        }

        public void AddMessage(string message, bool stacktoleft, Color textcolor, Color backcolor)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            if (chatItem.TextColor != textcolor)
            {
                chatItem.TextColor = textcolor;
            }
            if (chatItem.BackGroundColor != backcolor)
            {
                chatItem.BackGroundColor = backcolor;
            }
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            chatItems.Add(chatItem);
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
            chatItems.Insert(0, chatItem);
            panel_Chat.Controls.Add(chatItem);
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
        }
        private void AddMessageToTop(string message, bool stacktoleft, Color textcolor, Color backcolor)
        {
            panel_Chat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            if (chatItem.TextColor != textcolor)
            {
                chatItem.TextColor = textcolor;
            }
            if (chatItem.BackGroundColor != backcolor)
            {
                chatItem.BackGroundColor = backcolor;
            }
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            chatItem.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            chatItems.Insert(0, chatItem);
            panel_Chat.Controls.Add(chatItem);
            chatItem.ResizeBubbles();
            panel_Chat.ResumeLayout();
        }

        public void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            textboxWriting.Select();
            if (e.KeyCode == Keys.Enter && !locking && !(e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter))
            {
                if (!string.IsNullOrWhiteSpace(textboxWriting.Texts))
                {
                    AFriendClient.Send_to_id(AFriendClient.client, FormApplication.currentID, AFriendClient.user.id, textboxWriting.Texts);
                    AddMessage(textboxWriting.Texts, false);
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                    Console.WriteLine("Wrote");
                    blockSending();
                    textboxWriting.Multiline = false;
                }
            }  
        }

        private void blockSending()
        {
            locking = true;
            timerChat.Start();
        }

        public void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts) && !locking)
            {
                AFriendClient.Send_to_id(AFriendClient.client, FormApplication.currentID, AFriendClient.user.id, textboxWriting.Texts);
                AddMessage(textboxWriting.Texts, false);
                textboxWriting.Texts = "";
                textboxWriting.RemovePlaceHolder();
                blockSending();
            }
        }

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(stateColor, 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(pen, friendPicture.Left - 1, friendPicture.Top - 1, friendPicture.Width + 2, friendPicture.Width + 2);
            }

            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, panelTopRight.Height - 1, panelTopRight.Width, panelTopRight.Height - 1);
                e.Graphics.DrawLine(pen, 0, panelTopRight.Height, 0, 0);
            }
        }

        private void panel_Chat_Click(object sender, EventArgs e)
        {
            panel_Chat.Focus();
        }

        public void LoadMessage()
        {
            panel_Chat.SuspendLayout();
            AddMessageToTop("Tạm biệt", false);
            AddMessageToTop("Chào Tạm biệt", true);
            AddMessageToTop("Chào", false);
            AddMessageToTop("Chào bạn", true);
            panel_Chat.ResumeLayout();
        }

        private void PanelChat_Load(object sender, EventArgs e)
        {
            textboxWriting.Focus();
            this.ActiveControl = textboxWriting;
        }

        public string GetLastMessage()
        {
            if (chatItems.Count == 0)
                return "";
            return chatItems[chatItems.Count - 1].Texts;
        }
        public string GetFirstMessage()
        {
            if (chatItems.Count == 0)
                return "";
            return chatItems[0].Texts;
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
            locking = false;
            textboxWriting.Select();

            timerChat.Stop();
        }

        private void panel_Chat_Scroll(object sender, ScrollEventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value == 0)
            {
                LoadMessage();
            }
        }

        private void textboxWriting_SizeChanged(object sender, EventArgs e)
        {
            panelBottomRight.Height = textboxWriting.Height + textboxWriting.Top * 2;
            panelBottomRight.Location = new Point(0, this.Height - panelBottomRight.Height);
            panel_Chat.Height = this.Height - panelBottomRight.Height - panelTopRight.Height;
        }

        private void panelBottomRight_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 1, panelBottomRight.Width, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, panelBottomRight.Height);
            }
        }

        private void panelTopRight_Resize(object sender, EventArgs e)
        {
            panelTopRight.Invalidate();
        }

        private void panelBottomRight_Resize(object sender, EventArgs e)
        {
            textboxWriting.DynamicResize();
            panelBottomRight.Invalidate();
        }

        private void panel_Chat_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 0, 0, panel_Chat.Height);
            }
        }

        private void textboxWriting__TextChanged(object sender, EventArgs e)
        {
            textboxWriting.Multiline = true;
        }
    }
}

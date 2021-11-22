﻿using System;
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
        public A_Friend.CustomControls.PanelChat currentpanelchat;

        public delegate void AddContactItem(Account acc);
        public AddContactItem addContactItemDelegate;
        public delegate void AddMessageItem(string str, bool left);
        public AddMessageItem addMessageItemDelegate;
        public delegate void TurnContactActiveState(string id, byte state);
        public TurnContactActiveState turnContactActiveStateDelegate;
        public Dictionary<string, CustomControls.PanelChat> panelChats = new Dictionary<string, CustomControls.PanelChat>();
        public static string currentID;

        Dictionary<string, CustomControls.ContactItem> contactItems = new Dictionary<string, CustomControls.ContactItem>();
        SortedDictionary<int, string> orderOfContactItems = new SortedDictionary<int, string>();

        public string currentUsername;
        private Panel panelRight2 = new Panel();
        private Panel panelContact2 = new Panel();
        private Panel panelGetStarted = new Panel();
        private FormGetStarted formGetStarted = new FormGetStarted();
        private bool check = true;
        private string searchText = "";

        public FormApplication()
        {
            InitializeComponent();
            this.SetStyle(
            System.Windows.Forms.ControlStyles.UserPaint |
            System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
            System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            InitializeSubPanels();
            addContactItemDelegate = new AddContactItem(AddContact);
            turnContactActiveStateDelegate = new TurnContactActiveState(TurnActiveState);
            //addMessageItemDelegate = new AddMessageItem(AddMessage);
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            //AddContact(new Account("DaiLoi", "Lê Đoàn Đại Lợi", "1111", 1));
            //AddContact(new Account("DangKhoa", "Võ Văn Đăng Khoa", "2222", 2));
            //AddContact(new Account("PhuongQuyen", "Lê Thị Phương Quyên", "3333", 1));
            //AddContact(new Account("ThanhTu", "Thanh Tu", "4444", 1));
            //AddContact(new Account("AnhPhong", "Anh Phong", "5555", 0));
            //AddContact(new Account("LoiDai", "Le Loi", "9999", 0));
            //AddContact(new Account("KhoaDang", "Vo Khoa", "32143", 1));
            //AddContact(new Account("TuThanh", "Vo Tu", "11rew11", 2));
            //AddContact(new Account("QuyenPhuong", "Le Quyen", "1eqwr111", 1));
            //AddContact(new Account("PhongAnh", "Nguyen Phong", "132414111", 0));

            if (panelChats.Count > 0)
                ShowPanelChat(panelChats.Keys.Last());
            else
            {
                panelRight.Controls.Clear();
                customTextBoxSearch.Visible = false;
                formGetStarted.Dock = DockStyle.Fill;
                formGetStarted.TopLevel = false;
                formGetStarted.FormBorderStyle = FormBorderStyle.None;
                panelGetStarted.Controls.Add(formGetStarted);
                panelGetStarted.BringToFront();
                formGetStarted.Visible = true;
            }
            notifyIconApp.BalloonTipTitle = "Notify";
            notifyIconApp.BalloonTipText = "Apps running in the background";
            notifyIconApp.Text = "AppChat";
            this.ResumeLayout();
        }

        private void InitializeSubPanels()
        {
            this.Controls.Add(panelRight2);
            this.panelRight2.Anchor = panelRight.Anchor;
            this.panelRight2.Location = panelRight.Location;
            this.panelRight2.Size = panelRight.Size;
            this.panelRight2.Margin = panelRight.Margin;

            this.panelLeft.Controls.Add(this.panelContact2);
            this.panelContact2.Padding = panelContact.Padding;
            this.panelContact2.Margin = panelContact.Margin;
            this.panelContact2.Anchor = panelContact.Anchor;
            this.panelContact2.AutoScroll = true;
            this.panelContact2.BackColor = panelContact.BackColor;
            this.panelContact2.Location = panelContact.Location;
            this.panelContact2.Size = panelContact.Size;
            this.panelContact2.Paint += panelContact_Paint;

            this.Controls.Add(panelGetStarted);
            panelGetStarted.SendToBack();
            panelGetStarted.Anchor = panelRight.Anchor;
            panelGetStarted.Location = new Point(0, 0);
            panelGetStarted.Size = new Size(this.Width, panelBottomLeft.Top + 2);
            panelGetStarted.Padding = new Padding(1);

            panelAdd.SendToBack();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            currentpanelchat.buttonSend_Click(sender, e);
        }
        private void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            currentpanelchat.textboxWriting_KeyDown(sender, e);
        }

        public void AddContact(Account account)
        {
            if (!panelChats.ContainsKey(account.id))
            {
                panelContact.SuspendLayout();
                var contactItem = new CustomControls.ContactItem(account);
                contactItem.Dock = DockStyle.Top;
                contactItem.BackColor = panelContact.BackColor;
                panelContact.Controls.Add(contactItem);
                //contactItem.BringToFront();
                panelContact.ResumeLayout();

                if (orderOfContactItems.Count == 0)
                {
                    orderOfContactItems.Add(0, account.id);
                }
                else
                {
                    orderOfContactItems.Add(orderOfContactItems.Keys.Last() + 1, account.id);
                }

                panelContact.ScrollControlIntoView(contactItem);
                contactItems.Add(account.id, contactItem);
                CustomControls.PanelChat panelChat = new CustomControls.PanelChat(account);
                panelChats.Add(account.id, panelChat);

                panelChat.LoadMessage();
                contactItem.LastMessage = panelChat.GetLastMessage();

                panelChat.ControlAdded += delegate
                {
                    contactItem.LastMessage = panelChat.GetLastMessage();
                    if (contactItem.ID != orderOfContactItems.Values.Last())
                    {
                        BringContactItemToTop(panelChat.ID);
                    }

                    if (GetCurrentPanelChatId() != panelChat.ID)
                    {
                        if (!panelChat.IsLastMessageFromYou())
                        {
                            contactItem.Unread = true;
                        }
                    }
                    else
                    {
                        contactItem.Unread = false;
                    }
                };

                panelChat.ControlRemoved += delegate
                {
                    contactItem.LastMessage = panelChat.GetLastMessage();
                };

                contactItem.Click += delegate
                {
                    ShowPanelChat(account.id);
                    contactItem.Unread = false;

                    if (!string.IsNullOrEmpty(customTextBoxSearch.Texts))
                    {
                        check = false;
                        customTextBoxSearch.Texts = "";
                        customTextBoxSearch.SetPlaceHolder();
                        panelContact2.Controls.Clear();
                        panelContact.Controls.Clear();
                        foreach(KeyValuePair<int, string> i in orderOfContactItems)
                        {
                            panelContact.Controls.Add(contactItems[i.Value]);
                        }
                        panelContact.BringToFront();
                        check = true;
                    }
                };
            }
        }

        private string GetCurrentPanelChatId()
        {
            if (panelRight.Controls.Count > 0)
            {
                if (panelRight.Controls[0] is CustomControls.PanelChat)
                {
                    return (panelRight.Controls[0] as CustomControls.PanelChat).ID;
                }
            }

            if (panelRight2.Controls.Count > 0)
            {
                if (panelRight2.Controls[0] is CustomControls.PanelChat)
                {
                    return (panelRight2.Controls[0] as CustomControls.PanelChat).ID;
                }

            }

            return "";
        }

        private void BringContactItemToTop(string id)
        {
            if (orderOfContactItems.Count <= 1)
                return;

            int key = -1;
            foreach (KeyValuePair<int, string> i in orderOfContactItems)
            {
                if (i.Value == id)
                {
                    key = i.Key;
                    break;
                }
            }

            if (key != -1)
            {
                orderOfContactItems.Remove(key);
                orderOfContactItems.Add(orderOfContactItems.Keys.Last() + 1, id);
            }

            CustomControls.ContactItem item = contactItems[id];
            if (searchText == "")
            {
                panelContact.Controls.Remove(item);
                panelContact.Controls.Add(item);
            }
        }

        public void ShowPanelChat(string id)
        {
            CustomControls.PanelChat item = panelChats[id];

            if (panelRight.Controls.Count == 0)
            {
                if ((GetCurrentPanelChatId() == "") || !(panelRight2.Controls[0] is CustomControls.PanelChat) || (panelRight2.Controls[0] as CustomControls.PanelChat).ID != id)
                {
                    panelRight.Controls.Add(item);
                    panelRight.BringToFront();
                    panelRight2.SendToBack();
                    panelRight2.Controls.Clear();
                }
            }
            else
            {
                if ((GetCurrentPanelChatId() == "") || !(panelRight.Controls[0] is CustomControls.PanelChat) || (panelRight.Controls[0] as CustomControls.PanelChat).ID != id)
                {
                    panelRight2.Controls.Add(item);
                    panelRight2.BringToFront();
                    panelRight.SendToBack();
                    panelRight.Controls.Clear();
                }
            }
            item.Dock = DockStyle.Fill;
            currentID = item.ID;
            currentpanelchat = item;
        }

        // state (0,1,2) => (offline, online, away)
        public void TurnActiveState(string id, byte state)
        {
            if (panelChats.ContainsKey(id))
            {
                CustomControls.PanelChat item = panelChats[id];
                item.State = state;
            }

            if (contactItems.ContainsKey(id))
            {
                CustomControls.ContactItem item = contactItems[id];
                item.State = state;
            }

            Console.WriteLine("state changed");

        }

        private void LogoutButton_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin lg = new FormLogin();
            lg.Show();
            Program.mainform = null;
            try
            {
                if (AFriendClient.user != null)
                {
                    AFriendClient.user.state = 0;
                }
            } catch /*(Exception ex)*/
            {
                AFriendClient.user = null;
            }
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            FormSettings frm = new FormSettings();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
        }
        int tempadd = 0;
        public void ButtonAdd_Click_1(object sender, EventArgs e)
        {
            PanelGetStartedSlideToRight();
            FormCollection forms = Application.OpenForms;
            FormAddContact frm = new FormAddContact();
            //panelContact.Height = panelContact.Height - panel2.Height;
            //panelContact2.Height = panelContact.Height - panel2.Height;
            //panel2.Show();
            //frm.TopLevel = false;
            //panel2.Controls.Add(frm);
            //frm.Show();
            //i = Application.OpenForms.Count;
            do
            {
                if (tempadd > 2)
                {
                    if (tempadd > 2)
                    {
                        panelAdd.Hide();
                        panelContact.Height = panelContact.Height + panelAdd.Height;
                        panelContact2.Height = panelContact.Height + panelAdd.Height;
                        tempadd = 0;
                        break;
                    }
                    return;
                }
                panelContact.Height = panelContact.Height - panelAdd.Height;
                panelContact2.Height = panelContact.Height - panelAdd.Height;
                panelAdd.Show();
                frm.TopLevel = false;
                panelAdd.Controls.Add(frm);
                frm.Show();
                tempadd = Application.OpenForms.Count;
                break;
            }
            while (false);
            //PanelGetStartedFill();
            //Reload list friends
        }

        private void FormApplication_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.mainform = null;
            Application.Exit();
        }

        private bool UsernameCheck()
        {
            //Check username in list friends
            return true;
        }
        private void customTextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (customTextBoxSearch.Text == "")
            //    {
            //        labelWarning.Text = "Please enter a username";
            //    }
            //    else
            //    {
            //        if (!UsernameCheck())
            //            labelWarning.Text = "This user does not exist";
            //        else
            //            labelUsername.Text = customTextBoxSearch.Texts;
            //        //Load chat history from database
            //    }
            //}
        }
        private void customTextBoxSearch__TextChanged(object sender, EventArgs e)
        {
            if (check)
            {
                string text = customTextBoxSearch.Texts.Trim().ToLower();
                if (text == searchText)
                    return;
                searchText = text;
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (panelContact.Controls.Count > 0)
                    {
                        foreach (KeyValuePair<string, CustomControls.ContactItem> i in contactItems)
                        {
                            if (i.Value.FriendName.ToLower().Contains(searchText))
                            {
                                panelContact2.Controls.Add(i.Value);
                            }
                        }
                        panelContact2.BringToFront();
                        panelContact.Controls.Clear();
                    }
                    else
                    {
                        foreach (KeyValuePair<string, CustomControls.ContactItem> i in contactItems)
                        {
                            if (i.Value.FriendName.ToLower().Contains(searchText))
                            {
                                panelContact.Controls.Add(i.Value);
                            }
                        }
                        panelContact.BringToFront();
                        panelContact2.Controls.Clear();
                    }
                }
                else
                {
                    panelContact.Controls.Clear();
                    panelContact2.Controls.Clear();
                    foreach(KeyValuePair<int, string> i in orderOfContactItems)
                    {
                        panelContact.Controls.Add(contactItems[i.Value]);
                    }
                    panelContact.BringToFront();
                }
            }
        }

        private void panelChat_Click(object sender, EventArgs e)
        {
            //panelChat.Focus();
        }

        internal bool Is_this_person_added(string id)
        {
            return contactItems.ContainsKey(id);
        }

        private void notifyIconApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIconApp.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        private void closeMessengerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
            notifyIconApp.Icon = null;
        }

        private void FormApplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                e.Cancel = true;
                ((Control)sender).Hide();
                notifyIconApp.Visible = true;
                notifyIconApp.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIconApp.Visible = false;
            }
        }

        private void PanelGetStartedSlideToRight()
        {
            panelGetStarted.Location = panelRight.Location;
            panelGetStarted.Size = new Size(panelRight.Width, this.Height);
            formGetStarted.TopColor = panelTopLeft.BackColor;
            var graphic = panelGetStarted.CreateGraphics(); 
            using (Pen pen = new Pen (Color.Gray, 1))
            {
                graphic.DrawLine(pen, 0, 0, 0, panelGetStarted.Height - 1);
            }

        }

        private void PanelGetStartedFill()
        {
            if (contactItems.Count == 0) {
                panelGetStarted.Location = new Point(0, 0);
                panelGetStarted.Size = new Size(this.Width, panelGetStarted.Height);
                formGetStarted.TopColor = Color.White;
            }
        }

        private void panelContact_ControlAdded(object sender, ControlEventArgs e)
        {
            if (panelGetStarted.Location.X != panelRight.Location.X)
            {
                PanelGetStartedSlideToRight();
            }
            customTextBoxSearch.Visible = true;
        }

        private void panelTopLeft_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, panelTopLeft.Height - 1, panelTopLeft.Width, panelTopLeft.Height -  1);
                //e.Graphics.DrawLine(pen, panelTopLeft.Width - 1, 0, panelTopLeft.Width - 1, panelTopLeft.Height);
            }
        }

        private void panelContact_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                //e.Graphics.DrawLine(pen, panelContact.Width - 1, 0, panelContact.Width - 1, panelContact.Height);
            }
        }

        private void panelBottomLeft_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 1, panelBottomLeft.Width - 0, 1);
                //e.Graphics.DrawLine(pen, panelBottomLeft.Width - 1, 0, panelBottomLeft.Width - 1, panelBottomLeft.Height);
            }
        }
    }
}

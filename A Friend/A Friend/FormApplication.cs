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
        public A_Friend.CustomControls.PanelChat currentpanelchat;

        public delegate void AddContactItem(Account acc);
        public AddContactItem addContactItemDelegate;
        public delegate void AddMessageItem(string str, bool left);
        public AddMessageItem addMessageItemDelegate;
        public Dictionary<string, CustomControls.PanelChat> panelChats = new Dictionary<string, CustomControls.PanelChat>();
        public static string currentID;

        Dictionary<string, CustomControls.ContactItem> contactItems = new Dictionary<string, CustomControls.ContactItem>();

        public string currentUsername;
        private Panel panelRight2 = new Panel();
        private Panel panelContact2 = new Panel();
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
            //addMessageItemDelegate = new AddMessageItem(AddMessage);
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            // Add some sample ContactItems
            //AddContact("DaiKhoa", "You can differentiate between builds clearly", true);
            //AddContact("ThanhPhong", "You: If you don’t have Microsoft Edge (dev or beta) installed ", false);
            //AddContact("AnhQuyen", "Linux Mint, and other Ubuntu-based distros walks though the steps needed to add the Microsoft Linux repo", false);
            //AddContact("DangTu", "So why use Edge?", true);
            //AddContact("PhuongLoi", "Honestly, nothing Microsoft can do will ever be universally loved by Linux users. ", false);
            //AddContact("AnhKhoa", "If you’re a fan of Edge", true);
            //AddContact("DaiLoi", "You: hello", false);
            //AddContact("DangKhoa", "You: Microsoft Edge for Linux has reached stable status after spending more than a year in development.", true);
            //AddContact("ThanhTu", "You: Now that date has arrived!", false);
            //AddContact("PhuongQuyen", "Now that date has arrived!", true);
            //AddContact("AnhPhong", "You: Just open a new terminal and run the following command ", false);
            AddContact(new Account("DaiLoi", "Dai Loi", "1111", 1));
            AddContact(new Account("DangKhoa", "Dang Khoa", "2222", 2));
            AddContact(new Account("PhuongQuyen", "Phuong Quyen", "3333", 1));
            AddContact(new Account("ThanhTu", "Thanh Tu", "4444", 1));
            AddContact(new Account("AnhPhong", "Anh Phong", "5555", 0));
            AddContact(new Account("LoiDai", "Le Loi", "9999", 0));
            AddContact(new Account("KhoaDang", "Vo Khoa", "32143", 1));
            AddContact(new Account("TuThanh", "Vo Tu", "11rew11", 2));
            AddContact(new Account("QuyenPhuong", "Le Quyen", "1eqwr111", 1));
            AddContact(new Account("PhongAnh", "Nguyen Phong", "132414111", 0));

            ShowPanelChat(panelChats.Keys.Last());
            this.ResumeLayout();
            notifyIconApp.BalloonTipTitle = "Notify";
            notifyIconApp.BalloonTipText = "Apps running in the background";
            notifyIconApp.Text = "AppChat";
        }

        private void InitializeSubPanels()
        {
            this.Controls.Add(panelRight2);
            this.panelRight2.Anchor = panelRight.Anchor;
            this.panelRight2.Location = panelRight.Location;
            this.panelRight2.Size = panelRight.Size;
            this.panelRight2.Margin = panelRight.Margin;

            this.panelLeft.Controls.Add(this.panelContact2);
            this.panelContact2.Anchor = panelContact.Anchor;
            this.panelContact2.AutoScroll = true;
            this.panelContact2.BackColor = panelContact.BackColor;
            this.panelContact2.Location = panelContact.Location;
            this.panelContact2.Size = panelContact.Size;
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

                panelContact.ScrollControlIntoView(contactItem);
                contactItems.Add(account.id, contactItem);
                CustomControls.PanelChat panelChat = new CustomControls.PanelChat(account);
                panelChats.Add(account.id, panelChat);

                panelChat.LoadMessage();
                contactItem.LastMessage = panelChat.GetFirstMessage();

                panelChat.ControlAdded += delegate
                {
                    contactItem.LastMessage = panelChat.GetLastMessage();

                    if (!panelChat.IsLastMessageFromYou())
                    {
                        contactItem.Unread = true;
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
                        panelContact.Controls.Clear();
                        foreach (KeyValuePair<string, CustomControls.ContactItem> i in contactItems)
                        {
                            panelContact.Controls.Add(i.Value);
                        }
                        panelContact.BringToFront();
                        check = true;
                    }
                };
            }
        }

        public void ShowPanelChat(string id)
        {
            CustomControls.PanelChat item = panelChats[id];

            if (panelRight.Controls.Count == 0)
            {
                if (!(panelRight2.Controls[0] is CustomControls.PanelChat) || (panelRight2.Controls[0] as CustomControls.PanelChat).ID != id)
                {
                    panelRight.Controls.Add(item);
                    panelRight.BringToFront();
                    panelRight2.SendToBack();
                    panelRight2.Controls.Clear();
                }
            }
            else
            {
                if (!(panelRight.Controls[0] is CustomControls.PanelChat) || (panelRight.Controls[0] as CustomControls.PanelChat).ID != id)
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

        private void ButtonAdd_Click_1(object sender, EventArgs e)
        {
            FormAddContact frm = new FormAddContact();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
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
            if (e.KeyCode == Keys.Enter)
            {
                if (customTextBoxSearch.Text == "")
                {
                    labelWarning.Text = "Please enter a username";
                }
                else
                {
                    if (!UsernameCheck())
                        labelWarning.Text = "This user does not exist";
                    else
                        labelUsername.Text = customTextBoxSearch.Texts;
                    //Load chat history from database
                }
            }
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
                    foreach (KeyValuePair<string, CustomControls.ContactItem> i in contactItems)
                    {
                        panelContact.Controls.Add(i.Value);
                    }
                    panelContact.BringToFront();
                }
            }
        }

        private void panelChat_Click(object sender, EventArgs e)
        {
            panelChat.Focus();
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
    }
}

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
        public delegate void SortContactItemsdelegate();
        public SortContactItemsdelegate sort_contact_item_delegate;

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
        private Panel panelLoading = new Panel();
        private PictureBox pictureBoxNotFound = new PictureBox();
        private FormContactRemoved formContactRemoved = new FormContactRemoved();
        private FormGetStarted formGetStarted = new FormGetStarted();
        private FormLoading formLoading = new FormLoading();    
        public FormSettings formSettings = new FormSettings();
        public FormAddContact formAddContact = new FormAddContact();
        private bool check = true;
        private string searchText = "";
        private bool loaded = false;

        public FormApplication()
        {
            InitializeComponent();
            this.ResizeBegin += FormApplication_ResizeBegin;
            this.ResizeEnd += FormApplication_ResizeEnd;
            this.SetStyle(
            System.Windows.Forms.ControlStyles.UserPaint |
            System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
            System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            InitializeSubPanels();
            addContactItemDelegate = new AddContactItem(AddContact);
            turnContactActiveStateDelegate = new TurnContactActiveState(TurnActiveState);
            sort_contact_item_delegate = new SortContactItemsdelegate(SortContactItems);
            //addMessageItemDelegate = new AddMessageItem(AddMessage);
        }

        private void FormApplication_ResizeBegin(Object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void FormApplication_ResizeEnd(Object sender, EventArgs e)
        {
            this.ResumeLayout(true);
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

            //if (panelChats.Count > 0)
            //    ShowPanelChat(panelChats.Keys.Last());
            //else
            //{
            //    panelRight.Controls.Clear();
            //    customTextBoxSearch.Visible = false;
            //    formGetStarted.Dock = DockStyle.Fill;
            //    formGetStarted.TopLevel = false;
            //    formGetStarted.FormBorderStyle = FormBorderStyle.None;
            //    panelGetStarted.Controls.Add(formGetStarted);
            //    panelGetStarted.BringToFront();
            //    formGetStarted.Visible = true;
            //}
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
            panelGetStarted.Resize += delegate { 
                if (panelGetStarted.Width != this.Width)
                {
                    var graphic = panelGetStarted.CreateGraphics();
                    using (Pen pen = new Pen(Color.Gray, 1))
                    {
                        graphic.DrawLine(pen, 0, 0, 0, panelGetStarted.Height - 1);
                    }
                }
            };    

            panelAdd.Hide();
            formAddContact.Dock = DockStyle.Fill;
            formAddContact.TopLevel = false;
            panelAdd.Controls.Add(formAddContact);
            panelAdd.BringToFront();
            formAddContact.Visible = true;

            this.Controls.Add(panelLoading);
            panelLoading.Location = new Point(0, 0);
            panelLoading.Size = this.Size;
            panelLoading.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            formLoading.TopLevel = false;
            formLoading.Size = panelLoading.Size;
            formLoading.Anchor = panelLoading.Anchor;
            //formLoading.Dock = DockStyle.Fill;
            formLoading.Visible = true;
            panelLoading.Controls.Add(formLoading);
            panelLoading.BringToFront();

            pictureBoxNotFound.Dock = DockStyle.Fill;
            pictureBoxNotFound.Image = global::A_Friend.Properties.Resources.no_result_found;
            pictureBoxNotFound.SizeMode = PictureBoxSizeMode.Zoom;
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
                //contactItem.BringToFront();
                if (loaded)
                {
                    panelContact.Controls.Add(contactItem);
                }
                panelContact.ResumeLayout();


                if (orderOfContactItems.Count == 0)
                {
                    orderOfContactItems.Add(0, account.id);
                }
                else
                {
                    orderOfContactItems.Add(orderOfContactItems.Keys.Last() + 1, account.id);
                }

                if (loaded)
                {
                    panelContact.ScrollControlIntoView(contactItem);
                }
                contactItems.Add(account.id, contactItem);
                CustomControls.PanelChat panelChat = new CustomControls.PanelChat(account);
                panelChats.Add(account.id, panelChat);

                panelChat.LoadMessage();
                panelChat.ScrollToBottom();
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
                    panelChat.ScrollToBottom();

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
            else
            {
                formAddContact.ChangeWarning("This user existed in your contacting list!", Color.Red);
            }
        }

        public void SortContactItems()
        {
            int lenght = contactItems.Count;
            for (int i = 0; i < contactItems.Count; i++)
            {
                string min = "";
                int j = 0;
                foreach (KeyValuePair<int, string> keyValuePair in orderOfContactItems)
                {
                    if (j == lenght)
                        break;
                    if (min == "")
                    {
                        min = keyValuePair.Value;
                    }
                    else
                    {
                        if (panelChats[min].DateTimeOflastMessage > panelChats[keyValuePair.Value].DateTimeOflastMessage)
                        {
                            min = keyValuePair.Value;
                        }
                    }
                    j++;
                }
                lenght--;
                BringContactItemToTop(min);
            }

            foreach (KeyValuePair <int, string> keyValuePair1 in orderOfContactItems)
            {
                panelContact.Controls.Add(contactItems[keyValuePair1.Value]);
            }

            loaded = true;

            panelLoading.SendToBack();
            formLoading.StopSpinning();
            formLoading.Dispose();
            panelLoading.Dispose();

            if (panelChats.Count > 0)
                ShowPanelChat(orderOfContactItems.Values.Last());
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

            if (loaded)
            {
                CustomControls.ContactItem item = contactItems[id];
                if (searchText == "")
                {
                    panelContact.Controls.Remove(item);
                    panelContact.Controls.Add(item);
                }
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
            //FormSettings frm = new FormSettings();
            formSettings.StartPosition = FormStartPosition.CenterScreen;
            this.Hide();
            formSettings.ShowDialog();
            this.Show();
        }
        //int tempadd = 0;
        public void ButtonAdd_Click_1(object sender, EventArgs e)
        {
            PanelGetStartedSlideToRight();
            //FormCollection forms = Application.OpenForms;
            //FormAddContact frm = new FormAddContact();
            ////panelContact.Height = panelContact.Height - panel2.Height;
            ////panelContact2.Height = panelContact.Height - panel2.Height;
            ////panel2.Show();
            ////frm.TopLevel = false;
            ////panel2.Controls.Add(frm);
            ////frm.Show();
            ////i = Application.OpenForms.Count;
            //do
            //{
            //    if (tempadd >= 2)
            //    {
            //        if (tempadd >= 2)
            //        {
            //            panelAdd.Hide();
            //            panelContact.Height = panelContact.Height + panelAdd.Height;
            //            panelContact2.Height = panelContact.Height + panelAdd.Height;
            //            tempadd = 0;
            //            break;
            //        }
            //        return;
            //    }
            //    panelContact.Height = panelContact.Height - panelAdd.Height;
            //    panelContact2.Height = panelContact.Height - panelAdd.Height;
            //    panelAdd.Show();
            //    frm.TopLevel = false;
            //    panelAdd.Controls.Add(frm);
            //    frm.Show();
            //    tempadd = Application.OpenForms.Count;
            //    break;
            //}
            //while (false);
            ////PanelGetStartedFill();
            ////Reload list friends
            //if (formAddContact == null)
            //{
            //    formAddContact = new FormAddContact();
            //    formAddContact.Dock = DockStyle.Fill;
            //    formAddContact.TopLevel = false;
            //    panelAdd.Controls.Add(formAddContact);
            //    panelAdd.BringToFront();
            //    formAddContact.Visible = true;
            //}

            if (panelContact.Height == panelBottomLeft.Top - panelTopLeft.Bottom)
            {
                panelContact.Height -= panelAdd.Height;
                panelContact2.Height -= panelAdd.Height;
                formAddContact.ResetTexts();
                formAddContact.ChangeWarning("Enter your friend's user name", Color.FromArgb(143, 228, 185));
                panelAdd.Show();
            }
            else
            {
                panelContact.Height += panelAdd.Height;
                panelContact2.Height += panelAdd.Height;
                panelAdd.Hide();
            }
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
                        if (panelContact2.Controls.Count == 0)
                        {
                            if (panelContact.Controls.Contains(pictureBoxNotFound))
                            {
                                return;
                            }
                            panelContact2.Controls.Add(pictureBoxNotFound);
                            pictureBoxNotFound.BringToFront();
                            Console.WriteLine("not found");
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
                        if (panelContact.Controls.Count == 0)
                        {
                            if (panelContact2.Controls.Contains(pictureBoxNotFound))
                            {
                                return;
                            }
                            panelContact.Controls.Add(pictureBoxNotFound);
                            pictureBoxNotFound.BringToFront();
                            Console.WriteLine("not found2");
                        }
                        //panelContact2.BringToFront();
                        //panelContact.Controls.Clear();
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

        public void RemoveContact(string id)
        {
            if (!panelChats.ContainsKey(id) || !contactItems.ContainsKey(id))
                return;

            if (panelContact.Controls.Contains(contactItems[id]))
            {
                panelContact.Controls.Remove(contactItems[id]); 
            }
            else if (panelContact2.Controls.Contains(contactItems[id]))
            {
                panelContact2.Controls.Remove(contactItems[id]); 
            }

            if (panelRight.Controls.Contains(panelChats[id]))
            {
                if (id == GetCurrentPanelChatId())
                {
                    panelRight.Controls.Remove(panelChats[id]);
                    formContactRemoved.Dock = DockStyle.Fill;
                    formContactRemoved.TopLevel = false;
                    panelRight.Controls.Add(formContactRemoved);
                    panelRight.BringToFront();
                    formContactRemoved.Visible = true;
                }
                else
                {
                    panelRight.Controls.Remove(panelChats[id]);
                }
            }
            else if (panelRight2.Controls.Contains(panelChats[id]))
            {
                if (id == GetCurrentPanelChatId())
                {
                    panelRight2.Controls.Remove(panelChats[id]);
                    formContactRemoved.Dock = DockStyle.Fill;
                    formContactRemoved.TopLevel = false;
                    panelRight2.Controls.Add(formContactRemoved);
                    panelRight2.BringToFront();
                    formContactRemoved.Visible = true;
                }
                else
                {
                    panelRight2.Controls.Remove(panelChats[id]);
                }
            }

            panelChats.Remove(id);
            contactItems.Remove(id);
            foreach (KeyValuePair<int, string> pair in orderOfContactItems)
            {
                if (pair.Value == id)
                {
                    orderOfContactItems.Remove(pair.Key);
                    break;
                }
            }

            //code to remove or block contact
        }
    }
}

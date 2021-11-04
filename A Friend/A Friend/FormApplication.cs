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

        Dictionary<string, CustomControls.PanelChat> panelChats = new Dictionary<string, CustomControls.PanelChat>();
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
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
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

        public void AddContact(Account account)
        {
            panelContact.SuspendLayout();
            var contactItem = new CustomControls.ContactItem(account);
            contactItem.Dock = DockStyle.Top;
            contactItem.BackColor = panelContact.BackColor;
            panelContact.Controls.Add(contactItem);
            //contactItem.BringToFront();
            panelContact.ResumeLayout();

            panelContact.ScrollControlIntoView(contactItem);
            contactItems.Add(account.id ,contactItem);
            CustomControls.PanelChat panelChat = new CustomControls.PanelChat(account); 
            panelChats.Add(account.id , panelChat);

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
                    contactItem.Unread= false;
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

        private void ShowPanelChat(string id)
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
            Application.Exit();
        }

        private void LogoutButton_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin lg = new FormLogin();
            lg.Show();
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
    }
}

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

        List<CustomControls.PanelChat> panelChats = new List<CustomControls.PanelChat>();

        public string currentUsername;
        private Panel panelRight2 = new Panel();

        public FormApplication()
        {
            InitializeComponent();
            labelWarning.Visible = false;
            this.SetStyle(
            System.Windows.Forms.ControlStyles.UserPaint |
            System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
            System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
            true);

            this.Controls.Add(panelRight2);
            this.panelRight2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
           | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight2.Location = new System.Drawing.Point(300, 0);
            this.panelRight2.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight2.Name = "panelRight2";
            this.panelRight2.Size = new System.Drawing.Size(912, 712);
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
            this.ResumeLayout();
        }

        public void AddMessage(string message, bool stacktoleft)
        {
            panelChat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panelChat.BackColor;
            panelChat.Controls.Add(chatItem);
            chatItem.BringToFront();
            chatItem.ResizeBubbles();
            panelChat.ResumeLayout();
            panelChat.ScrollControlIntoView(chatItem);
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
            contactItem.Click += delegate 
            {
                showPanelChat(account); 
            };
        }
        private CustomControls.PanelChat checkPanelChatExisted(string ID)
        {
            foreach (CustomControls.PanelChat i in panelChats)
            {
                if (i.ID == ID)
                {
                    return i;
                }
            }
            return null;
        }

        private void showPanelChat(Account account)
        {
            var item = checkPanelChatExisted(account.id);
            if (item == null)
            {
                item = new CustomControls.PanelChat(account);
                panelChats.Add(item);
            }

            Console.WriteLine("asd;");

            if (panelRight.Controls.Count == 0)
            {
                if (!(panelRight2.Controls[0] is CustomControls.PanelChat) || (panelRight2.Controls[0] as CustomControls.PanelChat).ID != account.id)
                {
                    panelRight.Controls.Add(item);
                    panelRight.BringToFront();
                    panelRight2.SendToBack();
                    panelRight2.Controls.Clear();
                }
            }
            else
            {
                if (!(panelRight.Controls[0] is CustomControls.PanelChat) || (panelRight.Controls[0] as CustomControls.PanelChat).ID != account.id)
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
        public void TurnActiveState(string id, int state)
        {
            foreach (CustomControls.ContactItem item in panelContact.Controls)
            {
                if (item.ID == id)
                {
                    item.State = state;
                }
            }

            foreach (CustomControls.ContactItem item in panelRight.Controls)
            {
                if (item.ID == id)
                {
                    item.State = state;
                }
            }

            foreach (CustomControls.ContactItem item in panelRight2.Controls)
            {
                if (item.ID == id)
                {
                    item.State = state;
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

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            //Pen pen = new Pen(Color.FromArgb(112, 155, 170), 5);
            //e.Graphics.DrawLine(pen, panelTopRight.Left + 5, panelTopRight.Bottom, panelTopRight.Right - 5, panelTopRight.Bottom);
        }

        private void panelTopRight_Resize(object sender, EventArgs e)
        {
            panelTopRight.Invalidate();
            labelWarning.Text = "";
            labelUsername.Text = "";
            //Load friends data from database to listbox
        }

        private void LogoutButton_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin lg = new FormLogin();
            lg.Show();
            try
            {
                if (AFriendClient.user != null)
                {
                    AFriendClient.user.state = 0;
                }
            } catch (Exception ex)
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

        private bool UsernameCheck()
        {
            //Check username in list friends
            return true;
        }

        private void FormApplication_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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
    }
}

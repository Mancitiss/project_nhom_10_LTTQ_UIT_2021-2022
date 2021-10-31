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
        public string currentUsername;
        public FormApplication()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            labelWarning.Visible = false;
        }

        private void FormApplication_Load(object sender, EventArgs e)
        {
            // Add some sample ContactItems
            AddContact("DaiKhoa", "You can differentiate between builds clearly", true);
            AddContact("ThanhPhong", "You: If you don’t have Microsoft Edge (dev or beta) installed ", false);
            AddContact("AnhQuyen", "Linux Mint, and other Ubuntu-based distros walks though the steps needed to add the Microsoft Linux repo", false);
            AddContact("DangTu", "So why use Edge?", true);
            AddContact("PhuongLoi", "Honestly, nothing Microsoft can do will ever be universally loved by Linux users. ", false);
            AddContact("AnhKhoa", "If you’re a fan of Edge", true);
            AddContact("DaiLoi", "You: hello", false);
            AddContact("DangKhoa", "You: Microsoft Edge for Linux has reached stable status after spending more than a year in development.", true);
            AddContact("ThanhTu", "You: Now that date has arrived!", false);
            AddContact("PhuongQuyen", "Now that date has arrived!", true);
            AddContact("AnhPhong", "You: Just open a new terminal and run the following command ", false);
            TurnAwayState("DaiKhoa");
            TurnAwayState("ThanhTu");
            TurnOfflineState("PhuongQuyen");
            TurnOfflineState("DaiLoi");
            TurnOfflineState("DangKhoa");
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

        public void AddContact(string name, string lastmessage, bool unread = false)
        {
            panelContact.SuspendLayout();
            var contactItem = new CustomControls.ContactItem(name, lastmessage, unread);
            contactItem.Name = "Friend_" + contactItem.FriendName;
            contactItem.Dock = DockStyle.Top;
            contactItem.BackColor = panelContact.BackColor;
            panelContact.Controls.Add(contactItem);
            //contactItem.BringToFront();
            panelContact.ResumeLayout();
            panelContact.ScrollControlIntoView(contactItem);
        }

        public void bringContactItemToTop(CustomControls.ContactItem item)
        {
            Console.WriteLine(panelContact.Controls.GetChildIndex(item));
            for (int i = panelContact.Controls.GetChildIndex(item) + 1; i < panelContact.Controls.Count; i++)
            {
                panelContact.Controls.SetChildIndex(panelContact.Controls[i], i - 1);
                panelContact.Controls.SetChildIndex(item, i);
            }
            Console.WriteLine();
        }

        public void TurnActiveState(string name)
        {
            foreach (CustomControls.ContactItem item in panelContact.Controls)
            {
                if (item.Name == "Friend_" + name)
                {
                    item.TurnActive();
                }
            }
        }

        public void TurnAwayState(string name)
        {
            foreach (CustomControls.ContactItem item in panelContact.Controls)
            {
                if (item.Name == "Friend_" + name)
                {
                    item.TurnAway();
                }
            }
        }

        public void TurnOfflineState(string name)
        {
            foreach (CustomControls.ContactItem item in panelContact.Controls)
            {
                if (item.Name == "Friend_" + name)
                {
                    item.TurnOffline();
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

        private void customButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin lg = new FormLogin();
            lg.Show();
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            FormSettings frm = new FormSettings();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog(); 
        }

        private void customTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (customTextBox1.Text == "")
                {
                    labelWarning.Text = "Please enter a username";
                }
                else
                {
                    if (!UsernameCheck())
                        labelWarning.Text = "This user does not exist";
                    else
                        labelUsername.Text = customTextBox1.Texts;
                        //Load chat history from database
                }
            }
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
        //protected override void OnResizeBegin(EventArgs e)

        //{
        //    SuspendLayout();
        //    base.OnResizeBegin(e);
        //}
        //protected override void OnResizeEnd(EventArgs e)
        //{
        //    ResumeLayout();
        //    base.OnResizeEnd(e);
        //}
    }
}

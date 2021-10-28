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
        }
        public void AddMessage(string message, bool stacktoleft)
        {
            panelChat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Name = "chatItem" + panelChat.Controls.Count;
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panelChat.BackColor;
            panelChat.Controls.Add(chatItem);
            chatItem.BringToFront();

            chatItem.ResizeBubbles();
            //chatItem.ResizeBubbles((int)(panelChat.Width * 0.6));
            panelChat.ResumeLayout();

            panelChat.ScrollControlIntoView(chatItem);
            
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
                if (!string.IsNullOrEmpty(textboxWriting.Texts))
                {
                    AddMessage(textboxWriting.Texts, false); 
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                }
            }
        }

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(112, 155, 170), 5);
            e.Graphics.DrawLine(pen, panelTopRight.Left + 5, panelTopRight.Bottom, panelTopRight.Right - 5, panelTopRight.Bottom);
        }

        private void panelTopRight_Resize(object sender, EventArgs e)
        {
            panelTopRight.Invalidate();
            labelWarning.Text = "";
            listFriend.Text = "";
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
            frm.Show();
            this.Hide(); 
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
            frm.Show();
            this.Hide();
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

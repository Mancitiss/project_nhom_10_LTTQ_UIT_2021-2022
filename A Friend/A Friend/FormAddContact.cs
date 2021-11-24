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
    public partial class FormAddContact : Form
    {
        public delegate void ChangeWarningLabel(string text, Color color);
        public ChangeWarningLabel changeWarningLabelDelegate;

        public FormAddContact()
        {
            InitializeComponent();
            labelWarning.ForeColor = Color.FromArgb(143, 228, 185);
            labelWarning.Text = "Enter your friend user name";
            txtNewUser.RemovePlaceHolder();
            changeWarningLabelDelegate = new ChangeWarningLabel(ChangeWarning);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (txtNewUser.Texts == "")
            {
                //labelWarning.Text = "Please enter a username";
                ChangeWarning("Please enter your friend user name", Color.Red);
            }  
            else
            {
                string data = txtNewUser.Texts;
                string databyte = Encoding.Unicode.GetByteCount(data).ToString();
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("0610" + databyte.Length.ToString().PadLeft(2, '0') + databyte + data));
            }
            //this.Hide();
        }

        private void AddContact()
        {
            if (txtNewUser.Texts == "")
            {
                //labelWarning.Text = "Please enter a username";
                ChangeWarning("Please enter your friend user name", Color.Red);
            }  
            else
            {
                string data = txtNewUser.Texts;
                string databyte = Encoding.Unicode.GetByteCount(data).ToString();
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("0610" + databyte.Length.ToString().PadLeft(2, '0') + databyte + data));
            }
        }

        private void FormAddContact_Shown(object sender, EventArgs e)
        {
            txtNewUser.Focus();
        }

        private void txtNewUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddContact();
            }
        }

        public void ChangeWarning(string text, Color textcolor)
        {
            labelWarning.Text = text;
            labelWarning.ForeColor = textcolor;
            //txtNewUser.Texts = "";
            txtNewUser.Location = new Point(txtNewUser.Location.X, (int)(this.Height / 2 - txtNewUser.Height / 2 - labelWarning.Height / 2));
            labelWarning.Location = new Point(0, txtNewUser.Bottom);
        }

        private void FormAddContact_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 1, this.Width, 1);
                //e.Graphics.DrawLine(pen, panelBottomLeft.Width - 1, 0, panelBottomLeft.Width - 1, panelBottomLeft.Height);
            }
        }

        private void txtNewUser__TextChanged(object sender, EventArgs e)
        {
            labelWarning.Text = "";
            txtNewUser.Location = new Point(txtNewUser.Location.X, (int)(this.Height / 2 - txtNewUser.Height / 2));
            txtNewUser.BringToFront();
        }

        public void ResetTexts()
        {
            txtNewUser.Texts = "";
            labelWarning.Text = "";
            txtNewUser.Location = new Point(txtNewUser.Location.X, (int)(this.Height / 2 - txtNewUser.Height / 2));
            txtNewUser.BringToFront();
        }
    }
}

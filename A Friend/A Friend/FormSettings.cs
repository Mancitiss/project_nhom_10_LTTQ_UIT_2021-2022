﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace A_Friend
{
    public partial class FormSettings : Form
    {
        public delegate void ChangeSettingsWarning(string text);
        public ChangeSettingsWarning changeSettingsWarning;

        public FormSettings()
        {
            InitializeComponent();
            labelUsername.Location = new Point((this.Width - labelUsername.Width) / 2 - 5, labelUsername.Top);
            customButtonUsername.Location = new Point(labelUsername.Left - 20, customButtonUsername.Top);
            labelWarning.Text = "";
            changeSettingsWarning = new ChangeSettingsWarning(ChangeLabel);
        }

        public void ChangeLabel(string text)
        {
            labelWarning.Text = text;
            labelWarning.Location = new Point((this.Width - labelWarning.Width) / 2 - 5, labelWarning.Top);
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            this.labelUsername.Text = AFriendClient.user.name;
            panelPassword.Hide();
            panelUsername.Hide();
        }

        private void customButtonUsername_Click(object sender, EventArgs e)
        {
            panelPassword.Hide();
            panelUsername.Show();
        }

        private void buttonSaveUsername_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(customTextBoxUsername.Texts.Trim()))
                //MessageBox.Show("Please enter new username!", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter new name!");
            else
            {
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("1012" + AFriendClient.data_with_byte(customTextBoxUsername.Texts.Trim())));
                AFriendClient.temp_name = customTextBoxUsername.Texts.Trim();
                //panelUsername.Hide();
                this.Close();
            }
        }

        private void customButtonPassword_Click(object sender, EventArgs e)
        {
            panelUsername.Hide();
            panelPassword.Show();
        }

        private void buttonSavePassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCurrentPassword.Texts) || string.IsNullOrEmpty(textBoxConfirmPassword.Texts) || string.IsNullOrEmpty(textBoxNewPassword.Texts))
                //MessageBox.Show("Please enter your password!", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ChangeLabel("Please enter your password!");
            else
            {
                if (textBoxConfirmPassword.Texts.Equals(textBoxNewPassword.Texts))
                {
                    AFriendClient.client.Send(Encoding.Unicode.GetBytes("4269" + AFriendClient.data_with_byte(textBoxCurrentPassword.Texts) + AFriendClient.data_with_byte(textBoxConfirmPassword.Texts)));
                }
                panelPassword.Hide();
            }
        }

        private void customButtonAvatar_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Custom Files|*.pjp;*.jpg;*.pjpeg;*.jpeg;*.jfif;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    Bitmap bitmap = new Bitmap(ofd.FileName);
                    //circlePictureBox1.Image = bitmap;
                    circlePictureBox1.Crop((Image)(bitmap));
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void customButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

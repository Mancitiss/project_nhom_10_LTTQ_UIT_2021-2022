﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace A_Friend
{
    
    public partial class FormLogin : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public FormLogin()
        {
            InitializeComponent();
            labelWarning.Text = "";
            this.MouseDown += (sender, e) => Form1_MouseDown(sender, e);
            Console.WriteLine("test");
        }

        private void ResetTexts()
        {
            textBoxUserName.Texts = "";
            textBoxPassword.Texts = "";
            labelWarning.Text = "";
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            var frm = new FormSignUp();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            this.ResetTexts();
            frm.Show();
            this.Hide();           
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (this.EmptyTextBoxes())
            {
                if (EmptyTextBoxes())
                {
                    labelWarning.Text = "Something is missing!";
                    return;
                }
            }
            if (!CorrectPassword())
            {
                labelWarning.Text = "User name or Password is incorrect";
                return;
            }
            labelWarning.Text = "You have logged in successfully".ToUpper();
            labelWarning.ForeColor = Color.FromArgb(37, 75, 133);
            timerClosing.Start();
        }

        private bool CorrectPassword()
        {
            return true;
        }

        private bool EmptyTextBoxes()
        {
            if(string.IsNullOrEmpty(textBoxUserName.Texts) || string.IsNullOrEmpty(textBoxPassword.Texts))
            {
                return true;
            }
            return false;
        }

        private void textBoxUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if  (e.KeyCode == Keys.Enter) 
            {
                buttonLogIn.Focus();
                Login();
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLogIn.Focus();
                Login();
            }
        }

        private void textBoxUserName_Enter(object sender, EventArgs e)
        {
            if (labelWarning.Text == "" || labelWarning.Text == "Something is missing!")
            {
                return;
            }
            this.ResetTexts();
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (labelWarning.Text == "" || labelWarning.Text == "Something is missing!")
            {
                return;
            }
            this.ResetTexts();
        }

        private void timerClosing_Tick(object sender, EventArgs e)
        {
                timerClosing.Stop();
                var frm = new FormApplication();
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormClosing += delegate { this.Show(); this.Opacity = 1; };
                this.ResetTexts();
                frm.Show();
                this.Hide();
        }
    }
}
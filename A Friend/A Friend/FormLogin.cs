using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace A_Friend
{
    public partial class FormLogin : Form
    {
        clsResize _form_resize;

        public FormLogin()
        {
            InitializeComponent();
            labelWarning.Text = "";
            
            _form_resize = new clsResize(this); //I put this after the initialize event to be sure that all controls are initialized properly

            this.Load += new EventHandler(_Load); //This will be called after the initialization // form_load
            this.Resize += new EventHandler(_Resize); //form_resize
        }

        private void _Load(object sender, EventArgs e)
        {
            _form_resize._get_initial_size();
        }

        private void _Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                _form_resize._resize_minimize();
            } else _form_resize._resize();
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
            if (textBoxUserName.Texts.Trim() == "admin" && textBoxPassword.Texts.Trim() == "123456")
            {
                return true;
            }
            return false;
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
            if (this.Opacity > 0)
            {
                if (this.Opacity > 0.995)
                {
                    this.Opacity -= 0.01;
                }
                else
                this.Opacity -= 0.995;
            }
            else
            {
                timerClosing.Stop();
                var frm = new FormApp();
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormClosing += delegate { this.Show(); this.Opacity = 1; };
                this.ResetTexts();
                frm.Show();
                this.Hide();
            }
        }
    }
}

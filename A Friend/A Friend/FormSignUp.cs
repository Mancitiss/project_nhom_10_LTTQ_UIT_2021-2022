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
    public partial class FormSignUp : Form
    {
        public FormSignUp()
        {
            InitializeComponent();
            labelWarning.Text = "";
        }
       
        private void ResetTexts()
        {
            textBoxUserName.Texts = "";
            textBoxPassword.Texts = "";
            textBoxConfirmPassword.Texts = "";
            labelWarning.Text = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            this.Close();
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            SignUp();
        }

        private void SignUp()
        {
            if (ExistUserName())
            {
                labelWarning.Text = "That user name already exists";
                return;
            }
            if (EmptyTextBoxes())
            {
                labelWarning.Text = "Something is missing?";
                return;
            }
            if (!MatchPasswords())
            {
                labelWarning.Text = "Those passwords doesn't match";
                return;
            }
            labelWarning.Text = "You have sign up successfully".ToUpper();
            labelWarning.ForeColor = Color.FromArgb(143, 228, 185);
            timerClosing.Start();
        }

        private bool EmptyTextBoxes()
        {
            if (string.IsNullOrEmpty(textBoxUserName.Texts) || string.IsNullOrEmpty(textBoxPassword.Texts) || string.IsNullOrEmpty(textBoxConfirmPassword.Texts))
            {
                return true;
            }
            return false;
        }

        private bool ExistUserName()
        {
            if (textBoxUserName.Texts == "admin")
            {
                return true;
            }
            return false;
        }

        private bool MatchPasswords()
        {
            if (textBoxPassword.Texts == textBoxConfirmPassword.Texts)
            {
                return true;
            }
            return false;
        }

        private void textBoxUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSignUp.Focus();
                SignUp();
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSignUp.Focus();
                SignUp();
            }
        }

        private void textBoxConfirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSignUp.Focus();
                SignUp();
            }
        }

        private void textBoxUserName_Enter(object sender, EventArgs e)
        {
            if (labelWarning.Text == "" || labelWarning.Text == "Something is missing?")
            {
                return;
            }
            this.ResetTexts();
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (labelWarning.Text == "" || labelWarning.Text == "Something is missing?")
            {
                return;
            }
            this.ResetTexts();
        }

        private void textBoxConfirmPassword_Enter(object sender, EventArgs e)
        {
            if (labelWarning.Text == "" || labelWarning.Text == "Something is missing?")
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
                this.Close();
            }
        }
    }
}

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
using System.Runtime.InteropServices;

namespace A_Friend
{
    public partial class FormSignUp : Form
    {
        //clsResize _form_resize;
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

        public FormSignUp()
        {
            InitializeComponent();
            labelWarning.Text = "";
            this.MouseDown += (sender, e) => Form1_MouseDown(sender, e);
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
            this.Close();
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            SignUp();
        }

        private void SignUp()
        {
            
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
            if (ExistUserName())
            {
                labelWarning.Text = "That user name already exists";
                return;
            }
            labelWarning.Text = "You have signed up successfully".ToUpper();
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
            return !AFriendClient.Signed_up(textBoxUserName.Texts, textBoxPassword.Texts);
            
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

        private void textBoxConfirmPassword_Enter(object sender, EventArgs e)
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
            this.Close();
        }
    }
}

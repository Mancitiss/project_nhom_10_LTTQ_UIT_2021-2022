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
    }
}

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
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            panelPassword.Hide();
            panelUsername.Hide();
        }

        private void customButtonClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void customButtonUsername_Click(object sender, EventArgs e)
        {
            panelUsername.Show();

        }

        private void customButtonChange1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(customTextBoxUsername.Text))
                MessageBox.Show("Please enter new username!", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                //
                panelUsername.Hide();
            }
        }

        private void customButtonPassword_Click(object sender, EventArgs e)
        {
            panelPassword.Show();
        }

        private void customButtonChange2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(customTextBoxPassword.Texts) || string.IsNullOrEmpty(customTextBoxCPassord.Texts) || string.IsNullOrEmpty(customTextBoxNPassword.Texts))
                MessageBox.Show("Please enter your password!", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                //
                panelPassword.Hide();
            }
        }

        private void customButtonAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Custom Files|*.pjp;*.jpg;*.pjpeg;*.jpeg;*.jfif;*.png";
            /*if(ofd.ShowDialog() == DialogResult.OK)
            {
               
            }*/
        }

        private void customButtonClose1_Click(object sender, EventArgs e)
        {
            panelUsername.Hide();
        }

        private void customButtonClose2_Click(object sender, EventArgs e)
        {
            panelPassword.Hide();
        }
    }
}

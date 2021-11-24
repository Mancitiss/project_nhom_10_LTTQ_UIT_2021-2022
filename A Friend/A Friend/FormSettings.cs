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
            this.labelUsername.Text = AFriendClient.user.name;
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
            if (String.IsNullOrEmpty(customTextBoxUsername.Texts.Trim()))
                MessageBox.Show("Please enter new username!", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            panelPassword.Show();
        }

        private void customButtonChange2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(customTextBoxPassword.Texts) || string.IsNullOrEmpty(customTextBoxCPassord.Texts) || string.IsNullOrEmpty(customTextBoxNPassword.Texts))
                MessageBox.Show("Please enter your password!", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                if (customTextBoxCPassord.Texts.Equals(customTextBoxNPassword.Texts))
                {
                    AFriendClient.client.Send(Encoding.Unicode.GetBytes("4269" + AFriendClient.data_with_byte(customTextBoxPassword.Texts) + AFriendClient.data_with_byte(customTextBoxCPassord.Texts)));
                }
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

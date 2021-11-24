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

        public FormAddContact()
        {
            InitializeComponent();
            labelWarning.Text = "";
            txtNewUser.RemovePlaceHolder();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (txtNewUser.Texts == "")
                labelWarning.Text = "Please enter a username";
            else
            {
                string data = txtNewUser.Texts;
                string databyte = Encoding.Unicode.GetByteCount(data).ToString();
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("0610" + databyte.Length.ToString().PadLeft(2, '0') + databyte + data));
            }
            this.Hide();
        }

        private void FormAddContact_Shown(object sender, EventArgs e)
        {
            txtNewUser.Focus();
        }

        private void txtNewUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonAdd.PerformClick();
            }
        }
    }
}

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
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (txtNewUser.Text == "")
                labelWarning.Text = "Please enter a username";
            else
            {
                if (!UsernameCheck())
                {
                    labelWarning.Text = "This user does not exist";
                }
                else
                {
                    if (isFriend())
                        labelWarning.Text = "This user is already added";
                    else
                    {
                        labelWarning.Text = "Added successfully";
                        //Add user to list friends in database
                    }
                }
            }
            
        }
        private bool UsernameCheck()
        {
            //Check new username in list users
            return true;
        }
        private bool isFriend()
        {
            //Check new username in list friends
            return true;
        }
    }
}

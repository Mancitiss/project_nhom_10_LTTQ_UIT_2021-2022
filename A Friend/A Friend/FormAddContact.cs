﻿using System;
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
                AFriendClient.client.Send(Encoding.Unicode.GetBytes("0610" + txtNewUser.Texts));
            }
        }
    }
}

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
    public partial class FormGetStarted : Form
    {
        public FormGetStarted()
        {
            InitializeComponent();
        }

        public Color TopColor
        {
            get
            {
                return panel1.BackColor;
            }
            set
            {
                panel1.BackColor = value;
            }
        }

        private void customButton1_Click(object sender, EventArgs e)
        {
            if (this.Parent.Parent != null && this.Parent.Parent is FormApplication)
            {
                (this.Parent.Parent as FormApplication).ButtonAdd_Click_1(sender, e);
            }
        }
    }
}

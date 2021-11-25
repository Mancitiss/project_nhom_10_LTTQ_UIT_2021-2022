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
    public partial class FormRemoveContactWarning : Form
    {
        public FormRemoveContactWarning()
        {
            InitializeComponent();
        }

        private void customButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void customButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormRemoveContactWarning_Paint(object sender, PaintEventArgs e)
        {
            using(Pen pen = new Pen(Color.Red,2))
            {
                e.Graphics.DrawLines(pen, new Point[5] { new Point(0, 0), new Point(0, this.Height - 2), new Point(this.Width - 2, this.Height - 2), new Point(this.Width - 2, 0), new Point(0,0)});
            }
        }

        private void FormRemoveContactWarning_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}

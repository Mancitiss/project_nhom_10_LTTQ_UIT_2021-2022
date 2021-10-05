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
    public partial class FormApp : Form
    {
        clsResize _form_resize;

        public FormApp()
        {
            InitializeComponent();
            //human-interference

            this.buttonExit = new A_Friend.CustomControls.CustomButton();
            this.buttonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(54)))), ((int)(((byte)(41)))));
            this.buttonExit.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonExit.BorderRadius = 20;
            this.buttonExit.BorderSize = 0;
            this.buttonExit.FlatAppearance.BorderSize = 0;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.ForeColor = System.Drawing.Color.White;
            this.buttonExit.Location = new System.Drawing.Point((int)this.Size.Width / 4, (int)this.Size.Height / 4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size((int)this.Size.Width / 2, (int)this.Size.Height / 2);
            this.buttonExit.TabIndex = 5;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            this.Controls.Add(this.buttonExit);

            _form_resize = new clsResize(this); //I put this after the initialize event to be sure that all controls are initialized properly

            this.Load += new EventHandler(_Load); //This will be called after the initialization // form_load
            this.Resize += new EventHandler(_Resize); //form_resize
            
        }

        private void _Load(object sender, EventArgs e)
        {
            _form_resize._get_initial_size();
        }

        private void _Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                _form_resize._resize_minimize();
            }
            else _form_resize._resize();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //end human-interference
        private void FormApp_Load(object sender, EventArgs e)
        {
        }
    }
}

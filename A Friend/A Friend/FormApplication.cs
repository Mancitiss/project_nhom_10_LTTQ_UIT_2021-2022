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
        public FormApplication()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        public void AddMessage(string message, bool stacktoleft)
        {
            panelChat.SuspendLayout();
            var chatItem = new CustomControls.ChatItem2(message, stacktoleft);
            chatItem.Name = "chatItem" + panelChat.Controls.Count;
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panelChat.BackColor;
            panelChat.Controls.Add(chatItem);
            chatItem.BringToFront();

            chatItem.ResizeBubbles();
            //chatItem.ResizeBubbles((int)(panelChat.Width * 0.6));
            panelChat.ResumeLayout();

            panelChat.ScrollControlIntoView(chatItem);
            
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts))
            {
                AddMessage(textboxWriting.Texts, true);
                textboxWriting.Texts = "";
            }
        }

        private void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textboxWriting.Texts))
                {
                    AddMessage(textboxWriting.Texts, false); 
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                }
            }
        }

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(112, 155, 170), 5);
            e.Graphics.DrawLine(pen, panelTopRight.Left + 5, panelTopRight.Bottom, panelTopRight.Right - 5, panelTopRight.Bottom);
        }

        private void panelTopRight_Resize(object sender, EventArgs e)
        {
            panelTopRight.Invalidate();
        }

        private void FormApplication_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //protected override void OnResizeBegin(EventArgs e)
        //{
        //    SuspendLayout();
        //    base.OnResizeBegin(e);
        //}
        //protected override void OnResizeEnd(EventArgs e)
        //{
        //    ResumeLayout();
        //    base.OnResizeEnd(e);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace A_Friend.CustomControls
{
    public partial class ChatItem : UserControl
    {
        public ChatItem(string text, bool stacktoleft)
        {
            InitializeComponent();

            textBoxBody.Text = text;
            buttonCopy.Enabled = false;
            buttonRemove.Enabled = false;
            buttonCopy.Visible = false;
            buttonRemove.Visible = false;

            if (stacktoleft)
            {
                textBoxBody.BackColor = Color.FromArgb(100, 100, 165);
                panelBody.BackColor = Color.FromArgb(100, 100, 165);
            }
            else
            {
                panelBody.Dock = DockStyle.Right;
                panelButton.Dock = DockStyle.Right;
            }

            panelBody.MouseEnter += delegate { ShowButtons(); };
            textBoxBody.MouseEnter += delegate { ShowButtons(); };
            panelButton.MouseEnter += delegate { ShowButtons(); };
            this.MouseEnter += delegate { ShowButtons(); };
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            buttonCopy.Location = new Point(buttonCopy.Left, (int)(buttonCopy.Parent.Height / 2 - buttonCopy.Height / 2));
            buttonRemove.Location = new Point(buttonRemove.Left, (int)(buttonRemove.Parent.Height / 2 - buttonRemove.Height / 2));
        }

        public void ResizeBubbles(int maxwidth)
        {
            string body = textBoxBody.Text.Trim();
            int fontheight = textBoxBody.Font.Height;
            var gfx = this.CreateGraphics();
            int lines = 1;
            double stringwidth = gfx.MeasureString(body, textBoxBody.Font).Width;

            if (stringwidth < maxwidth + panelBody.Width - textBoxBody.Width)
            {
                panelBody.Width = (int)(stringwidth + panelBody.Width - textBoxBody.Width);
                this.Height += (lines * fontheight) - textBoxBody.Height;
                return;
            }
            else
            {
                lines = 0;
                panelBody.Width = maxwidth + panelBody.Width - textBoxBody.Width;
                string noescapebody = body.Replace("\r\n", string.Empty).Replace("\r\n", string.Empty);
                stringwidth = gfx.MeasureString(noescapebody, textBoxBody.Font).Width;

                while (stringwidth > 0)
                {
                    stringwidth -= panelBody.Width;
                    lines++;
                }
            }
            if (body.Contains("\n"))
            {
                while (body.Contains("\r\n"))
                {
                    body = body.Remove(body.IndexOf("\r\n"), "\r\n".Length);
                    lines++;
                }
                while (body.Contains("\n"))
                {
                    body = body.Remove(body.IndexOf("\n"), "\n".Length);
                    lines++;
                }
            }

            this.Height += (lines * fontheight) - textBoxBody.Height + 5;
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxBody.Text);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.Parent.Controls.Count == 0)
                return;
            this.Parent.Controls.Remove(this);
        }

        public void HideButtons()
        {
            buttonCopy.Enabled = false;
            buttonRemove.Enabled = false;
            buttonCopy.Visible = false;
            buttonRemove.Visible = false;
        }

        public void ShowButtons()
        {
            buttonCopy.Enabled = true;
            buttonRemove.Enabled = true;
            buttonCopy.Visible = true;
            buttonRemove.Visible = true;
            buttonCopy.Location = new Point(buttonCopy.Left, (int)(panelButton.Height / 2 - buttonCopy.Height / 2));
            buttonRemove.Location = new Point(buttonRemove.Left, (int)(panelButton.Height / 2 - buttonRemove.Height / 2));

            foreach (var control in this.Parent.Controls)
            {
                if (control is ChatItem && control != this)
                {
                    (control as ChatItem).HideButtons(); ;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HideButtons();
        }
    }
}

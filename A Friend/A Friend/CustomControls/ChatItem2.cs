using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace A_Friend.CustomControls
{
    public partial class ChatItem2 : UserControl
    {
        public ChatItem2()
        {
            InitializeComponent();
        }
        public ChatItem2(string text, bool stacktoleft)
        {
            InitializeComponent();

            labelBody.Text = text;
            StackToLeft = stacktoleft;
            labelBody.MaximumSize = new Size(this.Width - 200 - 2 * labelBody.Left, int.MaxValue);
            panelBody.MaximumSize = new Size(this.Width - 200, int.MaxValue);
            buttonCopy.BackColor = this.BackColor;
            buttonRemove.BackColor = this.BackColor;
            buttonCopy.Enabled = false;
            buttonRemove.Enabled = false;
            buttonCopy.Visible = false;
            buttonRemove.Visible = false;

            if (stacktoleft)
            {
                labelBody.BackColor = Color.FromArgb(100, 100, 165);
                panelBody.BackColor = Color.FromArgb(100, 100, 165);
            }
            else
            {
                panelBody.Dock = DockStyle.Right;
                panelButton.Dock = DockStyle.Right;
            }

            panelBody.MouseEnter += delegate { ShowButtons(); };
            labelBody.MouseEnter += delegate { ShowButtons(); };
            panelButton.MouseEnter += delegate { ShowButtons(); };
            this.MouseEnter += delegate { ShowButtons(); };
            this.MouseLeave += delegate { HideButtons(); };
        }

        public string Texts
        {
            get
            {
                return labelBody.Text;
            }
        }

        public bool StackToLeft { get; set; }

        public Color TextColor
        {
            get { return labelBody.ForeColor; }
            set { labelBody.ForeColor = value; }
        }

        public Color BackGroundColor
        {
            get { return panelBody.BackColor; }
            set
            {
                labelBody.BackColor = value;
                panelBody.BackColor = value;
            }
        }

        public void ResizeBubbles()
        {
            SuspendLayout();
            int maxwidth = this.Width - 200;
            labelBody.MaximumSize = new Size(maxwidth - 2 * labelBody.Left, int.MaxValue);
            panelBody.MaximumSize = new Size(maxwidth, int.MaxValue);

            var size = TextRenderer.MeasureText("qwertyuiopasdfghjklzxcbnm1234567890", this.Font);
            if (labelBody.Width <= maxwidth - 2 * labelBody.Left && labelBody.Height <= size.Height)
            {
                panelBody.Width = labelBody.Width + 2 * labelBody.Left;          
            }
            panelBody.Width = labelBody.Width + 2 * labelBody.Left;          
            this.Height = labelBody.Height + 2 * labelBody.Top + 2 * panelBody.Top;
            ResumeLayout();
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
                if (control is ChatItem2 && control != this)
                {
                    (control as ChatItem2).HideButtons(); ;
                }
            }
        }

        public void HideButtons()
        {
            buttonCopy.Enabled = false;
            buttonRemove.Enabled = false;
            buttonCopy.Visible = false;
            buttonRemove.Visible = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ResizeBubbles();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() =>
            {
                Clipboard.SetText(labelBody.Text);
            }));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            //if (this.Parent.Parent is PanelChat)
            //{
            //    (this.Parent.Parent as PanelChat).RemoveChatItem(this);
            //}
        }
    }
}

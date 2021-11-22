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
using System.Threading;


namespace A_Friend.CustomControls
{
    public partial class ChatItem : UserControl
    {
        private bool showDetail = true;
        //public Message message;
        public MessageObject messageObject;
        //public ChatItem(Message message)
        //{
        //    InitializeComponent();

        //    this.DoubleBuffered = true;

        //    this.message = message;
        //    labelBody.Text = message.text;
        //    buttonCopy.Enabled = false;
        //    buttonRemove.Enabled = false;
        //    buttonCopy.Visible = false;
        //    buttonRemove.Visible = false;

        //    if (string.IsNullOrEmpty(message.author))
        //    {
        //        panelBody.Dock = DockStyle.Right;
        //        panelButton.Dock = DockStyle.Right;
        //        labelAuthor.Dock = DockStyle.Right;
        //    }
        //    else
        //    {
        //        BackgroundColor = Color.FromArgb(100, 100, 165);
        //    }

        //    if (message.time > DateTime.Today)
        //    {
        //        if (string.IsNullOrEmpty(message.author))
        //        {
        //            labelAuthor.Text = $"{message.time.ToShortTimeString()}";
        //        }
        //        else
        //        {
        //            labelAuthor.Text = $"{message.author}, {message.time.ToShortTimeString()}";
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(message.author))
        //        {
        //            labelAuthor.Text = $"{message.time.ToShortDateString()}";
        //        }
        //        else
        //        {
        //            labelAuthor.Text = $"{message.author}, {message.time.ToLongDateString()}";
        //        }
        //    }

        //    this.MouseEnter += delegate { ShowButtons(); };
        //    foreach (Control control in this.Controls)
        //    {
        //        control.MouseEnter += delegate { ShowButtons(); };
        //    }

        //    foreach (Control control in panelTop.Controls)
        //    {
        //        control.MouseEnter += delegate { ShowButtons(); };
        //    }

        //    labelAuthor.MouseEnter += delegate { ShowButtons(); };

        //    labelBody.Click += delegate
        //    {
        //        ShowDetail = !showDetail;
        //        if (this.Parent.Parent is PanelChat)
        //        {
        //            if (this != (this.Parent.Parent as PanelChat).CurrentChatItem)
        //                (this.Parent.Parent as PanelChat).CurrentChatItem = this;
        //        }
        //    };
        //}

        public ChatItem(MessageObject messageObject)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            
            this.messageObject = messageObject;
            labelBody.Text = messageObject.message;
            buttonCopy.Enabled = false;
            buttonRemove.Enabled = false;
            buttonCopy.Visible = false;
            buttonRemove.Visible = false;

            if (IsMyMessage())
            {
                panelBody.Dock = DockStyle.Right;
                panelButton.Dock = DockStyle.Right;
                labelAuthor.Dock = DockStyle.Right;
            }
            else
            {
                BackgroundColor = Color.FromArgb(100, 100, 165);
            }

            this.MouseEnter += delegate { ShowButtons(); };
            foreach (Control control in this.Controls)
            {
                control.MouseEnter += delegate { ShowButtons(); };
            }

            foreach (Control control in panelTop.Controls)
            {
                control.MouseEnter += delegate { ShowButtons(); };
            }

            labelAuthor.MouseEnter += delegate { ShowButtons(); };
        }

        public bool IsMyMessage()
        {
            if (messageObject.sender == false)
            {
                if (messageObject.id1 == FormApplication.currentID)
                {
                    return true;
                }
                return false;
            }
            if (messageObject.id2 == FormApplication.currentID)
            {
                return true;
            }
            return false;
        }

        public void UpdateDateTime()
        {
            if (messageObject.timesent > DateTime.Today)
            {
                if (IsMyMessage())
                {
                    labelAuthor.Text = $"{messageObject.timesent.ToShortTimeString()}";
                }
                else
                {
                    if (this.Parent.Parent != null && this.Parent.Parent is PanelChat)
                    {
                        string author = (this.Parent.Parent as PanelChat).account.name;
                        labelAuthor.Text = $"{author}, {messageObject.timesent.ToShortTimeString()}";
                    }
                }
            }
            else
            {
                if (IsMyMessage())
                {
                    labelAuthor.Text = $"{messageObject.timesent.ToShortDateString()}";
                }
                else
                {
                    if (this.Parent.Parent != null && this.Parent.Parent is PanelChat)
                    {
                        string author = (this.Parent.Parent as PanelChat).account.name;
                        labelAuthor.Text = $"{author}, {messageObject.timesent.ToShortDateString()}";
                    }
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return panelBody.BackColor;
            }

            set
            {
                labelBody.BackColor= value;
                panelBody.BackColor = value;
            }
        }

        public bool ShowDetail
        {
            get
            {
                return showDetail;
            }
            set
            {
                showDetail = value;
                panelBottom.Visible = value;
                if (value)
                {
                    this.Height = 5 + panelTop.Height + panelBottom.Height;
                }
                else
                {
                    this.Height = 5 + panelTop.Height;
                }
            } 
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ResizeBubbles();
            buttonCopy.Location = new Point(buttonCopy.Left, (int)(buttonCopy.Parent.Height / 2 - buttonCopy.Height / 2));
            buttonRemove.Location = new Point(buttonRemove.Left, (int)(buttonRemove.Parent.Height / 2 - buttonRemove.Height / 2));
        }

        public void ResizeBubbles()
        {
            SuspendLayout();
            int maxwidth = this.Width - 200;
            labelBody.MaximumSize = new Size(maxwidth - 2 * labelBody.Left, int.MaxValue);
            //panelBody.MaximumSize = new Size(maxwidth, int.MaxValue);

            var size = TextRenderer.MeasureText("qwertyuiopasdfghjklzxcbnm1234567890", labelBody.Font);
            if (labelBody.Width <= maxwidth - 2 * labelBody.Left && labelBody.Height <= size.Height)
            {
                panelBody.Width = labelBody.Width + 2 * labelBody.Left;
            }
            panelBody.Width = labelBody.Width + 2 * labelBody.Left;
            panelTop.Height = labelBody.Height + 2 * labelBody.Top;

            if (showDetail)
            {
                this.Height = 5 + panelTop.Height + panelBottom.Height;
            }
            else
            {
                this.Height = 5 + panelTop.Height;
            }
            panelBottom.Location = new Point(panelTop.Left, this.Height - panelBottom.Height);

            ResumeLayout();
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
            if (this.Parent.Parent is PanelChat)
            {
                (this.Parent.Parent as PanelChat).RemoveMessage(this);
            }
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
            if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                return;
            }
            base.OnMouseLeave(e);
            HideButtons();
        }

        private void ChatItem_Load(object sender, EventArgs e)
        {
            ResizeBubbles();
        }
    }
}

﻿using System;
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
using System.IO;
using System.Drawing.Imaging;

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

        public string ImageToString(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            Image im = Image.FromFile(path);
            MemoryStream ms = new MemoryStream();
            im.Save(ms, im.RawFormat);
            byte[] array = ms.ToArray();
            return Convert.ToBase64String(array);
        }
        public Image StringToImage(string imageString)
        {

            if (imageString == null)
                throw new ArgumentNullException("imageString");
            byte[] array = Convert.FromBase64String(imageString);
            Image image = Image.FromStream(new MemoryStream(array));
            return image;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        internal Image image;

        public ChatItem(MessageObject messageObject)
        {
            InitializeComponent();

            this.messageObject = messageObject;
            DoubleBuffered = true;

            if (this.messageObject.type == 0)
                labelBody.Text = messageObject.message;
            else if (this.messageObject.type == 1)
            {
                image = StringToImage(this.messageObject.message);
                this.Controls.Remove(labelBody);
                this.AutoSize = false;
                this.Size = new Size(this.Width - 200 -2*this.Left, image.Height);
                this.image = ResizeImage(image, this.Width - 200 - 2 * this.Left, image.Height);
            }
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
        }

        public long ID
        {
            get
            {
                return this.messageObject.messagenumber;
            }
        }
        public bool IsMyMessage()
        {
            if (messageObject.sender == false)
            {
                if (messageObject.id1 == AFriendClient.user.id)
                {
                    return true;
                }
                return false;
            }
            if (messageObject.id2 == AFriendClient.user.id)
            {
                return true;
            }
            return false;
        }

        public void UpdateDateTime()
        {
            if (IsMyMessage())
            {
                if (messageObject.timesent.ToLocalTime() < DateTime.Today)
                {
                    labelAuthor.Text = $"{messageObject.timesent.ToLocalTime().ToString("dd/MM/yyyy") + " - " + messageObject.timesent.ToLocalTime().ToShortTimeString()}";
                    //labelAuthor.Text = $"{messageObject.timesent.ToLocalTime().ToShortTimeString()}";
                }
                else
                {
                    labelAuthor.Text = $"{messageObject.timesent.ToLocalTime().ToShortTimeString()}";
                }
            }
            else
            {
                if (this.Parent.Parent != null && this.Parent.Parent is PanelChat)
                {
                    string author = (this.Parent.Parent as PanelChat).account.name;
                    if (messageObject.timesent < DateTime.Today)
                    {
                        labelAuthor.Text = $"{author}, {messageObject.timesent.ToLocalTime().ToString("dd/MM/yyyy") + " - " + messageObject.timesent.ToLocalTime().ToShortTimeString()}";
                    }
                    else
                    {
                        labelAuthor.Text = $"{author}, {messageObject.timesent.ToLocalTime().ToShortTimeString()}";
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
                labelBody.BackColor = value;
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

            //panelBody.MaximumSize = new Size(maxwidth, int.MaxValue);

            int maxwidth = this.Width - 200;
            labelBody.MaximumSize = new Size(maxwidth - 2 * labelBody.Left, int.MaxValue);
            SuspendLayout();
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
                (this.Parent.Parent as PanelChat).RemoveMessage(this.ID);
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
            if (!ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                base.OnMouseEnter(e);
            }
        }
        
        private void ChatItem_Load(object sender, EventArgs e)
        {
            ResizeBubbles();
        }

        private void ChatItem_MouseEnter(object sender, EventArgs e)
        {
            if (ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                ShowButtons();
            }
        }

        private void ChatItem_MouseLeave(object sender, EventArgs e)
        {
            if (!ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                HideButtons();
            }
        }

        private void labelBody_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}

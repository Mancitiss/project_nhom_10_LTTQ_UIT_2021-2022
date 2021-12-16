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
using System.IO;
using System.Drawing.Imaging;

namespace A_Friend.CustomControls
{
    public partial class ChatItem : UserControl
    {
        private bool showDetail = true;
        public MessageObject messageObject;

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

        private static void open_image(Image image)
        {
            string tempFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            (new Bitmap(image)).Save(tempFile, ImageFormat.Png);
            var process = System.Diagnostics.Process.Start(tempFile);
            //ThreadPool.QueueUserWorkItem((state) => wait_for_close(ref process, tempFile));
        }

        /*
        private static void wait_for_close(ref System.Diagnostics.Process p, String path)
        {
            try
            {
                try
                {
                    p.WaitForExit();
                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //object reference lost
                }
                System.IO.File.Delete(path);
            }catch(Exception Iknow)
            {
                Console.WriteLine(Iknow.ToString());
            }
        }
        */

        internal Image image;

        public ChatItem(MessageObject messageObject)
        {
            InitializeComponent();

            this.messageObject = messageObject;
            labelAuthor.Font = ApplicationFont.GetFont(labelAuthor.Font.Size);
            DoubleBuffered = true;

            if (this.messageObject.type == 0)
            {
                labelBody.Text = messageObject.message;
                labelBody.Font = ApplicationFont.GetFont(labelBody.Font.Size);
            }
            else if (this.messageObject.type == 1)
            {
                image = StringToImage(this.messageObject.message);
                panelBody.Controls.Remove(labelBody);
                labelBody.Dispose();
                panelBody.DoubleClick += delegate
                {
                    //code to open image in photo viewer 
                    ThreadPool.QueueUserWorkItem((state) => open_image(this.image));
                };
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
                BackgroundColor = Color.FromArgb(215, 244, 241);
                if (this.messageObject.type == 0)
                {
                    labelBody.ForeColor = SystemColors.ControlText;
                }
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
                if (messageObject.type == 0)
                {
                    labelBody.BackColor = value;
                }
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
            //panelBody.MaximumSize = new SizeMaxValue);
            if (messageObject != null && messageObject.type == 0)
            {
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
            else if (messageObject != null && messageObject.type == 1)
            {
                int maxwitdh = this.Width - 200;
                if (image.Width > maxwitdh)
                { 
                var img = ResizeImage(image, maxwitdh, maxwitdh * image.Height / image.Width);
                panelBody.BackgroundImage = img;
                }
                else if (panelBody.BackgroundImage != image)
                {
                    panelBody.BackgroundImage = image;
                }
                panelTop.Height = panelBody.BackgroundImage.Height;
                panelBody.Width = panelBody.BackgroundImage.Width;
                this.Height = 5 + panelTop.Height + panelBottom.Height;
                panelBottom.Location = new Point(panelTop.Left, this.Height - panelBottom.Height);
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() =>
            {
                if (messageObject.type == 0)
                { 
                    Clipboard.SetText(labelBody.Text);
                }
                else if (messageObject.type == 1)
                {
                    Clipboard.SetImage(image);
                }
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

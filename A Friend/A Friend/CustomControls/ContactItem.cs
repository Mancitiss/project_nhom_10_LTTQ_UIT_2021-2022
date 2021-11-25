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
    public partial class ContactItem : UserControl
    {
        Account account;
        byte state = 0;
        string id;
        Color mouseOnColor = Color.FromArgb(65, 165, 238);
        Color stateColor = Color.Gainsboro;
        int borderSize = 40;
        bool isMouseOn = false;
        bool unread = false;

        public ContactItem()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public ContactItem(Account account)
        {
            InitializeComponent();
            this.account = account;
            this.DoubleBuffered = true;
            this.Name = "contacItem_" + account.id;
            this.FriendName = account.name;
            this.id = account.id;
            State = account.state;
        }

        public Color MouseOnColor { get => mouseOnColor; set => mouseOnColor = value; }

        public ContactItem(string name, string lastmessage, bool unread)
        {
            InitializeComponent();
            FriendName = name;
            LastMessage = lastmessage;
            Unread = unread;
        }

        public string ID
        {
            get => id;
        }

        public bool Unread
        {
            get { return unread; }
            set
            {
                unread = value;
                if (unread)
                {
                    labelLastMessage.ForeColor = Color.FromArgb(65, 165, 238);
                }
                else
                {
                    labelLastMessage.ForeColor = Color.DarkGray;
                }
            }
        }

        public string FriendName
        {
            get
            {
                return account.name;
            }
            set
            {
                if (value.Length <= 18)
                {
                    labelName.Text = value;
                }
                else
                {
                    labelName.Text = value.Substring(0, 15) + "...";
                }
            }
        }
        public string LastMessage
        {
            set
            {
                if (value.Trim().Length <= 23)
                {
                    labelLastMessage.Text = value.Trim().Replace('\n', '-');
                }
                else
                {
                    labelLastMessage.Text = value.Trim().Replace('\n', '-').Substring(0,20) + "...";
                }
            }
        }

        public byte State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value) {
                    state = value;

                    if (state == 0)
                    {
                        stateColor = Color.Gainsboro;
                    }
                    else if (state == 1)
                    {
                        stateColor = Color.SpringGreen;
                    }
                    else
                    {
                        stateColor = Color.Red;
                    }
                    this.Invalidate();
                }
            }
        }

        public void TurnActive()
        {
            friendPicture.BorderColor = Color.FromArgb(58, 206, 58);
            friendPicture.BorderColor2 = Color.FromArgb(180, 236, 180);
        }

        public void TurnAway()
        {
            friendPicture.BorderColor = Color.FromArgb(255, 32, 21);
            friendPicture.BorderColor2 = Color.FromArgb(255, 178, 174);
        }

        public void TurnOffline()
        {
            friendPicture.BorderColor = Color.Gray;
            friendPicture.BorderColor2 = Color.Gray;
        }

        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            return path;
        }

        private void ContactItem_Paint(object sender, PaintEventArgs e)
        {
            if (isMouseOn)
            {
                using (var path = GetFigurePath(this.ClientRectangle, borderSize))
                using (Brush brush = new SolidBrush(mouseOnColor))
                {
                    this.Region = new Region(path);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    labelLastMessage.BackColor = mouseOnColor;
                    labelName.BackColor = mouseOnColor;
                    e.Graphics.FillPath(brush, path);
                }
            }
            else
            {
                this.Region = new Region(this.ClientRectangle); 

                labelLastMessage.BackColor = this.BackColor;
                labelName.BackColor= this.BackColor;    
                e.Graphics.DrawLine(new Pen(Color.SkyBlue, 2), 30, this.Height - 2, this.Width - 30, this.Height - 2);
            }

            using(Pen pen = new Pen(stateColor, 2))
            using(var path = GetFigurePath(new Rectangle(friendPicture.Left - 1, friendPicture.Top - 1, friendPicture.Width + 2, friendPicture.Width + 2), friendPicture.Width + 2))
            {
                e.Graphics.SmoothingMode= SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void labelName_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void friendPicture_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void labelName_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void labelLastMessage_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void ContactItem_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        //protected override void OnMouseLeave(EventArgs e)
        //{

        //    if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
        //        return;
        //    else
        //    {
        //        base.OnMouseLeave(e);
        //        isMouseOn = false;
        //        this.Invalidate();
        //    }
        //}
        //protected override void OnMouseEnter(EventArgs e)
        //{

        //    if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
        //        return;
        //    else
        //    {
        //        base.OnMouseLeave(e);
        //        if (!isMouseOn)
        //            this.Invalidate();
        //        isMouseOn = true;
        //    }
        //}
    }
}
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

        public ContactItem()
        {
            InitializeComponent();
        }
        public ContactItem(string name, string lastmessage, bool unread)
        {
            InitializeComponent();
            FriendName = name;
            LastMessage = lastmessage;
            Unread = unread;
        }

        bool unread = false;
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
                return labelName.Text;
            }
            set
            {
                labelName.Text = value;
            }
        }
        public string LastMessage
        {
            set
            {
                if (value.Length <= 25)
                {
                    labelLastMessage.Text = value;
                }
                else
                {
                    labelLastMessage.Text = value.Substring(0, 22) + "...";
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

        private void ContactItem_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.AliceBlue, 2), 20, this.Height - 2, this.Width - 20, this.Height - 2);
        }

        private void labelName_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
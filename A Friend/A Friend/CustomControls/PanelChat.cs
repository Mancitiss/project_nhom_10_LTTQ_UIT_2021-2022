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
using System.IO;
using System.Threading;

namespace A_Friend.CustomControls
{
    public partial class PanelChat : UserControl
    {
        public Account account;
        string id;
        byte state;
        Int64 loadedmessagenumber = 0;

        internal bool is_showing;
        internal int is_form_showing;

        Color stateColor = Color.Gainsboro;
        bool locking = false;
        List<CustomControls.ChatItem> chatItems = new List<ChatItem>();
        Dictionary<long, ChatItem> messages = new Dictionary<long, ChatItem>();
        ChatItem currentChatItem;
        bool currentChatItemShowing;
        public bool isloadingoldmessages = false;

        public delegate void AddMessageItem(MessageObject message);
        public AddMessageItem AddMessageDelegate;

        public delegate void LoadMessageItem(List<MessageObject> messageObjects);
        public LoadMessageItem LoadMessageDelegate;

        internal delegate void RemoveMessageInvoker(long messagenumber);
        internal RemoveMessageInvoker RemoveMessage_Invoke;

        private void Must_initialize()
        {
            InitializeComponent();
            LoadMessageDelegate = new LoadMessageItem(LoadMessage);
            AddMessageDelegate = new AddMessageItem(AddMessage);
            RemoveMessage_Invoke = new RemoveMessageInvoker(RemoveMessage_Passively);
            panel_Chat.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_Chat_MouseWheel);
            this.CreateControl();
            textboxWriting.dynamicMode = true;
            textboxWriting.SetMaximumTextLenght(2021);
        }

        public PanelChat()
        {
            Must_initialize();
        }

        public PanelChat(Account account)
        {
            Must_initialize();
            this.is_form_showing = 0;
            this.is_showing = false;

            labelFriendName.Font = ApplicationFont.GetFont(labelFriendName.Font.Size);
            labelState.Font = ApplicationFont.GetFont(labelState.Font.Size);
            textboxWriting.Font = ApplicationFont.GetFont(textboxWriting.Font.Size);

            this.account = account;
            this.DoubleBuffered = true;
            this.Name = "panelChat_" + account.id;
            labelFriendName.Text = account.name;
            textboxWriting.PlaceholderText = "to " + account.name;
            this.id = account.id;
            State = account.state;
            Console.WriteLine("Handler created");
            Console.WriteLine(this.id);
            panel_Chat.Click += panelTopRight_Click;
        }

        public static string ImageToString(Image im)
        {
            MemoryStream ms = new MemoryStream();
            im.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] array = ms.ToArray();
            return Convert.ToBase64String(array);
        }

        public Image Avatar
        {
            set
            {
                friendPicture.Crop(value);
            }
        }

        private void panel_Chat_MouseWheel(object sender, EventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value == 0 && !locking)
            {
                Int64 num = this.loadedmessagenumber - 1;
                if (num > 1)
                {
                    string datasend = num.ToString();
                    string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                    Console.WriteLine(datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend);
                    AFriendClient.Queue_command(Encoding.Unicode.GetBytes("6475" + this.ID + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                    locking = true;
                    timerChat.Start();
                    panel_Chat.VerticalScroll.Value = 5;
                }
            }
        }

        private void panel_Chat_Scroll(object sender, ScrollEventArgs e)
        {
            if (panel_Chat.VerticalScroll.Value == 0 && !locking)
            {
                Int64 num = this.loadedmessagenumber - 1;
                if (num > 1)
                {
                    string datasend = num.ToString();
                    string datasendbyte = Encoding.Unicode.GetByteCount(datasend).ToString();
                    Console.WriteLine(datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend);
                    AFriendClient.Queue_command(Encoding.Unicode.GetBytes("6475" + this.ID + datasendbyte.Length.ToString().PadLeft(2, '0') + datasendbyte + datasend));
                    locking = true;
                    timerChat.Start();
                    panel_Chat.VerticalScroll.Value = 5;
                }
            }
        }

        public ChatItem CurrentChatItem
        {
            get
            {
                return currentChatItem;
            }
            set
            {
                if (value == currentChatItem) 
                    return;
                Console.WriteLine("set");
                if (currentChatItem != null)
                {
                    currentChatItem.ShowDetail = currentChatItemShowing;
                }
                currentChatItem = value;
                currentChatItemShowing = !value.ShowDetail;
            }
        }

        public string ID
        {
            get { return this.id; }
        }

        public byte State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;

                    if (state == 0)
                    {
                        stateColor = Color.Gainsboro;
                        labelState.Text = "offline";
                        labelState.ForeColor = stateColor;
                        panelTopRight.Invalidate();
                    }
                    else if (state == 1)
                    {
                        stateColor = Color.SpringGreen;
                        labelState.Text = "online";
                        labelState.ForeColor = stateColor;
                        panelTopRight.Invalidate();
                    }
                    else
                    {
                        stateColor = Color.Red;
                        labelState.Text = "away";
                        labelState.ForeColor = stateColor;
                        panelTopRight.Invalidate();
                    }
                    this.Invalidate();
                }
            }
        }

        public DateTime DateTimeOflastMessage
        {
            get
            {
                if (chatItems.Count == 0)
                {
                    return DateTime.Now;
                }
                else
                {
                    return chatItems[chatItems.Count - 1].messageObject.timesent;
                }
            }
        }

        internal void panelTopRight_Click(object sender, EventArgs e)
        {
            //this.OnClick(e);
            textboxWriting.Focus();
        }

        internal void RemoveMessage_Passively(long messagenumber)
        {
            Console.WriteLine("Begin deleting");
            panel_Chat.Controls.Remove(messages[messagenumber]);
            Console.WriteLine("{0},{1}", chatItems.Remove(messages[messagenumber]), messages.Remove(messagenumber));
            Console.WriteLine("deleted: {0}", messagenumber);
        }

        public void RemoveMessage(long messagenumber)
        {
            chatItems.Remove(messages[messagenumber]);
            panel_Chat.Controls.Remove(messages[messagenumber]);
            messages.Remove(messagenumber);
            // code to remove message
            AFriendClient.Queue_command(Encoding.Unicode.GetBytes("2002"+this.ID+AFriendClient.data_with_byte(messagenumber.ToString())));
        }

        public void AddMessage(MessageObject message)
        {
            if (messages.ContainsKey(message.messagenumber))
            {
                Console.WriteLine($"message number {message.messagenumber} existed in this conversation!");
                return;
            }

            panel_Chat.SuspendLayout();
            ChatItem chatItem = new ChatItem(message);
            chatItem.Dock = DockStyle.Top;
            chatItem.BackColor = panel_Chat.BackColor;
            chatItems.Add(chatItem);
            panel_Chat.Controls.Add(chatItem);
            messages.Add(message.messagenumber, chatItem);
            chatItem.UpdateDateTime();
            chatItem.BringToFront();
            panel_Chat.ResumeLayout();
            panel_Chat.ScrollControlIntoView(chatItem);
        }

        private void AddMessageToTop(MessageObject message)
        {
            if (messages.ContainsKey(message.messagenumber))
            {
                Console.WriteLine($"message number {message.messagenumber} existed in this conversation!");
                return;
            }
            //await Task.Delay(5);
            this.loadedmessagenumber = message.messagenumber;
            Console.WriteLine(this.loadedmessagenumber);
            try
            {
                panel_Chat.SuspendLayout();
                ChatItem chatItem = new ChatItem(message);
                chatItem.Dock = DockStyle.Top;
                chatItem.BackColor = panel_Chat.BackColor;
                chatItems.Insert(0, chatItem);
                panel_Chat.Controls.Add(chatItem);
                chatItem.UpdateDateTime();
                messages.Add(message.messagenumber, chatItem);
                panel_Chat.ResumeLayout();
            }catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Finish successfully");
        }

        public void textboxWriting_KeyDown(object sender, KeyEventArgs e)
        {
            textboxWriting.Select();
            if (e.KeyCode == Keys.Enter /*&& !locking*/ && !(e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter))
            {
                if (!string.IsNullOrWhiteSpace(textboxWriting.Texts))
                {
                    AFriendClient.Send_to_id(AFriendClient.stream, FormApplication.currentID, AFriendClient.user.id, textboxWriting.Texts);
                    textboxWriting.Texts = "";
                    textboxWriting.RemovePlaceHolder();
                    Console.WriteLine("Wrote");
                    textboxWriting.Multiline = false;
                }
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control) 
            {
                Thread temp = new Thread(() => { do_shit(sender, e); });
                temp.IsBackground = true;
                temp.SetApartmentState(ApartmentState.STA);
                temp.Start();
                e.Handled = true;
                //e.SuppressKeyPress = true;
            }
        }

        private void do_shit(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Doing");
            /*
            if (Clipboard.ContainsText())
            {
                Console.WriteLine("Text detected");
                this.textboxWriting.Texts += Clipboard.GetText();
                //textboxWriting.
            }
            else */if (Clipboard.ContainsImage())
            {
                Console.WriteLine("Image detected");
                Image img = Clipboard.GetImage();
                if (img != null)
                {
                    string img_string = ImageToString(img);
                    Console.WriteLine("Finished img to string\n");
                    var b = AFriendClient.Combine(Encoding.Unicode.GetBytes("1902" + FormApplication.currentID), Encoding.ASCII.GetBytes(AFriendClient.data_with_ASCII_byte(img_string)));
                    //var b = new Byte[200000];
                    //for (int i = 0; i < 200000; i++) b[i] = 0;
                    Console.WriteLine("before sending nude: {0}", b.Length);
                    AFriendClient.Queue_command(b);
                    //AFriendClient.Ping();
                    Console.WriteLine("Nude sent");
                }
            }
            //else
            //{
            //    Console.WriteLine("IDK");
            //}
        }

        public void buttonSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textboxWriting.Texts) /*&& !locking*/)
            {
                AFriendClient.Send_to_id(AFriendClient.stream, FormApplication.currentID, AFriendClient.user.id, textboxWriting.Texts);
                textboxWriting.Texts = "";
                textboxWriting.RemovePlaceHolder();
                textboxWriting.Multiline = false;
            }
        }

        private void panelTopRight_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(stateColor, 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(pen, friendPicture.Left - 1, friendPicture.Top - 1, friendPicture.Width + 2, friendPicture.Width + 2);
            }

            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, panelTopRight.Height - 1, panelTopRight.Width, panelTopRight.Height - 1);
                e.Graphics.DrawLine(pen, 0, panelTopRight.Height, 0, 0);
            }
        }

        private void panel_Chat_Click(object sender, EventArgs e)
        {
            panel_Chat.Focus();
        }

        public void LoadMessage()
        {
            AFriendClient.Queue_command(Encoding.Unicode.GetBytes("6475"+this.ID+"0120"));
        }

        public void LoadMessage(List<MessageObject> messageObjects)
        {
            isloadingoldmessages = true;
            panel_Chat.SuspendLayout();
            foreach(MessageObject messageObject in messageObjects)
            {
                AddMessageToTop(messageObject);
            }
            panel_Chat.ResumeLayout();
            if (panel_Chat.Controls.Count > messageObjects.Count)
            {
                panel_Chat.ScrollControlIntoView(panel_Chat.Controls[panel_Chat.Controls.Count - messageObjects.Count - 1]);
            }
            isloadingoldmessages = false;
        }

        private void PanelChat_Load(object sender, EventArgs e)
        {
            textboxWriting.Focus();
            this.ActiveControl = textboxWriting;
        }

        public string GetLastMessage()
        {
            if (chatItems.Count == 0)
                return "New conversation!";
            var messageObject = chatItems[chatItems.Count - 1].messageObject;
            if (messageObject.type == 0)
            {
                return messageObject.message;
            }
            else if (messageObject.type == 1)
            {
                return "<Photo>";
            }
            return "";
        }
        public string GetFirstMessage()
        {
            if (chatItems.Count == 0)
                return "";
            return chatItems[0].messageObject.message;
        }

        public bool IsLastMessageFromYou()
        {
            if (panel_Chat.Controls.Count == 0)
                return true;
            ChatItem message = chatItems.Last();
            if (message.IsMyMessage())
                return true;
            return false;
        }

        private void panel_Chat_ControlAdded(object sender, ControlEventArgs e)
        {
            this.OnControlAdded(e);
        }

        private void panel_Chat_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.OnControlRemoved(e);
        }
        private void timerChat_Tick(object sender, EventArgs e)
        {
            locking = false;
            timerChat.Stop();
        }

        private void textboxWriting_SizeChanged(object sender, EventArgs e)
        {
            panelBottomRight.Height = textboxWriting.Height + buttonSend.Height;
            panelBottomRight.Location = new Point(0, this.Height - panelBottomRight.Height);
            panel_Chat.Height = this.Height - panelBottomRight.Height - panelTopRight.Height;
        }

        private void panelBottomRight_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 1, panelBottomRight.Width, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, panelBottomRight.Height);
            }
        }

        private void panelTopRight_Resize(object sender, EventArgs e)
        {
            panelTopRight.Invalidate();
        }

        private void panelBottomRight_Resize(object sender, EventArgs e)
        {
            textboxWriting.DynamicResize();
            panelBottomRight.Invalidate();
        }

        private void panel_Chat_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(pen, 0, 0, 0, panel_Chat.Height);
            }
        }

        private void textboxWriting__TextChanged(object sender, EventArgs e)
        {
            textboxWriting.Multiline = true;
            this.OnClick(e);
        }

        public void ScrollToBottom()
        {
            if (panel_Chat.Controls.Count > 0)
            {
                panel_Chat.ScrollControlIntoView(panel_Chat.Controls[0]);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You are about to delete your conversation with this person, this action cannot be undone, are you sure you want to DELETE ALL YOUR MESSAGES WITH THIS PERSON?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dialogResult = MessageBox.Show("This action will DELETE ALL YOUR MESSAGES with THIS PERSON! Think twice! Are you serious?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (this.Parent != null && this.Parent.Parent != null && this.Parent.Parent is FormApplication)
                    {
                        AFriendClient.Queue_command(Encoding.Unicode.GetBytes("5859" + this.ID));
                        (this.Parent.Parent as FormApplication).RemoveContact(this.ID);
                    }
                }
            }
        }
    }
}

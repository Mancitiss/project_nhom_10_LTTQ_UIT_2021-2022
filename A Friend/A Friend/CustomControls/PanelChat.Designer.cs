
namespace A_Friend.CustomControls
{
    partial class PanelChat
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelChat));
            this.panelTopRight = new System.Windows.Forms.Panel();
            this.labelFriendName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_Chat = new System.Windows.Forms.Panel();
            this.panelBottomRight = new System.Windows.Forms.Panel();
            this.timerChat = new System.Windows.Forms.Timer(this.components);
            this.textboxWriting = new A_Friend.CustomControls.CustomTextBox();
            this.buttonSend = new A_Friend.CustomControls.CustomButton();
            this.friendPicture = new A_Friend.CustomControls.CirclePictureBox();
            this.buttonDelete = new A_Friend.CustomControls.CustomButton();
            this.panelTopRight.SuspendLayout();
            this.panelBottomRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.friendPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTopRight
            // 
            this.panelTopRight.BackColor = System.Drawing.Color.White;
            this.panelTopRight.Controls.Add(this.labelFriendName);
            this.panelTopRight.Controls.Add(this.label3);
            this.panelTopRight.Controls.Add(this.friendPicture);
            this.panelTopRight.Controls.Add(this.buttonDelete);
            this.panelTopRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopRight.Location = new System.Drawing.Point(0, 0);
            this.panelTopRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopRight.Name = "panelTopRight";
            this.panelTopRight.Size = new System.Drawing.Size(912, 60);
            this.panelTopRight.TabIndex = 1;
            this.panelTopRight.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTopRight_Paint);
            this.panelTopRight.Resize += new System.EventHandler(this.panelTopRight_Resize);
            // 
            // labelFriendName
            // 
            this.labelFriendName.AutoSize = true;
            this.labelFriendName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFriendName.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelFriendName.Location = new System.Drawing.Point(72, 10);
            this.labelFriendName.Name = "labelFriendName";
            this.labelFriendName.Size = new System.Drawing.Size(99, 23);
            this.labelFriendName.TabIndex = 6;
            this.labelFriendName.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = " ";
            // 
            // panel_Chat
            // 
            this.panel_Chat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Chat.AutoScroll = true;
            this.panel_Chat.BackColor = System.Drawing.Color.White;
            this.panel_Chat.Location = new System.Drawing.Point(0, 60);
            this.panel_Chat.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Chat.Name = "panel_Chat";
            this.panel_Chat.Padding = new System.Windows.Forms.Padding(2);
            this.panel_Chat.Size = new System.Drawing.Size(912, 592);
            this.panel_Chat.TabIndex = 2;
            this.panel_Chat.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel_Chat_Scroll);
            this.panel_Chat.Click += new System.EventHandler(this.panel_Chat_Click);
            this.panel_Chat.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.panel_Chat_ControlAdded);
            this.panel_Chat.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.panel_Chat_ControlRemoved);
            this.panel_Chat.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Chat_Paint);
            // 
            // panelBottomRight
            // 
            this.panelBottomRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottomRight.BackColor = System.Drawing.SystemColors.Window;
            this.panelBottomRight.Controls.Add(this.textboxWriting);
            this.panelBottomRight.Controls.Add(this.buttonSend);
            this.panelBottomRight.Location = new System.Drawing.Point(0, 652);
            this.panelBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomRight.Name = "panelBottomRight";
            this.panelBottomRight.Size = new System.Drawing.Size(912, 60);
            this.panelBottomRight.TabIndex = 3;
            this.panelBottomRight.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBottomRight_Paint);
            this.panelBottomRight.Resize += new System.EventHandler(this.panelBottomRight_Resize);
            // 
            // timerChat
            // 
            this.timerChat.Interval = 1000;
            this.timerChat.Tick += new System.EventHandler(this.timerChat_Tick);
            // 
            // textboxWriting
            // 
            this.textboxWriting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxWriting.BackColor = System.Drawing.SystemColors.Window;
            this.textboxWriting.BorderColor = System.Drawing.SystemColors.Control;
            this.textboxWriting.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.textboxWriting.BorderRadius = 30;
            this.textboxWriting.BorderSize = 3;
            this.textboxWriting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxWriting.Location = new System.Drawing.Point(24, 6);
            this.textboxWriting.Margin = new System.Windows.Forms.Padding(0);
            this.textboxWriting.Multiline = true;
            this.textboxWriting.Name = "textboxWriting";
            this.textboxWriting.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.textboxWriting.PasswordChar = false;
            this.textboxWriting.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textboxWriting.PlaceholderText = "To ...";
            this.textboxWriting.Size = new System.Drawing.Size(816, 48);
            this.textboxWriting.TabIndex = 2;
            this.textboxWriting.Texts = "";
            this.textboxWriting.UnderlinedStyle = false;
            this.textboxWriting._TextChanged += new System.EventHandler(this.textboxWriting__TextChanged);
            this.textboxWriting.SizeChanged += new System.EventHandler(this.textboxWriting_SizeChanged);
            this.textboxWriting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textboxWriting_KeyDown);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonSend.BackColor = System.Drawing.Color.Transparent;
            this.buttonSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSend.BackgroundImage")));
            this.buttonSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSend.BorderColor = System.Drawing.Color.Empty;
            this.buttonSend.BorderRadius = 10;
            this.buttonSend.BorderSize = 0;
            this.buttonSend.FlatAppearance.BorderSize = 0;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.ForeColor = System.Drawing.Color.White;
            this.buttonSend.Location = new System.Drawing.Point(860, 15);
            this.buttonSend.MaximumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.MinimumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(40, 40);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // friendPicture
            // 
            this.friendPicture.BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat;
            this.friendPicture.BorderColor = System.Drawing.Color.GhostWhite;
            this.friendPicture.BorderColor2 = System.Drawing.Color.Snow;
            this.friendPicture.BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.friendPicture.BorderSize = 0;
            this.friendPicture.GradientAngle = 50F;
            this.friendPicture.Location = new System.Drawing.Point(18, 7);
            this.friendPicture.Name = "friendPicture";
            this.friendPicture.Size = new System.Drawing.Size(45, 45);
            this.friendPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.friendPicture.TabIndex = 1;
            this.friendPicture.TabStop = false;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDelete.BackColor = System.Drawing.Color.Transparent;
            this.buttonDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonDelete.BackgroundImage")));
            this.buttonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonDelete.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonDelete.BorderRadius = 10;
            this.buttonDelete.BorderSize = 0;
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.ForeColor = System.Drawing.Color.White;
            this.buttonDelete.Location = new System.Drawing.Point(860, 10);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(40, 40);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.UseVisualStyleBackColor = false;
            // 
            // PanelChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottomRight);
            this.Controls.Add(this.panel_Chat);
            this.Controls.Add(this.panelTopRight);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PanelChat";
            this.Size = new System.Drawing.Size(912, 712);
            this.Load += new System.EventHandler(this.PanelChat_Load);
            this.panelTopRight.ResumeLayout(false);
            this.panelTopRight.PerformLayout();
            this.panelBottomRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.friendPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTopRight;
        private System.Windows.Forms.Label labelFriendName;
        private System.Windows.Forms.Label label3;
        private CirclePictureBox friendPicture;
        private CustomButton buttonDelete;
        private System.Windows.Forms.Panel panel_Chat;
        private System.Windows.Forms.Panel panelBottomRight;
        private CustomTextBox textboxWriting;
        private CustomButton buttonSend;
        private System.Windows.Forms.Timer timerChat;
    }
}

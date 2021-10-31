
namespace A_Friend
{
    partial class FormApplication
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApplication));
            this.toolTip_DeleteC = new System.Windows.Forms.ToolTip(this.components);
            this.buttonSend = new A_Friend.CustomControls.CustomButton();
            this.toolTip_Send = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelBottomLeft = new System.Windows.Forms.Panel();
            this.ButtonAdd = new A_Friend.CustomControls.CustomButton();
            this.SettingButton = new A_Friend.CustomControls.CustomButton();
            this.LogoutButton = new A_Friend.CustomControls.CustomButton();
            this.panelContact = new System.Windows.Forms.Panel();
            this.panelTopLeft = new System.Windows.Forms.Panel();
            this.labelWarning = new System.Windows.Forms.Label();
            this.customTextBox1 = new A_Friend.CustomControls.CustomTextBox();
            this.panelBottomRight = new System.Windows.Forms.Panel();
            this.textboxWriting = new A_Friend.CustomControls.CustomTextBox();
            this.panelTopRight = new System.Windows.Forms.Panel();
            this.labelUsername = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rjCircularPictureBox1 = new A_Friend.CustomControls.CirclePictureBox();
            this.buttonDelete = new A_Friend.CustomControls.CustomButton();
            this.panelChat = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft.SuspendLayout();
            this.panelBottomLeft.SuspendLayout();
            this.panelTopLeft.SuspendLayout();
            this.panelBottomRight.SuspendLayout();
            this.panelTopRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rjCircularPictureBox1)).BeginInit();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip_DeleteC
            // 
            this.toolTip_DeleteC.AutoPopDelay = 5000;
            this.toolTip_DeleteC.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTip_DeleteC.InitialDelay = 1000;
            this.toolTip_DeleteC.ReshowDelay = 100;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.BackColor = System.Drawing.Color.Transparent;
            this.buttonSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSend.BackgroundImage")));
            this.buttonSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSend.BorderColor = System.Drawing.Color.Empty;
            this.buttonSend.BorderRadius = 10;
            this.buttonSend.BorderSize = 0;
            this.buttonSend.FlatAppearance.BorderSize = 0;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.ForeColor = System.Drawing.Color.White;
            this.buttonSend.Location = new System.Drawing.Point(860, 10);
            this.buttonSend.MaximumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.MinimumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(40, 40);
            this.buttonSend.TabIndex = 0;
            this.toolTip_DeleteC.SetToolTip(this.buttonSend, "send");
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // toolTip_Send
            // 
            this.toolTip_Send.AutoPopDelay = 5000;
            this.toolTip_Send.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTip_Send.InitialDelay = 2000;
            this.toolTip_Send.ReshowDelay = 100;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(250, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 584);
            this.panel1.TabIndex = 4;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.Control;
            this.panelLeft.Controls.Add(this.panelBottomLeft);
            this.panelLeft.Controls.Add(this.panelContact);
            this.panelLeft.Controls.Add(this.panelTopLeft);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.MaximumSize = new System.Drawing.Size(300, 10000);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(300, 720);
            this.panelLeft.TabIndex = 4;
            // 
            // panelBottomLeft
            // 
            this.panelBottomLeft.BackColor = System.Drawing.SystemColors.Window;
            this.panelBottomLeft.Controls.Add(this.ButtonAdd);
            this.panelBottomLeft.Controls.Add(this.SettingButton);
            this.panelBottomLeft.Controls.Add(this.LogoutButton);
            this.panelBottomLeft.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomLeft.Location = new System.Drawing.Point(0, 660);
            this.panelBottomLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomLeft.Name = "panelBottomLeft";
            this.panelBottomLeft.Size = new System.Drawing.Size(300, 60);
            this.panelBottomLeft.TabIndex = 1;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.BackColor = System.Drawing.Color.Transparent;
            this.ButtonAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonAdd.BackgroundImage")));
            this.ButtonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ButtonAdd.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonAdd.BorderRadius = 20;
            this.ButtonAdd.BorderSize = 0;
            this.ButtonAdd.FlatAppearance.BorderSize = 0;
            this.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAdd.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.Location = new System.Drawing.Point(110, 10);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(40, 40);
            this.ButtonAdd.TabIndex = 0;
            this.ButtonAdd.UseVisualStyleBackColor = false;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click_1);
            // 
            // SettingButton
            // 
            this.SettingButton.BackColor = System.Drawing.SystemColors.Window;
            this.SettingButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SettingButton.BackgroundImage")));
            this.SettingButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SettingButton.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.SettingButton.BorderRadius = 20;
            this.SettingButton.BorderSize = 0;
            this.SettingButton.FlatAppearance.BorderSize = 0;
            this.SettingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingButton.ForeColor = System.Drawing.Color.White;
            this.SettingButton.Location = new System.Drawing.Point(60, 10);
            this.SettingButton.Name = "SettingButton";
            this.SettingButton.Size = new System.Drawing.Size(40, 40);
            this.SettingButton.TabIndex = 1;
            this.SettingButton.UseVisualStyleBackColor = false;
            this.SettingButton.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // LogoutButton
            // 
            this.LogoutButton.BackColor = System.Drawing.SystemColors.Window;
            this.LogoutButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LogoutButton.BackgroundImage")));
            this.LogoutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LogoutButton.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.LogoutButton.BorderRadius = 20;
            this.LogoutButton.BorderSize = 0;
            this.LogoutButton.FlatAppearance.BorderSize = 0;
            this.LogoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LogoutButton.ForeColor = System.Drawing.Color.White;
            this.LogoutButton.Location = new System.Drawing.Point(10, 10);
            this.LogoutButton.Name = "LogoutButton";
            this.LogoutButton.Size = new System.Drawing.Size(40, 40);
            this.LogoutButton.TabIndex = 0;
            this.LogoutButton.UseVisualStyleBackColor = false;
            this.LogoutButton.Click += new System.EventHandler(this.customButton1_Click);
            // 
            // panelContact
            // 
            this.panelContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContact.AutoScroll = true;
            this.panelContact.BackColor = System.Drawing.SystemColors.Window;
            this.panelContact.Location = new System.Drawing.Point(2, 80);
            this.panelContact.Name = "panelContact";
            this.panelContact.Size = new System.Drawing.Size(298, 580);
            this.panelContact.TabIndex = 2;
            // 
            // panelTopLeft
            // 
            this.panelTopLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.panelTopLeft.Controls.Add(this.labelWarning);
            this.panelTopLeft.Controls.Add(this.customTextBox1);
            this.panelTopLeft.Location = new System.Drawing.Point(0, 0);
            this.panelTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopLeft.Name = "panelTopLeft";
            this.panelTopLeft.Size = new System.Drawing.Size(300, 80);
            this.panelTopLeft.TabIndex = 0;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(66, 62);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(158, 17);
            this.labelWarning.TabIndex = 0;
            this.labelWarning.Text = "This user does not exist";
            // 
            // customTextBox1
            // 
            this.customTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.customTextBox1.BorderColor = System.Drawing.SystemColors.Window;
            this.customTextBox1.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.customTextBox1.BorderRadius = 20;
            this.customTextBox1.BorderSize = 2;
            this.customTextBox1.Location = new System.Drawing.Point(13, 17);
            this.customTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.customTextBox1.Multiline = false;
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.customTextBox1.PasswordChar = false;
            this.customTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.customTextBox1.PlaceholderText = "Search";
            this.customTextBox1.Size = new System.Drawing.Size(271, 38);
            this.customTextBox1.TabIndex = 0;
            this.customTextBox1.Texts = "";
            this.customTextBox1.UnderlinedStyle = false;
            this.customTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.customTextBox1_KeyDown);
            // 
            // panelBottomRight
            // 
            this.panelBottomRight.BackColor = System.Drawing.SystemColors.Window;
            this.panelBottomRight.Controls.Add(this.textboxWriting);
            this.panelBottomRight.Controls.Add(this.buttonSend);
            this.panelBottomRight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomRight.Location = new System.Drawing.Point(0, 660);
            this.panelBottomRight.MaximumSize = new System.Drawing.Size(10000, 60);
            this.panelBottomRight.Name = "panelBottomRight";
            this.panelBottomRight.Size = new System.Drawing.Size(912, 60);
            this.panelBottomRight.TabIndex = 0;
            // 
            // textboxWriting
            // 
            this.textboxWriting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxWriting.BackColor = System.Drawing.SystemColors.Window;
            this.textboxWriting.BorderColor = System.Drawing.SystemColors.Control;
            this.textboxWriting.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.textboxWriting.BorderRadius = 30;
            this.textboxWriting.BorderSize = 3;
            this.textboxWriting.Font = new System.Drawing.Font("Microsoft New Tai Lue", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxWriting.Location = new System.Drawing.Point(24, 7);
            this.textboxWriting.Margin = new System.Windows.Forms.Padding(0);
            this.textboxWriting.Multiline = false;
            this.textboxWriting.Name = "textboxWriting";
            this.textboxWriting.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.textboxWriting.PasswordChar = false;
            this.textboxWriting.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textboxWriting.PlaceholderText = "To ...";
            this.textboxWriting.Size = new System.Drawing.Size(816, 46);
            this.textboxWriting.TabIndex = 2;
            this.textboxWriting.Texts = "";
            this.textboxWriting.UnderlinedStyle = false;
            this.textboxWriting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textboxWriting_KeyDown);
            // 
            // panelTopRight
            // 
            this.panelTopRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.panelTopRight.Controls.Add(this.labelUsername);
            this.panelTopRight.Controls.Add(this.label3);
            this.panelTopRight.Controls.Add(this.rjCircularPictureBox1);
            this.panelTopRight.Controls.Add(this.buttonDelete);
            this.panelTopRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopRight.Location = new System.Drawing.Point(0, 0);
            this.panelTopRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopRight.Name = "panelTopRight";
            this.panelTopRight.Size = new System.Drawing.Size(912, 80);
            this.panelTopRight.TabIndex = 0;
            this.panelTopRight.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTopRight_Paint);
            this.panelTopRight.Resize += new System.EventHandler(this.panelTopRight_Resize);
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelUsername.Location = new System.Drawing.Point(72, 21);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(83, 20);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = " ";
            // 
            // rjCircularPictureBox1
            // 
            this.rjCircularPictureBox1.BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat;
            this.rjCircularPictureBox1.BorderColor = System.Drawing.Color.FloralWhite;
            this.rjCircularPictureBox1.BorderColor2 = System.Drawing.Color.Snow;
            this.rjCircularPictureBox1.BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.rjCircularPictureBox1.BorderSize = 2;
            this.rjCircularPictureBox1.GradientAngle = 50F;
            this.rjCircularPictureBox1.Location = new System.Drawing.Point(10, 10);
            this.rjCircularPictureBox1.Name = "rjCircularPictureBox1";
            this.rjCircularPictureBox1.Size = new System.Drawing.Size(60, 60);
            this.rjCircularPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rjCircularPictureBox1.TabIndex = 1;
            this.rjCircularPictureBox1.TabStop = false;
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
            this.buttonDelete.Location = new System.Drawing.Point(860, 21);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(40, 40);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.UseVisualStyleBackColor = false;
            // 
            // panelChat
            // 
            this.panelChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChat.AutoScroll = true;
            this.panelChat.BackColor = System.Drawing.SystemColors.Control;
            this.panelChat.Location = new System.Drawing.Point(0, 80);
            this.panelChat.Margin = new System.Windows.Forms.Padding(0);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(912, 580);
            this.panelChat.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.Controls.Add(this.panelChat);
            this.panelRight.Controls.Add(this.panelTopRight);
            this.panelRight.Controls.Add(this.panelBottomRight);
            this.panelRight.Location = new System.Drawing.Point(300, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(912, 720);
            this.panelRight.TabIndex = 3;
            // 
            // FormApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 720);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "FormApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormApplication";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormApplication_FormClosed);
            this.Load += new System.EventHandler(this.FormApplication_Load);
            this.panelLeft.ResumeLayout(false);
            this.panelBottomLeft.ResumeLayout(false);
            this.panelTopLeft.ResumeLayout(false);
            this.panelTopLeft.PerformLayout();
            this.panelBottomRight.ResumeLayout(false);
            this.panelTopRight.ResumeLayout(false);
            this.panelTopRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rjCircularPictureBox1)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip_DeleteC;
        private System.Windows.Forms.ToolTip toolTip_Send;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelTopLeft;
        private CustomControls.CustomTextBox customTextBox1;
        private System.Windows.Forms.Panel panelBottomLeft;
        private CustomControls.CustomButton LogoutButton;
        private CustomControls.CustomButton SettingButton;
        private CustomControls.CustomButton ButtonAdd;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Panel panelContact;
        private System.Windows.Forms.Panel panelBottomRight;
        private CustomControls.CustomTextBox textboxWriting;
        private CustomControls.CustomButton buttonSend;
        private System.Windows.Forms.Panel panelTopRight;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label label3;
        private CustomControls.CirclePictureBox rjCircularPictureBox1;
        private CustomControls.CustomButton buttonDelete;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Panel panelRight;
    }
}
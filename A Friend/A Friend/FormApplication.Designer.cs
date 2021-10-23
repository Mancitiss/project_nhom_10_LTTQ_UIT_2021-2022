
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
            this.panelChat = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelTopRight = new System.Windows.Forms.Panel();
            this.panelBottomRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDelete = new A_Friend.CustomControls.CustomButton();
            this.textboxWriting = new A_Friend.CustomControls.CustomTextBox();
            this.buttonSend = new A_Friend.CustomControls.CustomButton();
            this.customLogout = new A_Friend.CustomControls.CustomButton();
            this.customSetting = new A_Friend.CustomControls.CustomButton();
            this.customTextBox1 = new A_Friend.CustomControls.CustomTextBox();
            this.panelRight.SuspendLayout();
            this.panelTopRight.SuspendLayout();
            this.panelBottomRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelChat
            // 
            this.panelChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChat.BackColor = System.Drawing.SystemColors.Control;
            this.panelChat.Location = new System.Drawing.Point(0, 50);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(734, 451);
            this.panelChat.TabIndex = 7;
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.Controls.Add(this.panelChat);
            this.panelRight.Controls.Add(this.panelTopRight);
            this.panelRight.Controls.Add(this.panelBottomRight);
            this.panelRight.Location = new System.Drawing.Point(250, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(734, 561);
            this.panelRight.TabIndex = 3;
            // 
            // panelTopRight
            // 
            this.panelTopRight.Controls.Add(this.buttonDelete);
            this.panelTopRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopRight.Location = new System.Drawing.Point(0, 0);
            this.panelTopRight.Name = "panelTopRight";
            this.panelTopRight.Size = new System.Drawing.Size(734, 50);
            this.panelTopRight.TabIndex = 6;
            // 
            // panelBottomRight
            // 
            this.panelBottomRight.Controls.Add(this.textboxWriting);
            this.panelBottomRight.Controls.Add(this.buttonSend);
            this.panelBottomRight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomRight.Location = new System.Drawing.Point(0, 501);
            this.panelBottomRight.MaximumSize = new System.Drawing.Size(10000, 60);
            this.panelBottomRight.Name = "panelBottomRight";
            this.panelBottomRight.Size = new System.Drawing.Size(734, 60);
            this.panelBottomRight.TabIndex = 5;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.Control;
            this.panelLeft.Controls.Add(this.panel3);
            this.panelLeft.Controls.Add(this.panel2);
            this.panelLeft.Controls.Add(this.panel1);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.MaximumSize = new System.Drawing.Size(250, 10000);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(250, 561);
            this.panelLeft.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(1, 89);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(249, 411);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.customLogout);
            this.panel2.Controls.Add(this.customSetting);
            this.panel2.Location = new System.Drawing.Point(0, 500);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(249, 60);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.customTextBox1);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(248, 89);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(2, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search:";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDelete.BackColor = System.Drawing.SystemColors.Control;
            this.buttonDelete.BackgroundImage = global::A_Friend.Properties.Resources.trash_alt_regular;
            this.buttonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonDelete.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonDelete.BorderRadius = 20;
            this.buttonDelete.BorderSize = 0;
            this.buttonDelete.FlatAppearance.BorderSize = 0;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.ForeColor = System.Drawing.Color.White;
            this.buttonDelete.Location = new System.Drawing.Point(691, 5);
            this.buttonDelete.MaximumSize = new System.Drawing.Size(40, 40);
            this.buttonDelete.MinimumSize = new System.Drawing.Size(40, 40);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(40, 40);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.UseVisualStyleBackColor = false;
            // 
            // textboxWriting
            // 
            this.textboxWriting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxWriting.BackColor = System.Drawing.SystemColors.Window;
            this.textboxWriting.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(136)))), ((int)(((byte)(235)))));
            this.textboxWriting.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(165)))));
            this.textboxWriting.BorderRadius = 30;
            this.textboxWriting.BorderSize = 3;
            this.textboxWriting.Location = new System.Drawing.Point(19, 10);
            this.textboxWriting.Margin = new System.Windows.Forms.Padding(0);
            this.textboxWriting.Multiline = false;
            this.textboxWriting.Name = "textboxWriting";
            this.textboxWriting.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.textboxWriting.PasswordChar = false;
            this.textboxWriting.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textboxWriting.PlaceholderText = "To ...";
            this.textboxWriting.Size = new System.Drawing.Size(656, 38);
            this.textboxWriting.TabIndex = 2;
            this.textboxWriting.Texts = "";
            this.textboxWriting.UnderlinedStyle = false;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSend.BackgroundImage = global::A_Friend.Properties.Resources.paper_plane_regular;
            this.buttonSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSend.BorderColor = System.Drawing.Color.Empty;
            this.buttonSend.BorderRadius = 10;
            this.buttonSend.BorderSize = 0;
            this.buttonSend.FlatAppearance.BorderSize = 0;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.ForeColor = System.Drawing.Color.White;
            this.buttonSend.Location = new System.Drawing.Point(682, 10);
            this.buttonSend.MaximumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.MinimumSize = new System.Drawing.Size(40, 40);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(40, 40);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.UseVisualStyleBackColor = false;
            // 
            // customLogout
            // 
            this.customLogout.BackColor = System.Drawing.SystemColors.Control;
            this.customLogout.BackgroundImage = global::A_Friend.Properties.Resources.sign_out_option;
            this.customLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.customLogout.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customLogout.BorderRadius = 0;
            this.customLogout.BorderSize = 0;
            this.customLogout.FlatAppearance.BorderSize = 0;
            this.customLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customLogout.ForeColor = System.Drawing.Color.White;
            this.customLogout.Location = new System.Drawing.Point(184, 11);
            this.customLogout.Name = "customLogout";
            this.customLogout.Size = new System.Drawing.Size(40, 40);
            this.customLogout.TabIndex = 0;
            this.customLogout.UseVisualStyleBackColor = false;
            this.customLogout.Click += new System.EventHandler(this.customLogout_Click);
            // 
            // customSetting
            // 
            this.customSetting.BackColor = System.Drawing.SystemColors.Control;
            this.customSetting.BackgroundImage = global::A_Friend.Properties.Resources.Cogs;
            this.customSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.customSetting.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customSetting.BorderRadius = 0;
            this.customSetting.BorderSize = 0;
            this.customSetting.FlatAppearance.BorderSize = 0;
            this.customSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customSetting.ForeColor = System.Drawing.Color.White;
            this.customSetting.Location = new System.Drawing.Point(12, 11);
            this.customSetting.Name = "customSetting";
            this.customSetting.Size = new System.Drawing.Size(50, 40);
            this.customSetting.TabIndex = 4;
            this.customSetting.UseVisualStyleBackColor = false;
            this.customSetting.Click += new System.EventHandler(this.customButton1_Click);
            // 
            // customTextBox1
            // 
            this.customTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.customTextBox1.BorderColor = System.Drawing.SystemColors.Highlight;
            this.customTextBox1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.customTextBox1.BorderRadius = 30;
            this.customTextBox1.BorderSize = 3;
            this.customTextBox1.Location = new System.Drawing.Point(0, 41);
            this.customTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.customTextBox1.Multiline = false;
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.customTextBox1.PasswordChar = false;
            this.customTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.customTextBox1.PlaceholderText = "Username..";
            this.customTextBox1.Size = new System.Drawing.Size(245, 38);
            this.customTextBox1.TabIndex = 0;
            this.customTextBox1.Texts = "";
            this.customTextBox1.UnderlinedStyle = false;
            // 
            // FormApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormApplication";
            this.Text = "FormApplication";
            this.panelRight.ResumeLayout(false);
            this.panelTopRight.ResumeLayout(false);
            this.panelBottomRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelTopRight;
        private CustomControls.CustomButton buttonDelete;
        private System.Windows.Forms.Panel panelBottomRight;
        private CustomControls.CustomTextBox textboxWriting;
        private CustomControls.CustomButton buttonSend;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private CustomControls.CustomTextBox customTextBox1;
        private System.Windows.Forms.Panel panel2;
        private CustomControls.CustomButton customSetting;
        private CustomControls.CustomButton customLogout;
        private System.Windows.Forms.Panel panel3;
    }
}

namespace A_Friend
{
    partial class FormSettings
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
            this.labelUsername = new System.Windows.Forms.Label();
            this.panelPassword = new System.Windows.Forms.Panel();
            this.panelUsername = new System.Windows.Forms.Panel();
            this.labelWarning = new System.Windows.Forms.Label();
            this.customButtonUsername = new A_Friend.CustomControls.CustomButton();
            this.customButtonAvatar = new A_Friend.CustomControls.CustomButton();
            this.textBoxConfirmPassword = new A_Friend.CustomControls.CustomTextBox();
            this.buttonSavePassword = new A_Friend.CustomControls.CustomButton();
            this.textBoxCurrentPassword = new A_Friend.CustomControls.CustomTextBox();
            this.textBoxNewPassword = new A_Friend.CustomControls.CustomTextBox();
            this.customButtonExit = new A_Friend.CustomControls.CustomButton();
            this.customTextBoxUsername = new A_Friend.CustomControls.CustomTextBox();
            this.buttonSaveUsername = new A_Friend.CustomControls.CustomButton();
            this.customButtonPassword = new A_Friend.CustomControls.CustomButton();
            this.circlePictureBox1 = new A_Friend.CustomControls.CirclePictureBox();
            this.panelPassword.SuspendLayout();
            this.panelUsername.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circlePictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelUsername.Location = new System.Drawing.Point(127, 115);
            this.labelUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(83, 20);
            this.labelUsername.TabIndex = 2;
            this.labelUsername.Text = "Username";
            // 
            // panelPassword
            // 
            this.panelPassword.Controls.Add(this.textBoxConfirmPassword);
            this.panelPassword.Controls.Add(this.buttonSavePassword);
            this.panelPassword.Controls.Add(this.textBoxCurrentPassword);
            this.panelPassword.Controls.Add(this.textBoxNewPassword);
            this.panelPassword.Location = new System.Drawing.Point(47, 165);
            this.panelPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelPassword.Name = "panelPassword";
            this.panelPassword.Size = new System.Drawing.Size(242, 173);
            this.panelPassword.TabIndex = 10;
            // 
            // panelUsername
            // 
            this.panelUsername.Controls.Add(this.customTextBoxUsername);
            this.panelUsername.Controls.Add(this.buttonSaveUsername);
            this.panelUsername.Location = new System.Drawing.Point(47, 209);
            this.panelUsername.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelUsername.Name = "panelUsername";
            this.panelUsername.Size = new System.Drawing.Size(262, 94);
            this.panelUsername.TabIndex = 13;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(88, 348);
            this.labelWarning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(187, 17);
            this.labelWarning.TabIndex = 14;
            this.labelWarning.Text = "Please enter your password!";
            // 
            // customButtonUsername
            // 
            this.customButtonUsername.BackColor = System.Drawing.Color.Transparent;
            this.customButtonUsername.BackgroundImage = global::A_Friend.Properties.Resources._353430_checkbox_pen_edit_pencil_icon;
            this.customButtonUsername.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.customButtonUsername.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButtonUsername.BorderRadius = 0;
            this.customButtonUsername.BorderSize = 0;
            this.customButtonUsername.FlatAppearance.BorderSize = 0;
            this.customButtonUsername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtonUsername.ForeColor = System.Drawing.Color.White;
            this.customButtonUsername.Location = new System.Drawing.Point(102, 114);
            this.customButtonUsername.Margin = new System.Windows.Forms.Padding(2);
            this.customButtonUsername.Name = "customButtonUsername";
            this.customButtonUsername.Size = new System.Drawing.Size(20, 22);
            this.customButtonUsername.TabIndex = 12;
            this.customButtonUsername.UseVisualStyleBackColor = false;
            this.customButtonUsername.Click += new System.EventHandler(this.customButtonUsername_Click);
            // 
            // customButtonAvatar
            // 
            this.customButtonAvatar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.customButtonAvatar.BackgroundImage = global::A_Friend.Properties.Resources._211634_camera_icon__1_;
            this.customButtonAvatar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.customButtonAvatar.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButtonAvatar.BorderRadius = 24;
            this.customButtonAvatar.BorderSize = 0;
            this.customButtonAvatar.FlatAppearance.BorderSize = 0;
            this.customButtonAvatar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtonAvatar.ForeColor = System.Drawing.Color.White;
            this.customButtonAvatar.Location = new System.Drawing.Point(189, 73);
            this.customButtonAvatar.Margin = new System.Windows.Forms.Padding(2);
            this.customButtonAvatar.Name = "customButtonAvatar";
            this.customButtonAvatar.Size = new System.Drawing.Size(22, 24);
            this.customButtonAvatar.TabIndex = 11;
            this.customButtonAvatar.UseVisualStyleBackColor = false;
            this.customButtonAvatar.Click += new System.EventHandler(this.customButtonAvatar_Click);
            // 
            // textBoxConfirmPassword
            // 
            this.textBoxConfirmPassword.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxConfirmPassword.BorderColor = System.Drawing.Color.SteelBlue;
            this.textBoxConfirmPassword.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBoxConfirmPassword.BorderRadius = 20;
            this.textBoxConfirmPassword.BorderSize = 2;
            this.textBoxConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBoxConfirmPassword.Location = new System.Drawing.Point(40, 87);
            this.textBoxConfirmPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxConfirmPassword.Multiline = false;
            this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            this.textBoxConfirmPassword.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.textBoxConfirmPassword.PasswordChar = true;
            this.textBoxConfirmPassword.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBoxConfirmPassword.PlaceholderText = "Confirm Password";
            this.textBoxConfirmPassword.Size = new System.Drawing.Size(169, 34);
            this.textBoxConfirmPassword.TabIndex = 7;
            this.textBoxConfirmPassword.Texts = "";
            this.textBoxConfirmPassword.UnderlinedStyle = false;
            // 
            // buttonSavePassword
            // 
            this.buttonSavePassword.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonSavePassword.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonSavePassword.BorderRadius = 20;
            this.buttonSavePassword.BorderSize = 0;
            this.buttonSavePassword.FlatAppearance.BorderSize = 0;
            this.buttonSavePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSavePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonSavePassword.ForeColor = System.Drawing.Color.White;
            this.buttonSavePassword.Location = new System.Drawing.Point(83, 134);
            this.buttonSavePassword.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSavePassword.Name = "buttonSavePassword";
            this.buttonSavePassword.Size = new System.Drawing.Size(79, 33);
            this.buttonSavePassword.TabIndex = 8;
            this.buttonSavePassword.Text = "Save";
            this.buttonSavePassword.UseVisualStyleBackColor = false;
            this.buttonSavePassword.Click += new System.EventHandler(this.buttonSavePassword_Click);
            // 
            // textBoxCurrentPassword
            // 
            this.textBoxCurrentPassword.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxCurrentPassword.BorderColor = System.Drawing.Color.SteelBlue;
            this.textBoxCurrentPassword.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBoxCurrentPassword.BorderRadius = 20;
            this.textBoxCurrentPassword.BorderSize = 2;
            this.textBoxCurrentPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBoxCurrentPassword.Location = new System.Drawing.Point(40, 4);
            this.textBoxCurrentPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCurrentPassword.Multiline = false;
            this.textBoxCurrentPassword.Name = "textBoxCurrentPassword";
            this.textBoxCurrentPassword.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.textBoxCurrentPassword.PasswordChar = true;
            this.textBoxCurrentPassword.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBoxCurrentPassword.PlaceholderText = "Current Password";
            this.textBoxCurrentPassword.Size = new System.Drawing.Size(168, 34);
            this.textBoxCurrentPassword.TabIndex = 5;
            this.textBoxCurrentPassword.Texts = "";
            this.textBoxCurrentPassword.UnderlinedStyle = false;
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxNewPassword.BorderColor = System.Drawing.Color.SteelBlue;
            this.textBoxNewPassword.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBoxNewPassword.BorderRadius = 20;
            this.textBoxNewPassword.BorderSize = 2;
            this.textBoxNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBoxNewPassword.Location = new System.Drawing.Point(40, 46);
            this.textBoxNewPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxNewPassword.Multiline = false;
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.textBoxNewPassword.PasswordChar = true;
            this.textBoxNewPassword.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBoxNewPassword.PlaceholderText = "New Password";
            this.textBoxNewPassword.Size = new System.Drawing.Size(168, 34);
            this.textBoxNewPassword.TabIndex = 6;
            this.textBoxNewPassword.Texts = "";
            this.textBoxNewPassword.UnderlinedStyle = false;
            // 
            // customButtonExit
            // 
            this.customButtonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(54)))), ((int)(((byte)(41)))));
            this.customButtonExit.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButtonExit.BorderRadius = 20;
            this.customButtonExit.BorderSize = 0;
            this.customButtonExit.FlatAppearance.BorderSize = 0;
            this.customButtonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.customButtonExit.ForeColor = System.Drawing.Color.White;
            this.customButtonExit.Location = new System.Drawing.Point(130, 380);
            this.customButtonExit.Margin = new System.Windows.Forms.Padding(2);
            this.customButtonExit.Name = "customButtonExit";
            this.customButtonExit.Size = new System.Drawing.Size(79, 32);
            this.customButtonExit.TabIndex = 0;
            this.customButtonExit.Text = "Exit ";
            this.customButtonExit.UseVisualStyleBackColor = false;
            this.customButtonExit.Click += new System.EventHandler(this.customButtonExit_Click);
            // 
            // customTextBoxUsername
            // 
            this.customTextBoxUsername.BackColor = System.Drawing.SystemColors.Window;
            this.customTextBoxUsername.BorderColor = System.Drawing.Color.SteelBlue;
            this.customTextBoxUsername.BorderFocusColor = System.Drawing.Color.HotPink;
            this.customTextBoxUsername.BorderRadius = 20;
            this.customTextBoxUsername.BorderSize = 2;
            this.customTextBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.customTextBoxUsername.Location = new System.Drawing.Point(40, 4);
            this.customTextBoxUsername.Margin = new System.Windows.Forms.Padding(4);
            this.customTextBoxUsername.Multiline = false;
            this.customTextBoxUsername.Name = "customTextBoxUsername";
            this.customTextBoxUsername.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.customTextBoxUsername.PasswordChar = false;
            this.customTextBoxUsername.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.customTextBoxUsername.PlaceholderText = "New Name";
            this.customTextBoxUsername.Size = new System.Drawing.Size(167, 34);
            this.customTextBoxUsername.TabIndex = 3;
            this.customTextBoxUsername.Texts = "";
            this.customTextBoxUsername.UnderlinedStyle = false;
            // 
            // buttonSaveUsername
            // 
            this.buttonSaveUsername.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonSaveUsername.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonSaveUsername.BorderRadius = 20;
            this.buttonSaveUsername.BorderSize = 0;
            this.buttonSaveUsername.FlatAppearance.BorderSize = 0;
            this.buttonSaveUsername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.buttonSaveUsername.ForeColor = System.Drawing.Color.White;
            this.buttonSaveUsername.Location = new System.Drawing.Point(83, 54);
            this.buttonSaveUsername.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveUsername.Name = "buttonSaveUsername";
            this.buttonSaveUsername.Size = new System.Drawing.Size(79, 33);
            this.buttonSaveUsername.TabIndex = 4;
            this.buttonSaveUsername.Text = "Save";
            this.buttonSaveUsername.UseVisualStyleBackColor = false;
            this.buttonSaveUsername.Click += new System.EventHandler(this.buttonSaveUsername_Click);
            // 
            // customButtonPassword
            // 
            this.customButtonPassword.BackColor = System.Drawing.Color.Transparent;
            this.customButtonPassword.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButtonPassword.BorderRadius = 0;
            this.customButtonPassword.BorderSize = 0;
            this.customButtonPassword.FlatAppearance.BorderSize = 0;
            this.customButtonPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButtonPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.customButtonPassword.ForeColor = System.Drawing.Color.SteelBlue;
            this.customButtonPassword.Location = new System.Drawing.Point(94, 138);
            this.customButtonPassword.Margin = new System.Windows.Forms.Padding(2);
            this.customButtonPassword.Name = "customButtonPassword";
            this.customButtonPassword.Size = new System.Drawing.Size(152, 25);
            this.customButtonPassword.TabIndex = 3;
            this.customButtonPassword.Text = "Change password?";
            this.customButtonPassword.UseVisualStyleBackColor = false;
            this.customButtonPassword.Click += new System.EventHandler(this.customButtonPassword_Click);
            // 
            // circlePictureBox1
            // 
            this.circlePictureBox1.BorderCapStyle = System.Drawing.Drawing2D.DashCap.Flat;
            this.circlePictureBox1.BorderColor = System.Drawing.Color.DimGray;
            this.circlePictureBox1.BorderColor2 = System.Drawing.Color.DimGray;
            this.circlePictureBox1.BorderLineStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.circlePictureBox1.BorderSize = 2;
            this.circlePictureBox1.GradientAngle = 50F;
            this.circlePictureBox1.Location = new System.Drawing.Point(122, 15);
            this.circlePictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.circlePictureBox1.Name = "circlePictureBox1";
            this.circlePictureBox1.Size = new System.Drawing.Size(90, 90);
            this.circlePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.circlePictureBox1.TabIndex = 1;
            this.circlePictureBox1.TabStop = false;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 430);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.customButtonUsername);
            this.Controls.Add(this.customButtonAvatar);
            this.Controls.Add(this.panelPassword);
            this.Controls.Add(this.customButtonExit);
            this.Controls.Add(this.panelUsername);
            this.Controls.Add(this.customButtonPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.circlePictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormSettings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.panelPassword.ResumeLayout(false);
            this.panelUsername.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.circlePictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.CustomButton customButtonExit;
        private CustomControls.CirclePictureBox circlePictureBox1;
        private System.Windows.Forms.Label labelUsername;
        private CustomControls.CustomButton customButtonPassword;
        private CustomControls.CustomTextBox customTextBoxUsername;
        private CustomControls.CustomTextBox textBoxCurrentPassword;
        private CustomControls.CustomTextBox textBoxNewPassword;
        private System.Windows.Forms.Panel panelPassword;
        private CustomControls.CustomButton customButtonAvatar;
        private CustomControls.CustomButton customButtonUsername;
        private System.Windows.Forms.Panel panelUsername;
        private CustomControls.CustomButton buttonSaveUsername;
        private CustomControls.CustomButton buttonSavePassword;
        private CustomControls.CustomTextBox textBoxConfirmPassword;
        private System.Windows.Forms.Label labelWarning;
    }
}
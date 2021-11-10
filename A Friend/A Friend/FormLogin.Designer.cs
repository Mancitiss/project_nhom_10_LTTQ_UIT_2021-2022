﻿namespace A_Friend
{
    partial class FormLogin
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
            this.labelTittle = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.timerClosing = new System.Windows.Forms.Timer(this.components);
            this.buttonSignUp = new A_Friend.CustomControls.CustomButton();
            this.textBoxPassword = new A_Friend.CustomControls.CustomTextBox();
            this.textBoxUserName = new A_Friend.CustomControls.CustomTextBox();
            this.buttonLogIn = new A_Friend.CustomControls.CustomButton();
            this.buttonExit = new A_Friend.CustomControls.CustomButton();
            this.timerDisconnect = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelTittle
            // 
            this.labelTittle.AutoSize = true;
            this.labelTittle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTittle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.labelTittle.Location = new System.Drawing.Point(100, 31);
            this.labelTittle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTittle.Name = "labelTittle";
            this.labelTittle.Size = new System.Drawing.Size(182, 39);
            this.labelTittle.TabIndex = 0;
            this.labelTittle.Text = "User Login";
            // 
            // labelWarning
            // 
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(0, 293);
            this.labelWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(400, 122);
            this.labelWarning.TabIndex = 0;
            this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerClosing
            // 
            this.timerClosing.Interval = 1000;
            this.timerClosing.Tick += new System.EventHandler(this.timerClosing_Tick);
            // 
            // buttonSignUp
            // 
            this.buttonSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(228)))), ((int)(((byte)(185)))));
            this.buttonSignUp.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonSignUp.BorderRadius = 20;
            this.buttonSignUp.BorderSize = 0;
            this.buttonSignUp.FlatAppearance.BorderSize = 0;
            this.buttonSignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSignUp.ForeColor = System.Drawing.Color.White;
            this.buttonSignUp.Location = new System.Drawing.Point(33, 418);
            this.buttonSignUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSignUp.Name = "buttonSignUp";
            this.buttonSignUp.Size = new System.Drawing.Size(333, 49);
            this.buttonSignUp.TabIndex = 4;
            this.buttonSignUp.Text = "SIGN UP";
            this.buttonSignUp.UseVisualStyleBackColor = false;
            this.buttonSignUp.Click += new System.EventHandler(this.buttonSignUp_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBoxPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBoxPassword.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.textBoxPassword.BorderRadius = 20;
            this.textBoxPassword.BorderSize = 2;
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(33, 166);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxPassword.Multiline = false;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Padding = new System.Windows.Forms.Padding(21, 12, 21, 12);
            this.textBoxPassword.PasswordChar = true;
            this.textBoxPassword.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBoxPassword.PlaceholderText = "Password";
            this.textBoxPassword.Size = new System.Drawing.Size(333, 45);
            this.textBoxPassword.TabIndex = 2;
            this.textBoxPassword.Texts = "";
            this.textBoxPassword.UnderlinedStyle = false;
            this.textBoxPassword.Enter += new System.EventHandler(this.textBoxPassword_Enter);
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_KeyDown);
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBoxUserName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBoxUserName.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.textBoxUserName.BorderRadius = 20;
            this.textBoxUserName.BorderSize = 2;
            this.textBoxUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUserName.Location = new System.Drawing.Point(33, 92);
            this.textBoxUserName.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxUserName.Multiline = false;
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Padding = new System.Windows.Forms.Padding(21, 12, 21, 12);
            this.textBoxUserName.PasswordChar = false;
            this.textBoxUserName.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBoxUserName.PlaceholderText = "User name";
            this.textBoxUserName.Size = new System.Drawing.Size(333, 45);
            this.textBoxUserName.TabIndex = 1;
            this.textBoxUserName.Texts = "";
            this.textBoxUserName.UnderlinedStyle = false;
            this.textBoxUserName.Enter += new System.EventHandler(this.textBoxUserName_Enter);
            this.textBoxUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxUserName_KeyDown);
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.buttonLogIn.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonLogIn.BorderRadius = 20;
            this.buttonLogIn.BorderSize = 0;
            this.buttonLogIn.FlatAppearance.BorderSize = 0;
            this.buttonLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLogIn.ForeColor = System.Drawing.Color.White;
            this.buttonLogIn.Location = new System.Drawing.Point(33, 240);
            this.buttonLogIn.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(333, 49);
            this.buttonLogIn.TabIndex = 3;
            this.buttonLogIn.Text = "LOG IN";
            this.buttonLogIn.UseVisualStyleBackColor = false;
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(54)))), ((int)(((byte)(41)))));
            this.buttonExit.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonExit.BorderRadius = 20;
            this.buttonExit.BorderSize = 0;
            this.buttonExit.FlatAppearance.BorderSize = 0;
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.ForeColor = System.Drawing.Color.White;
            this.buttonExit.Location = new System.Drawing.Point(33, 492);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(333, 49);
            this.buttonExit.TabIndex = 5;
            this.buttonExit.Text = "EXIT";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // timerDisconnect
            // 
            this.timerDisconnect.Interval = 19000;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 615);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.labelTittle);
            this.Controls.Add(this.buttonSignUp);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.buttonLogIn);
            this.Controls.Add(this.buttonExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.CustomButton buttonExit;
        private CustomControls.CustomButton buttonLogIn;
        private CustomControls.CustomTextBox textBoxUserName;
        private CustomControls.CustomTextBox textBoxPassword;
        private CustomControls.CustomButton buttonSignUp;
        private System.Windows.Forms.Label labelTittle;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Timer timerClosing;
        private System.Windows.Forms.Timer timerDisconnect;
    }
}
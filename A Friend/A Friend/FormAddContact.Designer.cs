
namespace A_Friend
{
    partial class FormAddContact
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
            this.labelWarning = new System.Windows.Forms.Label();
            this.txtNewUser = new A_Friend.CustomControls.CustomTextBox();
            this.ButtonAdd = new A_Friend.CustomControls.CustomButton();
            this.SuspendLayout();
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.BackColor = System.Drawing.SystemColors.Window;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(8, 43);
            this.labelWarning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(167, 15);
            this.labelWarning.TabIndex = 5;
            this.labelWarning.Text = "This username does not exist";
            // 
            // txtNewUser
            // 
            this.txtNewUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtNewUser.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtNewUser.BorderFocusColor = System.Drawing.Color.HotPink;
            this.txtNewUser.BorderRadius = 0;
            this.txtNewUser.BorderSize = 3;
            this.txtNewUser.Location = new System.Drawing.Point(4, 12);
            this.txtNewUser.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtNewUser.Multiline = false;
            this.txtNewUser.Name = "txtNewUser";
            this.txtNewUser.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.txtNewUser.PasswordChar = false;
            this.txtNewUser.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txtNewUser.PlaceholderText = "";
            this.txtNewUser.Size = new System.Drawing.Size(155, 30);
            this.txtNewUser.TabIndex = 1;
            this.txtNewUser.Texts = "";
            this.txtNewUser.UnderlinedStyle = false;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.BackColor = System.Drawing.SystemColors.Window;
            this.ButtonAdd.BackgroundImage = global::A_Friend.Properties.Resources.plus_circle_6;
            this.ButtonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ButtonAdd.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonAdd.BorderRadius = 0;
            this.ButtonAdd.BorderSize = 0;
            this.ButtonAdd.FlatAppearance.BorderSize = 0;
            this.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ButtonAdd.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.Location = new System.Drawing.Point(164, 12);
            this.ButtonAdd.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(34, 31);
            this.ButtonAdd.TabIndex = 3;
            this.ButtonAdd.UseVisualStyleBackColor = false;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // FormAddContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(202, 63);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.txtNewUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormAddContact";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAddContact";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CustomControls.CustomTextBox txtNewUser;
        private CustomControls.CustomButton ButtonAdd;
        private System.Windows.Forms.Label labelWarning;
    }
}
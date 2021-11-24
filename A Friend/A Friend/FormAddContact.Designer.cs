
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
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.txtNewUser = new A_Friend.CustomControls.CustomTextBox();
            this.SuspendLayout();
            // 
            // labelWarning
            // 
            this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWarning.BackColor = System.Drawing.SystemColors.Window;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(0, 4);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(295, 23);
            this.labelWarning.TabIndex = 5;
            this.labelWarning.Text = "This username does not exist";
            this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.BackgroundImage = global::A_Friend.Properties.Resources.plus_circle_6;
            this.ButtonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAdd.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.Location = new System.Drawing.Point(243, 28);
            this.ButtonAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(45, 46);
            this.ButtonAdd.TabIndex = 6;
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // txtNewUser
            // 
            this.txtNewUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtNewUser.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtNewUser.BorderFocusColor = System.Drawing.Color.HotPink;
            this.txtNewUser.BorderRadius = 30;
            this.txtNewUser.BorderSize = 3;
            this.txtNewUser.Location = new System.Drawing.Point(6, 29);
            this.txtNewUser.Margin = new System.Windows.Forms.Padding(8);
            this.txtNewUser.Multiline = false;
            this.txtNewUser.Name = "txtNewUser";
            this.txtNewUser.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.txtNewUser.PasswordChar = false;
            this.txtNewUser.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txtNewUser.PlaceholderText = "";
            this.txtNewUser.Size = new System.Drawing.Size(232, 45);
            this.txtNewUser.TabIndex = 0;
            this.txtNewUser.Texts = "";
            this.txtNewUser.UnderlinedStyle = false;
            this.txtNewUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewUser_KeyDown);
            // 
            // FormAddContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(297, 78);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.txtNewUser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAddContact";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAddContact";
            this.Shown += new System.EventHandler(this.FormAddContact_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormAddContact_Paint);
            this.ResumeLayout(false);

        }

        #endregion
        private CustomControls.CustomTextBox txtNewUser;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Button ButtonAdd;
    }
}
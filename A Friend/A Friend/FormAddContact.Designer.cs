
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ButtonClose = new A_Friend.CustomControls.CustomButton();
            this.ButtonAdd = new A_Friend.CustomControls.CustomButton();
            this.txtNewUser = new A_Friend.CustomControls.CustomTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(53, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.labelWarning.ForeColor = System.Drawing.Color.Red;
            this.labelWarning.Location = new System.Drawing.Point(53, 142);
            this.labelWarning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(193, 17);
            this.labelWarning.TabIndex = 5;
            this.labelWarning.Text = "This username does not exist";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(74, 35);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "Add Contact\r\n";
            // 
            // ButtonClose
            // 
            this.ButtonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ButtonClose.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonClose.BorderRadius = 0;
            this.ButtonClose.BorderSize = 0;
            this.ButtonClose.FlatAppearance.BorderSize = 0;
            this.ButtonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ButtonClose.ForeColor = System.Drawing.Color.White;
            this.ButtonClose.Location = new System.Drawing.Point(164, 187);
            this.ButtonClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(62, 32);
            this.ButtonClose.TabIndex = 4;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = false;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.BackColor = System.Drawing.Color.SteelBlue;
            this.ButtonAdd.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.ButtonAdd.BorderRadius = 0;
            this.ButtonAdd.BorderSize = 0;
            this.ButtonAdd.FlatAppearance.BorderSize = 0;
            this.ButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ButtonAdd.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.Location = new System.Drawing.Point(56, 186);
            this.ButtonAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(62, 33);
            this.ButtonAdd.TabIndex = 3;
            this.ButtonAdd.Text = "Add";
            this.ButtonAdd.UseVisualStyleBackColor = false;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // txtNewUser
            // 
            this.txtNewUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtNewUser.BorderColor = System.Drawing.Color.SteelBlue;
            this.txtNewUser.BorderFocusColor = System.Drawing.Color.HotPink;
            this.txtNewUser.BorderRadius = 0;
            this.txtNewUser.BorderSize = 2;
            this.txtNewUser.Location = new System.Drawing.Point(57, 109);
            this.txtNewUser.Multiline = false;
            this.txtNewUser.Name = "txtNewUser";
            this.txtNewUser.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.txtNewUser.PasswordChar = false;
            this.txtNewUser.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.txtNewUser.PlaceholderText = "";
            this.txtNewUser.Size = new System.Drawing.Size(169, 30);
            this.txtNewUser.TabIndex = 1;
            this.txtNewUser.Texts = "";
            this.txtNewUser.UnderlinedStyle = false;
            // 
            // FormAddContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 266);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.txtNewUser);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximumSize = new System.Drawing.Size(298, 305);
            this.MinimumSize = new System.Drawing.Size(298, 305);
            this.Name = "FormAddContact";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAddContact";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CustomControls.CustomTextBox txtNewUser;
        private CustomControls.CustomButton ButtonAdd;
        private CustomControls.CustomButton ButtonClose;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Label label3;
    }
}
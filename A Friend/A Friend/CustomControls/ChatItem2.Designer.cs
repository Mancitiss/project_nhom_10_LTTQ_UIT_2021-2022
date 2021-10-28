
namespace A_Friend.CustomControls
{
    partial class ChatItem2
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
            this.panelBody = new A_Friend.CustomControls.MessagePanel();
            this.labelBody = new System.Windows.Forms.Label();
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonCopy = new A_Friend.CustomControls.CustomButton();
            this.buttonRemove = new A_Friend.CustomControls.CustomButton();
            this.panelBody.SuspendLayout();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.panelBody.Controls.Add(this.labelBody);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelBody.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBody.Location = new System.Drawing.Point(15, 0);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(226, 118);
            this.panelBody.TabIndex = 1;
            // 
            // labelBody
            // 
            this.labelBody.AutoSize = true;
            this.labelBody.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelBody.Location = new System.Drawing.Point(10, 10);
            this.labelBody.Margin = new System.Windows.Forms.Padding(0);
            this.labelBody.Name = "labelBody";
            this.labelBody.Size = new System.Drawing.Size(53, 20);
            this.labelBody.TabIndex = 0;
            this.labelBody.Text = "label1";
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonRemove);
            this.panelButton.Controls.Add(this.buttonCopy);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelButton.Location = new System.Drawing.Point(241, 0);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(75, 118);
            this.panelButton.TabIndex = 2;
            // 
            // buttonCopy
            // 
            this.buttonCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.buttonCopy.BackgroundImage = global::A_Friend.Properties.Resources.copy_regular;
            this.buttonCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCopy.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonCopy.BorderRadius = 5;
            this.buttonCopy.BorderSize = 0;
            this.buttonCopy.FlatAppearance.BorderSize = 0;
            this.buttonCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopy.ForeColor = System.Drawing.Color.White;
            this.buttonCopy.Location = new System.Drawing.Point(5, 47);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(30, 30);
            this.buttonCopy.TabIndex = 0;
            this.buttonCopy.UseVisualStyleBackColor = false;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.buttonRemove.BackgroundImage = global::A_Friend.Properties.Resources.trash_alt_regular;
            this.buttonRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRemove.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.buttonRemove.BorderRadius = 5;
            this.buttonRemove.BorderSize = 0;
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.ForeColor = System.Drawing.Color.White;
            this.buttonRemove.Location = new System.Drawing.Point(40, 47);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(30, 30);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // ChatItem2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.panelBody);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ChatItem2";
            this.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.Size = new System.Drawing.Size(813, 118);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBody;
        private MessagePanel panelBody;
        private System.Windows.Forms.Panel panelButton;
        private CustomButton buttonRemove;
        private CustomButton buttonCopy;
    }
}

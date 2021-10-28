
namespace A_Friend.CustomControls
{
    partial class ChatItem
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
            this.panelButton = new System.Windows.Forms.Panel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.toolTip_DeleteM = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip_Copy = new System.Windows.Forms.ToolTip(this.components);
            this.panelBody = new A_Friend.CustomControls.MessagePanel();
            this.textBoxBody = new System.Windows.Forms.TextBox();
            this.panelButton.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonRemove);
            this.panelButton.Controls.Add(this.buttonCopy);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelButton.Location = new System.Drawing.Point(315, 5);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(80, 100);
            this.panelButton.TabIndex = 2;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonRemove.BackgroundImage = global::A_Friend.Properties.Resources.trash_alt_regular;
            this.buttonRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.Location = new System.Drawing.Point(40, 40);
            this.buttonRemove.MaximumSize = new System.Drawing.Size(30, 30);
            this.buttonRemove.MinimumSize = new System.Drawing.Size(30, 30);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(30, 30);
            this.buttonRemove.TabIndex = 1;
            this.toolTip_DeleteM.SetToolTip(this.buttonRemove, "Delete message");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonCopy.BackgroundImage = global::A_Friend.Properties.Resources.copy_regular;
            this.buttonCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCopy.FlatAppearance.BorderSize = 0;
            this.buttonCopy.Location = new System.Drawing.Point(10, 40);
            this.buttonCopy.MaximumSize = new System.Drawing.Size(30, 30);
            this.buttonCopy.MinimumSize = new System.Drawing.Size(30, 30);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(30, 30);
            this.buttonCopy.TabIndex = 0;
            this.toolTip_Copy.SetToolTip(this.buttonCopy, "Copy");
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // toolTip_DeleteM
            // 
            this.toolTip_DeleteM.AutoPopDelay = 5000;
            this.toolTip_DeleteM.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTip_DeleteM.InitialDelay = 0;
            this.toolTip_DeleteM.ReshowDelay = 100;
            // 
            // toolTip_Copy
            // 
            this.toolTip_Copy.AutoPopDelay = 5000;
            this.toolTip_Copy.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTip_Copy.InitialDelay = 0;
            this.toolTip_Copy.ReshowDelay = 100;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.panelBody.Controls.Add(this.textBoxBody);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelBody.Location = new System.Drawing.Point(15, 5);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(300, 100);
            this.panelBody.TabIndex = 1;
            // 
            // textBoxBody
            // 
            this.textBoxBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.textBoxBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxBody.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.textBoxBody.Location = new System.Drawing.Point(10, 10);
            this.textBoxBody.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxBody.Multiline = true;
            this.textBoxBody.Name = "textBoxBody";
            this.textBoxBody.ReadOnly = true;
            this.textBoxBody.Size = new System.Drawing.Size(280, 80);
            this.textBoxBody.TabIndex = 0;
            // 
            // ChatItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.panelBody);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ChatItem";
            this.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.Size = new System.Drawing.Size(600, 110);
            this.panelButton.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxBody;
        private MessagePanel panelBody;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ToolTip toolTip_DeleteM;
        private System.Windows.Forms.ToolTip toolTip_Copy;
    }
}

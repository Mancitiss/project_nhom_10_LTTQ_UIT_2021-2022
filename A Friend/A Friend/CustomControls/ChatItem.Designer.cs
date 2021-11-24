﻿
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
            this.labelAuthor = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBody = new A_Friend.CustomControls.MessagePanel();
            this.labelBody = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelButton.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.buttonRemove);
            this.panelButton.Controls.Add(this.buttonCopy);
            this.panelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelButton.Location = new System.Drawing.Point(300, 0);
            this.panelButton.Margin = new System.Windows.Forms.Padding(0);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(80, 132);
            this.panelButton.TabIndex = 2;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonRemove.BackgroundImage = global::A_Friend.Properties.Resources.trash_alt_regular;
            this.buttonRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.Location = new System.Drawing.Point(40, 58);
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
            this.buttonCopy.Location = new System.Drawing.Point(4, 58);
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
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAuthor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAuthor.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAuthor.Location = new System.Drawing.Point(10, 5);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(62, 23);
            this.labelAuthor.TabIndex = 3;
            this.labelAuthor.Text = "label1";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelButton);
            this.panelTop.Controls.Add(this.panelBody);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(15, 5);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(743, 132);
            this.panelTop.TabIndex = 4;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(161)))), ((int)(((byte)(252)))));
            this.panelBody.Controls.Add(this.labelBody);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Margin = new System.Windows.Forms.Padding(0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5);
            this.panelBody.Size = new System.Drawing.Size(300, 132);
            this.panelBody.TabIndex = 1;
            // 
            // labelBody
            // 
            this.labelBody.AutoSize = true;
            this.labelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBody.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBody.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelBody.Location = new System.Drawing.Point(5, 5);
            this.labelBody.Name = "labelBody";
            this.labelBody.Padding = new System.Windows.Forms.Padding(5);
            this.labelBody.Size = new System.Drawing.Size(106, 33);
            this.labelBody.TabIndex = 0;
            this.labelBody.Text = "labelBody";
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.labelAuthor);
            this.panelBottom.Location = new System.Drawing.Point(15, 137);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panelBottom.Size = new System.Drawing.Size(743, 23);
            this.panelBottom.TabIndex = 5;
            // 
            // ChatItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ChatItem";
            this.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.Size = new System.Drawing.Size(773, 160);
            this.Load += new System.EventHandler(this.ChatItem_Load);
            this.panelButton.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private MessagePanel panelBody;
        private System.Windows.Forms.Panel panelButton;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ToolTip toolTip_DeleteM;
        private System.Windows.Forms.ToolTip toolTip_Copy;
        private System.Windows.Forms.Label labelBody;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
    }
}

﻿
using System;
using System.Windows.Forms;

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
            this.toolTip_Send = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelBottomLeft = new System.Windows.Forms.Panel();
            this.ButtonAdd = new A_Friend.CustomControls.CustomButton();
            this.SettingButton = new A_Friend.CustomControls.CustomButton();
            this.LogoutButton = new A_Friend.CustomControls.CustomButton();
            this.panelContact = new System.Windows.Forms.Panel();
            this.panelTopLeft = new System.Windows.Forms.Panel();
            this.customTextBoxSearch = new A_Friend.CustomControls.CustomTextBox();
            this.labelWarning = new System.Windows.Forms.Label();
            this.notifyIconApp = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft.SuspendLayout();
            this.panelBottomLeft.SuspendLayout();
            this.panelTopLeft.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip_DeleteC
            // 
            this.toolTip_DeleteC.AutoPopDelay = 5000;
            this.toolTip_DeleteC.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTip_DeleteC.InitialDelay = 1000;
            this.toolTip_DeleteC.ReshowDelay = 100;
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
            this.panelLeft.BackColor = System.Drawing.Color.White;
            this.panelLeft.Controls.Add(this.panelBottomLeft);
            this.panelLeft.Controls.Add(this.panelContact);
            this.panelLeft.Controls.Add(this.panelTopLeft);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.MaximumSize = new System.Drawing.Size(300, 10000);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(300, 712);
            this.panelLeft.TabIndex = 4;
            // 
            // panelBottomLeft
            // 
            this.panelBottomLeft.BackColor = System.Drawing.SystemColors.Window;
            this.panelBottomLeft.Controls.Add(this.ButtonAdd);
            this.panelBottomLeft.Controls.Add(this.SettingButton);
            this.panelBottomLeft.Controls.Add(this.LogoutButton);
            this.panelBottomLeft.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomLeft.Location = new System.Drawing.Point(0, 652);
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
            this.LogoutButton.Click += new System.EventHandler(this.LogoutButton_Click_1);
            // 
            // panelContact
            // 
            this.panelContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContact.AutoScroll = true;
            this.panelContact.BackColor = System.Drawing.SystemColors.Window;
            this.panelContact.Location = new System.Drawing.Point(2, 60);
            this.panelContact.Name = "panelContact";
            this.panelContact.Size = new System.Drawing.Size(298, 592);
            this.panelContact.TabIndex = 2;
            this.panelContact.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.panelContact_ControlAdded);
            // 
            // panelTopLeft
            // 
            this.panelTopLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.panelTopLeft.Controls.Add(this.customTextBoxSearch);
            this.panelTopLeft.Location = new System.Drawing.Point(0, 0);
            this.panelTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopLeft.Name = "panelTopLeft";
            this.panelTopLeft.Size = new System.Drawing.Size(300, 60);
            this.panelTopLeft.TabIndex = 0;
            // 
            // customTextBoxSearch
            // 
            this.customTextBoxSearch.BackColor = System.Drawing.SystemColors.Window;
            this.customTextBoxSearch.BorderColor = System.Drawing.SystemColors.Window;
            this.customTextBoxSearch.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.customTextBoxSearch.BorderRadius = 20;
            this.customTextBoxSearch.BorderSize = 2;
            this.customTextBoxSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.customTextBoxSearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTextBoxSearch.Location = new System.Drawing.Point(13, 7);
            this.customTextBoxSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.customTextBoxSearch.Multiline = false;
            this.customTextBoxSearch.Name = "customTextBoxSearch";
            this.customTextBoxSearch.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.customTextBoxSearch.PasswordChar = false;
            this.customTextBoxSearch.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.customTextBoxSearch.PlaceholderText = "Search";
            this.customTextBoxSearch.Size = new System.Drawing.Size(258, 43);
            this.customTextBoxSearch.TabIndex = 0;
            this.customTextBoxSearch.Texts = "";
            this.customTextBoxSearch.UnderlinedStyle = false;
            this.customTextBoxSearch._TextChanged += new System.EventHandler(this.customTextBoxSearch__TextChanged);
            this.customTextBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.customTextBoxSearch_KeyDown);
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
            // notifyIconApp
            // 
            this.notifyIconApp.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIconApp.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconApp.Icon")));
            this.notifyIconApp.Text = "notifyIcon1";
            this.notifyIconApp.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconApp_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(115, 28);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeMessengerToolStripMenuItem_Click);
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Location = new System.Drawing.Point(300, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(915, 712);
            this.panelRight.TabIndex = 3;
            // 
            // FormApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 712);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "FormApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormApplication";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApplication_FormClosing);
            this.Load += new System.EventHandler(this.FormApplication_Load);
            this.panelLeft.ResumeLayout(false);
            this.panelBottomLeft.ResumeLayout(false);
            this.panelTopLeft.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip_DeleteC;
        private System.Windows.Forms.ToolTip toolTip_Send;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelTopLeft;
        private CustomControls.CustomTextBox customTextBoxSearch;
        private System.Windows.Forms.Panel panelBottomLeft;
        private CustomControls.CustomButton LogoutButton;
        private CustomControls.CustomButton SettingButton;
        private CustomControls.CustomButton ButtonAdd;
        private System.Windows.Forms.Panel panelContact;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.NotifyIcon notifyIconApp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private Panel panelRight;
    }
}
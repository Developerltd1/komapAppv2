﻿namespace KomaxApp.UI_Design
{
    partial class ParentForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.sidebarPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnBtnDashboard = new FontAwesome.Sharp.IconButton();
            this.pnBtnMotorTestingCreate = new FontAwesome.Sharp.IconButton();
            this.iconButtonDisplay = new FontAwesome.Sharp.IconButton();
            this.iconButtonReport = new FontAwesome.Sharp.IconButton();
            this.pnBtnLogout = new FontAwesome.Sharp.IconButton();
            this.panel1.SuspendLayout();
            this.sidebarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1739, 49);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(39, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "KomaxApp v.1";
            // 
            // sidebarPanel
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.sidebarPanel.Controls.Add(this.pnBtnDashboard);
            this.sidebarPanel.Controls.Add(this.pnBtnMotorTestingCreate);
            this.sidebarPanel.Controls.Add(this.iconButtonDisplay);
            this.sidebarPanel.Controls.Add(this.iconButtonReport);
            this.sidebarPanel.Controls.Add(this.pnBtnLogout);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 49);
            this.sidebarPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(221, 814);
            this.sidebarPanel.TabIndex = 2;
            // 
            // pnBtnDashboard
            // 
            this.pnBtnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnBtnDashboard.FlatAppearance.BorderSize = 0;
            this.pnBtnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnBtnDashboard.ForeColor = System.Drawing.Color.Black;
            this.pnBtnDashboard.IconChar = FontAwesome.Sharp.IconChar.Th;
            this.pnBtnDashboard.IconColor = System.Drawing.Color.Black;
            this.pnBtnDashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.pnBtnDashboard.IconSize = 32;
            this.pnBtnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnBtnDashboard.Location = new System.Drawing.Point(3, 2);
            this.pnBtnDashboard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnBtnDashboard.Name = "pnBtnDashboard";
            this.pnBtnDashboard.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnBtnDashboard.Size = new System.Drawing.Size(239, 59);
            this.pnBtnDashboard.TabIndex = 12;
            this.pnBtnDashboard.Text = "Dashboard";
            this.pnBtnDashboard.UseVisualStyleBackColor = false;
            this.pnBtnDashboard.Click += new System.EventHandler(this.pnBtnDashboard_Click);
            // 
            // pnBtnMotorTestingCreate
            // 
            this.pnBtnMotorTestingCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnBtnMotorTestingCreate.FlatAppearance.BorderSize = 0;
            this.pnBtnMotorTestingCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnBtnMotorTestingCreate.ForeColor = System.Drawing.Color.Black;
            this.pnBtnMotorTestingCreate.IconChar = FontAwesome.Sharp.IconChar.StickyNote;
            this.pnBtnMotorTestingCreate.IconColor = System.Drawing.Color.Black;
            this.pnBtnMotorTestingCreate.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.pnBtnMotorTestingCreate.IconSize = 32;
            this.pnBtnMotorTestingCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnBtnMotorTestingCreate.Location = new System.Drawing.Point(3, 65);
            this.pnBtnMotorTestingCreate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnBtnMotorTestingCreate.Name = "pnBtnMotorTestingCreate";
            this.pnBtnMotorTestingCreate.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnBtnMotorTestingCreate.Size = new System.Drawing.Size(239, 59);
            this.pnBtnMotorTestingCreate.TabIndex = 15;
            this.pnBtnMotorTestingCreate.Text = "Create";
            this.pnBtnMotorTestingCreate.UseVisualStyleBackColor = false;
            this.pnBtnMotorTestingCreate.Click += new System.EventHandler(this.pnBtnMotorTestingCreate_Click);
            // 
            // iconButtonDisplay
            // 
            this.iconButtonDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.iconButtonDisplay.FlatAppearance.BorderSize = 0;
            this.iconButtonDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonDisplay.ForeColor = System.Drawing.Color.Black;
            this.iconButtonDisplay.IconChar = FontAwesome.Sharp.IconChar.Stream;
            this.iconButtonDisplay.IconColor = System.Drawing.Color.Black;
            this.iconButtonDisplay.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonDisplay.IconSize = 32;
            this.iconButtonDisplay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonDisplay.Location = new System.Drawing.Point(3, 128);
            this.iconButtonDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconButtonDisplay.Name = "iconButtonDisplay";
            this.iconButtonDisplay.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.iconButtonDisplay.Size = new System.Drawing.Size(239, 59);
            this.iconButtonDisplay.TabIndex = 17;
            this.iconButtonDisplay.Text = "Display Record";
            this.iconButtonDisplay.UseVisualStyleBackColor = false;
            this.iconButtonDisplay.Click += new System.EventHandler(this.iconButtonDisplay_Click);
            // 
            // iconButtonReport
            // 
            this.iconButtonReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.iconButtonReport.FlatAppearance.BorderSize = 0;
            this.iconButtonReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonReport.ForeColor = System.Drawing.Color.Black;
            this.iconButtonReport.IconChar = FontAwesome.Sharp.IconChar.Twitch;
            this.iconButtonReport.IconColor = System.Drawing.Color.Black;
            this.iconButtonReport.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButtonReport.IconSize = 32;
            this.iconButtonReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonReport.Location = new System.Drawing.Point(3, 191);
            this.iconButtonReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconButtonReport.Name = "iconButtonReport";
            this.iconButtonReport.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.iconButtonReport.Size = new System.Drawing.Size(239, 59);
            this.iconButtonReport.TabIndex = 18;
            this.iconButtonReport.Text = "Report";
            this.iconButtonReport.UseVisualStyleBackColor = false;
            this.iconButtonReport.Click += new System.EventHandler(this.iconButtonReport_Click);
            // 
            // pnBtnLogout
            // 
            this.pnBtnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.pnBtnLogout.FlatAppearance.BorderSize = 0;
            this.pnBtnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnBtnLogout.ForeColor = System.Drawing.Color.Black;
            this.pnBtnLogout.IconChar = FontAwesome.Sharp.IconChar.Lock;
            this.pnBtnLogout.IconColor = System.Drawing.Color.Black;
            this.pnBtnLogout.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.pnBtnLogout.IconSize = 32;
            this.pnBtnLogout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnBtnLogout.Location = new System.Drawing.Point(3, 254);
            this.pnBtnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnBtnLogout.Name = "pnBtnLogout";
            this.pnBtnLogout.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnBtnLogout.Size = new System.Drawing.Size(239, 59);
            this.pnBtnLogout.TabIndex = 16;
            this.pnBtnLogout.Text = "Configuration";
            this.pnBtnLogout.UseVisualStyleBackColor = false;
            this.pnBtnLogout.Click += new System.EventHandler(this.pnBtnLogout_Click);
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1739, 863);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.panel1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ParentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ParentForm";
            this.Load += new System.EventHandler(this.ParentForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sidebarPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel sidebarPanel;
        private FontAwesome.Sharp.IconButton pnBtnDashboard;
        private FontAwesome.Sharp.IconButton pnBtnMotorTestingCreate;
        private FontAwesome.Sharp.IconButton pnBtnLogout;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconButton iconButtonDisplay;
        private FontAwesome.Sharp.IconButton iconButtonReport;
    }
}
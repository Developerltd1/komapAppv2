namespace KomaxApp.UI_Design
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParentForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStartReadng = new System.Windows.Forms.Button();
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
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnStartReadng);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1304, 40);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.Brown;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1202, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 60;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStartReadng
            // 
            this.btnStartReadng.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStartReadng.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(99)))));
            this.btnStartReadng.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartReadng.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartReadng.ForeColor = System.Drawing.Color.White;
            this.btnStartReadng.Location = new System.Drawing.Point(1106, 5);
            this.btnStartReadng.Name = "btnStartReadng";
            this.btnStartReadng.Size = new System.Drawing.Size(90, 28);
            this.btnStartReadng.TabIndex = 59;
            this.btnStartReadng.Text = "Open";
            this.btnStartReadng.UseVisualStyleBackColor = false;
            this.btnStartReadng.Click += new System.EventHandler(this.btnStartReadng_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 19);
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
            this.sidebarPanel.Location = new System.Drawing.Point(0, 40);
            this.sidebarPanel.Margin = new System.Windows.Forms.Padding(2);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(166, 661);
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
            this.pnBtnDashboard.Location = new System.Drawing.Point(2, 2);
            this.pnBtnDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.pnBtnDashboard.Name = "pnBtnDashboard";
            this.pnBtnDashboard.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.pnBtnDashboard.Size = new System.Drawing.Size(179, 48);
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
            this.pnBtnMotorTestingCreate.Location = new System.Drawing.Point(2, 54);
            this.pnBtnMotorTestingCreate.Margin = new System.Windows.Forms.Padding(2);
            this.pnBtnMotorTestingCreate.Name = "pnBtnMotorTestingCreate";
            this.pnBtnMotorTestingCreate.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.pnBtnMotorTestingCreate.Size = new System.Drawing.Size(179, 48);
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
            this.iconButtonDisplay.Location = new System.Drawing.Point(2, 106);
            this.iconButtonDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.iconButtonDisplay.Name = "iconButtonDisplay";
            this.iconButtonDisplay.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.iconButtonDisplay.Size = new System.Drawing.Size(179, 48);
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
            this.iconButtonReport.Location = new System.Drawing.Point(2, 158);
            this.iconButtonReport.Margin = new System.Windows.Forms.Padding(2);
            this.iconButtonReport.Name = "iconButtonReport";
            this.iconButtonReport.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.iconButtonReport.Size = new System.Drawing.Size(179, 48);
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
            this.pnBtnLogout.Location = new System.Drawing.Point(2, 210);
            this.pnBtnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.pnBtnLogout.Name = "pnBtnLogout";
            this.pnBtnLogout.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.pnBtnLogout.Size = new System.Drawing.Size(179, 48);
            this.pnBtnLogout.TabIndex = 16;
            this.pnBtnLogout.Text = "Configuration";
            this.pnBtnLogout.UseVisualStyleBackColor = false;
            this.pnBtnLogout.Click += new System.EventHandler(this.pnBtnLogout_Click);
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1304, 701);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
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
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStartReadng;
    }
}
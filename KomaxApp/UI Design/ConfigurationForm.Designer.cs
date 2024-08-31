namespace KomaxApp.UI_Design
{
    partial class ConfigurationForm
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
            this.cbPowerMeter = new System.Windows.Forms.ComboBox();
            this.cbTorqueMeter = new System.Windows.Forms.ComboBox();
            this.cbRPM = new System.Windows.Forms.ComboBox();
            this.cbTemperature = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnComportRefresh = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.infoMessages = new System.Windows.Forms.TextBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbPowerMeter
            // 
            this.cbPowerMeter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbPowerMeter.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.cbPowerMeter.FormattingEnabled = true;
            this.cbPowerMeter.Location = new System.Drawing.Point(379, 208);
            this.cbPowerMeter.Name = "cbPowerMeter";
            this.cbPowerMeter.Size = new System.Drawing.Size(210, 38);
            this.cbPowerMeter.TabIndex = 55;
            // 
            // cbTorqueMeter
            // 
            this.cbTorqueMeter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbTorqueMeter.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.cbTorqueMeter.FormattingEnabled = true;
            this.cbTorqueMeter.Location = new System.Drawing.Point(379, 263);
            this.cbTorqueMeter.Name = "cbTorqueMeter";
            this.cbTorqueMeter.Size = new System.Drawing.Size(210, 38);
            this.cbTorqueMeter.TabIndex = 56;
            // 
            // cbRPM
            // 
            this.cbRPM.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbRPM.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.cbRPM.FormattingEnabled = true;
            this.cbRPM.Location = new System.Drawing.Point(379, 318);
            this.cbRPM.Name = "cbRPM";
            this.cbRPM.Size = new System.Drawing.Size(210, 38);
            this.cbRPM.TabIndex = 57;
            // 
            // cbTemperature
            // 
            this.cbTemperature.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbTemperature.Font = new System.Drawing.Font("Segoe UI", 15.75F);
            this.cbTemperature.FormattingEnabled = true;
            this.cbTemperature.Location = new System.Drawing.Point(379, 374);
            this.cbTemperature.Name = "cbTemperature";
            this.cbTemperature.Size = new System.Drawing.Size(210, 38);
            this.cbTemperature.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(281, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 21);
            this.label3.TabIndex = 59;
            this.label3.Text = "Power Meter";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(271, 273);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 21);
            this.label1.TabIndex = 60;
            this.label1.Text = "Torque Meter";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(330, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 21);
            this.label2.TabIndex = 61;
            this.label2.Text = "RPM";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(290, 382);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 21);
            this.label4.TabIndex = 62;
            this.label4.Text = "Tempreture";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(66)))));
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveSettings.Location = new System.Drawing.Point(639, 295);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(198, 52);
            this.btnSaveSettings.TabIndex = 63;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnComportRefresh
            // 
            this.btnComportRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnComportRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnComportRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComportRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComportRefresh.ForeColor = System.Drawing.Color.White;
            this.btnComportRefresh.Location = new System.Drawing.Point(639, 361);
            this.btnComportRefresh.Name = "btnComportRefresh";
            this.btnComportRefresh.Size = new System.Drawing.Size(198, 52);
            this.btnComportRefresh.TabIndex = 64;
            this.btnComportRefresh.Text = "Comport Refresh";
            this.btnComportRefresh.UseVisualStyleBackColor = false;
            this.btnComportRefresh.Click += new System.EventHandler(this.btnComportRefresh_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label15.Location = new System.Drawing.Point(33, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(273, 37);
            this.label15.TabIndex = 65;
            this.label15.Text = "Configuration Panel";
            // 
            // infoMessages
            // 
            this.infoMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoMessages.Enabled = false;
            this.infoMessages.Location = new System.Drawing.Point(5, 599);
            this.infoMessages.Name = "infoMessages";
            this.infoMessages.Size = new System.Drawing.Size(1117, 20);
            this.infoMessages.TabIndex = 66;
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfo.AutoSize = true;
            this.labelInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo.Location = new System.Drawing.Point(10, 602);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(0, 13);
            this.labelInfo.TabIndex = 67;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 621);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.infoMessages);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnComportRefresh);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbTemperature);
            this.Controls.Add(this.cbRPM);
            this.Controls.Add(this.cbTorqueMeter);
            this.Controls.Add(this.cbPowerMeter);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ConfigurationForm";
            this.Text = "s";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPowerMeter;
        private System.Windows.Forms.ComboBox cbTorqueMeter;
        private System.Windows.Forms.ComboBox cbRPM;
        private System.Windows.Forms.ComboBox cbTemperature;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnComportRefresh;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox infoMessages;
        private System.Windows.Forms.Label labelInfo;
    }
}
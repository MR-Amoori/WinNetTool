namespace Win_Net_Tool
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnIpConfig = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.btnFlushDns = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnResetNetwork = new System.Windows.Forms.Button();
            this.btnInternetOptions = new System.Windows.Forms.Button();
            this.btnDisableLanSettings = new System.Windows.Forms.Button();
            this.btnSetAllAdaptersDhcp = new System.Windows.Forms.Button();
            this.txtPingTarget = new System.Windows.Forms.TextBox();
            this.btnPing = new System.Windows.Forms.Button();
            this.lblPingStatus = new System.Windows.Forms.Label();
            this.btnRemoveSystemProxy = new System.Windows.Forms.Button();
            this.txtPrimaryDns = new System.Windows.Forms.TextBox();
            this.txtSecondaryDns = new System.Windows.Forms.TextBox();
            this.btnApplyCustomDns = new System.Windows.Forms.Button();
            this.cmbDnsServers = new System.Windows.Forms.ComboBox();
            this.btnApplySelectedDns = new System.Windows.Forms.Button();
            this.btnPingAllDns = new System.Windows.Forms.Button();
            this.btnChangeAdapterSettings = new System.Windows.Forms.Button();
            this.btnDeveloperWebsite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnIpConfig
            // 
            this.btnIpConfig.BackColor = System.Drawing.Color.LightBlue;
            this.btnIpConfig.Location = new System.Drawing.Point(9, 12);
            this.btnIpConfig.Name = "btnIpConfig";
            this.btnIpConfig.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnIpConfig.Size = new System.Drawing.Size(176, 40);
            this.btnIpConfig.TabIndex = 0;
            this.btnIpConfig.Text = "دریافت اطلاعات IP";
            this.btnIpConfig.UseVisualStyleBackColor = false;
            this.btnIpConfig.Click += new System.EventHandler(this.btnIpConfig_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblStatus.Location = new System.Drawing.Point(6, 241);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "وضعیت";
            // 
            // rtbOutput
            // 
            this.rtbOutput.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbOutput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.Location = new System.Drawing.Point(0, 264);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rtbOutput.Size = new System.Drawing.Size(738, 390);
            this.rtbOutput.TabIndex = 18;
            this.rtbOutput.Text = "";
            // 
            // btnFlushDns
            // 
            this.btnFlushDns.BackColor = System.Drawing.Color.LightBlue;
            this.btnFlushDns.Location = new System.Drawing.Point(191, 12);
            this.btnFlushDns.Name = "btnFlushDns";
            this.btnFlushDns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnFlushDns.Size = new System.Drawing.Size(176, 40);
            this.btnFlushDns.TabIndex = 1;
            this.btnFlushDns.Text = "پاک‌سازی DNS Cache";
            this.btnFlushDns.UseVisualStyleBackColor = false;
            this.btnFlushDns.Click += new System.EventHandler(this.btnFlushDns_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Yellow;
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(656, 238);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "پاکسازی";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnResetNetwork
            // 
            this.btnResetNetwork.BackColor = System.Drawing.Color.LightBlue;
            this.btnResetNetwork.Location = new System.Drawing.Point(373, 12);
            this.btnResetNetwork.Name = "btnResetNetwork";
            this.btnResetNetwork.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnResetNetwork.Size = new System.Drawing.Size(176, 40);
            this.btnResetNetwork.TabIndex = 2;
            this.btnResetNetwork.Text = "ریست شبکه";
            this.btnResetNetwork.UseVisualStyleBackColor = false;
            this.btnResetNetwork.Click += new System.EventHandler(this.btnResetNetwork_Click);
            // 
            // btnInternetOptions
            // 
            this.btnInternetOptions.BackColor = System.Drawing.Color.LightBlue;
            this.btnInternetOptions.Location = new System.Drawing.Point(555, 12);
            this.btnInternetOptions.Name = "btnInternetOptions";
            this.btnInternetOptions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnInternetOptions.Size = new System.Drawing.Size(176, 40);
            this.btnInternetOptions.TabIndex = 3;
            this.btnInternetOptions.Text = "اینترنت آپشن";
            this.btnInternetOptions.UseVisualStyleBackColor = false;
            this.btnInternetOptions.Click += new System.EventHandler(this.btnInternetOptions_Click);
            // 
            // btnDisableLanSettings
            // 
            this.btnDisableLanSettings.BackColor = System.Drawing.Color.LightBlue;
            this.btnDisableLanSettings.Location = new System.Drawing.Point(555, 58);
            this.btnDisableLanSettings.Name = "btnDisableLanSettings";
            this.btnDisableLanSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDisableLanSettings.Size = new System.Drawing.Size(176, 40);
            this.btnDisableLanSettings.TabIndex = 7;
            this.btnDisableLanSettings.Text = "حذف لن اینترنت آپشن";
            this.btnDisableLanSettings.UseVisualStyleBackColor = false;
            this.btnDisableLanSettings.Click += new System.EventHandler(this.btnDisableLanSettings_Click);
            // 
            // btnSetAllAdaptersDhcp
            // 
            this.btnSetAllAdaptersDhcp.BackColor = System.Drawing.Color.LightBlue;
            this.btnSetAllAdaptersDhcp.Location = new System.Drawing.Point(373, 58);
            this.btnSetAllAdaptersDhcp.Name = "btnSetAllAdaptersDhcp";
            this.btnSetAllAdaptersDhcp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSetAllAdaptersDhcp.Size = new System.Drawing.Size(176, 40);
            this.btnSetAllAdaptersDhcp.TabIndex = 6;
            this.btnSetAllAdaptersDhcp.Text = "حذف DNS آداپتور ها";
            this.btnSetAllAdaptersDhcp.UseVisualStyleBackColor = false;
            this.btnSetAllAdaptersDhcp.Click += new System.EventHandler(this.btnSetAllAdaptersDhcp_Click);
            // 
            // txtPingTarget
            // 
            this.txtPingTarget.BackColor = System.Drawing.SystemColors.Info;
            this.txtPingTarget.Location = new System.Drawing.Point(12, 58);
            this.txtPingTarget.Name = "txtPingTarget";
            this.txtPingTarget.Size = new System.Drawing.Size(173, 26);
            this.txtPingTarget.TabIndex = 4;
            this.txtPingTarget.Text = "8.8.8.8";
            this.txtPingTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPing
            // 
            this.btnPing.BackColor = System.Drawing.SystemColors.Info;
            this.btnPing.Location = new System.Drawing.Point(191, 58);
            this.btnPing.Name = "btnPing";
            this.btnPing.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnPing.Size = new System.Drawing.Size(176, 40);
            this.btnPing.TabIndex = 5;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = false;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // lblPingStatus
            // 
            this.lblPingStatus.AutoSize = true;
            this.lblPingStatus.BackColor = System.Drawing.SystemColors.Info;
            this.lblPingStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPingStatus.Location = new System.Drawing.Point(12, 90);
            this.lblPingStatus.Name = "lblPingStatus";
            this.lblPingStatus.Size = new System.Drawing.Size(40, 14);
            this.lblPingStatus.TabIndex = 12;
            this.lblPingStatus.Text = "00 ms";
            // 
            // btnRemoveSystemProxy
            // 
            this.btnRemoveSystemProxy.BackColor = System.Drawing.Color.LightBlue;
            this.btnRemoveSystemProxy.Location = new System.Drawing.Point(555, 104);
            this.btnRemoveSystemProxy.Name = "btnRemoveSystemProxy";
            this.btnRemoveSystemProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnRemoveSystemProxy.Size = new System.Drawing.Size(176, 40);
            this.btnRemoveSystemProxy.TabIndex = 11;
            this.btnRemoveSystemProxy.Text = "حذف پروکسی";
            this.btnRemoveSystemProxy.UseVisualStyleBackColor = false;
            this.btnRemoveSystemProxy.Click += new System.EventHandler(this.btnRemoveSystemProxy_Click);
            // 
            // txtPrimaryDns
            // 
            this.txtPrimaryDns.BackColor = System.Drawing.Color.MistyRose;
            this.txtPrimaryDns.Location = new System.Drawing.Point(12, 112);
            this.txtPrimaryDns.Name = "txtPrimaryDns";
            this.txtPrimaryDns.Size = new System.Drawing.Size(173, 26);
            this.txtPrimaryDns.TabIndex = 8;
            this.txtPrimaryDns.Text = "DNS اولیه";
            this.txtPrimaryDns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSecondaryDns
            // 
            this.txtSecondaryDns.BackColor = System.Drawing.Color.MistyRose;
            this.txtSecondaryDns.Location = new System.Drawing.Point(191, 112);
            this.txtSecondaryDns.Name = "txtSecondaryDns";
            this.txtSecondaryDns.Size = new System.Drawing.Size(176, 26);
            this.txtSecondaryDns.TabIndex = 9;
            this.txtSecondaryDns.Text = "DNS ثانویه";
            this.txtSecondaryDns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnApplyCustomDns
            // 
            this.btnApplyCustomDns.BackColor = System.Drawing.Color.MistyRose;
            this.btnApplyCustomDns.Location = new System.Drawing.Point(373, 104);
            this.btnApplyCustomDns.Name = "btnApplyCustomDns";
            this.btnApplyCustomDns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnApplyCustomDns.Size = new System.Drawing.Size(176, 40);
            this.btnApplyCustomDns.TabIndex = 10;
            this.btnApplyCustomDns.Text = "اعمال DNS واردشده";
            this.btnApplyCustomDns.UseVisualStyleBackColor = false;
            this.btnApplyCustomDns.Click += new System.EventHandler(this.btnApplyCustomDns_Click);
            // 
            // cmbDnsServers
            // 
            this.cmbDnsServers.BackColor = System.Drawing.Color.Bisque;
            this.cmbDnsServers.FormattingEnabled = true;
            this.cmbDnsServers.Location = new System.Drawing.Point(12, 158);
            this.cmbDnsServers.Name = "cmbDnsServers";
            this.cmbDnsServers.Size = new System.Drawing.Size(445, 26);
            this.cmbDnsServers.TabIndex = 12;
            // 
            // btnApplySelectedDns
            // 
            this.btnApplySelectedDns.BackColor = System.Drawing.Color.Bisque;
            this.btnApplySelectedDns.Location = new System.Drawing.Point(555, 150);
            this.btnApplySelectedDns.Name = "btnApplySelectedDns";
            this.btnApplySelectedDns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnApplySelectedDns.Size = new System.Drawing.Size(176, 40);
            this.btnApplySelectedDns.TabIndex = 14;
            this.btnApplySelectedDns.Text = "اعمال DNS انتخاب‌شده";
            this.btnApplySelectedDns.UseVisualStyleBackColor = false;
            this.btnApplySelectedDns.Click += new System.EventHandler(this.btnApplySelectedDns_Click);
            // 
            // btnPingAllDns
            // 
            this.btnPingAllDns.BackColor = System.Drawing.Color.Bisque;
            this.btnPingAllDns.Location = new System.Drawing.Point(463, 150);
            this.btnPingAllDns.Name = "btnPingAllDns";
            this.btnPingAllDns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnPingAllDns.Size = new System.Drawing.Size(86, 40);
            this.btnPingAllDns.TabIndex = 13;
            this.btnPingAllDns.Text = "تست";
            this.btnPingAllDns.UseVisualStyleBackColor = false;
            this.btnPingAllDns.Click += new System.EventHandler(this.btnPingAllDns_Click);
            // 
            // btnChangeAdapterSettings
            // 
            this.btnChangeAdapterSettings.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnChangeAdapterSettings.Location = new System.Drawing.Point(373, 196);
            this.btnChangeAdapterSettings.Name = "btnChangeAdapterSettings";
            this.btnChangeAdapterSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnChangeAdapterSettings.Size = new System.Drawing.Size(358, 40);
            this.btnChangeAdapterSettings.TabIndex = 15;
            this.btnChangeAdapterSettings.Text = "Change adapter settings";
            this.btnChangeAdapterSettings.UseVisualStyleBackColor = false;
            this.btnChangeAdapterSettings.Click += new System.EventHandler(this.btnChangeAdapterSettings_Click);
            // 
            // btnDeveloperWebsite
            // 
            this.btnDeveloperWebsite.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnDeveloperWebsite.Location = new System.Drawing.Point(9, 196);
            this.btnDeveloperWebsite.Name = "btnDeveloperWebsite";
            this.btnDeveloperWebsite.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDeveloperWebsite.Size = new System.Drawing.Size(358, 40);
            this.btnDeveloperWebsite.TabIndex = 16;
            this.btnDeveloperWebsite.Text = "برنامه‌نویس محمدرضا عموری (mramoori.ir)";
            this.btnDeveloperWebsite.UseVisualStyleBackColor = false;
            this.btnDeveloperWebsite.Click += new System.EventHandler(this.btnDeveloperWebsite_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(738, 654);
            this.Controls.Add(this.cmbDnsServers);
            this.Controls.Add(this.txtSecondaryDns);
            this.Controls.Add(this.txtPrimaryDns);
            this.Controls.Add(this.lblPingStatus);
            this.Controls.Add(this.txtPingTarget);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnSetAllAdaptersDhcp);
            this.Controls.Add(this.btnPingAllDns);
            this.Controls.Add(this.btnDeveloperWebsite);
            this.Controls.Add(this.btnChangeAdapterSettings);
            this.Controls.Add(this.btnApplySelectedDns);
            this.Controls.Add(this.btnApplyCustomDns);
            this.Controls.Add(this.btnRemoveSystemProxy);
            this.Controls.Add(this.btnDisableLanSettings);
            this.Controls.Add(this.btnInternetOptions);
            this.Controls.Add(this.btnResetNetwork);
            this.Controls.Add(this.btnFlushDns);
            this.Controls.Add(this.btnIpConfig);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Win Net Tool - بزار مدیریت شبکه";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIpConfig;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button btnFlushDns;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnResetNetwork;
        private System.Windows.Forms.Button btnInternetOptions;
        private System.Windows.Forms.Button btnDisableLanSettings;
        private System.Windows.Forms.Button btnSetAllAdaptersDhcp;
        private System.Windows.Forms.TextBox txtPingTarget;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Label lblPingStatus;
        private System.Windows.Forms.Button btnRemoveSystemProxy;
        private System.Windows.Forms.TextBox txtPrimaryDns;
        private System.Windows.Forms.TextBox txtSecondaryDns;
        private System.Windows.Forms.Button btnApplyCustomDns;
        private System.Windows.Forms.ComboBox cmbDnsServers;
        private System.Windows.Forms.Button btnApplySelectedDns;
        private System.Windows.Forms.Button btnPingAllDns;
        private System.Windows.Forms.Button btnChangeAdapterSettings;
        private System.Windows.Forms.Button btnDeveloperWebsite;
    }
}


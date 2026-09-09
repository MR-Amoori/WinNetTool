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
            this.SuspendLayout();
            // 
            // btnIpConfig
            // 
            this.btnIpConfig.Location = new System.Drawing.Point(9, 12);
            this.btnIpConfig.Name = "btnIpConfig";
            this.btnIpConfig.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnIpConfig.Size = new System.Drawing.Size(176, 40);
            this.btnIpConfig.TabIndex = 0;
            this.btnIpConfig.Text = "دریافت اطلاعات IP";
            this.btnIpConfig.UseVisualStyleBackColor = true;
            this.btnIpConfig.Click += new System.EventHandler(this.btnIpConfig_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblStatus.Location = new System.Drawing.Point(6, 250);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "وضعیت";
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbOutput.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.Location = new System.Drawing.Point(0, 271);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(733, 383);
            this.rtbOutput.TabIndex = 2;
            this.rtbOutput.Text = "";
            // 
            // btnFlushDns
            // 
            this.btnFlushDns.Location = new System.Drawing.Point(191, 12);
            this.btnFlushDns.Name = "btnFlushDns";
            this.btnFlushDns.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnFlushDns.Size = new System.Drawing.Size(176, 40);
            this.btnFlushDns.TabIndex = 1;
            this.btnFlushDns.Text = "پاک‌سازی DNS Cache";
            this.btnFlushDns.UseVisualStyleBackColor = true;
            this.btnFlushDns.Click += new System.EventHandler(this.btnFlushDns_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(658, 250);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "پاکسازی";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnResetNetwork
            // 
            this.btnResetNetwork.Location = new System.Drawing.Point(373, 12);
            this.btnResetNetwork.Name = "btnResetNetwork";
            this.btnResetNetwork.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnResetNetwork.Size = new System.Drawing.Size(176, 40);
            this.btnResetNetwork.TabIndex = 2;
            this.btnResetNetwork.Text = "ریست شبکه";
            this.btnResetNetwork.UseVisualStyleBackColor = true;
            this.btnResetNetwork.Click += new System.EventHandler(this.btnResetNetwork_Click);
            // 
            // btnInternetOptions
            // 
            this.btnInternetOptions.Location = new System.Drawing.Point(555, 12);
            this.btnInternetOptions.Name = "btnInternetOptions";
            this.btnInternetOptions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnInternetOptions.Size = new System.Drawing.Size(176, 40);
            this.btnInternetOptions.TabIndex = 3;
            this.btnInternetOptions.Text = "اینترنت آپشن";
            this.btnInternetOptions.UseVisualStyleBackColor = true;
            this.btnInternetOptions.Click += new System.EventHandler(this.btnInternetOptions_Click);
            // 
            // btnDisableLanSettings
            // 
            this.btnDisableLanSettings.Location = new System.Drawing.Point(555, 58);
            this.btnDisableLanSettings.Name = "btnDisableLanSettings";
            this.btnDisableLanSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnDisableLanSettings.Size = new System.Drawing.Size(176, 40);
            this.btnDisableLanSettings.TabIndex = 3;
            this.btnDisableLanSettings.Text = "حذف لن اینترنت آپشن";
            this.btnDisableLanSettings.UseVisualStyleBackColor = true;
            this.btnDisableLanSettings.Click += new System.EventHandler(this.btnDisableLanSettings_Click);
            // 
            // btnSetAllAdaptersDhcp
            // 
            this.btnSetAllAdaptersDhcp.Location = new System.Drawing.Point(373, 58);
            this.btnSetAllAdaptersDhcp.Name = "btnSetAllAdaptersDhcp";
            this.btnSetAllAdaptersDhcp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnSetAllAdaptersDhcp.Size = new System.Drawing.Size(176, 40);
            this.btnSetAllAdaptersDhcp.TabIndex = 3;
            this.btnSetAllAdaptersDhcp.Text = "حذف DNS آداپتور ها";
            this.btnSetAllAdaptersDhcp.UseVisualStyleBackColor = true;
            this.btnSetAllAdaptersDhcp.Click += new System.EventHandler(this.btnSetAllAdaptersDhcp_Click);
            // 
            // txtPingTarget
            // 
            this.txtPingTarget.Location = new System.Drawing.Point(12, 58);
            this.txtPingTarget.Name = "txtPingTarget";
            this.txtPingTarget.Size = new System.Drawing.Size(173, 26);
            this.txtPingTarget.TabIndex = 11;
            this.txtPingTarget.Text = "8.8.8.8";
            this.txtPingTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(191, 58);
            this.btnPing.Name = "btnPing";
            this.btnPing.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnPing.Size = new System.Drawing.Size(176, 40);
            this.btnPing.TabIndex = 3;
            this.btnPing.Text = "Ping";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // lblPingStatus
            // 
            this.lblPingStatus.AutoSize = true;
            this.lblPingStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPingStatus.Location = new System.Drawing.Point(12, 84);
            this.lblPingStatus.Name = "lblPingStatus";
            this.lblPingStatus.Size = new System.Drawing.Size(40, 14);
            this.lblPingStatus.TabIndex = 12;
            this.lblPingStatus.Text = "00 ms";
            // 
            // btnRemoveSystemProxy
            // 
            this.btnRemoveSystemProxy.Location = new System.Drawing.Point(555, 104);
            this.btnRemoveSystemProxy.Name = "btnRemoveSystemProxy";
            this.btnRemoveSystemProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnRemoveSystemProxy.Size = new System.Drawing.Size(176, 40);
            this.btnRemoveSystemProxy.TabIndex = 3;
            this.btnRemoveSystemProxy.Text = "حذف پروکسی";
            this.btnRemoveSystemProxy.UseVisualStyleBackColor = true;
            this.btnRemoveSystemProxy.Click += new System.EventHandler(this.btnRemoveSystemProxy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 654);
            this.Controls.Add(this.lblPingStatus);
            this.Controls.Add(this.txtPingTarget);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnPing);
            this.Controls.Add(this.btnSetAllAdaptersDhcp);
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
    }
}


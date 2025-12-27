namespace GymCheckIn
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpSensor = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnTestMode = new System.Windows.Forms.Button();
            this.btnSimulateCheckIn = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpNewMember = new System.Windows.Forms.GroupBox();
            this.lblEnrollStatus = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.dtpExpiry = new System.Windows.Forms.DateTimePicker();
            this.lblExpiry = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.grpCheckIn = new System.Windows.Forms.GroupBox();
            this.lblMemberInfo = new System.Windows.Forms.Label();
            this.lblCheckInResult = new System.Windows.Forms.Label();
            this.pnlCheckInResult = new System.Windows.Forms.Panel();
            this.picFingerprint = new System.Windows.Forms.PictureBox();
            this.grpMembers = new System.Windows.Forms.GroupBox();
            this.btnOpenDataFolder = new System.Windows.Forms.Button();
            this.btnViewCheckIns = new System.Windows.Forms.Button();
            this.btnExtendMembership = new System.Windows.Forms.Button();
            this.btnDeleteMember = new System.Windows.Forms.Button();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.lstMembers = new System.Windows.Forms.ListBox();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.axZKFPEngX1 = new AxZKFPEngXControl.AxZKFPEngX();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpSensor.SuspendLayout();
            this.grpNewMember.SuspendLayout();
            this.grpCheckIn.SuspendLayout();
            this.pnlCheckInResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).BeginInit();
            this.grpMembers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axZKFPEngX1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSensor
            // 
            this.grpSensor.Controls.Add(this.btnDisconnect);
            this.grpSensor.Controls.Add(this.btnConnect);
            this.grpSensor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpSensor.Location = new System.Drawing.Point(12, 50);
            this.grpSensor.Name = "grpSensor";
            this.grpSensor.Size = new System.Drawing.Size(220, 60);
            this.grpSensor.TabIndex = 0;
            this.grpSensor.TabStop = false;
            this.grpSensor.Text = "Fingerprint Sensor";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(115, 22);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(95, 28);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // btnTestMode
            //
            this.btnTestMode.Location = new System.Drawing.Point(12, 12);
            this.btnTestMode.Name = "btnTestMode";
            this.btnTestMode.Size = new System.Drawing.Size(120, 30);
            this.btnTestMode.TabIndex = 100;
            this.btnTestMode.Text = "Test Mode: OFF";
            this.btnTestMode.UseVisualStyleBackColor = true;
            this.btnTestMode.Click += new System.EventHandler(this.btnTestMode_Click);
            //
            // btnSimulateCheckIn
            //
            this.btnSimulateCheckIn.Location = new System.Drawing.Point(140, 12);
            this.btnSimulateCheckIn.Name = "btnSimulateCheckIn";
            this.btnSimulateCheckIn.Size = new System.Drawing.Size(130, 30);
            this.btnSimulateCheckIn.TabIndex = 101;
            this.btnSimulateCheckIn.Text = "Simulate Check-In";
            this.btnSimulateCheckIn.UseVisualStyleBackColor = true;
            this.btnSimulateCheckIn.Visible = false;
            this.btnSimulateCheckIn.Click += new System.EventHandler(this.btnSimulateCheckIn_Click);

            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 22);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 28);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // grpNewMember
            // 
            this.grpNewMember.Controls.Add(this.lblEnrollStatus);
            this.grpNewMember.Controls.Add(this.btnRegister);
            this.grpNewMember.Controls.Add(this.dtpExpiry);
            this.grpNewMember.Controls.Add(this.lblExpiry);
            this.grpNewMember.Controls.Add(this.txtPhone);
            this.grpNewMember.Controls.Add(this.lblPhone);
            this.grpNewMember.Controls.Add(this.txtName);
            this.grpNewMember.Controls.Add(this.lblName);
            this.grpNewMember.Enabled = false;
            this.grpNewMember.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpNewMember.Location = new System.Drawing.Point(12, 120);
            this.grpNewMember.Name = "grpNewMember";
            this.grpNewMember.Size = new System.Drawing.Size(220, 190);
            this.grpNewMember.TabIndex = 1;
            this.grpNewMember.TabStop = false;
            this.grpNewMember.Text = "Register New Member";
            // 
            // lblEnrollStatus
            // 
            this.lblEnrollStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEnrollStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblEnrollStatus.Location = new System.Drawing.Point(10, 165);
            this.lblEnrollStatus.Name = "lblEnrollStatus";
            this.lblEnrollStatus.Size = new System.Drawing.Size(200, 20);
            this.lblEnrollStatus.TabIndex = 7;
            this.lblEnrollStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRegister.Enabled = false;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(10, 130);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(200, 32);
            this.btnRegister.TabIndex = 6;
            this.btnRegister.Text = "Register && Scan Fingerprint";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // dtpExpiry
            // 
            this.dtpExpiry.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpiry.Location = new System.Drawing.Point(80, 100);
            this.dtpExpiry.Name = "dtpExpiry";
            this.dtpExpiry.Size = new System.Drawing.Size(130, 23);
            this.dtpExpiry.TabIndex = 5;
            // 
            // lblExpiry
            // 
            this.lblExpiry.AutoSize = true;
            this.lblExpiry.Location = new System.Drawing.Point(10, 104);
            this.lblExpiry.Name = "lblExpiry";
            this.lblExpiry.Size = new System.Drawing.Size(47, 15);
            this.lblExpiry.TabIndex = 4;
            this.lblExpiry.Text = "Expires:";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(80, 65);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(130, 23);
            this.txtPhone.TabIndex = 3;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(10, 68);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(44, 15);
            this.lblPhone.TabIndex = 2;
            this.lblPhone.Text = "Phone:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(80, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(130, 23);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 33);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 15);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // grpCheckIn
            // 
            this.grpCheckIn.Controls.Add(this.lblMemberInfo);
            this.grpCheckIn.Controls.Add(this.lblCheckInResult);
            this.grpCheckIn.Controls.Add(this.pnlCheckInResult);
            this.grpCheckIn.Controls.Add(this.picFingerprint);
            this.grpCheckIn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpCheckIn.Location = new System.Drawing.Point(12, 320);
            this.grpCheckIn.Name = "grpCheckIn";
            this.grpCheckIn.Size = new System.Drawing.Size(220, 260);
            this.grpCheckIn.TabIndex = 2;
            this.grpCheckIn.TabStop = false;
            this.grpCheckIn.Text = "Check-In (Place finger on sensor)";
            // 
            // lblMemberInfo
            // 
            this.lblMemberInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMemberInfo.Location = new System.Drawing.Point(10, 205);
            this.lblMemberInfo.Name = "lblMemberInfo";
            this.lblMemberInfo.Size = new System.Drawing.Size(200, 50);
            this.lblMemberInfo.TabIndex = 3;
            this.lblMemberInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCheckInResult
            // 
            this.lblCheckInResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCheckInResult.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCheckInResult.ForeColor = System.Drawing.Color.White;
            this.lblCheckInResult.Location = new System.Drawing.Point(0, 0);
            this.lblCheckInResult.Name = "lblCheckInResult";
            this.lblCheckInResult.Size = new System.Drawing.Size(200, 40);
            this.lblCheckInResult.TabIndex = 2;
            this.lblCheckInResult.Text = "WAITING...";
            this.lblCheckInResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCheckInResult
            // 
            this.pnlCheckInResult.BackColor = System.Drawing.Color.Gray;
            this.pnlCheckInResult.Controls.Add(this.lblCheckInResult);
            this.pnlCheckInResult.Location = new System.Drawing.Point(10, 160);
            this.pnlCheckInResult.Name = "pnlCheckInResult";
            this.pnlCheckInResult.Size = new System.Drawing.Size(200, 40);
            this.pnlCheckInResult.TabIndex = 1;
            // 
            // picFingerprint
            // 
            this.picFingerprint.BackColor = System.Drawing.Color.LightGray;
            this.picFingerprint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFingerprint.Location = new System.Drawing.Point(35, 25);
            this.picFingerprint.Name = "picFingerprint";
            this.picFingerprint.Size = new System.Drawing.Size(150, 130);
            this.picFingerprint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFingerprint.TabIndex = 0;
            this.picFingerprint.TabStop = false;
            // 
            // grpMembers
            // 
            this.grpMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMembers.Controls.Add(this.btnOpenDataFolder);
            this.grpMembers.Controls.Add(this.btnViewCheckIns);
            this.grpMembers.Controls.Add(this.btnExtendMembership);
            this.grpMembers.Controls.Add(this.btnDeleteMember);
            this.grpMembers.Controls.Add(this.lblMemberCount);
            this.grpMembers.Controls.Add(this.lstMembers);
            this.grpMembers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpMembers.Location = new System.Drawing.Point(250, 50);
            this.grpMembers.Name = "grpMembers";
            this.grpMembers.Size = new System.Drawing.Size(420, 530);
            this.grpMembers.TabIndex = 3;
            this.grpMembers.TabStop = false;
            this.grpMembers.Text = "Members List";
            // 
            // btnOpenDataFolder
            // 
            this.btnOpenDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenDataFolder.Location = new System.Drawing.Point(310, 492);
            this.btnOpenDataFolder.Name = "btnOpenDataFolder";
            this.btnOpenDataFolder.Size = new System.Drawing.Size(100, 28);
            this.btnOpenDataFolder.TabIndex = 5;
            this.btnOpenDataFolder.Text = "Open Excel Files";
            this.btnOpenDataFolder.UseVisualStyleBackColor = true;
            this.btnOpenDataFolder.Click += new System.EventHandler(this.btnOpenDataFolder_Click);
            // 
            // btnViewCheckIns
            // 
            this.btnViewCheckIns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewCheckIns.Location = new System.Drawing.Point(204, 492);
            this.btnViewCheckIns.Name = "btnViewCheckIns";
            this.btnViewCheckIns.Size = new System.Drawing.Size(100, 28);
            this.btnViewCheckIns.TabIndex = 4;
            this.btnViewCheckIns.Text = "Today's Check-ins";
            this.btnViewCheckIns.UseVisualStyleBackColor = true;
            this.btnViewCheckIns.Click += new System.EventHandler(this.btnViewCheckIns_Click);
            // 
            // btnExtendMembership
            // 
            this.btnExtendMembership.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExtendMembership.Enabled = false;
            this.btnExtendMembership.Location = new System.Drawing.Point(98, 492);
            this.btnExtendMembership.Name = "btnExtendMembership";
            this.btnExtendMembership.Size = new System.Drawing.Size(100, 28);
            this.btnExtendMembership.TabIndex = 3;
            this.btnExtendMembership.Text = "Extend";
            this.btnExtendMembership.UseVisualStyleBackColor = true;
            this.btnExtendMembership.Click += new System.EventHandler(this.btnExtendMembership_Click);
            // 
            // btnDeleteMember
            // 
            this.btnDeleteMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMember.Enabled = false;
            this.btnDeleteMember.Location = new System.Drawing.Point(10, 492);
            this.btnDeleteMember.Name = "btnDeleteMember";
            this.btnDeleteMember.Size = new System.Drawing.Size(82, 28);
            this.btnDeleteMember.TabIndex = 2;
            this.btnDeleteMember.Text = "Delete";
            this.btnDeleteMember.UseVisualStyleBackColor = true;
            this.btnDeleteMember.Click += new System.EventHandler(this.btnDeleteMember_Click);
            // 
            // lblMemberCount
            // 
            this.lblMemberCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMemberCount.AutoSize = true;
            this.lblMemberCount.Location = new System.Drawing.Point(10, 470);
            this.lblMemberCount.Name = "lblMemberCount";
            this.lblMemberCount.Size = new System.Drawing.Size(94, 15);
            this.lblMemberCount.TabIndex = 1;
            this.lblMemberCount.Text = "Total Members: 0";
            // 
            // lstMembers
            // 
            this.lstMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMembers.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.lstMembers.FormattingEnabled = true;
            this.lstMembers.ItemHeight = 15;
            this.lstMembers.Location = new System.Drawing.Point(10, 25);
            this.lstMembers.Name = "lstMembers";
            this.lstMembers.Size = new System.Drawing.Size(400, 439);
            this.lstMembers.TabIndex = 0;
            this.lstMembers.SelectedIndexChanged += new System.EventHandler(this.lstMembers_SelectedIndexChanged);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 590);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(684, 22);
            this.statusBar.TabIndex = 4;
            this.statusBar.Text = "Ready";
            // 
            // axZKFPEngX1
            // 
            this.axZKFPEngX1.Enabled = true;
            this.axZKFPEngX1.Location = new System.Drawing.Point(180, 320);
            this.axZKFPEngX1.Name = "axZKFPEngX1";
            this.axZKFPEngX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axZKFPEngX1.OcxState")));
            this.axZKFPEngX1.Size = new System.Drawing.Size(24, 24);
            this.axZKFPEngX1.TabIndex = 5;
            this.axZKFPEngX1.Visible = false;
            this.axZKFPEngX1.OnFeatureInfo += new AxZKFPEngXControl.IZKFPEngXEvents_OnFeatureInfoEventHandler(this.axZKFPEngX1_OnFeatureInfo);
            this.axZKFPEngX1.OnImageReceived += new AxZKFPEngXControl.IZKFPEngXEvents_OnImageReceivedEventHandler(this.axZKFPEngX1_OnImageReceived);
            this.axZKFPEngX1.OnEnroll += new AxZKFPEngXControl.IZKFPEngXEvents_OnEnrollEventHandler(this.axZKFPEngX1_OnEnroll);
            this.axZKFPEngX1.OnCapture += new AxZKFPEngXControl.IZKFPEngXEvents_OnCaptureEventHandler(this.axZKFPEngX1_OnCapture);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(253, 30);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Gym Member Check-In";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 612);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.axZKFPEngX1);
            this.Controls.Add(this.btnTestMode);
            this.Controls.Add(this.btnSimulateCheckIn);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.grpMembers);
            this.Controls.Add(this.grpCheckIn);
            this.Controls.Add(this.grpNewMember);
            this.Controls.Add(this.grpSensor);
            this.MinimumSize = new System.Drawing.Size(700, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gym Member Check-In System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpSensor.ResumeLayout(false);
            this.grpNewMember.ResumeLayout(false);
            this.grpNewMember.PerformLayout();
            this.grpCheckIn.ResumeLayout(false);
            this.pnlCheckInResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).EndInit();
            this.grpMembers.ResumeLayout(false);
            this.grpMembers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axZKFPEngX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox grpSensor;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox grpNewMember;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.Label lblExpiry;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox grpCheckIn;
        private System.Windows.Forms.PictureBox picFingerprint;
        private System.Windows.Forms.GroupBox grpMembers;
        private System.Windows.Forms.ListBox lstMembers;
        private System.Windows.Forms.Button btnTestMode;
        private System.Windows.Forms.Button btnSimulateCheckIn;
        private System.Windows.Forms.StatusBar statusBar;
        private AxZKFPEngXControl.AxZKFPEngX axZKFPEngX1;
        private System.Windows.Forms.Label lblMemberCount;
        private System.Windows.Forms.Button btnDeleteMember;
        private System.Windows.Forms.Button btnExtendMembership;
        private System.Windows.Forms.Label lblEnrollStatus;
        private System.Windows.Forms.Panel pnlCheckInResult;
        private System.Windows.Forms.Label lblCheckInResult;
        private System.Windows.Forms.Label lblMemberInfo;
        private System.Windows.Forms.Button btnViewCheckIns;
        private System.Windows.Forms.Button btnOpenDataFolder;
        private System.Windows.Forms.Label lblTitle;
    }
}




namespace GymCheckIn.Forms
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
            this.components = new System.ComponentModel.Container();
            
            // System Tray
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayMenuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.trayMenuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.trayMenuExit = new System.Windows.Forms.ToolStripMenuItem();

            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCheckIn = new System.Windows.Forms.TabPage();
            this.tabEnrollment = new System.Windows.Forms.TabPage();
            this.tabMembers = new System.Windows.Forms.TabPage();
            this.tabLogs = new System.Windows.Forms.TabPage();

            // Check-In Tab Controls
            this.pnlCheckInResult = new System.Windows.Forms.Panel();
            this.lblCheckInTitle = new System.Windows.Forms.Label();
            this.lblCheckInDetails = new System.Windows.Forms.Label();
            this.picFingerprint = new System.Windows.Forms.PictureBox();
            this.grpSensor = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblSensorStatus = new System.Windows.Forms.Label();

            // Enrollment Tab Controls
            this.grpFitAddis = new System.Windows.Forms.GroupBox();
            this.btnFetchMembers = new System.Windows.Forms.Button();
            this.cmbFitAddisMembers = new System.Windows.Forms.ComboBox();
            this.lblMemberListInfo = new System.Windows.Forms.Label();
            this.grpEnrollment = new System.Windows.Forms.GroupBox();
            this.btnEnrollFingerprint = new System.Windows.Forms.Button();
            this.lblEnrollStatus = new System.Windows.Forms.Label();

            // Members Tab Controls
            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.btnDeleteMember = new System.Windows.Forms.Button();
            this.btnExportMembers = new System.Windows.Forms.Button();

            // Logs Tab Controls
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.dtpLogFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpLogTo = new System.Windows.Forms.DateTimePicker();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnExportLogs = new System.Windows.Forms.Button();
            this.lblLogCount = new System.Windows.Forms.Label();


            // Manual Check-In Controls
            this.grpManualCheckIn = new System.Windows.Forms.GroupBox();
            this.cmbManualCheckInMembers = new System.Windows.Forms.ComboBox();
            this.btnManualCheckIn = new System.Windows.Forms.Button();
            this.btnLoadMembers = new System.Windows.Forms.Button();

            // Status Bar Controls
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblSyncStatus = new System.Windows.Forms.Label();
            this.btnForceSync = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();

            // Log Panel (commented out)
            // this.grpLog = new System.Windows.Forms.GroupBox();
            // this.txtLog = new System.Windows.Forms.TextBox();

            // ZKTeco ActiveX Control
            this.axZKFPEngX1 = new AxZKFPEngXControl.AxZKFPEngX();

            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axZKFPEngX1)).BeginInit();
            this.SuspendLayout();

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabCheckIn);
            this.tabControl.Controls.Add(this.tabEnrollment);
            this.tabControl.Controls.Add(this.tabMembers);
            this.tabControl.Controls.Add(this.tabLogs);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1100, 520);
            this.tabControl.TabIndex = 0;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tabControl.Padding = new System.Drawing.Point(25, 8);

            // 
            // tabCheckIn
            // 
            this.tabCheckIn.Text = "Check-In";
            this.tabCheckIn.Padding = new System.Windows.Forms.Padding(10);
            this.tabCheckIn.Controls.Add(this.pnlCheckInResult);
            this.tabCheckIn.Controls.Add(this.picFingerprint);
            this.tabCheckIn.Controls.Add(this.grpSensor);
            this.tabCheckIn.Controls.Add(this.grpManualCheckIn);

            // 
            // pnlCheckInResult
            // 
            this.pnlCheckInResult.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.pnlCheckInResult.Location = new System.Drawing.Point(220, 20);
            this.pnlCheckInResult.Size = new System.Drawing.Size(520, 250);
            this.pnlCheckInResult.Controls.Add(this.lblCheckInTitle);
            this.pnlCheckInResult.Controls.Add(this.lblCheckInDetails);

            // 
            // lblCheckInTitle
            // 
            this.lblCheckInTitle.Font = new System.Drawing.Font("Segoe UI", 42F, System.Drawing.FontStyle.Bold);
            this.lblCheckInTitle.ForeColor = System.Drawing.Color.White;
            this.lblCheckInTitle.Location = new System.Drawing.Point(10, 40);
            this.lblCheckInTitle.Size = new System.Drawing.Size(500, 80);
            this.lblCheckInTitle.Text = "READY";
            this.lblCheckInTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblCheckInDetails
            // 
            this.lblCheckInDetails.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblCheckInDetails.ForeColor = System.Drawing.Color.White;
            this.lblCheckInDetails.Location = new System.Drawing.Point(10, 130);
            this.lblCheckInDetails.Size = new System.Drawing.Size(500, 100);
            this.lblCheckInDetails.Text = "Scan fingerprint to check in";
            this.lblCheckInDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // picFingerprint
            // 
            this.picFingerprint.BackColor = System.Drawing.Color.White;
            this.picFingerprint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.picFingerprint.Location = new System.Drawing.Point(220, 285);
            this.picFingerprint.Size = new System.Drawing.Size(180, 180);
            this.picFingerprint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // 
            // grpSensor
            // 
            this.grpSensor.Text = "  Fingerprint Sensor  ";
            this.grpSensor.Location = new System.Drawing.Point(20, 20);
            this.grpSensor.Size = new System.Drawing.Size(185, 190);
            this.grpSensor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSensor.Controls.Add(this.btnConnect);
            this.grpSensor.Controls.Add(this.btnDisconnect);
            this.grpSensor.Controls.Add(this.lblSensorStatus);

            // 
            // btnConnect
            // 
            this.btnConnect.Text = "Connect";
            this.btnConnect.Location = new System.Drawing.Point(15, 35);
            this.btnConnect.Size = new System.Drawing.Size(155, 45);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Location = new System.Drawing.Point(15, 90);
            this.btnDisconnect.Size = new System.Drawing.Size(155, 45);
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // 
            // lblSensorStatus
            // 
            this.lblSensorStatus.Text = "● DISCONNECTED";
            this.lblSensorStatus.Location = new System.Drawing.Point(15, 145);
            this.lblSensorStatus.Size = new System.Drawing.Size(155, 30);
            this.lblSensorStatus.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lblSensorStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSensorStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // grpManualCheckIn
            // 
            this.grpManualCheckIn.Text = "  Manual Check-In  ";
            this.grpManualCheckIn.Location = new System.Drawing.Point(755, 20);
            this.grpManualCheckIn.Size = new System.Drawing.Size(222, 180);
            this.grpManualCheckIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpManualCheckIn.Controls.Add(this.btnLoadMembers);
            this.grpManualCheckIn.Controls.Add(this.cmbManualCheckInMembers);
            this.grpManualCheckIn.Controls.Add(this.btnManualCheckIn);

            // 
            // btnLoadMembers
            // 
            this.btnLoadMembers.Text = "🔄 Load Members";
            this.btnLoadMembers.Location = new System.Drawing.Point(15, 30);
            this.btnLoadMembers.Size = new System.Drawing.Size(190, 35);
            this.btnLoadMembers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLoadMembers.Click += new System.EventHandler(this.btnLoadMembers_Click);

            // 
            // cmbManualCheckInMembers
            // 
            this.cmbManualCheckInMembers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManualCheckInMembers.Location = new System.Drawing.Point(15, 75);
            this.cmbManualCheckInMembers.Size = new System.Drawing.Size(190, 30);
            this.cmbManualCheckInMembers.Font = new System.Drawing.Font("Segoe UI", 9F);

            // 
            // btnManualCheckIn
            // 
            this.btnManualCheckIn.Text = "✓ Check In";
            this.btnManualCheckIn.Location = new System.Drawing.Point(15, 120);
            this.btnManualCheckIn.Size = new System.Drawing.Size(190, 45);
            this.btnManualCheckIn.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnManualCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnManualCheckIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualCheckIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnManualCheckIn.Click += new System.EventHandler(this.btnManualCheckIn_Click);

            // 
            // tabEnrollment
            // 
            this.tabEnrollment.Text = "Enrollment";
            this.tabEnrollment.Padding = new System.Windows.Forms.Padding(20);
            this.tabEnrollment.Controls.Add(this.grpFitAddis);
            this.tabEnrollment.Controls.Add(this.grpEnrollment);

            // 
            // grpFitAddis
            // 
            this.grpFitAddis.Text = "  Fit Addis Members  ";
            this.grpFitAddis.Location = new System.Drawing.Point(20, 20);
            this.grpFitAddis.Size = new System.Drawing.Size(520, 150);
            this.grpFitAddis.Controls.Add(this.btnFetchMembers);
            this.grpFitAddis.Controls.Add(this.cmbFitAddisMembers);
            this.grpFitAddis.Controls.Add(this.lblMemberListInfo);

            // 
            // btnFetchMembers
            // 
            this.btnFetchMembers.Text = "🔄  Refresh Members List";
            this.btnFetchMembers.Location = new System.Drawing.Point(20, 30);
            this.btnFetchMembers.Size = new System.Drawing.Size(200, 40);
            this.btnFetchMembers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFetchMembers.Click += new System.EventHandler(this.btnFetchMembers_Click);

            // 
            // lblMemberListInfo
            // 
            this.lblMemberListInfo.Text = "Select member to enroll:";
            this.lblMemberListInfo.Location = new System.Drawing.Point(20, 80);
            this.lblMemberListInfo.Size = new System.Drawing.Size(200, 25);
            this.lblMemberListInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMemberListInfo.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);

            // 
            // cmbFitAddisMembers
            // 
            this.cmbFitAddisMembers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFitAddisMembers.Location = new System.Drawing.Point(20, 105);
            this.cmbFitAddisMembers.Size = new System.Drawing.Size(480, 35);
            this.cmbFitAddisMembers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFitAddisMembers.ItemHeight = 20;

            // 
            // grpEnrollment
            // 
            this.grpEnrollment.Text = "  Fingerprint Enrollment  ";
            this.grpEnrollment.Location = new System.Drawing.Point(20, 180);
            this.grpEnrollment.Size = new System.Drawing.Size(520, 280);
            this.grpEnrollment.Enabled = false;
            this.grpEnrollment.Controls.Add(this.btnEnrollFingerprint);
            this.grpEnrollment.Controls.Add(this.lblEnrollStatus);

            // 
            // btnEnrollFingerprint
            // 
            this.btnEnrollFingerprint.Text = "👆  Enroll Selected Member's Fingerprint";
            this.btnEnrollFingerprint.Location = new System.Drawing.Point(20, 40);
            this.btnEnrollFingerprint.Size = new System.Drawing.Size(480, 55);
            this.btnEnrollFingerprint.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEnrollFingerprint.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnEnrollFingerprint.ForeColor = System.Drawing.Color.White;
            this.btnEnrollFingerprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnrollFingerprint.Click += new System.EventHandler(this.btnEnrollFingerprint_Click);

            // 
            // lblEnrollStatus
            // 
            this.lblEnrollStatus.Text = "Select a member and click Enroll";
            this.lblEnrollStatus.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.lblEnrollStatus.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblEnrollStatus.Location = new System.Drawing.Point(20, 110);
            this.lblEnrollStatus.Size = new System.Drawing.Size(480, 150);
            this.lblEnrollStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // tabMembers
            // 
            this.tabMembers.Text = "Members";
            this.tabMembers.Padding = new System.Windows.Forms.Padding(10);
            this.tabMembers.Controls.Add(this.dgvMembers);
            this.tabMembers.Controls.Add(this.lblMemberCount);
            this.tabMembers.Controls.Add(this.btnDeleteMember);
            this.tabMembers.Controls.Add(this.btnExportMembers);

            // 
            // dgvMembers
            // 
            this.dgvMembers.Location = new System.Drawing.Point(20, 20);
            this.dgvMembers.Size = new System.Drawing.Size(860, 390);
            this.dgvMembers.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvMembers.AllowUserToAddRows = false;
            this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.ReadOnly = true;
            this.dgvMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMembers.ScrollBars = System.Windows.Forms.ScrollBars.Both;

            // 
            // lblMemberCount
            // 
            this.lblMemberCount.Text = "📊 Members: 0";
            this.lblMemberCount.Location = new System.Drawing.Point(20, 420);
            this.lblMemberCount.Size = new System.Drawing.Size(300, 30);
            this.lblMemberCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMemberCount.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);

            // 
            // btnDeleteMember
            // 
            this.btnDeleteMember.Text = "🗑️  Delete Selected";
            this.btnDeleteMember.Location = new System.Drawing.Point(900, 20);
            this.btnDeleteMember.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnDeleteMember.Size = new System.Drawing.Size(160, 45);
            this.btnDeleteMember.Click += new System.EventHandler(this.btnDeleteMember_Click);

            // 
            // btnExportMembers
            // 
            this.btnExportMembers.Text = "📥  Export to CSV";
            this.btnExportMembers.Location = new System.Drawing.Point(900, 75);
            this.btnExportMembers.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnExportMembers.Size = new System.Drawing.Size(160, 45);
            this.btnExportMembers.Click += new System.EventHandler(this.btnExportMembers_Click);

            // 
            // tabLogs
            // 
            this.tabLogs.Text = "Check-In Logs";
            this.tabLogs.Padding = new System.Windows.Forms.Padding(20);
            this.tabLogs.Controls.Add(this.dgvLogs);
            this.tabLogs.Controls.Add(this.dtpLogFrom);
            this.tabLogs.Controls.Add(this.dtpLogTo);
            this.tabLogs.Controls.Add(this.btnViewLogs);
            this.tabLogs.Controls.Add(this.btnExportLogs);
            this.tabLogs.Controls.Add(this.lblLogCount);
            var lblFrom = new System.Windows.Forms.Label();
            lblFrom.Text = "From:";
            lblFrom.Location = new System.Drawing.Point(20, 28);
            lblFrom.Size = new System.Drawing.Size(50, 25);
            lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabLogs.Controls.Add(lblFrom);
            var lblTo = new System.Windows.Forms.Label();
            lblTo.Text = "To:";
            lblTo.Location = new System.Drawing.Point(250, 28);
            lblTo.Size = new System.Drawing.Size(30, 25);
            lblTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabLogs.Controls.Add(lblTo);

            // 
            // dgvLogs
            // 
            this.dgvLogs.Location = new System.Drawing.Point(20, 70);
            this.dgvLogs.Size = new System.Drawing.Size(860, 360);
            this.dgvLogs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // dtpLogFrom
            // 
            this.dtpLogFrom.Location = new System.Drawing.Point(75, 23);
            this.dtpLogFrom.Size = new System.Drawing.Size(165, 32);
            this.dtpLogFrom.Value = System.DateTime.Today;
            this.dtpLogFrom.Font = new System.Drawing.Font("Segoe UI", 10F);

            // 
            // dtpLogTo
            // 
            this.dtpLogTo.Location = new System.Drawing.Point(285, 23);
            this.dtpLogTo.Size = new System.Drawing.Size(165, 32);
            this.dtpLogTo.Value = System.DateTime.Today;
            this.dtpLogTo.Font = new System.Drawing.Font("Segoe UI", 10F);

            // 
            // btnViewLogs
            // 
            this.btnViewLogs.Text = "🔍  View";
            this.btnViewLogs.Location = new System.Drawing.Point(470, 20);
            this.btnViewLogs.Size = new System.Drawing.Size(100, 40);
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);

            // 
            // btnExportLogs
            // 
            this.btnExportLogs.Text = "📥  Export All to CSV";
            this.btnExportLogs.Location = new System.Drawing.Point(900, 20);
            this.btnExportLogs.Size = new System.Drawing.Size(160, 45);
            this.btnExportLogs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnExportLogs.Click += new System.EventHandler(this.btnExportLogs_Click);

            // 
            // lblLogCount
            // 
            this.lblLogCount.Text = "";
            this.lblLogCount.Location = new System.Drawing.Point(20, 440);
            this.lblLogCount.Size = new System.Drawing.Size(400, 30);
            this.lblLogCount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogCount.ForeColor = System.Drawing.Color.FromArgb(41, 128, 185);

            // 
            // pnlStatus
            // 
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatus.Height = 50;
            this.pnlStatus.BackColor = System.Drawing.Color.White;
            this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlStatus.Controls.Add(this.lblConnectionStatus);
            this.pnlStatus.Controls.Add(this.lblSyncStatus);
            this.pnlStatus.Controls.Add(this.btnForceSync);
            this.pnlStatus.Controls.Add(this.btnLogout);

            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Text = "● OFFLINE";
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.Location = new System.Drawing.Point(20, 12);
            this.lblConnectionStatus.Size = new System.Drawing.Size(120, 30);

            // 
            // lblSyncStatus
            // 
            this.lblSyncStatus.Text = "📤 Pending sync: 0";
            this.lblSyncStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSyncStatus.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblSyncStatus.Location = new System.Drawing.Point(160, 14);
            this.lblSyncStatus.Size = new System.Drawing.Size(300, 25);

            // 
            // btnForceSync
            // 
            this.btnForceSync.Text = "🔄 Sync Now";
            this.btnForceSync.Location = new System.Drawing.Point(480, 8);
            this.btnForceSync.Size = new System.Drawing.Size(130, 35);
            this.btnForceSync.Click += new System.EventHandler(this.btnForceSync_Click);

            // 
            // btnLogout
            // 
            this.btnLogout.Text = "Logout";
            this.btnLogout.Location = new System.Drawing.Point(630, 8);
            this.btnLogout.Size = new System.Drawing.Size(100, 35);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // grpLog (commented out)
            // 
            // this.grpLog.Text = "  📋 Activity Log  ";
            // this.grpLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            // this.grpLog.Height = 140;
            // this.grpLog.Controls.Add(this.txtLog);

            // 
            // txtLog (commented out)
            // 
            // this.txtLog.Multiline = true;
            // this.txtLog.ReadOnly = true;
            // this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            // this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            // this.txtLog.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            // this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            // this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            // this.txtLog.BackColor = System.Drawing.Color.White;

            // 
            // axZKFPEngX1
            // 
            this.axZKFPEngX1.Enabled = true;
            this.axZKFPEngX1.Location = new System.Drawing.Point(800, 300);
            this.axZKFPEngX1.Size = new System.Drawing.Size(100, 50);
            this.axZKFPEngX1.Visible = false;
            this.axZKFPEngX1.OnCapture += new AxZKFPEngXControl.IZKFPEngXEvents_OnCaptureEventHandler(this.axZKFPEngX1_OnCapture);
            this.axZKFPEngX1.OnEnroll += new AxZKFPEngXControl.IZKFPEngXEvents_OnEnrollEventHandler(this.axZKFPEngX1_OnEnroll);
            this.axZKFPEngX1.OnImageReceived += new AxZKFPEngXControl.IZKFPEngXEvents_OnImageReceivedEventHandler(this.axZKFPEngX1_OnImageReceived);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 760);
            this.Controls.Add(this.tabControl);
            // this.Controls.Add(this.grpLog);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.axZKFPEngX1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏋️ Fit Addis - Gym Check-In System";
            this.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);

            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "Fit Addis - Gym Check-In";
            this.notifyIcon.Icon = System.Drawing.SystemIcons.Application;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ContextMenuStrip = this.trayContextMenu;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);

            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.trayMenuShow,
                this.trayMenuLogout,
                this.trayMenuExit
            });

            // 
            // trayMenuShow
            // 
            this.trayMenuShow.Text = "Show";
            this.trayMenuShow.Click += new System.EventHandler(this.trayMenuShow_Click);

            // 
            // trayMenuLogout
            // 
            this.trayMenuLogout.Text = "Logout";
            this.trayMenuLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // trayMenuExit
            // 
            this.trayMenuExit.Text = "Exit";
            this.trayMenuExit.Click += new System.EventHandler(this.trayMenuExit_Click);

            ((System.ComponentModel.ISupportInitialize)(this.picFingerprint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axZKFPEngX1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCheckIn;
        private System.Windows.Forms.TabPage tabEnrollment;
        private System.Windows.Forms.TabPage tabMembers;
        private System.Windows.Forms.TabPage tabLogs;

        private System.Windows.Forms.Panel pnlCheckInResult;
        private System.Windows.Forms.Label lblCheckInTitle;
        private System.Windows.Forms.Label lblCheckInDetails;
        private System.Windows.Forms.PictureBox picFingerprint;

        private System.Windows.Forms.GroupBox grpSensor;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblSensorStatus;

        private System.Windows.Forms.GroupBox grpFitAddis;
        private System.Windows.Forms.Button btnFetchMembers;
        private System.Windows.Forms.ComboBox cmbFitAddisMembers;
        private System.Windows.Forms.Label lblMemberListInfo;

        private System.Windows.Forms.GroupBox grpEnrollment;
        private System.Windows.Forms.Button btnEnrollFingerprint;
        private System.Windows.Forms.Label lblEnrollStatus;

        private System.Windows.Forms.DataGridView dgvMembers;
        private System.Windows.Forms.Label lblMemberCount;
        private System.Windows.Forms.Button btnDeleteMember;
        private System.Windows.Forms.Button btnExportMembers;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.DateTimePicker dtpLogFrom;
        private System.Windows.Forms.DateTimePicker dtpLogTo;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnExportLogs;
        private System.Windows.Forms.Label lblLogCount;


        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblSyncStatus;
        private System.Windows.Forms.Button btnForceSync;
        private System.Windows.Forms.Button btnLogout;

        // private System.Windows.Forms.GroupBox grpLog;
        // private System.Windows.Forms.TextBox txtLog;

        private System.Windows.Forms.GroupBox grpManualCheckIn;
        private System.Windows.Forms.ComboBox cmbManualCheckInMembers;
        private System.Windows.Forms.Button btnManualCheckIn;
        private System.Windows.Forms.Button btnLoadMembers;

        private AxZKFPEngXControl.AxZKFPEngX axZKFPEngX1;

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem trayMenuShow;
        private System.Windows.Forms.ToolStripMenuItem trayMenuLogout;
        private System.Windows.Forms.ToolStripMenuItem trayMenuExit;
    }
}

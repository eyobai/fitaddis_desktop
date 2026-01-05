using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymCheckIn.Models;
using GymCheckIn.Services;
using GymCheckIn.UI;

namespace GymCheckIn.Forms
{
    public partial class MainForm : Form
    {
        private ZKFingerService _fingerService;
        private DatabaseService _db;
        private FitAddisApiService _api;
        private SyncService _syncService;
        private ExcelExportService _exportService;
        private ApiSettings _apiSettings;
        private LoginResponse _loginData;

        private List<Member> _localMembers = new List<Member>();
        private List<FitAddisMember> _fitAddisMembers = new List<FitAddisMember>();
        private Member _enrollingMember;

        // Sound API
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x0001;
        private string _successSoundPath;
        private string _errorSoundPath;

        public MainForm(LoginResponse loginData)
        {
            _loginData = loginData;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            InitializeServices();
            LoadLocalMembers();
            UpdateUI();
            
            // Initialize sound paths from app directory
            string appDir = Application.StartupPath;
            _successSoundPath = Path.Combine(appDir, @"Sounds\success.wav");
            _errorSoundPath = Path.Combine(appDir, @"Sounds\error.wav");
            Log($"Sound paths: {_successSoundPath} | Exists: {File.Exists(_successSoundPath)}");
            
            // Start sync service
            _syncService.Start();
            
            Log("Application started. Connect fingerprint sensor to begin.");
        }

        private void ApplyTheme()
        {
            // Apply form theme
            ThemeManager.ApplyTheme(this);
            
            // Style buttons
            ThemeManager.StylePrimaryButton(btnConnect);
            ThemeManager.StyleSecondaryButton(btnDisconnect);
            ThemeManager.StylePrimaryButton(btnFetchMembers);
            ThemeManager.StyleSuccessButton(btnEnrollFingerprint);
            ThemeManager.StylePrimaryButton(btnViewLogs);
            ThemeManager.StyleSecondaryButton(btnExportLogs);
            ThemeManager.StyleSecondaryButton(btnExportMembers);
            ThemeManager.StyleDangerButton(btnDeleteMember);
            ThemeManager.StyleSuccessButton(btnForceSync);
            
            // Style group boxes
            ThemeManager.StyleGroupBox(grpSensor);
            ThemeManager.StyleGroupBox(grpFitAddis);
            ThemeManager.StyleGroupBox(grpEnrollment);
            // ThemeManager.StyleGroupBox(grpLog);
            ThemeManager.StyleGroupBox(grpManualCheckIn);
            
            // Style manual check-in controls
            ThemeManager.StylePrimaryButton(btnLoadMembers);
            ThemeManager.StyleSuccessButton(btnManualCheckIn);
            ThemeManager.StyleComboBox(cmbManualCheckInMembers);
            
            // Style data grids
            ThemeManager.StyleDataGridView(dgvMembers);
            ThemeManager.StyleDataGridView(dgvLogs);
            
            // Style tab control
            ThemeManager.StyleTabControl(tabControl);
            
            // Style combo box
            ThemeManager.StyleComboBox(cmbFitAddisMembers);
            
            // Style check-in result panel
            pnlCheckInResult.BackColor = ThemeManager.PrimaryColor;
            
            // Style status panel
            pnlStatus.BackColor = ThemeManager.CardBackground;
            
            // Style tab pages
            foreach (TabPage tab in tabControl.TabPages)
            {
                tab.BackColor = ThemeManager.BackgroundLight;
            }
        }

        private void InitializeServices()
        {
            // Use AppData folder for data storage (doesn't require admin rights)
            string dataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FitAddis", "Data");
            
            _db = new DatabaseService(dataFolder);
            
            // Use login data for API settings
            _apiSettings = new ApiSettings
            {
                BaseUrl = "https://fitaddis-app-53y6g.ondigitalocean.app",
                FitnessCenterId = _loginData.FitnessCenter.Id.ToString(),
                ApiKey = _loginData.Token,
                DeviceId = _db.GetSetting("DeviceId", Environment.MachineName),
                SyncIntervalSeconds = int.Parse(_db.GetSetting("SyncInterval", "30"))
            };

            // Save to database for offline reference
            _db.SaveSetting("FitnessCenterId", _apiSettings.FitnessCenterId);
            _db.SaveSetting("ApiKey", _apiSettings.ApiKey);
            _db.SaveSetting("FitnessCenterName", _loginData.FitnessCenter.Name);

            _api = new FitAddisApiService(_apiSettings);
            _api.OnLog += (s, msg) => Log(msg);

            _syncService = new SyncService(_db, _api, _apiSettings);
            _syncService.OnLog += (s, msg) => Log(msg);
            _syncService.OnSyncStatusChanged += SyncService_OnSyncStatusChanged;
            _syncService.OnConnectionStatusChanged += SyncService_OnConnectionStatusChanged;
            _syncService.OnMembersUpdated += SyncService_OnMembersUpdated;

            _exportService = new ExcelExportService();
            
            // Update form title with fitness center name
            this.Text = $"🏋️ {_loginData.FitnessCenter.Name} - Gym Check-In System";
        }

        private void LoadLocalMembers()
        {
            _localMembers = _db.GetAllMembers();
            RefreshMembersGrid();
        }

        private void SyncLocalMembersWithApi()
        {
            if (_fitAddisMembers == null || _fitAddisMembers.Count == 0) return;

            int updated = 0;
            foreach (var localMember in _localMembers)
            {
                var apiMember = _fitAddisMembers.FirstOrDefault(m => m.CheckInCode == localMember.FitAddisMemberCode);
                if (apiMember != null)
                {
                    _db.UpdateMemberFromApi(
                        localMember.FitAddisMemberCode,
                        apiMember.FullName,
                        apiMember.PhoneNumber,
                        apiMember.Email,
                        apiMember.MembershipName,
                        apiMember.MembershipExpiryDate
                    );
                    updated++;
                }
            }

            if (updated > 0)
            {
                Log($"Synced {updated} local members with API data");
                LoadLocalMembers(); // Refresh local data
            }
        }

        private void RefreshMembersGrid()
        {
            dgvMembers.DataSource = null;
            dgvMembers.DataSource = _localMembers.Select(m => new
            {
                m.Id,
                MemberCode = m.FitAddisMemberCode,
                m.Name,
                m.Phone,
                DaysToExpire = m.MembershipExpiryDate.HasValue 
                    ? (int)(m.MembershipExpiryDate.Value - DateTime.Now).TotalDays 
                    : 0,
                Expiry = m.MembershipExpiryDate?.ToString("dd/MM/yyyy") ?? "N/A",
                Status = m.IsExpired ? "EXPIRED" : "ACTIVE",
                Enrolled = m.IsEnrolled ? "Yes" : "No",
                Plan = m.MembershipPlan ?? ""
            }).ToList();

            lblMemberCount.Text = $"Members: {_localMembers.Count} | Enrolled: {_localMembers.Count(m => m.IsEnrolled)}";
        }

        private void UpdateUI()
        {
            int unsyncedCount = _db.GetUnsyncedCount();
            lblSyncStatus.Text = unsyncedCount > 0 
                ? $"Pending sync: {unsyncedCount}" 
                : "All synced";
            lblSyncStatus.ForeColor = unsyncedCount > 0 ? Color.Orange : Color.Green;
        }

        #region Fingerprint Sensor

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialize the fingerprint service
                _fingerService = new ZKFingerService();
                _fingerService.OnLog += (s, msg) => { try { Log(msg); } catch { } };
                _fingerService.OnFingerprintCaptured += FingerService_OnFingerprintCaptured;
                _fingerService.OnEnrollmentComplete += FingerService_OnEnrollmentComplete;
                _fingerService.OnEnrollmentProgress += FingerService_OnEnrollmentProgress;

                int initResult = _fingerService.Initialize();
                if (initResult != 0)
                {
                    MessageBox.Show($"Failed to initialize fingerprint SDK. Error: {initResult}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int deviceCount = _fingerService.GetDeviceCount();
                Log($"Found {deviceCount} fingerprint device(s)");

                if (deviceCount > 0)
                {
                    int openResult = _fingerService.OpenDevice(0);
                    if (openResult == 0)
                    {
                        // Load enrolled fingerprints into cache
                        var enrolledMembers = _db.GetEnrolledMembers();
                        foreach (var member in enrolledMembers)
                        {
                            if (member.FingerprintId.HasValue && !string.IsNullOrEmpty(member.FingerprintTemplate))
                            {
                                byte[] template = ZKFingerService.Base64ToTemplate(member.FingerprintTemplate);
                                _fingerService.AddTemplateToDb(member.FingerprintId.Value, template);
                            }
                        }

                        // Start capturing
                        _fingerService.StartCapture();

                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = true;
                        grpEnrollment.Enabled = true;

                        Log($"Sensor connected. {enrolledMembers.Count} fingerprints loaded.");
                        UpdateSensorStatus(true);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to open device. Error: {openResult}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _fingerService.Terminate();
                    }
                }
                else
                {
                    MessageBox.Show("No fingerprint sensor detected!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _fingerService.Terminate();
                }
            }
            catch (Exception ex)
            {
                Log($"Connection error: {ex.Message}");
                MessageBox.Show($"Connection error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fingerService != null)
                {
                    _fingerService.Dispose();
                    _fingerService = null;
                }

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                grpEnrollment.Enabled = false;

                Log("Sensor disconnected.");
                UpdateSensorStatus(false);
            }
            catch (Exception ex)
            {
                Log($"Disconnect error: {ex.Message}");
            }
        }

        private void UpdateSensorStatus(bool connected)
        {
            lblSensorStatus.Text = connected ? "CONNECTED" : "DISCONNECTED";
            lblSensorStatus.ForeColor = connected ? Color.Green : Color.Red;
        }

        #endregion

        #region Fit Addis API

        private async void btnFetchMembers_Click(object sender, EventArgs e)
        {
            btnFetchMembers.Enabled = false;
            btnFetchMembers.Text = "Fetching...";

            try
            {
                _fitAddisMembers = await _api.GetMembersAsync();

                if (_fitAddisMembers.Count > 0)
                {
                    // Sync local members with API data (update expiry dates, etc.)
                    SyncLocalMembersWithApi();
                    
                    FilterEnrollmentMembers("");
                    Log($"Loaded {_fitAddisMembers.Count} members from Fit Addis");
                    grpEnrollment.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No members found or API connection failed.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log($"Error fetching members: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFetchMembers.Enabled = true;
                btnFetchMembers.Text = "🔄  Refresh Members List";
            }
        }

        private void txtSearchEnrollment_TextChanged(object sender, EventArgs e)
        {
            FilterEnrollmentMembers(txtSearchEnrollment.Text);
        }

        private void FilterEnrollmentMembers(string searchText)
        {
            if (_fitAddisMembers == null || _fitAddisMembers.Count == 0) return;

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? _fitAddisMembers
                : _fitAddisMembers.Where(m =>
                    m.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.CheckInCode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (m.PhoneNumber != null && m.PhoneNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

            cmbFitAddisMembers.DataSource = null;
            cmbFitAddisMembers.DisplayMember = "Display";
            cmbFitAddisMembers.ValueMember = "CheckInCode";
            cmbFitAddisMembers.DataSource = filtered.Select(m => new
            {
                m.CheckInCode,
                Display = $"{m.FullName} - {m.CheckInCode} ({m.MembershipName})"
            }).ToList();
        }

        #endregion

        #region Manual Check-In

        private async void btnLoadMembers_Click(object sender, EventArgs e)
        {
            btnLoadMembers.Enabled = false;
            btnLoadMembers.Text = "Loading...";

            try
            {
                _fitAddisMembers = await _api.GetMembersAsync();

                if (_fitAddisMembers.Count > 0)
                {
                    FilterManualCheckInMembers("");
                    Log($"Loaded {_fitAddisMembers.Count} members for manual check-in");
                }
                else
                {
                    MessageBox.Show("No members found or API connection failed.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log($"Error loading members: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoadMembers.Enabled = true;
                btnLoadMembers.Text = "🔄 Load Members";
            }
        }

        private void txtSearchManualCheckIn_TextChanged(object sender, EventArgs e)
        {
            FilterManualCheckInMembers(txtSearchManualCheckIn.Text);
        }

        private void FilterManualCheckInMembers(string searchText)
        {
            if (_fitAddisMembers == null || _fitAddisMembers.Count == 0) return;

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? _fitAddisMembers
                : _fitAddisMembers.Where(m =>
                    m.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    m.CheckInCode.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (m.PhoneNumber != null && m.PhoneNumber.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

            cmbManualCheckInMembers.DataSource = null;
            cmbManualCheckInMembers.DisplayMember = "Display";
            cmbManualCheckInMembers.ValueMember = "CheckInCode";
            cmbManualCheckInMembers.DataSource = filtered.Select(m => new
            {
                m.CheckInCode,
                Display = $"{m.FullName} - {m.CheckInCode}"
            }).ToList();
        }

        private async void btnManualCheckIn_Click(object sender, EventArgs e)
        {
            if (cmbManualCheckInMembers.SelectedItem == null)
            {
                MessageBox.Show("Please select a member to check in.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selected = cmbManualCheckInMembers.SelectedItem;
            string checkInCode = selected.CheckInCode;

            var member = _fitAddisMembers.FirstOrDefault(m => m.CheckInCode == checkInCode);
            if (member == null)
            {
                MessageBox.Show("Member not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check membership expiry
            bool isExpired = member.MembershipExpiryDate.HasValue && member.MembershipExpiryDate.Value < DateTime.Now;
            int daysRemaining = member.MembershipExpiryDate.HasValue
                ? (int)(member.MembershipExpiryDate.Value - DateTime.Now).TotalDays
                : 0;

            string status;
            Color color;

            if (isExpired)
            {
                status = "EXPIRED";
                color = Color.Red;
                ShowCheckInResult("MEMBERSHIP EXPIRED",
                    $"{member.FullName}\nExpired: {member.MembershipExpiryDate:dd/MM/yyyy}", color);
                Log($"Check-in DENIED: {member.FullName} - Membership expired");
                PlaySound(_errorSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }
            else
            {
                status = "OK";
                color = Color.Green;
                ShowCheckInResult("CHECK-IN SUCCESS",
                    $"{member.FullName}\n{(daysRemaining > 0 ? $"{daysRemaining} days remaining" : member.MembershipName)}", color);
                Log($"Check-in OK: {member.FullName}");
                PlaySound(_successSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }

            // Save check-in record locally
            var checkIn = new CheckInRecord
            {
                FitAddisMemberCode = member.CheckInCode,
                MemberName = member.FullName,
                CheckInTime = DateTime.Now,
                Status = status,
                IsSynced = false
            };

            _db.SaveCheckIn(checkIn);
            UpdateUI();

            Log($"Check-in saved locally (ID: {checkIn.Id}). Will sync when online.");

            // Send to API immediately
            try
            {
                var request = new CheckInRequest
                {
                    CheckInCode = member.CheckInCode,
                    CheckInTime = DateTime.Now
                };

                var response = await _api.SendCheckInAsync(request);
                if (response.Success)
                {
                    _db.MarkCheckInSynced(checkIn.Id, true, null);
                    Log($"Check-in synced to server for {member.FullName}");
                }
            }
            catch (Exception ex)
            {
                Log($"Failed to sync check-in: {ex.Message}");
            }
        }

        #endregion

        #region Enrollment

        private void btnEnrollFingerprint_Click(object sender, EventArgs e)
        {
            if (_fingerService == null || !_fingerService.IsConnected)
            {
                MessageBox.Show("Please connect the fingerprint sensor first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbFitAddisMembers.SelectedItem == null)
            {
                MessageBox.Show("Please select a member to enroll.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selected = cmbFitAddisMembers.SelectedItem;
            string checkInCode = selected.CheckInCode;

            // Check if already enrolled
            var existing = _localMembers.FirstOrDefault(m => m.FitAddisMemberCode == checkInCode);
            if (existing != null && existing.IsEnrolled)
            {
                var result = MessageBox.Show(
                    $"Member {existing.Name} is already enrolled. Re-enroll fingerprint?",
                    "Confirm Re-enrollment",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;
            }

            // Get member details from Fit Addis list
            var fitAddisMember = _fitAddisMembers.FirstOrDefault(m => m.CheckInCode == checkInCode);
            if (fitAddisMember == null)
            {
                MessageBox.Show("Member not found in Fit Addis list.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create or update local member
            _enrollingMember = existing ?? new Member();
            _enrollingMember.FitAddisMemberCode = fitAddisMember.CheckInCode;
            _enrollingMember.Name = fitAddisMember.FullName;
            _enrollingMember.Phone = fitAddisMember.PhoneNumber;
            _enrollingMember.Email = fitAddisMember.Email;
            _enrollingMember.MembershipPlan = fitAddisMember.MembershipName;
            _enrollingMember.MembershipExpiryDate = fitAddisMember.MembershipExpiryDate;

            if (_enrollingMember.Id == 0)
            {
                _enrollingMember.EnrolledDate = DateTime.Now;
                _enrollingMember.FingerprintId = _db.GetNextFingerprintId();
            }

            // Start enrollment with new SDK
            _fingerService.BeginEnroll();

            lblEnrollStatus.Text = "Scan finger 1 of 3... (Place finger on sensor)";
            lblEnrollStatus.ForeColor = Color.Blue;
            Log($"Enrolling {_enrollingMember.Name}. Please scan finger 3 times.");
        }

        private void FingerService_OnEnrollmentComplete(object sender, EnrollmentCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => FingerService_OnEnrollmentComplete(sender, e)));
                return;
            }

            if (e.Success && _enrollingMember != null)
            {
                // Save template as Base64 string
                _enrollingMember.FingerprintTemplate = ZKFingerService.TemplateToBase64(e.Template);
                _enrollingMember.FingerprintTemplate10 = _enrollingMember.FingerprintTemplate; // Same template for new SDK

                // Save to database
                _db.SaveMember(_enrollingMember);

                // Add to fingerprint cache
                _fingerService.AddTemplateToDb(_enrollingMember.FingerprintId.Value, e.Template);

                // Refresh local list
                LoadLocalMembers();

                lblEnrollStatus.Text = "Enrollment Successful!";
                lblEnrollStatus.ForeColor = Color.Green;
                Log($"Member '{_enrollingMember.Name}' enrolled successfully. Fingerprint ID: {_enrollingMember.FingerprintId}");
                PlaySound(_successSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }
            else
            {
                lblEnrollStatus.Text = $"Enrollment Failed: {e.ErrorMessage}";
                lblEnrollStatus.ForeColor = Color.Red;
                Log($"Enrollment failed: {e.ErrorMessage}");
                PlaySound(_errorSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }

            _enrollingMember = null;
        }

        private void FingerService_OnEnrollmentProgress(object sender, EnrollmentProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => FingerService_OnEnrollmentProgress(sender, e)));
                return;
            }

            // Update UI to show enrollment progress
            if (e.CurrentScan < e.TotalScans)
            {
                lblEnrollStatus.Text = $"Scan {e.CurrentScan} captured! Now scan finger {e.CurrentScan + 1} of {e.TotalScans}...";
            }
            else
            {
                lblEnrollStatus.Text = $"All {e.TotalScans} scans captured! Processing...";
            }
            lblEnrollStatus.ForeColor = Color.Green;
            Log($"Enrollment progress: {e.CurrentScan} of {e.TotalScans} scans captured");
        }

        private void FingerService_OnFingerprintCaptured(object sender, FingerprintCapturedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => FingerService_OnFingerprintCaptured(sender, e)));
                return;
            }

            // Update enrollment progress if enrolling
            if (_fingerService.IsEnrolling)
            {
                int progress = _fingerService.GetEnrollProgress();
                lblEnrollStatus.Text = $"Scan finger {progress + 1} of 3...";
                Log($"Enrollment scan {progress + 1} of 3 captured");
                return;
            }

            // Update fingerprint image
            if (e.Image != null)
            {
                picFingerprint.Image = e.Image;
            }

            // Check-in mode - 1:N fingerprint identification
            int fingerprintId = 0;
            int score = 0;
            int result = _fingerService.Identify(e.Template, out fingerprintId, out score);

            if (result != 0 || fingerprintId <= 0)
            {
                ShowCheckInResult("NOT RECOGNIZED", "Fingerprint not registered", Color.Orange);
                Log("Fingerprint not recognized.");
                PlaySound(_errorSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }
            else
            {
                var member = _db.GetMemberByFingerprintId(fingerprintId);
                if (member != null)
                {
                    ProcessCheckIn(member);
                }
                else
                {
                    ShowCheckInResult("ERROR", "Member not found in database", Color.Red);
                    Log($"Fingerprint ID {fingerprintId} found but member not in database.");
                }
            }
        }

        #endregion

        #region Check-In

        private void ProcessCheckIn(Member member)
        {
            string status;
            Color color;

            if (member.IsExpired)
            {
                status = "EXPIRED";
                color = Color.Red;
                ShowCheckInResult("MEMBERSHIP EXPIRED", 
                    $"{member.Name}\nExpired: {member.MembershipExpiryDate:dd/MM/yyyy}", color);
                Log($"Check-in DENIED: {member.Name} - Membership expired");
                PlaySound(_errorSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }
            else
            {
                status = "OK";
                color = Color.Green;
                ShowCheckInResult("CHECK-IN SUCCESS",
                    $"{member.Name}\n{member.DaysRemaining} days remaining", color);
                Log($"Check-in OK: {member.Name} - {member.DaysRemaining} days remaining");
                PlaySound(_successSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);
            }

            // Save check-in record locally
            var checkIn = new CheckInRecord
            {
                FitAddisMemberCode = member.FitAddisMemberCode,
                MemberName = member.Name,
                CheckInTime = DateTime.Now,
                Status = status,
                IsSynced = false
            };

            _db.SaveCheckIn(checkIn);
            UpdateUI();

            Log($"Check-in saved locally (ID: {checkIn.Id}). Will sync when online.");
        }

        private void ShowCheckInResult(string title, string details, Color color)
        {
            pnlCheckInResult.BackColor = color;
            lblCheckInTitle.Text = title;
            lblCheckInDetails.Text = details;
            lblCheckInTitle.ForeColor = Color.White;
            lblCheckInDetails.ForeColor = Color.White;
        }

        #endregion

        #region Sync Events

        private void SyncService_OnSyncStatusChanged(object sender, SyncEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SyncService_OnSyncStatusChanged(sender, e)));
                return;
            }

            UpdateUI();
            
            switch (e.Status)
            {
                case SyncStatus.Syncing:
                    lblSyncStatus.Text = "Syncing...";
                    lblSyncStatus.ForeColor = Color.Blue;
                    break;
                case SyncStatus.Success:
                    lblSyncStatus.Text = "All synced";
                    lblSyncStatus.ForeColor = Color.Green;
                    break;
                case SyncStatus.PartialSuccess:
                    lblSyncStatus.Text = $"Partial: {e.SyncedCount} ok, {e.FailedCount} failed";
                    lblSyncStatus.ForeColor = Color.Orange;
                    break;
                case SyncStatus.Error:
                    lblSyncStatus.Text = "Sync error";
                    lblSyncStatus.ForeColor = Color.Red;
                    break;
            }
        }

        private void SyncService_OnConnectionStatusChanged(object sender, bool isOnline)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SyncService_OnConnectionStatusChanged(sender, isOnline)));
                return;
            }

            lblConnectionStatus.Text = isOnline ? "ONLINE" : "OFFLINE";
            lblConnectionStatus.ForeColor = isOnline ? Color.Green : Color.Red;
        }

        private void SyncService_OnMembersUpdated(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SyncService_OnMembersUpdated(sender, e)));
                return;
            }

            LoadLocalMembers();
        }

        private async void btnForceSync_Click(object sender, EventArgs e)
        {
            btnForceSync.Enabled = false;
            await _syncService.ForceSyncAsync();
            btnForceSync.Enabled = true;
        }

        #endregion

        #region Check-In Logs

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            var logs = _db.GetCheckIns(dtpLogFrom.Value.Date, dtpLogTo.Value.Date.AddDays(1));
            
            dgvLogs.DataSource = null;
            dgvLogs.DataSource = logs.Select(l => new
            {
                l.Id,
                MemberCode = l.FitAddisMemberCode,
                Name = l.MemberName,
                Time = l.CheckInTime.ToString("dd/MM/yyyy HH:mm:ss"),
                l.Status,
                Synced = l.IsSynced ? "Yes" : "No",
                SyncedAt = l.SyncedAt?.ToString("dd/MM/yyyy HH:mm") ?? "",
                Error = l.SyncError ?? ""
            }).ToList();

            lblLogCount.Text = $"Showing {logs.Count} records";
        }

        private void btnExportLogs_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files|*.csv";
                sfd.FileName = $"CheckIns_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var logs = _db.GetAllCheckIns();
                    var error = _exportService.ExportCheckIns(logs, sfd.FileName);

                    if (error == null)
                    {
                        Log($"Exported {logs.Count} check-ins to {sfd.FileName}");
                        MessageBox.Show("Export successful!", "Success", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Export failed: {error}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportMembers_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files|*.csv";
                sfd.FileName = $"Members_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var error = _exportService.ExportMembers(_localMembers, sfd.FileName);

                    if (error == null)
                    {
                        Log($"Exported {_localMembers.Count} members to {sfd.FileName}");
                        MessageBox.Show("Export successful!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Export failed: {error}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region Logging

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(message)));
                return;
            }

            // Activity log commented out
            // string logLine = $"[{DateTime.Now:HH:mm:ss}] {message}";
            // txtLog.AppendText(logLine + Environment.NewLine);
            // txtLog.SelectionStart = txtLog.TextLength;
            // txtLog.ScrollToCaret();
        }

        #endregion

        #region Form Events

        private bool _forceClose = false;

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceClose && e.CloseReason == CloseReason.UserClosing)
            {
                // Minimize to tray instead of closing
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                notifyIcon.ShowBalloonTip(1000, "Fit Addis", "Application minimized to system tray. Double-click to restore.", ToolTipIcon.Info);
                return;
            }

            _syncService?.Stop();
            
            // Cleanup fingerprint service
            _fingerService?.Dispose();

            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            _db?.Dispose();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void trayMenuShow_Click(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void trayMenuExit_Click(object sender, EventArgs e)
        {
            _forceClose = true;
            Application.Exit();
        }

        public bool LogoutRequested { get; private set; } = false;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // Clear saved credentials
                string credPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "FitAddis", "credentials.json");
                if (File.Exists(credPath))
                    File.Delete(credPath);
                
                LogoutRequested = true;
                _forceClose = true;
                this.Close();
            }
        }

        private void RestoreFromTray()
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a member to delete.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int memberId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Id"].Value);
            var member = _localMembers.FirstOrDefault(m => m.Id == memberId);

            if (member == null) return;

            var result = MessageBox.Show(
                $"Delete member '{member.Name}'?\n\nThis will remove their fingerprint enrollment.",
                "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Remove from fingerprint cache
                if (_fingerService != null && member.FingerprintId.HasValue)
                {
                    _fingerService.RemoveTemplateFromDb(member.FingerprintId.Value);
                }

                _db.DeleteMember(member.Id);
                LoadLocalMembers();
                Log($"Member '{member.Name}' deleted.");
            }
        }

        #endregion
    }
}

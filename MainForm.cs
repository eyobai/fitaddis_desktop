using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GymCheckIn
{
    public partial class MainForm : Form
    {
        private int fpcHandle = 0;
        private ExcelHelper excelHelper;
        private List<Member> members = new List<Member>();
        private Member currentEnrollingMember;
        private int enrollMode = 0; // 0 = CheckIn mode, 1 = Enroll mode
        private bool testMode = false;

        private string sRegTemplate = "";
        private string sRegTemplate10 = "";

        private string successSoundPath;
        private string errorSoundPath;
        private string warningSoundPath;

        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x0001;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string dataFolder = Path.Combine(Application.StartupPath, "Data");
            excelHelper = new ExcelHelper(dataFolder);
            LoadMembersFromExcel();
            UpdateMembersList();
            dtpExpiry.Value = DateTime.Now.AddMonths(1);
            
            // Initialize sound paths from Windows
            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            successSoundPath = Path.Combine(winDir, @"Media\chord.wav");
            errorSoundPath = Path.Combine(winDir, @"Media\Windows Background.wav");
            warningSoundPath = Path.Combine(winDir, @"Media\ding.wav");
            
            // Auto-connect to fingerprint sensor
            ConnectToSensor(showErrors: false);
        }

        private void LoadMembersFromExcel()
        {
            members = excelHelper.LoadMembers();
        }

        private void UpdateMembersList()
        {
            lstMembers.Items.Clear();
            foreach (var member in members)
            {
                string status = member.IsExpired ? " [EXPIRED]" : $" [{member.DaysRemaining} days left]";
                lstMembers.Items.Add($"{member.Id} - {member.Name} - {member.Phone}{status}");
            }
            lblMemberCount.Text = $"Total Members: {members.Count}";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToSensor(showErrors: true);
        }

        private void ConnectToSensor(bool showErrors)
        {
            try
            {
                if (axZKFPEngX1.InitEngine() == 0)
                {
                    int sensorCount = axZKFPEngX1.SensorCount;
                    if (sensorCount > 0)
                    {
                        axZKFPEngX1.SensorIndex = 0;
                        fpcHandle = axZKFPEngX1.CreateFPCacheDBEx();
                        
                        // Load existing fingerprints into cache
                        foreach (var member in members)
                        {
                            if (!string.IsNullOrEmpty(member.FingerprintTemplate) && 
                                !string.IsNullOrEmpty(member.FingerprintTemplate10))
                            {
                                axZKFPEngX1.AddRegTemplateStrToFPCacheDBEx(fpcHandle, member.Id, 
                                    member.FingerprintTemplate, member.FingerprintTemplate10);
                            }
                        }

                        btnConnect.Enabled = false;
                        btnDisconnect.Enabled = true;
                        EnableControls(true);
                        UpdateStatus($"Sensor connected. {sensorCount} sensor(s) found. {members.Count} fingerprints loaded.");
                    }
                    else
                    {
                        if (showErrors)
                            MessageBox.Show("No fingerprint sensor detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                            UpdateStatus("No fingerprint sensor detected. Click Connect when ready.");
                        axZKFPEngX1.EndEngine();
                    }
                }
                else
                {
                    if (showErrors)
                        MessageBox.Show("Failed to initialize fingerprint engine!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        UpdateStatus("Failed to initialize fingerprint engine. Click Connect to retry.");
                }
            }
            catch (Exception ex)
            {
                if (showErrors)
                    MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    UpdateStatus($"Connection error: {ex.Message}");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (fpcHandle != 0)
                {
                    axZKFPEngX1.FreeFPCacheDB(fpcHandle);
                    fpcHandle = 0;
                }
                axZKFPEngX1.EndEngine();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                EnableControls(false);
                UpdateStatus("Sensor disconnected.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Disconnect error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableControls(bool enabled)
        {
            btnRegister.Enabled = enabled;
            grpNewMember.Enabled = enabled;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter member name!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter phone number!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            currentEnrollingMember = new Member
            {
                Id = excelHelper.GetNextMemberId(),
                Name = txtName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                RegistrationDate = DateTime.Now,
                ExpiryDate = dtpExpiry.Value.Date.AddDays(1).AddSeconds(-1)
            };

            enrollMode = 1;
            sRegTemplate = "";
            sRegTemplate10 = "";

            if (axZKFPEngX1.IsRegister)
                axZKFPEngX1.CancelEnroll();

            axZKFPEngX1.EnrollCount = 3;
            axZKFPEngX1.BeginEnroll();

            UpdateStatus($"Enrolling {currentEnrollingMember.Name}. Please scan finger 3 times...");
            lblEnrollStatus.Text = "Scan 1 of 3";
            lblEnrollStatus.ForeColor = Color.Blue;
        }

        private void axZKFPEngX1_OnEnroll(object sender, AxZKFPEngXControl.IZKFPEngXEvents_OnEnrollEvent e)
        {
            if (!e.actionResult)
            {
                UpdateStatus("Enrollment failed. Please try again.");
                lblEnrollStatus.Text = "Failed";
                lblEnrollStatus.ForeColor = Color.Red;
                enrollMode = 0;
                return;
            }

            if (enrollMode == 1 && currentEnrollingMember != null)
            {
                sRegTemplate = axZKFPEngX1.GetTemplateAsStringEx("9");
                sRegTemplate10 = axZKFPEngX1.GetTemplateAsStringEx("10");

                if (!string.IsNullOrEmpty(sRegTemplate) && !string.IsNullOrEmpty(sRegTemplate10))
                {
                    currentEnrollingMember.FingerprintTemplate = sRegTemplate;
                    currentEnrollingMember.FingerprintTemplate10 = sRegTemplate10;

                    // Add to cache
                    axZKFPEngX1.AddRegTemplateStrToFPCacheDBEx(fpcHandle, currentEnrollingMember.Id,
                        sRegTemplate, sRegTemplate10);

                    // Save to Excel
                    excelHelper.SaveMember(currentEnrollingMember);
                    members.Add(currentEnrollingMember);
                    UpdateMembersList();

                    UpdateStatus($"Member '{currentEnrollingMember.Name}' registered successfully! ID: {currentEnrollingMember.Id}");
                    lblEnrollStatus.Text = "Success!";
                    lblEnrollStatus.ForeColor = Color.Green;

                    // Clear form
                    txtName.Clear();
                    txtPhone.Clear();
                    dtpExpiry.Value = DateTime.Now.AddMonths(1);

                    axZKFPEngX1.ControlSensor(11, 1);  // Green LED
                    axZKFPEngX1.ControlSensor(13, 1);  // Beep
                }
                else
                {
                    UpdateStatus("Failed to capture fingerprint template.");
                    lblEnrollStatus.Text = "Failed";
                    lblEnrollStatus.ForeColor = Color.Red;
                }

                currentEnrollingMember = null;
                enrollMode = 0;
            }
        }

        private void axZKFPEngX1_OnCapture(object sender, AxZKFPEngXControl.IZKFPEngXEvents_OnCaptureEvent e)
        {
            if (enrollMode == 1)
            {
                int remaining = axZKFPEngX1.EnrollIndex;
                lblEnrollStatus.Text = $"Scan {4 - remaining} of 3";
                return;
            }

            // Check-in mode (1:N identification)
            if (fpcHandle == 0) return;

            int score = 0;
            int processedNum = 1;
            int memberId = axZKFPEngX1.IdentificationInFPCacheDB(fpcHandle, e.aTemplate, ref score, ref processedNum);

            if (memberId == -1)
            {
                UpdateStatus("Fingerprint not recognized. Please register first.");
                pnlCheckInResult.BackColor = Color.Orange;
                lblCheckInResult.Text = "NOT RECOGNIZED";
                lblCheckInResult.ForeColor = Color.White;
                lblMemberInfo.Text = "";
                axZKFPEngX1.ControlSensor(12, 1);  // Red LED
                axZKFPEngX1.ControlSensor(13, 1);  // Beep
                PlaySound(warningSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);  // Warning sound
            }
            else
            {
                Member member = members.Find(m => m.Id == memberId);
                if (member != null)
                {
                    ProcessCheckIn(member);
                }
            }
        }

        private void ProcessCheckIn(Member member)
        {
            member.LastCheckIn = DateTime.Now;

            if (member.IsExpired)
            {
                pnlCheckInResult.BackColor = Color.Red;
                lblCheckInResult.Text = "MEMBERSHIP EXPIRED";
                lblCheckInResult.ForeColor = Color.White;
                lblMemberInfo.Text = $"{member.Name}\nExpired: {member.ExpiryDate:dd/MM/yyyy}";
                excelHelper.LogCheckIn(member, "EXPIRED");
                UpdateStatus($"Check-in DENIED: {member.Name} - Membership expired on {member.ExpiryDate:dd/MM/yyyy}");
                axZKFPEngX1.ControlSensor(12, 1);  // Red LED
                axZKFPEngX1.ControlSensor(13, 1);  // Beep
                PlaySound(errorSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);  // Error sound
            }
            else
            {
                pnlCheckInResult.BackColor = Color.Green;
                lblCheckInResult.Text = "CHECK-IN SUCCESS";
                lblCheckInResult.ForeColor = Color.White;
                lblMemberInfo.Text = $"{member.Name}\n{member.DaysRemaining} days remaining";
                excelHelper.LogCheckIn(member, "OK");
                UpdateStatus($"Check-in OK: {member.Name} - {member.DaysRemaining} days remaining");
                axZKFPEngX1.ControlSensor(11, 1);  // Green LED
                axZKFPEngX1.ControlSensor(13, 1);  // Beep
                MessageBox.Show($"Playing: {successSoundPath}");  // DEBUG
                PlaySound(successSoundPath, IntPtr.Zero, SND_FILENAME | SND_ASYNC);  // Success sound
            }
        }

        private void axZKFPEngX1_OnImageReceived(object sender, AxZKFPEngXControl.IZKFPEngXEvents_OnImageReceivedEvent e)
        {
            if (e.aImageValid)
            {
                Bitmap bmp = new Bitmap(axZKFPEngX1.ImageWidth, axZKFPEngX1.ImageHeight);
                Graphics g = Graphics.FromImage(bmp);
                axZKFPEngX1.PrintImageAt(g.GetHdc().ToInt32(), 0, 0, bmp.Width, bmp.Height);
                g.ReleaseHdc();
                picFingerprint.Image = bmp;
            }
        }

        private void axZKFPEngX1_OnFeatureInfo(object sender, AxZKFPEngXControl.IZKFPEngXEvents_OnFeatureInfoEvent e)
        {
            // Feature info received
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if (lstMembers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a member to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedMember = members[lstMembers.SelectedIndex];
            var result = MessageBox.Show($"Are you sure you want to delete member '{selectedMember.Name}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (fpcHandle != 0)
                {
                    axZKFPEngX1.RemoveRegTemplateFromFPCacheDB(fpcHandle, selectedMember.Id);
                }
                excelHelper.DeleteMember(selectedMember.Id);
                members.Remove(selectedMember);
                UpdateMembersList();
                UpdateStatus($"Member '{selectedMember.Name}' deleted.");
            }
        }

        private void btnExtendMembership_Click(object sender, EventArgs e)
        {
            if (lstMembers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a member to extend.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedMember = members[lstMembers.SelectedIndex];
            
            using (var form = new Form())
            {
                form.Text = "Extend Membership";
                form.Size = new Size(300, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var label = new Label { Text = "New Expiry Date:", Location = new Point(20, 20), AutoSize = true };
                var datePicker = new DateTimePicker { Location = new Point(20, 45), Width = 240, Value = selectedMember.ExpiryDate.AddMonths(1) };
                var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(100, 80), Width = 80 };

                form.Controls.AddRange(new Control[] { label, datePicker, btnOk });
                form.AcceptButton = btnOk;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    selectedMember.ExpiryDate = datePicker.Value.Date.AddDays(1).AddSeconds(-1);
                    excelHelper.UpdateMember(selectedMember);
                    UpdateMembersList();
                    UpdateStatus($"Membership extended for '{selectedMember.Name}' until {selectedMember.ExpiryDate:dd/MM/yyyy}");
                }
            }
        }

        private void btnViewCheckIns_Click(object sender, EventArgs e)
        {
            var records = excelHelper.LoadCheckIns(DateTime.Today, DateTime.Today.AddDays(1));
            
            string message = $"Today's Check-ins ({DateTime.Today:dd/MM/yyyy}):\n\n";
            if (records.Count == 0)
            {
                message += "No check-ins today.";
            }
            else
            {
                foreach (var record in records)
                {
                    message += $"{record.CheckInTime:HH:mm} - {record.MemberName} [{record.Status}]\n";
                }
            }

            MessageBox.Show(message, "Today's Check-ins", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnOpenDataFolder_Click(object sender, EventArgs e)
        {
            string dataFolder = Path.Combine(Application.StartupPath, "Data");
            if (Directory.Exists(dataFolder))
            {
                System.Diagnostics.Process.Start("explorer.exe", dataFolder);
            }
        }

        private void UpdateStatus(string message)
        {
            statusBar.Text = message;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fpcHandle != 0)
            {
                axZKFPEngX1.FreeFPCacheDB(fpcHandle);
            }
            axZKFPEngX1.EndEngine();
        }

        private void lstMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteMember.Enabled = lstMembers.SelectedIndex >= 0;
            btnExtendMembership.Enabled = lstMembers.SelectedIndex >= 0;
        }
        private void btnTestMode_Click(object sender, EventArgs e)
        {
            testMode = !testMode;
            btnTestMode.Text = testMode ? "Test Mode: ON" : "Test Mode: OFF";
            btnTestMode.BackColor = testMode ? Color.Orange : SystemColors.Control;
            
            if (testMode)
            {
                EnableControls(true);
                btnSimulateCheckIn.Visible = true;
                UpdateStatus("TEST MODE enabled - No fingerprint device required");
            }
            else
            {
                btnSimulateCheckIn.Visible = false;
                UpdateStatus("TEST MODE disabled");
            }
        }

        private void btnSimulateCheckIn_Click(object sender, EventArgs e)
        {
            if (lstMembers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a member from the list to simulate check-in.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedMember = members[lstMembers.SelectedIndex];
            ProcessCheckIn(selectedMember);
        }
    }
}





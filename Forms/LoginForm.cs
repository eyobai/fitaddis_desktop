using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GymCheckIn.Models;
using GymCheckIn.Services;
using GymCheckIn.UI;
using Newtonsoft.Json;

namespace GymCheckIn.Forms
{
    public partial class LoginForm : Form
    {
        public LoginResponse LoginResult { get; private set; }
        private static string CredentialsPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FitAddis", "credentials.json");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            TryAutoLogin();
        }

        private async void TryAutoLogin()
        {
            var saved = LoadSavedCredentials();
            if (saved != null)
            {
                txtPhoneNumber.Text = saved.PhoneNumber;
                txtPassword.Text = saved.Password;
                
                // Try auto-login
                lblError.Text = "Logging in...";
                lblError.ForeColor = ThemeManager.TextSecondary;
                lblError.Visible = true;
                btnLogin.Enabled = false;

                try
                {
                    var (response, error) = await FitAddisApiService.LoginAsync(saved.PhoneNumber, saved.Password);
                    if (response != null && response.FitnessCenter != null)
                    {
                        LoginResult = response;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                }
                catch { }
                
                lblError.Text = "Session expired. Please login again.";
                lblError.ForeColor = ThemeManager.AccentRed;
                btnLogin.Enabled = true;
            }
            txtPhoneNumber.Focus();
        }

        private void ApplyTheme()
        {
            this.BackColor = ThemeManager.BackgroundLight;
            
            pnlLogin.BackColor = ThemeManager.CardBackground;
            
            ThemeManager.StylePrimaryButton(btnLogin);
            ThemeManager.StyleTextBox(txtPhoneNumber);
            ThemeManager.StyleTextBox(txtPassword);
            
            lblTitle.ForeColor = ThemeManager.PrimaryColor;
            lblSubtitle.ForeColor = ThemeManager.TextSecondary;
            lblPhoneNumber.ForeColor = ThemeManager.TextPrimary;
            lblPassword.ForeColor = ThemeManager.TextPrimary;
            lblError.ForeColor = ThemeManager.AccentRed;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string phoneNumber = txtPhoneNumber.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                ShowError("Please enter your phone number.");
                txtPhoneNumber.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Please enter your password.");
                txtPassword.Focus();
                return;
            }

            lblError.Visible = false;
            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";

            try
            {
                var (response, error) = await FitAddisApiService.LoginAsync(phoneNumber, password);

                if (response != null && response.FitnessCenter != null)
                {
                    LoginResult = response;
                    SaveCredentials(phoneNumber, password);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError(error ?? "Invalid phone number or password.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Connection error: {ex.Message}");
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Login";
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnLogin_Click(sender, e);
            }
        }

        private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                txtPassword.Focus();
            }
        }

        private void SaveCredentials(string phoneNumber, string password)
        {
            try
            {
                var dir = Path.GetDirectoryName(CredentialsPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var data = new SavedCredentials { PhoneNumber = phoneNumber, Password = password };
                File.WriteAllText(CredentialsPath, JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save credentials: {ex.Message}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private SavedCredentials LoadSavedCredentials()
        {
            try
            {
                if (File.Exists(CredentialsPath))
                {
                    var json = File.ReadAllText(CredentialsPath);
                    return JsonConvert.DeserializeObject<SavedCredentials>(json);
                }
            }
            catch { }
            return null;
        }

        private class SavedCredentials
        {
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }
    }
}

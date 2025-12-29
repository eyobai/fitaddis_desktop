using System;
using System.Windows.Forms;
using GymCheckIn.Forms;

namespace GymCheckIn
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                bool continueRunning = true;
                
                while (continueRunning)
                {
                    using (var loginForm = new LoginForm())
                    {
                        if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoginResult != null)
                        {
                            var mainForm = new MainForm(loginForm.LoginResult);
                            Application.Run(mainForm);
                            
                            // Check if user requested logout
                            continueRunning = mainForm.LogoutRequested;
                        }
                        else
                        {
                            continueRunning = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application Error:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

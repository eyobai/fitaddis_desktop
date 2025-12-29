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
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoginResult != null)
                    {
                        Application.Run(new MainForm(loginForm.LoginResult));
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

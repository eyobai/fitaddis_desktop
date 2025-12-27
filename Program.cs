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

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoginResult != null)
                {
                    Application.Run(new MainForm(loginForm.LoginResult));
                }
            }
        }
    }
}

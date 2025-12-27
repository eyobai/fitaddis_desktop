using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymCheckIn.UI
{
    public static class ThemeManager
    {
        // Primary Colors - Modern Blue Theme
        public static Color PrimaryColor = Color.FromArgb(41, 128, 185);      // Professional Blue
        public static Color PrimaryDark = Color.FromArgb(31, 97, 141);        // Darker Blue
        public static Color PrimaryLight = Color.FromArgb(52, 152, 219);      // Lighter Blue
        
        // Accent Colors
        public static Color AccentGreen = Color.FromArgb(39, 174, 96);        // Success Green
        public static Color AccentRed = Color.FromArgb(231, 76, 60);          // Error/Expired Red
        public static Color AccentOrange = Color.FromArgb(243, 156, 18);      // Warning Orange
        public static Color AccentPurple = Color.FromArgb(142, 68, 173);      // Purple accent
        
        // Neutral Colors
        public static Color BackgroundLight = Color.FromArgb(248, 249, 250);  // Light gray background
        public static Color BackgroundWhite = Color.White;
        public static Color CardBackground = Color.White;
        public static Color BorderColor = Color.FromArgb(222, 226, 230);      // Light border
        public static Color TextPrimary = Color.FromArgb(33, 37, 41);         // Dark text
        public static Color TextSecondary = Color.FromArgb(108, 117, 125);    // Gray text
        public static Color TextLight = Color.White;
        
        // Fonts
        public static Font HeaderFont = new Font("Segoe UI", 24F, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font TitleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font SmallFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        
        // Sizing
        public static int BorderRadius = 8;
        public static int ButtonHeight = 40;
        public static int InputHeight = 35;
        public static Padding DefaultPadding = new Padding(15);
        
        public static void ApplyTheme(Form form)
        {
            form.BackColor = BackgroundLight;
            form.Font = BodyFont;
        }
        
        public static void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = TextLight;
            btn.Font = ButtonFont;
            btn.Height = ButtonHeight;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = PrimaryDark;
            btn.FlatAppearance.MouseDownBackColor = PrimaryDark;
        }
        
        public static void StyleSuccessButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = AccentGreen;
            btn.ForeColor = TextLight;
            btn.Font = ButtonFont;
            btn.Height = ButtonHeight;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 132, 73);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 132, 73);
        }
        
        public static void StyleDangerButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = AccentRed;
            btn.ForeColor = TextLight;
            btn.Font = ButtonFont;
            btn.Height = ButtonHeight;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
        }
        
        public static void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BorderColor;
            btn.BackColor = BackgroundWhite;
            btn.ForeColor = TextPrimary;
            btn.Font = ButtonFont;
            btn.Height = ButtonHeight;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = BackgroundLight;
            btn.FlatAppearance.MouseDownBackColor = BackgroundLight;
        }
        
        public static void StyleCard(Panel panel)
        {
            panel.BackColor = CardBackground;
            panel.Padding = DefaultPadding;
        }
        
        public static void StyleGroupBox(GroupBox grp)
        {
            grp.BackColor = CardBackground;
            grp.ForeColor = TextPrimary;
            grp.Font = TitleFont;
            grp.Padding = new Padding(10);
        }
        
        public static void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = BodyFont;
            txt.Height = InputHeight;
        }
        
        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = BodyFont;
            cmb.Height = InputHeight;
        }
        
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = CardBackground;
            dgv.GridColor = BorderColor;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            dgv.DefaultCellStyle.BackColor = BackgroundWhite;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = BodyFont;
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = TextLight;
            dgv.DefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = BackgroundLight;
            
            dgv.RowTemplate.Height = 40;
            dgv.RowHeadersVisible = false;
        }
        
        public static void StyleTabControl(TabControl tab)
        {
            tab.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            tab.Padding = new Point(20, 10);
        }
        
        public static Panel CreateStatusIndicator(string text, bool isSuccess)
        {
            var panel = new Panel
            {
                Height = 30,
                AutoSize = true,
                BackColor = isSuccess ? AccentGreen : AccentRed,
                Padding = new Padding(10, 5, 10, 5)
            };
            
            var label = new Label
            {
                Text = text,
                ForeColor = TextLight,
                Font = SmallFont,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            panel.Controls.Add(label);
            return panel;
        }
    }
}

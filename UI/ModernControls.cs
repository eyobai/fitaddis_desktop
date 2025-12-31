using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GymCheckIn.UI
{
    /// <summary>
    /// Modern rounded panel with optional border and shadow
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 12;
        private Color _borderColor = ThemeManager.SurfaceBorder;
        private int _borderWidth = 1;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color PanelBorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public int PanelBorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = value; Invalidate(); }
        }

        public RoundedPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = ThemeManager.Surface;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.Background);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = CreateRoundedRectangle(rect, _borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    g.FillPath(brush, path);
                }
                if (_borderWidth > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Gradient panel for Access Denied / Status messages
    /// </summary>
    public class GradientPanel : Panel
    {
        private Color _gradientStart = ThemeManager.GradientStart;
        private Color _gradientEnd = ThemeManager.GradientEnd;
        private int _borderRadius = 12;
        private LinearGradientMode _gradientMode = LinearGradientMode.Horizontal;

        public Color GradientStart
        {
            get => _gradientStart;
            set { _gradientStart = value; Invalidate(); }
        }

        public Color GradientEnd
        {
            get => _gradientEnd;
            set { _gradientEnd = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public LinearGradientMode GradientMode
        {
            get => _gradientMode;
            set { _gradientMode = value; Invalidate(); }
        }

        public GradientPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.Background);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = CreateRoundedRectangle(rect, _borderRadius))
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, _gradientStart, _gradientEnd, _gradientMode))
            {
                g.FillPath(brush, path);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Circular panel with glow effect for fingerprint display
    /// </summary>
    public class GlowCirclePanel : Panel
    {
        private Color _glowColor = ThemeManager.PrimaryColor;
        private Color _innerColor = ThemeManager.Surface;
        private int _glowSize = 15;
        private bool _isGlowing = true;

        public Color GlowColor
        {
            get => _glowColor;
            set { _glowColor = value; Invalidate(); }
        }

        public Color InnerColor
        {
            get => _innerColor;
            set { _innerColor = value; Invalidate(); }
        }

        public int GlowSize
        {
            get => _glowSize;
            set { _glowSize = value; Invalidate(); }
        }

        public bool IsGlowing
        {
            get => _isGlowing;
            set { _isGlowing = value; Invalidate(); }
        }

        public GlowCirclePanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | 
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(200, 200);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int size = Math.Min(Width, Height) - _glowSize * 2;
            int x = (Width - size) / 2;
            int y = (Height - size) / 2;

            // Draw glow effect
            if (_isGlowing)
            {
                for (int i = _glowSize; i > 0; i--)
                {
                    int alpha = (int)(80 * (1 - (float)i / _glowSize));
                    using (Pen glowPen = new Pen(Color.FromArgb(alpha, _glowColor), 2))
                    {
                        g.DrawEllipse(glowPen, x - i, y - i, size + i * 2, size + i * 2);
                    }
                }
            }

            // Draw inner circle
            using (SolidBrush innerBrush = new SolidBrush(_innerColor))
            {
                g.FillEllipse(innerBrush, x, y, size, size);
            }

            // Draw border
            using (Pen borderPen = new Pen(_glowColor, 2))
            {
                g.DrawEllipse(borderPen, x, y, size, size);
            }
        }
    }

    /// <summary>
    /// Status indicator with glow effect
    /// </summary>
    public class StatusIndicator : Control
    {
        private bool _isOnline = false;
        private string _statusText = "OFFLINE";

        public bool IsOnline
        {
            get => _isOnline;
            set { _isOnline = value; _statusText = value ? "CONNECTED" : "DISCONNECTED"; Invalidate(); }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; Invalidate(); }
        }

        public StatusIndicator()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(180, 24);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color dotColor = _isOnline ? ThemeManager.AccentGreen : ThemeManager.AccentRed;
            
            // Draw glow behind dot
            for (int i = 8; i > 0; i--)
            {
                int alpha = (int)(60 * (1 - (float)i / 8));
                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(alpha, dotColor)))
                {
                    g.FillEllipse(glowBrush, 2 - i, (Height - 12) / 2 - i, 12 + i * 2, 12 + i * 2);
                }
            }

            // Draw status dot
            using (SolidBrush dotBrush = new SolidBrush(dotColor))
            {
                g.FillEllipse(dotBrush, 2, (Height - 12) / 2, 12, 12);
            }

            // Draw text
            using (Font font = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(ThemeManager.TextPrimary))
            {
                g.DrawString(_statusText, font, textBrush, 20, (Height - font.Height) / 2);
            }
        }
    }

    /// <summary>
    /// Modern flat button with hover effects
    /// </summary>
    public class ModernButton : Button
    {
        private int _borderRadius = 6;
        private bool _isHovering = false;
        private bool _isPressed = false;
        private bool _isGhostButton = false;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public bool IsGhostButton
        {
            get => _isGhostButton;
            set { _isGhostButton = value; Invalidate(); }
        }

        public ModernButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = ThemeManager.PrimaryColor;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Cursor = Cursors.Hand;
            Size = new Size(120, 38);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.Background);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color bgColor = BackColor;

            if (!Enabled)
                bgColor = ThemeManager.SurfaceLight;
            else if (_isPressed)
                bgColor = ControlPaint.Dark(BackColor, 0.15f);
            else if (_isHovering)
                bgColor = _isGhostButton ? Color.FromArgb(40, BackColor) : ControlPaint.Light(BackColor, 0.1f);

            using (GraphicsPath path = CreateRoundedRectangle(rect, _borderRadius))
            {
                if (_isGhostButton && !_isHovering)
                {
                    // Ghost button - border only
                    using (Pen borderPen = new Pen(BackColor, 2))
                    {
                        g.DrawPath(borderPen, path);
                    }
                }
                else
                {
                    using (SolidBrush brush = new SolidBrush(bgColor))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }

            // Draw text
            Color textColor = (_isGhostButton && !_isHovering) ? BackColor : ForeColor;
            TextRenderer.DrawText(g, Text, Font, rect, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isPressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Modern search textbox with placeholder and rounded border
    /// </summary>
    public class ModernSearchBox : UserControl
    {
        private TextBox _innerTextBox;
        private string _placeholder = "Search...";
        private bool _isFocused = false;

        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; Invalidate(); }
        }

        public override string Text
        {
            get => _innerTextBox.Text;
            set => _innerTextBox.Text = value;
        }

        public event EventHandler SearchTextChanged;

        public ModernSearchBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            
            Size = new Size(200, 36);
            BackColor = ThemeManager.SurfaceLight;

            _innerTextBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = ThemeManager.SurfaceLight,
                ForeColor = ThemeManager.TextPrimary,
                Font = new Font("Segoe UI", 10F)
            };

            _innerTextBox.GotFocus += (s, e) => { _isFocused = true; Invalidate(); };
            _innerTextBox.LostFocus += (s, e) => { _isFocused = false; Invalidate(); };
            _innerTextBox.TextChanged += (s, e) => { SearchTextChanged?.Invoke(this, e); Invalidate(); };

            Controls.Add(_innerTextBox);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _innerTextBox.Location = new Point(35, (Height - _innerTextBox.Height) / 2);
            _innerTextBox.Width = Width - 45;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? ThemeManager.Background);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color borderColor = _isFocused ? ThemeManager.PrimaryColor : ThemeManager.SurfaceBorder;

            using (GraphicsPath path = CreateRoundedRectangle(rect, 8))
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    g.FillPath(brush, path);
                }
                using (Pen pen = new Pen(borderColor, _isFocused ? 2 : 1))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Draw search icon
            using (Pen iconPen = new Pen(ThemeManager.TextSecondary, 2))
            {
                g.DrawEllipse(iconPen, 10, 10, 12, 12);
                g.DrawLine(iconPen, 20, 20, 25, 25);
            }

            // Draw placeholder
            if (string.IsNullOrEmpty(_innerTextBox.Text) && !_isFocused)
            {
                using (SolidBrush brush = new SolidBrush(ThemeManager.TextSecondary))
                {
                    g.DrawString(_placeholder, _innerTextBox.Font, brush, 35, (Height - _innerTextBox.Font.Height) / 2);
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Modern badge for status display
    /// </summary>
    public class StatusBadge : Control
    {
        private string _text = "ONLINE";
        private Color _badgeColor = ThemeManager.AccentGreen;

        public string BadgeText
        {
            get => _text;
            set { _text = value; Invalidate(); }
        }

        public Color BadgeColor
        {
            get => _badgeColor;
            set { _badgeColor = value; Invalidate(); }
        }

        public StatusBadge()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Size = new Size(80, 24);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            
            using (GraphicsPath path = CreateRoundedRectangle(rect, Height / 2))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(40, _badgeColor)))
            {
                g.FillPath(brush, path);
            }

            using (Font font = new Font("Segoe UI", 8F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(_badgeColor))
            {
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(_text, font, textBrush, rect, sf);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}

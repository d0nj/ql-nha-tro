using System.Drawing.Drawing2D;

namespace QLNhaTro.UserControls
{
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 12;
        public Color BorderColor { get; set; } = Color.FromArgb(40, 50, 70);
        public int ShadowDepth { get; set; } = 3;
        public Color ShadowColor { get; set; } = Color.FromArgb(20, 0, 0, 0);
        public bool UseGradient { get; set; } = false;
        public Color GradientStartColor { get; set; } = Color.FromArgb(30, 41, 59);
        public Color GradientEndColor { get; set; } = Color.FromArgb(15, 23, 42);
        public float GradientAngle { get; set; } = 135F;

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
        }

        private GraphicsPath GetRoundedRect(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var shadowRect = new RectangleF(ShadowDepth, ShadowDepth, Width - ShadowDepth * 2, Height - ShadowDepth * 2);
            for (int i = ShadowDepth; i > 0; i--)
            {
                var sr = new RectangleF(shadowRect.X - i, shadowRect.Y - i, shadowRect.Width + i * 2, shadowRect.Height + i * 2);
                using var sp = GetRoundedRect(sr, BorderRadius + i);
                using var sb = new SolidBrush(Color.FromArgb(10 * (ShadowDepth - i + 1), ShadowColor.R, ShadowColor.G, ShadowColor.B));
                e.Graphics.FillPath(sb, sp);
            }

            var rect = new RectangleF(ShadowDepth, ShadowDepth, Width - ShadowDepth * 2 - 1, Height - ShadowDepth * 2 - 1);
            using var path = GetRoundedRect(rect, BorderRadius);

            if (UseGradient)
            {
                using var gradBrush = new LinearGradientBrush(rect, GradientStartColor, GradientEndColor, GradientAngle);
                e.Graphics.FillPath(gradBrush, path);
            }
            else
            {
                using var bgBrush = new SolidBrush(BackColor == Color.Transparent ? Color.White : BackColor);
                e.Graphics.FillPath(bgBrush, path);
            }

            if (BorderColor != Color.Transparent)
            {
                using var borderPen = new Pen(BorderColor, 1);
                e.Graphics.DrawPath(borderPen, path);
            }

            this.Region = null;
        }
    }
}

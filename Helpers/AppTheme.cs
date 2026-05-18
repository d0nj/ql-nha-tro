using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace QLNhaTro.Helpers
{
    public static class AppTheme
    {
        public static Color SidebarBg = Color.FromArgb(19, 42, 40);
        public static Color SidebarLogoBg = Color.FromArgb(19, 42, 40);
        public static Color SidebarLogoText = Color.FromArgb(247, 244, 235); // Warm Ivory
        public static Color SidebarItemDefault = Color.FromArgb(106, 130, 126); // Muted Teal
        public static Color SidebarItemHover = Color.FromArgb(30, 63, 60);
        public static Color SidebarItemActive = Color.FromArgb(217, 108, 74); // Terracotta
        public static Color SidebarItemActiveText = Color.White;
        public static Color SidebarAccentStripe = Color.FromArgb(242, 157, 82); // Ochre
        public static Color SidebarFooter = Color.FromArgb(106, 130, 126);

        public static Color HeaderBg = Color.FromArgb(247, 244, 235); // Warm Ivory
        public static Color HeaderBorder = Color.FromArgb(212, 203, 179);

        public static Color ContentBg = Color.FromArgb(235, 230, 216); // Beige/Sand

        public static Color CardBg = Color.FromArgb(247, 244, 235);
        public static Color CardBorder = Color.FromArgb(212, 203, 179);

        public static Color TextPrimary = Color.FromArgb(45, 58, 56);
        public static Color TextSecondary = Color.FromArgb(102, 112, 110);
        public static Color TextMuted = Color.FromArgb(154, 163, 161);

        public static Color AccentBlue = Color.FromArgb(44, 94, 88); // Dark Teal
        public static Color AccentGreen = Color.FromArgb(92, 124, 89); // Muted Moss
        public static Color AccentAmber = Color.FromArgb(217, 138, 74); // Burnt Orange
        public static Color AccentRed = Color.FromArgb(163, 59, 59); // Muted Crimson
        public static Color AccentPurple = Color.FromArgb(111, 82, 107); // Muted Plum
        public static Color AccentIndigo = Color.FromArgb(69, 90, 115); // Muted Navy
        public static Color AccentCyan = Color.FromArgb(106, 130, 126);

        public static Color DgvHeaderBg = Color.FromArgb(247, 244, 235);
        public static Color DgvHeaderFg = Color.FromArgb(45, 58, 56);
        public static Color DgvAltRow = Color.FromArgb(242, 239, 230);
        public static Color DgvGridLine = Color.FromArgb(212, 203, 179);
        public static Color DgvSelectionBg = Color.FromArgb(44, 94, 88);
        public static Color DgvSelectionFg = Color.FromArgb(247, 244, 235);

        public static Color StatusBg = Color.FromArgb(247, 244, 235);
        public static Color StatusFg = Color.FromArgb(102, 112, 110);

        public static Color InputBg = Color.White;
        public static Color InputFg = Color.FromArgb(45, 58, 56);
        public static Color InputBorder = Color.FromArgb(212, 203, 179);

        public static readonly Font FontTitle = new("Segoe UI Semibold", 18F, FontStyle.Bold);
        public static readonly Font FontHeader = new("Segoe UI Semibold", 13F, FontStyle.Bold);
        public static readonly Font FontBody = new("Segoe UI", 10.5F);
        public static readonly Font FontSmall = new("Segoe UI", 9F);
        public static readonly Font FontSmallBold = new("Segoe UI Semibold", 9F, FontStyle.Bold);
        public static readonly Font FontSubtitle = new("Segoe UI", 14F, FontStyle.Regular);
        public static readonly Font FontSidebar = new("Segoe UI", 11F);
        public static readonly Font FontSidebarActive = new("Segoe UI Semibold", 11F, FontStyle.Bold);
        public static readonly Font FontDashboardValue = new("Segoe UI", 28F, FontStyle.Bold);
        public static readonly Font FontCardValue = new("Segoe UI", 22F, FontStyle.Bold);
        public static readonly Font FontCardLabel = new("Segoe UI Semibold", 9F, FontStyle.Bold);
        public static readonly Font FontDgvHeader = new("Segoe UI Semibold", 10F, FontStyle.Bold);

        public enum ThemeMode { Light, Dark }

        public static ThemeMode CurrentMode { get; private set; } = ThemeMode.Light;
        public static event EventHandler? ThemeChanged;
        public static bool IsDark => CurrentMode == ThemeMode.Dark;

        static AppTheme()
        {
            var saved = LoadPersistedMode();
            ApplyMode(saved, raiseEvent: false);
        }

        public static void ToggleMode() => ApplyMode(IsDark ? ThemeMode.Light : ThemeMode.Dark);

        public static void ApplyMode(ThemeMode mode, bool raiseEvent = true)
        {
            CurrentMode = mode;
            if (mode == ThemeMode.Dark) ApplyDarkPalette();
            else ApplyLightPalette();
            SavePersistedMode(mode);
            if (raiseEvent) ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        private static void ApplyLightPalette()
        {
            SidebarBg = Color.FromArgb(19, 42, 40);
            SidebarLogoBg = Color.FromArgb(19, 42, 40);
            SidebarLogoText = Color.FromArgb(247, 244, 235);
            SidebarItemDefault = Color.FromArgb(106, 130, 126);
            SidebarItemHover = Color.FromArgb(30, 63, 60);
            SidebarItemActive = Color.FromArgb(217, 108, 74);
            SidebarItemActiveText = Color.White;
            SidebarAccentStripe = Color.FromArgb(242, 157, 82);
            SidebarFooter = Color.FromArgb(106, 130, 126);
            HeaderBg = Color.FromArgb(247, 244, 235);
            HeaderBorder = Color.FromArgb(212, 203, 179);
            ContentBg = Color.FromArgb(235, 230, 216);
            CardBg = Color.FromArgb(247, 244, 235);
            CardBorder = Color.FromArgb(212, 203, 179);
            TextPrimary = Color.FromArgb(45, 58, 56);
            TextSecondary = Color.FromArgb(102, 112, 110);
            TextMuted = Color.FromArgb(154, 163, 161);
            AccentBlue = Color.FromArgb(44, 94, 88);
            AccentGreen = Color.FromArgb(92, 124, 89);
            AccentAmber = Color.FromArgb(217, 138, 74);
            AccentRed = Color.FromArgb(163, 59, 59);
            AccentPurple = Color.FromArgb(111, 82, 107);
            AccentIndigo = Color.FromArgb(69, 90, 115);
            AccentCyan = Color.FromArgb(106, 130, 126);
            DgvHeaderBg = Color.FromArgb(247, 244, 235);
            DgvHeaderFg = Color.FromArgb(45, 58, 56);
            DgvAltRow = Color.FromArgb(242, 239, 230);
            DgvGridLine = Color.FromArgb(212, 203, 179);
            DgvSelectionBg = Color.FromArgb(44, 94, 88);
            DgvSelectionFg = Color.FromArgb(247, 244, 235);
            StatusBg = Color.FromArgb(247, 244, 235);
            StatusFg = Color.FromArgb(102, 112, 110);
            InputBg = Color.White;
            InputFg = Color.FromArgb(45, 58, 56);
            InputBorder = Color.FromArgb(212, 203, 179);
        }

        private static void ApplyDarkPalette()
        {
            SidebarBg = Color.FromArgb(10, 23, 22);
            SidebarLogoBg = Color.FromArgb(10, 23, 22);
            SidebarLogoText = Color.FromArgb(240, 235, 224);
            SidebarItemDefault = Color.FromArgb(143, 169, 164);
            SidebarItemHover = Color.FromArgb(23, 51, 49);
            SidebarItemActive = Color.FromArgb(209, 117, 84);
            SidebarItemActiveText = Color.White;
            SidebarAccentStripe = Color.FromArgb(232, 148, 62);
            SidebarFooter = Color.FromArgb(143, 169, 164);
            HeaderBg = Color.FromArgb(42, 46, 44);
            HeaderBorder = Color.FromArgb(61, 69, 65);
            ContentBg = Color.FromArgb(26, 31, 30);
            CardBg = Color.FromArgb(35, 41, 39);
            CardBorder = Color.FromArgb(61, 69, 65);
            TextPrimary = Color.FromArgb(240, 235, 224);
            TextSecondary = Color.FromArgb(181, 176, 165);
            TextMuted = Color.FromArgb(126, 137, 133);
            AccentBlue = Color.FromArgb(74, 133, 128);
            AccentGreen = Color.FromArgb(123, 169, 120);
            AccentAmber = Color.FromArgb(229, 160, 98);
            AccentRed = Color.FromArgb(194, 85, 85);
            AccentPurple = Color.FromArgb(152, 118, 160);
            AccentIndigo = Color.FromArgb(106, 130, 160);
            AccentCyan = Color.FromArgb(143, 169, 164);
            DgvHeaderBg = Color.FromArgb(42, 48, 46);
            DgvHeaderFg = Color.FromArgb(240, 235, 224);
            DgvAltRow = Color.FromArgb(35, 41, 39);
            DgvGridLine = Color.FromArgb(61, 69, 65);
            DgvSelectionBg = Color.FromArgb(74, 133, 128);
            DgvSelectionFg = Color.White;
            StatusBg = Color.FromArgb(35, 41, 39);
            StatusFg = Color.FromArgb(181, 176, 165);
            InputBg = Color.FromArgb(45, 51, 49);
            InputFg = Color.FromArgb(240, 235, 224);
            InputBorder = Color.FromArgb(72, 80, 76);
        }

        private static ThemeMode LoadPersistedMode()
        {
            try
            {
                var raw = new Services.CaiDatService().GetThemeMode();
                if (string.Equals(raw, "dark", StringComparison.OrdinalIgnoreCase))
                    return ThemeMode.Dark;
            }
            catch { }
            return ThemeMode.Light;
        }

        private static void SavePersistedMode(ThemeMode mode)
        {
            try
            {
                new Services.CaiDatService().SaveThemeMode(mode == ThemeMode.Dark ? "dark" : "light");
            }
            catch { }
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static void ApplySystemTitleBar(Form form)
        {
            if (Environment.OSVersion.Version.Major < 10) return;
            int useImmersiveDarkMode = IsDark ? 1 : 0;
            int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useImmersiveDarkMode, sizeof(int));
            int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref useImmersiveDarkMode, sizeof(int));
        }

        public static Button CreatePrimaryButton(string text, int w = 130, int h = 36, string? iconName = null)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Mix(AccentBlue, -0.12f);
            btn.FlatAppearance.MouseDownBackColor = Mix(AccentBlue, -0.25f);
            AttachButtonIcon(btn, iconName, Color.White);
            return btn;
        }

        public static Button CreateSecondaryButton(string text, int w = 110, int h = 36, string? iconName = null)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = CardBg,
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = CardBorder;
            btn.FlatAppearance.MouseOverBackColor = IsDark ? Mix(CardBg, 0.08f) : Mix(CardBg, -0.04f);
            btn.FlatAppearance.MouseDownBackColor = IsDark ? Mix(CardBg, 0.16f) : Mix(CardBg, -0.10f);
            AttachButtonIcon(btn, iconName, TextPrimary);
            return btn;
        }

        public static Button CreateSuccessButton(string text, int w = 130, int h = 36, string? iconName = null)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentGreen,
                ForeColor = Color.White,
                Font = new Font("Candara", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Mix(AccentGreen, -0.12f);
            btn.FlatAppearance.MouseDownBackColor = Mix(AccentGreen, -0.25f);
            AttachButtonIcon(btn, iconName, Color.White);
            return btn;
        }

        public static Button CreateDangerButton(string text, int w = 110, int h = 36, string? iconName = null)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5F),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Mix(AccentRed, -0.12f);
            btn.FlatAppearance.MouseDownBackColor = Mix(AccentRed, -0.25f);
            AttachButtonIcon(btn, iconName, Color.White);
            return btn;
        }

        private static void AttachButtonIcon(Button btn, string? iconName, Color tint)
        {
            if (string.IsNullOrEmpty(iconName)) return;
            var img = AppIcons.Load(iconName, 18, tint);
            if (img == null) return;
            btn.Image = img;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.Text = "  " + btn.Text;
        }

        public static Panel CreateListPage(UserControl owner, string title, string subtitle, out FlowLayoutPanel filters, out FlowLayoutPanel actions)
        {
            owner.Controls.Clear();
            owner.BackColor = ContentBg;
            owner.Padding = new Padding(0);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = ContentBg,
                Padding = new Padding(0)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ContentBg,
                Margin = new Padding(0),
                Padding = new Padding(0, 0, 0, 10)
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var titleBlock = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            titleBlock.Controls.Add(new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 38,
                Font = FontTitle,
                ForeColor = TextPrimary,
                TextAlign = ContentAlignment.BottomLeft
            });
            titleBlock.Controls.Add(new Label
            {
                Text = subtitle,
                Dock = DockStyle.Bottom,
                Height = 24,
                Font = FontSmall,
                ForeColor = TextSecondary,
                TextAlign = ContentAlignment.TopLeft
            });

            actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(18, 22, 0, 0),
                Padding = new Padding(0)
            };

            header.Controls.Add(titleBlock, 0, 0);
            header.Controls.Add(actions, 1, 0);

            var commandBar = new Panel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = CardBg,
                Padding = new Padding(16, 11, 16, 10),
                Margin = new Padding(0)
            };
            commandBar.Paint += (s, e) =>
            {
                using var pen = new Pen(CardBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, commandBar.Width - 1, commandBar.Height - 1);
            };

            filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            commandBar.Controls.Add(filters);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Margin = new Padding(0, 14, 0, 0),
                Padding = new Padding(1)
            };
            content.Paint += (s, e) =>
            {
                using var pen = new Pen(CardBorder);
                e.Graphics.DrawRectangle(pen, 0, 0, content.Width - 1, content.Height - 1);
            };

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(commandBar, 0, 1);
            root.Controls.Add(content, 0, 2);
            owner.Controls.Add(root);
            return content;
        }

        public static Label CreateCommandLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = FontSmallBold,
                ForeColor = TextSecondary,
                Margin = new Padding(0, 8, 8, 0)
            };
        }

        public static void StyleCommandControl(Control control, int width)
        {
            control.Width = width;
            control.Height = 36;
            control.Font = FontBody;
            control.Margin = new Padding(0, 0, 10, 0);
            StyleInputControl(control);
        }

        public static Color Mix(Color baseColor, float amount)
        {
            if (amount == 0f) return baseColor;
            if (amount < 0f)
            {
                float t = -amount;
                return Color.FromArgb(
                    Math.Clamp((int)(baseColor.R * (1 - t)), 0, 255),
                    Math.Clamp((int)(baseColor.G * (1 - t)), 0, 255),
                    Math.Clamp((int)(baseColor.B * (1 - t)), 0, 255));
            }
            return Color.FromArgb(
                Math.Clamp((int)(baseColor.R + (255 - baseColor.R) * amount), 0, 255),
                Math.Clamp((int)(baseColor.G + (255 - baseColor.G) * amount), 0, 255),
                Math.Clamp((int)(baseColor.B + (255 - baseColor.B) * amount), 0, 255));
        }

        public static void StyleInputControl(Control control)
        {
            if (control == null) return;
            control.BackColor = InputBg;
            control.ForeColor = InputFg;
            switch (control)
            {
                case TextBox tb:
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case ComboBox cb:
                    cb.FlatStyle = FlatStyle.Flat;
                    break;
                case NumericUpDown nud:
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case DateTimePicker dtp:
                    dtp.CalendarMonthBackground = InputBg;
                    dtp.CalendarForeColor = InputFg;
                    dtp.CalendarTitleBackColor = CardBg;
                    dtp.CalendarTitleForeColor = TextPrimary;
                    break;
            }
        }

        public static GraphicsPath RoundedRectPath(int x, int y, int w, int h, int r)
        {
            var path = new GraphicsPath();
            int d = r * 2;
            if (d > w) d = w;
            if (d > h) d = h;
            path.AddArc(x, y, d, d, 180, 90);
            path.AddArc(x + w - d, y, d, d, 270, 90);
            path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
            path.AddArc(x, y + h - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void DrawPill(Graphics g, Rectangle bounds, string text, Color color, Font? font = null, bool centered = true)
        {
            font ??= FontSmallBold;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var size = g.MeasureString(text, font);
            int w = (int)size.Width + 22;
            int h = 22;
            int x = centered ? bounds.X + (bounds.Width - w) / 2 : bounds.X + 8;
            int y = bounds.Y + (bounds.Height - h) / 2;
            var bgColor = Color.FromArgb(IsDark ? 70 : 38, color);
            using var path = RoundedRectPath(x, y, w, h, h / 2);
            using var brush = new SolidBrush(bgColor);
            g.FillPath(brush, path);
            using var fg = new SolidBrush(color);
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(text, font, fg, new RectangleF(x, y, w, h), sf);
        }

        public static Bitmap CreateAvatarBitmap(string text, int size, Color bg, Color fg)
        {
            var initials = string.IsNullOrWhiteSpace(text)
                ? "?"
                : string.Concat(text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Take(2).Select(p => char.ToUpper(p[0])));
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(bg);
            g.FillEllipse(brush, 0, 0, size - 1, size - 1);
            using var font = new Font("Segoe UI Semibold", size * 0.36f, FontStyle.Bold);
            using var fgBrush = new SolidBrush(fg);
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(initials, font, fgBrush, new RectangleF(0, 0, size, size), sf);
            return bmp;
        }

        public static Color AvatarBgFor(string text)
        {
            var palette = new[] { AccentBlue, AccentGreen, AccentPurple, AccentIndigo, AccentCyan, AccentAmber };
            int hash = string.IsNullOrEmpty(text) ? 0 : Math.Abs(text.GetHashCode());
            return palette[hash % palette.Length];
        }

        public static void EnableStatusPillColumn(DataGridView dgv, string columnName, Func<object?, (Color color, string text)?> map)
        {
            dgv.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgv.Columns[e.ColumnIndex].Name != columnName) return;
                var pill = map(e.Value);
                if (pill == null) return;
                bool selected = dgv.Rows[e.RowIndex].Selected;
                e.PaintBackground(e.CellBounds, selected);
                DrawPill(e.Graphics!, e.CellBounds, pill.Value.text, pill.Value.color);
                e.Handled = true;
            };
        }

        public static void EnableProgressBarColumn(DataGridView dgv, string columnName, Func<int, float> progressOf, Color color)
        {
            dgv.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgv.Columns[e.ColumnIndex].Name != columnName) return;
                bool selected = dgv.Rows[e.RowIndex].Selected;
                e.PaintBackground(e.CellBounds, selected);

                float t = Math.Clamp(progressOf(e.RowIndex), 0f, 1f);
                int pad = 12;
                int trackW = e.CellBounds.Width - pad * 2;
                int trackH = 6;
                int trackX = e.CellBounds.X + pad;
                int trackY = e.CellBounds.Y + (e.CellBounds.Height - trackH) / 2;

                var g = e.Graphics!;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var trackPath = RoundedRectPath(trackX, trackY, trackW, trackH, trackH / 2);
                using var trackBrush = new SolidBrush(Color.FromArgb(IsDark ? 50 : 30, color));
                g.FillPath(trackBrush, trackPath);

                int fillW = (int)(trackW * t);
                if (fillW > 0)
                {
                    using var fillPath = RoundedRectPath(trackX, trackY, Math.Max(fillW, trackH), trackH, trackH / 2);
                    using var fillBrush = new SolidBrush(color);
                    g.FillPath(fillBrush, fillPath);
                }

                using var lblBrush = new SolidBrush(IsDark ? TextSecondary : TextMuted);
                var pct = $"{(int)(t * 100)}%";
                var sz = g.MeasureString(pct, FontSmall);
                g.DrawString(pct, FontSmall, lblBrush,
                    e.CellBounds.X + e.CellBounds.Width - pad - sz.Width,
                    trackY - 14);
                e.Handled = true;
            };
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = DgvHeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = DgvHeaderFg;
            dgv.ColumnHeadersDefaultCellStyle.Font = FontDgvHeader;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = DgvHeaderBg;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = DgvHeaderFg;
            dgv.ColumnHeadersHeight = 42;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            dgv.DefaultCellStyle.BackColor = CardBg;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = DgvSelectionBg;
            dgv.DefaultCellStyle.SelectionForeColor = DgvSelectionFg;
            dgv.DefaultCellStyle.Font = FontSmall;

            dgv.RowTemplate.Height = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = DgvAltRow;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = DgvSelectionBg;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = DgvSelectionFg;
            dgv.RowHeadersDefaultCellStyle.BackColor = CardBg;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = DgvGridLine;
            dgv.BackgroundColor = CardBg;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.Margin = new Padding(0);
        }
    }
}

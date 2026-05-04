using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace QLNhaTro.Helpers
{
    public static class AppTheme
    {
        // Sidebar (Deep Jungle Green/Teal)
        public static readonly Color SidebarBg = Color.FromArgb(19, 42, 40);
        public static readonly Color SidebarLogoBg = Color.FromArgb(19, 42, 40);
        public static readonly Color SidebarLogoText = Color.FromArgb(247, 244, 235); // Warm Ivory
        public static readonly Color SidebarItemDefault = Color.FromArgb(106, 130, 126); // Muted Teal
        public static readonly Color SidebarItemHover = Color.FromArgb(30, 63, 60);
        public static readonly Color SidebarItemActive = Color.FromArgb(217, 108, 74); // Terracotta
        public static readonly Color SidebarItemActiveText = Color.White;
        public static readonly Color SidebarAccentStripe = Color.FromArgb(242, 157, 82); // Ochre
        public static readonly Color SidebarFooter = Color.FromArgb(106, 130, 126);

        // Header
        public static readonly Color HeaderBg = Color.FromArgb(247, 244, 235); // Warm Ivory
        public static readonly Color HeaderBorder = Color.FromArgb(212, 203, 179);

        // Content
        public static readonly Color ContentBg = Color.FromArgb(235, 230, 216); // Beige/Sand

        // Cards
        public static readonly Color CardBg = Color.FromArgb(247, 244, 235);
        public static readonly Color CardBorder = Color.FromArgb(212, 203, 179);

        // Text
        public static readonly Color TextPrimary = Color.FromArgb(45, 58, 56);
        public static readonly Color TextSecondary = Color.FromArgb(102, 112, 110);
        public static readonly Color TextMuted = Color.FromArgb(154, 163, 161);

        // Accent colors (Curated Muted Tones)
        public static readonly Color AccentBlue = Color.FromArgb(44, 94, 88); // Dark Teal
        public static readonly Color AccentGreen = Color.FromArgb(92, 124, 89); // Muted Moss
        public static readonly Color AccentAmber = Color.FromArgb(217, 138, 74); // Burnt Orange
        public static readonly Color AccentRed = Color.FromArgb(163, 59, 59); // Muted Crimson
        public static readonly Color AccentPurple = Color.FromArgb(111, 82, 107); // Muted Plum
        public static readonly Color AccentIndigo = Color.FromArgb(69, 90, 115); // Muted Navy
        public static readonly Color AccentCyan = Color.FromArgb(106, 130, 126);

        // DataGridView
        public static readonly Color DgvHeaderBg = Color.FromArgb(247, 244, 235);
        public static readonly Color DgvHeaderFg = Color.FromArgb(45, 58, 56);
        public static readonly Color DgvAltRow = Color.FromArgb(242, 239, 230);
        public static readonly Color DgvGridLine = Color.FromArgb(212, 203, 179);
        public static readonly Color DgvSelectionBg = Color.FromArgb(44, 94, 88);
        public static readonly Color DgvSelectionFg = Color.FromArgb(247, 244, 235);

        // StatusBar
        public static readonly Color StatusBg = Color.FromArgb(247, 244, 235);
        public static readonly Color StatusFg = Color.FromArgb(102, 112, 110);

        // Fonts (Editorial / Architectural with Vietnamese Support)
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

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static void ApplySystemTitleBar(Form form)
        {
            if (Environment.OSVersion.Version.Major >= 10)
            {
                int useImmersiveDarkMode = 0;
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                    if (key != null)
                    {
                        var val = key.GetValue("AppsUseLightTheme");
                        if (val != null && (int)val == 0)
                            useImmersiveDarkMode = 1;
                    }
                }
                catch { }

                int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useImmersiveDarkMode, sizeof(int));
                
                int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref useImmersiveDarkMode, sizeof(int));
            }
        }

        // Button factory
        public static Button CreatePrimaryButton(string text, int w = 130, int h = 36)
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(73, 80, 87);
            btn.FlatAppearance.MouseDownBackColor = Color.Black;
            return btn;
        }

        public static Button CreateSecondaryButton(string text, int w = 110, int h = 36)
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(248, 249, 250);
            return btn;
        }

        public static Button CreateSuccessButton(string text, int w = 130, int h = 36)
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(73, 80, 87);
            return btn;
        }

        public static Button CreateDangerButton(string text, int w = 110, int h = 36)
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
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 30, 30);
            return btn;
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
            if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.BackColor = Color.White;
            }
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
            dgv.DefaultCellStyle.SelectionBackColor = DgvSelectionBg;
            dgv.DefaultCellStyle.SelectionForeColor = DgvSelectionFg;
            dgv.DefaultCellStyle.Font = FontSmall;

            dgv.RowTemplate.Height = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = DgvAltRow;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = DgvGridLine;
            dgv.BackgroundColor = Color.White;
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

using System.Drawing.Drawing2D;
using QLNhaTro.Helpers;

namespace QLNhaTro.Forms.Main
{
    public partial class FrmMain : Form
    {
        private readonly Dictionary<string, Button> _navButtons = new();
        private Panel pnlSidebar = null!;
        private Panel pnlContent = null!;
        private Label lblFooter = null!;
        private UserControl? _activeControl;

        public FrmMain()
        {
            InitializeComponent();
            AppTheme.ApplySystemTitleBar(this);
            SetupForm();
            BuildShell();
            NavigateTo("Trang chủ", () => new UserControls.UcDashboard());
        }

        private void SetupForm()
        {
            Text = "Quản Lý Nhà Trọ";
            Size = new Size(1360, 820);
            MinimumSize = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = AppTheme.ContentBg;
            Font = AppTheme.FontBody;
            DoubleBuffered = true;
        }

        private void BuildShell()
        {
            Controls.Clear();

            var shell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = AppTheme.ContentBg,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 232));
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            pnlSidebar = BuildSidebar();
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.ContentBg,
                Padding = new Padding(28, 24, 28, 24),
                Margin = new Padding(0)
            };

            shell.Controls.Add(pnlSidebar, 0, 0);
            shell.Controls.Add(pnlContent, 1, 0);
            Controls.Add(shell);
        }

        private Panel BuildSidebar()
        {
            var sidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = AppTheme.SidebarBg,
                Margin = new Padding(0)
            };

            var logo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Color.Transparent,
                Padding = new Padding(22, 20, 22, 14)
            };

            var mark = new Label
            {
                Text = "\uE825", // Building icon
                Size = new Size(36, 36),
                Location = new Point(22, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.SidebarLogoText,
                Font = new Font("Segoe MDL2 Assets", 16F)
            };
            var title = new Label
            {
                Text = "QL Nhà Trọ",
                Location = new Point(70, 24),
                Size = new Size(140, 36),
                ForeColor = AppTheme.SidebarLogoText,
                Font = new Font("Segoe UI Semibold", 14F),
                TextAlign = ContentAlignment.MiddleLeft
            };
            logo.Controls.AddRange(new Control[] { mark, title });
            sidebar.Controls.Add(logo);

            lblFooter = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ForeColor = AppTheme.SidebarFooter,
                Font = AppTheme.FontSmall,
                TextAlign = ContentAlignment.MiddleCenter
            };
            sidebar.Controls.Add(lblFooter);

            var timer = new System.Windows.Forms.Timer { Interval = 60000 };
            timer.Tick += (s, e) => lblFooter.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            timer.Start();

            var nav = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(14, 8, 14, 0)
            };
            sidebar.Controls.Add(nav);
            nav.BringToFront();

            var menuItems = new (string text, Func<UserControl> factory)[]
            {
                ("Trang chủ",    () => new UserControls.UcDashboard()),
                ("Phòng",        () => new UserControls.UcPhong()),
                ("Khách thuê",   () => new UserControls.UcKhachThue()),
                ("Hợp đồng",     () => new UserControls.UcHopDong()),
                ("Điện nước",    () => new UserControls.UcDienNuoc()),
                ("Hóa đơn",      () => new UserControls.UcHoaDon()),
                ("Báo cáo",      () => new UserControls.UcBaoCao()),
                ("Cài đặt",      () => new UserControls.UcCaiDat()),
            };

            foreach (var (text, factory) in menuItems)
            {
                var button = CreateNavButton(text);
                button.Click += (s, e) => NavigateTo(text, factory);
                nav.Controls.Add(button);
                _navButtons[text] = button;
            }

            return sidebar;
        }

        private Button CreateNavButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Width = 204,
                Height = 42,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(14, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.SidebarItemDefault,
                Font = AppTheme.FontSidebar,
                Cursor = Cursors.Hand,
                Tag = text
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = AppTheme.SidebarItemHover;
            button.FlatAppearance.MouseDownBackColor = AppTheme.SidebarItemActive;
            return button;
        }

        private void NavigateTo(string title, Func<UserControl> factory)
        {
            foreach (var (key, button) in _navButtons)
            {
                var active = key == title;
                button.BackColor = active ? AppTheme.SidebarItemActive : Color.Transparent;
                button.ForeColor = active ? AppTheme.SidebarItemActiveText : AppTheme.SidebarItemDefault;
                button.Font = active ? AppTheme.FontSidebarActive : AppTheme.FontSidebar;
                button.FlatAppearance.MouseOverBackColor = active ? AppTheme.SidebarItemActive : AppTheme.SidebarItemHover;
            }

            _activeControl?.Dispose();
            _activeControl = factory();
            _activeControl.Dock = DockStyle.Fill;

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(_activeControl);
        }
    }
}

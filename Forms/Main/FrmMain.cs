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
        private string _activeTitle = "Trang chủ";
        private Func<UserControl> _activeFactory = () => new UserControls.UcDashboard();
        private Button _themeToggle = null!;

        public FrmMain()
        {
            InitializeComponent();
            AppTheme.ApplySystemTitleBar(this);
            SetupForm();
            BuildShell();
            NavigateTo(_activeTitle, _activeFactory);
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

            var markImg = AppIcons.Load(AppIcons.Logo.Home, 30, AppTheme.SidebarLogoText);
            Control mark = markImg != null
                ? new PictureBox
                {
                    Image = markImg,
                    Size = new Size(36, 36),
                    Location = new Point(22, 24),
                    SizeMode = PictureBoxSizeMode.CenterImage,
                    BackColor = Color.Transparent
                }
                : new Label
                {
                    Text = "\uE825",
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

            _themeToggle = BuildThemeToggle();
            sidebar.Controls.Add(_themeToggle);

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

            var menuItems = new (string text, string icon, Func<UserControl> factory)[]
            {
                ("Trang chủ",    AppIcons.Nav.Dashboard, () => new UserControls.UcDashboard()),
                ("Phòng",        AppIcons.Nav.Room,      () => new UserControls.UcPhong()),
                ("Khách thuê",   AppIcons.Nav.Tenant,    () => new UserControls.UcKhachThue()),
                ("Hợp đồng",     AppIcons.Nav.Contract,  () => new UserControls.UcHopDong()),
                ("Điện nước",    AppIcons.Nav.Utility,   () => new UserControls.UcDienNuoc()),
                ("Hóa đơn",      AppIcons.Nav.Invoice,   () => new UserControls.UcHoaDon()),
                ("Báo cáo",      AppIcons.Nav.Report,    () => new UserControls.UcBaoCao()),
                ("Cài đặt",      AppIcons.Nav.Settings,  () => new UserControls.UcCaiDat()),
            };

            foreach (var (text, icon, factory) in menuItems)
            {
                var button = CreateNavButton(text, icon);
                button.Click += (s, e) => NavigateTo(text, factory);
                nav.Controls.Add(button);
                _navButtons[text] = button;
            }

            return sidebar;
        }

        private Button CreateNavButton(string text, string iconName)
        {
            var button = new Button
            {
                Text = "   " + text,
                Width = 204,
                Height = 42,
                Margin = new Padding(0, 0, 0, 6),
                Padding = new Padding(12, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.SidebarItemDefault,
                Font = AppTheme.FontSidebar,
                Cursor = Cursors.Hand,
                Tag = new NavTag(text, iconName)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = AppTheme.SidebarItemHover;
            button.FlatAppearance.MouseDownBackColor = AppTheme.SidebarItemActive;
            button.Image = AppIcons.Load(iconName, 20, AppTheme.SidebarItemDefault);
            return button;
        }

        private sealed record NavTag(string Title, string IconName);

        private void NavigateTo(string title, Func<UserControl> factory)
        {
            _activeTitle = title;
            _activeFactory = factory;

            foreach (var (key, button) in _navButtons)
            {
                var active = key == title;
                button.BackColor = active ? AppTheme.SidebarItemActive : Color.Transparent;
                button.ForeColor = active ? AppTheme.SidebarItemActiveText : AppTheme.SidebarItemDefault;
                button.Font = active ? AppTheme.FontSidebarActive : AppTheme.FontSidebar;
                button.FlatAppearance.MouseOverBackColor = active ? AppTheme.SidebarItemActive : AppTheme.SidebarItemHover;
                if (button.Tag is NavTag tag)
                {
                    button.Image = AppIcons.Load(tag.IconName, 20,
                        active ? AppTheme.SidebarItemActiveText : AppTheme.SidebarItemDefault);
                }
            }

            _activeControl?.Dispose();
            _activeControl = factory();
            _activeControl.Dock = DockStyle.Fill;

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(_activeControl);
        }

        private Button BuildThemeToggle()
        {
            var btn = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                Margin = new Padding(14, 0, 14, 8),
                Padding = new Padding(14, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.SidebarItemDefault,
                Font = AppTheme.FontSidebar,
                Cursor = Cursors.Hand,
                Text = AppTheme.IsDark ? "   Chế độ sáng" : "   Chế độ tối"
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = AppTheme.SidebarItemHover;
            btn.Image = AppIcons.Load(AppTheme.IsDark ? "theme_sun" : "theme_moon", 20, AppTheme.SidebarItemDefault);
            btn.Click += (s, e) => ToggleTheme();
            return btn;
        }

        private void ToggleTheme()
        {
            AppTheme.ToggleMode();
            AppTheme.ApplySystemTitleBar(this);
            BackColor = AppTheme.ContentBg;
            _navButtons.Clear();
            BuildShell();
            NavigateTo(_activeTitle, _activeFactory);
        }
    }
}

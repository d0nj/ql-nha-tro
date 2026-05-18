using System.Drawing.Drawing2D;
using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcDashboard : UserControl
    {
        private readonly PhongService _phongSvc = new();
        private readonly HopDongService _hopDongSvc = new();
        private readonly HoaDonService _hoaDonSvc = new();
        private readonly KhachThueService _khachSvc = new();

        public UcDashboard()
        {
            Dock = DockStyle.Fill;
            BackColor = AppTheme.ContentBg;
            Padding = new Padding(0);
            DoubleBuffered = true;
            BuildUI();
        }

        private void BuildUI()
        {
            int totalPhong = _phongSvc.CountAll();
            int phongTrong = _phongSvc.CountByTrangThai(TrangThaiPhong.Trong);
            int phongDangThue = _phongSvc.CountByTrangThai(TrangThaiPhong.DangThue);
            decimal doanhThu = _hoaDonSvc.GetRevenueByMonth(DateTime.Now.Month, DateTime.Now.Year);
            int chuaThanhToan = _hoaDonSvc.CountUnpaid();
            int tongKhach = _khachSvc.CountAll();

            var content = AppTheme.CreateListPage(this, "Trang chủ", "Tổng quan phòng, doanh thu và việc cần xử lý.", out var filters, out var actions);
            filters.Parent!.Visible = false;
            actions.Visible = false;
            
            content.Padding = new Padding(24, 0, 24, 24);
            content.BackColor = AppTheme.ContentBg;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 140));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.Controls.Add(root);

            root.Controls.Add(CreateHeroBanner(), 0, 0);

            var pnlCards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                Padding = new Padding(0),
                Margin = new Padding(0, 8, 0, 0),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 5; i++)
                pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            pnlCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            pnlCards.Controls.Add(CreateStatCard("Tổng phòng", totalPhong.ToString(), AppTheme.AccentBlue, "Số phòng đang quản lý", AppIcons.Kpi.Building), 0, 0);
            pnlCards.Controls.Add(CreateStatCard("Phòng trống", phongTrong.ToString(), AppTheme.AccentGreen, "Sẵn sàng cho thuê", AppIcons.Kpi.DoorOpen), 1, 0);
            pnlCards.Controls.Add(CreateStatCard("Đang thuê", phongDangThue.ToString(), AppTheme.AccentAmber, "Phòng có hợp đồng", AppIcons.Kpi.DoorClosed), 2, 0);
            pnlCards.Controls.Add(CreateStatCard("Doanh thu tháng", FormatHelper.FormatVND(doanhThu), AppTheme.AccentPurple, $"Tháng {DateTime.Now:MM/yyyy}", AppIcons.Kpi.Wallet), 3, 0);
            var lastCard = CreateStatCard("Khách thuê", tongKhach.ToString(), AppTheme.AccentCyan, $"{chuaThanhToan} hóa đơn chưa thanh toán", AppIcons.Kpi.Users);
            lastCard.Margin = new Padding(0, 0, 0, 0);
            pnlCards.Controls.Add(lastCard, 4, 0);
            
            root.Controls.Add(pnlCards, 0, 1);

            var pnlBottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(0),
                Margin = new Padding(0, 12, 0, 0),
                BackColor = Color.Transparent
            };
            pnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.Controls.Add(pnlBottom, 0, 2);

            var dgvExpiring = CreateStyledDgv();
            dgvExpiring.Columns.Add("MaHD", "Mã HĐ");
            dgvExpiring.Columns.Add("Phong", "Phòng");
            dgvExpiring.Columns.Add("Khach", "Khách thuê");
            dgvExpiring.Columns.Add("NgayKT", "Hết hạn");
            foreach (var hd in _hopDongSvc.GetExpiringSoon(30))
                dgvExpiring.Rows.Add(hd.MaHopDong, hd.Phong.TenPhong, hd.KhachThue.HoTen, hd.NgayKetThuc?.ToString("dd/MM/yyyy") ?? "");
            
            var cardExpiring = CreateSectionCard("Hợp đồng sắp hết hạn", "Trong 30 ngày tới", AppTheme.AccentAmber, dgvExpiring);
            pnlBottom.Controls.Add(cardExpiring, 0, 0);

            var dgvUnpaid = CreateStyledDgv();
            dgvUnpaid.Columns.Add("MaHD", "Mã HĐ");
            dgvUnpaid.Columns.Add("Phong", "Phòng");
            dgvUnpaid.Columns.Add("ThangNam", "Tháng");
            dgvUnpaid.Columns.Add("TongTien", "Tổng tiền");
            foreach (var hd in _hoaDonSvc.Search(null!, TrangThaiHoaDon.ChuaThanhToan).Take(10))
                dgvUnpaid.Rows.Add(hd.MaHoaDon, hd.HopDong.Phong.TenPhong, $"{hd.Thang}/{hd.Nam}", FormatHelper.FormatVND(hd.TongTien));
            
            var cardUnpaid = CreateSectionCard("Hóa đơn chưa thanh toán", "10 hóa đơn mới nhất", AppTheme.AccentRed, dgvUnpaid);
            cardUnpaid.Margin = new Padding(0);
            pnlBottom.Controls.Add(cardUnpaid, 1, 0);
        }

        private Panel CreateStatCard(string label, string value, Color accentColor, string detail, string? iconName = null)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                BackColor = AppTheme.CardBg,
                Padding = new Padding(16)
            };

            card.Paint += (s, e) =>
            {
                var rect = card.ClientRectangle;
                if (rect.Width < 10 || rect.Height < 10) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using var borderPen = new Pen(AppTheme.CardBorder);
                e.Graphics.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);

                var accentRect = new Rectangle(0, 0, 4, rect.Height);
                using var accentBrush = new SolidBrush(accentColor);
                e.Graphics.FillRectangle(accentBrush, accentRect);
            };

            if (!string.IsNullOrEmpty(iconName))
            {
                var iconImg = AppIcons.Load(iconName, 32, accentColor);
                if (iconImg != null)
                {
                    var iconBox = new PictureBox
                    {
                        Image = iconImg,
                        Size = new Size(36, 36),
                        SizeMode = PictureBoxSizeMode.CenterImage,
                        BackColor = Color.Transparent,
                        Anchor = AnchorStyles.Top | AnchorStyles.Right
                    };
                    card.Controls.Add(iconBox);
                    iconBox.BringToFront();
                    void PositionIcon() => iconBox.Location = new Point(Math.Max(0, card.Width - iconBox.Width - 16), 12);
                    card.SizeChanged += (s, e) => PositionIcon();
                    card.HandleCreated += (s, e) => PositionIcon();
                }
            }

            var lblLabel = new Label
            {
                Text = label.ToUpperInvariant(),
                Font = AppTheme.FontCardLabel,
                ForeColor = AppTheme.TextSecondary,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.BottomLeft,
                AutoEllipsis = true,
                UseMnemonic = false
            };

            var lblValue = new Label
            {
                Text = value,
                Font = value.Length > 12 ? new Font("Segoe UI", 12F, FontStyle.Bold) : new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                UseMnemonic = false
            };

            var lblDetail = new Label
            {
                Text = detail,
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true,
                UseMnemonic = false
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            lblLabel.Dock = DockStyle.Fill;
            lblValue.Dock = DockStyle.Fill;
            lblDetail.Dock = DockStyle.Fill;

            layout.Controls.Add(lblLabel, 0, 0);
            layout.Controls.Add(lblValue, 0, 1);
            layout.Controls.Add(lblDetail, 0, 2);

            card.Controls.Add(layout);
            return card;
        }

        private Panel CreateSectionCard(string title, string subtitle, Color accentColor, Control content)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 16, 0),
                BackColor = AppTheme.CardBg,
                Padding = new Padding(1)
            };
            card.Paint += (s, e) =>
            {
                using var borderPen = new Pen(AppTheme.CardBorder);
                e.Graphics.DrawRectangle(borderPen, 0, 0, card.Width - 1, card.Height - 1);
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.CardBg, Padding = new Padding(20, 12, 16, 0) };
            var accent = new Panel { Dock = DockStyle.Left, Width = 4, BackColor = accentColor, Margin = new Padding(0, 0, 0, 4) };
            
            var lblTitle = new Label
            {
                Text = title,
                Font = AppTheme.FontSubtitle,
                ForeColor = AppTheme.TextPrimary,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.BottomLeft
            };
            var lblSubtitle = new Label
            {
                Text = subtitle,
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.TopLeft
            };
            
            header.Controls.Add(lblSubtitle);
            header.Controls.Add(lblTitle);
            header.Controls.Add(accent);
            
            var headerLine = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = AppTheme.DgvGridLine };
            header.Controls.Add(headerLine);

            layout.Controls.Add(header, 0, 0);
            
            var pnlContent = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8, 0, 8, 8) };
            pnlContent.Controls.Add(content);
            layout.Controls.Add(pnlContent, 0, 1);

            card.Controls.Add(layout);
            return card;
        }

        private Panel CreateHeroBanner()
        {
            var banner = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0),
                BackColor = AppTheme.SidebarBg
            };

            banner.Paint += (s, e) =>
            {
                var rect = banner.ClientRectangle;
                if (rect.Width < 10 || rect.Height < 10) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var hero = AppIcons.LoadImageCropped(AppIcons.Image.DashboardHero, rect.Width, rect.Height);
                if (hero != null)
                {
                    e.Graphics.DrawImage(hero, 0, 0, rect.Width, rect.Height);
                    using var overlay = new LinearGradientBrush(
                        new Rectangle(0, 0, rect.Width, rect.Height),
                        Color.FromArgb(190, AppTheme.SidebarBg),
                        Color.FromArgb(80, AppTheme.SidebarBg),
                        LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(overlay, rect);
                }
                else
                {
                    using var bg = new SolidBrush(AppTheme.SidebarBg);
                    e.Graphics.FillRectangle(bg, rect);
                }

                using var borderPen = new Pen(AppTheme.CardBorder);
                e.Graphics.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);

                using var titleBrush = new SolidBrush(AppTheme.SidebarLogoText);
                using var subBrush = new SolidBrush(Color.FromArgb(220, AppTheme.SidebarLogoText));
                var nhaTroName = TryGetPropertyName();
                e.Graphics.DrawString(nhaTroName, new Font("Segoe UI Semibold", 18F, FontStyle.Bold), titleBrush, 20, 18);
                e.Graphics.DrawString($"Hôm nay {DateTime.Now:dddd, dd/MM/yyyy}", new Font("Segoe UI", 10F), subBrush, 22, 54);
            };

            return banner;
        }

        private static string TryGetPropertyName()
        {
            try
            {
                var svc = new Services.CaiDatService();
                var cd = svc.Get();
                return string.IsNullOrWhiteSpace(cd.TenNhaTro) ? "Quản Lý Nhà Trọ" : cd.TenNhaTro;
            }
            catch
            {
                return "Quản Lý Nhà Trọ";
            }
        }

        private DataGridView CreateStyledDgv()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                Font = AppTheme.FontSmall,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            AppTheme.StyleDataGridView(dgv);
            dgv.DataBindingComplete += (s, e) => UpdateEmptyGridMessage(dgv);
            dgv.RowsAdded += (s, e) => UpdateEmptyGridMessage(dgv);
            dgv.RowsRemoved += (s, e) => UpdateEmptyGridMessage(dgv);
            return dgv;
        }

        private static void UpdateEmptyGridMessage(DataGridView dgv)
        {
            dgv.BackgroundColor = dgv.Rows.Count == 0 ? AppTheme.DgvHeaderBg : AppTheme.CardBg;
        }
    }
}

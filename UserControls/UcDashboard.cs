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
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.Controls.Add(root);

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

            pnlCards.Controls.Add(CreateStatCard("Tổng phòng", totalPhong.ToString(), AppTheme.AccentBlue, "Số phòng đang quản lý"), 0, 0);
            pnlCards.Controls.Add(CreateStatCard("Phòng trống", phongTrong.ToString(), AppTheme.AccentGreen, "Sẵn sàng cho thuê"), 1, 0);
            pnlCards.Controls.Add(CreateStatCard("Đang thuê", phongDangThue.ToString(), AppTheme.AccentAmber, "Phòng có hợp đồng"), 2, 0);
            pnlCards.Controls.Add(CreateStatCard("Doanh thu tháng", FormatHelper.FormatVND(doanhThu), AppTheme.AccentPurple, $"Tháng {DateTime.Now:MM/yyyy}"), 3, 0);
            var lastCard = CreateStatCard("Khách thuê", tongKhach.ToString(), AppTheme.AccentCyan, $"{chuaThanhToan} hóa đơn chưa thanh toán");
            lastCard.Margin = new Padding(0, 0, 0, 0); // No right margin for last card
            pnlCards.Controls.Add(lastCard, 4, 0);
            
            root.Controls.Add(pnlCards, 0, 0);

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
            root.Controls.Add(pnlBottom, 0, 1);

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

        private Panel CreateStatCard(string label, string value, Color accentColor, string detail)
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

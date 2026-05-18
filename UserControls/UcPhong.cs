using System.Drawing.Drawing2D;
using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcPhong : UserControl
    {
        private readonly PhongService _svc = new();
        private Panel _content = null!;
        private FlowLayoutPanel pnlCards = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboTrangThai = null!;

        public UcPhong()
        {
            Dock = DockStyle.Fill;
            BackColor = AppTheme.ContentBg;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            _content = AppTheme.CreateListPage(this, "Phòng", "Danh sách phòng, giá thuê và trạng thái sử dụng.", out var filters, out var actions);

            txtSearch = new TextBox { PlaceholderText = "Tìm mã phòng, tên phòng..." };
            AppTheme.StyleCommandControl(txtSearch, 300);
            txtSearch.TextChanged += (s, e) => LoadData();

            cboTrangThai = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleCommandControl(cboTrangThai, 160);
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang thuê", "Sửa chữa" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => LoadData();

            var btnAdd = AppTheme.CreatePrimaryButton("Thêm phòng", 148, 36, AppIcons.Btn.Add);
            btnAdd.Click += (s, e) => { using var frm = new Forms.Phong.FrmPhongEdit(); if (frm.ShowDialog() == DialogResult.OK) LoadData(); };

            filters.Controls.Add(AppTheme.CreateCommandLabel("Tìm kiếm"));
            filters.Controls.Add(txtSearch);
            filters.Controls.Add(AppTheme.CreateCommandLabel("Trạng thái"));
            filters.Controls.Add(cboTrangThai);
            actions.Controls.Add(btnAdd);

            pnlCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = AppTheme.CardBg,
                Padding = new Padding(20),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            _content.Controls.Add(pnlCards);
        }

        private void LoadData()
        {
            TrangThaiPhong? filter = cboTrangThai.SelectedIndex switch
            {
                1 => TrangThaiPhong.Trong,
                2 => TrangThaiPhong.DangThue,
                3 => TrangThaiPhong.SuaChua,
                _ => null
            };
            var data = _svc.Search(txtSearch.Text, filter).ToList();

            pnlCards.SuspendLayout();
            foreach (Control c in pnlCards.Controls) c.Dispose();
            pnlCards.Controls.Clear();

            if (data.Count == 0)
            {
                pnlCards.Visible = false;
                var empty = CreateEmptyState();
                _content.Controls.Add(empty);
                empty.BringToFront();
            }
            else
            {
                pnlCards.Visible = true;
                foreach (var c in _content.Controls.Cast<Control>().Where(c => c != pnlCards).ToList())
                    _content.Controls.Remove(c);
                foreach (var p in data)
                    pnlCards.Controls.Add(CreateRoomCard(p));
            }
            pnlCards.ResumeLayout();
        }

        private Control CreateRoomCard(Phong p)
        {
            var (statusColor, statusText) = StatusFor(p.TrangThai);

            var card = new DoubleBufferedPanel
            {
                Width = 248,
                Height = 162,
                Margin = new Padding(0, 0, 16, 16),
                BackColor = AppTheme.CardBg,
                Cursor = Cursors.Hand
            };

            bool hovered = false;

            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var bgFill = hovered
                    ? Blend(AppTheme.CardBg, statusColor, AppTheme.IsDark ? 0.08f : 0.05f)
                    : AppTheme.CardBg;
                using var path = AppTheme.RoundedRectPath(0, 0, card.Width - 1, card.Height - 1, 10);
                using var fillBrush = new SolidBrush(bgFill);
                g.FillPath(fillBrush, path);
                using var borderPen = new Pen(hovered ? statusColor : AppTheme.CardBorder, hovered ? 1.5f : 1f);
                g.DrawPath(borderPen, path);

                using var topStrip = new GraphicsPath();
                int sw = card.Width - 1;
                topStrip.AddArc(0, 0, 20, 20, 180, 90);
                topStrip.AddArc(sw - 20, 0, 20, 20, 270, 90);
                topStrip.AddLine(sw, 4, 0, 4);
                topStrip.CloseFigure();
                using var stripBrush = new SolidBrush(statusColor);
                g.FillPath(stripBrush, topStrip);

                AppTheme.DrawPill(g,
                    new Rectangle(card.Width - 116, card.Height - 38, 104, 28),
                    statusText, statusColor);
            };

            void SetHover(bool h) { hovered = h; card.Invalidate(); }
            card.MouseEnter += (s, e) => SetHover(true);
            card.MouseLeave += (s, e) => SetHover(false);

            var lblName = new Label
            {
                Text = p.TenPhong,
                Font = AppTheme.FontHeader,
                ForeColor = AppTheme.TextPrimary,
                Location = new Point(18, 18),
                Size = new Size(card.Width - 36, 24),
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                UseMnemonic = false
            };
            var lblCode = new Label
            {
                Text = p.MaPhong,
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextMuted,
                Location = new Point(18, 44),
                AutoSize = true,
                BackColor = Color.Transparent,
                UseMnemonic = false
            };
            var lblPrice = new Label
            {
                Text = FormatHelper.FormatVND(p.GiaThue),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = AppTheme.AccentBlue,
                Location = new Point(18, 66),
                AutoSize = true,
                BackColor = Color.Transparent,
                UseMnemonic = false
            };
            var lblMeta = new Label
            {
                Text = $"{p.DienTich:N1} m²    ·    Tối đa {p.SoNguoiToiDa} người",
                Font = AppTheme.FontSmall,
                ForeColor = AppTheme.TextSecondary,
                Location = new Point(18, 100),
                AutoSize = true,
                BackColor = Color.Transparent,
                UseMnemonic = false
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblCode);
            card.Controls.Add(lblPrice);
            card.Controls.Add(lblMeta);

            void OnEdit(object? s, EventArgs e) { EditRoom(p.Id); }
            card.Click += OnEdit;
            foreach (Control c in card.Controls)
            {
                c.Click += OnEdit;
                c.MouseEnter += (s, e) => SetHover(true);
                c.MouseLeave += (s, e) =>
                {
                    var pos = card.PointToClient(Cursor.Position);
                    if (!card.ClientRectangle.Contains(pos)) SetHover(false);
                };
            }

            var ctx = new ContextMenuStrip { Font = AppTheme.FontBody };
            ctx.Items.Add("Sửa phòng", null, (s, e) => EditRoom(p.Id));
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("Xóa phòng", null, (s, e) => DeleteRoom(p.Id, p.TenPhong));
            card.ContextMenuStrip = ctx;
            foreach (Control c in card.Controls) c.ContextMenuStrip = ctx;

            return card;
        }

        private static (Color color, string text) StatusFor(TrangThaiPhong status) => status switch
        {
            TrangThaiPhong.Trong => (AppTheme.AccentGreen, "Trống"),
            TrangThaiPhong.DangThue => (AppTheme.AccentBlue, "Đang thuê"),
            TrangThaiPhong.SuaChua => (AppTheme.AccentAmber, "Sửa chữa"),
            _ => (AppTheme.TextMuted, "—")
        };

        private static Color Blend(Color a, Color b, float t)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        private Control CreateEmptyState()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var pic = new PictureBox
            {
                Image = AppIcons.Load(AppIcons.Empty.Door, 96, AppTheme.TextMuted),
                Size = new Size(96, 96),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = "Chưa có phòng nào",
                Font = AppTheme.FontSubtitle,
                ForeColor = AppTheme.TextPrimary,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var lblDesc = new Label
            {
                Text = "Bấm 'Thêm phòng' ở góc phải để bắt đầu quản lý.",
                Font = AppTheme.FontBody,
                ForeColor = AppTheme.TextSecondary,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDesc);

            void CenterContents(object? s, EventArgs e)
            {
                int cx = panel.ClientSize.Width / 2;
                int cy = panel.ClientSize.Height / 2;
                pic.Location = new Point(cx - 48, cy - 72);
                lblTitle.Location = new Point(0, cy + 28);
                lblTitle.Width = panel.ClientSize.Width;
                lblDesc.Location = new Point(0, cy + 64);
                lblDesc.Width = panel.ClientSize.Width;
            }

            panel.Resize += CenterContents;
            panel.HandleCreated += (s, e) => CenterContents(s, e);
            return panel;
        }

        private void EditRoom(int id)
        {
            using var frm = new Forms.Phong.FrmPhongEdit(id);
            if (frm.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void DeleteRoom(int id, string name)
        {
            if (MessageBox.Show($"Xóa phòng \"{name}\"?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (!_svc.Delete(id))
                    MessageBox.Show("Không thể xóa phòng đang có hợp đồng hiệu lực.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else LoadData();
            }
        }
    }

    internal sealed class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }
}

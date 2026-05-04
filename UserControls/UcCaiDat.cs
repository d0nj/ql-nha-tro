using System.Drawing.Drawing2D;
using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcCaiDat : UserControl
    {
        private readonly CaiDatService _svc = new();
        private TextBox txtTenNhaTro = null!, txtDiaChi = null!, txtSDT = null!;
        private TextBox txtGiaDien = null!, txtGiaNuoc = null!, txtPhiDV = null!;

        public UcCaiDat()
        {
            Dock = DockStyle.Fill;
            BackColor = AppTheme.ContentBg;
            DoubleBuffered = true;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Cài đặt", "Thông tin nhà trọ và đơn giá sử dụng khi tạo hóa đơn.", out var filters, out var actions);
            filters.Controls.Add(new Label
            {
                Text = "Các thay đổi được dùng cho hóa đơn tạo sau khi lưu.",
                AutoSize = true,
                Font = AppTheme.FontBody,
                ForeColor = AppTheme.TextSecondary,
                Margin = new Padding(0, 7, 0, 0)
            });

            var btnSave = AppTheme.CreatePrimaryButton("Lưu cài đặt", 150, 36);
            btnSave.Click += BtnSave_Click;
            actions.Controls.Add(btnSave);

            var scroll = new FlowLayoutPanel 
            { 
                Dock = DockStyle.Fill, 
                AutoScroll = true, 
                BackColor = AppTheme.CardBg, 
                Padding = new Padding(24, 16, 24, 16),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // General Settings Section
            var sectionGeneral = CreateSection("Thông tin nhà trọ", 560);
            int y = 52;
            txtTenNhaTro = AddField(sectionGeneral, "Tên nhà trọ", "Nhập tên nhà trọ...", ref y);
            txtDiaChi = AddField(sectionGeneral, "Địa chỉ", "Nhập địa chỉ...", ref y);
            txtSDT = AddField(sectionGeneral, "Số điện thoại", "Nhập SĐT liên hệ...", ref y);
            sectionGeneral.Height = y + 20;

            // Pricing Section
            var sectionPricing = CreateSection("Bảng giá dịch vụ", 560);
            sectionPricing.Margin = new Padding(0, 16, 0, 0);
            y = 52;
            txtGiaDien = AddField(sectionPricing, "Giá điện (VNĐ/kWh)", "0", ref y);
            txtGiaNuoc = AddField(sectionPricing, "Giá nước (VNĐ/m³)", "0", ref y);
            txtPhiDV = AddField(sectionPricing, "Phí dịch vụ (VNĐ/tháng)", "0", ref y);
            sectionPricing.Height = y + 20;

            scroll.Controls.Add(sectionGeneral);
            scroll.Controls.Add(sectionPricing);
            content.Controls.Add(scroll);
        }

        private Panel CreateSection(string title, int width)
        {
            var section = new Panel
            {
                Margin = new Padding(0),
                Width = width,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 24, 12)
            };
            section.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var borderPen = new Pen(AppTheme.CardBorder);
                e.Graphics.DrawRectangle(borderPen, 0, 0, section.Width - 1, section.Height - 1);

                // Title
                e.Graphics.DrawString(title, AppTheme.FontSubtitle, new SolidBrush(AppTheme.TextPrimary), 24, 16);

                // Separator line
                using var linePen = new Pen(Color.FromArgb(243, 244, 246));
                e.Graphics.DrawLine(linePen, 24, 44, section.Width - 24, 44);
            };
            return section;
        }

        private TextBox AddField(Panel parent, string label, string placeholder, ref int y)
        {
            var lbl = new Label
            {
                Text = label,
                Location = new Point(24, y + 8),
                Size = new Size(180, 24),
                Font = AppTheme.FontBody,
                ForeColor = AppTheme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(lbl);

            var txt = new TextBox
            {
                Location = new Point(210, y + 4),
                Size = new Size(280, 32),
                Font = new Font("Segoe UI", 10.5F),
                PlaceholderText = placeholder,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(txt);

            y += 44;
            return txt;
        }

        private void LoadData()
        {
            var cd = _svc.Get();
            txtTenNhaTro.Text = cd.TenNhaTro;
            txtDiaChi.Text = cd.DiaChi ?? "";
            txtSDT.Text = cd.SoDienThoai ?? "";
            txtGiaDien.Text = cd.GiaDien.ToString("N0");
            txtGiaNuoc.Text = cd.GiaNuoc.ToString("N0");
            txtPhiDV.Text = cd.PhiDichVu.ToString("N0");
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!decimal.TryParse(txtGiaDien.Text.Replace(",", "").Replace(".", ""), out decimal gd) || gd < 0 ||
                !decimal.TryParse(txtGiaNuoc.Text.Replace(",", "").Replace(".", ""), out decimal gn) || gn < 0 ||
                !decimal.TryParse(txtPhiDV.Text.Replace(",", "").Replace(".", ""), out decimal pdv) || pdv < 0)
            {
                MessageBox.Show("Giá trị không hợp lệ. Vui lòng nhập số dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _svc.Update(new CaiDat
            {
                TenNhaTro = txtTenNhaTro.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                GiaDien = gd,
                GiaNuoc = gn,
                PhiDichVu = pdv
            });
            MessageBox.Show("Đã lưu cài đặt thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

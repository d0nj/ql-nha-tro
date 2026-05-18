using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcHoaDon : UserControl
    {
        private readonly HoaDonService _svc = new();
        private readonly HopDongService _hdSvc = new();
        private DataGridView dgv = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboTrangThai = null!;
        private NumericUpDown nudThang = null!, nudNam = null!;

        public UcHoaDon()
        {
            Dock = DockStyle.Fill; BackColor = AppTheme.ContentBg;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Hóa đơn", "Tạo, lọc và xác nhận thanh toán hóa đơn hàng tháng.", out var filters, out var actions);

            txtSearch = new TextBox { PlaceholderText = "Tìm mã HĐ, phòng..." };
            AppTheme.StyleCommandControl(txtSearch, 240);
            txtSearch.TextChanged += (s, e) => LoadData();

            cboTrangThai = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleCommandControl(cboTrangThai, 140);
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Chưa TT", "Đã TT" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => LoadData();

            var lblThang = AppTheme.CreateCommandLabel("Tháng");
            nudThang = new NumericUpDown { Minimum = 0, Maximum = 12, Value = DateTime.Now.Month };
            AppTheme.StyleCommandControl(nudThang, 64);
            nudThang.ValueChanged += (s, e) => LoadData();

            var lblNam = AppTheme.CreateCommandLabel("Năm");
            nudNam = new NumericUpDown { Minimum = 0, Maximum = 2099, Value = DateTime.Now.Year };
            AppTheme.StyleCommandControl(nudNam, 78);
            nudNam.ValueChanged += (s, e) => LoadData();

            var btnGenerate = AppTheme.CreatePrimaryButton("Tạo hóa đơn", 148, 36, AppIcons.Btn.Add);
            btnGenerate.Click += BtnGenerate_Click;

            var btnMarkPaid = AppTheme.CreateSuccessButton("Đã thanh toán", 164, 36, AppIcons.Btn.Check);
            btnMarkPaid.Click += BtnMarkPaid_Click;

            filters.Controls.AddRange(new Control[] { AppTheme.CreateCommandLabel("Tìm kiếm"), txtSearch, AppTheme.CreateCommandLabel("Trạng thái"), cboTrangThai, lblThang, nudThang, lblNam, nudNam });
            actions.Controls.AddRange(new Control[] { btnGenerate, btnMarkPaid });

            dgv = new DataGridView { Dock = DockStyle.Fill, Font = AppTheme.FontSmall };
            AppTheme.StyleDataGridView(dgv);
            AppTheme.EnableStatusPillColumn(dgv, "TrangThai", val =>
            {
                var s = val?.ToString();
                if (s == "Đã TT") return (AppTheme.AccentGreen, s);
                if (s == "Chưa TT") return (AppTheme.AccentRed, s);
                return null;
            });
            content.Controls.Add(dgv);
        }

        private void LoadData()
        {
            TrangThaiHoaDon? filter = cboTrangThai.SelectedIndex switch { 1 => TrangThaiHoaDon.ChuaThanhToan, 2 => TrangThaiHoaDon.DaThanhToan, _ => null };
            int? thang = (int)nudThang.Value == 0 ? null : (int)nudThang.Value;
            int? nam = (int)nudNam.Value == 0 ? null : (int)nudNam.Value;
            var data = _svc.Search(txtSearch.Text, filter, thang, nam);

            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID"); dgv.Columns["Id"]!.Visible = false;
            dgv.Columns.Add("MaHD", "Mã HĐ"); dgv.Columns.Add("Phong", "Phòng"); dgv.Columns.Add("Khach", "Khách");
            dgv.Columns.Add("ThangNam", "Tháng/Năm"); dgv.Columns.Add("TienPhong", "Tiền phòng"); dgv.Columns.Add("TienDien", "Tiền điện");
            dgv.Columns.Add("TienNuoc", "Tiền nước"); dgv.Columns.Add("PhiDV", "Phí DV"); dgv.Columns.Add("TongTien", "Tổng tiền");
            dgv.Columns.Add("TrangThai", "Trạng thái");
            dgv.Rows.Clear();
            foreach (var hd in data)
            {
                var rowIdx = dgv.Rows.Add(hd.Id, hd.MaHoaDon, hd.HopDong.Phong.TenPhong, hd.HopDong.KhachThue.HoTen,
                    $"{hd.Thang:D2}/{hd.Nam}", FormatHelper.FormatVND(hd.TienPhong), FormatHelper.FormatVND(hd.TienDien),
                    FormatHelper.FormatVND(hd.TienNuoc), FormatHelper.FormatVND(hd.PhiDichVu), FormatHelper.FormatVND(hd.TongTien),
                    hd.TrangThai == TrangThaiHoaDon.DaThanhToan ? "Đã TT" : "Chưa TT");
                _ = rowIdx;
            }
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            if (thang == 0 || nam == 0) { MessageBox.Show("Chọn tháng/năm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var activeContracts = _hdSvc.GetActive();
            int count = 0;
            foreach (var hd in activeContracts)
            {
                if (!_svc.InvoiceExists(hd.Id, thang, nam))
                { var invoice = _svc.GenerateInvoice(hd.Id, thang, nam); _svc.Add(invoice); count++; }
            }
            MessageBox.Show($"Đã tạo {count} hóa đơn tháng {thang}/{nam}.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void BtnMarkPaid_Click(object? sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;
            var status = dgv.CurrentRow.Cells["TrangThai"].Value?.ToString();
            if (status?.Contains("Đã TT") == true) return;
            _svc.MarkAsPaid(id); LoadData();
        }
    }
}

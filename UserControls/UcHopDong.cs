using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcHopDong : UserControl
    {
        private readonly HopDongService _svc = new();
        private DataGridView dgv = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboTrangThai = null!;

        public UcHopDong()
        {
            Dock = DockStyle.Fill; BackColor = AppTheme.ContentBg;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Hợp đồng", "Theo dõi hợp đồng hiệu lực, thời hạn và giá thuê.", out var filters, out var actions);

            txtSearch = new TextBox { PlaceholderText = "Tìm mã HĐ, phòng, khách..." };
            AppTheme.StyleCommandControl(txtSearch, 300);
            txtSearch.TextChanged += (s, e) => LoadData();

            cboTrangThai = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleCommandControl(cboTrangThai, 160);
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Hiệu lực", "Đã kết thúc" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => LoadData();

            var btnAdd = AppTheme.CreatePrimaryButton("Tạo hợp đồng", 144);
            btnAdd.Click += (s, e) => { using var frm = new Forms.HopDong.FrmHopDongEdit(); if (frm.ShowDialog() == DialogResult.OK) LoadData(); };

            filters.Controls.Add(AppTheme.CreateCommandLabel("Tìm kiếm"));
            filters.Controls.Add(txtSearch);
            filters.Controls.Add(AppTheme.CreateCommandLabel("Trạng thái"));
            filters.Controls.Add(cboTrangThai);
            actions.Controls.Add(btnAdd);

            dgv = new DataGridView { Dock = DockStyle.Fill, Font = AppTheme.FontSmall };
            AppTheme.StyleDataGridView(dgv);
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) TerminateSelected(); };
            var ctx = new ContextMenuStrip { Font = AppTheme.FontBody };
            ctx.Items.Add("Quản lý người ở (Cùng phòng)", null, (s, e) => ManageMembers());
            ctx.Items.Add("Kết thúc HĐ", null, (s, e) => TerminateSelected());
            dgv.ContextMenuStrip = ctx;
            content.Controls.Add(dgv);
        }

        private void LoadData()
        {
            TrangThaiHopDong? filter = cboTrangThai.SelectedIndex switch { 1 => TrangThaiHopDong.ConHieuLuc, 2 => TrangThaiHopDong.DaHuy, _ => null };
            var data = _svc.Search(txtSearch.Text, filter);
            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID"); dgv.Columns["Id"]!.Visible = false;
            dgv.Columns.Add("MaHD", "Mã HĐ"); dgv.Columns.Add("Phong", "Phòng"); dgv.Columns.Add("Khach", "Khách thuê");
            dgv.Columns.Add("GiaThue", "Giá thuê"); dgv.Columns.Add("NgayBD", "Bắt đầu"); dgv.Columns.Add("NgayKT", "Kết thúc");
            dgv.Columns.Add("TrangThai", "Trạng thái");
            dgv.Rows.Clear();
            foreach (var hd in data)
            {
                var rowIdx = dgv.Rows.Add(hd.Id, hd.MaHopDong, hd.Phong.TenPhong, hd.KhachThue.HoTen, FormatHelper.FormatVND(hd.GiaThueThucTe),
                    hd.NgayBatDau.ToString("dd/MM/yyyy"), hd.NgayKetThuc?.ToString("dd/MM/yyyy") ?? "Không XĐ",
                    hd.TrangThai == TrangThaiHopDong.ConHieuLuc ? "Hiệu lực" : "Đã kết thúc");
                if (hd.TrangThai == TrangThaiHopDong.DaHuy)
                    dgv.Rows[rowIdx].DefaultCellStyle.ForeColor = AppTheme.TextMuted;
            }
        }

        private void TerminateSelected()
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;
            var status = dgv.CurrentRow.Cells["TrangThai"].Value?.ToString();
            if (status?.Contains("Đã kết thúc") == true) return;
            if (MessageBox.Show("Kết thúc hợp đồng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { _svc.Terminate(id); LoadData(); }
        }

        private void ManageMembers()
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;
            using var frm = new Forms.HopDong.FrmThanhVien(id);
            frm.ShowDialog();
        }
    }
}

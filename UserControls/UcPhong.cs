using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcPhong : UserControl
    {
        private readonly PhongService _svc = new();
        private DataGridView dgv = null!;
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
            var content = AppTheme.CreateListPage(this, "Phòng", "Danh sách phòng, giá thuê và trạng thái sử dụng.", out var filters, out var actions);

            txtSearch = new TextBox { PlaceholderText = "Tìm mã phòng, tên phòng..." };
            AppTheme.StyleCommandControl(txtSearch, 300);
            txtSearch.TextChanged += (s, e) => LoadData();

            cboTrangThai = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            AppTheme.StyleCommandControl(cboTrangThai, 160);
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang thuê", "Sửa chữa" });
            cboTrangThai.SelectedIndex = 0;
            cboTrangThai.SelectedIndexChanged += (s, e) => LoadData();

            var btnAdd = AppTheme.CreatePrimaryButton("Thêm phòng", 132);
            btnAdd.Click += (s, e) => { using var frm = new Forms.Phong.FrmPhongEdit(); if (frm.ShowDialog() == DialogResult.OK) LoadData(); };

            filters.Controls.Add(AppTheme.CreateCommandLabel("Tìm kiếm"));
            filters.Controls.Add(txtSearch);
            filters.Controls.Add(AppTheme.CreateCommandLabel("Trạng thái"));
            filters.Controls.Add(cboTrangThai);
            actions.Controls.Add(btnAdd);

            dgv = new DataGridView { Dock = DockStyle.Fill, Font = AppTheme.FontSmall };
            AppTheme.StyleDataGridView(dgv);
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditSelected(); };

            var ctx = new ContextMenuStrip { Font = AppTheme.FontBody };
            ctx.Items.Add("Sửa phòng", null, (s, e) => EditSelected());
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("Xóa phòng", null, (s, e) => DeleteSelected());
            dgv.ContextMenuStrip = ctx;

            content.Controls.Add(dgv);
        }

        private void LoadData()
        {
            TrangThaiPhong? filter = cboTrangThai.SelectedIndex switch { 1 => TrangThaiPhong.Trong, 2 => TrangThaiPhong.DangThue, 3 => TrangThaiPhong.SuaChua, _ => null };
            var data = _svc.Search(txtSearch.Text, filter);

            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID"); dgv.Columns["Id"]!.Visible = false;
            dgv.Columns.Add("MaPhong", "Mã phòng");
            dgv.Columns.Add("TenPhong", "Tên phòng");
            dgv.Columns.Add("GiaThue", "Giá thuê");
            dgv.Columns.Add("DienTich", "Diện tích (m²)");
            dgv.Columns.Add("SoNguoi", "Tối đa");
            dgv.Columns.Add("TrangThai", "Trạng thái");
            dgv.Columns.Add("MoTa", "Mô tả");

            dgv.Rows.Clear();
            foreach (var p in data)
            {
                var row = dgv.Rows.Add(p.Id, p.MaPhong, p.TenPhong, FormatHelper.FormatVND(p.GiaThue), p.DienTich.ToString("N1"), p.SoNguoiToiDa,
                    p.TrangThai switch { TrangThaiPhong.Trong => "Trống", TrangThaiPhong.DangThue => "Đang thuê", TrangThaiPhong.SuaChua => "Sửa chữa", _ => "" },
                    p.MoTa ?? "");
            }
        }

        private void EditSelected()
        {
            if (dgv.CurrentRow == null) return;
            using var frm = new Forms.Phong.FrmPhongEdit((int)dgv.CurrentRow.Cells["Id"].Value);
            if (frm.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void DeleteSelected()
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;
            string name = dgv.CurrentRow.Cells["TenPhong"].Value?.ToString() ?? "";
            if (MessageBox.Show($"Xóa phòng \"{name}\"?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (!_svc.Delete(id)) MessageBox.Show("Không thể xóa phòng đang có hợp đồng hiệu lực.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else LoadData();
            }
        }
    }
}

using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcKhachThue : UserControl
    {
        private readonly KhachThueService _svc = new();
        private DataGridView dgv = null!;
        private TextBox txtSearch = null!;

        public UcKhachThue()
        {
            Dock = DockStyle.Fill; BackColor = AppTheme.ContentBg;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Khách thuê", "Hồ sơ người thuê, liên hệ và thông tin cá nhân.", out var filters, out var actions);

            txtSearch = new TextBox { PlaceholderText = "Tìm tên, CCCD, SĐT..." };
            AppTheme.StyleCommandControl(txtSearch, 320);
            txtSearch.TextChanged += (s, e) => LoadData();

            var btnAdd = AppTheme.CreatePrimaryButton("Thêm khách", 148, 36, AppIcons.Btn.Add);
            btnAdd.Click += (s, e) => { using var frm = new Forms.KhachThue.FrmKhachThueEdit(); if (frm.ShowDialog() == DialogResult.OK) LoadData(); };
            filters.Controls.Add(AppTheme.CreateCommandLabel("Tìm kiếm"));
            filters.Controls.Add(txtSearch);
            actions.Controls.Add(btnAdd);

            dgv = new DataGridView { Dock = DockStyle.Fill, Font = AppTheme.FontSmall };
            AppTheme.StyleDataGridView(dgv);
            dgv.RowTemplate.Height = 56;
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditSelected(); };
            AppTheme.EnableStatusPillColumn(dgv, "GioiTinh", val =>
            {
                var s = val?.ToString();
                if (s == "Nam") return (AppTheme.AccentBlue, "Nam");
                if (s == "Nữ") return (AppTheme.AccentPurple, "Nữ");
                return null;
            });
            var ctx = new ContextMenuStrip { Font = AppTheme.FontBody };
            ctx.Items.Add("Sửa", null, (s, e) => EditSelected());
            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("Xóa", null, (s, e) => DeleteSelected());
            dgv.ContextMenuStrip = ctx;
            content.Controls.Add(dgv);
        }

        private void LoadData()
        {
            var data = _svc.Search(txtSearch.Text);
            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID"); dgv.Columns["Id"]!.Visible = false;

            var avatarCol = new DataGridViewImageColumn
            {
                Name = "Avatar",
                HeaderText = "",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Resizable = DataGridViewTriState.False
            };
            dgv.Columns.Add(avatarCol);
            dgv.Columns.Add("HoTen", "Họ tên");
            dgv.Columns.Add("CCCD", "CCCD");
            dgv.Columns.Add("SoDienThoai", "SĐT");
            dgv.Columns.Add("GioiTinh", "Giới tính");
            dgv.Columns.Add("NgaySinh", "Ngày sinh");
            dgv.Columns.Add("NgheNghiep", "Nghề nghiệp");
            dgv.Columns.Add("QueQuan", "Quê quán");
            dgv.Rows.Clear();
            foreach (var k in data)
            {
                var avatar = AppTheme.CreateAvatarBitmap(k.HoTen, 36, AppTheme.AvatarBgFor(k.HoTen), Color.White);
                dgv.Rows.Add(k.Id, avatar, k.HoTen, k.CCCD ?? "", k.SoDienThoai ?? "", k.GioiTinh == GioiTinh.Nam ? "Nam" : "Nữ", k.NgaySinh?.ToString("dd/MM/yyyy") ?? "", k.NgheNghiep ?? "", k.QueQuan ?? "");
            }
        }

        private void EditSelected()
        {
            if (dgv.CurrentRow == null) return;
            using var frm = new Forms.KhachThue.FrmKhachThueEdit((int)dgv.CurrentRow.Cells["Id"].Value);
            if (frm.ShowDialog() == DialogResult.OK) LoadData();
        }

        private void DeleteSelected()
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;
            string name = dgv.CurrentRow.Cells["HoTen"].Value?.ToString() ?? "";
            if (MessageBox.Show($"Xóa khách \"{name}\"?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            { if (!_svc.Delete(id)) MessageBox.Show("Không thể xóa khách đang có hợp đồng hiệu lực.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); else LoadData(); }
        }
    }
}

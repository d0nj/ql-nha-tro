using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcDienNuoc : UserControl
    {
        private readonly ChiSoDienNuocService _svc = new();
        private readonly PhongService _phongSvc = new();
        private readonly HopDongService _hdSvc = new();
        private DataGridView dgv = null!;
        private NumericUpDown nudThang = null!, nudNam = null!;

        public UcDienNuoc()
        {
            Dock = DockStyle.Fill; BackColor = AppTheme.ContentBg;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Điện nước", "Nhập chỉ số điện nước theo phòng và kỳ ghi nhận.", out var filters, out var actions);

            filters.Controls.Add(AppTheme.CreateCommandLabel("Tháng"));
            nudThang = new NumericUpDown { Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            AppTheme.StyleCommandControl(nudThang, 72);
            nudThang.ValueChanged += (s, e) => LoadData();
            filters.Controls.Add(nudThang);

            filters.Controls.Add(AppTheme.CreateCommandLabel("Năm"));
            nudNam = new NumericUpDown { Minimum = 2020, Maximum = 2099, Value = DateTime.Now.Year };
            AppTheme.StyleCommandControl(nudNam, 88);
            nudNam.ValueChanged += (s, e) => LoadData();
            filters.Controls.Add(nudNam);

            var btnSave = AppTheme.CreateSuccessButton("Lưu chỉ số", 140, 36, AppIcons.Btn.Save);
            btnSave.Click += BtnSave_Click;
            actions.Controls.Add(btnSave);

            dgv = new DataGridView { Dock = DockStyle.Fill };
            AppTheme.StyleDataGridView(dgv);
            dgv.ReadOnly = false;
            dgv.AllowUserToAddRows = false;
            content.Controls.Add(dgv);
        }

        private void LoadData()
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            var phongs = _phongSvc.GetAll();
            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "PhongId", HeaderText = "ID", Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenPhong", HeaderText = "Phòng", ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienCu", HeaderText = "Điện cũ" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienMoi", HeaderText = "Điện mới" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "NuocCu", HeaderText = "Nước cũ" });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "NuocMoi", HeaderText = "Nước mới" });

            dgv.Rows.Clear();
            foreach (var p in phongs)
            {
                var chiSo = _svc.GetByPhongThangNam(p.Id, thang, nam);
                int dienCu = 0, nuocCu = 0;
                if (chiSo == null)
                {
                    var prev = _svc.GetPrevious(p.Id, thang, nam);
                    if (prev != null) { dienCu = prev.ChiSoDienMoi; nuocCu = prev.ChiSoNuocMoi; }
                }
                dgv.Rows.Add(p.Id, p.TenPhong, chiSo?.ChiSoDienCu ?? dienCu, chiSo?.ChiSoDienMoi ?? 0, chiSo?.ChiSoNuocCu ?? nuocCu, chiSo?.ChiSoNuocMoi ?? 0);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value, saved = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                int phongId = (int)row.Cells["PhongId"].Value;
                if (!int.TryParse(row.Cells["DienCu"].Value?.ToString(), out int dc)) continue;
                if (!int.TryParse(row.Cells["DienMoi"].Value?.ToString(), out int dm)) continue;
                if (!int.TryParse(row.Cells["NuocCu"].Value?.ToString(), out int nc)) continue;
                if (!int.TryParse(row.Cells["NuocMoi"].Value?.ToString(), out int nm)) continue;
                _svc.SaveOrUpdate(new QLNhaTro.Models.ChiSoDienNuoc { PhongId = phongId, Thang = thang, Nam = nam, ChiSoDienCu = dc, ChiSoDienMoi = dm, ChiSoNuocCu = nc, ChiSoNuocMoi = nm }); saved++;
            }
            MessageBox.Show($"Đã lưu {saved} bản ghi.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

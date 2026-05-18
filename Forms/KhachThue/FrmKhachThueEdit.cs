using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.Forms.KhachThue
{
    public class FrmKhachThueEdit : Form
    {
        private readonly KhachThueService _svc = new();
        private readonly int? _editId;
        private TextBox txtHoTen = null!, txtCCCD = null!, txtSDT = null!, txtEmail = null!, txtQueQuan = null!, txtNgheNghiep = null!;
        private ComboBox cboGioiTinh = null!;
        private DateTimePicker dtpNgaySinh = null!;

        public FrmKhachThueEdit(int? editId = null)
        {
            _editId = editId;
            AppTheme.ApplySystemTitleBar(this);
            BuildUI();
            if (_editId.HasValue) LoadData();
        }

        private void BuildUI()
        {
            Text = _editId.HasValue ? "Sửa khách thuê" : "Thêm khách thuê";
            ClientSize = new Size(484, 520);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = AppTheme.ContentBg;
            Font = AppTheme.FontBody;

            var card = new Panel { Location = new Point(20, 20), Size = new Size(444, 420), BackColor = AppTheme.CardBg };
            card.Paint += (s, e) => { using var p = new Pen(AppTheme.CardBorder); e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1); };
            Controls.Add(card);

            int y = 20, ix = 160, iw = 260;
            AddLabel(card, "Họ tên (*)", y); txtHoTen = AddTxt(card, ix, y, iw); y += 44;
            AddLabel(card, "CCCD", y); txtCCCD = AddTxt(card, ix, y, iw); y += 44;
            AddLabel(card, "SĐT", y); txtSDT = AddTxt(card, ix, y, iw); y += 44;
            AddLabel(card, "Email", y); txtEmail = AddTxt(card, ix, y, iw); y += 44;
            AddLabel(card, "Giới tính", y);
            cboGioiTinh = new ComboBox { Location = new Point(ix, y), Size = new Size(iw, 32), DropDownStyle = ComboBoxStyle.DropDownList, Font = AppTheme.FontBody };
            cboGioiTinh.Items.AddRange(new[] { "Nam", "Nữ" }); cboGioiTinh.SelectedIndex = 0;
            AppTheme.StyleInputControl(cboGioiTinh);
            card.Controls.Add(cboGioiTinh); y += 44;
            AddLabel(card, "Ngày sinh", y);
            dtpNgaySinh = new DateTimePicker { Location = new Point(ix, y), Size = new Size(iw, 32), Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Font = AppTheme.FontBody };
            AppTheme.StyleInputControl(dtpNgaySinh);
            card.Controls.Add(dtpNgaySinh); y += 44;
            AddLabel(card, "Nghề nghiệp", y); txtNgheNghiep = AddTxt(card, ix, y, iw); y += 44;
            AddLabel(card, "Quê quán", y); txtQueQuan = AddTxt(card, ix, y, iw);

            var btnSave = AppTheme.CreatePrimaryButton("Lưu", 120, 40, AppIcons.Btn.Save);
            btnSave.Location = new Point(20, 454);
            btnSave.Click += BtnSave_Click;
            var btnCancel = AppTheme.CreateSecondaryButton("Hủy", 100, 40, AppIcons.Btn.Cancel);
            btnCancel.Location = new Point(148, 454);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            txtCCCD.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            txtSDT.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            Controls.AddRange(new Control[] { btnSave, btnCancel });
        }

        private void LoadData()
        {
            var k = _svc.GetById(_editId!.Value);
            if (k == null) { Close(); return; }
            txtHoTen.Text = k.HoTen; txtCCCD.Text = k.CCCD ?? ""; txtSDT.Text = k.SoDienThoai ?? "";
            txtEmail.Text = k.Email ?? ""; cboGioiTinh.SelectedIndex = (int)k.GioiTinh;
            if (k.NgaySinh.HasValue) { dtpNgaySinh.Checked = true; dtpNgaySinh.Value = k.NgaySinh.Value; }
            txtNgheNghiep.Text = k.NgheNghiep ?? ""; txtQueQuan.Text = k.QueQuan ?? "";
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            { MessageBox.Show("Họ tên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var kt = new Models.KhachThue
            {
                Id = _editId ?? 0, HoTen = txtHoTen.Text.Trim(),
                CCCD = string.IsNullOrWhiteSpace(txtCCCD.Text) ? null : txtCCCD.Text.Trim(),
                SoDienThoai = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                GioiTinh = (GioiTinh)cboGioiTinh.SelectedIndex,
                NgaySinh = dtpNgaySinh.Checked ? dtpNgaySinh.Value.Date : null,
                NgheNghiep = string.IsNullOrWhiteSpace(txtNgheNghiep.Text) ? null : txtNgheNghiep.Text.Trim(),
                QueQuan = string.IsNullOrWhiteSpace(txtQueQuan.Text) ? null : txtQueQuan.Text.Trim()
            };
            if (_editId.HasValue) _svc.Update(kt); else _svc.Add(kt);
            DialogResult = DialogResult.OK; Close();
        }

        private void AddLabel(Panel p, string t, int y) { p.Controls.Add(new Label { Text = t, Location = new Point(20, y + 6), AutoSize = true, ForeColor = AppTheme.TextSecondary, Font = AppTheme.FontBody }); }
        private TextBox AddTxt(Panel p, int x, int y, int w) { var t = new TextBox { Location = new Point(x, y), Size = new Size(w, 32), Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle }; AppTheme.StyleInputControl(t); p.Controls.Add(t); return t; }
    }
}

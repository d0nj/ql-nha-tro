using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.Forms.Phong
{
    public class FrmPhongEdit : Form
    {
        private readonly PhongService _svc = new();
        private readonly int? _editId;
        private TextBox txtMaPhong = null!, txtTenPhong = null!, txtGiaThue = null!, txtDienTich = null!, txtSoNguoi = null!, txtMoTa = null!;
        private ComboBox cboTrangThai = null!;

        public FrmPhongEdit(int? editId = null)
        {
            _editId = editId;
            AppTheme.ApplySystemTitleBar(this);
            BuildUI();
            if (_editId.HasValue) LoadData();
        }

        private void BuildUI()
        {
            Text = _editId.HasValue ? "Sửa phòng" : "Thêm phòng mới";
            ClientSize = new Size(484, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = AppTheme.ContentBg;
            Font = AppTheme.FontBody;

            // Card container
            var card = new Panel { Location = new Point(20, 20), Size = new Size(444, 400), BackColor = AppTheme.CardBg };
            card.Paint += (s, e) => { using var p = new Pen(AppTheme.CardBorder); e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1); };
            Controls.Add(card);

            int y = 20, inputX = 160, inputW = 260;
            AddLabel(card, "Mã phòng", y); txtMaPhong = AddTextBox(card, inputX, y, inputW); y += 44;
            AddLabel(card, "Tên phòng", y); txtTenPhong = AddTextBox(card, inputX, y, inputW); y += 44;
            AddLabel(card, "Giá thuê (VNĐ)", y); txtGiaThue = AddTextBox(card, inputX, y, inputW); y += 44;
            AddLabel(card, "Diện tích (m²)", y); txtDienTich = AddTextBox(card, inputX, y, inputW); y += 44;
            AddLabel(card, "Số người tối đa", y); txtSoNguoi = AddTextBox(card, inputX, y, inputW); txtSoNguoi.Text = "4"; y += 44;
            AddLabel(card, "Trạng thái", y);
            cboTrangThai = new ComboBox { Location = new Point(inputX, y), Size = new Size(inputW, 32), DropDownStyle = ComboBoxStyle.DropDownList, Font = AppTheme.FontBody };
            cboTrangThai.Items.AddRange(new[] { "Trống", "Đang thuê", "Sửa chữa" });
            cboTrangThai.SelectedIndex = 0;
            card.Controls.Add(cboTrangThai); y += 44;
            AddLabel(card, "Mô tả", y);
            txtMoTa = new TextBox { Location = new Point(inputX, y), Size = new Size(inputW, 60), Multiline = true, Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle };
            card.Controls.Add(txtMoTa);

            // Buttons
            var btnSave = AppTheme.CreatePrimaryButton("Lưu", 120, 40);
            btnSave.Location = new Point(20, 434);
            btnSave.Click += BtnSave_Click;

            var btnCancel = AppTheme.CreateSecondaryButton("Hủy", 100, 40);
            btnCancel.Location = new Point(148, 434);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            txtGiaThue.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            txtDienTich.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',') e.Handled = true; };
            txtSoNguoi.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            Controls.AddRange(new Control[] { btnSave, btnCancel });
        }

        private void LoadData()
        {
            var p = _svc.GetById(_editId!.Value);
            if (p == null) { Close(); return; }
            txtMaPhong.Text = p.MaPhong; txtTenPhong.Text = p.TenPhong;
            txtGiaThue.Text = p.GiaThue.ToString("N0"); txtDienTich.Text = p.DienTich.ToString("N1");
            txtSoNguoi.Text = p.SoNguoiToiDa.ToString(); cboTrangThai.SelectedIndex = (int)p.TrangThai;
            txtMoTa.Text = p.MoTa ?? "";
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhong.Text) || string.IsNullOrWhiteSpace(txtTenPhong.Text))
            { MessageBox.Show("Mã phòng và Tên phòng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!decimal.TryParse(txtGiaThue.Text.Replace(",", "").Replace(".", ""), out decimal gia) || gia < 0)
            { MessageBox.Show("Giá thuê không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!double.TryParse(txtDienTich.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double dt) || dt < 0)
            { MessageBox.Show("Diện tích không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!int.TryParse(txtSoNguoi.Text, out int sn) || sn <= 0)
            { MessageBox.Show("Số người tối đa không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_svc.IsMaPhongExists(txtMaPhong.Text.Trim(), _editId ?? 0))
            { MessageBox.Show("Mã phòng đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var phong = new Models.Phong { Id = _editId ?? 0, MaPhong = txtMaPhong.Text.Trim(), TenPhong = txtTenPhong.Text.Trim(), GiaThue = gia, DienTich = dt, SoNguoiToiDa = sn, TrangThai = (TrangThaiPhong)cboTrangThai.SelectedIndex, MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim() };
            if (_editId.HasValue) _svc.Update(phong); else _svc.Add(phong);
            DialogResult = DialogResult.OK; Close();
        }

        private void AddLabel(Panel p, string text, int y) { p.Controls.Add(new Label { Text = text, Location = new Point(20, y + 6), AutoSize = true, ForeColor = AppTheme.TextSecondary, Font = AppTheme.FontBody }); }
        private TextBox AddTextBox(Panel p, int x, int y, int w) { var t = new TextBox { Location = new Point(x, y), Size = new Size(w, 32), Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle }; p.Controls.Add(t); return t; }
    }
}

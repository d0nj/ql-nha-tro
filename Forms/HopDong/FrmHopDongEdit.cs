using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Services;

namespace QLNhaTro.Forms.HopDong
{
    public class FrmHopDongEdit : Form
    {
        private readonly HopDongService _hdSvc = new();
        private readonly PhongService _phongSvc = new();
        private readonly KhachThueService _khachSvc = new();
        private ComboBox cboPhong = null!, cboKhach = null!;
        private DateTimePicker dtpBatDau = null!, dtpKetThuc = null!;
        private TextBox txtGiaThue = null!, txtTienCoc = null!;
        private CheckBox chkKetThuc = null!;

        public FrmHopDongEdit()
        {
            AppTheme.ApplySystemTitleBar(this);
            BuildUI();
        }

        private void BuildUI()
        {
            Text = "Tạo hợp đồng mới"; ClientSize = new Size(504, 440);
            StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = AppTheme.ContentBg; Font = AppTheme.FontBody;

            var card = new Panel { Location = new Point(20, 20), Size = new Size(464, 340), BackColor = AppTheme.CardBg };
            card.Paint += (s, e) => { using var p = new Pen(AppTheme.CardBorder); e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1); };
            Controls.Add(card);

            int y = 20, ix = 170, iw = 270;
            AddLabel(card, "Phòng (trống)", y);
            cboPhong = new ComboBox { Location = new Point(ix, y), Size = new Size(iw, 32), DropDownStyle = ComboBoxStyle.DropDownList, Font = AppTheme.FontBody };
            var rooms = _phongSvc.Search("", TrangThaiPhong.Trong);
            foreach (var r in rooms) cboPhong.Items.Add(new ComboItem(r.Id, $"{r.MaPhong} - {r.TenPhong} ({FormatHelper.FormatVND(r.GiaThue)})"));
            if (cboPhong.Items.Count > 0) cboPhong.SelectedIndex = 0;
            cboPhong.SelectedIndexChanged += (s, e) => { if (cboPhong.SelectedItem is ComboItem ci) { var p = _phongSvc.GetById(ci.Value); if (p != null) txtGiaThue.Text = p.GiaThue.ToString("N0"); } };
            card.Controls.Add(cboPhong); y += 44;

            AddLabel(card, "Khách thuê", y);
            cboKhach = new ComboBox { Location = new Point(ix, y), Size = new Size(iw, 32), DropDownStyle = ComboBoxStyle.DropDownList, Font = AppTheme.FontBody };
            var tenants = _khachSvc.GetAvailableTenants();
            foreach (var t in tenants) cboKhach.Items.Add(new ComboItem(t.Id, $"{t.HoTen} ({t.CCCD ?? "N/A"})"));
            if (cboKhach.Items.Count > 0) cboKhach.SelectedIndex = 0;
            card.Controls.Add(cboKhach); y += 44;

            AddLabel(card, "Ngày bắt đầu", y);
            dtpBatDau = new DateTimePicker { Location = new Point(ix, y), Size = new Size(iw, 32), Format = DateTimePickerFormat.Short, Font = AppTheme.FontBody };
            card.Controls.Add(dtpBatDau); y += 44;

            AddLabel(card, "Ngày kết thúc", y);
            chkKetThuc = new CheckBox { Text = "", Location = new Point(ix, y + 4), AutoSize = true, Checked = true };
            dtpKetThuc = new DateTimePicker { Location = new Point(ix + 25, y), Size = new Size(iw - 25, 32), Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddMonths(12), Font = AppTheme.FontBody };
            chkKetThuc.CheckedChanged += (s, e) => dtpKetThuc.Enabled = chkKetThuc.Checked;
            card.Controls.AddRange(new Control[] { chkKetThuc, dtpKetThuc }); y += 44;

            AddLabel(card, "Giá thuê (VNĐ)", y); txtGiaThue = AddTxt(card, ix, y, iw);
            if (cboPhong.SelectedItem is ComboItem first) { var p = _phongSvc.GetById(first.Value); if (p != null) txtGiaThue.Text = p.GiaThue.ToString("N0"); }
            y += 44;
            AddLabel(card, "Tiền cọc (VNĐ)", y); txtTienCoc = AddTxt(card, ix, y, iw); txtTienCoc.Text = "0";

            var btnSave = AppTheme.CreatePrimaryButton("Tạo hợp đồng", 150, 40);
            btnSave.Location = new Point(20, 374);
            btnSave.Click += BtnSave_Click;
            var btnCancel = AppTheme.CreateSecondaryButton("Hủy", 100, 40);
            btnCancel.Location = new Point(178, 374);
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            txtGiaThue.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };
            txtTienCoc.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            Controls.AddRange(new Control[] { btnSave, btnCancel });
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (cboPhong.SelectedItem is not ComboItem phongItem) { MessageBox.Show("Chọn phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cboKhach.SelectedItem is not ComboItem khachItem) { MessageBox.Show("Chọn khách thuê.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!decimal.TryParse(txtGiaThue.Text.Replace(",", "").Replace(".", ""), out decimal gia) || gia <= 0) { MessageBox.Show("Giá thuê không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!decimal.TryParse(txtTienCoc.Text.Replace(",", "").Replace(".", ""), out decimal coc) || coc < 0) { MessageBox.Show("Tiền cọc không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var hd = new Models.HopDong
            {
                PhongId = phongItem.Value, KhachThueId = khachItem.Value,
                MaHopDong = _hdSvc.GenerateMaHopDong(), NgayBatDau = dtpBatDau.Value.Date,
                NgayKetThuc = chkKetThuc.Checked ? dtpKetThuc.Value.Date : null,
                GiaThueThucTe = gia, TienCoc = coc, TrangThai = TrangThaiHopDong.ConHieuLuc
            };
            _hdSvc.Add(hd);
            DialogResult = DialogResult.OK; Close();
        }

        private void AddLabel(Panel p, string t, int y) { p.Controls.Add(new Label { Text = t, Location = new Point(20, y + 6), AutoSize = true, ForeColor = AppTheme.TextSecondary, Font = AppTheme.FontBody }); }
        private TextBox AddTxt(Panel p, int x, int y, int w) { var t = new TextBox { Location = new Point(x, y), Size = new Size(w, 32), Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle }; p.Controls.Add(t); return t; }

        private class ComboItem
        {
            public int Value { get; }
            public string Display { get; }
            public ComboItem(int value, string display) { Value = value; Display = display; }
            public override string ToString() => Display;
        }
    }
}

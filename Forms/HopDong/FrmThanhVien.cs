using QLNhaTro.Helpers;
using QLNhaTro.Models;
using QLNhaTro.Data;
using Microsoft.EntityFrameworkCore;

namespace QLNhaTro.Forms.HopDong
{
    public class FrmThanhVien : Form
    {
        private readonly int _hopDongId;
        private DataGridView dgv = null!;
        private TextBox txtHoTen = null!, txtCCCD = null!, txtSDT = null!, txtQuanHe = null!;
        private Button btnAdd = null!;

        public FrmThanhVien(int hopDongId)
        {
            _hopDongId = hopDongId;
            AppTheme.ApplySystemTitleBar(this);
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            Text = "Quản lý thành viên cùng phòng";
            ClientSize = new Size(600, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = AppTheme.ContentBg;
            Font = AppTheme.FontBody;

            // Form container
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(20) };
            
            // Inputs
            int ix = 20, iw = 260;
            var lblHoTen = new Label { Text = "Họ Tên (*)", Location = new Point(ix, 10), AutoSize = true, Font = AppTheme.FontCardLabel, ForeColor = AppTheme.TextSecondary };
            txtHoTen = new TextBox { Location = new Point(ix, 30), Width = iw, Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle };
            AppTheme.StyleInputControl(txtHoTen);
            
            var lblCCCD = new Label { Text = "CCCD", Location = new Point(ix + iw + 20, 10), AutoSize = true, Font = AppTheme.FontCardLabel, ForeColor = AppTheme.TextSecondary };
            txtCCCD = new TextBox { Location = new Point(ix + iw + 20, 30), Width = iw, Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle };
            AppTheme.StyleInputControl(txtCCCD);

            var lblSDT = new Label { Text = "Số ĐT", Location = new Point(ix, 70), AutoSize = true, Font = AppTheme.FontCardLabel, ForeColor = AppTheme.TextSecondary };
            txtSDT = new TextBox { Location = new Point(ix, 90), Width = iw, Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle };
            AppTheme.StyleInputControl(txtSDT);

            var lblQuanHe = new Label { Text = "Quan hệ (với người thuê chính)", Location = new Point(ix + iw + 20, 70), AutoSize = true, Font = AppTheme.FontCardLabel, ForeColor = AppTheme.TextSecondary };
            txtQuanHe = new TextBox { Location = new Point(ix + iw + 20, 90), Width = iw - 110, Font = AppTheme.FontBody, BorderStyle = BorderStyle.FixedSingle };
            AppTheme.StyleInputControl(txtQuanHe);

            btnAdd = AppTheme.CreatePrimaryButton("Thêm", 100, 36, AppIcons.Btn.Add);
            btnAdd.Location = new Point(ix + iw + 20 + iw - 100, 89);
            btnAdd.Click += (s, e) => AddMember();

            pnlTop.Controls.AddRange(new Control[] { lblHoTen, txtHoTen, lblCCCD, txtCCCD, lblSDT, txtSDT, lblQuanHe, txtQuanHe, btnAdd });
            Controls.Add(pnlTop);

            // Grid
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 20, 20) };
            dgv = new DataGridView { Dock = DockStyle.Fill, Font = AppTheme.FontSmall };
            AppTheme.StyleDataGridView(dgv);
            
            var ctx = new ContextMenuStrip { Font = AppTheme.FontBody };
            ctx.Items.Add("Xóa thành viên", null, (s, e) => RemoveMember());
            dgv.ContextMenuStrip = ctx;
            
            pnlGrid.Controls.Add(dgv);
            Controls.Add(pnlGrid);
        }

        private void LoadData()
        {
            using var ctx = new AppDbContext();
            var members = ctx.ThanhViens.Where(t => t.HopDongId == _hopDongId).OrderBy(t => t.NgayTao).ToList();
            
            dgv.Columns.Clear();
            dgv.Columns.Add("Id", "ID"); dgv.Columns["Id"]!.Visible = false;
            dgv.Columns.Add("HoTen", "Họ tên"); dgv.Columns["HoTen"]!.Width = 150;
            dgv.Columns.Add("CCCD", "CCCD");
            dgv.Columns.Add("SDT", "Số ĐT");
            dgv.Columns.Add("QuanHe", "Quan hệ");
            dgv.Columns.Add("NgayTao", "Ngày thêm");

            dgv.Rows.Clear();
            foreach (var m in members)
            {
                dgv.Rows.Add(m.Id, m.HoTen, m.CCCD, m.SoDienThoai, m.QuanHe, m.NgayTao.ToString("dd/MM/yyyy"));
            }
        }

        private void AddMember()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var ctx = new AppDbContext();
            var tv = new ThanhVien
            {
                HopDongId = _hopDongId,
                HoTen = txtHoTen.Text.Trim(),
                CCCD = txtCCCD.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim(),
                QuanHe = txtQuanHe.Text.Trim(),
                NgayTao = DateTime.Now
            };

            ctx.ThanhViens.Add(tv);
            ctx.SaveChanges();
            
            txtHoTen.Clear(); txtCCCD.Clear(); txtSDT.Clear(); txtQuanHe.Clear();
            LoadData();
        }

        private void RemoveMember()
        {
            if (dgv.CurrentRow == null) return;
            int id = (int)dgv.CurrentRow.Cells["Id"].Value;

            if (MessageBox.Show("Xóa thành viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using var ctx = new AppDbContext();
                var tv = ctx.ThanhViens.Find(id);
                if (tv != null)
                {
                    ctx.ThanhViens.Remove(tv);
                    ctx.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}

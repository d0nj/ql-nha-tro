using System.Drawing.Drawing2D;
using QLNhaTro.Helpers;
using QLNhaTro.Services;

namespace QLNhaTro.UserControls
{
    public class UcBaoCao : UserControl
    {
        private readonly HoaDonService _svc = new();
        private NumericUpDown nudNam = null!;
        private DataGridView dgv = null!;
        private Panel pnlChart = null!;

        public UcBaoCao()
        {
            Dock = DockStyle.Fill; BackColor = AppTheme.ContentBg;
            BuildUI(); LoadData();
        }

        private void BuildUI()
        {
            var content = AppTheme.CreateListPage(this, "Báo cáo", "Doanh thu theo tháng và xuất dữ liệu tổng hợp.", out var filters, out var actions);

            filters.Controls.Add(AppTheme.CreateCommandLabel("Năm"));
            nudNam = new NumericUpDown { Minimum = 2020, Maximum = 2099, Value = DateTime.Now.Year };
            AppTheme.StyleCommandControl(nudNam, 88);
            nudNam.ValueChanged += (s, e) => LoadData();

            var btnExport = AppTheme.CreateSecondaryButton("Xuất CSV", 116);
            btnExport.Click += BtnExport_Click;
            filters.Controls.Add(nudNam);
            actions.Controls.Add(btnExport);

            var splitter = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300,
                BackColor = AppTheme.ContentBg,
                Panel1MinSize = 200,
                Panel2MinSize = 100
            };
            content.Controls.Add(splitter);

            pnlChart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(24, 16, 24, 16)
            };
            pnlChart.Paint += PnlChart_Paint;
            splitter.Panel1.Controls.Add(pnlChart);

            dgv = new DataGridView { Dock = DockStyle.Fill };
            AppTheme.StyleDataGridView(dgv);
            splitter.Panel2.Controls.Add(dgv);
        }

        private void LoadData()
        {
            int nam = (int)nudNam.Value;
            dgv.Columns.Clear();
            dgv.Columns.Add("Thang", "Tháng"); dgv.Columns.Add("DoanhThu", "Doanh thu");
            dgv.Rows.Clear();
            for (int i = 1; i <= 12; i++)
            {
                decimal rev = _svc.GetRevenueByMonth(i, nam);
                dgv.Rows.Add($"Tháng {i}", FormatHelper.FormatVND(rev));
            }
            pnlChart.Invalidate();
        }

        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int nam = (int)nudNam.Value;
            decimal[] data = new decimal[12];
            for (int i = 0; i < 12; i++) data[i] = _svc.GetRevenueByMonth(i + 1, nam);
            decimal maxVal = data.Max();
            if (maxVal == 0) maxVal = 1;

            int pad = 60, right = 20, top = 40, bottom = 50;
            int chartW = pnlChart.Width - pad - right;
            int chartH = pnlChart.Height - top - bottom;
            float barW = chartW / 14f;
            float gap = chartW / 14f * 0.16f;

            // Title
            g.DrawString($"Doanh thu năm {nam}", AppTheme.FontSubtitle, new SolidBrush(AppTheme.TextPrimary), pad, 8);

            // Y-axis grid lines
            using var gridPen = new Pen(Color.FromArgb(243, 244, 246), 1);
            for (int i = 0; i <= 5; i++)
            {
                int y = top + chartH - (int)(chartH * i / 5.0);
                g.DrawLine(gridPen, pad, y, pad + chartW, y);
                string label = FormatHelper.FormatVND(maxVal * i / 5);
                g.DrawString(label, AppTheme.FontSmall, new SolidBrush(AppTheme.TextMuted), 2, y - 8);
            }

            // Bars
            var barColors = new (Color start, Color end)[]
            {
                (Color.FromArgb(99, 102, 241), Color.FromArgb(79, 70, 229)),
                (Color.FromArgb(59, 130, 246), Color.FromArgb(37, 99, 235)),
            };

            for (int i = 0; i < 12; i++)
            {
                float x = pad + i * (barW + gap) + gap;
                float h = (float)(chartH * (double)data[i] / (double)maxVal);
                float y = top + chartH - h;

                if (h > 2)
                {
                    var barRect = new RectangleF(x, y, barW, h);
                    var colorPair = barColors[i % 2];
                    using var brush = new LinearGradientBrush(barRect, colorPair.start, colorPair.end, LinearGradientMode.Vertical);
                    g.FillRectangle(brush, barRect);

                    // Rounded top effect
                    using var topPath = new GraphicsPath();
                    float r = Math.Min(4, barW / 2);
                    topPath.AddArc(x, y, r * 2, r * 2, 180, 90);
                    topPath.AddArc(x + barW - r * 2, y, r * 2, r * 2, 270, 90);
                    topPath.AddLine(x + barW, y + r, x + barW, y + h);
                    topPath.AddLine(x, y + h, x, y + r);
                    topPath.CloseFigure();
                    g.FillPath(brush, topPath);
                }

                // Month label
                g.DrawString($"T{i + 1}", AppTheme.FontSmall, new SolidBrush(AppTheme.TextSecondary), x + barW / 2 - 10, top + chartH + 6);
            }
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = $"BaoCao_{nudNam.Value}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            using var sw = new StreamWriter(dlg.FileName, false, System.Text.Encoding.UTF8);
            sw.WriteLine("Tháng,Doanh thu");
            for (int i = 1; i <= 12; i++)
                sw.WriteLine($"{i},{_svc.GetRevenueByMonth(i, (int)nudNam.Value)}");
            MessageBox.Show("Đã xuất CSV.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

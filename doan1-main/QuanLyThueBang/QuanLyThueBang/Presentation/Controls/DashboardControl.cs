using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Helpers;
using ScottPlot.WinForms;

namespace QuanLyThueBang.Presentation.Controls
{
    public class DashboardControl : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        // KPI labels
        private Label lblCountPhim = null!;
        private Label lblPctPhim = null!;
        private Label lblCountBanSao = null!;
        private Label lblPctBanSao = null!;
        private Label lblCountPhieuMuon = null!;
        private Label lblPctPhieuMuon = null!;
        private Label lblTongDoanhThu = null!;
        private Label lblPctDoanhThu = null!;

        // Filter
        private ComboBox cboTimeFilter = null!;

        // Tables
        private DataGridView dgvTopTrending = null!;
        private DataGridView dgvQuaHan = null!;

        // Charts
        private FormsPlot _barChart = null!;
        private Panel _branchPanel = null!;       // Thay thế pie chart = horizontal bar chart
        private Label lblBranchTitle = null!;

        // Selected month state — null month = hiển thị cả năm
        private int _selectedBarYear = DateTime.Now.Year;
        private int? _selectedBarMonth = null;  // null = tổng cả năm

        // Branch data for drawing
        private List<(string Name, decimal Revenue, double Pct)> _branchData = new();

        // Color palette for branches
        private static readonly string[] BranchColors = {
            "#28a745", "#0d6efd", "#6f42c1", "#fd7e14",
            "#20c997", "#e83e8c", "#17a2b8", "#ffc107",
            "#dc3545", "#6c757d"
        };

        public DashboardControl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeUI();
            this.Load += (s, e) => LoadDashboardData();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // ── Header ──────────────────────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "📊 TỔNG QUAN BÁO CÁO & THỐNG KÊ",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(184, 123, 125),
                Location = new Point(20, 15),
                AutoSize = true
            };
            cboTimeFilter = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(660, 18),
                Size = new Size(145, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboTimeFilter.Items.AddRange(new string[] { "7 ngày qua", "1 tháng qua", "3 tháng qua", "Toàn thời gian" });
            cboTimeFilter.SelectedIndex = 3;
            cboTimeFilter.SelectedIndexChanged += (s, e) => LoadDashboardData();
            var btnRefresh = new Button
            {
                Text = "🔄 Làm Mới",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(815, 14),
                Size = new Size(110, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadDashboardData();
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, cboTimeFilter, btnRefresh });

            // ── 4 KPI Cards ─────────────────────────────────────────────────────
            var pnlCards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 115,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(15, 15, 15, 10)
            };
            for (int i = 0; i < 4; i++) pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            (lblCountPhim, lblPctPhim) = CreateKpiCard(pnlCards, 0, "🎬 TỔNG SỐ PHIM", "0 Phim", Color.FromArgb(111, 66, 193));
            (lblCountBanSao, lblPctBanSao) = CreateKpiCard(pnlCards, 1, "📼 TỔNG KHO BẢN SAO", "0 Cuốn", Color.FromArgb(13, 110, 253));
            (lblCountPhieuMuon, lblPctPhieuMuon) = CreateKpiCard(pnlCards, 2, "📋 PHIẾU ĐANG MƯỢN", "0 Phiếu", Color.FromArgb(253, 126, 20));
            (lblTongDoanhThu, lblPctDoanhThu) = CreateKpiCard(pnlCards, 3, "💰 TỔNG DOANH THU", "0 VNĐ", Color.FromArgb(40, 167, 69));

            // ── Body (Charts + Tables) ────────────────────────────────────────
            var pnlBody = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            pnlBody.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            pnlBody.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));

            // Row 1 – Charts
            var tlpCharts = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(15, 5, 15, 5)
            };
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // Left: Bar chart (12 tháng)
            var pnlBar = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(0, 0, 8, 0) };
            var lblBarTitle = new Label
            {
                Text = "📈 DOANH THU 12 THÁNG  (Bấm vào cột để lọc chi nhánh)",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Dock = DockStyle.Top,
                Height = 32
            };
            _barChart = new FormsPlot { Dock = DockStyle.Fill };
            _barChart.MouseDown += BarChart_MouseDown;
            pnlBar.Controls.Add(_barChart);
            pnlBar.Controls.Add(lblBarTitle);

            // Right: Horizontal Branch Bar Chart
            var pnlBranch = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16, 12, 16, 12), Margin = new Padding(8, 0, 0, 0) };
            lblBranchTitle = new Label
            {
                Text = $"🏪 DOANH THU CHI NHÁNH — T{DateTime.Now.Month}/{DateTime.Now.Year}",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Dock = DockStyle.Top,
                Height = 32
            };
            _branchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            _branchPanel.Paint += BranchPanel_Paint;
            pnlBranch.Controls.Add(_branchPanel);
            pnlBranch.Controls.Add(lblBranchTitle);

            tlpCharts.Controls.Add(pnlBar, 0, 0);
            tlpCharts.Controls.Add(pnlBranch, 1, 0);

            // Row 2 – Tables
            var tlpTables = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(15, 5, 15, 15)
            };
            tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Top 5 panel
            var pnlTop = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(0, 0, 8, 0) };
            var lblTopTitle = new Label { Text = "🔥 TOP 5 BỘ PHIM THUÊ NHIỀU NHẤT", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50), Dock = DockStyle.Top, Height = 32 };
            dgvTopTrending = BuildDgv(rowH: 44);
            dgvTopTrending.Columns.Add("colSTT", "TOP"); dgvTopTrending.Columns[0].Width = 55;
            var colImg = new DataGridViewImageColumn { Name = "colAnh", HeaderText = "Ảnh", Width = 44, ImageLayout = DataGridViewImageCellLayout.Zoom };
            dgvTopTrending.Columns.Add(colImg);
            dgvTopTrending.Columns.Add("colTuaDe", "Tựa Đề Phim"); dgvTopTrending.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTopTrending.Columns.Add("colLuot", "Lượt Thuê"); dgvTopTrending.Columns[3].Width = 100;
            pnlTop.Controls.Add(dgvTopTrending);
            pnlTop.Controls.Add(lblTopTitle);

            // Overdue panel
            var pnlOD = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(8, 0, 0, 0) };
            var lblODTitle = new Label { Text = "⚠️ PHIẾU MƯỢN QUÁ HẠN", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(220, 53, 69), Dock = DockStyle.Top, Height = 32 };
            dgvQuaHan = BuildDgv(rowH: 42);
            dgvQuaHan.Columns.Add("colKH", "Khách Hàng"); dgvQuaHan.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvQuaHan.Columns.Add("colPhim", "Phim"); dgvQuaHan.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvQuaHan.Columns.Add("colTre", "Ngày Trễ"); dgvQuaHan.Columns[2].Width = 95;
            dgvQuaHan.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuaHan.CellPainting += DgvQuaHan_CellPainting;
            pnlOD.Controls.Add(dgvQuaHan);
            pnlOD.Controls.Add(lblODTitle);

            tlpTables.Controls.Add(pnlTop, 0, 0);
            tlpTables.Controls.Add(pnlOD, 1, 0);

            pnlBody.Controls.Add(tlpCharts, 0, 0);
            pnlBody.Controls.Add(tlpTables, 0, 1);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlHeader);
        }

        // ── Helper: Build DataGridView ────────────────────────────────────────
        private DataGridView BuildDgv(int rowH)
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeight = 38
            };
            dgv.RowTemplate.Height = rowH;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };
            return dgv;
        }

        // ── Helper: Create KPI Card ───────────────────────────────────────────
        private (Label lblVal, Label lblPct) CreateKpiCard(TableLayoutPanel parent, int col, string title, string initVal, Color accent)
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(5) };
            var t = new Label { Text = title, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = accent, Location = new Point(15, 12), AutoSize = true };
            var v = new Label { Text = initVal, Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(35, 35, 35), Location = new Point(15, 40), AutoSize = true };
            var p = new Label { Text = "—", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 100, 100), Location = new Point(15, 74), AutoSize = true };
            card.Controls.AddRange(new Control[] { t, v, p });
            parent.Controls.Add(card, col, 0);
            return (v, p);
        }

        // ── Badge CellPainting for Overdue column ─────────────────────────────
        private void DgvQuaHan_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2 || e.Value == null) return;
            e.PaintBackground(e.CellBounds, true);
            using var gp = new GraphicsPath();
            const int r = 10;
            var rect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + 7, e.CellBounds.Width - 12, e.CellBounds.Height - 14);
            gp.AddArc(rect.X, rect.Y, r, r, 180, 90);
            gp.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
            gp.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
            gp.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
            gp.CloseFigure();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(Color.FromArgb(220, 53, 69));
            e.Graphics.FillPath(brush, gp);
            TextRenderer.DrawText(e.Graphics, e.Value.ToString(), e.CellStyle.Font, e.CellBounds, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            e.Handled = true;
        }

        // ── Bar Chart Mouse Click → lọc chi nhánh theo tháng ──────────────
        private void BarChart_MouseDown(object? sender, MouseEventArgs e)
        {
            try
            {
                var coords = _barChart.Plot.GetCoordinates(e.X, e.Y);
                int barIndex = (int)Math.Round(coords.X);
                int monthsAgo = 11 - barIndex;
                if (monthsAgo < 0 || monthsAgo > 11) return;

                var targetDate = DateTime.Now.AddMonths(-monthsAgo);
                _selectedBarYear = targetDate.Year;
                _selectedBarMonth = targetDate.Month;  // Lọc theo tháng cụ thể

                // Highlight cột được chọn
                UpdateBarChart_HighlightOnly();

                using var scope = _serviceProvider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();
                LoadBranchData(ctx, _selectedBarYear, _selectedBarMonth);
                _branchPanel.Invalidate();
            }
            catch { }
        }

        // ── Main Data Load ────────────────────────────────────────────────────
        private void LoadDashboardData()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();

                var now = DateTime.Now;
                DateTime since = DateTime.MinValue;
                if (cboTimeFilter.SelectedIndex == 0) since = now.AddDays(-7);
                else if (cboTimeFilter.SelectedIndex == 1) since = now.AddMonths(-1);
                else if (cboTimeFilter.SelectedIndex == 2) since = now.AddMonths(-3);

                bool isManager = !AppSession.IsAdmin && !string.IsNullOrEmpty(AppSession.CurrentMaCuaHang);
                string? branchId = isManager ? AppSession.CurrentMaCuaHang : null;

                // KPI
                lblCountPhim.Text = $"{ctx.Phims.Count():N0} Phim";
                
                var countBanSao = branchId == null ? ctx.BanSaoBangs.Count() : ctx.BanSaoBangs.Count(bs => bs.MaCuaHangHienTai == branchId);
                lblCountBanSao.Text = $"{countBanSao:N0} Cuốn";
                
                var phieuQ = ctx.PhieuMuons.AsQueryable();
                if (since != DateTime.MinValue) phieuQ = phieuQ.Where(p => p.NgayMuon >= since);
                if (branchId != null) phieuQ = phieuQ.Where(p => p.MaCuaHangMuon == branchId);
                lblCountPhieuMuon.Text = $"{phieuQ.Count(p => p.ChiTietPhieuMuons.Any(ct => !ct.TrangThaiTra)):N0} Phiếu";
                
                var ptQ = ctx.PhieuTras.AsQueryable();
                if (since != DateTime.MinValue) ptQ = ptQ.Where(p => p.NgayTra >= since);
                if (branchId != null) ptQ = ptQ.Where(p => p.MaCuaHangNhanTra == branchId);
                decimal tongThu = ptQ.Sum(pt => (decimal?)pt.TongTienThu) ?? 0;
                lblTongDoanhThu.Text = $"{tongThu:N0} VNĐ";
                
                lblPctPhim.Text = "Tổng cộng";
                lblPctBanSao.Text = "Tổng cộng";
                lblPctPhieuMuon.Text = "Chưa trả băng";
                lblPctDoanhThu.Text = $"Từ {(since == DateTime.MinValue ? "đầu" : since.ToString("dd/MM/yy"))}";

                // Top 5 Trending
                var topPhimQ = ctx.ChiTietPhieuMuons
                    .Include(ct => ct.BanSaoBang).ThenInclude(bs => bs!.Phim)
                    .Include(ct => ct.PhieuMuon)
                    .Where(ct => ct.BanSaoBang != null && ct.BanSaoBang.Phim != null);
                if (since != DateTime.MinValue) topPhimQ = topPhimQ.Where(ct => ct.PhieuMuon != null && ct.PhieuMuon.NgayMuon >= since);
                if (branchId != null) topPhimQ = topPhimQ.Where(ct => ct.PhieuMuon != null && ct.PhieuMuon.MaCuaHangMuon == branchId);

                var topPhim = topPhimQ
                    .GroupBy(ct => ct.BanSaoBang!.Phim!.TuaDe)
                    .Select(g => new { TuaDe = g.Key, Luot = g.Count() })
                    .OrderByDescending(x => x.Luot).Take(5).ToList();

                dgvTopTrending.Rows.Clear();
                int idx = 1;
                var imgPlaceholder = MakePlaceholderImage();
                foreach (var item in topPhim)
                    dgvTopTrending.Rows.Add($"#{idx++}", imgPlaceholder, item.TuaDe, $"{item.Luot} lượt");

                // Quá Hạn
                var odQ = ctx.ChiTietPhieuMuons
                    .Include(ct => ct.PhieuMuon).ThenInclude(pm => pm!.KhachHang)
                    .Include(ct => ct.BanSaoBang).ThenInclude(bs => bs!.Phim)
                    .Where(ct => !ct.TrangThaiTra && ct.PhieuMuon != null && ct.PhieuMuon.NgayDuKienTra < now);
                if (branchId != null) odQ = odQ.Where(ct => ct.PhieuMuon != null && ct.PhieuMuon.MaCuaHangMuon == branchId);

                var odRaw = odQ
                    .Select(ct => new { Khach = ct.PhieuMuon!.KhachHang!.HoTen, Phim = ct.BanSaoBang!.Phim!.TuaDe, Due = ct.PhieuMuon.NgayDuKienTra })
                    .ToList();
                dgvQuaHan.Rows.Clear();
                foreach (var od in odRaw.OrderByDescending(x => (now - x.Due).Days))
                    dgvQuaHan.Rows.Add(od.Khach, od.Phim, $"{(now - od.Due).Days} ngày");

                // Charts
                _selectedBarMonth = null;  // Reset về tổng cả năm mỗi khi load lại
                UpdateBarChart(ctx);
                LoadBranchData(ctx, _selectedBarYear, null);  // null = tổng cả năm
                _branchPanel.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dashboard load error: " + ex.Message);
            }
        }

        // ── Bar Chart (12 tháng) ──────────────────────────────────────────────
        private void UpdateBarChart(QuanLyThueBangContext ctx)
        {
            _barChart.Plot.Clear();
            var now = DateTime.Now;
            double[] vals = new double[12];
            string[] labs = new string[12];

            bool isManager = !AppSession.IsAdmin && !string.IsNullOrEmpty(AppSession.CurrentMaCuaHang);
            string? branchId = isManager ? AppSession.CurrentMaCuaHang : null;

            for (int i = 11; i >= 0; i--)
            {
                var d = now.AddMonths(-i);
                int idx2 = 11 - i;
                labs[idx2] = $"T{d.Month}";
                
                var barQ = ctx.PhieuTras.Where(pt => pt.NgayTra.Year == d.Year && pt.NgayTra.Month == d.Month);
                if (branchId != null) barQ = barQ.Where(pt => pt.MaCuaHangNhanTra == branchId);
                
                decimal v = barQ.Sum(pt => (decimal?)pt.TongTienThu) ?? 0;
                vals[idx2] = (double)v;
            }

            var bars = _barChart.Plot.Add.Bars(vals);
            for (int i = 0; i < bars.Bars.Count; i++)
            {
                var d = now.AddMonths(-(11 - i));
                bool isSelected = (d.Year == _selectedBarYear && d.Month == _selectedBarMonth);
                bars.Bars[i].FillColor = isSelected ? ScottPlot.Color.FromHex("#b87b7d") : ScottPlot.Color.FromHex("#6f42c1");
            }

            var ticks = new ScottPlot.Tick[12];
            for (int i = 0; i < 12; i++) ticks[i] = new ScottPlot.Tick(i, labs[i]);
            _barChart.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            _barChart.Plot.Axes.Margins(bottom: 0);
            _barChart.Plot.HideGrid();
            _barChart.Refresh();
        }

        private void UpdateBarChart_HighlightOnly()
        {
            var now = DateTime.Now;
            _barChart.Plot.Clear();
            double[] vals = new double[12];
            string[] labs = new string[12];

            using var scope = _serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();
            bool isManager = !AppSession.IsAdmin && !string.IsNullOrEmpty(AppSession.CurrentMaCuaHang);
            string? branchId = isManager ? AppSession.CurrentMaCuaHang : null;

            for (int i = 11; i >= 0; i--)
            {
                var d = now.AddMonths(-i);
                int idx2 = 11 - i;
                labs[idx2] = $"T{d.Month}";
                
                var barQ = ctx.PhieuTras.Where(pt => pt.NgayTra.Year == d.Year && pt.NgayTra.Month == d.Month);
                if (branchId != null) barQ = barQ.Where(pt => pt.MaCuaHangNhanTra == branchId);
                
                decimal v = barQ.Sum(pt => (decimal?)pt.TongTienThu) ?? 0;
                vals[idx2] = (double)v;
            }
            var bars = _barChart.Plot.Add.Bars(vals);
            for (int i = 0; i < bars.Bars.Count; i++)
            {
                var d = now.AddMonths(-(11 - i));
                bool isSel = (d.Year == _selectedBarYear && d.Month == _selectedBarMonth);
                bars.Bars[i].FillColor = isSel ? ScottPlot.Color.FromHex("#b87b7d") : ScottPlot.Color.FromHex("#6f42c1");
            }
            var ticks = new ScottPlot.Tick[12];
            for (int i = 0; i < 12; i++) ticks[i] = new ScottPlot.Tick(i, labs[i]);
            _barChart.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            _barChart.Plot.Axes.Margins(bottom: 0);
            _barChart.Plot.HideGrid();
            _barChart.Refresh();
        }

        // ── Load branch data — month=null nghĩa là tổng cả năm ──────────────
        private void LoadBranchData(QuanLyThueBangContext ctx, int year, int? month)
        {
            bool isManager = !AppSession.IsAdmin && !string.IsNullOrEmpty(AppSession.CurrentMaCuaHang);
            string? branchId = isManager ? AppSession.CurrentMaCuaHang : null;

            if (branchId == null)
            {
                // ADMIN: Xem doanh thu các chi nhánh
                lblBranchTitle.Text = month.HasValue ? $"🏪 CHI NHÁNH — T{month.Value}/{year}" : $"🏪 DOANH THU CHI NHÁNH — Cả năm {year}";
                lblBranchTitle.ForeColor = month.HasValue ? Color.FromArgb(184, 123, 125) : Color.FromArgb(50, 50, 50);

                var query = ctx.PhieuTras
                    .Include(pt => pt.CuaHangNhanTra)
                    .Where(pt => pt.NgayTra.Year == year && pt.TongTienThu > 0);

                if (month.HasValue)
                    query = query.Where(pt => pt.NgayTra.Month == month.Value);

                var raw = query
                    .GroupBy(pt => pt.CuaHangNhanTra != null ? pt.CuaHangNhanTra.DiaChi : "Không xác định")
                    .Select(g => new { Ten = g.Key, DT = g.Sum(pt => pt.TongTienThu) })
                    .OrderByDescending(x => x.DT)
                    .ToList();

                decimal total = raw.Sum(x => x.DT);
                _branchData = raw.Select(x => (
                    Name: x.Ten,
                    Revenue: x.DT,
                    Pct: total > 0 ? Math.Round((double)x.DT / (double)total * 100, 1) : 0.0
                )).ToList();
            }
            else
            {
                // QUẢN LÝ: Xem phần trăm doanh thu theo Thể Loại của chi nhánh mình
                lblBranchTitle.Text = month.HasValue ? $"🏷️ DOANH THU THỂ LOẠI — T{month.Value}/{year}" : $"🏷️ DOANH THU THỂ LOẠI — Cả năm {year}";
                lblBranchTitle.ForeColor = month.HasValue ? Color.FromArgb(184, 123, 125) : Color.FromArgb(50, 50, 50);

                var query = ctx.ChiTietPhieuTras
                    .Include(ct => ct.PhieuTra)
                    .Include(ct => ct.ChiTietPhieuMuon).ThenInclude(cm => cm!.BanSaoBang).ThenInclude(bs => bs!.Phim).ThenInclude(p => p!.TheLoai)
                    .Where(ct => ct.PhieuTra != null && ct.PhieuTra.NgayTra.Year == year && ct.PhieuTra.MaCuaHangNhanTra == branchId);

                if (month.HasValue)
                    query = query.Where(ct => ct.PhieuTra!.NgayTra.Month == month.Value);

                var raw = query
                    .Where(ct => ct.ChiTietPhieuMuon != null && ct.ChiTietPhieuMuon.BanSaoBang != null && ct.ChiTietPhieuMuon.BanSaoBang.Phim != null && ct.ChiTietPhieuMuon.BanSaoBang.Phim.TheLoai != null)
                    .GroupBy(ct => ct.ChiTietPhieuMuon!.BanSaoBang!.Phim!.TheLoai!.TenTheLoai)
                    .Select(g => new { Ten = g.Key, DT = g.Sum(ct => ct.TienThue + ct.TienPhat) })
                    .OrderByDescending(x => x.DT)
                    .ToList();

                decimal total = raw.Sum(x => x.DT);
                _branchData = raw.Select(x => (
                    Name: x.Ten,
                    Revenue: x.DT,
                    Pct: total > 0 ? Math.Round((double)x.DT / (double)total * 100, 1) : 0.0
                )).ToList();
            }
        }

        // ── Custom Horizontal Bar Chart Paint ────────────────────────────────
        private void BranchPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int panelW = _branchPanel.ClientSize.Width;
            int rowH = 52;
            int paddingLeft = 8;
            int paddingRight = 8;

            if (_branchData.Count == 0)
            {
                using var emptyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                using var emptyBrush = new SolidBrush(Color.FromArgb(160, 160, 160));
                g.DrawString("Chưa có dữ liệu chi nhánh\ntrong tháng này.", emptyFont, emptyBrush,
                    new RectangleF(paddingLeft, 30, panelW - paddingLeft * 2, 60));
                return;
            }

            // Update scroll height
            int totalH = _branchData.Count * rowH + 16;
            if (_branchPanel.AutoScrollMinSize.Height != totalH)
                _branchPanel.AutoScrollMinSize = new Size(0, totalH);

            int scrollY = _branchPanel.AutoScrollPosition.Y;
            int y = scrollY + 8;

            using var nameFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            using var pctFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 8F, FontStyle.Regular);

            int barTrackX = paddingLeft;
            int pctLabelW = 48;
            int barAreaW = panelW - paddingLeft - paddingRight - pctLabelW;

            for (int i = 0; i < _branchData.Count; i++)
            {
                var (name, revenue, pct) = _branchData[i];
                Color barColor = ColorTranslator.FromHtml(BranchColors[i % BranchColors.Length]);
                Color barTrack = Color.FromArgb(235, 238, 245);

                // Row background (hover-like highlight for top 1)
                if (i == 0)
                {
                    using var highlightBrush = new SolidBrush(Color.FromArgb(12, barColor));
                    g.FillRectangle(highlightBrush, new Rectangle(0, y - 4, panelW, rowH));
                }

                // Branch name
                int nameY = y;
                string shortName = ShortenAddress(name);
                using var nameBrush = new SolidBrush(i == 0 ? barColor : Color.FromArgb(40, 40, 40));
                g.DrawString(shortName, nameFont, nameBrush, new RectangleF(barTrackX, nameY, barAreaW - 8, 22));

                // Bar track (background)
                int barY = nameY + 22;
                int barH = 10;
                int barMaxW = barAreaW - 8;

                using var trackBrush = new SolidBrush(barTrack);
                FillRoundedRect(g, trackBrush, new Rectangle(barTrackX, barY, barMaxW, barH), 5);

                // Bar fill
                int fillW = (int)(barMaxW * pct / 100.0);
                if (fillW > 4)
                {
                    using var fillBrush = new LinearGradientBrush(
                        new Rectangle(barTrackX, barY, fillW, barH),
                        barColor,
                        ControlPaint.Light(barColor, 0.3f),
                        LinearGradientMode.Horizontal);
                    FillRoundedRect(g, fillBrush, new Rectangle(barTrackX, barY, fillW, barH), 5);
                }

                // Revenue sub-label
                int revenueY = barY + barH + 2;
                string revenueStr = FormatRevenue(revenue);
                using var subBrush = new SolidBrush(Color.FromArgb(130, 130, 130));
                g.DrawString(revenueStr, subFont, subBrush, new RectangleF(barTrackX, revenueY, barAreaW - 8, 14));

                // Percentage label (right-aligned)
                int pctX = barTrackX + barAreaW;
                using var pctBrush = new SolidBrush(barColor);
                g.DrawString($"{pct:F1}%", pctFont, pctBrush,
                    new RectangleF(pctX, nameY + 8, pctLabelW - 4, 22),
                    new StringFormat { Alignment = StringAlignment.Far });

                // Separator line
                if (i < _branchData.Count - 1)
                {
                    using var sepPen = new Pen(Color.FromArgb(240, 240, 244), 1);
                    g.DrawLine(sepPen, paddingLeft, y + rowH - 4, panelW - paddingRight, y + rowH - 4);
                }

                y += rowH;
            }
        }

        private void FillRoundedRect(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            g.FillPath(brush, path);
        }

        private string FormatRevenue(decimal v)
        {
            if (v >= 1_000_000) return $"{v / 1_000_000:F1}M VNĐ";
            if (v >= 1_000) return $"{v / 1_000:F0}K VNĐ";
            return $"{v:N0} VNĐ";
        }

        private string ShortenAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return "Chi Nhánh";
            var parts = address.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            return parts[0].Trim().Length > 28 ? parts[0].Trim()[..28] + "…" : parts[0].Trim();
        }

        private Image MakePlaceholderImage()
        {
            var bmp = new Bitmap(32, 44);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(220, 225, 235));
            g.DrawRectangle(Pens.LightGray, 0, 0, 31, 43);
            g.DrawString("🎬", new Font("Segoe UI", 10F), Brushes.Gray, new PointF(4, 12));
            return bmp;
        }
    }
}

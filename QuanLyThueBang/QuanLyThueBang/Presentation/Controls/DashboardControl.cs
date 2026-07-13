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

        private Label lblCountPhim = null!;
        private Label lblCountBanSao = null!;
        private Label lblCountPhieuMuon = null!;
        private Label lblTongDoanhThu = null!;
        private DataGridView dgvTopTrending = null!;
        private FormsPlot _formsPlot = null!;

        private int _cntSanSang = 0;
        private int _cntDangMuon = 0;
        private int _cntHuHong = 0;

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

            // Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "📊 TỔNG QUAN BÁO CÁO & THỐNG KÊ (DASHBOARD)",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(184, 123, 125),
                Location = new Point(20, 15),
                AutoSize = true
            };
            var btnRefresh = new Button
            {
                Text = "🔄 Làm Mới Dữ Liệu",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(800, 14),
                Size = new Size(160, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadDashboardData();

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnRefresh);

            // Cards Panel (Top KPI Cards)
            var pnlCards = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 115,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(15, 15, 15, 10)
            };
            for (int i = 0; i < 4; i++) pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            lblCountPhim = CreateKpiCard(pnlCards, 0, "🎬 TỔNG SỐ PHIM", "0 Phim", Color.FromArgb(111, 66, 193));
            lblCountBanSao = CreateKpiCard(pnlCards, 1, "📼 TỔNG KHO BẢN SAO", "0 Cuốn", Color.FromArgb(13, 110, 253));
            lblCountPhieuMuon = CreateKpiCard(pnlCards, 2, "📋 PHIẾU ĐANG MƯỢN", "0 Phiếu", Color.FromArgb(253, 126, 20));
            lblTongDoanhThu = CreateKpiCard(pnlCards, 3, "💰 TỔNG DOAN THU", "0 VNĐ", Color.FromArgb(40, 167, 69));

            // Main Content Split (Trending + Pie Chart)
            var tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(15, 5, 15, 15)
            };
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));

            // Left: Top Trending Phim
            var pnlTrending = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };
            var lblTrendingTitle = new Label { Text = "🔥 TOP 5 BỘ PHIM THUÊ NHIỀU NHẤT (TRENDING)", Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50), Dock = DockStyle.Top, Height = 35 };

            dgvTopTrending = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 42 }
            };
            dgvTopTrending.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold)
            };
            dgvTopTrending.Columns.Add("colSTT", "TOP");
            dgvTopTrending.Columns[0].Width = 55;
            dgvTopTrending.Columns.Add("colTuaDe", "Tựa Đề Phim");
            dgvTopTrending.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTopTrending.Columns.Add("colLuotMuon", "Lượt Thuê");
            dgvTopTrending.Columns[2].Width = 110;

            pnlTrending.Controls.Add(dgvTopTrending);
            pnlTrending.Controls.Add(lblTrendingTitle);

            // Right: Kho Băng Pie Chart (Sử dụng Framework chuyên nghiệp ScottPlot 5)
            var pnlRightBox = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15) };
            var lblPieTitle = new Label { Text = "📦 TỶ LỆ TÌNH TRẠNG KHO BĂNG ĐĨA", Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(50, 50, 50), Dock = DockStyle.Top, Height = 35 };

            _formsPlot = new FormsPlot { Dock = DockStyle.Fill };

            pnlRightBox.Controls.Add(_formsPlot);
            pnlRightBox.Controls.Add(lblPieTitle);

            tlpMain.Controls.Add(pnlTrending, 0, 0);
            tlpMain.Controls.Add(pnlRightBox, 1, 0);

            this.Controls.Add(tlpMain);
            this.Controls.Add(pnlCards);
            this.Controls.Add(pnlHeader);
        }

        private Label CreateKpiCard(TableLayoutPanel parent, int col, string title, string val, Color color)
        {
            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Margin = new Padding(5) };
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = color, Location = new Point(15, 14), AutoSize = true };
            var lblValue = new Label { Text = val, Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), Location = new Point(15, 42), AutoSize = true };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            parent.Controls.Add(card, col, 0);
            return lblValue;
        }

        private void LoadDashboardData()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();

            lblCountPhim.Text = $"{context.Phims.Count():N0} Phim";
            lblCountBanSao.Text = $"{context.BanSaoBangs.Count():N0} Cuốn";
            lblCountPhieuMuon.Text = $"{context.PhieuMuons.Count(p => p.ChiTietPhieuMuons.Any(ct => !ct.TrangThaiTra)):N0} Phiếu";

            decimal tongMuon = context.ChiTietPhieuMuons
                .Include(ct => ct.BanSaoBang)
                .Sum(ct => (decimal?)(ct.BanSaoBang != null ? ct.BanSaoBang.DonGiaThue : 0)) ?? 0;
            decimal tongPhat = context.ChiTietPhieuTras.Sum(ct => (decimal?)ct.TienPhat) ?? 0;
            lblTongDoanhThu.Text = $"{(tongMuon + tongPhat):N0} VNĐ";

            // Top Trending
            var topPhim = context.ChiTietPhieuMuons
                .Include(ct => ct.BanSaoBang)
                .ThenInclude(bs => bs!.Phim)
                .Where(ct => ct.BanSaoBang != null && ct.BanSaoBang.Phim != null)
                .GroupBy(ct => ct.BanSaoBang!.Phim!.TuaDe)
                .Select(g => new { TuaDe = g.Key, Luot = g.Count() })
                .OrderByDescending(x => x.Luot)
                .Take(5)
                .ToList();

            dgvTopTrending.Rows.Clear();
            int idx = 1;
            foreach (var item in topPhim)
            {
                dgvTopTrending.Rows.Add($"#{idx++}", item.TuaDe, $"{item.Luot} lượt");
            }

            // Pie Chart Stats
            _cntSanSang = context.BanSaoBangs.Count(b => b.TrangThai == Constants.TrangThaiBang_SanSang);
            _cntDangMuon = context.BanSaoBangs.Count(b => b.TrangThai == Constants.TrangThaiBang_DangChoMuon);
            _cntHuHong = context.BanSaoBangs.Count(b => b.TrangThai != Constants.TrangThaiBang_SanSang && b.TrangThai != Constants.TrangThaiBang_DangChoMuon);

            UpdatePieChart();
        }

        private void UpdatePieChart()
        {
            _formsPlot.Plot.Clear();

            int total = _cntSanSang + _cntDangMuon + _cntHuHong;
            if (total == 0)
            {
                var emptyPie = _formsPlot.Plot.Add.Pie(new List<ScottPlot.PieSlice>
                {
                    new ScottPlot.PieSlice { Value = 1, FillColor = ScottPlot.Color.FromHex("#E0E4E8"), Label = "Chưa có dữ liệu" }
                });
                _formsPlot.Plot.Axes.Frameless();
                _formsPlot.Plot.HideGrid();
                _formsPlot.Refresh();
                return;
            }

            var slices = new List<ScottPlot.PieSlice>();
            if (_cntSanSang > 0)
            {
                slices.Add(new ScottPlot.PieSlice { Value = _cntSanSang, FillColor = ScottPlot.Color.FromHex("#28a745"), Label = $"Sẵn sàng ({_cntSanSang})" });
            }
            if (_cntDangMuon > 0)
            {
                slices.Add(new ScottPlot.PieSlice { Value = _cntDangMuon, FillColor = ScottPlot.Color.FromHex("#0d6efd"), Label = $"Đang cho mượn ({_cntDangMuon})" });
            }
            if (_cntHuHong > 0)
            {
                slices.Add(new ScottPlot.PieSlice { Value = _cntHuHong, FillColor = ScottPlot.Color.FromHex("#dc3545"), Label = $"Hư hỏng / Bảo trì ({_cntHuHong})" });
            }

            var pie = _formsPlot.Plot.Add.Pie(slices);
            pie.ExplodeFraction = 0.03; // Thêm khoảng hở sang trọng giữa các lát cắt

            _formsPlot.Plot.ShowLegend();
            _formsPlot.Plot.Axes.Frameless();
            _formsPlot.Plot.HideGrid();
            _formsPlot.Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.HoaDon
{
    public class ChiTietPhieuMuonForm : Form
    {
        private readonly Color SurfaceDim = Color.FromArgb(233, 214, 211);
        private readonly Color SurfaceLowest = Color.White;
        private readonly Color Brand200 = Color.FromArgb(245, 214, 218);
        private readonly Color Brand50 = Color.FromArgb(253, 246, 247);
        private readonly Color Primary = Color.FromArgb(140, 74, 82);
        private readonly Color OnSurface = Color.FromArgb(35, 25, 25);
        private readonly Color OnSurfaceVariant = Color.FromArgb(83, 67, 68);
        private readonly Color OutlineVariant = Color.FromArgb(216, 194, 195);
        
        private PhieuMuonViewDTO _phieuMuon;
        private List<ChiTietPhieuMuonViewDTO> _chiTiets;

        public ChiTietPhieuMuonForm(PhieuMuonViewDTO phieuMuon, List<ChiTietPhieuMuonViewDTO> chiTiets)
        {
            _phieuMuon = phieuMuon;
            _chiTiets = chiTiets;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi Tiết Băng Trong Phiếu Mượn";
            this.Size = new Size(950, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SurfaceDim;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(30, 30, 30, 30);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SurfaceLowest,
                Padding = new Padding(40)
            };
            mainPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(OutlineVariant, 1);
                var rect = new Rectangle(0, 0, mainPanel.Width - 1, mainPanel.Height - 1);
                e.Graphics.DrawPath(pen, RoundedRect(rect, 10));
            };

            // Top Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 90 };
            var pnlHeaderBorder = new Panel { Dock = DockStyle.Bottom, Height = 2, BackColor = Brand200 };
            pnlHeader.Controls.Add(pnlHeaderBorder);

            var lblLogo = new Label
            {
                Text = "SB",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Primary,
                BackColor = Brand200,
                Size = new Size(50, 50),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblLogo.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(Brand200);
                e.Graphics.FillEllipse(brush, 0, 0, lblLogo.Width - 1, lblLogo.Height - 1);
                using var textBrush = new SolidBrush(Primary);
                var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(lblLogo.Text, lblLogo.Font, textBrush, new Rectangle(0, 0, lblLogo.Width, lblLogo.Height), format);
            };

            var lblTitle = new Label { Text = "Serene Boutique", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Primary, Location = new Point(65, 10), AutoSize = true };
            var lblSubtitle = new Label { Text = "Hóa Đơn Thuê Băng Đĩa", Font = new Font("Segoe UI", 10F), ForeColor = OnSurfaceVariant, Location = new Point(68, 42), AutoSize = true };

            var lblMaPhieuTitle = new Label { Text = "MÃ PHIẾU MƯỢN", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = OnSurfaceVariant, Location = new Point(mainPanel.Width - 250, 15), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var lblMaPhieu = new Label { Text = _phieuMuon.MaPhieuMuon, Font = new Font("Consolas", 14F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(mainPanel.Width - 250, 35), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Brand50, Padding = new Padding(5) };

            pnlHeader.Controls.AddRange(new Control[] { lblLogo, lblTitle, lblSubtitle, lblMaPhieuTitle, lblMaPhieu });
            
            mainPanel.Resize += (s, e) => {
                lblMaPhieuTitle.Left = mainPanel.Width - 240;
                lblMaPhieu.Left = mainPanel.Width - 240;
            };

            // Customer Section
            var pnlCustomer = new Panel { Dock = DockStyle.Top, Height = 100, Padding = new Padding(0, 20, 0, 10) };
            var lblCusTitle = new Label { Text = "THÔNG TIN KHÁCH HÀNG", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = OnSurfaceVariant, Location = new Point(0, 20), AutoSize = true };
            
            var pnlCusBox = new Panel { Location = new Point(0, 45), Size = new Size(300, 65), BackColor = Brand50 };
            pnlCusBox.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(OutlineVariant, 1);
                e.Graphics.DrawPath(pen, RoundedRect(new Rectangle(0, 0, pnlCusBox.Width - 1, pnlCusBox.Height - 1), 6));
            };
            var lblCusName = new Label { Text = _phieuMuon.TenKhachHang, Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(15, 10), AutoSize = true, BackColor = Color.Transparent };
            var lblCusCode = new Label { Text = $"Mã KH: {_phieuMuon.MaKhachHang}", Font = new Font("Segoe UI", 9.5F), ForeColor = Primary, Location = new Point(15, 35), AutoSize = true, BackColor = Color.Transparent };
            pnlCusBox.Controls.AddRange(new Control[] { lblCusName, lblCusCode });

            var lblDate = new Label { Text = $"Ngày lập: {_phieuMuon.NgayMuon:dd/MM/yyyy}", Font = new Font("Segoe UI", 10F), ForeColor = OnSurfaceVariant, Location = new Point(mainPanel.Width - 240, 45), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            pnlCustomer.Controls.AddRange(new Control[] { lblCusTitle, pnlCusBox, lblDate });
            
            mainPanel.Resize += (s, e) => {
                lblDate.Left = mainPanel.Width - 240;
            };

            // DataGridView Section
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 20, 0, 20) };
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = SurfaceLowest,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Brand200
            };
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Brand50, ForeColor = OnSurfaceVariant, Font = new Font("Segoe UI Semibold", 9.5F), SelectionBackColor = Brand50, SelectionForeColor = OnSurfaceVariant, Padding = new Padding(10) };
            grid.DefaultCellStyle = new DataGridViewCellStyle { BackColor = SurfaceLowest, ForeColor = OnSurface, SelectionBackColor = Brand50, SelectionForeColor = OnSurface, Padding = new Padding(10), Font = new Font("Segoe UI", 10F) };
            grid.RowTemplate.Height = 45;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 45;
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaBanSao", HeaderText = "Mã Bản Sao", Width = 130 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TuaDe", HeaderText = "Tựa Đề Phim", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenTheLoai", HeaderText = "Thể Loại", Width = 150 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThaiTra", HeaderText = "Tình Trạng Trả", Width = 130, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGiaThue", HeaderText = "Đơn Giá Thuê", Width = 130, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
            
            grid.CellFormatting += (s, e) => {
                if (grid.Columns[e.ColumnIndex].DataPropertyName == "TrangThaiTra" && e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status.Equals("Đã trả", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.OrangeRed;
                    }
                }
            };
            
            grid.DataSource = _chiTiets;
            
            pnlGrid.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(OutlineVariant, 1);
                e.Graphics.DrawPath(pen, RoundedRect(new Rectangle(0, 20, pnlGrid.Width - 1, pnlGrid.Height - 41), 8));
            };
            var pnlGridContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(1, 21, 1, 21) };
            pnlGridContainer.Controls.Add(grid);
            pnlGrid.Controls.Add(pnlGridContainer);

            // Summary Section
            var pnlSummary = new Panel { Dock = DockStyle.Bottom, Height = 140 };
            var pnlSumTopBorder = new Panel { Dock = DockStyle.Top, Height = 2, BackColor = Brand200 };
            pnlSummary.Controls.Add(pnlSumTopBorder);

            var lblThanks1 = new Label { Text = "Cảm ơn quý khách đã sử dụng dịch vụ tại Serene Boutique.", Font = new Font("Segoe UI", 10F), ForeColor = OnSurfaceVariant, Location = new Point(0, 20), AutoSize = true };
            var lblThanks2 = new Label { Text = "Vui lòng hoàn trả băng đĩa đúng hạn để tránh phí phạt.", Font = new Font("Segoe UI", 10F), ForeColor = OnSurfaceVariant, Location = new Point(0, 45), AutoSize = true };

            var pnlSumBox = new Panel { Size = new Size(300, 90), Location = new Point(mainPanel.Width - 340, 20), BackColor = Brand50, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            pnlSumBox.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Brand200, 1);
                e.Graphics.DrawPath(pen, RoundedRect(new Rectangle(0, 0, pnlSumBox.Width - 1, pnlSumBox.Height - 1), 8));
            };
            
            int soLuong = _chiTiets.Count;
            decimal tongTien = _chiTiets.Sum(x => x.DonGiaThue);

            var lblQtyTitle = new Label { Text = "Tổng số lượng:", Font = new Font("Segoe UI", 10F), ForeColor = OnSurfaceVariant, Location = new Point(15, 15), AutoSize = true, BackColor = Color.Transparent };
            var lblQty = new Label { Text = soLuong.ToString(), Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = OnSurface, Location = new Point(260, 15), AutoSize = true, BackColor = Color.Transparent };
            
            var pnlSumBoxDiv = new Panel { Location = new Point(15, 45), Size = new Size(270, 1), BackColor = Brand200 };
            
            var lblTotTitle = new Label { Text = "Tổng Tiền:", Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Primary, Location = new Point(15, 55), AutoSize = true, BackColor = Color.Transparent };
            var lblTot = new Label { Text = $"{tongTien:N0} ₫", Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), ForeColor = Primary, Location = new Point(160, 55), Size = new Size(125, 25), BackColor = Color.Transparent, TextAlign = ContentAlignment.TopRight };
            
            pnlSumBox.Controls.AddRange(new Control[] { lblQtyTitle, lblQty, pnlSumBoxDiv, lblTotTitle, lblTot });

            pnlSummary.Controls.AddRange(new Control[] { lblThanks1, lblThanks2, pnlSumBox });
            
            mainPanel.Resize += (s, e) => {
                pnlSumBox.Left = mainPanel.Width - 340;
            };

            // Action Footer
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(0, 20, 0, 0) };
            var btnClose = new Button
            {
                Text = "Đóng",
                Size = new Size(120, 40),
                Location = new Point(mainPanel.Width - 160, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            pnlFooter.Controls.Add(btnClose);
            
            mainPanel.Resize += (s, e) => {
                btnClose.Left = mainPanel.Width - 160;
            };

            mainPanel.Controls.Add(pnlGrid);
            mainPanel.Controls.Add(pnlCustomer);
            mainPanel.Controls.Add(pnlHeader);
            mainPanel.Controls.Add(pnlSummary);
            mainPanel.Controls.Add(pnlFooter);
            
            pnlHeader.BringToFront();
            pnlCustomer.BringToFront();
            pnlGrid.BringToFront();
            pnlSummary.SendToBack();
            pnlFooter.SendToBack();

            this.Controls.Add(mainPanel);
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}

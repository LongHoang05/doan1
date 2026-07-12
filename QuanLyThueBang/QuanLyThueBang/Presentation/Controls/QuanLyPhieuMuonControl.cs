using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Helpers;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyPhieuMuonControl : UserControl
    {
        private readonly MuonTraService _muonTraService;
        private readonly KhachHangService _khachHangService;
        private readonly CuaHangService _cuaHangService;
        private readonly NhanVienService _nhanVienService;

        private TabControl _tabControl = null!;
        private TabPage _tabDanhSach = null!;
        private TabPage _tabLapPhieu = null!;

        // --- TAB 1: DANH SÁCH & TRA CỨU PHIẾU MƯỢN ---
        private TextBox txtSearchPhieuMuon = null!;
        private DataGridView dgvPhieuMuon = null!;
        private DataGridView dgvChiTietPhieuMuon = null!;
        private Label lblHeaderChiTiet = null!;
        private readonly List<PhieuMuonViewDTO> _phieuMuonList = new();

        // --- TAB 2: LẬP PHIẾU MƯỢN ---
        private ComboBox cboKhachHangMuon = null!;
        private ComboBox cboCuaHangMuon = null!;
        private ComboBox cboNhanVienMuon = null!;
        private TextBox txtInputMaBanSaoMuon = null!;
        private DataGridView dgvGioMuon = null!;
        private DateTimePicker dtpNgayDuKienTra = null!;
        private Label lblTongTienGioMuon = null!;
        private readonly List<ChiTietGioMuonDTO> _gioMuonList = new();

        public QuanLyPhieuMuonControl(MuonTraService muonTraService, KhachHangService khachHangService, CuaHangService cuaHangService, NhanVienService nhanVienService)
        {
            _muonTraService = muonTraService;
            _khachHangService = khachHangService;
            _cuaHangService = cuaHangService;
            _nhanVienService = nhanVienService;

            InitializeUI();
            LoadMasterData();
            LoadDanhSachPhieuMuon();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            var layoutMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            layoutMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            layoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Header Panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(25, 12, 25, 12)
            };

            var lblTitle = new Label
            {
                Text = "Quản Lý Phiếu Mượn & Lập Phiếu Mượn Băng",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 12),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Tra cứu danh sách hồ sơ phiếu mượn, chi tiết băng đã thuê và lập phiếu mượn mới tại quầy.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 46),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            layoutMain.Controls.Add(pnlHeader, 0, 0);

            // Body Panel
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 15),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Padding = new Point(18, 8)
            };

            _tabDanhSach = new TabPage("📂 1. Danh Sách & Tra Cứu Phiếu Mượn") { BackColor = Color.FromArgb(248, 249, 250) };
            _tabLapPhieu = new TabPage("➕ 2. Lập Phiếu Mượn Mới (Tại Quầy)") { BackColor = Color.FromArgb(248, 249, 250) };

            BuildTabDanhSachUI(_tabDanhSach);
            BuildTabLapPhieuUI(_tabLapPhieu);

            _tabControl.TabPages.Add(_tabDanhSach);
            _tabControl.TabPages.Add(_tabLapPhieu);

            _tabControl.SelectedIndexChanged += (s, e) =>
            {
                if (_tabControl.SelectedTab == _tabDanhSach)
                {
                    LoadDanhSachPhieuMuon();
                }
            };

            pnlBody.Controls.Add(_tabControl);
            layoutMain.Controls.Add(pnlBody, 0, 1);

            this.Controls.Add(layoutMain);
        }

        #region TAB 1: DANH SÁCH & TRA CỨU PHIẾU MƯỢN
        private void BuildTabDanhSachUI(TabPage tab)
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.White, Padding = new Padding(15) };

            var lblSearch = new Label { Text = "Tìm Kiếm:", Location = new Point(15, 22), AutoSize = true };
            txtSearchPhieuMuon = new TextBox { Location = new Point(95, 18), Width = 280, PlaceholderText = "Nhập mã phiếu mượn hoặc tên KH..." };
            txtSearchPhieuMuon.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; LoadDanhSachPhieuMuon(); } };

            var btnSearch = new Button
            {
                Text = "🔍 Tìm Kiếm",
                Location = new Point(390, 16),
                Size = new Size(120, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => LoadDanhSachPhieuMuon();

            var btnReload = new Button
            {
                Text = "🔄 Làm Mới",
                Location = new Point(520, 16),
                Size = new Size(110, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += (s, e) =>
            {
                txtSearchPhieuMuon.Clear();
                LoadDanhSachPhieuMuon();
            };

            var btnSwitchLapPhieu = new Button
            {
                Text = "➕ Lập Phiếu Mới",
                Location = new Point(640, 16),
                Size = new Size(135, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSwitchLapPhieu.FlatAppearance.BorderSize = 0;
            btnSwitchLapPhieu.Click += (s, e) => _tabControl.SelectedTab = _tabLapPhieu;

            var btnPrintInvoice = new Button
            {
                Text = "🖨️ In Hóa Đơn",
                Location = new Point(785, 16),
                Size = new Size(125, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnPrintInvoice.FlatAppearance.BorderSize = 0;
            btnPrintInvoice.Click += (s, e) =>
            {
                if (dgvPhieuMuon.CurrentRow?.DataBoundItem is PhieuMuonViewDTO pm)
                {
                    var details = _muonTraService.GetChiTietByPhieuMuon(pm.MaPhieuMuon);
                    var dtoList = details.Select(x => new ChiTietGioMuonDTO
                    {
                        MaBanSao = x.MaBanSao,
                        TuaDe = x.TuaDe,
                        TenTheLoai = x.TenTheLoai,
                        DonGiaThue = x.DonGiaThue
                    }).ToList();
                    ExportHelper.ExportPhieuMuonInvoice(pm, dtoList);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một Phiếu Mượn trên bảng để in hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            var btnExportExcel = new Button
            {
                Text = "📊 Xuất Excel",
                Location = new Point(920, 16),
                Size = new Size(115, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportDataGridViewToExcel(dgvPhieuMuon, "DanhSachPhieuMuon");

            pnlTop.Controls.AddRange(new Control[] { lblSearch, txtSearchPhieuMuon, btnSearch, btnReload, btnSwitchLapPhieu, btnPrintInvoice, btnExportExcel });

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 240,
                BackColor = Color.FromArgb(230, 235, 240)
            };

            dgvPhieuMuon = SetupGrid();
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.MaPhieuMuon), HeaderText = "Mã Phiếu", Width = 140, MinimumWidth = 120 });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.TenKhachHang), HeaderText = "Khách Hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 180 });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.TenCuaHang), HeaderText = "Chi Nhánh", Width = 160, MinimumWidth = 140 });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.TenNhanVien), HeaderText = "Nhân Viên Lập", Width = 160, MinimumWidth = 140 });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.NgayMuon), HeaderText = "Ngày Lập", Width = 140, MinimumWidth = 120, DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" } });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.NgayDuKienTra), HeaderText = "Hẹn Trả", Width = 120, MinimumWidth = 110, DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.SoLuongBang), HeaderText = "SL Băng", Width = 95, MinimumWidth = 85, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(PhieuMuonViewDTO.TrangThaiPhieu), HeaderText = "Trạng Thái", Width = 125, MinimumWidth = 115 });

            var colDelPhieu = new DataGridViewButtonColumn { Name = "colDelPhieu", HeaderText = "Hành Động", Text = "🗑️ Xóa Phiếu", UseColumnTextForButtonValue = true, Width = 120, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvPhieuMuon.Columns.Add(colDelPhieu);

            dgvPhieuMuon.SelectionChanged += (s, e) =>
            {
                if (dgvPhieuMuon.CurrentRow != null)
                {
                    var pm = dgvPhieuMuon.CurrentRow.DataBoundItem as PhieuMuonViewDTO;
                    if (pm != null)
                    {
                        LoadChiTietPhieuMuon(pm);
                    }
                }
            };

            dgvPhieuMuon.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgvPhieuMuon.Columns["colDelPhieu"].Index)
                {
                    var item = _phieuMuonList[e.RowIndex];
                    var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu mượn '{item.MaPhieuMuon}' không?", "Xác Nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        var (success, msg) = _muonTraService.XoaPhieuMuon(item.MaPhieuMuon);
                        MessageBox.Show(msg, success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                        if (success) LoadDanhSachPhieuMuon();
                    }
                }
            };

            splitContainer.Panel1.Controls.Add(dgvPhieuMuon);

            var pnlDetailContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var pnlDetailHeader = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.FromArgb(243, 244, 246), Padding = new Padding(15, 8, 15, 8) };
            lblHeaderChiTiet = new Label { Text = "📦 Chi Tiết Băng Trong Phiếu Mượn", Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41), AutoSize = true };
            pnlDetailHeader.Controls.Add(lblHeaderChiTiet);

            dgvChiTietPhieuMuon = SetupGrid();
            dgvChiTietPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietPhieuMuonViewDTO.MaBanSao), HeaderText = "Mã Bản Sao", Width = 150 });
            dgvChiTietPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietPhieuMuonViewDTO.TuaDe), HeaderText = "Tựa Đề Phim", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTietPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietPhieuMuonViewDTO.TenTheLoai), HeaderText = "Thể Loại", Width = 180 });
            dgvChiTietPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietPhieuMuonViewDTO.DonGiaThue), HeaderText = "Đơn Giá Thuê", Width = 150, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
            dgvChiTietPhieuMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietPhieuMuonViewDTO.TrangThaiTra), HeaderText = "Tình Trạng Trả", Width = 140, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });

            pnlDetailContainer.Controls.Add(dgvChiTietPhieuMuon);
            pnlDetailContainer.Controls.Add(pnlDetailHeader);

            splitContainer.Panel2.Controls.Add(pnlDetailContainer);

            var pnlBodyDanhSach = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 0) };
            pnlBodyDanhSach.Controls.Add(splitContainer);

            tab.Controls.Add(pnlBodyDanhSach);
            tab.Controls.Add(pnlTop);
        }

        private void LoadDanhSachPhieuMuon()
        {
            _phieuMuonList.Clear();
            _phieuMuonList.AddRange(_muonTraService.GetAllPhieuMuon(txtSearchPhieuMuon.Text.Trim()));
            dgvPhieuMuon.DataSource = null;
            dgvPhieuMuon.DataSource = _phieuMuonList;

            if (_phieuMuonList.Count > 0)
            {
                LoadChiTietPhieuMuon(_phieuMuonList[0]);
            }
            else
            {
                dgvChiTietPhieuMuon.DataSource = null;
                lblHeaderChiTiet.Text = "📦 Chi Tiết Băng Trong Phiếu Mượn";
            }
        }

        private void LoadChiTietPhieuMuon(PhieuMuonViewDTO pm)
        {
            lblHeaderChiTiet.Text = $"📦 Chi Tiết Băng Trong Phiếu Mượn: [{pm.MaPhieuMuon}] — Khách hàng: {pm.TenKhachHang}";
            var chiTiets = _muonTraService.GetChiTietByPhieuMuon(pm.MaPhieuMuon);
            dgvChiTietPhieuMuon.DataSource = null;
            dgvChiTietPhieuMuon.DataSource = chiTiets;
        }
        #endregion

        #region TAB 2: LẬP PHIẾU MƯỢN MỚI
        private void BuildTabLapPhieuUI(TabPage tab)
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 155, BackColor = Color.White };

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(15, 12, 15, 8)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            // Dòng 1: Khách Hàng & Chi Nhánh
            var lblKH = new Label { Text = "Khách Hàng:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboKhachHangMuon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblCH = new Label { Text = "Chi Nhánh:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboCuaHangMuon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            tlp.Controls.Add(lblKH, 0, 0);
            tlp.Controls.Add(cboKhachHangMuon, 1, 0);
            tlp.Controls.Add(lblCH, 2, 0);
            tlp.Controls.Add(cboCuaHangMuon, 3, 0);

            // Dòng 2: Nhân Viên & Hẹn Trả
            var lblNV = new Label { Text = "Nhân Viên:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboNhanVienMuon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblNgayTra = new Label { Text = "Hẹn Trả:", Anchor = AnchorStyles.Left, AutoSize = true };
            dtpNgayDuKienTra = new DateTimePicker { Dock = DockStyle.Left, Width = 160, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(3) };

            tlp.Controls.Add(lblNV, 0, 1);
            tlp.Controls.Add(cboNhanVienMuon, 1, 1);
            tlp.Controls.Add(lblNgayTra, 2, 1);
            tlp.Controls.Add(dtpNgayDuKienTra, 3, 1);

            // Dòng 3: Quét Mã Băng & Nút Thêm Vào Giỏ
            var lblScan = new Label { Text = "Quét Mã Băng / RFID:", Anchor = AnchorStyles.Left, AutoSize = true };
            txtInputMaBanSaoMuon = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Nhập mã bản sao hoặc RFID..." };

            var btnAddGio = new Button
            {
                Text = "➕ Thêm Vào Giỏ",
                Dock = DockStyle.Left,
                Width = 175,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnAddGio.FlatAppearance.BorderSize = 0;
            btnAddGio.Click += BtnAddGio_Click;
            txtInputMaBanSaoMuon.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnAddGio_Click(s, e); } };

            tlp.Controls.Add(lblScan, 0, 2);
            tlp.Controls.Add(txtInputMaBanSaoMuon, 1, 2);
            tlp.SetColumnSpan(btnAddGio, 2);
            tlp.Controls.Add(btnAddGio, 2, 2);

            pnlTop.Controls.Add(tlp);

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.White, Padding = new Padding(15, 11, 15, 11) };
            lblTongTienGioMuon = new Label { Text = "Tổng Tiền Thuê Dự Kiến: 0 VNĐ", Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), ForeColor = Color.FromArgb(184, 123, 125), Dock = DockStyle.Left, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };

            var btnChotMuon = new Button
            {
                Text = "💾 Chốt Phiếu Mượn",
                Dock = DockStyle.Right,
                Width = 240,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChotMuon.FlatAppearance.BorderSize = 0;
            btnChotMuon.Click += BtnChotMuon_Click;
            pnlBottom.Controls.Add(lblTongTienGioMuon);
            pnlBottom.Controls.Add(btnChotMuon);

            dgvGioMuon = SetupGrid();
            dgvGioMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietGioMuonDTO.MaBanSao), HeaderText = "Mã Bản Sao", Width = 140, MinimumWidth = 120 });
            dgvGioMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietGioMuonDTO.RFID), HeaderText = "Thẻ RFID", Width = 140, MinimumWidth = 120 });
            dgvGioMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietGioMuonDTO.TuaDe), HeaderText = "Tựa Đề Phim", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 200 });
            dgvGioMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietGioMuonDTO.TenTheLoai), HeaderText = "Thể Loại", Width = 160, MinimumWidth = 140 });
            dgvGioMuon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ChiTietGioMuonDTO.DonGiaThue), HeaderText = "Đơn Giá Thuê", Width = 155, MinimumWidth = 140, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });

            var colRemove = new DataGridViewButtonColumn { Name = "colRemove", HeaderText = "Hành Động", Text = "🗑️ Xóa", UseColumnTextForButtonValue = true, Width = 140, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvGioMuon.Columns.Add(colRemove);
            dgvGioMuon.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgvGioMuon.Columns["colRemove"].Index)
                {
                    _gioMuonList.RemoveAt(e.RowIndex);
                    RefreshGioMuonGrid();
                }
            };

            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            pnlGrid.Controls.Add(dgvGioMuon);

            tab.Controls.Add(pnlGrid);
            tab.Controls.Add(pnlBottom);
            tab.Controls.Add(pnlTop);
        }

        private void BtnAddGio_Click(object? sender, EventArgs e)
        {
            string query = txtInputMaBanSaoMuon.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            string maCH = cboCuaHangMuon.SelectedValue?.ToString() ?? "";
            var (isValid, err, item) = _muonTraService.ValidateAndGetBangForMuon(query, maCH);
            if (!isValid || item == null)
            {
                MessageBox.Show(err, "Không Đủ Điều Kiện Cho Mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_gioMuonList.Any(g => g.MaBanSao == item.MaBanSao))
            {
                MessageBox.Show("Cuốn băng này đã có trong giỏ hàng mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _gioMuonList.Add(item);
            txtInputMaBanSaoMuon.Clear();
            RefreshGioMuonGrid();
        }

        private void RefreshGioMuonGrid()
        {
            dgvGioMuon.DataSource = null;
            dgvGioMuon.DataSource = _gioMuonList.ToList();
            decimal tong = _gioMuonList.Sum(x => x.DonGiaThue);
            lblTongTienGioMuon.Text = $"Tổng Tiền Thuê Dự Kiến: {tong:N0} VNĐ";
        }

        private void BtnChotMuon_Click(object? sender, EventArgs e)
        {
            if (_gioMuonList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 1 cuốn băng vào giỏ mượn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKH = cboKhachHangMuon.SelectedValue?.ToString() ?? "";
            string maCH = cboCuaHangMuon.SelectedValue?.ToString() ?? "";
            string maNV = cboNhanVienMuon.SelectedValue?.ToString() ?? "";

            var (success, msg, maPM) = _muonTraService.LapPhieuMuon(maKH, maCH, maNV, _gioMuonList.Select(g => g.MaBanSao).ToList(), dtpNgayDuKienTra.Value);
            if (success)
            {
                MessageBox.Show(msg, "Lập Phiếu Mượn Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gioMuonList.Clear();
                RefreshGioMuonGrid();
                _tabControl.SelectedTab = _tabDanhSach;
                LoadDanhSachPhieuMuon();
            }
            else
            {
                MessageBox.Show(msg, "Lỗi Lập Phiếu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void LoadMasterData()
        {
            try
            {
                var khs = _khachHangService.GetAllKhachHang();
                cboKhachHangMuon.DataSource = khs;
                cboKhachHangMuon.DisplayMember = nameof(KhachHangDTO.HoTen);
                cboKhachHangMuon.ValueMember = nameof(KhachHangDTO.MaKhachHang);

                var chs = _cuaHangService.GetAllCuaHang();
                cboCuaHangMuon.DataSource = chs.ToList();
                cboCuaHangMuon.DisplayMember = nameof(CuaHangDTO.DiaChi);
                cboCuaHangMuon.ValueMember = nameof(CuaHangDTO.MaCuaHang);

                var nvs = _nhanVienService.GetAllNhanVien();
                cboNhanVienMuon.DataSource = nvs.ToList();
                cboNhanVienMuon.DisplayMember = nameof(NhanVienDTO.HoTen);
                cboNhanVienMuon.ValueMember = nameof(NhanVienDTO.MaNhanVien);
            }
            catch { }
        }

        private DataGridView SetupGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeight = 46,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowTemplate = { Height = 44 },
                AutoGenerateColumns = false
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Padding = new Padding(8, 4, 8, 4) };
            dgv.DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.FromArgb(250, 235, 235), SelectionForeColor = Color.FromArgb(184, 123, 125), Padding = new Padding(10, 0, 10, 0) };
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(252, 253, 254) };
            return dgv;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Presentation.Forms.HoaDon;

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
        private readonly List<PhieuMuonViewDTO> _phieuMuonList = new();
        private TextBox txtKhachHangMuon = null!;
        private KhachHangDTO? _selectedKhachHang = null;
        private ToolStripDropDown _popupKhachHang = null!;
        private ListBox _lstKhachHangSuggestions = null!;
        private bool _isSelectingKH = false;
        private ComboBox cboCuaHangMuon = null!;
        private ComboBox cboNhanVienMuon = null!;
        private ComboBox cboInputMaBanSaoMuon = null!;
        private Label lblTongTienGioMuon = null!;
        private DataGridView dgvGioMuon = null!;
        private readonly List<ChiTietGioMuonDTO> _gioMuonList = new();
        private List<KhachHangDTO> _allKhachHang = new();
        private List<string> _allMaBanSao = new();
        private DateTimePicker dtpNgayDuKienTra = null!;
        private System.Windows.Forms.Timer _typingTimerKH = null!;
        private System.Windows.Forms.Timer _typingTimerBS = null!;

        public QuanLyPhieuMuonControl(MuonTraService muonTraService, KhachHangService khachHangService, CuaHangService cuaHangService, NhanVienService nhanVienService)
        {
            _muonTraService = muonTraService;
            _khachHangService = khachHangService;
            _cuaHangService = cuaHangService;
            _nhanVienService = nhanVienService;

            InitializeUI();
            
            _typingTimerKH = new System.Windows.Forms.Timer { Interval = 400 };
            _typingTimerKH.Tick += TypingTimerKH_Tick;
            
            _typingTimerBS = new System.Windows.Forms.Timer { Interval = 400 };
            _typingTimerBS.Tick += TypingTimerBS_Tick;

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

            var colViewPhieu = new DataGridViewButtonColumn { Name = "colViewPhieu", HeaderText = "", Text = "👁 Chi Tiết", UseColumnTextForButtonValue = true, Width = 100, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvPhieuMuon.Columns.Add(colViewPhieu);

            var colDelPhieu = new DataGridViewButtonColumn { Name = "colDelPhieu", HeaderText = "Hành Động", Text = "🗑️ Xóa Phiếu", UseColumnTextForButtonValue = true, Width = 100, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvPhieuMuon.Columns.Add(colDelPhieu);

            dgvPhieuMuon.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var item = _phieuMuonList[e.RowIndex];
                    if (e.ColumnIndex == dgvPhieuMuon.Columns["colViewPhieu"].Index)
                    {
                        OpenChiTietDialog(item);
                    }
                    else if (e.ColumnIndex == dgvPhieuMuon.Columns["colDelPhieu"].Index)
                    {
                        var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu mượn '{item.MaPhieuMuon}' không?", "Xác Nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirm == DialogResult.Yes)
                        {
                            var (success, msg) = _muonTraService.XoaPhieuMuon(item.MaPhieuMuon);
                            MessageBox.Show(msg, success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                            if (success) LoadDanhSachPhieuMuon();
                        }
                    }
                }
            };

            dgvPhieuMuon.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    OpenChiTietDialog(_phieuMuonList[e.RowIndex]);
                }
            };

            var pnlBodyDanhSach = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 0) };
            pnlBodyDanhSach.Controls.Add(dgvPhieuMuon);

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
                // Optional: Select first row implicitly
            }
        }

        private void OpenChiTietDialog(PhieuMuonViewDTO pm)
        {
            var chiTiets = _muonTraService.GetChiTietByPhieuMuon(pm.MaPhieuMuon);
            using var dlg = new ChiTietPhieuMuonForm(pm, chiTiets);
            dlg.ShowDialog(this.FindForm());
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
            txtKhachHangMuon = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Nhập tên, CMND, SĐT..." };
            txtKhachHangMuon.TextChanged += (s, e) => {
                if (_isSelectingKH) return;
                _selectedKhachHang = null;
                _typingTimerKH.Stop();
                _typingTimerKH.Start();
            };
            txtKhachHangMuon.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Down && _popupKhachHang.Visible)
                {
                    _lstKhachHangSuggestions.Focus();
                    if (_lstKhachHangSuggestions.Items.Count > 0) _lstKhachHangSuggestions.SelectedIndex = 0;
                }
            };

            _lstKhachHangSuggestions = new ListBox
            {
                DrawMode = DrawMode.OwnerDrawVariable,
                BorderStyle = BorderStyle.None,
                Width = 400,
                BackColor = Color.FromArgb(245, 245, 245)
            };
            _lstKhachHangSuggestions.MeasureItem += (s, e) => e.ItemHeight = 45;
            _lstKhachHangSuggestions.DrawItem += LstKhachHangSuggestions_DrawItem;
            _lstKhachHangSuggestions.Click += LstKhachHangSuggestions_Click;
            _lstKhachHangSuggestions.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LstKhachHangSuggestions_Click(s, e); };

            var host = new ToolStripControlHost(_lstKhachHangSuggestions) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false };
            _popupKhachHang = new ToolStripDropDown { Padding = Padding.Empty, Margin = Padding.Empty, AutoSize = false };
            _popupKhachHang.Items.Add(host);

            var lblCH = new Label { Text = "Chi Nhánh:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboCuaHangMuon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            tlp.Controls.Add(lblKH, 0, 0);
            tlp.Controls.Add(txtKhachHangMuon, 1, 0);
            tlp.Controls.Add(lblCH, 2, 0);
            tlp.Controls.Add(cboCuaHangMuon, 3, 0);

            // Dòng 2: Nhân Viên & Hẹn Trả
            var lblNV = new Label { Text = "Nhân Viên:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboNhanVienMuon = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblNgayTra = new Label { Text = "Hẹn Trả:", Anchor = AnchorStyles.Left, AutoSize = true };
            dtpNgayDuKienTra = new DateTimePicker { Dock = DockStyle.Left, Width = 160, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(3) };
            dtpNgayDuKienTra.ValueChanged += (s, e) => RefreshGioMuonGrid();

            tlp.Controls.Add(lblNV, 0, 1);
            tlp.Controls.Add(cboNhanVienMuon, 1, 1);
            tlp.Controls.Add(lblNgayTra, 2, 1);
            tlp.Controls.Add(dtpNgayDuKienTra, 3, 1);

            // Dòng 3: Quét Mã Băng & Nút Thêm Vào Giỏ
            var lblScan = new Label { Text = "Quét Mã Băng / RFID:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboInputMaBanSaoMuon = new ComboBox 
            { 
                Dock = DockStyle.Fill, 
                DropDownStyle = ComboBoxStyle.DropDown
            };
            cboInputMaBanSaoMuon.TextUpdate += CboInputMaBanSaoMuon_TextUpdate;

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
            cboInputMaBanSaoMuon.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnAddGio_Click(s, e); } };

            tlp.Controls.Add(lblScan, 0, 2);
            tlp.Controls.Add(cboInputMaBanSaoMuon, 1, 2);
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
            string query = cboInputMaBanSaoMuon.Text.Trim();
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
            cboInputMaBanSaoMuon.Text = "";
            RefreshGioMuonGrid();
        }

        private void RefreshGioMuonGrid()
        {
            dgvGioMuon.DataSource = null;
            dgvGioMuon.DataSource = _gioMuonList.ToList();
            int soNgay = Math.Max(1, (dtpNgayDuKienTra.Value.Date - DateTime.Today).Days);
            decimal tongDonGia = _gioMuonList.Sum(x => x.DonGiaThue);
            decimal tongTienThue = tongDonGia * soNgay;
            lblTongTienGioMuon.Text = $"Thuê {soNgay} ngày  |  Tổng Tiền (Thanh Toán Ngay): {tongTienThue:N0} VNĐ";
        }

        private string GetSelectedId(ComboBox cbo)
        {

            if (cbo.SelectedItem is KhachHangDTO kh)
                return kh.MaKhachHang;
            if (cbo.SelectedItem is CuaHangDTO ch)
                return ch.MaCuaHang;
            if (cbo.SelectedItem is NhanVienDTO nv)
                return nv.MaNhanVien;
            if (cbo.SelectedValue is string strId && !string.IsNullOrWhiteSpace(strId))
                return strId;
            return cbo.Text;
        }

        private void BtnChotMuon_Click(object? sender, EventArgs e)
        {
            if (_gioMuonList.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 1 cuốn băng vào giỏ mượn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKH = _selectedKhachHang?.MaKhachHang ?? txtKhachHangMuon.Text;
            string maCH = GetSelectedId(cboCuaHangMuon);
            string maNV = GetSelectedId(cboNhanVienMuon);

            var (success, msg, maPM) = _muonTraService.LapPhieuMuon(maKH, maCH, maNV, _gioMuonList.Select(g => g.MaBanSao).ToList(), dtpNgayDuKienTra.Value);
            if (success)
            {
                int soNgay = Math.Max(1, (dtpNgayDuKienTra.Value.Date - DateTime.Today).Days);
                decimal tongTienThue = _gioMuonList.Sum(x => x.DonGiaThue) * soNgay;
                var copyList = _gioMuonList.ToList();
                MessageBox.Show($"{msg}\n\n💰 Khách hàng thuê trong {soNgay} ngày - Đã thanh toán Tiền Thuê: {tongTienThue:N0} VNĐ cho {copyList.Count} cuốn băng.", "Lập Phiếu Mượn & Thanh Toán Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var askPrint = MessageBox.Show("Bạn có muốn xem và in Hóa Đơn Thuê Băng (Đã Thanh Toán) cho khách hàng không?", "In Hóa Đơn Thuê Băng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (askPrint == DialogResult.Yes)
                {
                    using var dlg = new HoaDonMuonDialogForm(maPM, txtKhachHangMuon.Text, cboCuaHangMuon.Text, cboNhanVienMuon.Text, dtpNgayDuKienTra.Value, copyList);
                    dlg.ShowDialog(this.FindForm());
                }

                _gioMuonList.Clear();
                _selectedKhachHang = null;
                txtKhachHangMuon.Text = "";
                cboInputMaBanSaoMuon.SelectedIndex = -1;
                cboInputMaBanSaoMuon.Text = "";
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
                _allKhachHang = _khachHangService.GetAllKhachHang();

                cboCuaHangMuon.DisplayMember = nameof(CuaHangDTO.DiaChi);
                cboCuaHangMuon.ValueMember = nameof(CuaHangDTO.MaCuaHang);
                cboCuaHangMuon.DataSource = _cuaHangService.GetAllCuaHang();

                cboNhanVienMuon.DisplayMember = nameof(NhanVienDTO.HoTen);
                cboNhanVienMuon.ValueMember = nameof(NhanVienDTO.MaNhanVien);
                cboNhanVienMuon.DataSource = _nhanVienService.GetAllNhanVien();

                _allMaBanSao = _muonTraService.GetActiveMaBanSaoList();
                cboInputMaBanSaoMuon.Items.Clear();
                cboInputMaBanSaoMuon.Items.AddRange(_allMaBanSao.Take(10).ToArray());
                cboInputMaBanSaoMuon.SelectedIndex = -1;
            }
            catch { }
        }

        private void TypingTimerKH_Tick(object? sender, EventArgs e)
        {
            _typingTimerKH.Stop();
            string search = txtKhachHangMuon.Text.Trim();

            var filtered = string.IsNullOrWhiteSpace(search) 
                ? _allKhachHang.Take(10).ToArray()
                : _allKhachHang.Where(k => 
                    k.HoTen.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                    k.MaKhachHang.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    k.CMND.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    k.SoDienThoai.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Take(10).ToArray();

            _lstKhachHangSuggestions.Items.Clear();
            if (filtered.Length > 0)
            {
                var newSize = new Size(400, filtered.Length * 45);
                _lstKhachHangSuggestions.Size = newSize;
                if (_popupKhachHang.Items.Count > 0) 
                    _popupKhachHang.Items[0].Size = newSize;
                _popupKhachHang.Size = newSize;

                _lstKhachHangSuggestions.Items.AddRange(filtered);
                _popupKhachHang.Show(txtKhachHangMuon, new Point(0, txtKhachHangMuon.Height));
            }
            else
            {
                _popupKhachHang.Hide();
            }
        }

        private void LstKhachHangSuggestions_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();

            var kh = (KhachHangDTO)_lstKhachHangSuggestions.Items[e.Index];

            using var fontBold = new Font(e.Font ?? new Font("Segoe UI", 10F), FontStyle.Bold);
            using var fontSmall = new Font(e.Font?.FontFamily ?? new FontFamily("Segoe UI"), (e.Font?.Size ?? 10F) - 1F, FontStyle.Regular);
            
            var brushText = new SolidBrush(e.ForeColor);
            var brushGray = new SolidBrush(Color.Gray);

            e.Graphics.DrawString(kh.HoTen, fontBold, brushText, e.Bounds.Left + 5, e.Bounds.Top + 5);
            e.Graphics.DrawString($"SĐT: {kh.SoDienThoai}  |  CMND: {kh.CMND}  |  ĐC: {kh.DiaChi}", fontSmall, brushGray, e.Bounds.Left + 5, e.Bounds.Top + 25);
            
            e.DrawFocusRectangle();
        }

        private void LstKhachHangSuggestions_Click(object? sender, EventArgs e)
        {
            if (_lstKhachHangSuggestions.SelectedItem is KhachHangDTO kh)
            {
                _isSelectingKH = true;
                _selectedKhachHang = kh;
                txtKhachHangMuon.Text = kh.HoTen;
                txtKhachHangMuon.SelectionStart = txtKhachHangMuon.Text.Length;
                _popupKhachHang.Hide();
                txtKhachHangMuon.Focus();
                _isSelectingKH = false;
            }
        }

        private void CboInputMaBanSaoMuon_TextUpdate(object? sender, EventArgs e)
        {
            _typingTimerBS.Stop();
            _typingTimerBS.Start();
        }

        private void TypingTimerBS_Tick(object? sender, EventArgs e)
        {
            _typingTimerBS.Stop();
            string search = cboInputMaBanSaoMuon.Text;

            var filtered = string.IsNullOrWhiteSpace(search) 
                ? _allMaBanSao.Take(10).ToArray()
                : _allMaBanSao.Where(k => k.Contains(search, StringComparison.OrdinalIgnoreCase)).Take(10).ToArray();

            cboInputMaBanSaoMuon.Items.Clear();
            cboInputMaBanSaoMuon.Items.AddRange(filtered);
            
            if (cboInputMaBanSaoMuon.Text != search)
            {
                cboInputMaBanSaoMuon.Text = search;
            }

            if (filtered.Length > 0)
            {
                cboInputMaBanSaoMuon.DroppedDown = true;
                cboInputMaBanSaoMuon.SelectionStart = cboInputMaBanSaoMuon.Text.Length;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                cboInputMaBanSaoMuon.DroppedDown = false;
            }
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Presentation.Forms.HoaDon;
using QuanLyThueBang.Presentation.Forms.MuonTra;

namespace QuanLyThueBang.Presentation.Controls
{
    public class MuonTraBangControl : UserControl
    {
        private readonly MuonTraService _muonTraService;
        private readonly KhachHangService _khachHangService;
        private readonly CuaHangService _cuaHangService;
        private readonly NhanVienService _nhanVienService;

        private TabControl _tabControl = null!;
        private TabPage _tabTra = null!;
        private TabPage _tabQuaHan = null!;

        // --- TAB 1: NHẬN TRẢ BĂNG & LUÂN CHUYỂN KHO ---
        private TextBox txtInputMaBanSaoTra = null!;
        private ComboBox cboCuaHangTra = null!;
        private ComboBox cboNhanVienTra = null!;
        private DataGridView dgvDanhSachTra = null!;
        private Label lblTongTienTra = null!;
        private Label lblInvoiceMaPhieu = null!;
        private Label lblInvoiceTenKH = null!;
        private Label lblInvoiceMaKH = null!;
        private Label lblInvoiceTongSoLuong = null!;
        private Panel pnlInvoiceCard = null!;
        private readonly List<ThongTinBangMuonChuaTraDTO> _danhSachTraList = new();

        // --- TAB 2: QUÉT BĂNG QUÁ HẠN ---
        private DataGridView dgvQuaHan = null!;

        public MuonTraBangControl(MuonTraService muonTraService, KhachHangService khachHangService, CuaHangService cuaHangService, NhanVienService nhanVienService)
        {
            _muonTraService = muonTraService;
            _khachHangService = khachHangService;
            _cuaHangService = cuaHangService;
            _nhanVienService = nhanVienService;

            InitializeUI();
            LoadMasterData();
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
                Text = "Nghiệp Vụ Nhận Trả Băng & Luân Chuyển Kho",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 12),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Xử lý nhận trả băng liên chi nhánh (Cross-store), tính tiền phạt vi phạm và quét cảnh báo băng quá hạn.",
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

            _tabTra = new TabPage("📥 1. Nhận Trả Băng & Luân Chuyển Kho") { BackColor = Color.FromArgb(248, 249, 250) };
            _tabQuaHan = new TabPage("🚨 2. Cảnh Báo Băng Quá Hạn") { BackColor = Color.FromArgb(248, 249, 250) };

            BuildTabTraUI(_tabTra);
            BuildTabQuaHanUI(_tabQuaHan);

            _tabControl.TabPages.Add(_tabTra);
            _tabControl.TabPages.Add(_tabQuaHan);

            _tabControl.SelectedIndexChanged += (s, e) =>
            {
                if (_tabControl.SelectedTab == _tabQuaHan)
                {
                    LoadDanhSachQuaHan();
                }
            };

            pnlBody.Controls.Add(_tabControl);
            layoutMain.Controls.Add(pnlBody, 0, 1);

            this.Controls.Add(layoutMain);
        }

        #region TAB 1: NHẬN TRẢ BĂNG & LUÂN CHUYỂN KHO
        private void BuildTabTraUI(TabPage tab)
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 115, BackColor = Color.White };

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                Padding = new Padding(15, 12, 15, 8)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));

            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            // Dòng 1: Chi Nhánh & Nhân Viên
            var lblCHTra = new Label { Text = "CH Nhận Trả:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboCuaHangTra = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblNVTra = new Label { Text = "Nhân Viên:", Anchor = AnchorStyles.Left, AutoSize = true };
            cboNhanVienTra = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };

            tlp.Controls.Add(lblCHTra, 0, 0);
            tlp.Controls.Add(cboCuaHangTra, 1, 0);
            tlp.Controls.Add(lblNVTra, 2, 0);
            tlp.Controls.Add(cboNhanVienTra, 3, 0);

            // Dòng 2: Quét Mã & Nút Nhận Diện
            var lblScanTra = new Label { Text = "Tìm Phiếu Mượn:", Anchor = AnchorStyles.Left, AutoSize = true };
            txtInputMaBanSaoTra = new TextBox 
            { 
                Dock = DockStyle.Fill, 
                PlaceholderText = "Nhập Mã Phiếu Mượn...",
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.CustomSource
            };
            var autoCompleteList = _muonTraService.GetActiveMaPhieuMuonList();
            var source = new AutoCompleteStringCollection();
            source.AddRange(autoCompleteList.ToArray());
            txtInputMaBanSaoTra.AutoCompleteCustomSource = source;

            var btnScanTra = new Button
            {
                Text = "🔍 Tìm Kiếm",
                Dock = DockStyle.Fill,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnScanTra.FlatAppearance.BorderSize = 0;
            btnScanTra.Click += BtnScanTra_Click;
            txtInputMaBanSaoTra.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnScanTra_Click(s, e); } };

            tlp.Controls.Add(lblScanTra, 0, 1);
            tlp.Controls.Add(txtInputMaBanSaoTra, 1, 1);
            tlp.Controls.Add(btnScanTra, 2, 1);

            pnlTop.Controls.Add(tlp);

            // Bottom Panel: Chốt trả (Main Action)
            var pnlBottomActions = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.White, Padding = new Padding(15, 11, 15, 11) };
            var btnChotTra = new Button
            {
                Text = "✅ Chốt Nhận Trả & Luân Chuyển Kho",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(140, 74, 82),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            btnChotTra.FlatAppearance.BorderSize = 0;
            btnChotTra.Click += BtnChotTra_Click;

            var btnInBienLai = new Button
            {
                Text = "🖨️ Xem & In Hóa Đơn",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnInBienLai.FlatAppearance.BorderSize = 0;
            btnInBienLai.Click += (s, e) => OpenReceiptDialog("PT_TEMP");

            pnlBottomActions.Controls.Add(btnChotTra);
            pnlBottomActions.Controls.Add(new Panel { Width = 12, Dock = DockStyle.Right });
            pnlBottomActions.Controls.Add(btnInBienLai);

            // Header Info Panel (2x2 layout like the screenshot)
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.White, Padding = new Padding(15, 20, 15, 10) };
            
            var tlpHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.White
            };
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            var lblCustomerTitle = new Label { Text = "Khách hàng: ", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Margin = new Padding(0) };
            lblInvoiceTenKH = new Label { Text = "---", Font = new Font("Segoe UI", 11F), AutoSize = true, Margin = new Padding(0) };
            var pnlCustomer = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Dock = DockStyle.Fill };
            pnlCustomer.Controls.Add(lblCustomerTitle);
            pnlCustomer.Controls.Add(lblInvoiceTenKH);

            var lblBranchTitle = new Label { Text = "Chi nhánh: ", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Margin = new Padding(0) };
            lblInvoiceMaPhieu = new Label { Text = "---", Font = new Font("Segoe UI", 11F), AutoSize = true, Margin = new Padding(0) }; // Reusing for Branch
            var pnlBranch = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Dock = DockStyle.Fill };
            pnlBranch.Controls.Add(lblBranchTitle);
            pnlBranch.Controls.Add(lblInvoiceMaPhieu); // This will hold Chi Nhanh

            var lblDateTitle = new Label { Text = "Ngày hẹn trả: ", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Margin = new Padding(0) };
            lblInvoiceMaKH = new Label { Text = "---", Font = new Font("Segoe UI", 11F), AutoSize = true, Margin = new Padding(0) }; // Reusing for Date
            var pnlDate = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Dock = DockStyle.Fill };
            pnlDate.Controls.Add(lblDateTitle);
            pnlDate.Controls.Add(lblInvoiceMaKH); // This will hold NgayHenTra

            var lblEmpTitle = new Label { Text = "Nhân viên lập: ", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Margin = new Padding(0) };
            var lblEmployeeName = new Label { Text = "---", Name = "lblEmployeeName", Font = new Font("Segoe UI", 11F), AutoSize = true, Margin = new Padding(0) };
            var pnlEmp = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Dock = DockStyle.Fill };
            pnlEmp.Controls.Add(lblEmpTitle);
            pnlEmp.Controls.Add(lblEmployeeName);

            tlpHeader.Controls.Add(pnlCustomer, 0, 0);
            tlpHeader.Controls.Add(pnlBranch, 1, 0);
            tlpHeader.Controls.Add(pnlDate, 0, 1);
            tlpHeader.Controls.Add(pnlEmp, 1, 1);

            pnlHeader.Controls.Add(tlpHeader);

            // Grid Section
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 10, 15, 10), BackColor = Color.White };
            dgvDanhSachTra = SetupGrid();
            dgvDanhSachTra.ReadOnly = false; // Allow checking the "Chọn" checkbox
            dgvDanhSachTra.BackgroundColor = Color.White;
            dgvDanhSachTra.BorderStyle = BorderStyle.None;
            dgvDanhSachTra.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDanhSachTra.GridColor = Color.FromArgb(222, 226, 230);
            dgvDanhSachTra.EnableHeadersVisualStyles = false;
            dgvDanhSachTra.ColumnHeadersHeight = 45;
            dgvDanhSachTra.RowTemplate.Height = 45;
            dgvDanhSachTra.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvDanhSachTra.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvDanhSachTra.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            dgvDanhSachTra.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgvDanhSachTra.DefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 249, 250);
            dgvDanhSachTra.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDanhSachTra.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            // Draw top and bottom border for header
            dgvDanhSachTra.Paint += (s, e) =>
            {
                var pen = new Pen(Color.FromArgb(222, 226, 230), 2);
                e.Graphics.DrawLine(pen, 0, 0, dgvDanhSachTra.Width, 0);
                e.Graphics.DrawLine(pen, 0, 44, dgvDanhSachTra.Width, 44);
            };

            var colCheckTra = new DataGridViewCheckBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.IsSelected), HeaderText = "Chọn", Width = 60 };
            dgvDanhSachTra.Columns.Add(colCheckTra);
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.MaBanSao), HeaderText = "Mã Bản Sao", Width = 120, MinimumWidth = 110, ReadOnly = true, DefaultCellStyle = { Font = new Font("Segoe UI", 10F, FontStyle.Bold) } });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TuaDe), HeaderText = "Tựa Đề Phim", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 180, ReadOnly = true });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TenTheLoai), HeaderText = "Thể Loại", Width = 140, MinimumWidth = 120, ReadOnly = true });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.DonGiaThue), HeaderText = "Đơn Giá Thuê", Width = 130, MinimumWidth = 110, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, ReadOnly = true });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TinhTrangKhiTra), HeaderText = "Tình Trạng", Width = 140, MinimumWidth = 120, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }, ReadOnly = true });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TienPhat), HeaderText = "Tiền Phạt", Width = 110, MinimumWidth = 100, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0", ForeColor = Color.Red }, ReadOnly = true });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TongTien), HeaderText = "Thành Tiền", Width = 120, MinimumWidth = 110, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }, ReadOnly = true });

            var colPhat = new DataGridViewButtonColumn { Name = "colEditPhat", HeaderText = "Hành Động", Text = "⚙️", UseColumnTextForButtonValue = true, Width = 95, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvDanhSachTra.Columns.Add(colPhat);

            dgvDanhSachTra.CurrentCellDirtyStateChanged += (s, e) => { if (dgvDanhSachTra.IsCurrentCellDirty) dgvDanhSachTra.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            dgvDanhSachTra.CellValueChanged += (s, e) => { if (e.ColumnIndex == dgvDanhSachTra.Columns[0].Index) RefreshDanhSachTraGrid(); };
            dgvDanhSachTra.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var item = _danhSachTraList[e.RowIndex];
                    if (e.ColumnIndex == dgvDanhSachTra.Columns["colEditPhat"].Index)
                    {
                        OpenDialogTinhPhat(item);
                    }
                }
            };
            dgvDanhSachTra.CellFormatting += (s, e) =>
            {
                // Format DonGia, TienPhat, TongTien to add " VNĐ"
                if (e.RowIndex >= 0 && e.Value != null)
                {
                    var colName = dgvDanhSachTra.Columns[e.ColumnIndex].DataPropertyName;
                    if (colName == nameof(ThongTinBangMuonChuaTraDTO.DonGiaThue) || 
                        colName == nameof(ThongTinBangMuonChuaTraDTO.TienPhat) || 
                        colName == nameof(ThongTinBangMuonChuaTraDTO.TongTien))
                    {
                        if (decimal.TryParse(e.Value.ToString(), out decimal val))
                        {
                            e.Value = $"{val:N0} VNĐ";
                            e.FormattingApplied = true;
                        }
                    }
                }
            };

            pnlGrid.Controls.Add(dgvDanhSachTra);

            // Summary Section (Bottom inside Card)
            var pnlSummary = new Panel { Dock = DockStyle.Bottom, Height = 80, BackColor = Color.White, Padding = new Padding(0, 15, 15, 0) };
            
            var pnlTotalBox = new Panel { Size = new Size(350, 65), Location = new Point(pnlSummary.Width - 350, 5), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            
            var lblCountLabel = new Label { Text = "Tổng số băng trả:", Font = new Font("Segoe UI", 11F, FontStyle.Regular), ForeColor = Color.FromArgb(101, 75, 75), AutoSize = true, Location = new Point(20, 5) };
            lblInvoiceTongSoLuong = new Label { Text = "0", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(101, 75, 75), AutoSize = false, Size = new Size(180, 25), Location = new Point(160, 5), TextAlign = ContentAlignment.MiddleRight, Anchor = AnchorStyles.Top | AnchorStyles.Right };

            var lblTotalLabel = new Label { Text = "TỔNG CỘNG:", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(101, 75, 75), AutoSize = true, Location = new Point(20, 35) };
            lblTongTienTra = new Label { Text = "0 VNĐ", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.FromArgb(101, 75, 75), AutoSize = false, Size = new Size(180, 25), Location = new Point(160, 34), TextAlign = ContentAlignment.MiddleRight, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            
            pnlTotalBox.Controls.Add(lblCountLabel);
            pnlTotalBox.Controls.Add(lblInvoiceTongSoLuong);
            pnlTotalBox.Controls.Add(lblTotalLabel);
            pnlTotalBox.Controls.Add(lblTongTienTra);
            
            pnlSummary.Resize += (s, e) => {
                pnlTotalBox.Left = pnlSummary.Width - pnlTotalBox.Width - 5;
            };
            pnlSummary.Controls.Add(pnlTotalBox);

            pnlInvoiceCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Visible = false };
            
            // Add in reverse order of Dock Fill priority!
            pnlInvoiceCard.Controls.Add(pnlHeader); // Top
            pnlInvoiceCard.Controls.Add(pnlSummary); // Bottom
            pnlInvoiceCard.Controls.Add(pnlGrid); // Fill
            pnlGrid.BringToFront();

            tab.Controls.Add(pnlInvoiceCard);
            tab.Controls.Add(pnlBottomActions);
            tab.Controls.Add(pnlTop);
        }

        private void BtnScanTra_Click(object? sender, EventArgs e)
        {
            string query = txtInputMaBanSaoTra.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            var (success, err, dataList) = _muonTraService.TraCuuBangMuonChuaTraTheoPhieu(query);
            if (!success || dataList == null)
            {
                MessageBox.Show(err, "Không Tìm Thấy Phiếu Mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _danhSachTraList.Clear();
            _danhSachTraList.AddRange(dataList);

            var first = dataList[0];
            lblInvoiceTenKH.Text = $"{first.MaKhachHang} - {first.HoTenKhachHang}";
            lblInvoiceMaPhieu.Text = first.TenChiNhanh; // lblInvoiceMaPhieu reused for Branch
            lblInvoiceMaKH.Text = first.NgayDuKienTra.ToString("dd/MM/yyyy"); // lblInvoiceMaKH reused for Date
            
            var lblEmp = pnlInvoiceCard.Controls.Find("lblEmployeeName", true).FirstOrDefault() as Label;
            if (lblEmp != null) lblEmp.Text = first.TenNhanVienLap;

            pnlInvoiceCard.Visible = true;

            RefreshDanhSachTraGrid();

            // Cảnh báo băng trễ hạn
            var listTreHan = dataList.Where(x => x.IsTreHan).ToList();
            if (listTreHan.Any())
            {
                string warnMsg = $"CẢNH BÁO: Phiếu mượn này có {listTreHan.Count} cuốn băng đã bị trễ hạn!\nVui lòng kiểm tra tình trạng băng và nhập tiền phạt nếu cần.";
                MessageBox.Show(warnMsg, "Băng Trễ Hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OpenDialogTinhPhat(ThongTinBangMuonChuaTraDTO item)
        {
            using var dlg = new PhatHongDialogForm(item, isBaoHongMatDirect: false);
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                item.TinhTrangKhiTra = dlg.TinhTrangChon;
                item.TienPhat = dlg.TienPhatChon;
                RefreshDanhSachTraGrid();
            }
        }

        private void RefreshDanhSachTraGrid()
        {
            dgvDanhSachTra.DataSource = null;
            dgvDanhSachTra.DataSource = _danhSachTraList.ToList();
            
            int selectedCount = _danhSachTraList.Count(x => x.IsSelected);
            decimal tong = _danhSachTraList.Where(x => x.IsSelected).Sum(x => x.TongTien);
            
            if (lblInvoiceTongSoLuong != null)
            {
                lblInvoiceTongSoLuong.Text = selectedCount.ToString();
            }
            lblTongTienTra.Text = $"{tong:N0} VNĐ";
        }

        private void BtnChotTra_Click(object? sender, EventArgs e)
        {
            var selectedItems = _danhSachTraList.Where(x => x.IsSelected).ToList();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Chưa chọn cuốn băng nào để trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKH = selectedItems[0].MaKhachHang;
            string maCHTra = cboCuaHangTra.SelectedValue?.ToString() ?? "";
            string maNVTra = cboNhanVienTra.SelectedValue?.ToString() ?? "";

            var (success, msg, maPT) = _muonTraService.ChotNhanTraBang(maKH, maCHTra, maNVTra, selectedItems);
            if (success)
            {
                MessageBox.Show(msg, "Hoàn Tất Nhận Trả & Luân Chuyển Kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Loại bỏ những băng đã trả thành công ra khỏi lưới
                _danhSachTraList.RemoveAll(x => x.IsSelected);
                RefreshDanhSachTraGrid();
                
                if (_danhSachTraList.Count == 0)
                {
                    pnlInvoiceCard.Visible = false;
                }

                var dlgAsk = MessageBox.Show("Bạn có muốn xem và in Biên Lai Trả Băng cho khách hàng không?", "In Biên Lai", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgAsk == DialogResult.Yes)
                {
                    using var dlgReceipt = new HoaDonDialogForm(maPT, selectedItems[0].HoTenKhachHang, cboCuaHangTra.Text, cboNhanVienTra.Text, selectedItems);
                    dlgReceipt.ShowDialog(this.FindForm());
                }
                
                // Cập nhật lại autocomplete
                var autoCompleteList = _muonTraService.GetActiveMaPhieuMuonList();
                var source = new AutoCompleteStringCollection();
                source.AddRange(autoCompleteList.ToArray());
                txtInputMaBanSaoTra.AutoCompleteCustomSource = source;
            }
            else
            {
                MessageBox.Show(msg, "Lỗi Nhận Trả", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenReceiptDialog(string maPT)
        {
            if (_danhSachTraList.Count == 0)
            {
                MessageBox.Show("Chưa có cuốn băng nào trong danh sách trả bên dưới để in hóa đơn.\nVui lòng quét hoặc thêm băng mang trả trước.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tenKH = _danhSachTraList[0].HoTenKhachHang;
            string tenCH = cboCuaHangTra.Text;
            string tenNV = cboNhanVienTra.Text;
            using var dlg = new HoaDonDialogForm(maPT, tenKH, tenCH, tenNV, _danhSachTraList.ToList());
            dlg.ShowDialog(this.FindForm());
        }
        #endregion

        #region TAB 2: QUÉT BĂNG QUÁ HẠN
        private void BuildTabQuaHanUI(TabPage tab)
        {
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White, Padding = new Padding(15) };
            var btnReload = new Button
            {
                Text = "🔄 Quét Lại Danh Sách Quá Hạn",
                Location = new Point(15, 12),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(12, 5, 12, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReload.FlatAppearance.BorderSize = 0;
            btnReload.Click += (s, e) => LoadDanhSachQuaHan();
            pnlTop.Controls.Add(btnReload);

            dgvQuaHan = SetupGrid();
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.MaPhieuMuon), HeaderText = "Mã Phiếu Mượn", Width = 140, MinimumWidth = 125 });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.HoTenKhachHang), HeaderText = "Khách Hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 180 });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.SoDienThoai), HeaderText = "Số Điện Thoại", Width = 150, MinimumWidth = 135 });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.MaBanSao), HeaderText = "Mã Bản Sao", Width = 135, MinimumWidth = 120 });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.TuaDe), HeaderText = "Tựa Đề Phim", Width = 200, MinimumWidth = 170 });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.NgayDuKienTra), HeaderText = "Ngày Hẹn Trả", Width = 140, MinimumWidth = 125, DefaultCellStyle = { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvQuaHan.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(BangQuaHanDTO.SoNgayTreHan), HeaderText = "Số Ngày Trễ", Width = 130, MinimumWidth = 115, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, ForeColor = Color.Red, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold) } });

            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            pnlGrid.Controls.Add(dgvQuaHan);

            tab.Controls.Add(pnlGrid);
            tab.Controls.Add(pnlTop);
        }

        private void LoadDanhSachQuaHan()
        {
            var list = _muonTraService.GetDanhSachBangQuaHan();
            dgvQuaHan.DataSource = null;
            dgvQuaHan.DataSource = list;
        }
        #endregion

        private void LoadMasterData()
        {
            try
            {
                var chs = _cuaHangService.GetAllCuaHang();
                cboCuaHangTra.DataSource = chs.ToList();
                cboCuaHangTra.DisplayMember = nameof(CuaHangDTO.DiaChi);
                cboCuaHangTra.ValueMember = nameof(CuaHangDTO.MaCuaHang);

                var nvs = _nhanVienService.GetAllNhanVien();
                cboNhanVienTra.DataSource = nvs.ToList();
                cboNhanVienTra.DisplayMember = nameof(NhanVienDTO.HoTen);
                cboNhanVienTra.ValueMember = nameof(NhanVienDTO.MaNhanVien);
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

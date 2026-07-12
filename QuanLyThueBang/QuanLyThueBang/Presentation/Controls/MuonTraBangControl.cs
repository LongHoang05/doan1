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
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
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
            var lblScanTra = new Label { Text = "Quét Mã Băng Trả:", Anchor = AnchorStyles.Left, AutoSize = true };
            txtInputMaBanSaoTra = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Quét MaBanSao hoặc RFID..." };

            var btnScanTra = new Button
            {
                Text = "🔍 Nhận Diện Băng Trả",
                Dock = DockStyle.Left,
                Width = 195,
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
            tlp.SetColumnSpan(btnScanTra, 2);
            tlp.Controls.Add(btnScanTra, 2, 1);

            pnlTop.Controls.Add(tlp);

            // Bottom Panel: Chốt trả
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 65, BackColor = Color.White, Padding = new Padding(15, 11, 15, 11) };
            lblTongTienTra = new Label { Text = "Tổng Thu Giao Dịch Trả: 0 VNĐ", Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), ForeColor = Color.FromArgb(40, 167, 69), Dock = DockStyle.Left, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };

            var btnChotTra = new Button
            {
                Text = "✅ Chốt Nhận Trả & Luân Chuyển Kho",
                Dock = DockStyle.Right,
                Width = 285,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnChotTra.FlatAppearance.BorderSize = 0;
            btnChotTra.Click += BtnChotTra_Click;
            pnlBottom.Controls.Add(lblTongTienTra);
            pnlBottom.Controls.Add(btnChotTra);

            // Grid Danh Sách Trả
            dgvDanhSachTra = SetupGrid();
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.MaPhieuMuon), HeaderText = "Mã Phiếu Mượn", Width = 140, MinimumWidth = 125 });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.MaBanSao), HeaderText = "Mã Bản Sao", Width = 130, MinimumWidth = 115 });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TuaDe), HeaderText = "Tựa Đề Phim", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 180 });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.HoTenKhachHang), HeaderText = "Khách Mượn", Width = 160, MinimumWidth = 140 });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.SoNgayTreHan), HeaderText = "Số Ngày Trễ", Width = 120, MinimumWidth = 110, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TinhTrangKhiTra), HeaderText = "Tình Trạng Trả", Width = 160, MinimumWidth = 140 });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TienPhat), HeaderText = "Tiền Phạt (VNĐ)", Width = 150, MinimumWidth = 135, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
            dgvDanhSachTra.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ThongTinBangMuonChuaTraDTO.TongTien), HeaderText = "Thành Tiền", Width = 140, MinimumWidth = 125, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });

            var colPhat = new DataGridViewButtonColumn { Name = "colEditPhat", HeaderText = "Hành Động", Text = "⚙️ Phạt/Hỏng", UseColumnTextForButtonValue = true, Width = 140, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            var colXoaTra = new DataGridViewButtonColumn { Name = "colRemoveTra", HeaderText = "", Text = "🗑️ Bỏ", UseColumnTextForButtonValue = true, Width = 85, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), Alignment = DataGridViewContentAlignment.MiddleCenter } };
            dgvDanhSachTra.Columns.Add(colPhat);
            dgvDanhSachTra.Columns.Add(colXoaTra);

            dgvDanhSachTra.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var item = _danhSachTraList[e.RowIndex];
                    if (e.ColumnIndex == dgvDanhSachTra.Columns["colRemoveTra"].Index)
                    {
                        _danhSachTraList.RemoveAt(e.RowIndex);
                        RefreshDanhSachTraGrid();
                    }
                    else if (e.ColumnIndex == dgvDanhSachTra.Columns["colEditPhat"].Index)
                    {
                        OpenDialogTinhPhat(item);
                    }
                }
            };

            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            pnlGrid.Controls.Add(dgvDanhSachTra);

            tab.Controls.Add(pnlGrid);
            tab.Controls.Add(pnlBottom);
            tab.Controls.Add(pnlTop);
        }

        private void BtnScanTra_Click(object? sender, EventArgs e)
        {
            string query = txtInputMaBanSaoTra.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            var (success, err, data) = _muonTraService.TraCuuBangMuonChuaTra(query);
            if (!success || data == null)
            {
                MessageBox.Show(err, "Không Tìm Thấy Phiếu Mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_danhSachTraList.Any(x => x.MaBanSao == data.MaBanSao))
            {
                MessageBox.Show("Cuốn băng này đã được quét vào danh sách trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (data.IsTreHan)
            {
                MessageBox.Show($"CẢNH BÁO: Cuốn băng '{data.TuaDe}' đã bị trễ hạn {data.SoNgayTreHan} ngày!\nVui lòng kiểm tra tình trạng băng và nhập tiền phạt nếu cần.", "Băng Trễ Hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _danhSachTraList.Add(data);
            txtInputMaBanSaoTra.Clear();
            RefreshDanhSachTraGrid();
        }

        private void OpenDialogTinhPhat(ThongTinBangMuonChuaTraDTO item)
        {
            string promptPhat = $"Nhập số tiền phạt (VNĐ) cho băng [{item.MaBanSao} - {item.TuaDe}]:";
            string inputTien = Microsoft.VisualBasic.Interaction.InputBox(promptPhat, "Ghi Nhận Phạt Vi Phạm", item.TienPhat.ToString("F0"));
            if (decimal.TryParse(inputTien, out decimal tienPhat) && tienPhat >= 0)
            {
                item.TienPhat = tienPhat;
                string inputTinhTrang = Microsoft.VisualBasic.Interaction.InputBox("Mô tả tình trạng khi trả:", "Tình Trạng Vật Lý", item.TinhTrangKhiTra);
                if (!string.IsNullOrWhiteSpace(inputTinhTrang))
                    item.TinhTrangKhiTra = inputTinhTrang;

                RefreshDanhSachTraGrid();
            }
        }

        private void RefreshDanhSachTraGrid()
        {
            dgvDanhSachTra.DataSource = null;
            dgvDanhSachTra.DataSource = _danhSachTraList.ToList();
            decimal tong = _danhSachTraList.Sum(x => x.TongTien);
            lblTongTienTra.Text = $"Tổng Thu Giao Dịch Trả: {tong:N0} VNĐ";
        }

        private void BtnChotTra_Click(object? sender, EventArgs e)
        {
            if (_danhSachTraList.Count == 0)
            {
                MessageBox.Show("Chưa có cuốn băng nào trong danh sách trả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKH = _danhSachTraList[0].MaKhachHang;
            string maCHTra = cboCuaHangTra.SelectedValue?.ToString() ?? "";
            string maNVTra = cboNhanVienTra.SelectedValue?.ToString() ?? "";

            var (success, msg, maPT) = _muonTraService.ChotNhanTraBang(maKH, maCHTra, maNVTra, _danhSachTraList);
            if (success)
            {
                MessageBox.Show(msg, "Hoàn Tất Nhận Trả & Luân Chuyển Kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _danhSachTraList.Clear();
                RefreshDanhSachTraGrid();
            }
            else
            {
                MessageBox.Show(msg, "Lỗi Nhận Trả", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Size = new Size(240, 36),
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

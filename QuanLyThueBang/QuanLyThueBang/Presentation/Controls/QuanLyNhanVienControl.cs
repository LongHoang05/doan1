using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.NhanVien;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyNhanVienControl : UserControl
    {
        private readonly NhanVienService _nvService;
        private readonly CuaHangService _chService;
        private List<NhanVienDTO> _list = new List<NhanVienDTO>();

        private DataGridView dgvNhanVien = null!;
        private TextBox txtSearch = null!;

        public QuanLyNhanVienControl(NhanVienService nvService, CuaHangService chService)
        {
            _nvService = nvService;
            _chService = chService;
            InitializeComponent();
            this.Load += (s, e) => LoadData();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.75F);

            // Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 85, BackColor = Color.White, Padding = new Padding(25, 15, 25, 15) };
            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 220, BackColor = Color.Transparent };

            var btnAdd = new Button
            {
                Text = "+ Thêm Nhân Viên Mới",
                Size = new Size(185, 42),
                Location = new Point(15, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddNew();
            pnlRight.Controls.Add(btnAdd);

            var lblTitle = new Label { Text = "Quản Lý Nhân Sự & Tài Khoản", Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), Location = new Point(25, 15), AutoSize = true };
            var lblSub = new Label { Text = "Quản lý hồ sơ nhân viên, phân quyền vai trò (Quản lý/Nhân viên) và chi nhánh trực thuộc.", ForeColor = Color.FromArgb(108, 117, 125), Location = new Point(27, 47), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(pnlRight);

            // Filter
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(25, 15, 25, 15) };
            txtSearch = new TextBox { Location = new Point(25, 22), Size = new Size(360, 27), PlaceholderText = "Tìm mã nhân viên, tên hoặc tài khoản..." };
            txtSearch.TextChanged += (s, e) => LoadData();

            var btnRefresh = new Button { Text = "🔄 Làm mới", Location = new Point(400, 21), Size = new Size(110, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnRefresh);

            // Grid
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 0, 25, 25) };
            dgvNhanVien = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowTemplate = { Height = 44 }
            };
            dgvNhanVien.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Padding = new Padding(10, 8, 10, 8) };
            dgvNhanVien.DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.FromArgb(250, 235, 235), SelectionForeColor = Color.FromArgb(184, 123, 125), Padding = new Padding(10, 0, 10, 0) };
            dgvNhanVien.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(252, 253, 254) };

            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(NhanVienDTO.MaNhanVien), HeaderText = "Mã NV", Width = 110 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(NhanVienDTO.HoTen), HeaderText = "Họ và Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(NhanVienDTO.TenDangNhap), HeaderText = "Tài Khoản", Width = 135 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(NhanVienDTO.TenVaiTro), HeaderText = "Vai Trò", Width = 135 });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(NhanVienDTO.TenCuaHang), HeaderText = "Chi Nhánh", Width = 175 });

            dgvNhanVien.Columns.Add(new DataGridViewButtonColumn { Name = "colEdit", HeaderText = "Hành Động", Text = "✏️ Sửa", UseColumnTextForButtonValue = true, Width = 90, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvNhanVien.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "", Text = "🗑️ Xóa", UseColumnTextForButtonValue = true, Width = 90, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgvNhanVien.CellClick += Dgv_CellClick;

            pnlGrid.Controls.Add(dgvNhanVien);
            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);
        }

        private void LoadData()
        {
            _list = _nvService.GetAllNhanVien(txtSearch.Text);
            dgvNhanVien.DataSource = _list;
        }

        private void AddNew()
        {
            string nextMa = _nvService.GenerateNextMaNhanVien();
            var vaiTros = _nvService.GetAllVaiTro();
            var cuaHangs = _chService.GetAllCuaHang().Select(c => new QuanLyThueBang.Models.CuaHang { MaCuaHang = c.MaCuaHang, DiaChi = c.DiaChi }).ToList();

            using var dlg = new NhanVienEditDialogForm(vaiTros, cuaHangs, null, nextMa);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var res = _nvService.AddNhanVien(dlg.MaNhanVien, dlg.HoTen, dlg.CMND, dlg.SoDienThoai, dlg.TenDangNhap, dlg.MatKhau, dlg.MaVaiTro, dlg.MaCuaHang);
                MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (res.Success) LoadData();
            }
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dgvNhanVien.Columns[e.ColumnIndex].Name;
            var item = dgvNhanVien.Rows[e.RowIndex].DataBoundItem as NhanVienDTO;
            if (item == null) return;

            if (col == "colEdit")
            {
                var vaiTros = _nvService.GetAllVaiTro();
                var cuaHangs = _chService.GetAllCuaHang().Select(c => new QuanLyThueBang.Models.CuaHang { MaCuaHang = c.MaCuaHang, DiaChi = c.DiaChi }).ToList();

                using var dlg = new NhanVienEditDialogForm(vaiTros, cuaHangs, item);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var res = _nvService.UpdateNhanVien(item.MaNhanVien, dlg.HoTen, dlg.CMND, dlg.SoDienThoai, dlg.MaVaiTro, dlg.MaCuaHang);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                    if (res.Success) LoadData();
                }
            }
            else if (col == "colDelete")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa nhân viên '{item.HoTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = _nvService.DeleteNhanVien(item.MaNhanVien);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Cảnh báo", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (res.Success) LoadData();
                }
            }
        }
    }
}

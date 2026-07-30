using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.CuaHang;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyCuaHangControl : UserControl
    {
        private readonly CuaHangService _cuaHangService;
        private List<CuaHangDTO> _list = new List<CuaHangDTO>();

        private DataGridView dgvCuaHang = null!;
        private TextBox txtSearch = null!;

        public QuanLyCuaHangControl(CuaHangService cuaHangService)
        {
            _cuaHangService = cuaHangService;
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
            var pnlRight = new Panel { Dock = DockStyle.Right, Width = 260, BackColor = Color.Transparent };

            var btnAdd = new Button
            {
                Text = "+ Thêm Cửa Hàng Mới",
                Size = new Size(230, 42),
                Location = new Point(15, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 0, 0, 4),
                UseCompatibleTextRendering = true
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddNew();
            pnlRight.Controls.Add(btnAdd);

            var lblTitle = new Label { Text = "Quản Lý Hệ Thống Cửa Hàng", Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), Location = new Point(25, 15), AutoSize = true };
            var lblSub = new Label { Text = "Quản lý danh sách chi nhánh, địa chỉ, số điện thoại liên hệ và số lượng nhân sự trực thuộc.", ForeColor = Color.FromArgb(108, 117, 125), Location = new Point(27, 47), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(pnlRight);

            // Filter
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(25, 15, 25, 15) };
            txtSearch = new TextBox { Location = new Point(25, 22), Size = new Size(360, 27), PlaceholderText = "Tìm mã cửa hàng, địa chỉ hoặc số điện thoại..." };
            txtSearch.TextChanged += (s, e) => LoadData();

            var btnRefresh = new Button { Text = "🔄 Làm mới", Location = new Point(400, 21), Size = new Size(110, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnRefresh);

            // Grid
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 0, 25, 25) };
            dgvCuaHang = new DataGridView
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
                RowTemplate = { Height = 44 }
            };
            dgvCuaHang.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Padding = new Padding(8, 4, 8, 4) };
            dgvCuaHang.DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.FromArgb(250, 235, 235), SelectionForeColor = Color.FromArgb(184, 123, 125), Padding = new Padding(10, 0, 10, 0) };
            dgvCuaHang.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(252, 253, 254) };

            dgvCuaHang.AutoGenerateColumns = false;
            dgvCuaHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(CuaHangDTO.MaCuaHang), HeaderText = "Mã Chi Nhánh", Width = 150, MinimumWidth = 130 });
            dgvCuaHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(CuaHangDTO.DiaChi), HeaderText = "Địa Chỉ Chi Nhánh", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 200 });
            dgvCuaHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(CuaHangDTO.SoDienThoai), HeaderText = "Số Điện Thoại", Width = 160, MinimumWidth = 140 });
            dgvCuaHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(CuaHangDTO.SoLuongNhanVien), HeaderText = "Nhân Sự", Width = 130, MinimumWidth = 115, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvCuaHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(CuaHangDTO.SoLuongBanSao), HeaderText = "Kho Băng", Width = 130, MinimumWidth = 115, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgvCuaHang.Columns.Add(new DataGridViewButtonColumn { Name = "colEdit", HeaderText = "Hành Động", Text = "✏️ Sửa", UseColumnTextForButtonValue = true, Width = 155, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvCuaHang.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "", Text = "🗑️ Xóa", UseColumnTextForButtonValue = true, Width = 105, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgvCuaHang.CellClick += Dgv_CellClick;

            pnlGrid.Controls.Add(dgvCuaHang);
            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);
        }

        private void LoadData()
        {
            _list = _cuaHangService.GetAllCuaHang(txtSearch.Text);
            dgvCuaHang.DataSource = _list;
        }

        private void AddNew()
        {
            string nextMa = _cuaHangService.GenerateNextMaCuaHang();
            using var dlg = new CuaHangEditDialogForm(null, nextMa);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var res = _cuaHangService.AddCuaHang(dlg.MaCuaHang, dlg.DiaChi, dlg.SoDienThoai);
                MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (res.Success) LoadData();
            }
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dgvCuaHang.Columns[e.ColumnIndex].Name;
            var item = dgvCuaHang.Rows[e.RowIndex].DataBoundItem as CuaHangDTO;
            if (item == null) return;

            if (col == "colEdit")
            {
                using var dlg = new CuaHangEditDialogForm(item);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var res = _cuaHangService.UpdateCuaHang(item.MaCuaHang, dlg.DiaChi, dlg.SoDienThoai);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                    if (res.Success) LoadData();
                }
            }
            else if (col == "colDelete")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa chi nhánh '{item.MaCuaHang}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = _cuaHangService.DeleteCuaHang(item.MaCuaHang);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Cảnh báo", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (res.Success) LoadData();
                }
            }
        }
    }
}

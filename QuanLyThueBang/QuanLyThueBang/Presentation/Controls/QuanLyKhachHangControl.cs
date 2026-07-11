using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Presentation.Forms.KhachHang;

namespace QuanLyThueBang.Presentation.Controls
{
    public class QuanLyKhachHangControl : UserControl
    {
        private readonly KhachHangService _khService;
        private List<KhachHangDTO> _list = new List<KhachHangDTO>();

        private DataGridView dgvKhachHang = null!;
        private TextBox txtSearch = null!;

        public QuanLyKhachHangControl(KhachHangService khService)
        {
            _khService = khService;
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
                Text = "+ Đăng Ký Khách Mới",
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

            var lblTitle = new Label { Text = "Quản Lý Khách Hàng Thuê Băng", Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), Location = new Point(25, 15), AutoSize = true };
            var lblSub = new Label { Text = "Quản lý hồ sơ thành viên, thông tin liên lạc, CMND/CCCD và lịch sử lượt thuê.", ForeColor = Color.FromArgb(108, 117, 125), Location = new Point(27, 47), AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(pnlRight);

            // Filter
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(25, 15, 25, 15) };
            txtSearch = new TextBox { Location = new Point(25, 22), Size = new Size(360, 27), PlaceholderText = "Tìm theo tên, SĐT hoặc CMND..." };
            txtSearch.TextChanged += (s, e) => LoadData();

            var btnRefresh = new Button { Text = "🔄 Làm mới", Location = new Point(400, 21), Size = new Size(110, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.White };
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(206, 212, 218);
            btnRefresh.Click += (s, e) => { txtSearch.Clear(); LoadData(); };
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnRefresh);

            // Grid
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(25, 0, 25, 25) };
            dgvKhachHang = new DataGridView
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
            dgvKhachHang.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(243, 244, 246), Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Padding = new Padding(10, 8, 10, 8) };
            dgvKhachHang.DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.FromArgb(250, 235, 235), SelectionForeColor = Color.FromArgb(184, 123, 125), Padding = new Padding(10, 0, 10, 0) };
            dgvKhachHang.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(252, 253, 254) };

            dgvKhachHang.AutoGenerateColumns = false;
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KhachHangDTO.MaKhachHang), HeaderText = "Mã KH", Width = 110 });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KhachHangDTO.HoTen), HeaderText = "Họ và Tên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KhachHangDTO.SoDienThoai), HeaderText = "Số Điện Thoại", Width = 140 });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KhachHangDTO.CMND), HeaderText = "CMND / CCCD", Width = 140 });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(KhachHangDTO.SoLuotMuon), HeaderText = "Lượt Thuê", Width = 105, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgvKhachHang.Columns.Add(new DataGridViewButtonColumn { Name = "colEdit", HeaderText = "Hành Động", Text = "✏️ Sửa", UseColumnTextForButtonValue = true, Width = 90, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(13, 110, 253), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvKhachHang.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "", Text = "🗑️ Xóa", UseColumnTextForButtonValue = true, Width = 90, FlatStyle = FlatStyle.Flat, DefaultCellStyle = { ForeColor = Color.FromArgb(220, 53, 69), BackColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgvKhachHang.CellClick += Dgv_CellClick;

            pnlGrid.Controls.Add(dgvKhachHang);
            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlHeader);
        }

        private void LoadData()
        {
            _list = _khService.GetAllKhachHang(txtSearch.Text);
            dgvKhachHang.DataSource = _list;
        }

        private void AddNew()
        {
            string nextMa = _khService.GenerateNextMaKhachHang();
            using var dlg = new KhachHangEditDialogForm(null, nextMa);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var res = _khService.AddKhachHang(dlg.MaKhachHang, dlg.HoTen, dlg.CMND, dlg.SoDienThoai, dlg.DiaChi);
                MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (res.Success) LoadData();
            }
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dgvKhachHang.Columns[e.ColumnIndex].Name;
            var item = dgvKhachHang.Rows[e.RowIndex].DataBoundItem as KhachHangDTO;
            if (item == null) return;

            if (col == "colEdit")
            {
                using var dlg = new KhachHangEditDialogForm(item);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var res = _khService.UpdateKhachHang(item.MaKhachHang, dlg.HoTen, dlg.CMND, dlg.SoDienThoai, dlg.DiaChi);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Lỗi", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                    if (res.Success) LoadData();
                }
            }
            else if (col == "colDelete")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa khách hàng '{item.HoTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var res = _khService.DeleteKhachHang(item.MaKhachHang);
                    MessageBox.Show(res.Message, res.Success ? "Thành công" : "Cảnh báo", MessageBoxButtons.OK, res.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    if (res.Success) LoadData();
                }
            }
        }
    }
}

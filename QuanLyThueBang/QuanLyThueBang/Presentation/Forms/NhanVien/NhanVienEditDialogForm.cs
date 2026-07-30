using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.Presentation.Forms.NhanVien
{
    public class NhanVienEditDialogForm : Form
    {
        public string MaNhanVien { get; private set; } = string.Empty;
        public string HoTen { get; private set; } = string.Empty;
        public string CMND { get; private set; } = string.Empty;
        public string SoDienThoai { get; private set; } = string.Empty;
        public string TenDangNhap { get; private set; } = string.Empty;
        public string MatKhau { get; private set; } = string.Empty;
        public int MaVaiTro { get; private set; }
        public string? MaCuaHang { get; private set; }

        private TextBox txtMaNV = null!;
        private TextBox txtHoTen = null!;
        private TextBox txtCMND = null!;
        private TextBox txtSDT = null!;
        private TextBox txtUser = null!;
        private TextBox txtPass = null!;
        private ComboBox cboVaiTro = null!;
        private ComboBox cboCuaHang = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public NhanVienEditDialogForm(List<VaiTro> vaiTros, List<QuanLyThueBang.Models.CuaHang> cuaHangs, NhanVienDTO? existing = null, string? suggestedMaNV = "NV001")
        {
            InitializeComponent();

            if (!AppSession.IsAdmin)
            {
                vaiTros = vaiTros.Where(v => v.TenVaiTro != null && !v.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            cboVaiTro.DataSource = vaiTros;
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";

            if (AppSession.IsAdmin)
            {
                var listCH = new List<QuanLyThueBang.Models.CuaHang> { new QuanLyThueBang.Models.CuaHang { MaCuaHang = "", DiaChi = "--- Quản lý toàn chuỗi ---" } };
                listCH.AddRange(cuaHangs);
                cboCuaHang.DataSource = listCH;
            }
            else
            {
                var filteredCH = cuaHangs;
                if (!string.IsNullOrEmpty(AppSession.CurrentMaCuaHang))
                {
                    filteredCH = cuaHangs.Where(c => c.MaCuaHang == AppSession.CurrentMaCuaHang).ToList();
                }
                cboCuaHang.DataSource = filteredCH;
                cboCuaHang.Enabled = false;
            }
            cboCuaHang.DisplayMember = "DiaChi";
            cboCuaHang.ValueMember = "MaCuaHang";

            if (existing != null)
            {
                this.Text = "Cập Nhật Hồ Sơ Nhân Viên - " + existing.HoTen;
                txtMaNV.Text = existing.MaNhanVien;
                txtMaNV.ReadOnly = true;
                txtMaNV.BackColor = Color.FromArgb(240, 242, 245);
                txtHoTen.Text = existing.HoTen;
                txtCMND.Text = existing.CMND;
                txtSDT.Text = existing.SoDienThoai;
                txtUser.Text = existing.TenDangNhap;
                txtUser.ReadOnly = true;
                txtUser.BackColor = Color.FromArgb(240, 242, 245);
                txtPass.Text = "********";
                txtPass.ReadOnly = true;
                cboVaiTro.SelectedValue = existing.MaVaiTro;
                cboCuaHang.SelectedValue = existing.MaCuaHang;
            }
            else
            {
                this.Text = "Thêm Nhân Viên Mới";
                txtMaNV.Text = suggestedMaNV ?? "NV001";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(520, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.75F);

            var lblTitle = new Label { Text = "Hồ Sơ Nhân Viên & Tài Khoản", Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), Location = new Point(25, 20), AutoSize = true };

            var lblMa = new Label { Text = "Mã nhân viên (*):", Location = new Point(25, 65), AutoSize = true };
            txtMaNV = new TextBox { Location = new Point(28, 90), Size = new Size(215, 27) };

            var lblVaiTro = new Label { Text = "Vai trò chức vụ:", Location = new Point(263, 65), AutoSize = true };
            cboVaiTro = new ComboBox { Location = new Point(263, 90), Size = new Size(215, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblTen = new Label { Text = "Họ và tên nhân viên (*):", Location = new Point(25, 130), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(28, 155), Size = new Size(450, 27) };

            var lblSDT = new Label { Text = "Số điện thoại:", Location = new Point(25, 195), AutoSize = true };
            txtSDT = new TextBox { Location = new Point(28, 220), Size = new Size(215, 27) };

            var lblCMND = new Label { Text = "Số CCCD / CMND:", Location = new Point(263, 195), AutoSize = true };
            txtCMND = new TextBox { Location = new Point(263, 220), Size = new Size(215, 27) };

            var lblCH = new Label { Text = "Chi nhánh làm việc:", Location = new Point(25, 260), AutoSize = true };
            cboCuaHang = new ComboBox { Location = new Point(28, 285), Size = new Size(450, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblUser = new Label { Text = "Tên đăng nhập (*):", Location = new Point(25, 325), AutoSize = true };
            txtUser = new TextBox { Location = new Point(28, 350), Size = new Size(215, 27) };

            var lblPass = new Label { Text = "Mật khẩu (*):", Location = new Point(263, 325), AutoSize = true };
            txtPass = new TextBox { Location = new Point(263, 350), Size = new Size(215, 27), UseSystemPasswordChar = true };

            btnSave = new Button
            {
                Text = "💾 Lưu hồ sơ",
                Location = new Point(230, 455),
                Size = new Size(150, 38),
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Location = new Point(395, 455),
                Size = new Size(83, 38),
                BackColor = Color.FromArgb(240, 242, 245),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblMa);
            this.Controls.Add(txtMaNV);
            this.Controls.Add(lblVaiTro);
            this.Controls.Add(cboVaiTro);
            this.Controls.Add(lblTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSDT);
            this.Controls.Add(lblCMND);
            this.Controls.Add(txtCMND);
            this.Controls.Add(lblCH);
            this.Controls.Add(cboCuaHang);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUser);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPass);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Tên đăng nhập và Mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MaNhanVien = txtMaNV.Text.Trim();
            HoTen = txtHoTen.Text.Trim();
            SoDienThoai = txtSDT.Text.Trim();
            CMND = txtCMND.Text.Trim();
            TenDangNhap = txtUser.Text.Trim();
            MatKhau = txtPass.Text;
            MaVaiTro = Convert.ToInt32(cboVaiTro.SelectedValue);
            MaCuaHang = cboCuaHang.SelectedValue?.ToString();
            if (string.IsNullOrWhiteSpace(MaCuaHang)) MaCuaHang = null;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.KhachHang
{
    public class KhachHangEditDialogForm : Form
    {
        public string MaKhachHang { get; private set; } = string.Empty;
        public string HoTen { get; private set; } = string.Empty;
        public string CMND { get; private set; } = string.Empty;
        public string SoDienThoai { get; private set; } = string.Empty;
        public string DiaChi { get; private set; } = string.Empty;

        private TextBox txtMaKH = null!;
        private TextBox txtHoTen = null!;
        private TextBox txtCMND = null!;
        private TextBox txtSDT = null!;
        private TextBox txtDiaChi = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public KhachHangEditDialogForm(KhachHangDTO? existing = null, string? suggestedMaKH = "KH001")
        {
            InitializeComponent();
            if (existing != null)
            {
                this.Text = "Cập Nhật Hồ Sơ Khách Hàng - " + existing.HoTen;
                txtMaKH.Text = existing.MaKhachHang;
                txtMaKH.ReadOnly = true;
                txtMaKH.BackColor = Color.FromArgb(240, 242, 245);
                txtHoTen.Text = existing.HoTen;
                txtCMND.Text = existing.CMND;
                txtSDT.Text = existing.SoDienThoai;
                txtDiaChi.Text = existing.DiaChi;
            }
            else
            {
                this.Text = "Đăng Ký Khách Hàng Mới";
                txtMaKH.Text = suggestedMaKH ?? "KH001";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.75F);

            var lblTitle = new Label { Text = "Hồ Sơ Thành Viên / Khách Hàng", Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), Location = new Point(25, 20), AutoSize = true };

            var lblMa = new Label { Text = "Mã khách hàng (*):", Location = new Point(25, 65), AutoSize = true };
            txtMaKH = new TextBox { Location = new Point(28, 90), Size = new Size(430, 27) };

            var lblTen = new Label { Text = "Họ và tên (*):", Location = new Point(25, 130), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(28, 155), Size = new Size(430, 27) };

            var lblSDT = new Label { Text = "Số điện thoại (*):", Location = new Point(25, 195), AutoSize = true };
            txtSDT = new TextBox { Location = new Point(28, 220), Size = new Size(205, 27) };

            var lblCCCD = new Label { Text = "Số CCCD / CMND:", Location = new Point(253, 195), AutoSize = true };
            txtCMND = new TextBox { Location = new Point(253, 220), Size = new Size(205, 27) };

            var lblDC = new Label { Text = "Địa chỉ cư trú:", Location = new Point(25, 260), AutoSize = true };
            txtDiaChi = new TextBox { Location = new Point(28, 285), Size = new Size(430, 27) };

            btnSave = new Button
            {
                Text = "💾 Lưu hồ sơ",
                Location = new Point(220, 380),
                Size = new Size(135, 38),
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
                Location = new Point(365, 380),
                Size = new Size(95, 38),
                BackColor = Color.FromArgb(240, 242, 245),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblMa);
            this.Controls.Add(txtMaKH);
            this.Controls.Add(lblTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSDT);
            this.Controls.Add(lblCCCD);
            this.Controls.Add(txtCMND);
            this.Controls.Add(lblDC);
            this.Controls.Add(txtDiaChi);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Số điện thoại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MaKhachHang = txtMaKH.Text.Trim();
            HoTen = txtHoTen.Text.Trim();
            SoDienThoai = txtSDT.Text.Trim();
            CMND = txtCMND.Text.Trim();
            DiaChi = txtDiaChi.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

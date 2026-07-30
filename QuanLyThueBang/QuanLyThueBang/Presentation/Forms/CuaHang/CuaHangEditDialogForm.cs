using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.CuaHang
{
    public class CuaHangEditDialogForm : Form
    {
        public string MaCuaHang { get; private set; } = string.Empty;
        public string DiaChi { get; private set; } = string.Empty;
        public string SoDienThoai { get; private set; } = string.Empty;

        private TextBox txtMaCuaHang = null!;
        private TextBox txtDiaChi = null!;
        private TextBox txtSoDienThoai = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public CuaHangEditDialogForm(CuaHangDTO? existing = null, string? suggestedMaCH = "CH01")
        {
            InitializeComponent();
            if (existing != null)
            {
                this.Text = "Cập Nhật Cửa Hàng - " + existing.MaCuaHang;
                txtMaCuaHang.Text = existing.MaCuaHang;
                txtMaCuaHang.ReadOnly = true;
                txtMaCuaHang.BackColor = Color.FromArgb(240, 242, 245);
                txtDiaChi.Text = existing.DiaChi;
                txtSoDienThoai.Text = existing.SoDienThoai;
            }
            else
            {
                this.Text = "Thêm Cửa Hàng Mới";
                txtMaCuaHang.Text = suggestedMaCH ?? "CH01";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(480, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.75F);

            var lblTitle = new Label
            {
                Text = "Thông Tin Chi Nhánh Cửa Hàng",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                Location = new Point(25, 20),
                AutoSize = true
            };

            var lblMa = new Label { Text = "Mã cửa hàng (*):", Location = new Point(25, 65), AutoSize = true };
            txtMaCuaHang = new TextBox { Location = new Point(28, 90), Size = new Size(410, 27) };

            var lblDiaChi = new Label { Text = "Địa chỉ chi nhánh (*):", Location = new Point(25, 130), AutoSize = true };
            txtDiaChi = new TextBox { Location = new Point(28, 155), Size = new Size(410, 27) };

            var lblSDT = new Label { Text = "Số điện thoại liên hệ:", Location = new Point(25, 195), AutoSize = true };
            txtSoDienThoai = new TextBox { Location = new Point(28, 220), Size = new Size(410, 27) };

            btnSave = new Button
            {
                Text = "💾 Lưu thay đổi",
                Location = new Point(200, 265),
                Size = new Size(145, 38),
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
                Location = new Point(355, 265),
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
            this.Controls.Add(txtMaCuaHang);
            this.Controls.Add(lblDiaChi);
            this.Controls.Add(txtDiaChi);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSoDienThoai);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaCuaHang.Text) || string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã cửa hàng và Địa chỉ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MaCuaHang = txtMaCuaHang.Text.Trim();
            DiaChi = txtDiaChi.Text.Trim();
            SoDienThoai = txtSoDienThoai.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

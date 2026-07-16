using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using QuanLyThueBang.BLL;

namespace QuanLyThueBang.Presentation.Forms
{
    public class LoginForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;

        public LoginForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Rental Manager Enterprise - Đăng Nhập";
            this.Size = new Size(480, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            var pnlCard = new Panel
            {
                Size = new Size(420, 440),
                Location = new Point(22, 20),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "RENTAL MANAGER",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(184, 123, 125),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 25),
                Width = 380,
                Height = 35
            };

            var lblSub = new Label
            {
                Text = "Hệ Thống Quản Lý Thuê Băng Đĩa Enterprise",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 60),
                Width = 380
            };

            var lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(40, 105), AutoSize = true, Font = new Font("Segoe UI Semibold", 10F) };
            txtUsername = new TextBox { Location = new Point(40, 130), Width = 340, Font = new Font("Segoe UI", 11F), Text = "admin" };

            var lblPass = new Label { Text = "Mật khẩu:", Location = new Point(40, 180), AutoSize = true, Font = new Font("Segoe UI Semibold", 10F) };
            txtPassword = new TextBox { Location = new Point(40, 205), Width = 340, Font = new Font("Segoe UI", 11F), PasswordChar = '•', Text = "1" };

            var btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Location = new Point(40, 260),
                Size = new Size(340, 44),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnLogin_Click(s, e); } };

            var lblQuick = new Label
            {
                Text = "--- ĐĂNG NHẬP NHANH VAI TRÒ (KIỂM THỬ) ---",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(40, 325),
                Width = 340
            };

            var btnAdmin = new Button { Text = "👑 Admin", Location = new Point(40, 355), Size = new Size(105, 34), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(235, 240, 248), Cursor = Cursors.Hand };
            btnAdmin.FlatAppearance.BorderSize = 0;
            btnAdmin.Click += (s, e) => { txtUsername.Text = "admin"; txtPassword.Text = "1"; BtnLogin_Click(s, e); };

            var btnQuanLy = new Button { Text = "🏪 Quản Lý CH", Location = new Point(155, 355), Size = new Size(110, 34), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(235, 240, 248), Cursor = Cursors.Hand };
            btnQuanLy.FlatAppearance.BorderSize = 0;
            btnQuanLy.Click += (s, e) => { txtUsername.Text = "quanly"; txtPassword.Text = "1"; BtnLogin_Click(s, e); };

            var btnNhanVien = new Button { Text = "🧑‍💼 Nhân Viên", Location = new Point(275, 355), Size = new Size(105, 34), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(235, 240, 248), Cursor = Cursors.Hand };
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.Click += (s, e) => { txtUsername.Text = "nhanvien"; txtPassword.Text = "1"; BtnLogin_Click(s, e); };

            pnlCard.Controls.AddRange(new Control[] { lblTitle, lblSub, lblUser, txtUsername, lblPass, txtPassword, btnLogin, lblQuick, btnAdmin, btnQuanLy, btnNhanVien });
            this.Controls.Add(pnlCard);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            var authService = _serviceProvider.GetRequiredService<AuthService>();
            var (success, msg, user) = authService.Login(txtUsername.Text.Trim(), txtPassword.Text);

            if (success && user != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(msg, "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

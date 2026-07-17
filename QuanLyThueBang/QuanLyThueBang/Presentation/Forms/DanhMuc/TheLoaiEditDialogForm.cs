using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.DanhMuc
{
    /// <summary>
    /// Dialog thêm mới hoặc chỉnh sửa Thể Loại Phim (Danh Mục)
    /// </summary>
    public class TheLoaiEditDialogForm : Form
    {
        public string TenTheLoai { get; private set; } = string.Empty;
        private readonly bool _isEditMode;

        private TextBox txtTenTheLoai = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public TheLoaiEditDialogForm(TheLoaiDTO? existingTheLoai = null)
        {
            _isEditMode = existingTheLoai != null;
            InitializeComponent();

            if (existingTheLoai != null)
            {
                txtTenTheLoai.Text = existingTheLoai.TenTheLoai;
            }
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Cập Nhật Danh Mục Phim" : "Thêm Danh Mục Phim Mới";
            this.Size = new Size(460, 240);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            var lblTitle = new Label
            {
                Text = _isEditMode ? "Chỉnh sửa Tên Danh Mục" : "Nhập Thông Tin Danh Mục Mới",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 20),
                AutoSize = true
            };

            var lblTen = new Label
            {
                Text = "Tên thể loại / danh mục (*):",
                Location = new Point(25, 70),
                AutoSize = true,
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            txtTenTheLoai = new TextBox
            {
                Location = new Point(28, 95),
                Size = new Size(390, 27),
                Font = new Font("Segoe UI", 10F)
            };

            btnSave = new Button
            {
                Text = "💾 Lưu thay đổi",
                Location = new Point(180, 145),
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
                Location = new Point(335, 145),
                Size = new Size(95, 38),
                BackColor = Color.FromArgb(240, 242, 245),
                ForeColor = Color.FromArgb(73, 80, 87),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblTen);
            this.Controls.Add(txtTenTheLoai);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục/thể loại phim.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
                return;
            }

            TenTheLoai = txtTenTheLoai.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

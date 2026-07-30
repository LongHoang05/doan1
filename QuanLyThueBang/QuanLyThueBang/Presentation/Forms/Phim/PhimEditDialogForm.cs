using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.Phim
{
    /// <summary>
    /// Dialog Form Thêm mới / Cập nhật Phim (Giao diện hiện đại)
    /// </summary>
    public class PhimEditDialogForm : Form
    {
        public bool IsEditMode { get; private set; }
        public string MaPhim => txtMaPhim.Text.Trim();
        public string TuaDe => txtTuaDe.Text.Trim();
        public int MaTheLoai => cboTheLoai.SelectedItem is TheLoaiDTO tl ? tl.MaTheLoai : 0;
        public int? NamPhatHanh => (int)numNamPhatHanh.Value;
        public int? DoDaiPhut => (int)numDoDaiPhut.Value;

        private TextBox txtMaPhim = null!;
        private TextBox txtTuaDe = null!;
        private ComboBox cboTheLoai = null!;
        private NumericUpDown numNamPhatHanh = null!;
        private NumericUpDown numDoDaiPhut = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public PhimEditDialogForm(List<TheLoaiDTO> theLoais, PhimDTO? existingPhim = null, string? suggestedMaPhim = "PHIM001")
        {
            IsEditMode = existingPhim != null;
            InitializeComponent();

            // Populate Combobox Thể Loại
            cboTheLoai.DataSource = theLoais;
            cboTheLoai.DisplayMember = "TenTheLoai";
            cboTheLoai.ValueMember = "MaTheLoai";

            if (IsEditMode && existingPhim != null)
            {
                this.Text = "Cập Nhật Thông Tin Phim - " + existingPhim.TuaDe;
                txtMaPhim.Text = existingPhim.MaPhim;
                txtMaPhim.ReadOnly = true;
                txtMaPhim.BackColor = Color.FromArgb(240, 242, 245);
                txtTuaDe.Text = existingPhim.TuaDe;
                numNamPhatHanh.Value = existingPhim.NamPhatHanh ?? DateTime.Now.Year;
                numDoDaiPhut.Value = existingPhim.DoDaiPhut ?? 90;
                cboTheLoai.SelectedValue = existingPhim.MaTheLoai;
            }
            else
            {
                this.Text = "Thêm Phim Mới";
                txtMaPhim.Text = !string.IsNullOrWhiteSpace(suggestedMaPhim) ? suggestedMaPhim : "PHIM001";
                numNamPhatHanh.Value = DateTime.Now.Year;
                numDoDaiPhut.Value = 90;
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 430);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);

            // Title Label
            var lblTitle = new Label
            {
                Text = IsEditMode ? "CẬP NHẬT THÔNG TIN PHIM" : "THÊM TỰA PHIM MỚI",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Mã Phim
            var lblMaPhim = new Label { Text = "Mã Phim (*):", Location = new Point(25, 65), AutoSize = true };
            txtMaPhim = new TextBox { Location = new Point(25, 90), Size = new Size(435, 27) };
            this.Controls.Add(lblMaPhim);
            this.Controls.Add(txtMaPhim);

            // Tựa đề
            var lblTuaDe = new Label { Text = "Tựa Đề Phim (*):", Location = new Point(25, 130), AutoSize = true };
            txtTuaDe = new TextBox { Location = new Point(25, 155), Size = new Size(435, 27) };
            this.Controls.Add(lblTuaDe);
            this.Controls.Add(txtTuaDe);

            // Thể loại
            var lblTheLoai = new Label { Text = "Thể Loại (*):", Location = new Point(25, 195), AutoSize = true };
            cboTheLoai = new ComboBox
            {
                Location = new Point(25, 220),
                Size = new Size(435, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(lblTheLoai);
            this.Controls.Add(cboTheLoai);

            // Năm phát hành & Độ dài
            var lblNam = new Label { Text = "Năm Phát Hành:", Location = new Point(25, 260), AutoSize = true };
            numNamPhatHanh = new NumericUpDown
            {
                Location = new Point(25, 285),
                Size = new Size(205, 27),
                Minimum = 1900,
                Maximum = 2100
            };
            this.Controls.Add(lblNam);
            this.Controls.Add(numNamPhatHanh);

            var lblDoDai = new Label { Text = "Thời Lượng (phút):", Location = new Point(255, 260), AutoSize = true };
            numDoDaiPhut = new NumericUpDown
            {
                Location = new Point(255, 285),
                Size = new Size(205, 27),
                Minimum = 1,
                Maximum = 999
            };
            this.Controls.Add(lblDoDai);
            this.Controls.Add(numDoDaiPhut);

            // Nút action
            btnCancel = new Button
            {
                Text = "Hủy Bỏ",
                Location = new Point(240, 335),
                Size = new Size(95, 38),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(240, 242, 245),
                ForeColor = Color.FromArgb(70, 75, 80),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            btnSave = new Button
            {
                Text = "Lưu Thay Đổi",
                Location = new Point(345, 335),
                Size = new Size(125, 38),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(184, 123, 125), // Tone màu hồng/đất ấm áp theo giao diện mẫu
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnCancel);
            this.Controls.Add(btnSave);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaPhim.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã phim.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaPhim.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTuaDe.Text))
            {
                MessageBox.Show("Vui lòng nhập Tựa đề phim.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTuaDe.Focus();
                return;
            }
            if (cboTheLoai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Thể loại phim.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

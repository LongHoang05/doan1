using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.Presentation.Forms.BanSao
{
    public class BanSaoEditDialogForm : Form
    {
        private readonly BanSaoBangService _banSaoService;
        private readonly bool _isEditMode;

        public string MaPhim { get; private set; } = string.Empty;
        public string MaBanSao { get; private set; } = string.Empty;
        public string LoaiBang { get; private set; } = "PAL";
        public decimal DonGiaThue { get; private set; } = 15000m;
        public string TrangThai { get; private set; } = "Sẵn sàng";
        public string MaCuaHang { get; private set; } = "CH01";

        private ComboBox cboPhim = null!;
        private TextBox txtMaBanSao = null!;
        private ComboBox cboLoaiBang = null!;
        private NumericUpDown numDonGia = null!;
        private ComboBox cboTrangThai = null!;
        private ComboBox cboCuaHang = null!;

        private Button btnSave = null!;
        private Button btnCancel = null!;

        public BanSaoEditDialogForm(BanSaoBangService banSaoService, List<PhimDTO> phims, List<QuanLyThueBang.Models.CuaHang> cuaHangs, BanSaoBangViewDTO? existingBanSao = null)
        {
            _banSaoService = banSaoService;
            _isEditMode = existingBanSao != null;

            InitializeComponent();

            cboPhim.DataSource = phims;
            cboPhim.DisplayMember = "TuaDe";
            cboPhim.ValueMember = "MaPhim";

            cboCuaHang.DataSource = cuaHangs;
            cboCuaHang.DisplayMember = "DiaChi";
            cboCuaHang.ValueMember = "MaCuaHang";

            cboLoaiBang.Items.AddRange(new object[] { "PAL", "NTSC", "SECAM", "VHS", "Blu-ray" });
            cboTrangThai.Items.AddRange(new object[] { "Sẵn sàng", "Bảo trì", "Thất lạc" });

            if (_isEditMode && existingBanSao != null)
            {
                this.Text = "Cập Nhật Bản Sao Băng - " + existingBanSao.MaBanSao;
                cboPhim.SelectedValue = existingBanSao.MaPhim;
                cboPhim.Enabled = false;

                txtMaBanSao.Text = existingBanSao.MaBanSao;
                txtMaBanSao.ReadOnly = true;
                txtMaBanSao.BackColor = Color.FromArgb(240, 242, 245);

                cboLoaiBang.SelectedItem = existingBanSao.LoaiBang;
                numDonGia.Value = existingBanSao.DonGiaThue > 0 ? existingBanSao.DonGiaThue : 15000m;
                cboTrangThai.SelectedItem = existingBanSao.TrangThai;
                cboCuaHang.SelectedValue = existingBanSao.MaCuaHangHienTai;
            }
            else
            {
                this.Text = "Thêm Bản Sao Băng Mới";
                cboLoaiBang.SelectedIndex = 0;
                cboTrangThai.SelectedIndex = 0;
                numDonGia.Value = 15000m;

                cboPhim.SelectedIndexChanged += (s, e) => UpdateSuggestedMaBanSao();
                UpdateSuggestedMaBanSao();
            }
        }

        private void UpdateSuggestedMaBanSao()
        {
            if (_isEditMode) return;
            if (cboPhim.SelectedValue is string selectedMaPhim && !string.IsNullOrWhiteSpace(selectedMaPhim))
            {
                txtMaBanSao.Text = _banSaoService.GenerateNextMaBanSao(selectedMaPhim);
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(520, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);

            var lblTitle = new Label
            {
                Text = _isEditMode ? "Chỉnh Sửa Thông Tin Bản Sao" : "Nhập Bản Sao Băng Mới Vào Kho",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 20),
                AutoSize = true
            };

            var lblPhim = new Label { Text = "Tựa phim gốc (*):", Location = new Point(25, 65), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            cboPhim = new ComboBox { Location = new Point(28, 90), Size = new Size(450, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblMaBS = new Label { Text = "Mã bản sao/mã vạch (*):", Location = new Point(25, 130), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            txtMaBanSao = new TextBox { Location = new Point(28, 155), Size = new Size(215, 27) };

            var lblLoai = new Label { Text = "Loại băng:", Location = new Point(263, 130), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            cboLoaiBang = new ComboBox { Location = new Point(263, 155), Size = new Size(215, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblDonGia = new Label { Text = "Đơn giá thuê (VNĐ):", Location = new Point(25, 195), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            numDonGia = new NumericUpDown
            {
                Location = new Point(28, 220),
                Size = new Size(215, 27),
                Minimum = 1000,
                Maximum = 1000000,
                Increment = 1000,
                Value = 15000
            };

            var lblTrangThai = new Label { Text = "Trạng thái bản sao:", Location = new Point(263, 195), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            cboTrangThai = new ComboBox { Location = new Point(263, 220), Size = new Size(215, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            var lblCuaHang = new Label { Text = "Cửa hàng lưu kho:", Location = new Point(25, 260), AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87) };
            cboCuaHang = new ComboBox { Location = new Point(28, 285), Size = new Size(450, 27), DropDownStyle = ComboBoxStyle.DropDownList };

            btnSave = new Button
            {
                Text = "💾 Lưu bản sao",
                Location = new Point(230, 420),
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
                Location = new Point(395, 420),
                Size = new Size(83, 38),
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
            this.Controls.Add(lblPhim);
            this.Controls.Add(cboPhim);
            this.Controls.Add(lblMaBS);
            this.Controls.Add(txtMaBanSao);
            this.Controls.Add(lblLoai);
            this.Controls.Add(cboLoaiBang);
            this.Controls.Add(lblDonGia);
            this.Controls.Add(numDonGia);
            this.Controls.Add(lblTrangThai);
            this.Controls.Add(cboTrangThai);
            this.Controls.Add(lblCuaHang);
            this.Controls.Add(cboCuaHang);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaBanSao.Text))
            {
                MessageBox.Show("Vui lòng nhập mã bản sao băng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaBanSao.Focus();
                return;
            }

            MaPhim = cboPhim.SelectedValue?.ToString() ?? string.Empty;
            MaBanSao = txtMaBanSao.Text.Trim();
            LoaiBang = cboLoaiBang.SelectedItem?.ToString() ?? "PAL";
            DonGiaThue = numDonGia.Value;
            TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Sẵn sàng";
            MaCuaHang = cboCuaHang.SelectedValue?.ToString() ?? "CH01";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

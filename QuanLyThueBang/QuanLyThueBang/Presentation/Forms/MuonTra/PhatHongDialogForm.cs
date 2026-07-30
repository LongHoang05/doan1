using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.MuonTra
{
    /// <summary>
    /// Form Ghi Nhận Tình Trạng Vật Lý & Tiền Phạt / Bồi Thường Hỏng Mất Băng
    /// Giao diện gom chung 1 popup duy nhất, đẹp mắt và thuận tiện cho nhân viên quầy
    /// </summary>
    public class PhatHongDialogForm : Form
    {
        private ComboBox cboTinhTrang = null!;
        private TextBox txtGhiChu = null!;
        private NumericUpDown numTienPhat = null!;

        public string TinhTrangChon { get; private set; } = "Bình thường";
        public decimal TienPhatChon { get; private set; } = 0;

        public PhatHongDialogForm(ThongTinBangMuonChuaTraDTO item, bool isBaoHongMatDirect = false)
        {
            InitializeUI(item, isBaoHongMatDirect);
        }

        private void InitializeUI(ThongTinBangMuonChuaTraDTO item, bool isBaoHongMatDirect)
        {
            this.Text = "Ghi Nhận Phạt Vi Phạm & Sự Cố Hỏng / Mất Băng";
            this.Size = new Size(520, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 1. Header banner
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(20, 12, 20, 12)
            };
            var lblTitle = new Label
            {
                Text = isBaoHongMatDirect ? "⚠️ BÁO HỎNG / MẤT BĂNG & BỒI THƯỜNG" : "⚙️ GHI NHẬN TÌNH TRẠNG & PHẠT VI PHẠM",
                Font = new Font("Segoe UI Semibold", 13.5F, FontStyle.Bold),
                ForeColor = isBaoHongMatDirect ? Color.FromArgb(220, 53, 69) : Color.FromArgb(184, 123, 125),
                Location = new Point(20, 12),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = $"Cuốn băng: [{item.MaBanSao}] - {item.TuaDe}",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(22, 40),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // 2. Footer buttons
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 65,
                BackColor = Color.White,
                Padding = new Padding(20, 12, 20, 12)
            };
            var btnCancel = new Button
            {
                Text = "❌ Hủy Bỏ",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            var btnSave = new Button
            {
                Text = "💾 Xác Nhận Ghi Nhận",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(new Panel { Width = 12, Dock = DockStyle.Right });
            pnlFooter.Controls.Add(btnSave);

            // 3. Body Content
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25, 15, 25, 15)
            };

            int y = 15;
            // Card thông tin nhanh
            var pnlCard = new Panel
            {
                Location = new Point(25, y),
                Size = new Size(450, 65),
                BackColor = Color.FromArgb(233, 236, 239)
            };
            var lblKhach = new Label
            {
                Text = $"Khách mượn: {item.HoTenKhachHang}",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Location = new Point(12, 10),
                AutoSize = true
            };
            var lblTre = new Label
            {
                Text = item.SoNgayTreHan > 0 ? $"🚨 Trễ hạn: {item.SoNgayTreHan} ngày" : "✅ Trả đúng hạn",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = item.SoNgayTreHan > 0 ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69),
                Location = new Point(12, 34),
                AutoSize = true
            };
            pnlCard.Controls.Add(lblKhach);
            pnlCard.Controls.Add(lblTre);
            pnlBody.Controls.Add(pnlCard);

            y += 80;
            var lblCbo = new Label
            {
                Text = "Tình trạng băng khi trả / Phân loại sự cố:",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Location = new Point(25, y),
                AutoSize = true
            };
            y += 26;
            cboTinhTrang = new ComboBox
            {
                Location = new Point(25, y),
                Size = new Size(450, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTinhTrang.Items.AddRange(new object[]
            {
                "Bình thường",
                "Trễ hạn (Phạt trả muộn)",
                "Hỏng vỏ / Xước nhẹ (Phạt nhẹ)",
                "Hư hỏng nặng (Bồi thường băng)",
                "Mất băng / Thất lạc (Bồi thường toàn bộ)"
            });

            // Set giá trị mặc định theo tình trạng item hoặc isBaoHongMatDirect
            if (isBaoHongMatDirect)
            {
                cboTinhTrang.SelectedIndex = 3; // Hư hỏng nặng
            }
            else
            {
                int matchIdx = cboTinhTrang.FindStringExact(item.TinhTrangKhiTra);
                cboTinhTrang.SelectedIndex = matchIdx >= 0 ? matchIdx : (item.SoNgayTreHan > 0 ? 1 : 0);
            }

            cboTinhTrang.SelectedIndexChanged += CboTinhTrang_SelectedIndexChanged;
            pnlBody.Controls.Add(lblCbo);
            pnlBody.Controls.Add(cboTinhTrang);

            y += 45;
            var lblNum = new Label
            {
                Text = "Số tiền Phạt / Bồi thường (VNĐ):",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Location = new Point(25, y),
                AutoSize = true
            };
            y += 26;
            numTienPhat = new NumericUpDown
            {
                Location = new Point(25, y),
                Size = new Size(450, 30),
                Minimum = 0,
                Maximum = 10000000,
                Increment = 5000,
                ThousandsSeparator = true,
                DecimalPlaces = 0,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Value = item.TienPhat > 0 ? item.TienPhat : SuggestFee(cboTinhTrang.Text, item.SoNgayTreHan)
            };
            pnlBody.Controls.Add(lblNum);
            pnlBody.Controls.Add(numTienPhat);

            y += 45;
            var lblNote = new Label
            {
                Text = "Ghi chú bổ sung (tuỳ chọn):",
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(25, y),
                AutoSize = true
            };
            y += 24;
            txtGhiChu = new TextBox
            {
                Location = new Point(25, y),
                Size = new Size(450, 30),
                PlaceholderText = "Ví dụ: Khách làm rơi vỡ vỏ hộp, hỏng dây băng..."
            };
            pnlBody.Controls.Add(lblNote);
            pnlBody.Controls.Add(txtGhiChu);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private decimal SuggestFee(string tinhTrang, int soNgayTre)
        {
            if (tinhTrang.Contains("Hư hỏng", StringComparison.OrdinalIgnoreCase)) return 150000;
            if (tinhTrang.Contains("Mất", StringComparison.OrdinalIgnoreCase) || tinhTrang.Contains("Thất lạc", StringComparison.OrdinalIgnoreCase)) return 200000;
            if (tinhTrang.Contains("Hỏng vỏ", StringComparison.OrdinalIgnoreCase)) return 30000;
            if (tinhTrang.Contains("Trễ hạn", StringComparison.OrdinalIgnoreCase) && soNgayTre > 0) return soNgayTre * 10000;
            return 0;
        }

        private void CboTinhTrang_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (numTienPhat != null && cboTinhTrang != null)
            {
                numTienPhat.Value = SuggestFee(cboTinhTrang.Text, 0);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string baseTinhTrang = cboTinhTrang.Text;
            if (!string.IsNullOrWhiteSpace(txtGhiChu.Text))
            {
                baseTinhTrang += $" ({txtGhiChu.Text.Trim()})";
            }

            TinhTrangChon = baseTinhTrang;
            TienPhatChon = numTienPhat.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Presentation.Forms.HoaDon
{
    /// <summary>
    /// Form Biên Lai / Hóa Đơn Trả Băng - Hỗ trợ in ấn và xem trước hóa đơn thanh toán
    /// </summary>
    public class HoaDonDialogForm : Form
    {
        private readonly string _maPhieuTra;
        private readonly string _tenKhachHang;
        private readonly string _tenCuaHang;
        private readonly string _tenNhanVien;
        private readonly List<ThongTinBangMuonChuaTraDTO> _danhSachTra;
        private RichTextBox rtbReceipt = null!;

        public HoaDonDialogForm(string maPhieuTra, string tenKhachHang, string tenCuaHang, string tenNhanVien, List<ThongTinBangMuonChuaTraDTO> danhSachTra)
        {
            _maPhieuTra = maPhieuTra;
            _tenKhachHang = tenKhachHang;
            _tenCuaHang = tenCuaHang;
            _tenNhanVien = tenNhanVien;
            _danhSachTra = danhSachTra ?? new List<ThongTinBangMuonChuaTraDTO>();

            InitializeUI();
            GenerateReceiptContent();
        }

        private void InitializeUI()
        {
            this.Text = $"Hóa Đơn / Biên Lai Trả Băng - {_maPhieuTra}";
            this.Size = new Size(540, 680);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Header banner
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.White };
            var lblTitle = new Label
            {
                Text = "BIÊN LAI THANH TOÁN TRẢ BĂNG",
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(184, 123, 125),
                Location = new Point(20, 18),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);

            // Footer buttons
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.White, Padding = new Padding(15, 12, 15, 12) };

            var btnClose = new Button
            {
                Text = "Đóng",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            var btnCopy = new Button
            {
                Text = "📋 Copy Text",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += (s, e) =>
            {
                Clipboard.SetText(rtbReceipt.Text);
                MessageBox.Show("Đã sao chép nội dung biên lai vào Clipboard!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            var btnPrint = new Button
            {
                Text = "🖨️ In Biên Lai",
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 5, 14, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Controls.Add(new Panel { Width = 10, Dock = DockStyle.Right });
            pnlFooter.Controls.Add(btnCopy);
            pnlFooter.Controls.Add(new Panel { Width = 10, Dock = DockStyle.Right });
            pnlFooter.Controls.Add(btnPrint);

            // Center area - Receipt text
            var pnlCenter = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 15, 20, 15) };
            rtbReceipt = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlCenter.Controls.Add(rtbReceipt);

            this.Controls.Add(pnlCenter);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private void GenerateReceiptContent()
        {
            var sb = new StringBuilder();
            sb.AppendLine("==================================================");
            sb.AppendLine("         RENTAL MANAGER ENTERPRISE SYSTEM         ");
            sb.AppendLine("           BIÊN LAI NHẬN TRẢ BĂNG ĐĨA             ");
            sb.AppendLine("==================================================");
            sb.AppendLine($" Mã Phiếu Trả : {_maPhieuTra}");
            sb.AppendLine($" Ngày Giao Dịch: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($" Chi Nhánh    : {_tenCuaHang}");
            sb.AppendLine($" Nhân Viên    : {_tenNhanVien}");
            sb.AppendLine($" Khách Hàng   : {_tenKhachHang}");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine(string.Format("{0,-10} | {1,-18} | {2,14}", "MÃ BĂNG", "TỰA PHIM", "THÀNH TIỀN"));
            sb.AppendLine("--------------------------------------------------");

            decimal tongTien = 0;
            foreach (var item in _danhSachTra)
            {
                string tua = item.TuaDe.Length > 18 ? item.TuaDe.Substring(0, 15) + "..." : item.TuaDe;
                sb.AppendLine(string.Format("{0,-10} | {1,-18} | {2,14:N0}", item.MaBanSao, tua, item.TongTien));
                if (item.SoNgayTreHan > 0)
                {
                    sb.AppendLine($"   * Trễ hạn: {item.SoNgayTreHan} ngày");
                }
                if (item.TienPhat > 0)
                {
                    sb.AppendLine($"   * Phạt / Bồi thường ({item.TinhTrangKhiTra}): +{item.TienPhat:N0} đ");
                }
                tongTien += item.TongTien;
            }

            sb.AppendLine("==================================================");
            sb.AppendLine(string.Format("{0,-30} : {1,14:N0} đ", "TỔNG CỘNG THANH TOÁN", tongTien));
            sb.AppendLine("==================================================");
            sb.AppendLine();
            sb.AppendLine("     Cảm ơn Quý Khách hàng và Hẹn gặp lại!        ");
            sb.AppendLine("   Mọi khiếu nại vui lòng liên hệ quầy thu ngân.  ");
            sb.AppendLine("==================================================");

            rtbReceipt.Text = sb.ToString();
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                var pd = new PrintDocument();
                pd.PrintPage += (s, ev) =>
                {
                    var font = new Font("Consolas", 10F, FontStyle.Regular);
                    ev.Graphics?.DrawString(rtbReceipt.Text, font, Brushes.Black, new PointF(40, 40));
                };

                using var ppd = new PrintPreviewDialog { Document = pd, Width = 600, Height = 700 };
                ppd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể in hóa đơn: {ex.Message}", "Lỗi In Ấn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

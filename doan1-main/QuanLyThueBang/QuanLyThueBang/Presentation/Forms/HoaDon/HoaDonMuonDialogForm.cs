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
    /// Form Biên Lai / Hóa Đơn Thuê Băng (Thanh toán trả trước - Mô hình A)
    /// </summary>
    public class HoaDonMuonDialogForm : Form
    {
        private readonly string _maPhieuMuon;
        private readonly string _tenKhachHang;
        private readonly string _tenCuaHang;
        private readonly string _tenNhanVien;
        private readonly DateTime _ngayHenTra;
        private readonly List<ChiTietGioMuonDTO> _danhSachMuon;
        private RichTextBox rtbReceipt = null!;

        public HoaDonMuonDialogForm(string maPhieuMuon, string tenKhachHang, string tenCuaHang, string tenNhanVien, DateTime ngayHenTra, List<ChiTietGioMuonDTO> danhSachMuon)
        {
            _maPhieuMuon = maPhieuMuon;
            _tenKhachHang = tenKhachHang;
            _tenCuaHang = tenCuaHang;
            _tenNhanVien = tenNhanVien;
            _ngayHenTra = ngayHenTra;
            _danhSachMuon = danhSachMuon ?? new List<ChiTietGioMuonDTO>();

            InitializeUI();
            GenerateReceiptContent();
        }

        private void InitializeUI()
        {
            this.Text = $"Hóa Đơn Thuê Băng (Đã Thanh Toán) - {_maPhieuMuon}";
            this.Size = new Size(540, 680);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 249, 250);

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.White, Padding = new Padding(15, 10, 15, 10) };
            var btnClose = new Button { Text = "Đóng", Dock = DockStyle.Right, Width = 100, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            var btnPrint = new Button { Text = "🖨️ In Hóa Đơn", Dock = DockStyle.Right, Width = 140, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(13, 110, 253), ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;

            var btnCopy = new Button { Text = "📋 Copy Text", Dock = DockStyle.Left, Width = 130, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(25, 135, 84), ForeColor = Color.White, Cursor = Cursors.Hand };
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.Click += BtnCopy_Click;

            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(new Panel { Width = 10, Dock = DockStyle.Right });
            pnlBottom.Controls.Add(btnPrint);
            pnlBottom.Controls.Add(btnCopy);

            rtbReceipt = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F, FontStyle.Regular),
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
            pnlBody.Controls.Add(rtbReceipt);

            this.Controls.Add(pnlBody);
            this.Controls.Add(pnlBottom);
        }

        private void GenerateReceiptContent()
        {
            int soNgayThue = Math.Max(1, (_ngayHenTra.Date - DateTime.Today).Days);
            var sb = new StringBuilder();
            sb.AppendLine("=================================================");
            sb.AppendLine("          HỆ THỐNG QUẢN LÝ THUÊ BĂNG VIDEO       ");
            sb.AppendLine("       HÓA ĐƠN THUÊ BĂNG & THANH TOÁN NGAY       ");
            sb.AppendLine("=================================================");
            sb.AppendLine($"Mã Phiếu Mượn: {_maPhieuMuon}");
            sb.AppendLine($"Ngày Lập Phiếu: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Ngày Hẹn Trả:  {_ngayHenTra:dd/MM/yyyy} ({soNgayThue} ngày)");
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine($"Chi Nhánh:  {_tenCuaHang}");
            sb.AppendLine($"Nhân Viên:  {_tenNhanVien}");
            sb.AppendLine($"Khách Hàng:  {_tenKhachHang}");
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine(string.Format("{0,-4} {1,-12} {2,-18} {3,10}", "STT", "Mã Băng", "Tựa Đề", "Thành Tiền"));
            sb.AppendLine("-------------------------------------------------");

            decimal total = 0;
            for (int i = 0; i < _danhSachMuon.Count; i++)
            {
                var item = _danhSachMuon[i];
                string title = item.TuaDe.Length > 18 ? item.TuaDe.Substring(0, 15) + "..." : item.TuaDe;
                decimal thanhTien = item.DonGiaThue * soNgayThue;
                sb.AppendLine(string.Format("{0,-4} {1,-12} {2,-18} {3,10:N0}",
                    i + 1, item.MaBanSao, title, thanhTien));
                total += thanhTien;
            }

            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine($"THỜI GIAN THUÊ:                {soNgayThue,10} ngày");
            sb.AppendLine($"TỔNG SỐ LƯỢNG BĂNG THUÊ:       {_danhSachMuon.Count,10} cuốn");
            sb.AppendLine($"TỔNG TIỀN THANH TOÁN (ĐÃ THU): {total,10:N0} VNĐ");
            sb.AppendLine("=================================================");
            sb.AppendLine("  Quý khách vui lòng bảo quản băng nguyên vẹn.");
            sb.AppendLine("  Khi trả băng muộn hoặc hỏng/mất sẽ tính phí");
            sb.AppendLine("          theo quy định của cửa hàng.");
            sb.AppendLine("           XIN CẢM ƠN QUÝ KHÁCH!");
            sb.AppendLine("=================================================");

            rtbReceipt.Text = sb.ToString();
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                var pd = new PrintDocument();
                pd.PrintPage += (s, ev) =>
                {
                    using var font = new Font("Consolas", 10);
                    float yPos = 20;
                    float leftMargin = ev.MarginBounds.Left;
                    foreach (var line in rtbReceipt.Lines)
                    {
                        ev.Graphics?.DrawString(line, font, Brushes.Black, leftMargin, yPos);
                        yPos += font.GetHeight(ev.Graphics!);
                    }
                };

                using var ppd = new PrintPreviewDialog { Document = pd, Width = 600, Height = 700 };
                ppd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCopy_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rtbReceipt.Text))
            {
                Clipboard.SetText(rtbReceipt.Text);
                MessageBox.Show("Đã sao chép nội dung Hóa đơn Thuê Băng vào bộ nhớ tạm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

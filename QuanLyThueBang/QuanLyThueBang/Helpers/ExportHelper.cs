using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuanLyThueBang.Domain.DTOs;

namespace QuanLyThueBang.Helpers
{
    public static class ExportHelper
    {
        /// <summary>
        /// Xuất Phiếu Mượn ra file Hóa Đơn HTML chuyên nghiệp và mở ngay để in/lưu PDF
        /// </summary>
        public static void ExportPhieuMuonInvoice(PhieuMuonViewDTO phieu, List<ChiTietGioMuonDTO> chiTietList)
        {
            try
            {
                string filePath = Path.Combine(Path.GetTempPath(), $"PhieuMuon_{phieu.MaPhieuMuon}_{DateTime.Now:yyyyMMddHHmmss}.html");

                var sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang='vi'><head><meta charset='UTF-8'><title>Hóa Đơn Thuê Băng Đĩa</title>");
                sb.AppendLine("<style>");
                sb.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 30px; color: #333; }");
                sb.AppendLine(".invoice-box { max-width: 800px; margin: auto; padding: 30px; border: 1px solid #eee; box-shadow: 0 0 10px rgba(0, 0, 0, 0.15); }");
                sb.AppendLine(".header { display: flex; justify-content: space-between; border-bottom: 2px solid #B87B7D; padding-bottom: 15px; margin-bottom: 20px; }");
                sb.AppendLine(".title { font-size: 24px; font-weight: bold; color: #B87B7D; }");
                sb.AppendLine(".info-table { width: 100%; margin-bottom: 20px; }");
                sb.AppendLine(".info-table td { padding: 6px 0; }");
                sb.AppendLine(".items-table { width: 100%; border-collapse: collapse; margin-top: 15px; }");
                sb.AppendLine(".items-table th { background-color: #f8f9fa; border-bottom: 2px solid #dee2e6; padding: 10px; text-align: left; }");
                sb.AppendLine(".items-table td { border-bottom: 1px solid #eee; padding: 10px; }");
                sb.AppendLine(".total-row { font-size: 18px; font-weight: bold; color: #B87B7D; text-align: right; margin-top: 20px; }");
                sb.AppendLine(".signatures { display: flex; justify-content: space-between; margin-top: 50px; text-align: center; }");
                sb.AppendLine(".sig-block { width: 200px; }");
                sb.AppendLine("</style></head><body>");

                sb.AppendLine("<div class='invoice-box'>");
                sb.AppendLine("  <div class='header'>");
                sb.AppendLine("    <div>");
                sb.AppendLine("      <div class='title'>RENTAL MANAGER ENTERPRISE</div>");
                sb.AppendLine("      <div style='font-size: 13px; color: #666;'>Hệ thống Quản lý Thuê Băng Đĩa Chuyên Nghiệp</div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("    <div style='text-align: right;'>");
                sb.AppendLine($"      <div style='font-size: 20px; font-weight: bold;'>PHIẾU MƯỢN BĂNG</div>");
                sb.AppendLine($"      <div>Mã phiếu: <strong>{phieu.MaPhieuMuon}</strong></div>");
                sb.AppendLine($"      <div>Ngày lập: {phieu.NgayMuon:dd/MM/yyyy HH:mm}</div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("  </div>");

                sb.AppendLine("  <table class='info-table'>");
                sb.AppendLine($"    <tr><td><strong>Khách hàng:</strong> {phieu.TenKhachHang}</td><td><strong>Chi nhánh:</strong> {phieu.TenCuaHang}</td></tr>");
                sb.AppendLine($"    <tr><td><strong>Ngày hẹn trả:</strong> {phieu.NgayDuKienTra:dd/MM/yyyy}</td><td><strong>Nhân viên lập:</strong> {phieu.TenNhanVien}</td></tr>");
                sb.AppendLine("  </table>");

                sb.AppendLine("  <table class='items-table'>");
                sb.AppendLine("    <thead><tr><th>STT</th><th>Mã Bản Sao</th><th>Tựa Đề Phim</th><th>Thể Loại</th><th style='text-align: right;'>Đơn Giá Thuê</th></tr></thead>");
                sb.AppendLine("    <tbody>");

                int stt = 1;
                decimal total = 0;
                foreach (var item in chiTietList)
                {
                    sb.AppendLine($"      <tr><td>{stt++}</td><td><strong>{item.MaBanSao}</strong></td><td>{item.TuaDe}</td><td>{item.TenTheLoai}</td><td style='text-align: right;'>{item.DonGiaThue:N0} VNĐ</td></tr>");
                    total += item.DonGiaThue;
                }

                sb.AppendLine("    </tbody>");
                sb.AppendLine("  </table>");

                sb.AppendLine($"  <div class='total-row'>TỔNG CỘNG: {total:N0} VNĐ</div>");

                sb.AppendLine("  <div class='signatures'>");
                sb.AppendLine("    <div class='sig-block'><strong>KHÁCH HÀNG</strong><br><small>(Ký và ghi rõ họ tên)</small><br><br><br><br></div>");
                sb.AppendLine("    <div class='sig-block'><strong>NHÂN VIÊN QUẦY</strong><br><small>(Ký và đóng dấu)</small><br><br><br><br></div>");
                sb.AppendLine("  </div>");
                sb.AppendLine("</div>");

                sb.AppendLine("<script>window.onload = function() { window.print(); }</script>");
                sb.AppendLine("</body></html>");

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xuất dữ liệu từ DataGridView ra file Excel / CSV (hỗ trợ tiếng Việt UTF-8 BOM)
        /// </summary>
        public static void ExportDataGridViewToExcel(DataGridView dgv, string defaultFileName)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel CSV File (*.csv)|*.csv",
                FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd}.csv",
                Title = "Xuất dữ liệu ra file Excel"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    var headers = new List<string>();

                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Visible && !(col is DataGridViewButtonColumn))
                        {
                            headers.Add($"\"{col.HeaderText}\"");
                        }
                    }
                    sb.AppendLine(string.Join(",", headers));

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        var values = new List<string>();
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.Visible && !(col is DataGridViewButtonColumn))
                            {
                                var val = row.Cells[col.Index].Value?.ToString() ?? "";
                                val = val.Replace("\"", "\"\"");
                                values.Add($"\"{val}\"");
                            }
                        }
                        sb.AppendLine(string.Join(",", values));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show("Xuất file Excel thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

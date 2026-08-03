using System;

namespace QuanLyThueBang.Domain.DTOs
{
    public class KhachHangDTO
    {
        public string MaKhachHang { get; set; } = string.Empty;
        public string CMND { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public int SoLuotMuon { get; set; }
    }
}

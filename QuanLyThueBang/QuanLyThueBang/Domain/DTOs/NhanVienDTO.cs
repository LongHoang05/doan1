namespace QuanLyThueBang.Domain.DTOs
{
    public class NhanVienDTO
    {
        public string MaNhanVien { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string CMND { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; } = string.Empty;
        public string MaCuaHang { get; set; } = string.Empty;
        public string TenCuaHang { get; set; } = string.Empty;
    }
}

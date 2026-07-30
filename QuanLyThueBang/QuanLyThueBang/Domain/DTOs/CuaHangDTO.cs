namespace QuanLyThueBang.Domain.DTOs
{
    public class CuaHangDTO
    {
        public string MaCuaHang { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public int SoLuongNhanVien { get; set; }
        public int SoLuongBanSao { get; set; }
    }
}

namespace QuanLyThueBang.Domain.DTOs
{
    /// <summary>
    /// DTO truyền tải thông tin Thể Loại phim
    /// </summary>
    public class TheLoaiDTO
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public int SoLuongPhim { get; set; }
    }
}

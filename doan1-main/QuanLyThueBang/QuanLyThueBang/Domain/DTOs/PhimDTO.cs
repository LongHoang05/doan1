namespace QuanLyThueBang.Domain.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách Phim kèm tên thể loại và số lượng bản sao hiện có
    /// </summary>
    public class PhimDTO
    {
        public string MaPhim { get; set; } = string.Empty;
        public string TuaDe { get; set; } = string.Empty;
        public int? NamPhatHanh { get; set; }
        public int? DoDaiPhut { get; set; }
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; } = string.Empty;
        public int SoLuongBanSao { get; set; }
    }
}

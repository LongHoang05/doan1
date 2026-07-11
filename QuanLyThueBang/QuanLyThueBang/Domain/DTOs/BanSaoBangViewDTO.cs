namespace QuanLyThueBang.Domain.DTOs
{
    /// <summary>
    /// DTO hiển thị danh sách bản sao băng trên DataGridView
    /// </summary>
    public class BanSaoBangViewDTO
    {
        public string MaBanSao { get; set; } = string.Empty;
        public string MaPhim { get; set; } = string.Empty;
        public string TuaDePhim { get; set; } = string.Empty;
        public string MaCuaHangHienTai { get; set; } = string.Empty;
        public string LoaiBang { get; set; } = string.Empty;
        public decimal DonGiaThue { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}

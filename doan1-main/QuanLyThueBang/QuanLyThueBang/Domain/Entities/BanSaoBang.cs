using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Bản Sao Băng - Từng cuốn băng cụ thể có mã vạch/RFID duy nhất
    /// </summary>
    [Table("BanSaoBang")]
    public class BanSaoBang
    {
        [Key]
        [StringLength(50)]
        public string MaBanSao { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaPhim { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string MaCuaHangHienTai { get; set; } = string.Empty;

        [Required]
        public int SoThuTuBanSao { get; set; }

        [StringLength(10)]
        public string LoaiBang { get; set; } = "PAL";

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DonGiaThue { get; set; }

        public DateTime? NgayHetHan { get; set; }

        /// <summary>
        /// Sẵn sàng, Đang cho mượn, Bảo trì, Thất lạc
        /// </summary>
        [StringLength(50)]
        public string TrangThai { get; set; } = "Sẵn sàng";

        // Navigation properties
        [ForeignKey(nameof(MaPhim))]
        public virtual Phim? Phim { get; set; }

        [ForeignKey(nameof(MaCuaHangHienTai))]
        public virtual CuaHang? CuaHangHienTai { get; set; }

        public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
    }
}

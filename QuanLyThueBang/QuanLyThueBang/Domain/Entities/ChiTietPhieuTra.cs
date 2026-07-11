using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Chi Tiết Phiếu Trả - Ghi nhận tình trạng từng bản sao băng khi trả, tiền thuê và tiền phạt
    /// </summary>
    [Table("ChiTietPhieuTra")]
    public class ChiTietPhieuTra
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string MaPhieuTra { get; set; } = string.Empty;

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaBanSao { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaPhieuMuon { get; set; } = string.Empty;

        /// <summary>
        /// Bình thường, Hỏng vỏ, Đứt băng...
        /// </summary>
        [StringLength(100)]
        public string TinhTrangBangKhiTra { get; set; } = "Bình thường";

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TienThue { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TienPhat { get; set; } = 0;

        // Navigation properties
        [ForeignKey(nameof(MaPhieuTra))]
        public virtual PhieuTra? PhieuTra { get; set; }

        /// <summary>
        /// Khóa ngoại kép tới ChiTietPhieuMuon (MaPhieuMuon, MaBanSao)
        /// </summary>
        public virtual ChiTietPhieuMuon? ChiTietPhieuMuon { get; set; }
    }
}

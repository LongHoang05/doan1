using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Phiếu Trả - Ghi nhận giao dịch trả băng của khách hàng (hỗ trợ trả cross-store)
    /// </summary>
    [Table("PhieuTra")]
    public class PhieuTra
    {
        [Key]
        [StringLength(20)]
        public string MaPhieuTra { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaKhachHang { get; set; } = string.Empty;

        /// <summary>
        /// Nơi khách trả (Có thể khác nơi mượn)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string MaCuaHangNhanTra { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaNhanVienNhanTra { get; set; } = string.Empty;

        public DateTime NgayTra { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTienThu { get; set; } = 0;

        // Navigation properties
        [ForeignKey(nameof(MaKhachHang))]
        public virtual KhachHang? KhachHang { get; set; }

        [ForeignKey(nameof(MaCuaHangNhanTra))]
        public virtual CuaHang? CuaHangNhanTra { get; set; }

        [ForeignKey(nameof(MaNhanVienNhanTra))]
        public virtual NhanVien? NhanVienNhanTra { get; set; }

        public virtual ICollection<ChiTietPhieuTra> ChiTietPhieuTras { get; set; } = new List<ChiTietPhieuTra>();
    }
}

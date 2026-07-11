using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Phiếu Mượn - Phiếu giao dịch cho khách thuê băng
    /// </summary>
    [Table("PhieuMuon")]
    public class PhieuMuon
    {
        [Key]
        [StringLength(20)]
        public string MaPhieuMuon { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaKhachHang { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string MaCuaHangMuon { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MaNhanVienChoMuon { get; set; } = string.Empty;

        public DateTime NgayMuon { get; set; } = DateTime.Now;

        [Required]
        public DateTime NgayDuKienTra { get; set; }

        // Navigation properties
        [ForeignKey(nameof(MaKhachHang))]
        public virtual KhachHang? KhachHang { get; set; }

        [ForeignKey(nameof(MaCuaHangMuon))]
        public virtual CuaHang? CuaHangMuon { get; set; }

        [ForeignKey(nameof(MaNhanVienChoMuon))]
        public virtual NhanVien? NhanVienChoMuon { get; set; }

        public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
    }
}

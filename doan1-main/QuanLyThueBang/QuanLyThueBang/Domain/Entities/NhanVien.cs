using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Nhân Viên - Lưu thông tin nhân viên, tài khoản đăng nhập và vai trò
    /// </summary>
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        [StringLength(20)]
        public string MaNhanVien { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        public string CMND { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(255)]
        public string? DiaChi { get; set; }

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        public int MaVaiTro { get; set; }

        /// <summary>
        /// NULL nếu là Admin cấp cao quản lý toàn chuỗi
        /// </summary>
        [StringLength(10)]
        public string? MaCuaHang { get; set; }

        // Navigation properties
        [ForeignKey(nameof(MaVaiTro))]
        public virtual VaiTro? VaiTro { get; set; }

        [ForeignKey(nameof(MaCuaHang))]
        public virtual CuaHang? CuaHang { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
        public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();
    }
}

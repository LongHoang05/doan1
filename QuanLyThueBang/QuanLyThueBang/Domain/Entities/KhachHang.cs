using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Khách Hàng - Thông tin khách hàng đăng ký thuê băng
    /// </summary>
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        [StringLength(20)]
        public string MaKhachHang { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        public string CMND { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(255)]
        public string? DiaChi { get; set; }

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
        public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();
    }
}

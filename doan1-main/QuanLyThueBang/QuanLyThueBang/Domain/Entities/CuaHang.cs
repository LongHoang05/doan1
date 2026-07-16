using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Cửa Hàng - Các chi nhánh trong hệ thống
    /// </summary>
    [Table("CuaHang")]
    public class CuaHang
    {
        [Key]
        [StringLength(10)]
        public string MaCuaHang { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string DiaChi { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
        public virtual ICollection<BanSaoBang> BanSaoBangs { get; set; } = new List<BanSaoBang>();
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
        public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();
    }
}

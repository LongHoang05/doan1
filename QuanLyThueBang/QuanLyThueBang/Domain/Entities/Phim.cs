using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Phim - Thông tin tựa phim gốc
    /// </summary>
    [Table("Phim")]
    public class Phim
    {
        [Key]
        [StringLength(20)]
        public string MaPhim { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string TuaDe { get; set; } = string.Empty;

        public int? NamPhatHanh { get; set; }

        public int? DoDaiPhut { get; set; }

        [Required]
        public int MaTheLoai { get; set; }

        // Navigation properties
        [ForeignKey(nameof(MaTheLoai))]
        public virtual TheLoai? TheLoai { get; set; }

        public virtual ICollection<BanSaoBang> BanSaoBangs { get; set; } = new List<BanSaoBang>();
    }
}

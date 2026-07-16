using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Thể Loại - Thể loại phim (Hành động, Tình cảm, Hoạt hình...)
    /// </summary>
    [Table("TheLoai")]
    public class TheLoai
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTheLoai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenTheLoai { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
    }
}

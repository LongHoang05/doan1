using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Vai Trò (Admin_CapCao, QuanLy_ChiNhanh, NhanVien_Quay)
    /// </summary>
    [Table("VaiTro")]
    public class VaiTro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaVaiTro { get; set; }

        [Required]
        [StringLength(50)]
        public string TenVaiTro { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
    }
}

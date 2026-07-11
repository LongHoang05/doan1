using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThueBang.Models
{
    /// <summary>
    /// Bảng Chi Tiết Phiếu Mượn - Danh sách băng mượn trong phiếu mượn
    /// </summary>
    [Table("ChiTietPhieuMuon")]
    public class ChiTietPhieuMuon
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string MaPhieuMuon { get; set; } = string.Empty;

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaBanSao { get; set; } = string.Empty;

        /// <summary>
        /// 0 (false): Chưa trả, 1 (true): Đã trả
        /// </summary>
        public bool TrangThaiTra { get; set; } = false;

        // Navigation properties
        [ForeignKey(nameof(MaPhieuMuon))]
        public virtual PhieuMuon? PhieuMuon { get; set; }

        [ForeignKey(nameof(MaBanSao))]
        public virtual BanSaoBang? BanSaoBang { get; set; }

        public virtual ICollection<ChiTietPhieuTra> ChiTietPhieuTras { get; set; } = new List<ChiTietPhieuTra>();
    }
}

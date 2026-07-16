using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.DAL
{
    /// <summary>
    /// Entity Framework Core 8 DbContext - Tầng Data Access Layer (DAL)
    /// Đóng vai trò là Unit of Work & Repository trung tâm
    /// </summary>
    public class QuanLyThueBangContext : DbContext
    {
        public QuanLyThueBangContext()
        {
        }

        public QuanLyThueBangContext(DbContextOptions<QuanLyThueBangContext> options)
            : base(options)
        {
        }

        // Nhóm 1: Quản lý hệ thống & nhân sự
        public virtual DbSet<VaiTro> VaiTros { get; set; }
        public virtual DbSet<CuaHang> CuaHangs { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }

        // Nhóm 2: Danh mục phim & kho băng bản sao
        public virtual DbSet<TheLoai> TheLoais { get; set; }
        public virtual DbSet<Phim> Phims { get; set; }
        public virtual DbSet<BanSaoBang> BanSaoBangs { get; set; }

        // Nhóm 3: Nghiệp vụ Mượn - Trả Băng (Cross-Store)
        public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }
        public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
        public virtual DbSet<PhieuTra> PhieuTras { get; set; }
        public virtual DbSet<ChiTietPhieuTra> ChiTietPhieuTras { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AppConfig.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 0. Ánh xạ chính xác tên bảng số ít trong SQL Server
            modelBuilder.Entity<VaiTro>().ToTable("VaiTro");
            modelBuilder.Entity<CuaHang>().ToTable("CuaHang");
            modelBuilder.Entity<NhanVien>().ToTable("NhanVien");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang");
            modelBuilder.Entity<TheLoai>().ToTable("TheLoai");
            modelBuilder.Entity<Phim>().ToTable("Phim");
            modelBuilder.Entity<BanSaoBang>().ToTable("BanSaoBang");
            modelBuilder.Entity<PhieuMuon>().ToTable("PhieuMuon");
            modelBuilder.Entity<ChiTietPhieuMuon>().ToTable("ChiTietPhieuMuon");
            modelBuilder.Entity<PhieuTra>().ToTable("PhieuTra");
            modelBuilder.Entity<ChiTietPhieuTra>().ToTable("ChiTietPhieuTra");

            // 1. Cấu hình khóa chính kép cho ChiTietPhieuMuon
            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasKey(ct => new { ct.MaPhieuMuon, ct.MaBanSao });

            // 2. Cấu hình khóa chính kép cho ChiTietPhieuTra
            modelBuilder.Entity<ChiTietPhieuTra>()
                .HasKey(ct => new { ct.MaPhieuTra, ct.MaBanSao });

            // 3. Cấu hình quan hệ kép ChiTietPhieuTra -> ChiTietPhieuMuon (MaPhieuMuon, MaBanSao)
            modelBuilder.Entity<ChiTietPhieuTra>()
                .HasOne(ctpt => ctpt.ChiTietPhieuMuon)
                .WithMany(ctpm => ctpm.ChiTietPhieuTras)
                .HasForeignKey(ctpt => new { ctpt.MaPhieuMuon, ctpt.MaBanSao })
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Ngăn chặn lỗi Cascade Delete trùng lặp trong SQL Server cho các bảng liên quan CuaHang & NhanVien
            modelBuilder.Entity<NhanVien>()
                .HasOne(nv => nv.VaiTro)
                .WithMany(vt => vt.NhanViens)
                .HasForeignKey(nv => nv.MaVaiTro)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NhanVien>()
                .HasOne(nv => nv.CuaHang)
                .WithMany(ch => ch.NhanViens)
                .HasForeignKey(nv => nv.MaCuaHang)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BanSaoBang>()
                .HasOne(bs => bs.CuaHangHienTai)
                .WithMany(ch => ch.BanSaoBangs)
                .HasForeignKey(bs => bs.MaCuaHangHienTai)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.CuaHangMuon)
                .WithMany(ch => ch.PhieuMuons)
                .HasForeignKey(pm => pm.MaCuaHangMuon)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuMuon>()
                .HasOne(pm => pm.NhanVienChoMuon)
                .WithMany(nv => nv.PhieuMuons)
                .HasForeignKey(pm => pm.MaNhanVienChoMuon)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(pt => pt.CuaHangNhanTra)
                .WithMany(ch => ch.PhieuTras)
                .HasForeignKey(pt => pt.MaCuaHangNhanTra)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhieuTra>()
                .HasOne(pt => pt.NhanVienNhanTra)
                .WithMany(nv => nv.PhieuTras)
                .HasForeignKey(pt => pt.MaNhanVienNhanTra)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.PhieuMuon)
                .WithMany(pm => pm.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaPhieuMuon)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietPhieuMuon>()
                .HasOne(ct => ct.BanSaoBang)
                .WithMany(bs => bs.ChiTietPhieuMuons)
                .HasForeignKey(ct => ct.MaBanSao)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

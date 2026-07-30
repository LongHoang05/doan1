using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.Helpers
{
    public static class DbSeeder
    {
        public static void SeedDefaultUsers(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();

                // Đảm bảo tạo CSDL nếu chưa có
                context.Database.EnsureCreated();

                // 1. Kiểm tra & tạo VaiTro
                var roleAdmin = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Admin" || v.TenVaiTro == "Admin Cấp Cao" || v.TenVaiTro == "Admin_CapCao");
                if (roleAdmin == null)
                {
                    roleAdmin = new VaiTro { TenVaiTro = "Admin" };
                    context.VaiTros.Add(roleAdmin);
                    context.SaveChanges();
                }

                var roleQuanLy = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Quản lý" || v.TenVaiTro == "Quản lý chi nhánh" || v.TenVaiTro == "QuanLy_ChiNhanh");
                if (roleQuanLy == null)
                {
                    roleQuanLy = new VaiTro { TenVaiTro = "Quản lý" };
                    context.VaiTros.Add(roleQuanLy);
                    context.SaveChanges();
                }

                var roleNhanVien = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Nhân viên" || v.TenVaiTro == "Nhân viên quầy" || v.TenVaiTro == "NhanVien_Quay");
                if (roleNhanVien == null)
                {
                    roleNhanVien = new VaiTro { TenVaiTro = "Nhân viên" };
                    context.VaiTros.Add(roleNhanVien);
                    context.SaveChanges();
                }

                // 2. Kiểm tra & tạo Cửa Hàng mẫu (nếu chưa có)
                var cuaHang = context.CuaHangs.FirstOrDefault();
                if (cuaHang == null)
                {
                    cuaHang = new CuaHang { MaCuaHang = "CH01", DiaChi = "Chi Nhánh Trung Tâm TP.HCM", SoDienThoai = "0909123456" };
                    context.CuaHangs.Add(cuaHang);
                    context.SaveChanges();
                }

                // 3. Tài khoản admin (Mật khẩu "1")
                var admin = context.NhanViens.FirstOrDefault(n => n.TenDangNhap == "admin");
                if (admin == null)
                {
                    // Tránh trùng MaNhanVien với các dữ liệu cũ (ví dụ NV001 đã có)
                    string maNVAdmin = context.NhanViens.Any(n => n.MaNhanVien == "ADM01") ? "ADM02" : "ADM01";
                    admin = new NhanVien
                    {
                        MaNhanVien = maNVAdmin,
                        CMND = "079090000001",
                        HoTen = "Quản Trị Viên Hệ Thống",
                        SoDienThoai = "0901111111",
                        TenDangNhap = "admin",
                        MatKhau = "1",
                        MaVaiTro = roleAdmin.MaVaiTro,
                        MaCuaHang = null
                    };
                    context.NhanViens.Add(admin);
                }
                else
                {
                    admin.MatKhau = "1";
                    admin.MaVaiTro = roleAdmin.MaVaiTro;
                }

                // 4. Tài khoản quản lý (Mật khẩu "1")
                var quanLy = context.NhanViens.FirstOrDefault(n => n.TenDangNhap == "quanly");
                if (quanLy == null)
                {
                    string maNVQL = context.NhanViens.Any(n => n.MaNhanVien == "QL01") ? "QL02" : "QL01";
                    quanLy = new NhanVien
                    {
                        MaNhanVien = maNVQL,
                        CMND = "079090000002",
                        HoTen = "Quản Lý Chi Nhánh 1",
                        SoDienThoai = "0902222222",
                        TenDangNhap = "quanly",
                        MatKhau = "1",
                        MaVaiTro = roleQuanLy.MaVaiTro,
                        MaCuaHang = cuaHang?.MaCuaHang
                    };
                    context.NhanViens.Add(quanLy);
                }
                else
                {
                    quanLy.MatKhau = "1";
                    quanLy.MaVaiTro = roleQuanLy.MaVaiTro;
                }

                // 5. Tài khoản nhân viên quầy (Mật khẩu "1")
                var nhanVien = context.NhanViens.FirstOrDefault(n => n.TenDangNhap == "nhanvien");
                if (nhanVien == null)
                {
                    string maNVQ = context.NhanViens.Any(n => n.MaNhanVien == "NVQ01") ? "NVQ02" : "NVQ01";
                    nhanVien = new NhanVien
                    {
                        MaNhanVien = maNVQ,
                        CMND = "079090000003",
                        HoTen = "Nhân Viên Quầy Giao Dịch",
                        SoDienThoai = "0903333333",
                        TenDangNhap = "nhanvien",
                        MatKhau = "1",
                        MaVaiTro = roleNhanVien.MaVaiTro,
                        MaCuaHang = cuaHang?.MaCuaHang
                    };
                    context.NhanViens.Add(nhanVien);
                }
                else
                {
                    nhanVien.MatKhau = "1";
                    nhanVien.MaVaiTro = roleNhanVien.MaVaiTro;
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi seed dữ liệu mặc định: " + ex.Message);
            }
        }

        public static void SeedSampleData(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<QuanLyThueBangContext>();

                // Nếu đã có phim thì bỏ qua
                if (context.Phims.Any()) return;

                // 1. Thể Loại
                var tlHanhDong = new TheLoai { TenTheLoai = "Hành Động" };
                var tlTinhCam = new TheLoai { TenTheLoai = "Tình Cảm" };
                var tlHaiHuoc = new TheLoai { TenTheLoai = "Hài Hước" };
                context.TheLoais.AddRange(tlHanhDong, tlTinhCam, tlHaiHuoc);
                context.SaveChanges();

                // 2. Phim
                var p1 = new Phim { MaPhim = "P001", TuaDe = "Avenger: Endgame", NamPhatHanh = 2019, DoDaiPhut = 180, MaTheLoai = tlHanhDong.MaTheLoai };
                var p2 = new Phim { MaPhim = "P002", TuaDe = "Titanic", NamPhatHanh = 1997, DoDaiPhut = 195, MaTheLoai = tlTinhCam.MaTheLoai };
                var p3 = new Phim { MaPhim = "P003", TuaDe = "Mr. Bean's Holiday", NamPhatHanh = 2007, DoDaiPhut = 90, MaTheLoai = tlHaiHuoc.MaTheLoai };
                var p4 = new Phim { MaPhim = "P004", TuaDe = "John Wick 4", NamPhatHanh = 2023, DoDaiPhut = 169, MaTheLoai = tlHanhDong.MaTheLoai };
                var p5 = new Phim { MaPhim = "P005", TuaDe = "Fast & Furious 10", NamPhatHanh = 2023, DoDaiPhut = 141, MaTheLoai = tlHanhDong.MaTheLoai };
                context.Phims.AddRange(p1, p2, p3, p4, p5);
                context.SaveChanges();

                // 3. Khách hàng
                var k1 = new KhachHang { MaKhachHang = "KH001", HoTen = "Nguyễn Văn An", CMND = "001122334455", SoDienThoai = "0901234567", NgayDangKy = DateTime.Now.AddMonths(-3) };
                var k2 = new KhachHang { MaKhachHang = "KH002", HoTen = "Trần Thị Bích", CMND = "001122334466", SoDienThoai = "0901234568", NgayDangKy = DateTime.Now.AddMonths(-2) };
                var k3 = new KhachHang { MaKhachHang = "KH003", HoTen = "Lê Văn Cường", CMND = "001122334477", SoDienThoai = "0901234569", NgayDangKy = DateTime.Now.AddMonths(-1) };
                context.KhachHangs.AddRange(k1, k2, k3);
                context.SaveChanges();

                // 4. Bản Sao Băng (Cần CuaHang đã có sẵn do SeedDefaultUsers)
                var cuaHang = context.CuaHangs.First();
                var bs1 = new BanSaoBang { MaBanSao = "BS001", MaPhim = p1.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 1, DonGiaThue = 15000, TrangThai = "Sẵn sàng" };
                var bs2 = new BanSaoBang { MaBanSao = "BS002", MaPhim = p1.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 2, DonGiaThue = 15000, TrangThai = "Đang cho mượn" };
                var bs3 = new BanSaoBang { MaBanSao = "BS003", MaPhim = p2.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 1, DonGiaThue = 10000, TrangThai = "Sẵn sàng" };
                var bs4 = new BanSaoBang { MaBanSao = "BS004", MaPhim = p3.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 1, DonGiaThue = 12000, TrangThai = "Đang cho mượn" };
                var bs5 = new BanSaoBang { MaBanSao = "BS005", MaPhim = p4.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 1, DonGiaThue = 20000, TrangThai = "Hư hỏng" };
                var bs6 = new BanSaoBang { MaBanSao = "BS006", MaPhim = p5.MaPhim, MaCuaHangHienTai = cuaHang.MaCuaHang, SoThuTuBanSao = 1, DonGiaThue = 18000, TrangThai = "Sẵn sàng" };
                context.BanSaoBangs.AddRange(bs1, bs2, bs3, bs4, bs5, bs6);
                context.SaveChanges();

                // 5. Phiếu mượn (Cần NhanVien đã có sẵn do SeedDefaultUsers)
                var nv = context.NhanViens.First();
                var pm1 = new PhieuMuon { MaPhieuMuon = "PM001", MaKhachHang = k1.MaKhachHang, MaCuaHangMuon = cuaHang.MaCuaHang, MaNhanVienChoMuon = nv.MaNhanVien, NgayMuon = DateTime.Now.AddDays(-10), NgayDuKienTra = DateTime.Now.AddDays(-3) };
                var pm2 = new PhieuMuon { MaPhieuMuon = "PM002", MaKhachHang = k2.MaKhachHang, MaCuaHangMuon = cuaHang.MaCuaHang, MaNhanVienChoMuon = nv.MaNhanVien, NgayMuon = DateTime.Now.AddMonths(-1), NgayDuKienTra = DateTime.Now.AddMonths(-1).AddDays(5) };
                var pm3 = new PhieuMuon { MaPhieuMuon = "PM003", MaKhachHang = k3.MaKhachHang, MaCuaHangMuon = cuaHang.MaCuaHang, MaNhanVienChoMuon = nv.MaNhanVien, NgayMuon = DateTime.Now.AddMonths(-2), NgayDuKienTra = DateTime.Now.AddMonths(-2).AddDays(5) };
                var pm4 = new PhieuMuon { MaPhieuMuon = "PM004", MaKhachHang = k1.MaKhachHang, MaCuaHangMuon = cuaHang.MaCuaHang, MaNhanVienChoMuon = nv.MaNhanVien, NgayMuon = DateTime.Now.AddMonths(-3), NgayDuKienTra = DateTime.Now.AddMonths(-3).AddDays(5) };
                context.PhieuMuons.AddRange(pm1, pm2, pm3, pm4);
                context.SaveChanges();

                // 6. Chi tiết phiếu mượn
                context.ChiTietPhieuMuons.AddRange(
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM001", MaBanSao = "BS002", TrangThaiTra = false }, // Quá hạn
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM001", MaBanSao = "BS004", TrangThaiTra = false }, // Quá hạn
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM002", MaBanSao = "BS001", TrangThaiTra = true },
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM003", MaBanSao = "BS003", TrangThaiTra = true },
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM004", MaBanSao = "BS001", TrangThaiTra = true },
                    new ChiTietPhieuMuon { MaPhieuMuon = "PM004", MaBanSao = "BS006", TrangThaiTra = true }
                );
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi seed sample data: " + ex.Message);
            }
        }
    }
}

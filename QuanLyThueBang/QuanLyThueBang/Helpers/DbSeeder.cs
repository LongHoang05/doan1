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
                var roleAdmin = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Admin" || v.TenVaiTro == "Admin Cấp Cao");
                if (roleAdmin == null)
                {
                    roleAdmin = new VaiTro { TenVaiTro = "Admin" };
                    context.VaiTros.Add(roleAdmin);
                    context.SaveChanges();
                }

                var roleQuanLy = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Quản lý" || v.TenVaiTro == "Quản lý chi nhánh");
                if (roleQuanLy == null)
                {
                    roleQuanLy = new VaiTro { TenVaiTro = "Quản lý" };
                    context.VaiTros.Add(roleQuanLy);
                    context.SaveChanges();
                }

                var roleNhanVien = context.VaiTros.FirstOrDefault(v => v.TenVaiTro == "Nhân viên" || v.TenVaiTro == "Nhân viên quầy");
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
                    admin = new NhanVien
                    {
                        MaNhanVien = "NV001",
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
                    quanLy = new NhanVien
                    {
                        MaNhanVien = "NV002",
                        CMND = "079090000002",
                        HoTen = "Quản Lý Chi Nhánh 1",
                        SoDienThoai = "0902222222",
                        TenDangNhap = "quanly",
                        MatKhau = "1",
                        MaVaiTro = roleQuanLy.MaVaiTro,
                        MaCuaHang = cuaHang.MaCuaHang
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
                    nhanVien = new NhanVien
                    {
                        MaNhanVien = "NV003",
                        CMND = "079090000003",
                        HoTen = "Nhân Viên Quầy Giao Dịch",
                        SoDienThoai = "0903333333",
                        TenDangNhap = "nhanvien",
                        MatKhau = "1",
                        MaVaiTro = roleNhanVien.MaVaiTro,
                        MaCuaHang = cuaHang.MaCuaHang
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    public class NhanVienService
    {
        private readonly QuanLyThueBangContext _context;

        public NhanVienService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        public List<VaiTro> GetAllVaiTro()
        {
            var list = _context.VaiTros.OrderBy(v => v.MaVaiTro).ToList();
            if (list.Count == 0)
            {
                var vt1 = new VaiTro { TenVaiTro = "Quản lý" };
                var vt2 = new VaiTro { TenVaiTro = "Nhân viên quầy" };
                _context.VaiTros.AddRange(vt1, vt2);
                _context.SaveChanges();
                list.AddRange(new[] { vt1, vt2 });
            }
            if (!AppSession.IsAdmin)
            {
                list = list.Where(v => v.TenVaiTro != null && !v.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return list;
        }

        public List<NhanVienDTO> GetAllNhanVien(string? keyword = null)
        {
            var query = _context.NhanViens
                .Include(n => n.VaiTro)
                .Include(n => n.CuaHang)
                .AsNoTracking()
                .AsQueryable();

            if (!AppSession.IsAdmin)
            {
                // Quản lý cửa hàng không được phép xem các tài khoản Admin
                query = query.Where(n => n.VaiTro == null || !n.VaiTro.TenVaiTro.Contains("Admin"));

                // Quản lý thuộc chi nhánh nào thì chỉ xem nhân viên của chi nhánh đó
                if (!string.IsNullOrEmpty(AppSession.CurrentMaCuaHang))
                {
                    query = query.Where(n => n.MaCuaHang == AppSession.CurrentMaCuaHang);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(n => n.MaNhanVien.ToLower().Contains(kw) ||
                                         n.HoTen.ToLower().Contains(kw) ||
                                         n.TenDangNhap.ToLower().Contains(kw) ||
                                         (n.SoDienThoai != null && n.SoDienThoai.Contains(kw)));
            }

            return query
                .OrderBy(n => n.MaNhanVien)
                .Select(n => new NhanVienDTO
                {
                    MaNhanVien = n.MaNhanVien,
                    HoTen = n.HoTen,
                    CMND = n.CMND,
                    SoDienThoai = n.SoDienThoai ?? string.Empty,
                    TenDangNhap = n.TenDangNhap,
                    MaVaiTro = n.MaVaiTro,
                    TenVaiTro = n.VaiTro != null ? n.VaiTro.TenVaiTro : "N/A",
                    MaCuaHang = n.MaCuaHang ?? string.Empty,
                    TenCuaHang = n.CuaHang != null ? n.CuaHang.DiaChi : "Toàn chuỗi"
                })
                .ToList();
        }

        public string GenerateNextMaNhanVien()
        {
            var existingIds = _context.NhanViens.Select(n => n.MaNhanVien).ToList();
            int maxNum = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith("NV", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(id.Substring(2), out int num))
                    {
                        if (num > maxNum) maxNum = num;
                    }
                }
            }
            return $"NV{(maxNum + 1):D3}";
        }

        public (bool Success, string Message) AddNhanVien(string maNV, string hoTen, string cmnd, string sdt, string user, string pass, int vaiTro, string? cuaHang)
        {
            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                return (false, "Họ tên, Tài khoản và Mật khẩu không được để trống.");

            if (!AppSession.IsAdmin)
            {
                var targetRole = _context.VaiTros.Find(vaiTro);
                if (targetRole != null && targetRole.TenVaiTro != null && targetRole.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Bạn không có quyền tạo tài khoản với chức vụ Admin.");
                }

                if (!string.IsNullOrEmpty(AppSession.CurrentMaCuaHang))
                {
                    cuaHang = AppSession.CurrentMaCuaHang;
                }
            }

            string ma = !string.IsNullOrWhiteSpace(maNV) ? maNV.Trim() : GenerateNextMaNhanVien();

            if (_context.NhanViens.Any(n => n.MaNhanVien.ToLower() == ma.ToLower()))
                return (false, $"Mã nhân viên '{ma}' đã tồn tại.");

            if (_context.NhanViens.Any(n => n.TenDangNhap.ToLower() == user.Trim().ToLower()))
                return (false, $"Tên đăng nhập '{user}' đã có người sử dụng.");

            var nv = new NhanVien
            {
                MaNhanVien = ma,
                HoTen = hoTen.Trim(),
                CMND = string.IsNullOrWhiteSpace(cmnd) ? "N/A" : cmnd.Trim(),
                SoDienThoai = sdt.Trim(),
                TenDangNhap = user.Trim(),
                MatKhau = pass,
                MaVaiTro = vaiTro,
                MaCuaHang = string.IsNullOrWhiteSpace(cuaHang) ? null : cuaHang
            };

            _context.NhanViens.Add(nv);
            _context.SaveChanges();
            return (true, $"Thêm nhân viên '{nv.HoTen}' thành công!");
        }

        public (bool Success, string Message) UpdateNhanVien(string maNV, string hoTen, string cmnd, string sdt, int vaiTro, string? cuaHang)
        {
            var nv = _context.NhanViens.Include(n => n.VaiTro).FirstOrDefault(n => n.MaNhanVien == maNV);
            if (nv == null) return (false, "Nhân viên không tồn tại.");

            if (!AppSession.IsAdmin)
            {
                if (nv.VaiTro != null && nv.VaiTro.TenVaiTro != null && nv.VaiTro.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Bạn không có quyền chỉnh sửa tài khoản Admin.");
                }

                var targetRole = _context.VaiTros.Find(vaiTro);
                if (targetRole != null && targetRole.TenVaiTro != null && targetRole.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Bạn không có quyền nâng cấp nhân viên lên chức vụ Admin.");
                }

                if (!string.IsNullOrEmpty(AppSession.CurrentMaCuaHang))
                {
                    cuaHang = AppSession.CurrentMaCuaHang;
                }
            }

            nv.HoTen = hoTen.Trim();
            nv.CMND = string.IsNullOrWhiteSpace(cmnd) ? "N/A" : cmnd.Trim();
            nv.SoDienThoai = sdt.Trim();
            nv.MaVaiTro = vaiTro;
            nv.MaCuaHang = string.IsNullOrWhiteSpace(cuaHang) ? null : cuaHang;

            _context.SaveChanges();
            return (true, "Cập nhật hồ sơ nhân sự thành công!");
        }

        public (bool Success, string Message) DeleteNhanVien(string maNV)
        {
            var nv = _context.NhanViens.Include(n => n.VaiTro).FirstOrDefault(n => n.MaNhanVien == maNV);
            if (nv == null) return (false, "Nhân viên không tồn tại.");

            if (!AppSession.IsAdmin)
            {
                if (nv.VaiTro != null && nv.VaiTro.TenVaiTro != null && nv.VaiTro.TenVaiTro.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Bạn không có quyền xóa tài khoản Admin.");
                }
            }

            if (_context.PhieuMuons.Any(p => p.MaNhanVienChoMuon == maNV))
                return (false, "Không thể xóa nhân viên đã tham gia lập phiếu mượn trả.");

            _context.NhanViens.Remove(nv);
            _context.SaveChanges();
            return (true, "Xóa hồ sơ nhân sự thành công!");
        }
    }
}

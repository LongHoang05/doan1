using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    public class KhachHangService
    {
        private readonly QuanLyThueBangContext _context;

        public KhachHangService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        public List<KhachHangDTO> GetAllKhachHang(string? keyword = null)
        {
            var query = _context.KhachHangs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(k => k.MaKhachHang.ToLower().Contains(kw) ||
                                         k.HoTen.ToLower().Contains(kw) ||
                                         k.SoDienThoai.Contains(kw) ||
                                         k.CMND.Contains(kw));
            }

            return query
                .OrderBy(k => k.MaKhachHang)
                .Select(k => new KhachHangDTO
                {
                    MaKhachHang = k.MaKhachHang,
                    HoTen = k.HoTen,
                    CMND = k.CMND,
                    SoDienThoai = k.SoDienThoai,
                    DiaChi = k.DiaChi ?? string.Empty,
                    NgayDangKy = k.NgayDangKy,
                    SoLuotMuon = k.PhieuMuons.Count
                })
                .ToList();
        }

        public string GenerateNextMaKhachHang()
        {
            var existingIds = _context.KhachHangs.Select(k => k.MaKhachHang).ToList();
            int maxNum = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith("KH", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(id.Substring(2), out int num))
                    {
                        if (num > maxNum) maxNum = num;
                    }
                }
            }
            return $"KH{(maxNum + 1):D3}";
        }

        public (bool Success, string Message) AddKhachHang(string maKhachHang, string hoTen, string cmnd, string sdt, string diaChi)
        {
            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(sdt))
                return (false, "Họ tên và số điện thoại khách hàng không được để trống.");

            string maKH = !string.IsNullOrWhiteSpace(maKhachHang) ? maKhachHang.Trim() : GenerateNextMaKhachHang();

            if (_context.KhachHangs.Any(k => k.MaKhachHang.ToLower() == maKH.ToLower()))
                return (false, $"Mã khách hàng '{maKH}' đã tồn tại.");

            var kh = new KhachHang
            {
                MaKhachHang = maKH,
                HoTen = hoTen.Trim(),
                CMND = string.IsNullOrWhiteSpace(cmnd) ? "N/A" : cmnd.Trim(),
                SoDienThoai = sdt.Trim(),
                DiaChi = diaChi.Trim(),
                NgayDangKy = DateTime.Now
            };

            _context.KhachHangs.Add(kh);
            _context.SaveChanges();
            return (true, $"Thêm khách hàng '{kh.HoTen}' (Mã: {maKH}) thành công!");
        }

        public (bool Success, string Message) UpdateKhachHang(string maKhachHang, string hoTen, string cmnd, string sdt, string diaChi)
        {
            var kh = _context.KhachHangs.Find(maKhachHang);
            if (kh == null) return (false, "Khách hàng không tồn tại.");

            kh.HoTen = hoTen.Trim();
            kh.CMND = string.IsNullOrWhiteSpace(cmnd) ? "N/A" : cmnd.Trim();
            kh.SoDienThoai = sdt.Trim();
            kh.DiaChi = diaChi.Trim();

            _context.SaveChanges();
            return (true, "Cập nhật thông tin khách hàng thành công!");
        }

        public (bool Success, string Message) DeleteKhachHang(string maKhachHang)
        {
            var kh = _context.KhachHangs.Find(maKhachHang);
            if (kh == null) return (false, "Khách hàng không tồn tại.");

            if (_context.PhieuMuons.Any(p => p.MaKhachHang == maKhachHang))
                return (false, "Không thể xóa khách hàng đã có lịch sử giao dịch thuê băng.");

            _context.KhachHangs.Remove(kh);
            _context.SaveChanges();
            return (true, "Xóa hồ sơ khách hàng thành công!");
        }
    }
}

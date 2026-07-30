using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    public class CuaHangService
    {
        private readonly QuanLyThueBangContext _context;

        public CuaHangService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        public List<CuaHangDTO> GetAllCuaHang(string? keyword = null)
        {
            var query = _context.CuaHangs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(c => c.MaCuaHang.ToLower().Contains(kw) ||
                                         c.DiaChi.ToLower().Contains(kw) ||
                                         c.SoDienThoai.Contains(kw));
            }

            return query
                .OrderBy(c => c.MaCuaHang)
                .Select(c => new CuaHangDTO
                {
                    MaCuaHang = c.MaCuaHang,
                    DiaChi = c.DiaChi,
                    SoDienThoai = c.SoDienThoai,
                    SoLuongNhanVien = c.NhanViens.Count,
                    SoLuongBanSao = c.BanSaoBangs.Count
                })
                .ToList();
        }

        public string GenerateNextMaCuaHang()
        {
            var existingIds = _context.CuaHangs.Select(c => c.MaCuaHang).ToList();
            int maxNum = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith("CH", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(id.Substring(2), out int num))
                    {
                        if (num > maxNum) maxNum = num;
                    }
                }
            }
            return $"CH{(maxNum + 1):D2}";
        }

        public (bool Success, string Message) AddCuaHang(string maCuaHang, string diaChi, string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(maCuaHang) || string.IsNullOrWhiteSpace(diaChi))
                return (false, "Mã cửa hàng và địa chỉ không được để trống.");

            maCuaHang = maCuaHang.Trim();
            if (_context.CuaHangs.Any(c => c.MaCuaHang.ToLower() == maCuaHang.ToLower()))
                return (false, $"Mã cửa hàng '{maCuaHang}' đã tồn tại.");

            var ch = new CuaHang
            {
                MaCuaHang = maCuaHang,
                DiaChi = diaChi.Trim(),
                SoDienThoai = soDienThoai.Trim()
            };

            _context.CuaHangs.Add(ch);
            _context.SaveChanges();
            return (true, $"Thêm cửa hàng '{maCuaHang}' thành công!");
        }

        public (bool Success, string Message) UpdateCuaHang(string maCuaHang, string diaChi, string soDienThoai)
        {
            var ch = _context.CuaHangs.Find(maCuaHang);
            if (ch == null) return (false, "Cửa hàng không tồn tại.");

            ch.DiaChi = diaChi.Trim();
            ch.SoDienThoai = soDienThoai.Trim();
            _context.SaveChanges();
            return (true, "Cập nhật thông tin cửa hàng thành công!");
        }

        public (bool Success, string Message) DeleteCuaHang(string maCuaHang)
        {
            var ch = _context.CuaHangs.Find(maCuaHang);
            if (ch == null) return (false, "Cửa hàng không tồn tại.");

            if (_context.BanSaoBangs.Any(b => b.MaCuaHangHienTai == maCuaHang))
                return (false, "Không thể xóa cửa hàng đang chứa bản sao băng trong kho.");

            _context.CuaHangs.Remove(ch);
            _context.SaveChanges();
            return (true, "Xóa cửa hàng thành công!");
        }
    }
}

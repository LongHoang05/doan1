using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    public class BanSaoBangService
    {
        private readonly QuanLyThueBangContext _context;

        public BanSaoBangService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        public List<CuaHang> GetAllCuaHang()
        {
            var list = _context.CuaHangs.OrderBy(c => c.MaCuaHang).ToList();
            if (list.Count == 0)
            {
                var chDefault = new CuaHang
                {
                    MaCuaHang = "CH01",
                    DiaChi = "Trụ sở chính - 123 Lê Lợi, TP.HCM",
                    SoDienThoai = "0901234567"
                };
                _context.CuaHangs.Add(chDefault);
                _context.SaveChanges();
                list.Add(chDefault);
            }
            return list;
        }

        public List<BanSaoBangViewDTO> GetAllBanSao(string? keyword = null, string? filterMaPhim = null, string? filterTrangThai = null)
        {
            var query = _context.BanSaoBangs
                .Include(b => b.Phim)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(b => b.MaBanSao.ToLower().Contains(kw) ||
                                         (b.Phim != null && b.Phim.TuaDe.ToLower().Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(filterMaPhim) && filterMaPhim != "ALL")
            {
                query = query.Where(b => b.MaPhim == filterMaPhim);
            }

            if (!string.IsNullOrWhiteSpace(filterTrangThai) && filterTrangThai != "ALL")
            {
                query = query.Where(b => b.TrangThai == filterTrangThai);
            }

            return query
                .OrderBy(b => b.MaPhim)
                .ThenBy(b => b.SoThuTuBanSao)
                .Select(b => new BanSaoBangViewDTO
                {
                    MaBanSao = b.MaBanSao,
                    MaPhim = b.MaPhim,
                    TuaDePhim = b.Phim != null ? b.Phim.TuaDe : string.Empty,
                    MaCuaHangHienTai = b.MaCuaHangHienTai,
                    LoaiBang = b.LoaiBang,
                    DonGiaThue = b.DonGiaThue,
                    TrangThai = b.TrangThai
                })
                .ToList();
        }

        public BanSaoBangViewDTO? GetBanSaoById(string maBanSao)
        {
            return _context.BanSaoBangs
                .Include(b => b.Phim)
                .AsNoTracking()
                .Where(b => b.MaBanSao == maBanSao)
                .Select(b => new BanSaoBangViewDTO
                {
                    MaBanSao = b.MaBanSao,
                    MaPhim = b.MaPhim,
                    TuaDePhim = b.Phim != null ? b.Phim.TuaDe : string.Empty,
                    MaCuaHangHienTai = b.MaCuaHangHienTai,
                    LoaiBang = b.LoaiBang,
                    DonGiaThue = b.DonGiaThue,
                    TrangThai = b.TrangThai
                })
                .FirstOrDefault();
        }

        public string GenerateNextMaBanSao(string maPhim)
        {
            int count = _context.BanSaoBangs.Count(b => b.MaPhim == maPhim);
            int nextSTT = count + 1;
            string candidate = $"{maPhim}-BS{nextSTT:D2}";

            while (_context.BanSaoBangs.Any(b => b.MaBanSao == candidate))
            {
                nextSTT++;
                candidate = $"{maPhim}-BS{nextSTT:D2}";
            }

            return candidate;
        }

        public (bool Success, string Message) AddBanSao(string maPhim, string maCuaHang, string loaiBang, decimal donGiaThue, string? customMaBanSao = null)
        {
            if (string.IsNullOrWhiteSpace(maPhim))
                return (false, "Vui lòng chọn Tựa phim gốc cho bản sao.");

            string maBanSao = !string.IsNullOrWhiteSpace(customMaBanSao)
                ? customMaBanSao.Trim()
                : GenerateNextMaBanSao(maPhim);

            if (_context.BanSaoBangs.Any(b => b.MaBanSao.ToLower() == maBanSao.ToLower()))
                return (false, $"Mã bản sao '{maBanSao}' đã tồn tại trong hệ thống.");

            int currentCount = _context.BanSaoBangs.Count(b => b.MaPhim == maPhim);

            var banSao = new BanSaoBang
            {
                MaBanSao = maBanSao,
                MaPhim = maPhim,
                MaCuaHangHienTai = string.IsNullOrWhiteSpace(maCuaHang) ? "CH01" : maCuaHang,
                SoThuTuBanSao = currentCount + 1,
                LoaiBang = string.IsNullOrWhiteSpace(loaiBang) ? "PAL" : loaiBang,
                DonGiaThue = donGiaThue > 0 ? donGiaThue : 15000m,
                TrangThai = "Sẵn sàng"
            };

            _context.BanSaoBangs.Add(banSao);
            _context.SaveChanges();
            return (true, $"Thêm bản sao băng '{maBanSao}' thành công!");
        }

        public (bool Success, string Message) UpdateBanSao(string maBanSao, string loaiBang, decimal donGiaThue, string trangThai, string maCuaHang)
        {
            var banSao = _context.BanSaoBangs.Find(maBanSao);
            if (banSao == null)
                return (false, "Bản sao băng không tồn tại.");

            banSao.LoaiBang = loaiBang;
            banSao.DonGiaThue = donGiaThue;
            banSao.TrangThai = trangThai;
            if (!string.IsNullOrWhiteSpace(maCuaHang))
            {
                banSao.MaCuaHangHienTai = maCuaHang;
            }

            _context.SaveChanges();
            return (true, "Cập nhật thông tin bản sao thành công!");
        }

        public (bool Success, string Message) DeleteBanSao(string maBanSao)
        {
            var banSao = _context.BanSaoBangs.Find(maBanSao);
            if (banSao == null)
                return (false, "Bản sao băng không tồn tại.");

            if (banSao.TrangThai == "Đang cho mượn")
                return (false, "Không thể xóa bản sao đang được khách mượn.");

            _context.BanSaoBangs.Remove(banSao);
            _context.SaveChanges();
            return (true, "Xóa bản sao băng thành công!");
        }
    }
}

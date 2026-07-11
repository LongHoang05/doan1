using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Domain.DTOs;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    /// <summary>
    /// Tầng Business Logic Layer (BLL) - Quản lý Danh mục Phim & Thể Loại
    /// </summary>
    public class PhimService
    {
        private readonly QuanLyThueBangContext _context;

        public PhimService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        #region NGHIỆP VỤ THỂ LOẠI (CATEGORY)

        public List<TheLoaiDTO> GetAllTheLoai(string? keyword = null)
        {
            var query = _context.TheLoais.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(tl => tl.TenTheLoai.ToLower().Contains(kw));
            }

            return query
                .OrderBy(tl => tl.TenTheLoai)
                .Select(tl => new TheLoaiDTO
                {
                    MaTheLoai = tl.MaTheLoai,
                    TenTheLoai = tl.TenTheLoai,
                    SoLuongPhim = tl.Phims.Count()
                })
                .ToList();
        }

        public (bool Success, string Message) AddTheLoai(string tenTheLoai)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai))
                return (false, "Tên thể loại không được để trống.");

            bool exists = _context.TheLoais.Any(t => t.TenTheLoai.ToLower() == tenTheLoai.Trim().ToLower());
            if (exists)
                return (false, "Tên thể loại này đã tồn tại trong hệ thống.");

            var theLoai = new TheLoai
            {
                TenTheLoai = tenTheLoai.Trim()
            };

            _context.TheLoais.Add(theLoai);
            _context.SaveChanges();
            return (true, "Thêm thể loại mới thành công!");
        }

        public (bool Success, string Message) UpdateTheLoai(int maTheLoai, string tenTheLoai)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai))
                return (false, "Tên thể loại không được để trống.");

            var theLoai = _context.TheLoais.Find(maTheLoai);
            if (theLoai == null)
                return (false, "Thể loại không tồn tại.");

            theLoai.TenTheLoai = tenTheLoai.Trim();
            _context.SaveChanges();
            return (true, "Cập nhật thể loại thành công!");
        }

        public (bool Success, string Message) DeleteTheLoai(int maTheLoai)
        {
            var theLoai = _context.TheLoais.Find(maTheLoai);
            if (theLoai == null)
                return (false, "Thể loại không tồn tại.");

            // Kiểm tra xem đã có phim nào thuộc thể loại này chưa
            bool hasMovies = _context.Phims.Any(p => p.MaTheLoai == maTheLoai);
            if (hasMovies)
                return (false, "Không thể xóa thể loại này vì đang có tựa phim thuộc thể loại này.");

            _context.TheLoais.Remove(theLoai);
            _context.SaveChanges();
            return (true, "Xóa thể loại thành công!");
        }

        #endregion

        #region NGHIỆP VỤ PHIM (MOVIES)

        /// <summary>
        /// Lấy danh sách phim (Hỗ trợ tìm kiếm theo từ khóa và lọc theo mã thể loại)
        /// </summary>
        public List<PhimDTO> GetAllPhim(string? keyword = null, int? filterMaTheLoai = null)
        {
            var query = _context.Phims.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(p => p.TuaDe.ToLower().Contains(kw) || p.MaPhim.ToLower().Contains(kw));
            }

            if (filterMaTheLoai.HasValue && filterMaTheLoai.Value > 0)
            {
                query = query.Where(p => p.MaTheLoai == filterMaTheLoai.Value);
            }

            return query
                .OrderBy(p => p.TuaDe)
                .Select(p => new PhimDTO
                {
                    MaPhim = p.MaPhim,
                    TuaDe = p.TuaDe,
                    NamPhatHanh = p.NamPhatHanh,
                    DoDaiPhut = p.DoDaiPhut,
                    MaTheLoai = p.MaTheLoai,
                    TenTheLoai = p.TheLoai != null ? p.TheLoai.TenTheLoai : string.Empty,
                    SoLuongBanSao = p.BanSaoBangs.Count
                })
                .ToList();
        }

        public PhimDTO? GetPhimById(string maPhim)
        {
            return _context.Phims
                .AsNoTracking()
                .Where(p => p.MaPhim == maPhim)
                .Select(p => new PhimDTO
                {
                    MaPhim = p.MaPhim,
                    TuaDe = p.TuaDe,
                    NamPhatHanh = p.NamPhatHanh,
                    DoDaiPhut = p.DoDaiPhut,
                    MaTheLoai = p.MaTheLoai,
                    TenTheLoai = p.TheLoai != null ? p.TheLoai.TenTheLoai : string.Empty,
                    SoLuongBanSao = p.BanSaoBangs.Count
                })
                .FirstOrDefault();
        }

        /// <summary>
        /// Tự động sinh mã phim tiếp theo theo định dạng PHIM001, PHIM002...
        /// </summary>
        public string GenerateNextMaPhim()
        {
            var existingIds = _context.Phims
                .Select(p => p.MaPhim)
                .ToList();

            int maxNumber = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith("PHIM", StringComparison.OrdinalIgnoreCase))
                {
                    string numPart = id.Substring(4);
                    if (int.TryParse(numPart, out int num))
                    {
                        if (num > maxNumber) maxNumber = num;
                    }
                }
            }

            return $"PHIM{(maxNumber + 1):D3}";
        }

        public (bool Success, string Message) AddPhim(string maPhim, string tuaDe, int? namPhatHanh, int? doDaiPhut, int maTheLoai)
        {
            if (string.IsNullOrWhiteSpace(maPhim) || string.IsNullOrWhiteSpace(tuaDe))
                return (false, "Mã phim và tựa đề phim không được để trống.");

            maPhim = maPhim.Trim();
            tuaDe = tuaDe.Trim();

            bool exists = _context.Phims.Any(p => p.MaPhim.ToLower() == maPhim.ToLower());
            if (exists)
                return (false, $"Mã phim '{maPhim}' đã tồn tại trong hệ thống.");

            bool categoryExists = _context.TheLoais.Any(t => t.MaTheLoai == maTheLoai);
            if (!categoryExists)
                return (false, "Thể loại phim được chọn không hợp lệ.");

            if (doDaiPhut.HasValue && doDaiPhut.Value <= 0)
                return (false, "Độ dài phim (phút) phải lớn hơn 0.");

            var phim = new Phim
            {
                MaPhim = maPhim,
                TuaDe = tuaDe,
                NamPhatHanh = namPhatHanh,
                DoDaiPhut = doDaiPhut,
                MaTheLoai = maTheLoai
            };

            _context.Phims.Add(phim);
            _context.SaveChanges();
            return (true, "Thêm phim mới thành công!");
        }

        public (bool Success, string Message) UpdatePhim(string maPhim, string tuaDe, int? namPhatHanh, int? doDaiPhut, int maTheLoai)
        {
            if (string.IsNullOrWhiteSpace(tuaDe))
                return (false, "Tựa đề phim không được để trống.");

            var phim = _context.Phims.Find(maPhim);
            if (phim == null)
                return (false, "Không tìm thấy phim cần cập nhật.");

            bool categoryExists = _context.TheLoais.Any(t => t.MaTheLoai == maTheLoai);
            if (!categoryExists)
                return (false, "Thể loại phim được chọn không hợp lệ.");

            if (doDaiPhut.HasValue && doDaiPhut.Value <= 0)
                return (false, "Độ dài phim (phút) phải lớn hơn 0.");

            phim.TuaDe = tuaDe.Trim();
            phim.NamPhatHanh = namPhatHanh;
            phim.DoDaiPhut = doDaiPhut;
            phim.MaTheLoai = maTheLoai;

            _context.SaveChanges();
            return (true, "Cập nhật thông tin phim thành công!");
        }

        public (bool Success, string Message) DeletePhim(string maPhim)
        {
            var phim = _context.Phims.Find(maPhim);
            if (phim == null)
                return (false, "Phim không tồn tại trong hệ thống.");

            bool hasCopies = _context.BanSaoBangs.Any(b => b.MaPhim == maPhim);
            if (hasCopies)
                return (false, "Không thể xóa phim này vì đang có bản sao băng trong kho. Hãy xóa hoặc chuyển bản sao băng trước.");

            _context.Phims.Remove(phim);
            _context.SaveChanges();
            return (true, "Xóa phim thành công!");
        }

        #endregion
    }
}

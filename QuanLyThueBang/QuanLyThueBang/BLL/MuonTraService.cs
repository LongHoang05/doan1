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
    /// <summary>
    /// Tầng Business Logic Layer (BLL) - Xử lý nghiệp vụ cốt lõi Mượn - Trả Băng & Luân chuyển kho
    /// </summary>
    public class MuonTraService
    {
        private readonly QuanLyThueBangContext _context;

        public MuonTraService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Kiểm tra và tra cứu thông tin bản sao băng cho nghiệp vụ lập phiếu mượn
        /// </summary>
        public (bool IsValid, string ErrorMessage, ChiTietGioMuonDTO? Item) ValidateAndGetBangForMuon(string queryMaHoacRFID, string maCuaHangHienTai)
        {
            queryMaHoacRFID = queryMaHoacRFID.Trim();
            var bang = _context.BanSaoBangs
                .Include(b => b.Phim).ThenInclude(p => p!.TheLoai)
                .FirstOrDefault(b => b.MaBanSao == queryMaHoacRFID);

            if (bang == null)
                return (false, $"Không tìm thấy bản sao băng có mã '{queryMaHoacRFID}'.", null);

            if (!string.IsNullOrEmpty(maCuaHangHienTai) && !string.Equals(bang.MaCuaHangHienTai, maCuaHangHienTai, StringComparison.OrdinalIgnoreCase))
                return (false, $"Cuốn băng '{bang.MaBanSao}' hiện thuộc kho chi nhánh [{bang.MaCuaHangHienTai}], không nằm tại quầy giao dịch hiện tại.", null);

            if (bang.TrangThai != Constants.TrangThaiBang_SanSang)
                return (false, $"Cuốn băng '{bang.MaBanSao}' đang ở trạng thái '{bang.TrangThai}', không thể cho mượn.", null);

            if (bang.NgayHetHan < DateTime.Today)
                return (false, $"Cuốn băng '{bang.MaBanSao}' đã hết hạn sử dụng (Ngày hết hạn: {bang.NgayHetHan:dd/MM/yyyy}).", null);

            var item = new ChiTietGioMuonDTO
            {
                MaBanSao = bang.MaBanSao,
                RFID = bang.MaBanSao,
                TuaDe = bang.Phim?.TuaDe ?? "N/A",
                TenTheLoai = bang.Phim?.TheLoai?.TenTheLoai ?? "N/A",
                DonGiaThue = bang.DonGiaThue
            };

            return (true, string.Empty, item);
        }

        /// <summary>
        /// Lập phiếu mượn mới cho khách hàng (Transaction Backend)
        /// </summary>
        public (bool Success, string Message, string MaPhieuMuon) LapPhieuMuon(string maKhachHang, string maCuaHang, string maNhanVien, List<string> listMaBanSao, DateTime ngayDuKienTra)
        {
            if (string.IsNullOrWhiteSpace(maKhachHang))
                return (false, "Vui lòng chọn hoặc đăng ký Khách hàng.", string.Empty);

            if (listMaBanSao == null || listMaBanSao.Count == 0)
                return (false, "Giỏ hàng mượn đang trống.", string.Empty);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string maPM = $"PM{DateTime.Now:yyMMddHHmmss}";
                var phieuMuon = new PhieuMuon
                {
                    MaPhieuMuon = maPM,
                    MaKhachHang = maKhachHang,
                    MaCuaHangMuon = maCuaHang,
                    MaNhanVienChoMuon = maNhanVien,
                    NgayMuon = DateTime.Now,
                    NgayDuKienTra = ngayDuKienTra
                };

                _context.PhieuMuons.Add(phieuMuon);

                foreach (var maBS in listMaBanSao)
                {
                    var bang = _context.BanSaoBangs.FirstOrDefault(b => b.MaBanSao == maBS);
                    if (bang == null || bang.TrangThai != Constants.TrangThaiBang_SanSang)
                        return (false, $"Cuốn băng '{maBS}' không còn sẵn sàng cho mượn.", string.Empty);

                    bang.TrangThai = Constants.TrangThaiBang_DangChoMuon;

                    var chiTiet = new ChiTietPhieuMuon
                    {
                        MaPhieuMuon = maPM,
                        MaBanSao = maBS,
                        TrangThaiTra = false
                    };
                    _context.ChiTietPhieuMuons.Add(chiTiet);
                }

                _context.SaveChanges();
                transaction.Commit();
                return (true, $"Lập phiếu mượn [{maPM}] thành công với {listMaBanSao.Count} cuốn băng!", maPM);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Lỗi transaction: {ex.Message}", string.Empty);
            }
        }

        /// <summary>
        /// Tra cứu thông tin cuốn băng đang được mượn khi khách mang trả tại quầy
        /// </summary>
        public (bool Success, string ErrorMessage, ThongTinBangMuonChuaTraDTO? Data) TraCuuBangMuonChuaTra(string queryMaHoacRFID)
        {
            queryMaHoacRFID = queryMaHoacRFID.Trim();
            var chiTiet = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon).ThenInclude(pm => pm!.KhachHang)
                .Include(ct => ct.BanSaoBang).ThenInclude(bs => bs!.Phim)
                .FirstOrDefault(ct => !ct.TrangThaiTra && ct.MaBanSao == queryMaHoacRFID);

            if (chiTiet == null || chiTiet.PhieuMuon == null || chiTiet.BanSaoBang == null)
                return (false, $"Không tìm thấy giao dịch đang cho mượn cho băng có mã '{queryMaHoacRFID}'.", null);

            int soNgayTre = (DateTime.Today - chiTiet.PhieuMuon.NgayDuKienTra.Date).Days;
            bool isTre = soNgayTre > 0;

            var dto = new ThongTinBangMuonChuaTraDTO
            {
                MaPhieuMuon = chiTiet.MaPhieuMuon,
                MaBanSao = chiTiet.MaBanSao,
                RFID = chiTiet.MaBanSao,
                TuaDe = chiTiet.BanSaoBang.Phim?.TuaDe ?? "N/A",
                MaKhachHang = chiTiet.PhieuMuon.MaKhachHang,
                HoTenKhachHang = chiTiet.PhieuMuon.KhachHang?.HoTen ?? "Khách vãng lai",
                SoDienThoai = chiTiet.PhieuMuon.KhachHang?.SoDienThoai ?? "",
                NgayMuon = chiTiet.PhieuMuon.NgayMuon,
                NgayDuKienTra = chiTiet.PhieuMuon.NgayDuKienTra,
                SoNgayTreHan = isTre ? soNgayTre : 0,
                IsTreHan = isTre,
                DonGiaThue = chiTiet.BanSaoBang.DonGiaThue,
                TinhTrangKhiTra = Constants.TinhTrangTra_BinhThuong,
                TienPhat = 0
            };

            return (true, string.Empty, dto);
        }

        /// <summary>
        /// Chốt giao dịch nhận trả băng, tính phạt thủ công & Luân chuyển kho liên chi nhánh
        /// </summary>
        public (bool Success, string Message, string MaPhieuTra) ChotNhanTraBang(string maKhachHang, string maCuaHangNhanTra, string maNhanVienNhanTra, List<ThongTinBangMuonChuaTraDTO> listBangTra)
        {
            if (listBangTra == null || listBangTra.Count == 0)
                return (false, "Danh sách băng mang trả đang trống.", string.Empty);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string maPT = $"PT{DateTime.Now:yyMMddHHmmss}";
                decimal tongTienThu = listBangTra.Sum(b => b.TongTien);

                var phieuTra = new PhieuTra
                {
                    MaPhieuTra = maPT,
                    MaKhachHang = maKhachHang,
                    MaCuaHangNhanTra = maCuaHangNhanTra,
                    MaNhanVienNhanTra = maNhanVienNhanTra,
                    NgayTra = DateTime.Now,
                    TongTienThu = tongTienThu
                };
                _context.PhieuTras.Add(phieuTra);

                foreach (var item in listBangTra)
                {
                    var ctMuon = _context.ChiTietPhieuMuons.FirstOrDefault(ct => ct.MaPhieuMuon == item.MaPhieuMuon && ct.MaBanSao == item.MaBanSao);
                    if (ctMuon != null)
                    {
                        ctMuon.TrangThaiTra = true;
                    }

                    var ctTra = new ChiTietPhieuTra
                    {
                        MaPhieuTra = maPT,
                        MaBanSao = item.MaBanSao,
                        MaPhieuMuon = item.MaPhieuMuon,
                        TinhTrangBangKhiTra = item.TinhTrangKhiTra,
                        TienThue = item.TienThue,
                        TienPhat = item.TienPhat
                    };
                    _context.ChiTietPhieuTras.Add(ctTra);

                    // Xử lý luân chuyển kho tự động:
                    // Cuốn băng được trả về chi nhánh nào thì cập nhật MaCuaHangHienTai về đúng chi nhánh đó
                    var bang = _context.BanSaoBangs.FirstOrDefault(b => b.MaBanSao == item.MaBanSao);
                    if (bang != null)
                    {
                        bang.TrangThai = Constants.TrangThaiBang_SanSang;
                        bang.MaCuaHangHienTai = maCuaHangNhanTra;
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return (true, $"Chốt trả {listBangTra.Count} cuốn băng thành công! Phiếu trả [{maPT}].", maPT);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Lỗi transaction: {ex.Message}", string.Empty);
            }
        }

        /// <summary>
        /// Quét tự động danh sách các cuốn băng đang cho mượn bị quá hạn
        /// </summary>
        public List<BangQuaHanDTO> GetDanhSachBangQuaHan()
        {
            var today = DateTime.Today;
            var list = _context.ChiTietPhieuMuons
                .Include(ct => ct.PhieuMuon).ThenInclude(pm => pm!.KhachHang)
                .Include(ct => ct.BanSaoBang).ThenInclude(bs => bs!.Phim)
                .Where(ct => !ct.TrangThaiTra && ct.PhieuMuon != null && ct.PhieuMuon.NgayDuKienTra < today)
                .ToList();

            return list.Select(ct => new BangQuaHanDTO
            {
                MaPhieuMuon = ct.MaPhieuMuon,
                MaBanSao = ct.MaBanSao,
                RFID = ct.MaBanSao,
                TuaDe = ct.BanSaoBang?.Phim?.TuaDe ?? "N/A",
                MaKhachHang = ct.PhieuMuon?.MaKhachHang ?? "",
                HoTenKhachHang = ct.PhieuMuon?.KhachHang?.HoTen ?? "Khách vãng lai",
                SoDienThoai = ct.PhieuMuon?.KhachHang?.SoDienThoai ?? "",
                NgayMuon = ct.PhieuMuon?.NgayMuon ?? DateTime.MinValue,
                NgayDuKienTra = ct.PhieuMuon?.NgayDuKienTra ?? DateTime.MinValue,
                SoNgayTreHan = (today - (ct.PhieuMuon?.NgayDuKienTra.Date ?? today)).Days
            }).ToList();
        }

        /// <summary>
        /// Lấy danh sách toàn bộ phiếu mượn có hỗ trợ tìm kiếm theo Mã PM hoặc Tên KH
        /// </summary>
        public List<PhieuMuonViewDTO> GetAllPhieuMuon(string keyword = "")
        {
            var query = _context.PhieuMuons
                .Include(p => p.KhachHang)
                .Include(p => p.CuaHangMuon)
                .Include(p => p.NhanVienChoMuon)
                .Include(p => p.ChiTietPhieuMuons)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(p => p.MaPhieuMuon.ToLower().Contains(kw) ||
                                         (p.KhachHang != null && p.KhachHang.HoTen.ToLower().Contains(kw)) ||
                                         p.MaKhachHang.ToLower().Contains(kw));
            }

            var result = query.OrderByDescending(p => p.NgayMuon).ToList();

            return result.Select(p =>
            {
                int total = p.ChiTietPhieuMuons?.Count ?? 0;
                int returned = p.ChiTietPhieuMuons?.Count(ct => ct.TrangThaiTra) ?? 0;
                string status = total > 0 && returned == total ? "Đã trả đủ" :
                                (p.NgayDuKienTra.Date < DateTime.Today ? "Quá hạn" : "Đang mượn");

                return new PhieuMuonViewDTO
                {
                    MaPhieuMuon = p.MaPhieuMuon,
                    MaKhachHang = p.MaKhachHang,
                    TenKhachHang = p.KhachHang != null ? $"{p.KhachHang.MaKhachHang} - {p.KhachHang.HoTen}" : p.MaKhachHang,
                    TenCuaHang = p.CuaHangMuon != null ? p.CuaHangMuon.DiaChi : (p.MaCuaHangMuon ?? ""),
                    TenNhanVien = p.NhanVienChoMuon != null ? p.NhanVienChoMuon.HoTen : (p.MaNhanVienChoMuon ?? ""),
                    NgayMuon = p.NgayMuon,
                    NgayDuKienTra = p.NgayDuKienTra,
                    SoLuongBang = total,
                    TrangThaiPhieu = status
                };
            }).ToList();
        }

        /// <summary>
        /// Lấy chi tiết các cuốn băng thuộc 1 phiếu mượn
        /// </summary>
        public List<ChiTietPhieuMuonViewDTO> GetChiTietByPhieuMuon(string maPhieuMuon)
        {
            var list = _context.ChiTietPhieuMuons
                .Include(ct => ct.BanSaoBang).ThenInclude(b => b!.Phim).ThenInclude(p => p!.TheLoai)
                .Where(ct => ct.MaPhieuMuon == maPhieuMuon)
                .ToList();

            return list.Select(ct => new ChiTietPhieuMuonViewDTO
            {
                MaBanSao = ct.MaBanSao,
                TuaDe = ct.BanSaoBang?.Phim?.TuaDe ?? "N/A",
                TenTheLoai = ct.BanSaoBang?.Phim?.TheLoai?.TenTheLoai ?? "N/A",
                DonGiaThue = ct.BanSaoBang?.DonGiaThue ?? 0,
                TrangThaiTra = ct.TrangThaiTra ? "Đã trả" : "Chưa trả"
            }).ToList();
        }

        /// <summary>
        /// Xóa phiếu mượn (chỉ khi tất cả băng trong phiếu chưa trả hoặc admin khôi phục trạng thái)
        /// </summary>
        public (bool Success, string Message) XoaPhieuMuon(string maPhieuMuon)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var phieu = _context.PhieuMuons
                    .Include(p => p.ChiTietPhieuMuons)
                    .FirstOrDefault(p => p.MaPhieuMuon == maPhieuMuon);

                if (phieu == null)
                    return (false, "Không tìm thấy phiếu mượn.");

                // Trước hết xóa các ChiTietPhieuTra liên quan đến Phiếu Mượn này (nếu có)
                var chiTietTras = _context.ChiTietPhieuTras.Where(ct => ct.MaPhieuMuon == maPhieuMuon).ToList();
                if (chiTietTras.Any())
                {
                    var maPhieuTras = chiTietTras.Select(x => x.MaPhieuTra).Distinct().ToList();
                    _context.ChiTietPhieuTras.RemoveRange(chiTietTras);

                    foreach (var mpt in maPhieuTras)
                    {
                        bool hasOther = _context.ChiTietPhieuTras.Any(ct => ct.MaPhieuTra == mpt && ct.MaPhieuMuon != maPhieuMuon);
                        if (!hasOther)
                        {
                            var pt = _context.PhieuTras.FirstOrDefault(p => p.MaPhieuTra == mpt);
                            if (pt != null) _context.PhieuTras.Remove(pt);
                        }
                    }
                }

                // Trả lại trạng thái cho các bản sao băng
                if (phieu.ChiTietPhieuMuons != null)
                {
                    foreach (var ct in phieu.ChiTietPhieuMuons)
                    {
                        var bang = _context.BanSaoBangs.FirstOrDefault(b => b.MaBanSao == ct.MaBanSao);
                        if (bang != null)
                        {
                            bang.TrangThai = Constants.TrangThaiBang_SanSang;
                        }
                        _context.ChiTietPhieuMuons.Remove(ct);
                    }
                }

                _context.PhieuMuons.Remove(phieu);
                _context.SaveChanges();
                transaction.Commit();
                return (true, "Xóa phiếu mượn thành công!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, $"Lỗi xóa phiếu mượn: {ex.Message}");
            }
        }
    }
}

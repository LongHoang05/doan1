using System;
using System.Linq;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Helpers;

namespace QuanLyThueBang.BLL
{
    /// <summary>
    /// Tầng Business Logic Layer (BLL) - Xử lý nghiệp vụ Mượn - Trả Băng (Sử dụng Dependency Injection)
    /// </summary>
    public class MuonTraService
    {
        private readonly QuanLyThueBangContext _context;

        public MuonTraService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Kiểm tra bản sao băng có sẵn sàng cho mượn hay không
        /// </summary>
        public (bool IsValid, string ErrorMessage) ValidateBangForMuon(string maBanSao)
        {
            var bang = _context.BanSaoBangs.FirstOrDefault(b => b.MaBanSao == maBanSao);
            if (bang == null)
            {
                return (false, $"Bản sao băng {maBanSao} không tồn tại.");
            }

            if (bang.TrangThai != Constants.TrangThaiBang_SanSang)
            {
                return (false, $"Bản sao băng {maBanSao} đang ở trạng thái: {bang.TrangThai}. Không thể cho mượn.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Cập nhật vị trí hiện tại của cuốn băng khi khách trả tại cửa hàng khác nơi mượn (Cross-Store)
        /// </summary>
        public bool CapNhatViTriBangKhiTra(string maBanSao, string maCuaHangNhanTra)
        {
            var bang = _context.BanSaoBangs.FirstOrDefault(b => b.MaBanSao == maBanSao);
            if (bang == null) return false;

            bang.TrangThai = Constants.TrangThaiBang_SanSang;
            bang.MaCuaHangHienTai = maCuaHangNhanTra;

            return _context.SaveChanges() > 0;
        }
    }
}

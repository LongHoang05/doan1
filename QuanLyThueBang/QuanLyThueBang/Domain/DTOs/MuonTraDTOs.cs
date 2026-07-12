using System;

namespace QuanLyThueBang.Domain.DTOs
{
    public class ChiTietGioMuonDTO
    {
        public string MaBanSao { get; set; } = string.Empty;
        public string RFID { get; set; } = string.Empty;
        public string TuaDe { get; set; } = string.Empty;
        public string TenTheLoai { get; set; } = string.Empty;
        public decimal DonGiaThue { get; set; }
    }

    public class ThongTinBangMuonChuaTraDTO
    {
        public string MaPhieuMuon { get; set; } = string.Empty;
        public string MaBanSao { get; set; } = string.Empty;
        public string RFID { get; set; } = string.Empty;
        public string TuaDe { get; set; } = string.Empty;
        public string MaKhachHang { get; set; } = string.Empty;
        public string HoTenKhachHang { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgayMuon { get; set; }
        public DateTime NgayDuKienTra { get; set; }
        public int SoNgayTreHan { get; set; }
        public bool IsTreHan { get; set; }
        public decimal DonGiaThue { get; set; }
        public string TinhTrangKhiTra { get; set; } = "Bình thường";
        public decimal TienPhat { get; set; } = 0;
        public decimal TienThue => DonGiaThue;
        public decimal TongTien => TienThue + TienPhat;
    }

    public class BangQuaHanDTO
    {
        public string MaPhieuMuon { get; set; } = string.Empty;
        public string MaBanSao { get; set; } = string.Empty;
        public string RFID { get; set; } = string.Empty;
        public string TuaDe { get; set; } = string.Empty;
        public string MaKhachHang { get; set; } = string.Empty;
        public string HoTenKhachHang { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public DateTime NgayMuon { get; set; }
        public DateTime NgayDuKienTra { get; set; }
        public int SoNgayTreHan { get; set; }
    }

    public class PhieuMuonViewDTO
    {
        public string MaPhieuMuon { get; set; } = string.Empty;
        public string MaKhachHang { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string TenCuaHang { get; set; } = string.Empty;
        public string TenNhanVien { get; set; } = string.Empty;
        public DateTime NgayMuon { get; set; }
        public DateTime NgayDuKienTra { get; set; }
        public int SoLuongBang { get; set; }
        public string TrangThaiPhieu { get; set; } = string.Empty;
    }

    public class ChiTietPhieuMuonViewDTO
    {
        public string MaBanSao { get; set; } = string.Empty;
        public string TuaDe { get; set; } = string.Empty;
        public string TenTheLoai { get; set; } = string.Empty;
        public decimal DonGiaThue { get; set; }
        public string TrangThaiTra { get; set; } = string.Empty;
    }
}

namespace QuanLyThueBang.Helpers
{
    /// <summary>
    /// Các hằng số chuẩn trong toàn bộ ứng dụng Quản Lý Thuê Băng
    /// </summary>
    public static class Constants
    {
        // Trạng thái bản sao băng
        public const string TrangThaiBang_SanSang = "Sẵn sàng";
        public const string TrangThaiBang_DangChoMuon = "Đang cho mượn";
        public const string TrangThaiBang_BaoTri = "Bảo trì";
        public const string TrangThaiBang_HuHong = "Hư hỏng";
        public const string TrangThaiBang_ThatLac = "Thất lạc";

        // Tình trạng băng khi trả
        public const string TinhTrangTra_BinhThuong = "Bình thường";
        public const string TinhTrangTra_HongVo = "Hỏng vỏ";
        public const string TinhTrangTra_DutBang = "Đứt băng";
        public const string TinhTrangTra_MatBang = "Mất băng";

        // Vai trò nhân sự
        public const string VaiTro_AdminCapCao = "Admin_CapCao";
        public const string VaiTro_QuanLyChiNhanh = "QuanLy_ChiNhanh";
        public const string VaiTro_NhanVienQuay = "NhanVien_Quay";
    }
}

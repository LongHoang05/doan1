using QuanLyThueBang.Models;

namespace QuanLyThueBang.Helpers
{
    /// <summary>
    /// Lưu thông tin phiên đăng nhập hiện tại của người dùng trong hệ thống
    /// </summary>
    public static class AppSession
    {
        public static NhanVien? CurrentUser { get; set; }

        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// Kiểm tra có phải Admin cấp cao quản lý toàn chuỗi hay không
        /// </summary>
        public static bool IsAdmin => CurrentUser != null && string.IsNullOrEmpty(CurrentUser.MaCuaHang);

        /// <summary>
        /// Mã cửa hàng của nhân viên đang làm việc
        /// </summary>
        public static string? CurrentMaCuaHang => CurrentUser?.MaCuaHang;

        public static bool WantsToLogout { get; set; } = false;

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}

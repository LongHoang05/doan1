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

        public static bool IsAdmin
        {
            get
            {
                if (CurrentUser == null) return false;
                if (string.Equals(CurrentUser.TenDangNhap, "admin", System.StringComparison.OrdinalIgnoreCase)) return true;
                if (CurrentUser.VaiTro?.TenVaiTro != null && CurrentUser.VaiTro.TenVaiTro.Contains("Admin", System.StringComparison.OrdinalIgnoreCase)) return true;
                if (CurrentUser.VaiTro?.TenVaiTro != null && (CurrentUser.VaiTro.TenVaiTro.Contains("Quản lý", System.StringComparison.OrdinalIgnoreCase) || CurrentUser.VaiTro.TenVaiTro.Contains("Nhân viên", System.StringComparison.OrdinalIgnoreCase))) return false;
                return string.IsNullOrEmpty(CurrentUser.MaCuaHang);
            }
        }

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

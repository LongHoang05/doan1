using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Helpers;
using QuanLyThueBang.Models;

namespace QuanLyThueBang.BLL
{
    /// <summary>
    /// Tầng Business Logic Layer (BLL) - Xử lý nghiệp vụ xác thực & phân quyền (Sử dụng Dependency Injection)
    /// </summary>
    public class AuthService
    {
        private readonly QuanLyThueBangContext _context;

        public AuthService(QuanLyThueBangContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Đăng nhập hệ thống bằng tên đăng nhập và mật khẩu
        /// </summary>
        public (bool Success, string Message, NhanVien? NhanVien) Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Tên đăng nhập và mật khẩu không được để trống.", null);
            }

            var user = _context.NhanViens
                .Include(nv => nv.VaiTro)
                .Include(nv => nv.CuaHang)
                .FirstOrDefault(nv => nv.TenDangNhap == username);

            if (user == null)
            {
                return (false, "Tên đăng nhập không tồn tại trong hệ thống.", null);
            }

            // Kiểm tra mật khẩu (hỗ trợ cả mật khẩu băm SHA-256 hoặc so sánh trực tiếp)
            bool isMatch = SecurityHelper.VerifyPassword(password, user.MatKhau) || string.Equals(password, user.MatKhau);
            if (!isMatch)
            {
                return (false, "Mật khẩu không chính xác.", null);
            }

            AppSession.CurrentUser = user;
            return (true, "Đăng nhập thành công!", user);
        }

        public void Logout()
        {
            AppSession.Logout();
        }
    }
}

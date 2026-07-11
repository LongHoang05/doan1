using System;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyThueBang.Helpers
{
    /// <summary>
    /// Tiện ích mã hóa & băm mật khẩu
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Băm mật khẩu bằng SHA-256
        /// </summary>
        public static string HashPassword(string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Kiểm tra mật khẩu khớp với chuỗi đã băm hay không
        /// </summary>
        public static bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            string hashedInput = HashPassword(rawPassword);
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}

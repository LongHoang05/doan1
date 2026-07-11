namespace QuanLyThueBang.Helpers
{
    /// <summary>
    /// Quản lý cấu hình chung cho ứng dụng (Chuỗi kết nối EF Core, cài đặt hệ thống)
    /// </summary>
    public static class AppConfig
    {
        // Cấu hình chuẩn cho SQL Server LocalDB (MSSQLLocalDB) hoặc thay đổi Server theo máy của bạn
        private static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=QuanLyThueBang;Integrated Security=True;TrustServerCertificate=True;";

        public static string ConnectionString
        {
            get => connectionString;
            set => connectionString = value;
        }
    }
}

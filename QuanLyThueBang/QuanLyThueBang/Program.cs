using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuanLyThueBang.BLL;
using QuanLyThueBang.DAL;
using QuanLyThueBang.Helpers;

namespace QuanLyThueBang
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application with Dependency Injection container.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Cấu hình N-Tier Dependency Injection Container (Microsoft.Extensions.Hosting)
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 1. Đăng ký DAL - DbContext
                    services.AddDbContext<QuanLyThueBangContext>(options =>
                        options.UseSqlServer(AppConfig.ConnectionString));

                    // 2. Đăng ký BLL - Các Services nghiệp vụ
                    services.AddScoped<AuthService>();
                    services.AddScoped<MuonTraService>();
                    services.AddScoped<PhimService>();
                    services.AddScoped<BanSaoBangService>();
                    services.AddScoped<CuaHangService>();
                    services.AddScoped<KhachHangService>();
                    services.AddScoped<NhanVienService>();

                    // 3. Đăng ký Presentation - Các Forms
                    services.AddTransient<Presentation.Forms.LoginForm>();
                    services.AddTransient<Presentation.Forms.MainShellForm>();
                })
                .Build();

            // Khởi tạo CSDL & tạo 2 tài khoản mặc định admin / quanly với mật khẩu "1"
            DbSeeder.SeedDefaultUsers(host.Services);

            // Hiển thị màn hình Đăng nhập (LoginForm) trước
            using (var loginForm = host.Services.GetRequiredService<Presentation.Forms.LoginForm>())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = host.Services.GetRequiredService<Presentation.Forms.MainShellForm>();
                    System.Windows.Forms.Application.Run(mainForm);
                }
            }
        }
    }
}
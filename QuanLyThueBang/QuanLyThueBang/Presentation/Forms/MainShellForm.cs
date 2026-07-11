using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using QuanLyThueBang.BLL;
using QuanLyThueBang.Presentation.Controls;

namespace QuanLyThueBang.Presentation.Forms
{
    /// <summary>
    /// Khung giao diện chính (Main Shell Form) chứa Sidebar cố định bên trái
    /// và nhúng các màn hình con vào vùng nội dung chính bên phải.
    /// </summary>
    public class MainShellForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private Panel pnlSidebar = null!;
        private Panel pnlContentContainer = null!;
        private Button? currentActiveButton = null;

        public MainShellForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            InitializeComponent();
            this.Load += MainShellForm_Load;
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ thống Quản Lý Thuê Băng Đĩa - Rental Manager Enterprise";
            this.Size = new Size(1350, 800);
            this.MinimumSize = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(248, 249, 250);

            BuildSidebar();
            BuildContentContainer();
        }

        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.White
            };

            // Viền phân cách phải mỏng
            var pnlBorderRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 1,
                BackColor = Color.FromArgb(230, 235, 240)
            };
            pnlSidebar.Controls.Add(pnlBorderRight);

            // Logo Header
            var pnlLogo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                Padding = new Padding(25, 20, 20, 10)
            };

            var lblAppTitle = new Label
            {
                Text = "Rental Manager",
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 20),
                AutoSize = true
            };

            var lblAppSubtitle = new Label
            {
                Text = "Hệ thống quản lý băng đĩa",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 50),
                AutoSize = true
            };

            pnlLogo.Controls.Add(lblAppTitle);
            pnlLogo.Controls.Add(lblAppSubtitle);
            pnlSidebar.Controls.Add(pnlLogo);

            // Các nút Navigation Menu
            var pnlMenu = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 10, 15, 10),
                AutoScroll = true
            };

            int topPos = 5;

            // Nhóm 1: TRANG CHỦ
            AddSectionHeader(pnlMenu, "TỔNG QUAN", ref topPos);
            topPos = AddMenuButton(pnlMenu, "🏠 Bảng điều khiển", topPos, (s, e) => ShowComingSoon("Bảng điều khiển"));

            // Nhóm 2: QUẢN LÝ DANH MỤC & KHO
            AddSectionHeader(pnlMenu, "QUẢN LÝ PHIM & KHO", ref topPos);
            topPos = AddMenuButton(pnlMenu, "🎬 Quản lý Phim", topPos, BtnMenuPhim_Click, isDefaultActive: true);
            topPos = AddMenuButton(pnlMenu, "🏷️ Quản lý Danh mục", topPos, BtnMenuDanhMuc_Click);
            topPos = AddMenuButton(pnlMenu, "📼 Quản lý Bản sao", topPos, BtnMenuBanSao_Click);
            topPos = AddMenuButton(pnlMenu, "🏪 Quản lý Cửa hàng", topPos, BtnMenuCuaHang_Click);

            // Nhóm 3: QUẢN LÝ NGHIỆP VỤ
            AddSectionHeader(pnlMenu, "NGHIỆP VỤ THUÊ BĂNG", ref topPos);
            topPos = AddMenuButton(pnlMenu, "👥 Quản lý Khách hàng", topPos, BtnMenuKhachHang_Click);
            topPos = AddMenuButton(pnlMenu, "🧑‍💼 Quản lý Nhân viên", topPos, BtnMenuNhanVien_Click);
            topPos = AddMenuButton(pnlMenu, "🔄 Mượn - Trả Băng", topPos, (s, e) => ShowComingSoon("Giao dịch Mượn - Trả"));

            // Nhóm 4: HỆ THỐNG
            AddSectionHeader(pnlMenu, "HỆ THỐNG", ref topPos);
            topPos = AddMenuButton(pnlMenu, "⚙️ Cài đặt Hệ thống", topPos, (s, e) => ShowComingSoon("Cài đặt"));

            pnlSidebar.Controls.Add(pnlMenu);
            this.Controls.Add(pnlSidebar);
        }

        private void AddSectionHeader(Panel pnlMenu, string title, ref int topPos)
        {
            var lblSection = new Label
            {
                Text = title,
                Location = new Point(18, topPos + 6),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 148, 158)
            };
            pnlMenu.Controls.Add(lblSection);
            topPos += 30;
        }

        private int AddMenuButton(Panel pnlMenu, string text, int topPos, EventHandler onClick, bool isDefaultActive = false)
        {
            var btn = new Button
            {
                Text = "   " + text,
                Location = new Point(15, topPos),
                Size = new Size(228, 40),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            if (isDefaultActive)
            {
                SetActiveMenuButton(btn);
            }
            else
            {
                btn.BackColor = Color.Transparent;
                btn.ForeColor = Color.FromArgb(73, 80, 87);
            }

            btn.Click += (s, e) =>
            {
                SetActiveMenuButton(btn);
                onClick(s, e);
            };

            pnlMenu.Controls.Add(btn);
            return topPos + 44;
        }

        private void SetActiveMenuButton(Button btn)
        {
            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.Transparent;
                currentActiveButton.ForeColor = Color.FromArgb(73, 80, 87);
                currentActiveButton.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            }

            currentActiveButton = btn;
            currentActiveButton.BackColor = Color.FromArgb(244, 234, 235); // Màu highlight nhạt ấm áp giống mockup
            currentActiveButton.ForeColor = Color.FromArgb(184, 123, 125);
            currentActiveButton.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
        }

        private void BuildContentContainer()
        {
            pnlContentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            this.Controls.Add(pnlContentContainer);
            pnlContentContainer.BringToFront();
        }

        public void LoadSubControl(UserControl control)
        {
            pnlContentContainer.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(control);
        }

        private void MainShellForm_Load(object? sender, EventArgs e)
        {
            // Mặc định khởi chạy vào màn hình Quản lý Phim
            BtnMenuPhim_Click(this, EventArgs.Empty);
        }

        private void BtnMenuPhim_Click(object? sender, EventArgs e)
        {
            var phimService = _serviceProvider.GetRequiredService<PhimService>();
            var phimControl = new QuanLyPhimControl(phimService);
            LoadSubControl(phimControl);
        }

        private void BtnMenuDanhMuc_Click(object? sender, EventArgs e)
        {
            var phimService = _serviceProvider.GetRequiredService<PhimService>();
            var danhMucControl = new QuanLyDanhMucControl(phimService);
            LoadSubControl(danhMucControl);
        }

        private void BtnMenuBanSao_Click(object? sender, EventArgs e)
        {
            var banSaoService = _serviceProvider.GetRequiredService<BanSaoBangService>();
            var phimService = _serviceProvider.GetRequiredService<PhimService>();
            var banSaoControl = new QuanLyBanSaoControl(banSaoService, phimService);
            LoadSubControl(banSaoControl);
        }

        private void BtnMenuCuaHang_Click(object? sender, EventArgs e)
        {
            var chService = _serviceProvider.GetRequiredService<CuaHangService>();
            var control = new QuanLyCuaHangControl(chService);
            LoadSubControl(control);
        }

        private void BtnMenuKhachHang_Click(object? sender, EventArgs e)
        {
            var khService = _serviceProvider.GetRequiredService<KhachHangService>();
            var control = new QuanLyKhachHangControl(khService);
            LoadSubControl(control);
        }

        private void BtnMenuNhanVien_Click(object? sender, EventArgs e)
        {
            var nvService = _serviceProvider.GetRequiredService<NhanVienService>();
            var chService = _serviceProvider.GetRequiredService<CuaHangService>();
            var control = new QuanLyNhanVienControl(nvService, chService);
            LoadSubControl(control);
        }

        private void ShowComingSoon(string moduleName)
        {
            MessageBox.Show($"Chức năng '{moduleName}' sẽ được tải vào khung này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

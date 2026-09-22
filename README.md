# 🎬 Hệ thống Quản lý Chuỗi Cửa hàng Cho thuê Băng đĩa

> **Đồ án môn học** — Ứng dụng Windows Forms (.NET 8) quản lý toàn diện hoạt động cho thuê băng đĩa phim cho chuỗi nhiều cửa hàng.

---

## 📋 Mô tả dự án

Phần mềm hỗ trợ quản lý nghiệp vụ **cho thuê băng đĩa phim** tại một chuỗi cửa hàng, bao gồm:

- **Quản lý Phim & Thể loại** — Thêm/sửa/xóa tựa phim, phân loại theo thể loại
- **Quản lý Bản sao băng** — Theo dõi từng cuốn băng vật lý (mã vạch, trạng thái, đơn giá thuê, cửa hàng hiện tại)
- **Quản lý Khách hàng** — Thông tin khách hàng, lịch sử mượn trả
- **Quản lý Nhân viên** — Phân quyền theo vai trò (Admin cấp cao / Quản lý chi nhánh / Nhân viên quầy)
- **Quản lý Cửa hàng (Chi nhánh)** — Thông tin chuỗi cửa hàng
- **Mượn - Trả băng** — Tạo phiếu mượn, phiếu trả, tính tiền thuê theo ngày, báo hỏng/mất
- **Dashboard & Báo cáo** — Thống kê doanh thu, biểu đồ trực quan (ScottPlot)
- **Xuất hóa đơn** — In hóa đơn mượn/trả băng
- **Đăng nhập & Phân quyền** — Bảo mật mật khẩu (SHA-256), phân quyền chức năng theo vai trò

---

## 🏗️ Kiến trúc

Dự án áp dụng kiến trúc **N-Tier (3 tầng)** kết hợp **Dependency Injection** (Microsoft.Extensions.Hosting):

```
┌─────────────────────────────────────────────────┐
│           Presentation Layer (UI)               │
│   WinForms: Forms, Controls, Dialogs           │
├─────────────────────────────────────────────────┤
│         Business Logic Layer (BLL)              │
│   Services: AuthService, MuonTraService, ...    │
├─────────────────────────────────────────────────┤
│         Data Access Layer (DAL)                 │
│   EF Core 8 — QuanLyThueBangContext             │
│   SQL Server (LocalDB)                          │
└─────────────────────────────────────────────────┘
```

### Cấu trúc thư mục

```
doan1/
├── Database/                       # Script SQL
│   ├── CreateDatabase.sql          # Tạo cấu trúc CSDL
│   └── SeedData.sql                # Dữ liệu mẫu
├── QuanLyThueBang/                 # Solution chính
│   ├── QuanLyThueBang.sln          # Solution file
│   ├── QuanLyThueBang/             # Project chính
│   │   ├── Program.cs              # Entry point + DI Container
│   │   ├── BLL/                    # Tầng nghiệp vụ (Business Logic)
│   │   │   ├── AuthService.cs      # Đăng nhập, xác thực
│   │   │   ├── MuonTraService.cs   # Nghiệp vụ mượn - trả băng
│   │   │   ├── PhimService.cs      # Quản lý phim
│   │   │   ├── BanSaoBangService.cs# Quản lý bản sao băng
│   │   │   ├── CuaHangService.cs   # Quản lý cửa hàng
│   │   │   ├── KhachHangService.cs # Quản lý khách hàng
│   │   │   └── NhanVienService.cs  # Quản lý nhân viên
│   │   ├── DAL/                    # Tầng truy cập dữ liệu
│   │   │   └── QuanLyThueBangContext.cs  # EF Core DbContext
│   │   ├── Domain/                 # Model & DTO
│   │   │   ├── Entities/           # Entity classes (11 bảng)
│   │   │   ├── DTOs/               # Data Transfer Objects
│   │   │   └── Enums/              # Enum (TrangThaiBang, VaiTro)
│   │   ├── Presentation/          # Tầng giao diện
│   │   │   ├── Forms/              # Windows Forms
│   │   │   └── Controls/           # UserControls (trang chức năng)
│   │   └── Helpers/               # Tiện ích
│   │       ├── AppConfig.cs        # Chuỗi kết nối DB
│   │       ├── AppSession.cs       # Session đăng nhập
│   │       ├── Constants.cs        # Hằng số hệ thống
│   │       ├── DbSeeder.cs         # Seed dữ liệu mặc định
│   │       ├── ExportHelper.cs     # Xuất hóa đơn/báo cáo
│   │       └── SecurityHelper.cs   # Mã hóa mật khẩu SHA-256
│   └── QuanLyThueBang.Tests/      # Unit Tests
├── .gitignore
└── README.md
```

---

## ⚙️ Yêu cầu hệ thống

| Thành phần | Phiên bản tối thiểu |
|---|---|
| **.NET SDK** | 8.0 trở lên |
| **Visual Studio** | 2022 (v17.8+) với workload *".NET Desktop Development"* |
| **SQL Server** | LocalDB (đi kèm VS) hoặc SQL Server Express/Developer |
| **Hệ điều hành** | Windows 10/11 |

---

## 🚀 Hướng dẫn cài đặt & chạy

### Cách 1: Chạy bằng Visual Studio (Khuyến nghị)

1. **Clone repository**
   ```bash
   git clone https://github.com/LongHoang05/doan1.git
   cd doan1
   ```

2. **Mở Solution**
   - Mở file `QuanLyThueBang/QuanLyThueBang.sln` bằng Visual Studio 2022

3. **Restore NuGet packages**
   - Visual Studio sẽ tự động restore, hoặc chạy:
   ```
   Menu: Tools → NuGet Package Manager → Restore NuGet Packages
   ```

4. **Cấu hình chuỗi kết nối** (nếu cần)
   - Mở file `QuanLyThueBang/Helpers/AppConfig.cs`
   - Mặc định sử dụng SQL Server LocalDB:
     ```csharp
     Server=(localdb)\MSSQLLocalDB;Database=QuanLyThueBang;Integrated Security=True;
     ```
   - Nếu dùng SQL Server instance khác, thay đổi `Server=` cho phù hợp

5. **Chạy ứng dụng**
   - Nhấn **F5** hoặc **Ctrl+F5** để build & chạy
   - Ứng dụng sẽ tự động tạo database và seed dữ liệu mẫu khi chạy lần đầu (EF Core Code-First)

### Cách 2: Chạy bằng .NET CLI

```bash
# Clone repo
git clone https://github.com/LongHoang05/doan1.git
cd doan1/QuanLyThueBang

# Restore dependencies
dotnet restore

# Build
dotnet build

# Chạy ứng dụng
dotnet run --project QuanLyThueBang
```

### (Tùy chọn) Tạo CSDL thủ công bằng SQL Script

Nếu muốn tạo database thủ công thay vì dùng EF Core Code-First:

1. Mở **SQL Server Management Studio (SSMS)** hoặc **Azure Data Studio**
2. Kết nối tới SQL Server instance của bạn
3. Chạy lần lượt:
   - `Database/CreateDatabase.sql` — Tạo cấu trúc bảng
   - `Database/SeedData.sql` — Thêm dữ liệu mẫu

---

## 🔐 Tài khoản mặc định

Ứng dụng tự động tạo các tài khoản mặc định khi khởi chạy lần đầu:

| Tên đăng nhập | Mật khẩu | Vai trò |
|---|---|---|
| `admin` | `1` | Admin cấp cao (toàn quyền) |
| `quanly` | `1` | Quản lý chi nhánh |

> ⚠️ **Lưu ý**: Hãy đổi mật khẩu sau khi đăng nhập lần đầu trong môi trường thực tế.

---

## 📦 Công nghệ sử dụng

| Công nghệ | Mô tả |
|---|---|
| **C# / .NET 8** | Ngôn ngữ lập trình & framework chính |
| **Windows Forms** | Framework giao diện desktop |
| **Entity Framework Core 8** | ORM, Code-First migrations |
| **SQL Server LocalDB** | Cơ sở dữ liệu |
| **Microsoft.Extensions.Hosting** | Dependency Injection container |
| **ScottPlot 5** | Thư viện vẽ biểu đồ thống kê |
| **SHA-256** | Mã hóa mật khẩu |

---

## 📊 Sơ đồ CSDL (ERD tóm tắt)

```
VaiTro ──┐
         ├──> NhanVien ──> PhieuMuon ──> ChiTietPhieuMuon
CuaHang ─┘                PhieuTra  ──> ChiTietPhieuTra
                                              │
TheLoai ──> Phim ──> BanSaoBang ──────────────┘
                          │
KhachHang ────────> PhieuMuon / PhieuTra
```

**11 bảng chính**: VaiTro, CuaHang, NhanVien, KhachHang, TheLoai, Phim, BanSaoBang, PhieuMuon, ChiTietPhieuMuon, PhieuTra, ChiTietPhieuTra

---

## 👥 Tác giả

- **LongHoang05** — [GitHub](https://github.com/LongHoang05)

---

## 📄 Giấy phép

Dự án phục vụ mục đích học tập (đồ án môn học).



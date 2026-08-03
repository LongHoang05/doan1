# Cấu trúc Dự án & Mô tả ERD — Hệ thống Quản lý Cho thuê Băng Video

---

## 1. Cấu trúc thư mục dự án

```
DOANMOT/
├── Database/
│   ├── CreateDatabase.sql
│   └── SeedData.sql
│
├── Giao diện/                      # Mockup giao diện (ảnh)
│   ├── Đăng nhập/
│   ├── Tổng quát/
│   ├── Quản lý danh mục/
│   ├── Quản lý phim/
│   ├── Quản lý bản sao/
│   ├── Quản lý khách hàng/
│   ├── Quản lý nhân viên/
│   ├── Quản lý chi nhánh mới/
│   ├── Quản lý mượn trả/
│   └── Chi tiết phiếu mượn/
│
├── QuanLyThueBang/                 # Solution .NET
│   ├── QuanLyThueBang.sln
│   └── QuanLyThueBang/
│       ├── Program.cs
│       ├── QuanLyThueBang.csproj
│       │
│       ├── Domain/                 # TẦNG DOMAIN
│       │   ├── Entities/
│       │   │   ├── VaiTro.cs
│       │   │   ├── CuaHang.cs
│       │   │   ├── NhanVien.cs
│       │   │   ├── KhachHang.cs
│       │   │   ├── TheLoai.cs
│       │   │   ├── Phim.cs
│       │   │   ├── BanSaoBang.cs
│       │   │   ├── PhieuMuon.cs
│       │   │   ├── ChiTietPhieuMuon.cs
│       │   │   ├── PhieuTra.cs
│       │   │   └── ChiTietPhieuTra.cs
│       │   ├── DTOs/
│       │   │   ├── CuaHangDTO.cs
│       │   │   ├── NhanVienDTO.cs
│       │   │   ├── KhachHangDTO.cs
│       │   │   ├── TheLoaiDTO.cs
│       │   │   ├── PhimDTO.cs
│       │   │   ├── BanSaoBangViewDTO.cs
│       │   │   └── MuonTraDTOs.cs
│       │   └── Enums/
│       │       └── TrangThaiBang.cs
│       │
│       ├── DAL/                    # TẦNG DATA ACCESS
│       │   └── QuanLyThueBangContext.cs
│       │
│       ├── BLL/                    # TẦNG BUSINESS LOGIC
│       │   ├── AuthService.cs
│       │   ├── CuaHangService.cs
│       │   ├── NhanVienService.cs
│       │   ├── KhachHangService.cs
│       │   ├── PhimService.cs
│       │   ├── BanSaoBangService.cs
│       │   └── MuonTraService.cs
│       │
│       ├── Presentation/           # TẦNG GIAO DIỆN
│       │   ├── Forms/
│       │   │   ├── LoginForm.cs
│       │   │   ├── MainShellForm.cs
│       │   │   ├── DanhMuc/
│       │   │   ├── Phim/
│       │   │   ├── BanSao/
│       │   │   ├── KhachHang/
│       │   │   ├── NhanVien/
│       │   │   ├── CuaHang/
│       │   │   ├── MuonTra/
│       │   │   └── HoaDon/
│       │   └── Controls/
│       │       ├── DashboardControl.cs
│       │       ├── QuanLyDanhMucControl.cs
│       │       ├── QuanLyPhimControl.cs
│       │       ├── QuanLyBanSaoControl.cs
│       │       ├── QuanLyKhachHangControl.cs
│       │       ├── QuanLyNhanVienControl.cs
│       │       ├── QuanLyCuaHangControl.cs
│       │       ├── QuanLyPhieuMuonControl.cs
│       │       └── MuonTraBangControl.cs
│       │
│       └── Helpers/
│           ├── AppConfig.cs
│           ├── AppSession.cs
│           ├── Constants.cs
│           ├── DbSeeder.cs
│           ├── ExportHelper.cs
│           └── SecurityHelper.cs
│
├── TestCase_ThemPhim.xlsx
└── BaoCao_DoAn1.md
```

**Kiến trúc**: 3-Layer (Presentation → BLL → DAL) + Domain  
**Công nghệ**: C# WinForms, EF Core 8 Code-First, SQL Server

---

## 2. Mô tả ERD — 11 bảng

### 2.1. VaiTro

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaVaiTro** | `int` | PK, Identity | Mã vai trò (tự tăng) |
| TenVaiTro | `nvarchar(50)` | NOT NULL | Admin / Quản lý / Nhân viên |

---

### 2.2. CuaHang

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaCuaHang** | `varchar(10)` | PK | Mã cửa hàng (VD: CH01) |
| DiaChi | `nvarchar(255)` | NOT NULL | Địa chỉ chi nhánh |
| SoDienThoai | `varchar(15)` | NOT NULL | Số điện thoại |

---

### 2.3. NhanVien

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaNhanVien** | `varchar(20)` | PK | Mã nhân viên (VD: ADM01, QL01, NVQ01) |
| CMND | `varchar(12)` | NOT NULL | Chứng minh nhân dân |
| HoTen | `nvarchar(100)` | NOT NULL | Họ tên |
| DiaChi | `nvarchar(255)` | NULL | Địa chỉ |
| SoDienThoai | `varchar(15)` | NULL | Số điện thoại |
| TenDangNhap | `varchar(50)` | NOT NULL | Tên đăng nhập |
| MatKhau | `varchar(255)` | NOT NULL | Mật khẩu |
| **MaVaiTro** | `int` | FK → VaiTro, NOT NULL | Vai trò |
| **MaCuaHang** | `varchar(10)` | FK → CuaHang, **NULL** | Cửa hàng (NULL = Admin cấp cao) |

---

### 2.4. KhachHang

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaKhachHang** | `varchar(20)` | PK | Mã khách hàng (VD: KH001) |
| CMND | `varchar(12)` | NOT NULL | Chứng minh nhân dân |
| HoTen | `nvarchar(100)` | NOT NULL | Họ tên |
| DiaChi | `nvarchar(255)` | NULL | Địa chỉ |
| SoDienThoai | `varchar(15)` | NOT NULL | Số điện thoại |
| NgayDangKy | `datetime` | Default: NOW | Ngày đăng ký |

---

### 2.5. TheLoai

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaTheLoai** | `int` | PK, Identity | Mã thể loại (tự tăng) |
| TenTheLoai | `nvarchar(100)` | NOT NULL | Hành Động / Tình Cảm / Hài Hước... |

---

### 2.6. Phim

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhim** | `varchar(20)` | PK | Mã phim (VD: P001) |
| TuaDe | `nvarchar(255)` | NOT NULL | Tựa đề phim |
| NamPhatHanh | `int` | NULL | Năm phát hành |
| DoDaiPhut | `int` | NULL | Độ dài (phút) |
| **MaTheLoai** | `int` | FK → TheLoai, NOT NULL | Thể loại |

---

### 2.7. BanSaoBang

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaBanSao** | `varchar(50)` | PK | Mã bản sao (VD: BS001) |
| **MaPhim** | `varchar(20)` | FK → Phim, NOT NULL | Phim gốc |
| **MaCuaHangHienTai** | `varchar(10)` | FK → CuaHang, NOT NULL | Cửa hàng đang giữ |
| SoThuTuBanSao | `int` | NOT NULL | Số thứ tự bản sao |
| LoaiBang | `varchar(10)` | Default: "PAL" | PAL / NTSC |
| DonGiaThue | `decimal(18,2)` | NOT NULL | Đơn giá cho thuê |
| NgayHetHan | `date` | NULL | Ngày hết hạn |
| TrangThai | `varchar(50)` | Default: "Sẵn sàng" | Sẵn sàng / Đang cho mượn / Bảo trì / Hư hỏng / Thất lạc |

---

### 2.8. PhieuMuon

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuMuon** | `varchar(20)` | PK | Mã phiếu mượn (VD: PM001) |
| **MaKhachHang** | `varchar(20)` | FK → KhachHang, NOT NULL | Khách mượn |
| **MaCuaHangMuon** | `varchar(10)` | FK → CuaHang, NOT NULL | Nơi mượn |
| **MaNhanVienChoMuon** | `varchar(20)` | FK → NhanVien, NOT NULL | NV xử lý |
| NgayMuon | `datetime` | Default: NOW | Ngày mượn |
| NgayDuKienTra | `datetime` | NOT NULL | Ngày dự kiến trả |

---

### 2.9. ChiTietPhieuMuon

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuMuon** | `varchar(20)` | PK₁, FK → PhieuMuon | Phiếu mượn |
| **MaBanSao** | `varchar(50)` | PK₂, FK → BanSaoBang | Bản sao được mượn |
| TrangThaiTra | `bit` | Default: false | 0 = Chưa trả, 1 = Đã trả |

> Khóa chính kép: **(MaPhieuMuon, MaBanSao)**

---

### 2.10. PhieuTra

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuTra** | `varchar(20)` | PK | Mã phiếu trả |
| **MaKhachHang** | `varchar(20)` | FK → KhachHang, NOT NULL | Khách trả |
| **MaCuaHangNhanTra** | `varchar(10)` | FK → CuaHang, NOT NULL | Nơi nhận trả (Cross-Store) |
| **MaNhanVienNhanTra** | `varchar(20)` | FK → NhanVien, NOT NULL | NV xử lý |
| NgayTra | `datetime` | Default: NOW | Ngày trả |
| TongTienThu | `decimal(18,2)` | Default: 0 | Tổng tiền thu |

---

### 2.11. ChiTietPhieuTra

| Cột | Kiểu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuTra** | `varchar(20)` | PK₁, FK → PhieuTra | Phiếu trả |
| **MaBanSao** | `varchar(50)` | PK₂ | Bản sao băng |
| **MaPhieuMuon** | `varchar(20)` | FK kép → ChiTietPhieuMuon | Phiếu mượn gốc |
| TinhTrangBangKhiTra | `nvarchar(100)` | Default: "Bình thường" | Bình thường / Hỏng vỏ / Đứt băng... |
| TienThue | `decimal(18,2)` | Default: 0 | Tiền thuê |
| TienPhat | `decimal(18,2)` | Default: 0 | Tiền phạt trễ hạn |

> Khóa chính kép: **(MaPhieuTra, MaBanSao)**  
> FK kép: **(MaPhieuMuon, MaBanSao)** → ChiTietPhieuMuon

---

## 3. Bảng tổng hợp Quan hệ — 15 FK

| # | Bảng cha | Bảng con | FK tại bảng con | Quan hệ | Delete Behavior |
|---|---|---|---|---|---|
| 1 | VaiTro | NhanVien | `MaVaiTro` | 1 — N | Restrict |
| 2 | CuaHang | NhanVien | `MaCuaHang` | 1 — N (optional) | Restrict |
| 3 | CuaHang | BanSaoBang | `MaCuaHangHienTai` | 1 — N | Restrict |
| 4 | CuaHang | PhieuMuon | `MaCuaHangMuon` | 1 — N | Restrict |
| 5 | CuaHang | PhieuTra | `MaCuaHangNhanTra` | 1 — N | Restrict |
| 6 | TheLoai | Phim | `MaTheLoai` | 1 — N | Cascade |
| 7 | Phim | BanSaoBang | `MaPhim` | 1 — N | Cascade |
| 8 | KhachHang | PhieuMuon | `MaKhachHang` | 1 — N | Cascade |
| 9 | KhachHang | PhieuTra | `MaKhachHang` | 1 — N | Cascade |
| 10 | NhanVien | PhieuMuon | `MaNhanVienChoMuon` | 1 — N | Restrict |
| 11 | NhanVien | PhieuTra | `MaNhanVienNhanTra` | 1 — N | Restrict |
| 12 | PhieuMuon | ChiTietPhieuMuon | `MaPhieuMuon` | 1 — N | Cascade |
| 13 | BanSaoBang | ChiTietPhieuMuon | `MaBanSao` | 1 — N | Restrict |
| 14 | PhieuTra | ChiTietPhieuTra | `MaPhieuTra` | 1 — N | Cascade |
| 15 | ChiTietPhieuMuon | ChiTietPhieuTra | `(MaPhieuMuon, MaBanSao)` | 1 — N | Restrict |

---

## 4. Sơ đồ quan hệ dạng text

```
┌──────────┐
│  VaiTro  │
└────┬─────┘
     │ 1:N
     ▼
┌──────────┐        1:N (optional)        ┌──────────┐
│ NhanVien │◄─────────────────────────────│ CuaHang  │
└──┬────┬──┘                              └─┬──┬──┬──┘
   │    │                                   │  │  │
   │    │ 1:N                          1:N  │  │  │ 1:N
   │    │           ┌───────────────────────┘  │  │
   │    │           │                     1:N  │  │
   │    ▼           ▼                          │  │
   │  ┌───────────────┐                       │  │
   │  │   PhieuMuon   │◄── KhachHang 1:N      │  │
   │  └───────┬───────┘                       │  │
   │          │ 1:N (Cascade)                 │  │
   │          ▼                               │  │
   │  ┌───────────────────┐                   │  │
   │  │ ChiTietPhieuMuon  │◄── BanSaoBang 1:N │  │
   │  │  PK: (MaPhieuMuon,│                   │  │
   │  │       MaBanSao)   │                   │  │
   │  └───────┬───────────┘                   │  │
   │          │ 1:N (Restrict)                │  │
   │          ▼                               │  │
   │  ┌───────────────────┐                   │  │
   │  │  ChiTietPhieuTra  │                   │  │
   │  │  PK: (MaPhieuTra, │                   │  │
   │  │       MaBanSao)   │                   │  │
   │  │  FK: (MaPhieuMuon,│                   │  │
   │  │       MaBanSao)   │                   │  │
   │  └───────▲───────────┘                   │  │
   │          │ 1:N                           │  │
   │  ┌───────┴───────┐                       │  │
   └─▶│   PhieuTra    │◄── KhachHang 1:N      │  │
      └───────────────┘                       │  │
                                              │  │
      ┌──────────┐    1:N    ┌──────────┐     │  │
      │BanSaoBang│◄──────────│ CuaHang  │─────┘  │
      └────┬─────┘           └──────────┘         │
           │ N:1                                  │
           ▼                                      │
      ┌──────────┐    1:N    ┌──────────┐         │
      │   Phim   │◄──────────│ TheLoai  │         │
      └──────────┘           └──────────┘         │
                                                  │
      ┌──────────┐◄───────────────────────────────┘
      │KhachHang │     1:N → PhieuMuon, PhieuTra
      └──────────┘
```

---

## 5. Điểm đặc biệt cần thể hiện trên ERD

1. **Khóa chính kép**: `ChiTietPhieuMuon(MaPhieuMuon, MaBanSao)` và `ChiTietPhieuTra(MaPhieuTra, MaBanSao)`
2. **FK kép**: `ChiTietPhieuTra` → `ChiTietPhieuMuon` qua `(MaPhieuMuon, MaBanSao)`
3. **CuaHang** là hub — được FK bởi 4 bảng: NhanVien, BanSaoBang, PhieuMuon, PhieuTra
4. **NhanVien.MaCuaHang** là optional NULL (Admin cấp cao không thuộc chi nhánh)
5. **Cross-Store**: PhieuTra.MaCuaHangNhanTra có thể khác PhieuMuon.MaCuaHangMuon

---

## 6. Tổng kết

| Nhóm | Bảng | Số lượng |
|---|---|---|
| Hệ thống & Nhân sự | VaiTro, CuaHang, NhanVien, KhachHang | 4 |
| Danh mục & Kho | TheLoai, Phim, BanSaoBang | 3 |
| Nghiệp vụ Mượn–Trả | PhieuMuon, ChiTietPhieuMuon, PhieuTra, ChiTietPhieuTra | 4 |
| **Tổng** | | **11 bảng, 15 quan hệ** |

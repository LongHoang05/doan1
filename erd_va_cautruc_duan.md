# Cấu trúc Dự án & Mô tả ERD — Hệ thống Quản lý Cho thuê Băng Video

---

## 1. Cấu trúc thư mục dự án

```
DOANMOT/
├── Database/
│   ├── CreateDatabase.sql          # Script tạo database SQL Server
│   └── SeedData.sql                # Script dữ liệu mẫu
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
├── QuanLyThueBang/                 # Solution .NET (C# WinForms + EF Core 8)
│   ├── QuanLyThueBang.sln
│   └── QuanLyThueBang/
│       ├── Program.cs              # Entry point
│       ├── QuanLyThueBang.csproj
│       │
│       ├── Domain/                 # ── TẦNG DOMAIN ──
│       │   ├── Entities/           # Các class thực thể (Entity)
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
│       │   ├── DTOs/               # Data Transfer Objects
│       │   │   ├── CuaHangDTO.cs
│       │   │   ├── NhanVienDTO.cs
│       │   │   ├── KhachHangDTO.cs
│       │   │   ├── TheLoaiDTO.cs
│       │   │   ├── PhimDTO.cs
│       │   │   ├── BanSaoBangViewDTO.cs
│       │   │   └── MuonTraDTOs.cs
│       │   └── Enums/
│       │       └── TrangThaiBang.cs # Enum: TrangThaiBang, VaiTroNhanVien
│       │
│       ├── DAL/                    # ── TẦNG DATA ACCESS ──
│       │   └── QuanLyThueBangContext.cs  # DbContext (EF Core 8)
│       │
│       ├── BLL/                    # ── TẦNG BUSINESS LOGIC ──
│       │   ├── AuthService.cs
│       │   ├── CuaHangService.cs
│       │   ├── NhanVienService.cs
│       │   ├── KhachHangService.cs
│       │   ├── PhimService.cs          # Quản lý TheLoai + Phim
│       │   ├── BanSaoBangService.cs
│       │   └── MuonTraService.cs       # Nghiệp vụ Mượn - Trả
│       │
│       ├── Presentation/           # ── TẦNG GIAO DIỆN ──
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
│       │   └── Controls/           # UserControls (giao diện từng module)
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
│       └── Helpers/                # ── TIỆN ÍCH ──
│           ├── AppConfig.cs        # Connection string config
│           ├── AppSession.cs       # Thông tin phiên đăng nhập
│           ├── Constants.cs        # Hằng số
│           ├── DbSeeder.cs         # Tạo dữ liệu mẫu
│           ├── ExportHelper.cs     # Xuất báo cáo
│           └── SecurityHelper.cs   # Mã hoá mật khẩu
│
├── TestCase_ThemPhim.xlsx          # Test cases
└── BaoCao_DoAn1.md                 # Báo cáo đồ án
```

**Kiến trúc**: 3-Layer (Presentation → BLL → DAL) + Domain riêng biệt. Sử dụng **Entity Framework Core 8** (Code-First) trên **SQL Server**.

---

## 2. Mô tả ERD — Danh sách Thực thể (Entity)

> **Lưu ý**: Dự án có **sự khác biệt giữa SQL gốc** ([CreateDatabase.sql](file:///d:/03_Active_Projects/DOANMOT/Database/CreateDatabase.sql)) và **Entity classes** trong code C#. Phần mô tả dưới đây dựa trên **Entity classes (Code-First)** — là bản thực tế đang chạy.

---

### 2.1. VaiTro (Vai Trò)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaVaiTro** | `int` | PK, Identity | Mã vai trò (tự tăng) |
| TenVaiTro | `nvarchar(50)` | NOT NULL | Tên vai trò: `Admin_CapCao`, `QuanLy_ChiNhanh`, `NhanVien_Quay` |

---

### 2.2. CuaHang (Cửa hàng / Chi nhánh)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaCuaHang** | `varchar(10)` | PK | Mã cửa hàng |
| DiaChi | `nvarchar(255)` | NOT NULL | Địa chỉ chi nhánh |
| SoDienThoai | `varchar(15)` | NOT NULL | Số điện thoại |

---

### 2.3. NhanVien (Nhân Viên)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaNhanVien** | `varchar(20)` | PK | Mã nhân viên |
| CMND | `varchar(12)` | NOT NULL | Chứng minh nhân dân |
| HoTen | `nvarchar(100)` | NOT NULL | Họ tên |
| DiaChi | `nvarchar(255)` | NULL | Địa chỉ |
| SoDienThoai | `varchar(15)` | NULL | Số điện thoại |
| TenDangNhap | `varchar(50)` | NOT NULL | Tên đăng nhập |
| MatKhau | `varchar(255)` | NOT NULL | Mật khẩu (đã hash) |
| **MaVaiTro** | `int` | FK → VaiTro | Vai trò nhân viên |
| **MaCuaHang** | `varchar(10)` | FK → CuaHang, NULL | Cửa hàng (NULL = Admin cấp cao) |

---

### 2.4. KhachHang (Khách Hàng)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaKhachHang** | `varchar(20)` | PK | Mã khách hàng |
| CMND | `varchar(12)` | NOT NULL | Chứng minh nhân dân |
| HoTen | `nvarchar(100)` | NOT NULL | Họ tên |
| DiaChi | `nvarchar(255)` | NULL | Địa chỉ |
| SoDienThoai | `varchar(15)` | NOT NULL | Số điện thoại |
| NgayDangKy | `datetime` | Default: NOW | Ngày đăng ký |

---

### 2.5. TheLoai (Thể Loại Phim)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaTheLoai** | `int` | PK, Identity | Mã thể loại (tự tăng) |
| TenTheLoai | `nvarchar(100)` | NOT NULL | Tên thể loại: Hành động, Tình cảm, Hoạt hình... |

---

### 2.6. Phim (Phim Gốc)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhim** | `varchar(20)` | PK | Mã phim |
| TuaDe | `nvarchar(255)` | NOT NULL | Tựa đề phim |
| NamPhatHanh | `int` | NULL | Năm phát hành |
| DoDaiPhut | `int` | NULL | Độ dài (phút) |
| **MaTheLoai** | `int` | FK → TheLoai | Thể loại phim |

---

### 2.7. BanSaoBang (Bản Sao Băng Video)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaBanSao** | `varchar(50)` | PK | Mã bản sao (mã vạch/RFID) |
| **MaPhim** | `varchar(20)` | FK → Phim | Phim gốc |
| **MaCuaHangHienTai** | `varchar(10)` | FK → CuaHang | Cửa hàng đang giữ |
| SoThuTuBanSao | `int` | NOT NULL | Số thứ tự bản sao |
| LoaiBang | `varchar(10)` | Default: "PAL" | Loại băng: PAL, NTSC... |
| DonGiaThue | `decimal(18,2)` | NOT NULL | Đơn giá cho thuê |
| NgayHetHan | `date` | NULL | Ngày hết hạn sử dụng |
| TrangThai | `varchar(50)` | Default: "Sẵn sàng" | Sẵn sàng / Đang cho mượn / Bảo trì / Thất lạc |

---

### 2.8. PhieuMuon (Phiếu Mượn)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuMuon** | `varchar(20)` | PK | Mã phiếu mượn |
| **MaKhachHang** | `varchar(20)` | FK → KhachHang | Khách hàng mượn |
| **MaCuaHangMuon** | `varchar(10)` | FK → CuaHang | Nơi mượn |
| **MaNhanVienChoMuon** | `varchar(20)` | FK → NhanVien | NV xử lý |
| NgayMuon | `datetime` | Default: NOW | Ngày mượn |
| NgayDuKienTra | `datetime` | NOT NULL | Ngày dự kiến trả |

---

### 2.9. ChiTietPhieuMuon (Chi Tiết Phiếu Mượn)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuMuon** | `varchar(20)` | PK, FK → PhieuMuon | Phiếu mượn |
| **MaBanSao** | `varchar(50)` | PK, FK → BanSaoBang | Bản sao được mượn |
| TrangThaiTra | `bit` | Default: false | `0` = Chưa trả, `1` = Đã trả |

> **Khóa chính kép**: (MaPhieuMuon, MaBanSao)

---

### 2.10. PhieuTra (Phiếu Trả)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuTra** | `varchar(20)` | PK | Mã phiếu trả |
| **MaKhachHang** | `varchar(20)` | FK → KhachHang | Khách hàng trả |
| **MaCuaHangNhanTra** | `varchar(10)` | FK → CuaHang | Nơi nhận trả (có thể khác nơi mượn → Cross-Store) |
| **MaNhanVienNhanTra** | `varchar(20)` | FK → NhanVien | NV xử lý |
| NgayTra | `datetime` | Default: NOW | Ngày trả |
| TongTienThu | `decimal(18,2)` | Default: 0 | Tổng tiền thu |

---

### 2.11. ChiTietPhieuTra (Chi Tiết Phiếu Trả)

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|---|---|---|---|
| **MaPhieuTra** | `varchar(20)` | PK, FK → PhieuTra | Phiếu trả |
| **MaBanSao** | `varchar(50)` | PK | Bản sao băng |
| **MaPhieuMuon** | `varchar(20)` | FK kép → ChiTietPhieuMuon | Phiếu mượn gốc |
| TinhTrangBangKhiTra | `nvarchar(100)` | Default: "Bình thường" | Tình trạng: Bình thường, Hỏng vỏ, Đứt băng... |
| TienThue | `decimal(18,2)` | Default: 0 | Tiền thuê |
| TienPhat | `decimal(18,2)` | Default: 0 | Tiền phạt trễ hạn |

> **Khóa chính kép**: (MaPhieuTra, MaBanSao)
> **FK kép**: (MaPhieuMuon, MaBanSao) → ChiTietPhieuMuon — đảm bảo chỉ trả đúng cuốn đã mượn

---

## 3. Mô tả Quan hệ (Relationships) — Để vẽ ERD

### Bảng tổng hợp quan hệ

| # | Bảng cha | Bảng con | FK tại bảng con | Quan hệ | Delete Behavior |
|---|---|---|---|---|---|
| 1 | **VaiTro** | **NhanVien** | `MaVaiTro` | 1 — N | Restrict |
| 2 | **CuaHang** | **NhanVien** | `MaCuaHang` | 1 — N (optional) | Restrict |
| 3 | **CuaHang** | **BanSaoBang** | `MaCuaHangHienTai` | 1 — N | Restrict |
| 4 | **CuaHang** | **PhieuMuon** | `MaCuaHangMuon` | 1 — N | Restrict |
| 5 | **CuaHang** | **PhieuTra** | `MaCuaHangNhanTra` | 1 — N | Restrict |
| 6 | **TheLoai** | **Phim** | `MaTheLoai` | 1 — N | (Default) |
| 7 | **Phim** | **BanSaoBang** | `MaPhim` | 1 — N | (Default) |
| 8 | **KhachHang** | **PhieuMuon** | `MaKhachHang` | 1 — N | (Default) |
| 9 | **KhachHang** | **PhieuTra** | `MaKhachHang` | 1 — N | (Default) |
| 10 | **NhanVien** | **PhieuMuon** | `MaNhanVienChoMuon` | 1 — N | Restrict |
| 11 | **NhanVien** | **PhieuTra** | `MaNhanVienNhanTra` | 1 — N | Restrict |
| 12 | **PhieuMuon** | **ChiTietPhieuMuon** | `MaPhieuMuon` | 1 — N | **Cascade** |
| 13 | **BanSaoBang** | **ChiTietPhieuMuon** | `MaBanSao` | 1 — N | Restrict |
| 14 | **PhieuTra** | **ChiTietPhieuTra** | `MaPhieuTra` | 1 — N | (Default) |
| 15 | **ChiTietPhieuMuon** | **ChiTietPhieuTra** | `(MaPhieuMuon, MaBanSao)` | 1 — N | Restrict |

---

### Sơ đồ quan hệ dạng text (để tham khảo khi vẽ)

```
    ┌──────────┐
    │  VaiTro  │
    └────┬─────┘
         │ 1:N
         ▼
    ┌──────────┐       1:N (optional)       ┌──────────┐
    │ NhanVien │◄───────────────────────────│ CuaHang  │
    └────┬─────┘                            └──┬──┬──┬─┘
         │ 1:N                                 │  │  │
         │          ┌──────────────────────────┘  │  │
         │          │ 1:N                    1:N  │  │ 1:N
         ▼          ▼                             │  │
    ┌──────────┐  ┌──────────┐                    │  │
    │PhieuMuon │  │PhieuTra  │                    │  │
    └────┬─────┘  └────┬─────┘                    │  │
         │ 1:N         │ 1:N                      │  │
         ▼             ▼                          │  │
  ┌──────────────┐  ┌──────────────┐              │  │
  │ChiTietPhieu  │  │ChiTietPhieu  │              │  │
  │    Muon      │◄─│    Tra       │              │  │
  └──────┬───────┘  └──────────────┘              │  │
         │ N:1                                    │  │
         ▼                                        │  │
    ┌──────────┐     1:N     ┌──────────┐         │  │
    │BanSaoBang│◄────────────│  CuaHang │─────────┘  │
    └────┬─────┘             └──────────┘             │
         │ N:1                                        │
         ▼                                            │
    ┌──────────┐     1:N     ┌──────────┐             │
    │   Phim   │◄────────────│ TheLoai  │             │
    └──────────┘             └──────────┘             │
                                                      │
    ┌──────────┐◄─────────────────────────────────────┘
    │KhachHang │──── 1:N ──→ PhieuMuon, PhieuTra
    └──────────┘
```

---

## 4. Gợi ý khi vẽ ERD

> [!TIP]
> - Nên chia ERD thành **3 nhóm** theo màu:
>   - 🟦 **Nhóm Hệ thống & Nhân sự**: VaiTro, CuaHang, NhanVien, KhachHang
>   - 🟩 **Nhóm Danh mục & Kho**: TheLoai, Phim, BanSaoBang
>   - 🟧 **Nhóm Nghiệp vụ Mượn–Trả**: PhieuMuon, ChiTietPhieuMuon, PhieuTra, ChiTietPhieuTra

> [!IMPORTANT]
> **Các điểm đặc biệt cần thể hiện trên ERD:**
> 1. **Khóa chính kép**: `ChiTietPhieuMuon(MaPhieuMuon, MaBanSao)` và `ChiTietPhieuTra(MaPhieuTra, MaBanSao)`
> 2. **FK kép**: `ChiTietPhieuTra` có FK kép `(MaPhieuMuon, MaBanSao)` trỏ về `ChiTietPhieuMuon` — đảm bảo chỉ trả đúng cuốn đã mượn
> 3. **CuaHang** được tham chiếu bởi **4 bảng** (NhanVien, BanSaoBang, PhieuMuon, PhieuTra)
> 4. **NhanVien.MaCuaHang** là **optional** (NULL = Admin cấp cao quản lý toàn chuỗi)
> 5. **Cross-Store Return**: `PhieuTra.MaCuaHangNhanTra` có thể khác `PhieuMuon.MaCuaHangMuon`

> [!NOTE]
> **Công cụ vẽ ERD gợi ý**: draw.io, dbdiagram.io, Lucidchart, hoặc MySQL Workbench (hỗ trợ SQL Server ERD).
> Trên **dbdiagram.io** bạn có thể paste cú pháp DBML để tự generate ERD rất nhanh.

---

## 5. Tổng kết: 11 bảng — 15 quan hệ

| Nhóm | Bảng | Số lượng |
|---|---|---|
| Hệ thống & Nhân sự | VaiTro, CuaHang, NhanVien, KhachHang | 4 |
| Danh mục & Kho | TheLoai, Phim, BanSaoBang | 3 |
| Nghiệp vụ Mượn–Trả | PhieuMuon, ChiTietPhieuMuon, PhieuTra, ChiTietPhieuTra | 4 |
| **Tổng** | | **11 bảng** |

# ✨ KẾ HOẠCH THIẾT KẾ GIAO DIỆN (UI/UX Design Plan) - PREMIUM EDITION

# Hệ Thống Quản Lý Cho Thuê Băng Video (VideoRental Pro)

---

## 1. Triết Lý Thiết Kế (Design Philosophy)

Để phá bỏ định kiến "giao diện WinForms nhàm chán và cũ kỹ", dự án sẽ áp dụng triết lý thiết kế **Modern Desktop App** (tương tự Fluent UI của Windows 11 hoặc Material Design 3). Giao diện phải mang lại cảm giác **cao cấp (Premium), hiện đại (Modern)** và **mượt mà (Dynamic)**.

### 1.1. Khuyến nghị công nghệ UI
Sử dụng bộ thư viện UI hiện đại cho WinForms:
- 🥇 **Guna UI 2 / Siticone UI Framework:** Hỗ trợ bo góc mượt, shadow, gradient, toggle switch, và animation.
- 📊 **LiveCharts.WinForms:** Hỗ trợ vẽ biểu đồ thống kê hiện đại.

### 1.2. Hệ thống màu sắc (Color Palette) - Premium Light Theme

| Token Màu | Mã Hex | Áp dụng cho |
|-----------|--------|-------------|
| **Background** | `#F4F7FE` | Nền chính của ứng dụng (trắng xám nhạt, rất dịu mắt). |
| **Surface/Card** | `#FFFFFF` | Nền của các GroupBox, Panel chứa nội dung. Có kèm Drop Shadow mờ. |
| **Primary (Brand)** | `#4318FF` | Xanh Violet hiện đại. Dùng cho Header, Button chính, Tab active, Link. |
| **Primary Hover** | `#3311DB` | Màu khi hover chuột vào nút Primary. |
| **Success** | `#05CD99` | Xanh ngọc lục bảo. Dùng cho nút Lưu, trạng thái "Có sẵn", tick xanh. |
| **Danger** | `#EE5D50` | Đỏ san hô. Dùng cho nút Xóa, báo lỗi, cảnh báo "Quá hạn". |
| **Warning** | `#FFCE20` | Vàng rực. Dùng cho nút Sửa, trạng thái "Đang mượn". |
| **Text Primary** | `#2B3674` | Chữ tiêu đề, text nổi bật (Tối xanh navy, sang trọng hơn màu đen thuần). |
| **Text Secondary**| `#A3AED0` | Chữ gợi ý, placeholder, chú thích. |

### 1.3. Nghệ thuật Typography (Kiểu chữ) & Hiệu ứng
- **Font chính:** `Segoe UI`, `Inter` hoặc `Roboto`.
- **Rounded Corners (Bo góc):** Tất cả các card, button, textbox đều được bo góc `Radius = 8px` hoặc `12px`.
- **Drop Shadows (Đổ bóng):** Các Card/Panel nổi lên so với nền tạo chiều sâu.

---

## 2. Form Chính — frmMain (Dashboard Hiện Đại)

### 2.1. Bố cục (Sidebar Navigation + Dashboard)

Hệ thống bỏ menu thanh ngang cũ kỹ, thay bằng **Sidebar Navigation** hiện đại bên trái.

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 🎛️ VIDEORENTAL PRO (Không viền - Borderless, bo góc, Shadow)  [_][□][X]│
├──────────────┬─────────────────────────────────────────────────────────┤
│ 👤 Xin chào, │ 🔍 [ Tìm kiếm mã mượn, tên khách hàng...             ]  │
│ Võ T.Phong   │                                                         │
│ ──────────── │ ┌── THỐNG KÊ NHANH (Shadow Cards) ────────────────────┐ │
│ 📁 QUẢN LÝ   │ │ ┌─────────────┐ ┌─────────────┐ ┌─────────────┐     │ │
│ 🏠 Dashboard │ │ │ 👥 Khách    │ │ 🎬 Phim     │ │ 📦 Đang mượn│     │ │
│ 🏪 Cửa hàng  │ │ │    124      │ │    85       │ │    32       │     │ │
│ 👤 Khách hàng│ │ │  (+12% tháng) │ │  (+5% tháng)  │ │  (⚠️ 5 quá hạn) │ │
│ 👔 Nhân viên*│ │ └─────────────┘ └─────────────┘ └─────────────┘     │ │
│ 🎞️ Phim      │ └─────────────────────────────────────────────────────┘ │
│ 📼 Băng video│                                                         │
│ ──────────── │ ┌── BIỂU ĐỒ DOANH THU ──┐ ┌── HOẠT ĐỘNG GẦN ĐÂY ──────┐ │
│ 📋 NGHIỆP VỤ │ │                       │ │ • 14:30: Mượn 2 cuốn      │ │
│ 📤 Mượn băng │ │      [ Biểu đồ ]      │ │ • 13:15: Trả 1 cuốn       │ │
│ 📥 Trả băng  │ │                       │ │ • 11:00: Thêm phim mới    │ │
│ ──────────── │ └───────────────────────┘ └───────────────────────────┘ │
│ 📊 BÁO CÁO   │                                                         │
│ 📉 Thống kê  │ ─────────────────────────────────────────────────────── │
│ ⚠️ Nhắc trả  │  Trạng thái: 🟢 Đã kết nối | 📅 07/07/2026               │
│ ──────────── │                                                         │
│ ⚙️ HỆ THỐNG* │ (*): Chỉ hiển thị khi đăng nhập bằng tài khoản Admin.    │
│ 🛠️ Cấu hình  │                                                         │
│ 📜 Xem Logs  │                                                         │
└──────────────┴─────────────────────────────────────────────────────────┘
```

---

## 3. Các Form Danh Mục (CRUD)
Nguyên tắc: Bỏ layout trên-dưới truyền thống. Dùng layout **30% trái (Input) - 70% phải (Grid)** để tối ưu màn hình Wide. Dùng Guna2TextBox (placeholder mờ, border radius 8px). DataGridView không viền ô, xen kẽ dòng trắng/#F8F9FA, hover dòng mượt mà.

### 3.1. Quản Lý Khách Hàng (frmKhachHang)

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 👥 Quản Lý Khách Hàng                                                  │
├─────────────────────────┬──────────────────────────────────────────────┤
│ ┌ THÔNG TIN ──────────┐ │ ┌ TÌM KIẾM ────────────────────────────────┐ │
│ │ Mã KH: (Tự động)    │ │ │ [🔍 Nhập CMND hoặc Tên...             ] │ │
│ │ [ #KH-0012        ] │ │ └──────────────────────────────────────────┘ │
│ │ Số CMND:            │ │ ┌ DANH SÁCH ───────────────────────────────┐ │
│ │ [ 079123456789    ] │ │ │ MaKH │ CMND       │ Họ Tên      │ Địa Chỉ│ │
│ │ Họ Tên:             │ │ │──────│────────────│─────────────│────────│ │
│ │ [ Nguyễn Văn An   ] │ │ │ 1    │ 0791234567 │ Nguyễn V.An │ Q.1    │ │
│ │ Địa Chỉ:            │ │ │ 2    │ 0792345678 │ Trần T.Bình │ Q.3    │ │
│ │ [ 12 Lý Tự Trọng  ] │ │ └──────────────────────────────────────────┘ │
│ │ ─────────────────── │ │ Tổng cộng: 124 khách hàng.                   │
│ │ [ ✨ THÊM MỚI  ]    │ │                                              │
│ │ [ 💾 CẬP NHẬT  ]    │ │                                              │
│ │ [ 🗑️ XÓA BỎ    ]    │ │                                              │
│ │ [ 🔄 LÀM MỚI   ]    │ │                                              │
│ └─────────────────────┘ │                                              │
└─────────────────────────┴──────────────────────────────────────────────┘
```

### 3.2. Quản Lý Nhân Viên (frmNhanVien) & Cửa Hàng (frmCuaHang)
- Áp dụng cùng layout 30/70 như Khách hàng.
- Riêng Nhân viên sẽ có thêm ComboBox `[▼ Chọn Cửa Hàng]` ở phần Thông tin.

### 3.3. Quản Lý Phim (frmPhim)
```text
┌────────────────────────────────────────────────────────────────────────┐
│ 🎬 Quản Lý Phim                                                        │
├─────────────────────────┬──────────────────────────────────────────────┤
│ ┌ THÔNG TIN ──────────┐ │ ┌ TÌM KIẾM & LỌC ──────────────────────────┐ │
│ │ Tựa Đề:             │ │ │ Thể loại: [▼ Tất cả] [🔍 Tên phim... ] │ │
│ │ [ Inception       ] │ │ └──────────────────────────────────────────┘ │
│ │ Năm Phát Hành:      │ │ ┌ DANH SÁCH PHIM ──────────────────────────┐ │
│ │ [ 2010            ] │ │ │ MaPhim │ Tựa Đề      │ Năm  │ Thể Loại │ │
│ │ Thể Loại:           │ │ │────────│─────────────│──────│──────────│ │
│ │ [▼ Khoa Học VT    ] │ │ │ 1      │ Inception   │ 2010 │ KHVT     │ │
│ │ Độ dài (phút):      │ │ │ 2      │ Dark Knight │ 2008 │ Hành động│ │
│ │ [ 148             ] │ │ └──────────────────────────────────────────┘ │
│ │ ─────────────────── │ │                                              │
│ │ [ ✨ THÊM MỚI  ]    │ │                                              │
│ │ ...                 │ │                                              │
│ └─────────────────────┘ │                                              │
└─────────────────────────┴──────────────────────────────────────────────┘
```

### 3.4. Quản Lý Băng Video (frmBangVideo) ⭐
Băng video chia làm Băng gốc (Phim) và Bản sao (Băng vật lý). Sử dụng Guna2TabControl.

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 📼 Quản Lý Băng Video                                                  │
├────────────────────────────────────────────────────────────────────────┤
│ ┌──────────────────┬────────────────────┐                              │
│ │ 🎬 Băng Gốc (1)  │ 📼 Bản Sao (2)     │   (TabControl hiện đại)      │
│ └──────────────────┴────────────────────┘                              │
│                                                                        │
│ ═══════ TAB 1: BĂNG GỐC ═════════════════════════════════════════════  │
│ ┌ THÔNG TIN BĂNG GỐC ─┐ ┌ DANH SÁCH BĂNG GỐC ────────────────────────┐ │
│ │ Chọn Phim:          │ │ │ BăngGốc│ Phim          │ Tổng Số Bản Sao│ │
│ │ [▼ Inception      ] │ │ │────────│───────────────│────────────────│ │
│ │                     │ │ │ 1      │ Inception     │ 3              │ │
│ │ [ ✨ THÊM MỚI  ]    │ │ │ 2      │ Dark Knight   │ 2              │ │
│ │ [ 🗑️ XÓA BỎ    ]    │ │ └──────────────────────────────────────────┘ │
│ └─────────────────────┘ └──────────────────────────────────────────────┘ │
│                                                                        │
│ ═══════ TAB 2: BẢN SAO VẬT LÝ ═══════════════════════════════════════  │
│ ┌ THÔNG TIN BẢN SAO ──┐ ┌ LỌC: Cửa hàng [▼] Trạng thái [▼] ──────────┐ │
│ │ Băng Gốc: [▼ Incept]│ │ ┌──────────────────────────────────────────┐ │
│ │ STT Băng: [ 4      ]│ │ │Gốc│STT│Phim      │CH │Loại│Giá │TrạngThái│ │
│ │ Loại:     [▼ PAL   ]│ │ │───│───│──────────│───│────│────│─────────│ │
│ │ Giá(đ):   [ 15,000 ]│ │ │ 1 │ 1 │Inception │CH1│PAL │15K │🟢 CoSan │ │
│ │ CH:       [▼ CH1   ]│ │ │ 2 │ 1 │DarkKnight│CH1│PAL │12K │🟡 ĐangM │ │
│ │ Hết hạn:  [📅 12/27]│ │ └──────────────────────────────────────────┘ │
│ │ [ ✨ THÊM MỚI  ]... │ │                                              │
│ └─────────────────────┘ └──────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────┘
```
- Khi chọn Băng gốc ở Tab 2, STT tự động +1 so với số bản sao hiện tại.
- Cột trạng thái dùng Label Badge có màu (Xanh/Vàng/Đỏ).

---

## 4. Giao diện Nghiệp vụ (Core UI)

Nghiệp vụ mượn/trả là nơi sử dụng nhiều nhất, thiết kế dạng **Wizard / Thẻ (Cards)** như mua hàng TMĐT.

### 4.1. Form Mượn Băng (frmMuonBang)

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 📤 LẬP HỒ SƠ MƯỢN BĂNG                                                 │
├────────────────────────────────────────────────────────────────────────┤
│ ┌ BƯỚC 1: KHÁCH HÀNG & CỬA HÀNG ─────────────────────────────────────┐ │
│ │ Khách: [🔍 Gõ sđt/cmnd/tên KH...] → 👤 Nguyễn Văn An (079123456789)│ │
│ │ Cửa hàng cho mượn: [▼ CH1 - Nguyễn Huệ]    Nhân viên: [▼ Võ Phong] │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│                                                                        │
│ ┌ BƯỚC 2: CHỌN BĂNG (Kho tại cửa hàng đã chọn) ──────────────────────┐ │
│ │ [🔍 Gõ tên phim cần mượn...]                                       │ │
│ │                                                                      │ │
│ │ 🎬 Inception (Khoa học viễn tưởng)                                  │ │
│ │ ├── [Bản 1] PAL  - 15K/ngày  [🟢 Có sẵn]  [ ➕ Thêm vào giỏ ]        │ │
│ │ └── [Bản 2] NTSC - 15K/ngày  [🟢 Có sẵn]  [ ➕ Thêm vào giỏ ]        │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│                                                                        │
│ ┌ BƯỚC 3: GIỎ BĂNG CHUẨN BỊ MƯỢN ────────────────────────────────────┐ │
│ │ 1. Inception (PAL)        │ Giá: 15,000đ/ngày  │ [❌ Xóa]          │ │
│ │ ────────────────────────────────────────────────────────────────── │ │
│ │ 📅 Ngày mượn: 07/07/2026     📅 Dự kiến trả: [14/07/2026] (7 ngày)   │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│                     [ HỦY BỎ ]       [ 🚀 XÁC NHẬN MƯỢN BĂNG (F5) ]    │
└────────────────────────────────────────────────────────────────────────┘
```

### 4.2. Form Trả Băng (frmTraBang)

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 📥 XỬ LÝ TRẢ BĂNG & THANH TOÁN                                         │
├────────────────────────────────────────────────────────────────────────┤
│ 👤 KH: [ Nguyễn Văn An (CMND: 079123) ▼ ]  ── 📅 Ngày trả: [14/07/2026]│
├────────────────────────────────────────────────────────────────────────┤
│ ┌ DANH SÁCH BĂNG ĐANG MƯỢN ──────────────────────────────────────────┐ │
│ │ [☑ Chọn tất cả]                                                    │ │
│ │                                                                      │ │
│ │ ☑ 1. Inception (PAL)      | Mượn: 01/07 | Hạn: 08/07 | ⚠️ Quá hạn 6 │ │
│ │   ↳ Số ngày tính phí: 14 ngày x 15,000đ = 210,000đ                   │ │
│ │                                                                      │ │
│ │ ☑ 2. Dark Knight (PAL)    | Mượn: 01/07 | Hạn: 08/07 | 🟢 Đúng hạn  │ │
│ │   ↳ Số ngày tính phí: 7 ngày x 12,000đ = 84,000đ                     │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│ ┌ TỔNG KẾT THANH TOÁN ───────────────────────────────────────────────┐ │
│ │ Số băng trả: 2 cuốn                                                  │ │
│ │ ───────────────────────────────────────────                          │ │
│ │ 💎 TỔNG PHẢI THU:                 294,000 đ  (Text đỏ, font 24pt)    │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│            [ 🧾 LƯU & IN BIÊN LAI (F8) ]       [ ĐÓNG ]                │
└────────────────────────────────────────────────────────────────────────┘
```
- Các dòng bị quá hạn sẽ chớp nháy hoặc có viền đỏ `#EE5D50`.
- Tổng phải thu cực kỳ to và rõ ràng.

---

## 5. Các Form Báo Cáo & Hỗ Trợ

### 5.1. Nhắc Trả Quá Hạn (frmNhacTra)
Mục đích: Tìm danh sách khách hàng đang mượn băng quá hạn.

```text
┌────────────────────────────────────────────────────────────────────────┐
│ ⚠️ THÔNG BÁO NHẮC TRẢ BĂNG QUÁ HẠN                                     │
├────────────────────────────────────────────────────────────────────────┤
│ 📅 Ngày đối chiếu: [ 14/07/2026 ]  [ 🔍 QUÉT DỮ LIỆU ]                 │
│                                                                        │
│ ┌ DANH SÁCH QUÁ HẠN ─────────────────────────────────────────────────┐ │
│ │ Khách Hàng  │ Phim Mượn      │ Ngày Hạn │ Quá Hạn │ SĐT Liên Hệ  │ │
│ │─────────────│────────────────│──────────│─────────│──────────────│ │
│ │ Nguyễn V.An │ Inception (1/1)│ 08/07/26 │ 🔴 6 ngày│ 0901234567   │ │
│ │ Trần T.Bình │ Avengers (5/2) │ 10/07/26 │ 🔴 4 ngày│ 0912345678   │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│ Tổng cộng: 2 cuốn quá hạn.                                             │
│                                                                        │
│           [ 🖨️ IN THÔNG BÁO NHẮC TRẢ (GỬI THƯ) ]       [ ĐÓNG ]        │
└────────────────────────────────────────────────────────────────────────┘
```

### 5.2. Báo Cáo Thống Kê (frmBaoCao)
- Sử dụng thư viện **LiveCharts.WinForms** để vẽ:
  - **Bar Chart:** Doanh thu theo từng cửa hàng.
  - **Pie Chart:** Tỷ lệ thể loại phim được mượn.
- Phía trên là bộ lọc: `[▼ Loại báo cáo]` (Doanh thu / Băng đang mượn / Phim hot), `[Từ ngày]` `[Đến ngày]`. Phía dưới bên trái là biểu đồ, bên phải là Grid chi tiết dữ liệu.

### 5.3. Hóa đơn Biên lai (frmBienLai)
Thiết kế biên lai dưới định dạng máy in nhiệt (POS Printer 80mm). Giao diện là một Panel trắng viền răng cưa, căn giữa, font chữ `Courier New`. Có nút `[🖨️ In]`.

---

## 6. Các Form Hệ Thống (Mới thêm)

### 6.1. Đăng nhập (frmDangNhap)
Bắt buộc hiển thị đầu tiên trước khi vào `frmMain`.

```text
┌─────────────────────────────────────────────────────────┐
│ 🔐 ĐĂNG NHẬP HỆ THỐNG - VIDEORENTAL PRO           [X]   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│        [ Logo Công Ty / Icon Băng Video ]               │
│                                                         │
│        Tên đăng nhập:                                   │
│        [👤 admin______________________________]         │
│                                                         │
│        Mật khẩu:                                        │
│        [🔑 ••••••_____________________________]         │
│                                                         │
│                [ 🚀 ĐĂNG NHẬP ]                         │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 6.2. Nhật Ký Hệ Thống (frmHeThongLog)
Dành cho Admin kiểm tra các thao tác đã diễn ra.

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 📜 NHẬT KÝ HỆ THỐNG (SYSTEM LOGS)                                      │
├────────────────────────────────────────────────────────────────────────┤
│ ┌ BỘ LỌC ────────────────────────────────────────────────────────────┐ │
│ │ Từ ngày: [ 01/07/2026 ]  Đến: [ 07/07/2026 ]                       │ │
│ │ Nhân viên: [▼ Tất cả]    Hành động: [▼ Tất cả]    [ 🔍 TÌM KIẾM ]  │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│ ┌ LỊCH SỬ THAO TÁC ──────────────────────────────────────────────────┐ │
│ │ Thời Gian     │ Nhân Viên │ Hành Động       │ Chi Tiết Mở Rộng     │ │
│ │───────────────│───────────│─────────────────│──────────────────────│ │
│ │ 07/07 14:30   │ Võ Phong  │ LẬP HỒ SƠ MƯỢN  │ Khách Nguyễn Văn An..│ │
│ │ 07/07 11:00   │ Admin     │ XÓA PHIM        │ (Xóa mềm) Inception  │ │
│ └────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────┘
```

### 6.3. Cấu Hình Hệ Thống (frmCauHinh)
Dành cho Admin tùy chỉnh các thông số.

```text
┌────────────────────────────────────────────────────────────────────────┐
│ 🛠️ CẤU HÌNH HỆ THỐNG                                                  │
├────────────────────────────────────────────────────────────────────────┤
│ ┌ THÔNG SỐ NGHIỆP VỤ ────────────────────────────────────────────────┐ │
│ │ Số ngày mượn tối đa mặc định (MAX_RENT_DAYS):                      │ │
│ │ [ 7                  ] ngày                                        │ │
│ │                                                                    │ │
│ │ Hệ số phạt quá hạn (LATE_FEE_MULTIPLIER):                          │ │
│ │ [ 1.5                ] (Tiền phạt = Đơn giá x Số ngày x Hệ số)     │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│ ┌ THÔNG TIN CÔNG TY ─────────────────────────────────────────────────┐ │
│ │ Tên công ty in trên biên lai (COMPANY_NAME):                       │ │
│ │ [ Công Ty Cho Thuê Băng Đĩa VideoRental Pro                      ] │ │
│ └────────────────────────────────────────────────────────────────────┘ │
│                                                                        │
│                      [ 💾 LƯU CẤU HÌNH ]                               │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 7. Sơ Đồ Điều Hướng & Kích Thước (Navigation & Size)

Sử dụng cơ chế MDI (Multiple Document Interface) ẩn hoặc Panel Swapping: Click Sidebar → Form con load đè lên Panel chính giữa. Do đó kích thước form con cố định.

| Form | Kích thước | Mô tả |
|------|-----------|-------|
| frmDangNhap | 400 × 500 | Đăng nhập hệ thống (ShowDialog) |
| frmMain | 1200 × 800 | Form chính, chứa Sidebar và Panel Content |
| frmDashboard | 950 × 800 | Load mặc định vào Panel Content |
| Danh mục (KH, NV...) | 950 × 800 | Load vào Panel Content |
| frmMuonBang | 950 × 800 | Form Mượn băng (Load vào Panel) |
| frmTraBang | 950 × 800 | Form Trả băng (Load vào Panel) |
| frmBienLai | 500 × 700 | Bật dạng Pop-up Dialog (ShowDialog) |
| frmNhacTra / frmBaoCao | 950 × 800 | Load vào Panel Content |
| frmHeThongLog | 950 × 800 | Load vào Panel Content (Chỉ Admin) |
| frmCauHinh | 950 × 800 | Load vào Panel Content (Chỉ Admin) |

---
> 💡 *"Một phần mềm tốt không chỉ chạy đúng, mà còn phải tạo cảm hứng cho người sử dụng."*
> Thiết kế UI/UX này sẽ đảm bảo Đồ án đạt điểm tối đa về sự đầu tư, tính thực tiễn và thẩm mỹ.

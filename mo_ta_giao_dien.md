# THIẾT KẾ GIAO DIỆN & TRẢI NGHIỆM NGƯỜI DÙNG (UI/UX)
*Dự án: Phần mềm Quản Lý Cho Thuê Băng Đĩa (Mô hình Đa cửa hàng)*

Dựa trên yêu cầu gốc của bài toán, để phần mềm đạt tiêu chuẩn **khoa học, đẹp mắt và đầy đủ nghiệp vụ**, chúng ta cần thiết kế giao diện theo hướng **Dashboard hiện đại** kết hợp **Mô hình chuỗi cửa hàng**.

---

## 1. Màn Hình Khởi Động & Đăng Nhập (`frmDangNhap`)
- **Giao diện:** Split-screen (chia đôi). Một nửa là hình ảnh minh họa (banner phim/cửa hàng băng đĩa vintage), nửa còn lại là form đăng nhập.
- **Chức năng:**
  - Nhập Username, Password.
  - **[MỚI] Chọn Cửa Hàng (Combobox):** Vì bài toán yêu cầu "khách có thể mượn/trả ở bất kỳ cửa hàng nào", nhân viên cần chọn họ đang trực ở cửa hàng nào trước khi vào hệ thống.

## 2. Giao Diện Chính (`frmMain`)
Sử dụng thiết kế **Navigation Menu bên trái (Sidebar)** kết hợp **Top-bar (chứa thông tin User & Cửa hàng đang trực)**. Sidebar có thể thu gọn/mở rộng.

### Các Module trên Sidebar:
1. **🏠 Tổng Quan (Dashboard / Báo Cáo)**
2. **📁 Quản Lý Danh Mục** (Menu sổ xuống)
   - Khách Hàng
   - Tựa Phim
   - Băng Video (Bản sao)
   - Cửa Hàng (Mới)
3. **🛒 Nghiệp Vụ Mượn - Trả** (Menu sổ xuống)
   - Lập Phiếu Mượn
   - Nhận Trả Băng & Thu Tiền
4. **⚙️ Hệ Thống** (Dành cho Admin)
   - Quản Lý Nhân Viên
   - Thùng Rác (Phục hồi dữ liệu)

---

## 3. Thiết Kế Chi Tiết Từng Màn Hình (Khoa học & Đầy đủ)

### A. Module Quản Lý Danh Mục (Chuẩn Layout 30/70)
Áp dụng chung cho: Khách Hàng, Phim, Băng Video, Nhân Viên, Cửa Hàng.
- **Panel Trái (30%):** Chứa các ô nhập liệu (TextBox, ComboBox) có Placeholder rõ ràng, viền bo góc (BorderRadius).
- **Bộ nút bấm (Action Buttons):**
  - `Thêm mới` (Xanh lá)
  - `Cập nhật` (Xanh dương)
  - `Xóa / Xóa tạm` (Đỏ)
  - `Làm mới form` (Xám)
- **Panel Phải (70%):** Chứa thanh Tìm Kiếm (Search bar) bo tròn và DataGridView hiển thị danh sách.
- **[Tính năng nâng cao]:** Click đúp vào Khách Hàng để xem Lịch Sử Thuê; Click đúp vào Phim để xem các Bản sao đang có.

### B. Module Nghiệp Vụ Mượn Trả (Split Layout 40/60)
Yêu cầu bài toán rất chặt chẽ về dữ liệu ghi nhận (ai mượn/trả, ở cửa hàng nào, nhân viên nào).

**Màn hình 1: Lập Phiếu Mượn**
- **Bên trái (Thông tin Phiếu):** Tìm & Chọn Khách Hàng, Tự động điền "Nhân viên cho mượn" và "Cửa hàng" (từ session đăng nhập). Chọn Ngày dự kiến trả.
- **Bên phải (Giỏ Băng):** Quét mã / Chọn băng video muốn thuê -> Thêm vào danh sách (giỏ hàng). Hiển thị tổng tiền tạm tính. Nút **[Lưu & In Phiếu Giao]**.

**Màn hình 2: Nhận Trả Băng & Thu Tiền**
- **Bước 1:** Tìm theo Mã Phiếu hoặc Tên Khách Hàng.
- **Bước 2:** Lưới hiển thị các băng khách đang giữ. Cột Checkbox để đánh dấu các đĩa khách mang trả (khách có thể trả lẻ tẻ).
- **Bước 3:** Hệ thống tính toán:
  - Tiền thuê gốc.
  - Số ngày trễ -> Tiền phạt.
- **Bước 4:** Nút **[Thanh Toán & In Biên Lai]**. (Yêu cầu bài toán: "in thành biên lai thu tiền cho khách").

### C. Module Báo Cáo & Nhắc Nhở (Dashboard Layout)
- **Biểu đồ/Thống kê nhanh (Top):** Tổng số đĩa đang cho mượn, Doanh thu trong ngày/tháng, Số lượng khách nợ đĩa.
- **Báo cáo 1:** Top phim thịnh hành.
- **Báo cáo 2:** Khách hàng không mượn đĩa.
- **[MỚI] Báo cáo 3:** Danh sách Phiếu Mượn Quá Hạn. 
  - Tích hợp nút **[In Thông Báo Nhắc Trả Băng]** (Yêu cầu bài toán: "Hằng ngày, dữ liệu mượn và trả băng được dùng để in thông báo nhắc trả").

---

## 4. Lộ Trình Triển Khai (Build Từng Phần)

**Giai đoạn 1: Nền tảng Đa Cửa Hàng & Khởi tạo Cấu trúc Sidebar UI**
- Thêm table/DAL cho Cửa Hàng, Khôi phục tính năng Đăng nhập để chọn Cửa hàng.
- Chuyển đổi frmMain sang giao diện Sidebar (NavMenu) hiện đại.

**Giai đoạn 2: Cải tiến UI Form Danh Mục & Thùng Rác**
- Áp dụng đầy đủ 4 nút (Thêm, Sửa, Xóa, Làm Mới) cho các form.
- Thêm chức năng "Thùng Rác" (Xem và khôi phục dữ liệu `DaXoa = true`).

**Giai đoạn 3: Hoàn thiện Nghiệp Vụ Mượn - Trả theo Yêu Cầu**
- Gắn session "Nhân viên" và "Cửa hàng" vào thao tác Mượn/Trả.
- Nâng cấp UI chọn Băng (Giỏ hàng).
- Chức năng **Xuất Biên Lai Thu Tiền** (In PDF/Excel).

**Giai đoạn 4: Báo cáo & Nhắc nhở Nâng cao**
- Xây dựng module "Danh sách nợ/quá hạn".
- Chức năng **In Thông báo nhắc trả băng** cho khách hàng.

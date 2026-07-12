# BÁO CÁO ĐỒ ÁN 1

# HỆ THỐNG QUẢN LÝ CHO THUÊ BĂNG VIDEO

---

**Giảng viên hướng dẫn:** [Tên GVHD]

**Sinh viên thực hiện:**

| STT | Họ và tên      | MSSV   |
| --- | -------------- | ------ |
| 1   | [Thành viên A] | [MSSV] |
| 2   | [Thành viên B] | [MSSV] |
| 3   | [Thành viên C] | [MSSV] |

**Năm học:** 2025 – 2026

---

# MỤC LỤC

- Chương 1: Tổng quan đề tài
- Chương 2: Cơ sở lý thuyết
- Chương 3: Phân tích và thiết kế hệ thống
- Chương 4: Cài đặt và thực hiện
- Chương 5: Kết luận
- Tài liệu tham khảo

---

# CHƯƠNG 1: TỔNG QUAN ĐỀ TÀI

## 1.1. Lý do chọn đề tài

Trong thời đại công nghệ thông tin phát triển mạnh mẽ, việc ứng dụng tin học vào quản lý hoạt động kinh doanh là nhu cầu thiết yếu của mọi doanh nghiệp. Đối với các công ty kinh doanh dịch vụ cho thuê băng video, việc quản lý thông tin khách hàng, nhân viên, cửa hàng, kho băng video cũng như các giao dịch mượn — trả băng bằng phương pháp thủ công (sổ sách, giấy tờ) gặp rất nhiều khó khăn:

- **Khối lượng dữ liệu lớn:** Với hàng trăm khách hàng, hàng nghìn cuốn băng video phân bố trên nhiều cửa hàng, việc ghi chép và tra cứu thủ công tốn nhiều thời gian và dễ sai sót.
- **Khó theo dõi tình trạng băng video:** Không thể biết nhanh chóng cuốn băng nào đang được mượn, cuốn nào còn sẵn, cuốn nào đã hết hạn sử dụng.
- **Quản lý mượn — trả phức tạp:** Một khách hàng có thể mượn nhiều cuốn băng từ nhiều cửa hàng khác nhau, trả ở cửa hàng khác, trả từng phần… Việc tính toán tiền thuê, theo dõi hạn trả rất phức tạp nếu không có hệ thống.
- **Nhắc trả băng không hiệu quả:** Khi không có hệ thống tự động, việc kiểm tra và gửi thông báo nhắc trả cho khách hàng mượn quá hạn hầu như không thể thực hiện kịp thời.
- **Báo cáo thống kê khó khăn:** Việc tổng hợp doanh thu, thống kê phim được mượn nhiều, khách hàng thường xuyên… đòi hỏi nhiều công sức nếu làm thủ công.

Từ những lý do trên, nhóm quyết định xây dựng **"Hệ thống quản lý cho thuê băng video"** — một ứng dụng phần mềm giúp tin học hóa toàn bộ quy trình quản lý hoạt động cho thuê băng video, từ quản lý danh mục đến xử lý nghiệp vụ mượn — trả và in biên lai.

## 1.2. Mục tiêu đề tài

### 1.2.1. Mục tiêu tổng quát

Xây dựng một ứng dụng phần mềm quản lý cho thuê băng video hoàn chỉnh, có khả năng:

- Lưu trữ và quản lý thông tin cửa hàng, khách hàng, nhân viên, phim và băng video.
- Xử lý nghiệp vụ mượn — trả băng video một cách chính xác và nhanh chóng.
- In biên lai thu tiền cho khách hàng khi trả băng.
- Tự động phát hiện và thông báo nhắc trả cho các trường hợp mượn quá hạn.

### 1.2.2. Mục tiêu cụ thể

1. **Quản lý cửa hàng:** Thêm, sửa, xóa, xem thông tin các cửa hàng (mã cửa hàng, địa chỉ, số điện thoại).
2. **Quản lý khách hàng:** Đăng ký khách hàng mới (nhập CMND, tên, địa chỉ → hệ thống cấp mã khách hàng), sửa, xóa, tìm kiếm thông tin khách hàng.
3. **Quản lý nhân viên:** Đăng ký nhân viên mới, phân công nhân viên cho cửa hàng, sửa, xóa, tìm kiếm.
4. **Quản lý phim:** Thêm, sửa, xóa thông tin phim (tựa đề, năm phát hành, thể loại, độ dài).
5. **Quản lý băng video:** Quản lý băng video gốc và các bản sao (loại băng, đơn giá, ngày hết hạn, trạng thái).
6. **Nghiệp vụ mượn băng:** Tạo hồ sơ mượn, ghi nhận khách hàng mượn những cuốn băng nào, tại cửa hàng nào, nhân viên nào cho mượn, ngày mượn và ngày dự kiến trả.
7. **Nghiệp vụ trả băng:** Tạo hồ sơ trả, tính toán tiền thuê (đơn giá × số ngày mượn), in biên lai thu tiền.
8. **Thông báo nhắc trả:** Quét cơ sở dữ liệu, phát hiện các hồ sơ mượn quá hạn và in thông báo nhắc trả.
9. **Báo cáo thống kê:** Thống kê doanh thu, danh sách băng đang cho mượn, phim được mượn nhiều nhất, v.v.

## 1.3. Phạm vi đề tài

### 1.3.1. Phạm vi chức năng

Hệ thống tập trung vào các chức năng cốt lõi phục vụ hoạt động cho thuê băng video:

| Nhóm chức năng   | Các chức năng cụ thể                                   |
| ---------------- | ------------------------------------------------------ |
| Quản lý danh mục | CRUD cửa hàng, khách hàng, nhân viên, phim, băng video |
| Nghiệp vụ chính  | Mượn băng, trả băng, in biên lai                       |
| Hỗ trợ           | Nhắc trả quá hạn, báo cáo thống kê, tìm kiếm           |

### 1.3.2. Giới hạn

- Ứng dụng hoạt động trên nền tảng **Windows Desktop** (không phải web hay mobile).
- Cơ sở dữ liệu được lưu trữ cục bộ trên máy tính (SQL Server Express / SQLite).
- Chưa hỗ trợ phân quyền nâng cao (admin, manager, staff).
- Chưa tích hợp thanh toán điện tử.

## 1.4. Công cụ và công nghệ sử dụng

| Thành phần           | Công nghệ / Công cụ          | Phiên bản   |
| -------------------- | ---------------------------- | ----------- |
| Ngôn ngữ lập trình   | C#                           | .NET 8.0    |
| Giao diện người dùng | Windows Forms (WinForms)     | .NET 8.0    |
| Cơ sở dữ liệu        | Microsoft SQL Server Express | 2019 / 2022 |
| Truy xuất dữ liệu    | ADO.NET                      | .NET 8.0    |
| IDE phát triển       | Visual Studio                | 2022        |
| Quản lý mã nguồn     | Git + GitHub                 | —           |
| Thiết kế báo cáo     | Microsoft Word               | 2021 / 365  |
| Thiết kế slide       | Microsoft PowerPoint         | 2021 / 365  |

## 1.5. Kiến trúc phần mềm tổng quan

Hệ thống được xây dựng theo **mô hình 3 lớp (3-Layer Architecture):**

| Lớp                       | Tên Project     | Chức năng                                                                     |
| ------------------------- | --------------- | ----------------------------------------------------------------------------- |
| **Presentation Layer**    | VideoRental.GUI | Giao diện người dùng (WinForms), nhận input từ người dùng và hiển thị kết quả |
| **Business Logic Layer**  | VideoRental.BLL | Xử lý logic nghiệp vụ, kiểm tra ràng buộc, tính toán                          |
| **Data Access Layer**     | VideoRental.DAL | Truy xuất cơ sở dữ liệu (CRUD operations) qua ADO.NET                         |
| **Data Transfer Objects** | VideoRental.DTO | Các lớp đối tượng dùng để truyền dữ liệu giữa các lớp                         |

Luồng dữ liệu: **GUI → BLL → DAL → Database**

## 1.6. Bố cục báo cáo

Báo cáo được trình bày gồm 5 chương:

- **Chương 1 – Tổng quan đề tài:** Giới thiệu lý do chọn đề tài, mục tiêu, phạm vi, công cụ sử dụng và bố cục báo cáo.
- **Chương 2 – Cơ sở lý thuyết:** Trình bày các kiến thức nền tảng về ngôn ngữ C#, Windows Forms, ADO.NET, SQL Server, và mô hình kiến trúc 3 lớp.
- **Chương 3 – Phân tích và thiết kế hệ thống:** Phân tích yêu cầu, thiết kế cơ sở dữ liệu (ERD), sơ đồ Use Case, Activity Diagram, Class Diagram.
- **Chương 4 – Cài đặt và thực hiện:** Trình bày chi tiết cài đặt từng chức năng kèm giao diện (screenshot) và giải thích code quan trọng.
- **Chương 5 – Kết luận:** Tổng kết kết quả đạt được, nêu hạn chế và đề xuất hướng phát triển.

---

# CHƯƠNG 2: CƠ SỞ LÝ THUYẾT

## 2.1. Ngôn ngữ lập trình C#

### 2.1.1. Giới thiệu C#

C# (đọc là "C-sharp") là ngôn ngữ lập trình hướng đối tượng, được phát triển bởi Microsoft, ra mắt lần đầu vào năm 2000 cùng với nền tảng .NET Framework. C# được thiết kế bởi Anders Hejlsberg — người cũng đã tạo ra ngôn ngữ Turbo Pascal và Delphi.

C# là ngôn ngữ lập trình hiện đại, mạnh mẽ, và đa mục đích. Nó kết hợp sức mạnh của C++ với sự đơn giản của Visual Basic, đồng thời loại bỏ nhiều phức tạp không cần thiết như quản lý con trỏ thủ công.

### 2.1.2. Đặc điểm chính của C#

- **Hướng đối tượng (Object-Oriented):** Hỗ trợ đầy đủ các tính chất: đóng gói (Encapsulation), kế thừa (Inheritance), đa hình (Polymorphism), trừu tượng (Abstraction).
- **Type-safe:** Kiểm tra kiểu dữ liệu nghiêm ngặt tại thời điểm biên dịch, giảm thiểu lỗi runtime.
- **Quản lý bộ nhớ tự động:** Garbage Collector tự động thu hồi bộ nhớ không còn sử dụng.
- **Xử lý ngoại lệ (Exception Handling):** Cơ chế try-catch-finally giúp xử lý lỗi một cách có cấu trúc.
- **Hỗ trợ LINQ:** Language Integrated Query cho phép truy vấn dữ liệu trực tiếp trong code C#.
- **Đa nền tảng:** Với .NET Core / .NET 5+, C# có thể chạy trên Windows, macOS và Linux.

### 2.1.3. Ứng dụng của C# trong đồ án

Trong đồ án này, C# được sử dụng để:

- Xây dựng giao diện người dùng bằng Windows Forms.
- Viết logic nghiệp vụ (tính tiền thuê, kiểm tra quá hạn, validate dữ liệu).
- Truy xuất cơ sở dữ liệu SQL Server thông qua ADO.NET.
- Tổ chức code theo mô hình 3 lớp với các class, interface rõ ràng.

## 2.2. Windows Forms (WinForms)

### 2.2.1. Giới thiệu Windows Forms

Windows Forms (WinForms) là một framework giao diện đồ họa người dùng (GUI) cho ứng dụng desktop trên nền tảng Windows. WinForms là một phần của .NET Framework và .NET (từ .NET Core 3.0 trở đi).

WinForms cung cấp cách tiếp cận kéo-thả (drag-and-drop) để thiết kế giao diện, cho phép lập trình viên nhanh chóng tạo ra các form với các điều khiển (controls) như: Button, TextBox, DataGridView, Label, ComboBox, MenuStrip, v.v.

### 2.2.2. Các thành phần chính của WinForms

| Thành phần         | Mô tả                          | Ví dụ sử dụng trong đồ án               |
| ------------------ | ------------------------------ | --------------------------------------- |
| **Form**           | Cửa sổ chính chứa các controls | frmMain, frmKhachHang, frmMuonBang      |
| **TextBox**        | Ô nhập văn bản                 | Nhập CMND, tên khách hàng, địa chỉ      |
| **ComboBox**       | Danh sách thả xuống            | Chọn cửa hàng, thể loại phim, loại băng |
| **DataGridView**   | Bảng hiển thị dữ liệu          | Hiển thị danh sách KH, phim, băng video |
| **Button**         | Nút nhấn                       | Thêm, Sửa, Xóa, Tìm kiếm, Lưu           |
| **DateTimePicker** | Chọn ngày tháng                | Ngày mượn, ngày dự kiến trả             |
| **MenuStrip**      | Thanh menu                     | Menu chính: Quản lý, Nghiệp vụ, Báo cáo |
| **PrintDocument**  | Điều khiển in ấn               | In biên lai thu tiền                    |

### 2.2.3. Ưu điểm của WinForms trong đồ án

- **Dễ học, dễ sử dụng:** Phù hợp cho sinh viên mới tiếp cận lập trình giao diện.
- **Phát triển nhanh:** Designer hỗ trợ kéo-thả, không cần viết code giao diện thủ công.
- **Tài liệu phong phú:** Microsoft cung cấp tài liệu chi tiết và có nhiều ví dụ trên cộng đồng.
- **Phù hợp với ứng dụng nghiệp vụ:** WinForms rất thích hợp cho các ứng dụng quản lý CRUD.

## 2.3. ADO.NET

### 2.3.1. Giới thiệu ADO.NET

ADO.NET (ActiveX Data Objects for .NET) là một tập hợp các lớp (classes) trong .NET Framework cho phép ứng dụng tương tác với các nguồn dữ liệu, đặc biệt là cơ sở dữ liệu quan hệ. ADO.NET cung cấp cách tiếp cận linh hoạt và hiệu suất cao cho việc truy xuất dữ liệu.

### 2.3.2. Kiến trúc ADO.NET

ADO.NET bao gồm hai thành phần chính:

**a) Data Provider (Nhà cung cấp dữ liệu)**

Là tập hợp các lớp dùng để kết nối và thao tác với cơ sở dữ liệu:

| Lớp                | Chức năng                                                     |
| ------------------ | ------------------------------------------------------------- |
| **SqlConnection**  | Tạo kết nối đến SQL Server                                    |
| **SqlCommand**     | Thực thi câu lệnh SQL (SELECT, INSERT, UPDATE, DELETE)        |
| **SqlDataReader**  | Đọc dữ liệu tuần tự (forward-only, read-only) — hiệu suất cao |
| **SqlDataAdapter** | Cầu nối giữa DataSet và cơ sở dữ liệu                         |
| **SqlParameter**   | Tham số hóa câu lệnh SQL — chống SQL Injection                |

**b) DataSet (Tập dữ liệu)**

DataSet là bộ nhớ đệm (cache) chứa dữ liệu ở phía client, cho phép thao tác dữ liệu mà không cần kết nối liên tục đến server. DataSet chứa các DataTable, DataRelation, và DataColumn.

### 2.3.3. Mô hình kết nối ADO.NET trong đồ án

Trong đồ án, nhóm sử dụng mô hình **Connected Model** (kết nối trực tiếp) với quy trình:

1. Mở kết nối (`SqlConnection.Open()`)
2. Tạo câu lệnh (`SqlCommand`)
3. Thực thi câu lệnh:
   - `ExecuteReader()` — cho câu SELECT (trả về SqlDataReader)
   - `ExecuteNonQuery()` — cho câu INSERT, UPDATE, DELETE (trả về số dòng bị ảnh hưởng)
   - `ExecuteScalar()` — cho câu trả về 1 giá trị đơn
4. Đóng kết nối (`SqlConnection.Close()`)

### 2.3.4. Phòng chống SQL Injection

Nhóm sử dụng **SqlParameter** để tham số hóa tất cả câu lệnh SQL, tránh lỗ hổng SQL Injection. Ví dụ:

```csharp
// Cách sai — dễ bị SQL Injection
string query = "SELECT * FROM KHACHHANG WHERE CMND = '" + cmnd + "'";

// Cách đúng — sử dụng SqlParameter
string query = "SELECT * FROM KHACHHANG WHERE CMND = @CMND";
SqlCommand cmd = new SqlCommand(query, conn);
cmd.Parameters.AddWithValue("@CMND", cmnd);
```

## 2.4. Microsoft SQL Server

### 2.4.1. Giới thiệu SQL Server

Microsoft SQL Server là hệ quản trị cơ sở dữ liệu quan hệ (RDBMS) được phát triển bởi Microsoft. SQL Server hỗ trợ ngôn ngữ truy vấn T-SQL (Transact-SQL), cung cấp nhiều tính năng quản lý dữ liệu mạnh mẽ.

### 2.4.2. Phiên bản sử dụng

Đồ án sử dụng **SQL Server Express** — phiên bản miễn phí của SQL Server, phù hợp cho ứng dụng quy mô nhỏ và vừa. SQL Server Express có giới hạn:

- Dung lượng database tối đa: 10 GB
- RAM tối đa: 1 GB
- CPU: 1 socket hoặc 4 core

Những giới hạn này hoàn toàn đủ cho yêu cầu của đồ án.

### 2.4.3. Các khái niệm CSDL quan hệ sử dụng trong đồ án

| Khái niệm            | Mô tả                                    | Ví dụ trong đồ án                    |
| -------------------- | ---------------------------------------- | ------------------------------------ |
| **Table**            | Bảng lưu trữ dữ liệu                     | KHACHHANG, PHIM, BANGVIDEO           |
| **Primary Key (PK)** | Khóa chính — định danh duy nhất mỗi dòng | MaKH, MaPhim                         |
| **Composite Key**    | Khóa phức hợp — gồm nhiều cột            | (MaBangGoc, SoThuTu) trong BANGVIDEO |
| **Foreign Key (FK)** | Khóa ngoại — liên kết giữa các bảng      | MaCuaHang trong NHANVIEN             |
| **IDENTITY**         | Tự động tăng giá trị                     | MaKH INT IDENTITY(1,1)               |
| **CONSTRAINT**       | Ràng buộc dữ liệu                        | CHECK, UNIQUE, NOT NULL              |
| **INDEX**            | Chỉ mục — tăng tốc truy vấn              | INDEX trên TrangThai, NgayDuKienTra  |

## 2.5. Mô hình kiến trúc 3 lớp (3-Layer Architecture)

### 2.5.1. Khái niệm

Mô hình 3 lớp là một kiến trúc phần mềm chia ứng dụng thành 3 tầng riêng biệt, mỗi tầng có trách nhiệm cụ thể và chỉ giao tiếp với tầng liền kề.

### 2.5.2. Các lớp trong mô hình thực tế của đồ án (`QuanLyThueBang`)

Hệ thống được phát triển trên nền tảng **C# .NET 8 WinForms** ứng dụng cơ chế tiêm phụ thuộc (**Dependency Injection - DI**) và **Entity Framework Core 8** theo mô hình 3 lớp phân tách rõ ràng:

**Lớp 1: Presentation Layer (Lớp giao diện & Điều hướng)**

- **Chức năng:** Hiển thị giao diện người dùng theo phong cách hiện đại (Giao diện 2 - Modern Card UI), nhận các thao tác nhập liệu, hiển thị dữ liệu dạng bảng và thông báo kết quả.
- **Đặc điểm:** Chỉ gọi các Service ở tầng BLL thông qua Dependency Injection (`_serviceProvider.GetRequiredService<T>()`), không truy cập trực tiếp vào cơ sở dữ liệu.
- **Trong đồ án:** Nằm trong namespace `QuanLyThueBang.Presentation`, gồm:
  - `Forms/MainShellForm.cs`: Khung chính với Sidebar điều hướng tĩnh bên trái và vùng làm việc động bên phải.
  - `Controls/`: Các màn hình quản lý chức năng dưới dạng `UserControl` (`QuanLyPhimControl`, `QuanLyDanhMucControl`, `QuanLyBanSaoControl`, `QuanLyCuaHangControl`, `QuanLyKhachHangControl`, `QuanLyNhanVienControl`, `MuonTraBangControl`).
  - `Forms/.../DialogForm`: Các cửa sổ Pop-up thêm/sửa chuyên dụng.

**Lớp 2: Business Logic Layer - BLL (Lớp nghiệp vụ)**

- **Chức năng:** Xử lý các quy tắc nghiệp vụ, kiểm tra tính hợp lệ dữ liệu (Validate ràng buộc chi nhánh, hạn sử dụng băng, tính tiền thuê/phạt), quản lý transaction CSDL.
- **Trong đồ án:** Thư mục `BLL/`, bao gồm các Service chuyên biệt: `AuthService`, `PhimService`, `BanSaoBangService`, `CuaHangService`, `KhachHangService`, `NhanVienService`, `MuonTraService`.

**Lớp 3: Data Access Layer - DAL (Lớp truy xuất dữ liệu & Thực thể)**

- **Chức năng:** Kết nối và thao tác với SQL Server thông qua ORM **Entity Framework Core 8**.
- **Trong đồ án:**
  - `DAL/QuanLyThueBangContext.cs`: Lớp `DbContext` định nghĩa ánh xạ thực thể và thực thi các thao tác CRUD/Transaction.
  - `Domain/Entities/`: Các lớp thực thể ánh xạ 1-1 với 11 bảng trong CSDL (`Phim`, `TheLoai`, `BanSaoBang`, `CuaHang`, `KhachHang`, `NhanVien`, `VaiTro`, `PhieuMuon`, `ChiTietPhieuMuon`, `PhieuTra`, `ChiTietPhieuTra`).

**Lớp truyền tải dữ liệu: Data Transfer Object (DTO)**

- **Chức năng:** Các lớp đối tượng gọn nhẹ dùng để truyền tải dữ liệu giữa tầng BLL và Presentation.
- **Trong đồ án:** Thư mục `Domain/DTOs/` (`PhimDTO`, `BanSaoBangViewDTO`, `ChiTietGioMuonDTO`, `ThongTinBangMuonChuaTraDTO`, `BangQuaHanDTO`, ...).

### 2.5.3. Luồng dữ liệu

```
Người dùng → [GUI] → [BLL] → [DAL] → [Database]
                ↑         ↑        ↑
               DTO       DTO      DTO
```

1. Người dùng tương tác với giao diện (GUI).
2. GUI tạo đối tượng DTO, gọi phương thức BLL.
3. BLL kiểm tra nghiệp vụ, nếu hợp lệ → gọi DAL.
4. DAL thực thi SQL, trả kết quả về qua DTO.
5. Kết quả truyền ngược: DAL → BLL → GUI → Hiển thị cho người dùng.

### 2.5.4. Ưu điểm của mô hình 3 lớp

- **Tách biệt trách nhiệm (Separation of Concerns):** Mỗi lớp có nhiệm vụ riêng, dễ bảo trì.
- **Dễ mở rộng:** Có thể thay đổi giao diện mà không ảnh hưởng đến logic nghiệp vụ, hoặc đổi CSDL mà không cần sửa GUI.
- **Tái sử dụng code:** Các lớp BLL và DAL có thể tái sử dụng cho giao diện khác (web, mobile).
- **Làm việc nhóm hiệu quả:** Mỗi thành viên có thể phụ trách một lớp riêng biệt.
- **Dễ kiểm thử:** Có thể viết unit test cho từng lớp độc lập.

---

# CHƯƠNG 3: PHÂN TÍCH VÀ THIẾT KẾ HỆ THỐNG

## 3.1. Phân tích yêu cầu

### 3.1.1. Yêu cầu chức năng

Dựa trên đề bài, hệ thống cần đáp ứng các yêu cầu chức năng sau:

**a) Quản lý thông tin cơ bản:**

| STT  | Yêu cầu                    | Mô tả                                                                     |
| ---- | -------------------------- | ------------------------------------------------------------------------- |
| YC01 | Quản lý cửa hàng           | Thêm, sửa, xóa, xem danh sách cửa hàng (mã, địa chỉ, SĐT)                 |
| YC02 | Quản lý khách hàng         | Đăng ký KH (nhập CMND, tên, địa chỉ → cấp mã KH), sửa, xóa, tìm kiếm      |
| YC03 | Quản lý nhân viên          | Đăng ký NV (nhập CMND, tên, địa chỉ → cấp mã NV), phân cửa hàng, sửa, xóa |
| YC04 | Quản lý phim               | Thêm, sửa, xóa phim (mã, tựa đề, năm, thể loại, độ dài)                   |
| YC05 | Quản lý băng video gốc     | Thêm, sửa, xóa băng gốc, liên kết với phim                                |
| YC06 | Quản lý bản sao băng video | Thêm, sửa, xóa bản sao (loại băng, đơn giá, ngày hết hạn, trạng thái)     |

**b) Nghiệp vụ chính:**

| STT  | Yêu cầu               | Mô tả                                                                                                                           |
| ---- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| YC07 | Mượn băng video       | Tạo hồ sơ mượn: chọn KH, chọn các cuốn băng, nhập ngày mượn, ngày dự kiến trả, ghi nhận cửa hàng và nhân viên cho mượn          |
| YC08 | Trả băng video        | Tạo hồ sơ trả: chọn KH, chọn các cuốn băng cần trả, tính tiền thuê (đơn giá × số ngày), ghi nhận cửa hàng và nhân viên nhận trả |
| YC09 | In biên lai thu tiền  | Sau khi trả băng, in biên lai ghi rõ: thông tin KH, danh sách băng trả, số tiền từng cuốn, tổng tiền                            |
| YC10 | Nhắc trả băng quá hạn | Quét hồ sơ mượn, tìm các trường hợp quá hạn (ngày hiện tại > ngày dự kiến trả), in thông báo nhắc trả                           |

### 3.1.2. Yêu cầu phi chức năng

| STT     | Yêu cầu          | Mô tả                                                              |
| ------- | ---------------- | ------------------------------------------------------------------ |
| YC-PF01 | Hiệu năng        | Thời gian phản hồi < 2 giây cho mỗi thao tác                       |
| YC-PF02 | Dễ sử dụng       | Giao diện trực quan, dễ thao tác cho nhân viên                     |
| YC-PF03 | Toàn vẹn dữ liệu | Dữ liệu phải nhất quán, có ràng buộc khóa ngoại                    |
| YC-PF04 | Validate dữ liệu | Kiểm tra đầu vào: không để trống, đúng định dạng, không trùng CMND |
| YC-PF05 | Bảo mật          | Chống SQL Injection bằng SqlParameter                              |

### 3.1.3. Các tác nhân (Actors)

| Tác nhân       | Mô tả                                                                      |
| -------------- | -------------------------------------------------------------------------- |
| **Nhân viên**  | Người dùng chính của hệ thống, thực hiện các thao tác quản lý và nghiệp vụ |
| **Khách hàng** | Đăng ký thông tin, yêu cầu mượn/trả băng (thông qua nhân viên)             |
| **Hệ thống**   | Tự động quét và tạo thông báo nhắc trả quá hạn                             |

## 3.2. Sơ đồ Use Case

### 3.2.1. Use Case tổng quát

| Use Case                 | Tác nhân              | Mô tả                                    |
| ------------------------ | --------------------- | ---------------------------------------- |
| UC01: Đăng ký khách hàng | Nhân viên, Khách hàng | NV nhập thông tin KH, hệ thống cấp mã    |
| UC02: Quản lý phim       | Nhân viên             | CRUD thông tin phim                      |
| UC03: Quản lý băng video | Nhân viên             | CRUD băng gốc và bản sao                 |
| UC04: Mượn băng video    | Nhân viên             | Tạo hồ sơ mượn                           |
| UC05: Trả băng video     | Nhân viên             | Tạo hồ sơ trả, tính tiền                 |
| UC06: In biên lai        | Nhân viên             | In biên lai sau khi trả (extend từ UC05) |
| UC07: Nhắc trả quá hạn   | Hệ thống              | Tự động quét và in thông báo             |
| UC08: Quản lý cửa hàng   | Nhân viên             | CRUD cửa hàng                            |
| UC09: Quản lý nhân viên  | Nhân viên             | CRUD nhân viên                           |
| UC10: Báo cáo thống kê   | Nhân viên             | Xem báo cáo doanh thu, thống kê          |
| UC11: Tìm kiếm phim      | Nhân viên, Khách hàng | Tìm phim theo tựa đề, thể loại           |

### 3.2.2. Đặc tả Use Case — UC04: Lập Phiếu Mượn Băng (Tại quầy)

| Thành phần         | Nội dung                                                                                                           |
| ------------------ | ------------------------------------------------------------------------------------------------------------------ |
| **Tên Use Case**   | Lập Phiếu Mượn Băng (Tại quầy)                                                                                     |
| **Mã**             | UC04                                                                                                               |
| **Tác nhân**       | Nhân viên quầy giao dịch                                                                                           |
| **Mô tả**          | Quy trình diễn ra khi khách hàng mang những cuốn băng họ muốn thuê đến quầy thanh toán                             |
| **Tiền điều kiện** | Khách hàng đã đăng ký hoặc tạo mới ngay tại quầy; Băng video ở trạng thái "Sẵn sàng" thuộc đúng chi nhánh hiện tại |
| **Hậu điều kiện**  | Phiếu mượn được khởi tạo trong CSDL; trạng thái bản sao băng chuyển sang "Đang cho mượn"                           |

**Luồng nghiệp vụ chi tiết (Main Flow):**

1. **Bước 1: Xác thực Khách hàng.**
   - Nhân viên nhập số CMND/CCCD hoặc Mã Khách Hàng vào hệ thống.
   - _Nếu là khách quen:_ Hệ thống tự động hiển thị hồ sơ thông tin (Họ tên, Số điện thoại, Địa chỉ).
   - _Nếu là khách mới:_ Nhân viên tiến hành tạo mới hồ sơ khách hàng nhanh ngay trên form mượn mà không cần gián đoạn giao dịch.
2. **Bước 2: Quét/Nhập mã bản sao băng.**
   - Nhân viên dùng máy quét mã vạch hoặc nhập tay `MaBanSao` (VD: `RFID-BS001`) của từng cuốn băng khách muốn mượn.
   - Hệ thống tự động kiểm tra nghiêm ngặt các điều kiện ràng buộc:
     - _Kiểm tra vị trí chi nhánh:_ Cuốn băng phải đang thuộc chi nhánh hiện tại (`MaCuaHangHienTai = Mã chi nhánh của nhân viên quầy`).
     - _Kiểm tra trạng thái:_ Trạng thái băng phải là `"Sẵn sàng"`.
     - _Kiểm tra hạn sử dụng:_ Băng không bị quá hạn sử dụng (`NgayHetHan >= Ngày hiện tại`).
3. **Bước 3: Hiển thị giỏ hàng mượn.**
   - Hệ thống liệt kê danh sách các cuốn băng hợp lệ vào bảng (`DataGridView`) trên màn hình, tự động hiển thị `DonGiaThue` của từng cuốn.
4. **Bước 4: Chốt phiếu mượn.**
   - Nhân viên chọn `NgayDuKienTra` (mặc định cộng thêm số ngày theo quy định) và bấm lưu giao dịch.

**Tác động tới Cơ sở dữ liệu (Transaction Backend):**

- Tạo 1 bản ghi vào bảng `PHIEUMUON` (lưu ngày mượn, ngày dự kiến trả, mã nhân viên cho mượn, mã khách hàng).
- Tạo $N$ bản ghi vào bảng `CHITIETPHIEUMUON` (tương ứng với số băng khách mượn), thiết lập mặc định `DaTra = false`.
- Cập nhật trạng thái của các bản sao băng tương ứng trong bảng `BANSAOBANG` thành `"Đang cho mượn"`.

---

### 3.2.3. Đặc tả Use Case — UC05: Nhận Trả Băng & Luân Chuyển Kho (Cross-Store Return)

| Thành phần | Nội dung |
| **Tên Use Case** | Nhận Trả Băng & Luân Chuyển Kho (Cross-Store Return) |
| **Mã** | UC05 |
| **Tác nhân** | Nhân viên quầy giao dịch |
| **Mô tả** | Nghiệp vụ giải quyết bài toán mượn một nơi trả một nẻo, cho phép trả từng phần, kiểm tra tình trạng vi phạm, tính phạt thủ công và luân chuyển kho tự động |
| **Tiền điều kiện** | Cuốn băng mang trả đang ở trạng thái "Đang cho mượn" và có lịch sử mượn chưa hoàn tất |
| **Hậu điều kiện** | Phiếu trả được lập; chi tiết phiếu mượn được đánh dấu đã trả; băng chuyển về "Sẵn sàng" và cập nhật chi nhánh sở hữu hiện tại |

**Luồng nghiệp vụ chi tiết (Main Flow):**

1. **Bước 1: Nhận diện băng trả.**
   - Khách hàng mang băng đến quầy. Nhân viên không cần hỏi khách số phiếu mượn, chỉ cần quét trực tiếp `MaBanSao` dán trên cuốn băng.
2. **Bước 2: Hệ thống truy xuất tự động.**
   - Từ `MaBanSao` vừa quét, hệ thống tự động tra cứu trong bảng `CHITIETPHIEUMUON` để truy xuất thông tin gốc: Cuốn băng này thuộc Phiếu mượn nào? Khách hàng nào mượn? Đơn giá thuê lúc mượn là bao nhiêu? Ngày dự kiến trả là ngày nào?
3. **Bước 3: Kiểm tra tình trạng & Tính phạt (Thủ công).**
   - _Kiểm tra hạn trả:_ Hệ thống tự động so sánh `NgayTra` (hôm nay) với `NgayDuKienTra`. Nếu quá hạn, hệ thống bật cảnh báo viền đỏ (Alert) trên màn hình và hiển thị số ngày trễ.
   - _Kiểm tra vật lý:_ Nhân viên kiểm tra thực tế tình trạng cuốn băng (vỏ nhựa, ruy-băng, nhãn dán).
   - _Ghi nhận vi phạm & Phạt:_ Nếu có vi phạm (trễ hạn hoặc hư hỏng), nhân viên ghi nhận mô tả vào cột `TinhTrangBangKhiTra` (VD: _"Vỡ vỏ nhựa trầy xước"_) và thỏa thuận số tiền phạt hợp lý vào cột `TienPhat` (VD: `10,000 VNĐ`).
4. **Bước 4: Tính tổng tiền & Thanh toán.**
   - Hệ thống tự động cộng dồn tổng tiền thu:
     $$\text{Tổng tiền} = \sum (\text{Đơn giá thuê gốc}) + \sum (\text{Tiền phạt})$$
   - Nhân viên thu tiền thanh toán và in biên lai (`PhieuTra`) cho khách hàng.

**Tác động tới Cơ sở dữ liệu (Transaction Backend):**

- Tạo 1 bản ghi vào bảng `PHIEUTRA` với tổng số tiền thu được cùng mã chi nhánh/nhân viên nhận trả.
- Tạo $N$ bản ghi vào bảng `CHITIETPHIEUTRA` ghi nhận rõ tình trạng thực tế và tiền phạt của từng cuốn.
- Cập nhật bản ghi tương ứng trong `CHITIETPHIEUMUON`: Thiết lập `DaTra = true` và `NgayTraThucTe`.
- **Xử lý luân chuyển kho tự động:** Cập nhật bảng `BANSAOBANG`:
  - Đổi trạng thái băng về `"Sẵn sàng"`.
  - Ghi đè `MaCuaHangHienTai` bằng **Mã chi nhánh nơi băng vừa được trả**. (Cuốn băng mượn ở chi nhánh A nay chính thức nhập kho chi nhánh B).

---

### 3.2.4. Đặc tả Use Case — UC07: Quét Băng Quá Hạn & Cảnh Báo (Chạy ngầm / Báo cáo)

| Thành phần         | Nội dung                                                                                      |
| ------------------ | --------------------------------------------------------------------------------------------- |
| **Tên Use Case**   | Quét Băng Quá Hạn & Cảnh Báo                                                                  |
| **Mã**             | UC07                                                                                          |
| **Tác nhân**       | Hệ thống (chạy ngầm định kỳ) / Quản lý cửa hàng                                               |
| **Mô tả**          | Quét toàn bộ dữ liệu giao dịch mượn chưa hoàn tất để phát hiện các trường hợp vi phạm hạn trả |
| **Tiền điều kiện** | Có dữ liệu phiếu mượn đang hoạt động                                                          |
| **Hậu điều kiện**  | Danh sách khách hàng giữ băng quá hạn được tổng hợp để liên hệ nhắc nhở                       |

**Luồng nghiệp vụ chi tiết:**

1. Hệ thống thực hiện truy vấn quét toàn bộ bảng `CHITIETPHIEUMUON` kết hợp với `PHIEUMUON` lọc ra các bản ghi thỏa mãn đồng thời 2 điều kiện:
   - `DaTra = false` (Băng vẫn đang nằm trong tay khách hàng).
   - `NgayDuKienTra < Ngày hiện tại` (Đã vượt quá hạn trả theo hợp đồng mượn).
2. Kết結果 được thực hiện kết nối (`INNER JOIN`) với bảng `KHACHHANG` và `BANSAOBANG` để hiển thị danh sách tổng hợp gồm:
   - Họ tên khách hàng & Số điện thoại liên hệ.
   - Mã bản sao & Tựa đề phim đang giữ.
   - Ngày mượn, Ngày dự kiến trả và **Số ngày trễ hạn thực tế**.
3. Nhân viên hoặc Quản lý chi nhánh sử dụng danh sách này để gọi điện nhắc nhở hoặc xuất thông báo bằng văn bản gửi về địa chỉ khách hàng.

## 3.3. Activity Diagram

### 3.3.1. Activity Diagram — Quy trình Mượn Băng Video

| Bước | Hành động                                    | Tác nhân  |
| ---- | -------------------------------------------- | --------- |
| 1    | Bắt đầu                                      | —         |
| 2    | NV xác nhận thông tin KH                     | Nhân viên |
| 3    | KH có trong hệ thống?                        | Hệ thống  |
| 3a   | Nếu KHÔNG → Đăng ký KH mới → quay lại bước 3 | Nhân viên |
| 4    | Chọn cửa hàng cho mượn                       | Nhân viên |
| 5    | KH chọn băng video cần mượn                  | Nhân viên |
| 6    | Băng có sẵn (TrangThai = 'CoSan')?           | Hệ thống  |
| 6a   | Nếu KHÔNG → Thông báo, quay lại bước 5       | Hệ thống  |
| 7    | Thêm băng vào danh sách mượn                 | Hệ thống  |
| 8    | Mượn thêm cuốn khác?                         | Nhân viên |
| 8a   | Nếu CÓ → quay lại bước 5                     | —         |
| 9    | Nhập ngày dự kiến trả                        | Nhân viên |
| 10   | Tạo Hồ Sơ Mượn + Chi Tiết Mượn               | Hệ thống  |
| 11   | Cập nhật TrangThai băng → "DangMuon"         | Hệ thống  |
| 12   | Kết thúc                                     | —         |

### 3.3.2. Activity Diagram — Quy trình Trả Băng Video

| Bước | Hành động                             | Tác nhân  |
| ---- | ------------------------------------- | --------- |
| 1    | Bắt đầu                               | —         |
| 2    | NV nhập mã KH                         | Nhân viên |
| 3    | Hiển thị danh sách băng đang mượn     | Hệ thống  |
| 4    | KH chọn băng cần trả                  | Nhân viên |
| 5    | Tính tiền thuê: DonGia × Số ngày mượn | Hệ thống  |
| 6    | Trả thêm cuốn khác?                   | Nhân viên |
| 6a   | Nếu CÓ → quay lại bước 4              | —         |
| 7    | Tính tổng tiền                        | Hệ thống  |
| 8    | Tạo Hồ Sơ Trả + Chi Tiết Trả          | Hệ thống  |
| 9    | Cập nhật TrangThai băng → "CoSan"     | Hệ thống  |
| 10   | In Biên Lai Thu Tiền                  | Hệ thống  |
| 11   | Kết thúc                              | —         |

## 3.4. Thiết kế cơ sở dữ liệu thực tế hiện tại (`QuanLyThueBang`)

### 3.4.1. Danh sách 11 bảng thực tế trong hệ thống

| STT | Tên bảng             | Mô tả                                      | Khóa chính                  | Khóa ngoại chính                                       |
| --- | -------------------- | ------------------------------------------ | --------------------------- | ------------------------------------------------------ |
| 1   | **CuaHang**          | Thông tin các chi nhánh cửa hàng           | `MaCuaHang` (VARCHAR(10))   | —                                                      |
| 2   | **KhachHang**        | Thông tin khách hàng thuê băng             | `MaKhachHang` (VARCHAR(20)) | —                                                      |
| 3   | **VaiTro**           | Phân quyền vai trò người dùng              | `MaVaiTro` (INT)            | —                                                      |
| 4   | **NhanVien**         | Tài khoản & thông tin nhân viên            | `MaNhanVien` (VARCHAR(20))  | `MaVaiTro`, `MaCuaHang`                                |
| 5   | **TheLoai**          | Thể loại phim (Hành động, Tình cảm...)     | `MaTheLoai` (INT)           | —                                                      |
| 6   | **Phim**             | Danh mục tựa đề phim gốc                   | `MaPhim` (VARCHAR(20))      | `MaTheLoai`                                            |
| 7   | **BanSaoBang**       | Từng bản sao cuốn băng vật lý (mã RFID)    | `MaBanSao` (VARCHAR(50))    | `MaPhim`, `MaCuaHangHienTai`                           |
| 8   | **PhieuMuon**        | Hồ sơ giao dịch cho mượn băng              | `MaPhieuMuon` (VARCHAR(20)) | `MaKhachHang`, `MaCuaHangMuon`, `MaNhanVienChoMuon`    |
| 9   | **ChiTietPhieuMuon** | Chi tiết từng cuốn băng trong phiếu mượn   | (`MaPhieuMuon`, `MaBanSao`) | `MaPhieuMuon`, `MaBanSao`                              |
| 10  | **PhieuTra**         | Hồ sơ giao dịch nhận trả băng              | `MaPhieuTra` (VARCHAR(20))  | `MaKhachHang`, `MaCuaHangNhanTra`, `MaNhanVienNhanTra` |
| 11  | **ChiTietPhieuTra**  | Chi tiết băng trả, tình trạng và tiền phạt | (`MaPhieuTra`, `MaBanSao`)  | `MaPhieuTra`, `MaBanSao`, `MaPhieuMuon`                |

### 3.4.2. Chi tiết cấu trúc từng bảng thực tế

**1. Bảng CuaHang** (Chi nhánh cửa hàng)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaCuaHang | VARCHAR(10) | PRIMARY KEY | Mã định danh chi nhánh (VD: CH01) |
| TenCuaHang | NVARCHAR(100) | NOT NULL | Tên chi nhánh |
| DiaChi | NVARCHAR(200) | NOT NULL | Địa chỉ chi nhánh |
| SoDienThoai | VARCHAR(20) | NOT NULL | Hotline liên hệ |

**2. Bảng KhachHang** (Khách hàng thành viên)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaKhachHang | VARCHAR(20) | PRIMARY KEY | Mã thành viên (VD: KH001) |
| HoTen | NVARCHAR(100) | NOT NULL | Họ tên khách hàng |
| CMND | VARCHAR(20) | UNIQUE, NOT NULL | CCCD / CMND |
| SoDienThoai | VARCHAR(20) | NOT NULL | Số điện thoại |
| DiaChi | NVARCHAR(200) | | Địa chỉ liên hệ |
| NgayDangKy | DATETIME | DEFAULT GETDATE() | Ngày đăng ký thành viên |

**3. Bảng VaiTro** (Quyền hạn hệ thống)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaVaiTro | INT | PRIMARY KEY | Mã vai trò (1: Quản lý, 2: Nhân viên quầy) |
| TenVaiTro | NVARCHAR(50) | NOT NULL | Tên chức vụ |

**4. Bảng NhanVien** (Nhân viên & Tài khoản đăng nhập)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaNhanVien | VARCHAR(20) | PRIMARY KEY | Mã nhân viên (VD: NV01) |
| HoTen | NVARCHAR(100) | NOT NULL | Họ tên nhân viên |
| TenDangNhap | VARCHAR(50) | UNIQUE, NOT NULL | Tên đăng nhập hệ thống |
| MatKhauHash | VARCHAR(255) | NOT NULL | Mật khẩu tài khoản |
| MaVaiTro | INT | FOREIGN KEY → VaiTro | Vai trò / quyền hạn |
| MaCuaHang | VARCHAR(10) | FOREIGN KEY → CuaHang | Chi nhánh làm việc |

**5. Bảng TheLoai** (Thể loại phim)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaTheLoai | INT | PRIMARY KEY | Mã thể loại |
| TenTheLoai | NVARCHAR(100) | NOT NULL | Tên thể loại (Hành động, Hài hước...) |

**6. Bảng Phim** (Thông tin tựa đề phim)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaPhim | VARCHAR(20) | PRIMARY KEY | Mã tựa đề phim (VD: PHIM001) |
| TuaDe | NVARCHAR(200) | NOT NULL | Tựa đề bộ phim |
| DaoDien | NVARCHAR(100) | | Đạo diễn |
| NamPhatHanh | INT | NOT NULL | Năm phát hành |

**7. Bảng BanSaoBang** (Bản sao cuốn băng vật lý / mã RFID)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaBanSao | VARCHAR(50) | PRIMARY KEY | Mã vạch / thẻ RFID định danh từng cuốn băng |
| MaPhim | VARCHAR(20) | FOREIGN KEY → Phim | Thuộc tựa phim nào |
| MaCuaHangHienTai | VARCHAR(10) | FOREIGN KEY → CuaHang | Kho chi nhánh hiện đang sở hữu (luân chuyển khi trả) |
| SoThuTuBanSao | INT | NOT NULL | Số thứ tự bản sao của phim |
| LoaiBang | VARCHAR(10) | DEFAULT 'PAL' | Định dạng băng (PAL, NTSC) |
| DonGiaThue | DECIMAL(18,2) | NOT NULL | Giá thuê cho mỗi giao dịch |
| NgayHetHan | DATETIME | | Ngày hết hạn sử dụng băng |
| TrangThai | NVARCHAR(50) | DEFAULT 'Sẵn sàng' | Trạng thái: 'Sẵn sàng', 'Đang cho mượn', 'Bảo trì'... |

**8. Bảng PhieuMuon** (Hồ sơ mượn băng)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaPhieuMuon | VARCHAR(20) | PRIMARY KEY | Mã phiếu mượn (VD: PM20260712_01) |
| MaKhachHang | VARCHAR(20) | FOREIGN KEY → KhachHang | Khách hàng mượn |
| MaCuaHangMuon | VARCHAR(10) | FOREIGN KEY → CuaHang | Chi nhánh xuất kho cho mượn |
| MaNhanVienChoMuon | VARCHAR(20) | FOREIGN KEY → NhanVien | Nhân viên lập phiếu mượn |
| NgayMuon | DATETIME | NOT NULL | Ngày giờ lập phiếu |
| NgayDuKienTra | DATETIME | NOT NULL | Ngày hẹn trả băng |

**9. Bảng ChiTietPhieuMuon** (Chi tiết băng mượn)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaPhieuMuon | VARCHAR(20) | PRIMARY KEY (Composite) | Khóa ngoại → PhieuMuon |
| MaBanSao | VARCHAR(50) | PRIMARY KEY (Composite) | Khóa ngoại → BanSaoBang |
| TrangThaiTra | BIT | DEFAULT 0 | Cờ đánh dấu đã trả (0: Chưa trả, 1: Đã trả) |

**10. Bảng PhieuTra** (Hồ sơ nhận trả băng)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaPhieuTra | VARCHAR(20) | PRIMARY KEY | Mã phiếu trả (VD: PT20260714_01) |
| MaKhachHang | VARCHAR(20) | FOREIGN KEY → KhachHang | Khách hàng mang trả |
| MaCuaHangNhanTra | VARCHAR(10) | FOREIGN KEY → CuaHang | Chi nhánh tiếp nhận trả băng |
| MaNhanVienNhanTra | VARCHAR(20) | FOREIGN KEY → NhanVien | Nhân viên xử lý trả băng |
| NgayTra | DATETIME | NOT NULL | Ngày giờ trả thực tế |
| TongTienThu | DECIMAL(18,2) | NOT NULL | Tổng số tiền thu (Tiền thuê + Phạt) |

**11. Bảng ChiTietPhieuTra** (Chi tiết thanh toán & vi phạm)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|--------------|-----------|-------|
| MaPhieuTra | VARCHAR(20) | PRIMARY KEY (Composite) | Khóa ngoại → PhieuTra |
| MaBanSao | VARCHAR(50) | PRIMARY KEY (Composite) | Khóa ngoại → BanSaoBang |
| MaPhieuMuon | VARCHAR(20) | FOREIGN KEY → PhieuMuon | Liên kết chính xác với phiếu mượn gốc |
| TinhTrangBangKhiTra | NVARCHAR(100) | | Ghi nhận tình trạng vật lý khi trả |
| TienThue | DECIMAL(18,2) | NOT NULL | Tiền thuê băng |
| TienPhat | DECIMAL(18,2) | DEFAULT 0 | Tiền phạt trễ hạn hoặc làm hỏng băng |

### 3.4.3. Các quan hệ chính trong CSDL thực tế

- `CuaHang (1) -> (N) NhanVien`, `CuaHang (1) -> (N) BanSaoBang`
- `TheLoai (1) -> (N) Phim (1) -> (N) BanSaoBang`
- `KhachHang (1) -> (N) PhieuMuon (1) -> (N) ChiTietPhieuMuon`
- `KhachHang (1) -> (N) PhieuTra (1) -> (N) ChiTietPhieuTra`
- `BanSaoBang (1) -> (N) ChiTietPhieuMuon`, `BanSaoBang (1) -> (N) ChiTietPhieuTra`

### 3.4.4. Script tạo cơ sở dữ liệu thực tế (`QuanLyThueBang`)

```sql
CREATE DATABASE QuanLyThueBang;
GO
USE QuanLyThueBang;
GO

CREATE TABLE CuaHang (
    MaCuaHang VARCHAR(10) PRIMARY KEY,
    TenCuaHang NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200) NOT NULL,
    SoDienThoai VARCHAR(20) NOT NULL
);

CREATE TABLE KhachHang (
    MaKhachHang VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    CMND VARCHAR(20) UNIQUE NOT NULL,
    SoDienThoai VARCHAR(20) NOT NULL,
    DiaChi NVARCHAR(200),
    NgayDangKy DATETIME DEFAULT GETDATE()
);

CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL
);

CREATE TABLE NhanVien (
    MaNhanVien VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhauHash VARCHAR(255) NOT NULL,
    MaVaiTro INT FOREIGN KEY REFERENCES VaiTro(MaVaiTro),
    MaCuaHang VARCHAR(10) FOREIGN KEY REFERENCES CuaHang(MaCuaHang)
);

CREATE TABLE TheLoai (
    MaTheLoai INT PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL
);

CREATE TABLE Phim (
    MaPhim VARCHAR(20) PRIMARY KEY,
    TuaDe NVARCHAR(200) NOT NULL,
    DaoDien NVARCHAR(100),
    NamPhatHanh INT NOT NULL,
    MaTheLoai INT FOREIGN KEY REFERENCES TheLoai(MaTheLoai),
    DoDaiPhut INT NOT NULL
);

CREATE TABLE BanSaoBang (
    MaBanSao VARCHAR(50) PRIMARY KEY,
    MaPhim VARCHAR(20) FOREIGN KEY REFERENCES Phim(MaPhim),
    MaCuaHangHienTai VARCHAR(10) FOREIGN KEY REFERENCES CuaHang(MaCuaHang),
    SoThuTuBanSao INT NOT NULL,
    LoaiBang VARCHAR(10) DEFAULT 'PAL',
    DonGiaThue DECIMAL(18,2) NOT NULL,
    NgayHetHan DATETIME,
    TrangThai NVARCHAR(50) DEFAULT N'Sẵn sàng'
);

CREATE TABLE PhieuMuon (
    MaPhieuMuon VARCHAR(20) PRIMARY KEY,
    MaKhachHang VARCHAR(20) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
    MaCuaHangMuon VARCHAR(10) FOREIGN KEY REFERENCES CuaHang(MaCuaHang),
    MaNhanVienChoMuon VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNhanVien),
    NgayMuon DATETIME NOT NULL,
    NgayDuKienTra DATETIME NOT NULL
);

CREATE TABLE ChiTietPhieuMuon (
    MaPhieuMuon VARCHAR(20) FOREIGN KEY REFERENCES PhieuMuon(MaPhieuMuon),
    MaBanSao VARCHAR(50) FOREIGN KEY REFERENCES BanSaoBang(MaBanSao),
    TrangThaiTra BIT DEFAULT 0,
    PRIMARY KEY (MaPhieuMuon, MaBanSao)
);

CREATE TABLE PhieuTra (
    MaPhieuTra VARCHAR(20) PRIMARY KEY,
    MaKhachHang VARCHAR(20) FOREIGN KEY REFERENCES KhachHang(MaKhachHang),
    MaCuaHangNhanTra VARCHAR(10) FOREIGN KEY REFERENCES CuaHang(MaCuaHang),
    MaNhanVienNhanTra VARCHAR(20) FOREIGN KEY REFERENCES NhanVien(MaNhanVien),
    NgayTra DATETIME NOT NULL,
    TongTienThu DECIMAL(18,2) NOT NULL
);

CREATE TABLE ChiTietPhieuTra (
    MaPhieuTra VARCHAR(20) FOREIGN KEY REFERENCES PhieuTra(MaPhieuTra),
    MaBanSao VARCHAR(50) FOREIGN KEY REFERENCES BanSaoBang(MaBanSao),
    MaPhieuMuon VARCHAR(20) FOREIGN KEY REFERENCES PhieuMuon(MaPhieuMuon),
    TinhTrangBangKhiTra NVARCHAR(100),
    TienThue DECIMAL(18,2) NOT NULL,
    TienPhat DECIMAL(18,2) DEFAULT 0,
    PRIMARY KEY (MaPhieuTra, MaBanSao)
);
```

## 3.5. Thiết kế giao diện người dùng thực tế (Chuẩn Giao diện 2 - Modern Pastel Card UI)

### 3.5.1. Khung chính điều hướng ứng dụng (`MainShellForm`)

- **Bố cục tổng thể:** Chia thành 2 khu vực rõ ràng:
  - **Sidebar tĩnh bên trái (Chiều rộng 260px):** Nền màu xanh than sang trọng (`#1E232A`), logo phần mềm phía trên cùng và danh sách menu nhóm theo chủ đề (Quản lý Danh mục, Nghiệp vụ Thuê Băng, Hệ thống).
  - **Khung nội dung chính bên phải (`Panel pnlContent`):** Động, tự động nạp các `UserControl` chuyên trách tương ứng với từng menu khi người dùng chọn.
- **Danh sách Menu chính:**
  - `🎬 Quản lý Phim` (`QuanLyPhimControl`)
  - `📂 Quản lý Danh mục` (`QuanLyDanhMucControl`)
  - `📼 Quản lý Bản sao Băng` (`QuanLyBanSaoControl`)
  - `🏪 Quản lý Chi nhánh Cửa hàng` (`QuanLyCuaHangControl`)
  - `👥 Quản lý Khách hàng` (`QuanLyKhachHangControl`)
  - `🧑‍💼 Quản lý Nhân viên` (`QuanLyNhanVienControl`)
  - `📋 Quản lý Phiếu mượn` (`QuanLyPhieuMuonControl`)
  - `📥 Nhận Trả & Luân Chuyển` (`MuonTraBangControl`)

### 3.5.2. Chuẩn mực thiết kế các màn hình Danh mục CRUD

Tất cả các màn hình quản lý danh mục (`QuanLyPhimControl`, `QuanLyBanSaoControl`, `QuanLyKhachHangControl`,...) tuân thủ tuyệt đối quy tắc thiết kế Giao diện 2:

1. **Header Panel phía trên:** Tiêu đề lớn font Segoe UI Semibold 16pt, dòng chú thích phụ màu xám, nút hành động chính (`+ Thêm mới...`) màu đỏ pastel sang trọng (`#B87B7D`) và ô tìm kiếm tức thì.
2. **Bảng dữ liệu `DataGridView` chuẩn hóa:**
   - Chiều cao tiêu đề (`ColumnHeadersHeight`) được thiết lập **46px**, ngăn chặn tuyệt đối tình trạng cắt chữ hiển thị.
   - Chiều cao dòng (`RowTemplate.Height`) thiết lập **44px** tạo không gian thoáng đãng.
   - Cột hành động tích hợp nút trực tiếp trên từng dòng (`Sửa`, `Xóa`).
3. **Cửa sổ nhập liệu (`DialogForm`):** Hiển thị dạng Pop-up bo góc hiện đại, hỗ trợ kiểm tra dữ liệu đầu vào (Validation) chặt chẽ trước khi lưu vào CSDL.

### 3.5.3. Màn hình Quản Lý Phiếu Mượn & Lập Phiếu (`QuanLyPhieuMuonControl`)

Được tách riêng biệt giúp giao diện rõ ràng, mạch lạc với **2 Tab nghiệp vụ chuyên sâu**:
- **Tab 1: 📂 Danh Sách & Tra Cứu Phiếu Mượn (Master-Detail)**
  - Hỗ trợ tìm kiếm tức thì theo Mã Phiếu Mượn hoặc Tên Khách Hàng.
  - Bảng chính phía trên hiển thị danh sách toàn bộ Phiếu Mượn trong hệ thống (Tình trạng: `Đang mượn`, `Đã trả đủ`, `Quá hạn`) kèm nút hủy phiếu sai.
  - Bảng phụ phía dưới hiển thị chi tiết danh sách từng cuốn băng trong phiếu mượn đang chọn.
  - Tích hợp nút `➕ Lập Phiếu Mượn Mới` chuyển sang Tab lập phiếu nhanh chóng.
- **Tab 2: ➕ Lập Phiếu Mượn Mới (Tại Quầy)**
  - Chọn Khách hàng mượn, Chi nhánh xuất kho và Nhân viên quầy.
  - Ô nhập liệu hỗ trợ quét trực tiếp **Mã bản sao (`MaBanSao`) hoặc RFID**.
  - Tự động kiểm tra ràng buộc: Băng phải thuộc chi nhánh giao dịch, trạng thái `Sẵn sàng` và chưa hết hạn sử dụng.
  - Lưới hiển thị Giỏ mượn cùng tổng tiền thuê dự kiến và nút chốt `💾 Chốt Phiếu Mượn`.

### 3.5.4. Màn hình Nhận Trả Băng & Luân Chuyển Kho (`MuonTraBangControl`)

Chuyên trách cho nghiệp vụ thu hồi băng và quản lý kho chéo chi nhánh với **2 Tab độc lập**:
- **Tab 1: 📥 Nhận Trả Băng & Luân Chuyển Kho (Cross-Store Return)**
  - Quét mã bản sao mang trả $\rightarrow$ Tự động truy xuất giao dịch mượn gốc, hiển thị cảnh báo đỏ nếu băng bị **Trễ hạn**.
  - Nút `⚙️ Phạt/Hỏng`: Cho phép nhập tình trạng hư hỏng thực tế và gõ số tiền phạt thủ công.
  - Nút `✅ Chốt Nhận Trả & Luân Chuyển Kho`: Thực thi giao dịch tạo `PhieuTra`, tất toán `ChiTietPhieuMuon`, cập nhật trạng thái băng về `Sẵn sàng` và **tự động luân chuyển kho (`MaCuaHangHienTai` về chi nhánh nhận trả)**.
- **Tab 2: 🚨 Cảnh Báo Băng Quá Hạn**
  - Danh sách cảnh báo toàn bộ khách hàng đang giữ băng lố ngày hẹn trả kèm số ngày trễ hạn thực tế.

---

# CHƯƠNG 4: CÀI ĐẶT VÀ THỰC HIỆN

## 4.1. Môi trường phát triển & Kiến trúc kỹ thuật

- **Ngôn ngữ lập trình:** C# 12 (.NET 8.0 Windows Forms)
- **Hệ quản trị CSDL:** Microsoft SQL Server
- **ORM / Truy xuất dữ liệu:** **Entity Framework Core 8** (Code-First / EF Core DbContext)
- **Quản lý phụ thuộc:** Dependency Injection (`Microsoft.Extensions.DependencyInjection`)

## 4.2. Cấu trúc Solution thực tế (`QuanLyThueBang`)

```
QuanLyThueBang.sln
└── QuanLyThueBang/
    ├── Domain/
    │   ├── Entities/         (11 lớp thực thể: Phim, BanSaoBang, KhachHang, PhieuMuon...)
    │   └── DTOs/             (Các đối tượng truyền dữ liệu DTO và ViewDTO)
    ├── DAL/
    │   └── QuanLyThueBangContext.cs  (EF Core DbContext cấu hình CSDL & chuỗi kết nối)
    ├── BLL/                  (7 Service nghiệp vụ chuyên biệt: MuonTraService, AuthService, PhimService...)
    ├── Presentation/
    │   ├── Forms/            (LoginForm, MainShellForm, PhimEditDialogForm, BanSaoEditDialogForm...)
    │   └── Controls/         (DashboardControl + 7 UserControl quản lý nghiệp vụ và danh mục)
    └── Helpers/              (AppConfig, AppSession, ExportHelper, Constants, SecurityHelper)
```

### 4.2.1. Cài đặt các Module nâng cao
- **Module Báo cáo & Thống kê (`DashboardControl`):** Hiển thị 4 thẻ chỉ số KPI tổng quan (Tổng phim, Bản sao, Phiếu đang mượn, Tổng doanh thu), Bảng xếp hạng Top 5 phim thuê nhiều nhất (Trending) và vẽ trực tiếp Biểu đồ tròn tình trạng kho (`Pie Chart`) sắc nét bằng GDI+.
- **Module Đăng nhập & Phân quyền (`LoginForm` & RBAC):** Kiểm soát quyền truy cập hệ thống theo 3 cấp độ (Admin cấp cao, Quản lý chi nhánh, Nhân viên quầy). Tự động điều chỉnh hiển thị Sidebar tương ứng với quyền hạn của nhân viên đang đăng nhập.
- **Module Xuất & In hóa đơn (`ExportHelper`):** Hỗ trợ xuất Phiếu Mượn ra hóa đơn định dạng HTML/PDF chuyên nghiệp để in trực tiếp cho khách hàng và xuất toàn bộ lưới dữ liệu ra file Excel/CSV chuẩn UTF-8 BOM.

## 4.3. Cài đặt tầng BLL (Business Logic Layer)

Tầng BLL chịu trách nhiệm kiểm soát tính toàn vẹn dữ liệu và giao dịch nghiệp vụ. Điểm nhấn quan trọng nhất là `MuonTraService.cs`:

- **Quản lý Giao dịch (Database Transaction):** Sử dụng `_context.Database.BeginTransaction()` trong phương thức `LapPhieuMuon` và `ChotNhanTraBang` nhằm đảm bảo tính nguyên tử (Atomicity): nếu xảy ra lỗi giữa chừng khi lưu chi tiết phiếu mượn/trả hoặc cập nhật kho băng, toàn bộ giao dịch sẽ tự động Rollback.
- **Thuật toán luân chuyển kho liên chi nhánh:** Khi khách trả băng tại chi nhánh khác nơi mượn, phương thức `ChotNhanTraBang` tự động ghi nhận chi nhánh mới vào `BanSaoBang.MaCuaHangHienTai`.

## 4.4. Kiểm thử hệ thống

### 4.4.1. Các kịch bản kiểm thử nghiệp vụ chính

| STT | Kịch bản kiểm thử (Test Case)             | Điều kiện đầu vào                                        | Kết quả thực tế đạt được                                             | Đánh giá |
| --- | ----------------------------------------- | -------------------------------------------------------- | -------------------------------------------------------------------- | -------- |
| 1   | Quét mượn cuốn băng đang ở chi nhánh khác | Băng có `MaCuaHangHienTai` khác Chi nhánh đang lập phiếu | Hệ thống từ chối cho mượn, hiển thị cảnh báo sai kho chi nhánh       | **Đạt**  |
| 2   | Quét mượn cuốn băng đã hết hạn sử dụng    | Băng có `NgayHetHan < Today`                             | Hệ thống chặn thêm vào giỏ mượn và báo lỗi băng hết hạn              | **Đạt**  |
| 3   | Quét trả băng trễ hạn so với ngày hẹn     | Phiếu mượn có `NgayDuKienTra < Today`                    | Hệ thống tô đỏ cảnh báo số ngày trễ hạn, cho phép nhập phạt thủ công | **Đạt**  |
| 4   | Mượn ở chi nhánh A, trả tại chi nhánh B   | Khách mang băng mượn ở CH01 trả tại CH02                 | Hệ thống tiếp nhận trả, cập nhật `MaCuaHangHienTai = CH02`           | **Đạt**  |
| 5   | Phân quyền hiển thị theo vai trò (RBAC)   | Đăng nhập tài khoản `nhanvien` vs `admin`                | Hệ thống tự động ẩn các menu Quản trị phim/kho với Nhân viên quầy    | **Đạt**  |
| 6   | Xuất & In Hóa đơn Phiếu Mượn PDF          | Bấm nút `🖨️ In Hóa Đơn` trên danh sách Phiếu Mượn        | Hệ thống tạo hóa đơn HTML/PDF đầy đủ logo, bảng băng và chữ ký       | **Đạt**  |

---

# CHƯƠNG 5: KẾT LUẬN & HƯỚNG PHÁT TRIỂN

## 5.1. Kết quả đạt được

1. **Hoàn thiện trọn vẹn đặc tả nghiệp vụ thực tiễn:** Xây dựng thành công quy trình quản lý cho thuê băng video đáp ứng các bài toán hiện đại như: luân chuyển kho liên chi nhánh (Cross-Store Return), quét mã RFID/Mã vạch định danh bản sao, quản lý phạt vi phạm thủ công và tự động báo cáo doanh thu trực quan.
2. **Kiến trúc phần mềm chuẩn mực:** Ứng dụng thành công **.NET 8 WinForms** kết hợp **Entity Framework Core 8**, **RBAC Security** và **Dependency Injection (DI)** theo kiến trúc 3 lớp rõ ràng, giúp mã nguồn an toàn, dễ bảo trì và mở rộng.
3. **Giao diện đẳng cấp cao (Giao diện 2):** Toàn bộ 8 màn hình chức năng (bao gồm Dashboard) sở hữu bảng màu nhã nhặn, chống cắt chữ tuyệt đối, trải nghiệm người dùng liền mạch và trực quan.

## 5.2. Hướng phát triển tương lai

- **Tích hợp IoT máy quét RFID:** Kết nối trực tiếp qua cổng COM/USB để nhận diện hàng loạt băng video trong giỏ chỉ trong 1 giây.
- **Ứng dụng Web / Mobile cho Khách hàng:** Xây dựng portal tra cứu danh mục phim và đặt giữ băng trước tại chi nhánh gần nhất.

---

# TÀI LIỆU THAM KHẢO

[1] Microsoft. (2024). _.NET 8 and Windows Forms Documentation_. https://learn.microsoft.com/en-us/dotnet/desktop/winforms/
[2] Microsoft. (2024). _Entity Framework Core 8 Documentation_. https://learn.microsoft.com/en-us/ef/core/
[3] Troelsen, A., & Japikse, P. (2022). _Pro C# 10 with .NET 6_. Apress.
[4] Connolly, T., & Begg, C. (2015). _Database Systems: A Practical Approach to Design, Implementation, and Management_ (6th Edition). Pearson.

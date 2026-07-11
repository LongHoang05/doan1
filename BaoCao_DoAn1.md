# BÁO CÁO ĐỒ ÁN 1

# HỆ THỐNG QUẢN LÝ CHO THUÊ BĂNG VIDEO

---

**Giảng viên hướng dẫn:** [Tên GVHD]

**Sinh viên thực hiện:**

| STT | Họ và tên | MSSV |
|-----|-----------|------|
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

| Nhóm chức năng | Các chức năng cụ thể |
|----------------|---------------------|
| Quản lý danh mục | CRUD cửa hàng, khách hàng, nhân viên, phim, băng video |
| Nghiệp vụ chính | Mượn băng, trả băng, in biên lai |
| Hỗ trợ | Nhắc trả quá hạn, báo cáo thống kê, tìm kiếm |

### 1.3.2. Giới hạn

- Ứng dụng hoạt động trên nền tảng **Windows Desktop** (không phải web hay mobile).
- Cơ sở dữ liệu được lưu trữ cục bộ trên máy tính (SQL Server Express / SQLite).
- Chưa hỗ trợ phân quyền nâng cao (admin, manager, staff).
- Chưa tích hợp thanh toán điện tử.

## 1.4. Công cụ và công nghệ sử dụng

| Thành phần | Công nghệ / Công cụ | Phiên bản |
|------------|---------------------|-----------|
| Ngôn ngữ lập trình | C# | .NET 8.0 |
| Giao diện người dùng | Windows Forms (WinForms) | .NET 8.0 |
| Cơ sở dữ liệu | Microsoft SQL Server Express | 2019 / 2022 |
| Truy xuất dữ liệu | ADO.NET | .NET 8.0 |
| IDE phát triển | Visual Studio | 2022 |
| Quản lý mã nguồn | Git + GitHub | — |
| Thiết kế báo cáo | Microsoft Word | 2021 / 365 |
| Thiết kế slide | Microsoft PowerPoint | 2021 / 365 |

## 1.5. Kiến trúc phần mềm tổng quan

Hệ thống được xây dựng theo **mô hình 3 lớp (3-Layer Architecture):**

| Lớp | Tên Project | Chức năng |
|-----|-------------|-----------|
| **Presentation Layer** | VideoRental.GUI | Giao diện người dùng (WinForms), nhận input từ người dùng và hiển thị kết quả |
| **Business Logic Layer** | VideoRental.BLL | Xử lý logic nghiệp vụ, kiểm tra ràng buộc, tính toán |
| **Data Access Layer** | VideoRental.DAL | Truy xuất cơ sở dữ liệu (CRUD operations) qua ADO.NET |
| **Data Transfer Objects** | VideoRental.DTO | Các lớp đối tượng dùng để truyền dữ liệu giữa các lớp |

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

| Thành phần | Mô tả | Ví dụ sử dụng trong đồ án |
|------------|-------|---------------------------|
| **Form** | Cửa sổ chính chứa các controls | frmMain, frmKhachHang, frmMuonBang |
| **TextBox** | Ô nhập văn bản | Nhập CMND, tên khách hàng, địa chỉ |
| **ComboBox** | Danh sách thả xuống | Chọn cửa hàng, thể loại phim, loại băng |
| **DataGridView** | Bảng hiển thị dữ liệu | Hiển thị danh sách KH, phim, băng video |
| **Button** | Nút nhấn | Thêm, Sửa, Xóa, Tìm kiếm, Lưu |
| **DateTimePicker** | Chọn ngày tháng | Ngày mượn, ngày dự kiến trả |
| **MenuStrip** | Thanh menu | Menu chính: Quản lý, Nghiệp vụ, Báo cáo |
| **PrintDocument** | Điều khiển in ấn | In biên lai thu tiền |

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

| Lớp | Chức năng |
|-----|-----------|
| **SqlConnection** | Tạo kết nối đến SQL Server |
| **SqlCommand** | Thực thi câu lệnh SQL (SELECT, INSERT, UPDATE, DELETE) |
| **SqlDataReader** | Đọc dữ liệu tuần tự (forward-only, read-only) — hiệu suất cao |
| **SqlDataAdapter** | Cầu nối giữa DataSet và cơ sở dữ liệu |
| **SqlParameter** | Tham số hóa câu lệnh SQL — chống SQL Injection |

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

| Khái niệm | Mô tả | Ví dụ trong đồ án |
|------------|-------|-------------------|
| **Table** | Bảng lưu trữ dữ liệu | KHACHHANG, PHIM, BANGVIDEO |
| **Primary Key (PK)** | Khóa chính — định danh duy nhất mỗi dòng | MaKH, MaPhim |
| **Composite Key** | Khóa phức hợp — gồm nhiều cột | (MaBangGoc, SoThuTu) trong BANGVIDEO |
| **Foreign Key (FK)** | Khóa ngoại — liên kết giữa các bảng | MaCuaHang trong NHANVIEN |
| **IDENTITY** | Tự động tăng giá trị | MaKH INT IDENTITY(1,1) |
| **CONSTRAINT** | Ràng buộc dữ liệu | CHECK, UNIQUE, NOT NULL |
| **INDEX** | Chỉ mục — tăng tốc truy vấn | INDEX trên TrangThai, NgayDuKienTra |

## 2.5. Mô hình kiến trúc 3 lớp (3-Layer Architecture)

### 2.5.1. Khái niệm

Mô hình 3 lớp là một kiến trúc phần mềm chia ứng dụng thành 3 tầng riêng biệt, mỗi tầng có trách nhiệm cụ thể và chỉ giao tiếp với tầng liền kề.

### 2.5.2. Các lớp trong mô hình

**Lớp 1: Presentation Layer (Lớp trình bày)**
- **Chức năng:** Hiển thị giao diện, nhận dữ liệu từ người dùng, trình bày kết quả.
- **Đặc điểm:** KHÔNG chứa logic nghiệp vụ, KHÔNG truy xuất database trực tiếp.
- **Trong đồ án:** Project `VideoRental.GUI` — chứa các WinForms.

**Lớp 2: Business Logic Layer (Lớp xử lý nghiệp vụ)**
- **Chức năng:** Xử lý logic nghiệp vụ, kiểm tra ràng buộc, tính toán.
- **Đặc điểm:** Nhận dữ liệu từ Presentation Layer, xử lý, gọi Data Access Layer.
- **Trong đồ án:** Project `VideoRental.BLL` — chứa các lớp BLL.
- **Ví dụ:** Kiểm tra khách hàng có băng quá hạn không trước khi cho mượn, tính tiền thuê khi trả băng.

**Lớp 3: Data Access Layer (Lớp truy xuất dữ liệu)**
- **Chức năng:** Thao tác trực tiếp với cơ sở dữ liệu (CRUD).
- **Đặc điểm:** KHÔNG chứa logic nghiệp vụ, chỉ thực thi câu lệnh SQL.
- **Trong đồ án:** Project `VideoRental.DAL` — chứa các lớp DAL và DatabaseHelper.

**Lớp bổ trợ: Data Transfer Object (DTO)**
- **Chức năng:** Các lớp đối tượng đơn giản, chỉ chứa thuộc tính (properties), dùng để truyền dữ liệu giữa các lớp.
- **Trong đồ án:** Project `VideoRental.DTO` — chứa các lớp DTO tương ứng với các bảng trong CSDL.

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

| STT | Yêu cầu | Mô tả |
|-----|---------|-------|
| YC01 | Quản lý cửa hàng | Thêm, sửa, xóa, xem danh sách cửa hàng (mã, địa chỉ, SĐT) |
| YC02 | Quản lý khách hàng | Đăng ký KH (nhập CMND, tên, địa chỉ → cấp mã KH), sửa, xóa, tìm kiếm |
| YC03 | Quản lý nhân viên | Đăng ký NV (nhập CMND, tên, địa chỉ → cấp mã NV), phân cửa hàng, sửa, xóa |
| YC04 | Quản lý phim | Thêm, sửa, xóa phim (mã, tựa đề, năm, thể loại, độ dài) |
| YC05 | Quản lý băng video gốc | Thêm, sửa, xóa băng gốc, liên kết với phim |
| YC06 | Quản lý bản sao băng video | Thêm, sửa, xóa bản sao (loại băng, đơn giá, ngày hết hạn, trạng thái) |

**b) Nghiệp vụ chính:**

| STT | Yêu cầu | Mô tả |
|-----|---------|-------|
| YC07 | Mượn băng video | Tạo hồ sơ mượn: chọn KH, chọn các cuốn băng, nhập ngày mượn, ngày dự kiến trả, ghi nhận cửa hàng và nhân viên cho mượn |
| YC08 | Trả băng video | Tạo hồ sơ trả: chọn KH, chọn các cuốn băng cần trả, tính tiền thuê (đơn giá × số ngày), ghi nhận cửa hàng và nhân viên nhận trả |
| YC09 | In biên lai thu tiền | Sau khi trả băng, in biên lai ghi rõ: thông tin KH, danh sách băng trả, số tiền từng cuốn, tổng tiền |
| YC10 | Nhắc trả băng quá hạn | Quét hồ sơ mượn, tìm các trường hợp quá hạn (ngày hiện tại > ngày dự kiến trả), in thông báo nhắc trả |

### 3.1.2. Yêu cầu phi chức năng

| STT | Yêu cầu | Mô tả |
|-----|---------|-------|
| YC-PF01 | Hiệu năng | Thời gian phản hồi < 2 giây cho mỗi thao tác |
| YC-PF02 | Dễ sử dụng | Giao diện trực quan, dễ thao tác cho nhân viên |
| YC-PF03 | Toàn vẹn dữ liệu | Dữ liệu phải nhất quán, có ràng buộc khóa ngoại |
| YC-PF04 | Validate dữ liệu | Kiểm tra đầu vào: không để trống, đúng định dạng, không trùng CMND |
| YC-PF05 | Bảo mật | Chống SQL Injection bằng SqlParameter |

### 3.1.3. Các tác nhân (Actors)

| Tác nhân | Mô tả |
|----------|-------|
| **Nhân viên** | Người dùng chính của hệ thống, thực hiện các thao tác quản lý và nghiệp vụ |
| **Khách hàng** | Đăng ký thông tin, yêu cầu mượn/trả băng (thông qua nhân viên) |
| **Hệ thống** | Tự động quét và tạo thông báo nhắc trả quá hạn |

## 3.2. Sơ đồ Use Case

### 3.2.1. Use Case tổng quát

| Use Case | Tác nhân | Mô tả |
|----------|----------|-------|
| UC01: Đăng ký khách hàng | Nhân viên, Khách hàng | NV nhập thông tin KH, hệ thống cấp mã |
| UC02: Quản lý phim | Nhân viên | CRUD thông tin phim |
| UC03: Quản lý băng video | Nhân viên | CRUD băng gốc và bản sao |
| UC04: Mượn băng video | Nhân viên | Tạo hồ sơ mượn |
| UC05: Trả băng video | Nhân viên | Tạo hồ sơ trả, tính tiền |
| UC06: In biên lai | Nhân viên | In biên lai sau khi trả (extend từ UC05) |
| UC07: Nhắc trả quá hạn | Hệ thống | Tự động quét và in thông báo |
| UC08: Quản lý cửa hàng | Nhân viên | CRUD cửa hàng |
| UC09: Quản lý nhân viên | Nhân viên | CRUD nhân viên |
| UC10: Báo cáo thống kê | Nhân viên | Xem báo cáo doanh thu, thống kê |
| UC11: Tìm kiếm phim | Nhân viên, Khách hàng | Tìm phim theo tựa đề, thể loại |

### 3.2.2. Đặc tả Use Case — UC04: Mượn Băng Video

| Thành phần | Nội dung |
|------------|----------|
| **Tên Use Case** | Mượn băng video |
| **Mã** | UC04 |
| **Tác nhân** | Nhân viên |
| **Mô tả** | Nhân viên tạo hồ sơ mượn cho khách hàng |
| **Tiền điều kiện** | Khách hàng đã đăng ký trong hệ thống |
| **Hậu điều kiện** | Hồ sơ mượn được lưu, trạng thái băng chuyển sang "DangMuon" |

**Luồng chính (Main Flow):**

1. Nhân viên chọn chức năng "Mượn băng".
2. Hệ thống hiển thị form mượn băng.
3. Nhân viên nhập/chọn mã khách hàng.
4. Hệ thống hiển thị thông tin khách hàng.
5. Nhân viên chọn cửa hàng cho mượn.
6. Hệ thống hiển thị danh sách băng video có sẵn tại cửa hàng.
7. Nhân viên chọn các cuốn băng cần mượn.
8. Nhân viên nhập ngày dự kiến trả.
9. Nhân viên nhấn "Lưu".
10. Hệ thống tạo hồ sơ mượn, lưu chi tiết mượn, cập nhật trạng thái các cuốn băng sang "DangMuon".
11. Hệ thống thông báo thành công.

**Luồng phụ (Alternative Flow):**
- **3a.** Khách hàng chưa đăng ký → Hệ thống chuyển sang form đăng ký KH mới.
- **7a.** Cuốn băng đã hết hạn sử dụng hoặc đang được mượn → Hệ thống thông báo và không cho chọn.

### 3.2.3. Đặc tả Use Case — UC05: Trả Băng Video

| Thành phần | Nội dung |
|------------|----------|
| **Tên Use Case** | Trả băng video |
| **Mã** | UC05 |
| **Tác nhân** | Nhân viên |
| **Mô tả** | Nhân viên tạo hồ sơ trả và tính tiền thuê |
| **Tiền điều kiện** | Khách hàng có hồ sơ mượn chưa trả hết |
| **Hậu điều kiện** | Hồ sơ trả được lưu, tiền được tính, biên lai được in, trạng thái băng chuyển về "CoSan" |

**Luồng chính (Main Flow):**

1. Nhân viên chọn chức năng "Trả băng".
2. Hệ thống hiển thị form trả băng.
3. Nhân viên nhập/chọn mã khách hàng.
4. Hệ thống hiển thị danh sách các cuốn băng khách đang mượn (kèm ngày mượn, ngày dự kiến trả, mã hồ sơ mượn).
5. Nhân viên chọn cửa hàng nhận trả và nhân viên nhận trả.
6. Nhân viên chọn các cuốn băng khách trả.
7. Hệ thống tự động tính tiền thuê mỗi cuốn: **Đơn giá × Số ngày mượn** (từ ngày mượn đến ngày trả).
8. Hệ thống hiển thị tổng tiền.
9. Nhân viên nhấn "Lưu & In biên lai".
10. Hệ thống tạo hồ sơ trả, lưu chi tiết trả, cập nhật trạng thái các cuốn băng về "CoSan".
11. Hệ thống in biên lai thu tiền.

### 3.2.4. Đặc tả Use Case — UC07: Nhắc Trả Quá Hạn

| Thành phần | Nội dung |
|------------|----------|
| **Tên Use Case** | Nhắc trả băng quá hạn |
| **Mã** | UC07 |
| **Tác nhân** | Hệ thống (tự động) / Nhân viên (kích hoạt) |
| **Mô tả** | Quét cơ sở dữ liệu, tìm hồ sơ mượn quá hạn, in thông báo |
| **Tiền điều kiện** | Có hồ sơ mượn chưa trả hết trong CSDL |
| **Hậu điều kiện** | Danh sách quá hạn được hiển thị và/hoặc in thông báo |

**Luồng chính:**

1. Nhân viên chọn chức năng "Nhắc trả quá hạn" (hoặc hệ thống tự kích hoạt định kỳ).
2. Hệ thống truy vấn CSDL: tìm tất cả chi tiết mượn (CHITIETMUON) mà cuốn băng tương ứng vẫn còn ở trạng thái "DangMuon" VÀ ngày hiện tại > ngày dự kiến trả (NgayDuKienTra) của hồ sơ mượn.
3. Hệ thống hiển thị danh sách kết quả gồm: thông tin KH, cuốn băng, ngày mượn, ngày dự kiến trả, số ngày quá hạn.
4. Nhân viên có thể chọn in thông báo nhắc trả cho từng khách hàng.

## 3.3. Activity Diagram

### 3.3.1. Activity Diagram — Quy trình Mượn Băng Video

| Bước | Hành động | Tác nhân |
|------|-----------|----------|
| 1 | Bắt đầu | — |
| 2 | NV xác nhận thông tin KH | Nhân viên |
| 3 | KH có trong hệ thống? | Hệ thống |
| 3a | Nếu KHÔNG → Đăng ký KH mới → quay lại bước 3 | Nhân viên |
| 4 | Chọn cửa hàng cho mượn | Nhân viên |
| 5 | KH chọn băng video cần mượn | Nhân viên |
| 6 | Băng có sẵn (TrangThai = 'CoSan')? | Hệ thống |
| 6a | Nếu KHÔNG → Thông báo, quay lại bước 5 | Hệ thống |
| 7 | Thêm băng vào danh sách mượn | Hệ thống |
| 8 | Mượn thêm cuốn khác? | Nhân viên |
| 8a | Nếu CÓ → quay lại bước 5 | — |
| 9 | Nhập ngày dự kiến trả | Nhân viên |
| 10 | Tạo Hồ Sơ Mượn + Chi Tiết Mượn | Hệ thống |
| 11 | Cập nhật TrangThai băng → "DangMuon" | Hệ thống |
| 12 | Kết thúc | — |

### 3.3.2. Activity Diagram — Quy trình Trả Băng Video

| Bước | Hành động | Tác nhân |
|------|-----------|----------|
| 1 | Bắt đầu | — |
| 2 | NV nhập mã KH | Nhân viên |
| 3 | Hiển thị danh sách băng đang mượn | Hệ thống |
| 4 | KH chọn băng cần trả | Nhân viên |
| 5 | Tính tiền thuê: DonGia × Số ngày mượn | Hệ thống |
| 6 | Trả thêm cuốn khác? | Nhân viên |
| 6a | Nếu CÓ → quay lại bước 4 | — |
| 7 | Tính tổng tiền | Hệ thống |
| 8 | Tạo Hồ Sơ Trả + Chi Tiết Trả | Hệ thống |
| 9 | Cập nhật TrangThai băng → "CoSan" | Hệ thống |
| 10 | In Biên Lai Thu Tiền | Hệ thống |
| 11 | Kết thúc | — |

## 3.4. Thiết kế cơ sở dữ liệu

### 3.4.1. Danh sách bảng

| STT | Tên bảng | Mô tả | Khóa chính |
|-----|----------|-------|------------|
| 1 | CUAHANG | Cửa hàng | MaCH (IDENTITY) |
| 2 | KHACHHANG | Khách hàng | MaKH (IDENTITY) |
| 3 | NHANVIEN | Nhân viên | MaNV (IDENTITY) |
| 4 | PHIM | Phim | MaPhim (IDENTITY) |
| 5 | BANGVIDEOGOC | Băng video gốc | MaBangGoc (IDENTITY) |
| 6 | BANGVIDEO | Bản sao băng video | (MaBangGoc, SoThuTu) — khóa phức hợp |
| 7 | HOSOMUON | Hồ sơ mượn | MaMuon (IDENTITY) |
| 8 | CHITIETMUON | Chi tiết mượn | (MaMuon, MaBangGoc, SoThuTu) |
| 9 | HOSOTRA | Hồ sơ trả | MaTra (IDENTITY) |
| 10 | CHITIETTRA | Chi tiết trả | (MaTra, MaBangGoc, SoThuTu) |
| 11 | NHATKY_HETHONG | Nhật ký hệ thống | MaLog (IDENTITY) |
| 12 | CAUHINH_HETHONG | Cấu hình tham số | MaCauHinh |

### 3.4.2. Chi tiết từng bảng

**Bảng CUAHANG**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaCH | INT IDENTITY(1,1) | PRIMARY KEY | Mã cửa hàng |
| DiaChi | NVARCHAR(200) | NOT NULL | Địa chỉ cửa hàng |
| SDT | VARCHAR(15) | NOT NULL | Số điện thoại |

**Bảng KHACHHANG**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaKH | INT IDENTITY(1,1) | PRIMARY KEY | Mã khách hàng (công ty cấp) |
| CMND | VARCHAR(20) | NOT NULL, UNIQUE | Số CMND |
| HoTen | NVARCHAR(100) | NOT NULL | Họ tên |
| DiaChi | NVARCHAR(200) | NOT NULL | Địa chỉ |
| DaXoa | BIT | NOT NULL, DEFAULT 0 | Cờ xóa mềm |

**Bảng NHANVIEN**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaNV | INT IDENTITY(1,1) | PRIMARY KEY | Mã nhân viên (công ty cấp) |
| CMND | VARCHAR(20) | NOT NULL, UNIQUE | Số CMND |
| HoTen | NVARCHAR(100) | NOT NULL | Họ tên |
| DiaChi | NVARCHAR(200) | NOT NULL | Địa chỉ |
| MaCuaHang | INT | NOT NULL, FK → CUAHANG(MaCH) | Cửa hàng trực thuộc |
| TenDangNhap | VARCHAR(50) | UNIQUE | Tên tài khoản |
| MatKhau | VARCHAR(255) | | Mật khẩu (mã hóa) |
| VaiTro | VARCHAR(20) | NOT NULL, DEFAULT 'Staff' | Quyền (Admin/Staff) |
| DaXoa | BIT | NOT NULL, DEFAULT 0 | Cờ xóa mềm |

**Bảng PHIM**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaPhim | INT IDENTITY(1,1) | PRIMARY KEY | Mã phim |
| TuaDe | NVARCHAR(200) | NOT NULL | Tựa đề phim |
| NamPhatHanh | INT | NOT NULL | Năm phát hành |
| TheLoai | NVARCHAR(50) | NOT NULL | Thể loại |
| DoDai | INT | NOT NULL | Độ dài (phút) |
| DaXoa | BIT | NOT NULL, DEFAULT 0 | Cờ xóa mềm |

**Bảng BANGVIDEOGOC**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaBangGoc | INT IDENTITY(1,1) | PRIMARY KEY | Mã băng gốc |
| MaPhim | INT | NOT NULL, FK → PHIM(MaPhim) | Phim chứa trong băng gốc |

Quan hệ PHIM → BANGVIDEOGOC là **1:N** (một phim có thể có nhiều băng gốc — ví dụ bản phát hành khác nhau).

**Bảng BANGVIDEO (Bản sao)**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaBangGoc | INT | PK (composite), FK → BANGVIDEOGOC | Thuộc băng gốc nào |
| SoThuTu | INT | PK (composite) | Số thứ tự bản sao (1, 2, 3, ...) |
| MaCuaHang | INT | NOT NULL, FK → CUAHANG(MaCH) | Cửa hàng sở hữu |
| LoaiBang | VARCHAR(10) | NOT NULL | Loại băng: PAL, NTSC, ... |
| DonGia | DECIMAL(10,2) | NOT NULL | Đơn giá cho thuê (VNĐ/ngày) |
| NgayHetHan | DATE | NOT NULL | Ngày hết hạn sử dụng |
| TrangThai | VARCHAR(20) | NOT NULL, DEFAULT 'CoSan', CHECK | Trạng thái: CoSan, DangMuon, HetHan |

Khóa chính là khóa phức hợp **(MaBangGoc, SoThuTu)** — đúng theo yêu cầu đề bài: "được định danh bằng số (1,2,3,…) cho mỗi bản sao của băng gốc".

**Bảng HOSOMUON**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaMuon | INT IDENTITY(1,1) | PRIMARY KEY | Mã hồ sơ mượn |
| MaKH | INT | NOT NULL, FK → KHACHHANG | Khách hàng mượn |
| MaCuaHang | INT | NOT NULL, FK → CUAHANG | Cửa hàng cho mượn |
| MaNV | INT | NOT NULL, FK → NHANVIEN | Nhân viên cho mượn |
| NgayMuon | DATE | NOT NULL | Ngày mượn |
| NgayDuKienTra | DATE | NOT NULL | Ngày dự kiến trả |

**Bảng CHITIETMUON**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaMuon | INT | PK (composite), FK → HOSOMUON | Hồ sơ mượn |
| MaBangGoc | INT | PK (composite), FK → BANGVIDEO | Phần 1 khóa băng video |
| SoThuTu | INT | PK (composite), FK → BANGVIDEO | Phần 2 khóa băng video |

**Bảng HOSOTRA**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaTra | INT IDENTITY(1,1) | PRIMARY KEY | Mã hồ sơ trả |
| MaKH | INT | NOT NULL, FK → KHACHHANG | Khách hàng trả |
| MaCuaHang | INT | NOT NULL, FK → CUAHANG | Cửa hàng nhận trả |
| MaNV | INT | NOT NULL, FK → NHANVIEN | Nhân viên nhận trả |
| NgayTra | DATE | NOT NULL | Ngày trả |
| TongTien | DECIMAL(12,2) | NOT NULL, DEFAULT 0 | Tổng tiền thuê |

**Bảng CHITIETTRA**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaTra | INT | PK (composite), FK → HOSOTRA | Hồ sơ trả |
| MaBangGoc | INT | PK (composite), FK → BANGVIDEO | Phần 1 khóa băng video |
| SoThuTu | INT | PK (composite), FK → BANGVIDEO | Phần 2 khóa băng video |
| MaMuon | INT | FK → CHITIETMUON | Liên kết chính xác với lần mượn |
| TienThue | DECIMAL(10,2) | NOT NULL | Tiền thuê tính theo số ngày |

**Bảng NHATKY_HETHONG**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaLog | INT IDENTITY(1,1) | PRIMARY KEY | Mã lưu vết |
| ThoiGian | DATETIME | NOT NULL | Thời điểm thao tác |
| MaNV | INT | NOT NULL, FK → NHANVIEN | Người thực hiện |
| HanhDong | NVARCHAR(100) | NOT NULL | Hành động (VD: ĐĂNG NHẬP) |
| ChiTiet | NVARCHAR(500) | NOT NULL | Chi tiết thao tác |

**Bảng CAUHINH_HETHONG**

| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|-----|---------------|-----------|-------|
| MaCauHinh | VARCHAR(50) | PRIMARY KEY | Key (VD: MAX_RENT_DAYS) |
| GiaTri | NVARCHAR(100) | NOT NULL | Giá trị |
| MoTa | NVARCHAR(200) | NOT NULL | Mô tả chi tiết |
| KieuDuLieu | VARCHAR(20) | NOT NULL | Để ép kiểu trong Code |

### 3.4.3. Bảng quan hệ giữa các bảng

| Quan hệ | Loại | Giải thích |
|---------|------|------------|
| CUAHANG → NHANVIEN | 1:N | Một cửa hàng có nhiều nhân viên |
| CUAHANG → BANGVIDEO | 1:N | Một cửa hàng sở hữu nhiều bản sao |
| PHIM → BANGVIDEOGOC | 1:N | Một phim có thể có nhiều băng gốc |
| BANGVIDEOGOC → BANGVIDEO | 1:N | Một băng gốc có nhiều bản sao |
| KHACHHANG → HOSOMUON | 1:N | Một KH có nhiều lần mượn |
| HOSOMUON → CHITIETMUON | 1:N | Một hồ sơ mượn gồm nhiều cuốn băng |
| BANGVIDEO → CHITIETMUON | 1:N | Một cuốn băng có thể được mượn nhiều lần |
| KHACHHANG → HOSOTRA | 1:N | Một KH có nhiều lần trả |
| HOSOTRA → CHITIETTRA | 1:N | Một hồ sơ trả gồm nhiều cuốn băng |
| BANGVIDEO → CHITIETTRA | 1:N | Một cuốn băng có thể được trả nhiều lần |
| HOSOMUON → CHITIETTRA | 1:N | Một hồ sơ mượn liên kết với nhiều chi tiết trả |
| NHANVIEN → HOSOMUON | 1:N | Một NV cho mượn nhiều lần |
| NHANVIEN → HOSOTRA | 1:N | Một NV nhận trả nhiều lần |
| CUAHANG → HOSOMUON | 1:N | Một CH có nhiều hồ sơ mượn |
| CUAHANG → HOSOTRA | 1:N | Một CH có nhiều hồ sơ trả |

### 3.4.4. Ràng buộc toàn vẹn

| Ràng buộc | Bảng | Mô tả |
|-----------|------|-------|
| UNIQUE(CMND) | KHACHHANG | Không trùng số CMND khách hàng |
| UNIQUE(CMND) | NHANVIEN | Không trùng số CMND nhân viên |
| CHECK(TrangThai IN (...)) | BANGVIDEO | Chỉ nhận giá trị: 'CoSan', 'DangMuon', 'HetHan' |
| DEFAULT 'CoSan' | BANGVIDEO.TrangThai | Mặc định băng mới ở trạng thái có sẵn |
| NOT NULL | Tất cả cột FK | Đảm bảo mọi khóa ngoại phải có giá trị |

### 3.4.5. Script tạo cơ sở dữ liệu

```sql
-- =============================================
-- TẠO DATABASE QUẢN LÝ CHO THUÊ BĂNG VIDEO
-- =============================================

CREATE DATABASE QuanLyBangVideo;
GO
USE QuanLyBangVideo;
GO

-- BẢNG CỬA HÀNG
CREATE TABLE CUAHANG (
    MaCH        INT IDENTITY(1,1) PRIMARY KEY,
    DiaChi      NVARCHAR(200)   NOT NULL,
    SDT         VARCHAR(15)     NOT NULL
);

-- BẢNG KHÁCH HÀNG
CREATE TABLE KHACHHANG (
    MaKH        INT IDENTITY(1,1) PRIMARY KEY,
    CMND        VARCHAR(20)     NOT NULL UNIQUE,
    HoTen       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NOT NULL
);

-- BẢNG NHÂN VIÊN
CREATE TABLE NHANVIEN (
    MaNV        INT IDENTITY(1,1) PRIMARY KEY,
    CMND        VARCHAR(20)     NOT NULL UNIQUE,
    HoTen       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NOT NULL,
    MaCuaHang   INT             NOT NULL,
    CONSTRAINT FK_NV_CuaHang FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH)
);

-- BẢNG PHIM
CREATE TABLE PHIM (
    MaPhim      INT IDENTITY(1,1) PRIMARY KEY,
    TuaDe       NVARCHAR(200)   NOT NULL,
    NamPhatHanh INT             NOT NULL,
    TheLoai     NVARCHAR(50)    NOT NULL,
    DoDai       INT             NOT NULL
);

-- BẢNG BĂNG VIDEO GỐC
CREATE TABLE BANGVIDEOGOC (
    MaBangGoc   INT IDENTITY(1,1) PRIMARY KEY,
    MaPhim      INT             NOT NULL,
    CONSTRAINT FK_BVG_Phim FOREIGN KEY (MaPhim) REFERENCES PHIM(MaPhim)
);

-- BẢNG BĂNG VIDEO (bản sao) — Khóa phức hợp
CREATE TABLE BANGVIDEO (
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,
    MaCuaHang   INT             NOT NULL,
    LoaiBang    VARCHAR(10)     NOT NULL,
    DonGia      DECIMAL(10,2)   NOT NULL,
    NgayHetHan  DATE            NOT NULL,
    TrangThai   VARCHAR(20)     NOT NULL DEFAULT 'CoSan',

    CONSTRAINT PK_BangVideo PRIMARY KEY (MaBangGoc, SoThuTu),
    CONSTRAINT FK_BV_BangGoc FOREIGN KEY (MaBangGoc)
        REFERENCES BANGVIDEOGOC(MaBangGoc),
    CONSTRAINT FK_BV_CuaHang FOREIGN KEY (MaCuaHang)
        REFERENCES CUAHANG(MaCH),
    CONSTRAINT CK_TrangThai CHECK (TrangThai IN ('CoSan', 'DangMuon', 'HetHan'))
);

-- BẢNG HỒ SƠ MƯỢN
CREATE TABLE HOSOMUON (
    MaMuon      INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        INT             NOT NULL,
    MaCuaHang   INT             NOT NULL,
    MaNV        INT             NOT NULL,
    NgayMuon    DATE            NOT NULL,
    NgayDuKienTra DATE          NOT NULL,

    CONSTRAINT FK_HSM_KH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    CONSTRAINT FK_HSM_CH FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH),
    CONSTRAINT FK_HSM_NV FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV)
);

-- BẢNG CHI TIẾT MƯỢN
CREATE TABLE CHITIETMUON (
    MaMuon      INT             NOT NULL,
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,

    CONSTRAINT PK_ChiTietMuon PRIMARY KEY (MaMuon, MaBangGoc, SoThuTu),
    CONSTRAINT FK_CTM_HSM FOREIGN KEY (MaMuon) REFERENCES HOSOMUON(MaMuon),
    CONSTRAINT FK_CTM_BV FOREIGN KEY (MaBangGoc, SoThuTu)
        REFERENCES BANGVIDEO(MaBangGoc, SoThuTu)
);

-- BẢNG HỒ SƠ TRẢ
CREATE TABLE HOSOTRA (
    MaTra       INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        INT             NOT NULL,
    MaCuaHang   INT             NOT NULL,
    MaNV        INT             NOT NULL,
    NgayTra     DATE            NOT NULL,
    TongTien    DECIMAL(12,2)   NOT NULL DEFAULT 0,

    CONSTRAINT FK_HST_KH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    CONSTRAINT FK_HST_CH FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH),
    CONSTRAINT FK_HST_NV FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV)
);

-- BẢNG CHI TIẾT TRẢ
CREATE TABLE CHITIETTRA (
    MaTra       INT             NOT NULL,
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,
    MaMuon      INT             NOT NULL,
    TienThue    DECIMAL(10,2)   NOT NULL,

    CONSTRAINT PK_ChiTietTra PRIMARY KEY (MaTra, MaBangGoc, SoThuTu),
    CONSTRAINT FK_CTT_HST FOREIGN KEY (MaTra) REFERENCES HOSOTRA(MaTra),
    CONSTRAINT FK_CTT_BV FOREIGN KEY (MaBangGoc, SoThuTu)
        REFERENCES BANGVIDEO(MaBangGoc, SoThuTu),
    CONSTRAINT FK_CTT_HSM FOREIGN KEY (MaMuon) REFERENCES HOSOMUON(MaMuon)
);

-- INDEX PHỤ TRỢ
CREATE INDEX IX_BangVideo_TrangThai ON BANGVIDEO(TrangThai);
CREATE INDEX IX_HoSoMuon_MaKH ON HOSOMUON(MaKH);
CREATE INDEX IX_HoSoMuon_NgayDuKienTra ON HOSOMUON(NgayDuKienTra);
CREATE INDEX IX_HoSoTra_MaKH ON HOSOTRA(MaKH);
```

## 3.5. Thiết kế giao diện

### 3.5.1. Cấu trúc menu chính (frmMain)

```
┌─────────────────────────────────────────────────────────────┐
│  📁 Quản lý  │  📋 Nghiệp vụ  │  📊 Báo cáo  │  ❓ Trợ giúp │
│  ├ Cửa hàng  │  ├ Mượn băng    │  ├ Thống kê   │  ├ Hướng dẫn │
│  ├ Khách hàng│  ├ Trả băng     │  └ Nhắc trả   │  └ Giới thiệu│
│  ├ Nhân viên │  └ In biên lai  │               │              │
│  ├ Phim      │                 │               │              │
│  └ Băng video│                 │               │              │
└─────────────────────────────────────────────────────────────┘
```

### 3.5.2. Mô tả layout form CRUD (mẫu: frmKhachHang)

```
┌──────────────────────────────────────────────────────┐
│  QUẢN LÝ KHÁCH HÀNG                                 │
├──────────────────────────────────────────────────────┤
│  Mã KH:    [________]    (tự động)                   │
│  CMND:     [________________]                        │
│  Họ tên:   [________________]                        │
│  Địa chỉ:  [________________]                        │
│                                                      │
│  [Thêm]  [Sửa]  [Xóa]  [Làm mới]                   │
│                                                      │
│  Tìm kiếm: [____________] [Tìm]                     │
│                                                      │
│  ┌──────────────────────────────────────────────┐    │
│  │ MaKH │ CMND         │ HoTen        │ DiaChi │    │
│  │------│--------------│--------------│--------│    │
│  │ 1    │ 079123456789 │ Nguyễn Văn An│ Q.1    │    │
│  │ 2    │ 079234567890 │ Trần Thị Bình│ Q.3    │    │
│  └──────────────────────────────────────────────┘    │
└──────────────────────────────────────────────────────┘
```

### 3.5.3. Mô tả layout form Mượn Băng (frmMuonBang)

```
┌──────────────────────────────────────────────────────┐
│  MƯỢN BĂNG VIDEO                                     │
├──────────────────────────────────────────────────────┤
│  Mã mượn:     [________]  (tự động)                  │
│  Khách hàng:  [Chọn KH___________]                   │
│  Cửa hàng:    [Chọn CH___________]                   │
│  Nhân viên:   [Chọn NV___________]                   │
│  Ngày mượn:   [07/07/2026]                           │
│  Ngày dự kiến trả: [14/07/2026]                      │
│                                                      │
│  -- Danh sách băng có sẵn -------------------------  │
│  │ Chọn │ BangGoc │ STT │ Phim       │ Giá    │     │
│  │  [x] │ 1       │ 1   │ Inception  │ 15,000 │     │
│  │  [ ] │ 1       │ 2   │ Inception  │ 15,000 │     │
│  │  [x] │ 2       │ 1   │ Dark Knight│ 12,000 │     │
│                                                      │
│  -- Băng đã chọn --------------------------------   │
│  │ BangGoc │ STT │ Phim        │ DonGia  │          │
│  │ 1       │ 1   │ Inception   │ 15,000  │          │
│  │ 2       │ 1   │ Dark Knight │ 12,000  │          │
│                                                      │
│  [Lưu hồ sơ mượn]  [Hủy]                            │
└──────────────────────────────────────────────────────┘
```

### 3.5.4. Mô tả layout form Trả Băng (frmTraBang)

```
┌──────────────────────────────────────────────────────┐
│  TRẢ BĂNG VIDEO                                      │
├──────────────────────────────────────────────────────┤
│  Khách hàng:  [Chọn KH___________]                   │
│  Cửa hàng:    [Chọn CH___________]                   │
│  Nhân viên:   [Chọn NV___________]                   │
│  Ngày trả:    [14/07/2026]                           │
│                                                      │
│  -- Danh sách băng đang mượn ---------------------   │
│  │Chọn│MaMuon│BangGoc│STT│Phim      │NgàyMượn│Hạn  │
│  │ [x]│ 1    │ 1     │ 1 │Inception │07/07   │14/07│
│  │ [x]│ 1    │ 2     │ 1 │DarkKnight│07/07   │14/07│
│                                                      │
│  -- Chi tiết tính tiền ---------------------------   │
│  │ Phim        │ DonGia  │ SốNgày │ ThànhTiền │     │
│  │ Inception   │ 15,000  │ 7      │ 105,000   │     │
│  │ Dark Knight │ 12,000  │ 7      │  84,000   │     │
│  │--------------------------------------------|     │
│  │                    TỔNG TIỀN: 189,000 VNĐ   │     │
│                                                      │
│  [Lưu va In biên lai]  [Hủy]                        │
└──────────────────────────────────────────────────────┘
```

### 3.5.5. Mô tả layout Biên Lai Thu Tiền

```
┌──────────────────────────────────────────┐
│         BIÊN LAI THU TIỀN               │
│     Công ty Cho thuê Băng Video         │
│-----------------------------------------│
│  Số biên lai:  BL-0001                   │
│  Ngày:         14/07/2026                │
│  Khách hàng:   Nguyễn Văn An            │
│  Cửa hàng:     CH1 - 123 Nguyễn Huệ    │
│  Nhân viên:    Võ Thanh Phong           │
│-----------------------------------------│
│  STT │ Tên phim     │ Ngày │ Giá    │ Tiền    │
│  1   │ Inception    │ 7    │ 15,000 │ 105,000 │
│  2   │ Dark Knight  │ 7    │ 12,000 │  84,000 │
│-----------------------------------------│
│           TỔNG CỘNG:        189,000 VNĐ │
│-----------------------------------------│
│  Chữ ký nhân viên     Chữ ký khách hàng│
└──────────────────────────────────────────┘
```

---

# CHƯƠNG 4: CÀI ĐẶT VÀ THỰC HIỆN

## 4.1. Cài đặt môi trường

### 4.1.1. Yêu cầu phần cứng
- CPU: Intel Core i3 trở lên
- RAM: 4 GB trở lên
- Ổ cứng: 2 GB trống
- Hệ điều hành: Windows 10 / 11

### 4.1.2. Yêu cầu phần mềm
- .NET 8.0 Runtime
- SQL Server Express 2019/2022
- SQL Server Management Studio (SSMS)

### 4.1.3. Hướng dẫn cài đặt
1. Cài đặt SQL Server Express từ trang chủ Microsoft.
2. Mở SSMS, kết nối đến SQL Server.
3. Chạy script `CreateDatabase.sql` để tạo cơ sở dữ liệu.
4. Chạy script `SeedData.sql` để thêm dữ liệu mẫu.
5. Chạy file `VideoRental.GUI.exe` để khởi động ứng dụng.

## 4.2. Cấu trúc Solution

```
VideoRentalManagement.sln
├── VideoRental.DTO      (Data Transfer Objects)
├── VideoRental.DAL      (Data Access Layer)
├── VideoRental.BLL      (Business Logic Layer)
└── VideoRental.GUI      (Presentation Layer - WinForms)
```

## 4.3. Tầng DTO — Các đối tượng truyền dữ liệu

*(Phần này sẽ bổ sung code minh họa cho mỗi DTO class sau khi lập trình xong)*

## 4.4. Tầng DAL — Truy xuất dữ liệu

### 4.4.1. DatabaseHelper — Lớp kết nối cơ sở dữ liệu

*(Phần này sẽ bổ sung code DatabaseHelper.cs)*

### 4.4.2. Ví dụ: KhachHangDAL

*(Phần này sẽ bổ sung code với các phương thức: GetAll, GetById, Insert, Update, Delete)*

## 4.5. Tầng BLL — Xử lý nghiệp vụ

### 4.5.1. MuonBangBLL — Xử lý nghiệp vụ mượn băng

*(Phần này sẽ bổ sung code MuonBangBLL.cs)*

### 4.5.2. TraBangBLL — Xử lý nghiệp vụ trả băng và tính tiền

*(Phần này sẽ bổ sung code TraBangBLL.cs)*

## 4.6. Tầng GUI — Giao diện người dùng

### 4.6.1. Form chính (frmMain)
*(Screenshot + mô tả chức năng)*

### 4.6.2. Quản lý cửa hàng (frmCuaHang)
*(Screenshot + mô tả chức năng)*

### 4.6.3. Quản lý khách hàng (frmKhachHang)
*(Screenshot + mô tả chức năng)*

### 4.6.4. Quản lý nhân viên (frmNhanVien)
*(Screenshot + mô tả chức năng)*

### 4.6.5. Quản lý phim (frmPhim)
*(Screenshot + mô tả chức năng)*

### 4.6.6. Quản lý băng video (frmBangVideo)
*(Screenshot + mô tả chức năng)*

### 4.6.7. Mượn băng video (frmMuonBang)
*(Screenshot + mô tả chức năng)*

### 4.6.8. Trả băng video (frmTraBang)
*(Screenshot + mô tả chức năng)*

### 4.6.9. In biên lai (frmBienLai)
*(Screenshot + mô tả chức năng)*

### 4.6.10. Nhắc trả quá hạn (frmNhacTra)
*(Screenshot + mô tả chức năng)*

### 4.6.11. Báo cáo thống kê (frmBaoCao)
*(Screenshot + mô tả chức năng)*

## 4.7. Kết quả kiểm thử

### 4.7.1. Kiểm thử chức năng

| STT | Test case | Kết quả mong đợi | Kết quả thực tế | Đạt |
|-----|-----------|-------------------|-----------------|-----|
| 1 | Thêm KH mới với CMND hợp lệ | KH được thêm, cấp mã tự động | | |
| 2 | Thêm KH với CMND đã tồn tại | Báo lỗi trùng CMND | | |
| 3 | Thêm KH với tên để trống | Báo lỗi validate | | |
| 4 | Mượn băng đang có sẵn | Hồ sơ tạo OK, trạng thái → DangMuon | | |
| 5 | Mượn băng đang cho mượn | Không cho chọn, báo lỗi | | |
| 6 | Trả băng, tính tiền đúng | Tiền = Đơn giá × Số ngày | | |
| 7 | Trả băng quá hạn | Tính tiền đúng theo số ngày thực tế | | |
| 8 | Nhắc trả: có hồ sơ quá hạn | Hiển thị danh sách quá hạn | | |
| 9 | Nhắc trả: không có quá hạn | Thông báo "Không có hồ sơ quá hạn" | | |
| 10 | Xóa KH đang có hồ sơ mượn | Báo lỗi, không cho xóa | | |

### 4.7.2. Kiểm thử luồng hoàn chỉnh

*(Mô tả chi tiết test flow: Đăng ký KH → Mượn băng → Trả băng → In biên lai → Kiểm tra nhắc trả)*

---

# CHƯƠNG 5: KẾT LUẬN

## 5.1. Kết quả đạt được

### 5.1.1. Về mặt chức năng

Hệ thống đã hiện thực thành công các chức năng chính theo yêu cầu đề bài:

- Quản lý cửa hàng, khách hàng, nhân viên (CRUD đầy đủ).
- Quản lý phim và băng video (bao gồm băng gốc và bản sao).
- Nghiệp vụ mượn băng video (tạo hồ sơ, cập nhật trạng thái).
- Nghiệp vụ trả băng video (tính tiền, in biên lai).
- Thông báo nhắc trả băng quá hạn.
- Báo cáo thống kê cơ bản.

### 5.1.2. Về mặt kỹ thuật

- Áp dụng thành công mô hình kiến trúc 3 lớp (3-Layer Architecture).
- Thiết kế cơ sở dữ liệu đúng chuẩn hóa, đầy đủ ràng buộc toàn vẹn.
- Sử dụng ADO.NET với SqlParameter phòng chống SQL Injection.
- Giao diện WinForms trực quan, dễ sử dụng.

### 5.1.3. Về mặt học thuật

- Hiểu rõ quy trình phân tích và thiết kế hệ thống thông tin.
- Nắm vững kiến trúc phần mềm 3 lớp.
- Thực hành lập trình C# với WinForms và ADO.NET.
- Rèn luyện kỹ năng làm việc nhóm và quản lý dự án.

## 5.2. Hạn chế

- Chưa có phân quyền người dùng (admin, manager, staff) — tất cả nhân viên đều có quyền như nhau.
- Chưa hỗ trợ nhiều người dùng đồng thời (multi-user / networking).
- Giao diện chưa được tối ưu về mặt UX/UI, còn đơn giản.
- Chưa có chức năng sao lưu và phục hồi dữ liệu (backup/restore).
- Chưa tích hợp thanh toán điện tử.
- Báo cáo thống kê còn cơ bản, chưa có biểu đồ trực quan.

## 5.3. Hướng phát triển

- **Phân quyền:** Thêm hệ thống đăng nhập và phân quyền (Admin quản lý toàn bộ, Staff chỉ thao tác nghiệp vụ).
- **Web hóa:** Chuyển đổi sang ứng dụng web ASP.NET Core để hỗ trợ truy cập từ xa.
- **Mobile app:** Phát triển ứng dụng mobile cho khách hàng tra cứu phim, đặt mượn online.
- **Báo cáo nâng cao:** Tích hợp biểu đồ (Chart) thống kê doanh thu theo tháng, phim hot, v.v.
- **Tích hợp thanh toán:** Hỗ trợ thanh toán qua VNPay, MoMo, ZaloPay.
- **Thông báo tự động:** Gửi SMS/Email nhắc trả băng tự động.
- **Barcode/QR Code:** Quét mã vạch trên băng video để thao tác nhanh hơn.

---

# TÀI LIỆU THAM KHẢO

[1] Microsoft. (2024). *C# Programming Guide*. https://learn.microsoft.com/en-us/dotnet/csharp/

[2] Microsoft. (2024). *Windows Forms documentation*. https://learn.microsoft.com/en-us/dotnet/desktop/winforms/

[3] Microsoft. (2024). *ADO.NET Overview*. https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/

[4] Microsoft. (2024). *SQL Server Technical Documentation*. https://learn.microsoft.com/en-us/sql/sql-server/

[5] Troelsen, A., & Japikse, P. (2022). *Pro C# 10 with .NET 6*. Apress.

[6] Murach, J. (2020). *Murach's C# (7th Edition)*. Mike Murach & Associates.

[7] Connolly, T., & Begg, C. (2015). *Database Systems: A Practical Approach to Design, Implementation, and Management* (6th Edition). Pearson.

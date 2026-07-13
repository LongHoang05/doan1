# CHƯƠNG 1: PHẦN MỞ ĐẦU VÀ HÌNH THÀNH Ý TƯỞNG (CONCEIVE)

---

## 1.1. MỞ ĐẦU ĐỀ TÀI

### 1.1.1. Lý do chọn đề tài

#### 1.1.1.1. Bối cảnh ngành dịch vụ giải trí truyền thống và mô hình kinh doanh chuỗi băng video
Trong những thập niên trước và cả trong các mô hình lưu trữ – sưu tầm phương tiện nghe nhìn đặc thù hiện nay, kinh doanh cho thuê băng đĩa video (VHS, DVD, Blu-ray) đóng vai trò là một trong những loại hình dịch vụ giải trí phổ biến. Tuy nhiên, khác với các cửa hàng bán lẻ thông thường (nơi hàng hóa bán ra là kết thúc chu trình sở hữu của cửa hàng), mô hình cho thuê băng đĩa sở hữu đặc thù nghiệp vụ phức tạp hơn rất nhiều: **tài sản kinh doanh (cuốn băng/đĩa) liên tục luân chuyển giữa cửa hàng và khách hàng theo những chu kỳ mượn – trả lặp đi lặp lại**.

Khi quy mô hoạt động phát triển từ một cửa hàng đơn lẻ lên mô hình **chuỗi chi nhánh kinh doanh đa điểm (Multi-Store Rental Chain)**, thách thức trong việc kiểm soát tài sản tăng lên cấp số nhân. Một hệ thống kinh doanh chuỗi đòi hỏi phải đáp ứng xu hướng dịch vụ hiện đại: **tạo sự thuận tiện tối đa cho người tiêu dùng thông qua chính sách "Mượn tại chi nhánh A – Trả tại chi nhánh B bất kỳ" (Cross-Store Rental & Return)**. Sự linh hoạt này đem lại lợi thế cạnh tranh vượt trội nhưng đồng thời đặt ra yêu cầu khắt khe về năng lực theo dõi kho hàng vật lý theo thời gian thực.

#### 1.1.1.2. Những bất cập trong việc đồng bộ dữ liệu giao dịch mượn một nơi – trả một nơi bằng phương pháp thủ công
Trong thực tế vận hành thủ công hoặc sử dụng các công cụ bảng tính đơn lẻ (Excel, sổ sách giấy tờ), các chuỗi cho thuê băng video thường xuyên đối mặt với những nhược điểm nghiêm trọng:
1. **Sự đứt gãy thông tin kho hàng giữa các chi nhánh:** Khi một cuốn băng được khách hàng thuê tại Chi nhánh Trung tâm (CH01) và mang trả tại Chi nhánh Phụ (CH02), sổ sách thủ công tại Chi nhánh A vẫn ghi nhận băng "Đang cho mượn", trong khi Chi nhánh B nhận băng về lại không thể định danh chính xác hoặc không ghi nhận được lịch sử tựa phim. Điều này dẫn tới hiện tượng **"kho ảo"**: chi nhánh A thiếu băng cho thuê dù tài sản đang nằm ở chi nhánh B.
2. **Khó khăn trong phân biệt Tựa phim gốc (Master) và Bản sao vật lý (Copy):** Các phương pháp quản lý truyền thống thường nhầm lẫn giữa *Tựa phim lý thuyết* (tên phim, đạo diễn, thể loại) và *từng cuốn băng thực tế trên kệ*. Một cửa hàng sở hữu 5 cuốn bản sao của cùng một bộ phim cần được phân biệt định danh rõ ràng bằng mã vạch/RFID và số thứ tự riêng (`1, 2, 3...`) để theo dõi tình trạng suy hao vật lý, ngày hết hạn sử dụng và lịch sử mượn trả của từng cuốn.
3. **Thất thoát tài sản và rủi ro tính phí phạt sai lệch:** Việc tính toán tiền thuê theo đơn giá từng loại băng (PAL/NTSC), giám sát thời hạn trả, phát hiện băng trễ hạn và tính số tiền phạt bồi thường khi băng bị trễ hạn hoặc hư hỏng vỏ/đứt băng dễ gây tranh cãi và sai sót nếu thực hiện thủ công.
4. **Thiếu tính bảo mật và phân quyền rõ ràng:** Nhân viên tại quầy có thể tự ý sửa đổi tiền phạt hoặc xóa lịch sử giao dịch nếu không có cơ chế phân quyền chặt chẽ giữa Nhân viên quầy, Quản lý chi nhánh và Quản trị viên hệ thống (Admin).

Xuất phát từ những yêu cầu thực tiễn gay gắt đó, đề tài **"Xây dựng ứng dụng Quản lý Thuê Băng Đĩa Chuỗi Cửa Hàng (Enterprise Video Rental Management System)"** được lựa chọn nhằm giải quyết triệt để bài toán số hóa nghiệp vụ luân chuyển tài sản đa chi nhánh.

---

### 1.1.2. Mục đích và ý nghĩa của đề tài

#### 1.1.2.1. Mục đích nghiên cứu phân tích và xây dựng ứng dụng quản lý tập trung
Mục đích cốt lõi của đề tài là phân tích, thiết kế và phát triển hoàn chỉnh một hệ thống phần mềm quản trị doanh nghiệp trên nền tảng **.NET 8 Windows Forms**, kết hợp cơ sở dữ liệu quan hệ **SQL Server / Entity Framework Core 8**. Hệ thống hướng đến các mục tiêu cụ thể:
- **Chuẩn hóa kiến trúc dữ liệu theo chuẩn 3NF:** Tách biệt rạch ròi giữa thực thể **Phim** (Băng gốc - lưu thông tin chung về tựa đề, thể loại, thời lượng) và thực thể **BanSaoBang** (từng bản sao vật lý được định danh bằng mã duy nhất, đi kèm thuộc tính cửa hàng hiện tại, loại băng PAL/NTSC, đơn giá và tình trạng).
- **Tự động hóa toàn diện quy trình Mượn – Trả liên chi nhánh:** Cho phép lập Phiếu Mượn tại bất kỳ chi nhánh nào, hỗ trợ tra cứu và làm thủ tục Nhận Trả ngay tại chi nhánh khác, đồng thời tự động cập nhật vị trí lưu kho (`MaCuaHangHienTai`) theo đúng địa điểm vừa tiếp nhận.
- **Tích hợp báo cáo thống kê trực quan (Executive Dashboard):** Sử dụng framework biểu đồ chuyên nghiệp **ScottPlot 5** để trực quan hóa doanh thu, tỷ lệ tình trạng kho (Sẵn sàng / Đang mượn / Hư hỏng) và Top xu hướng phim được mượn nhiều nhất.
- **Bảo đảm an toàn dữ liệu và phân quyền nguyên tắc RBAC:** Thiết lập 3 vai trò phân cấp nghiệp vụ rõ ràng (Admin Cấp Cao, Quản Lý Chi Nhánh, Nhân Viên Quầy).

#### 1.1.2.2. Ý nghĩa thực tiễn đối với việc tối ưu hóa kho hàng và nâng cao trải nghiệm khách hàng
- **Đối với nhà quản lý & chủ doanh nghiệp:** Mang lại tầm nhìn toàn cảnh theo thời gian thực (Real-time Visibility) về tài sản của toàn chuỗi. Quản lý nắm rõ từng bản sao đang ở chi nhánh nào, tình trạng ra sao, giúp đưa ra quyết định điều chuyển băng giữa các chi nhánh hợp lý, giảm chi phí đầu tư thừa thãi.
- **Đối với nhân viên tại quầy:** Rút ngắn thời gian thao tác lập phiếu mượn/trả nhờ hỗ trợ quét mã vạch/RFID, tự động tính toán tổng tiền thuê và số ngày trễ hạn, loại bỏ sai sót tính nhẩm.
- **Đối với khách hàng:** Tận hưởng dịch vụ chuyên nghiệp, minh bạch về chi phí và tự do trả băng tại điểm giao dịch gần nhất mà không bị gò bó bởi địa điểm thuê ban đầu.

---

### 1.1.3. Đối tượng và phạm vi nghiên cứu

#### 1.1.3.1. Đối tượng nghiên cứu
- **Quy trình nghiệp vụ bán lẻ và cho thuê phương tiện giải trí:** Quy trình đăng ký khách hàng, quản lý danh mục phim, tạo bản sao băng, xử lý giỏ hàng mượn băng, kiểm tra tình trạng băng khi trả, ghi nhận vi phạm và thanh toán.
- **Quy trình luân chuyển kho hàng đa điểm:** Cơ chế ghi nhận và theo dõi sự thay đổi vị trí kho (`MaCuaHangHienTai`) của từng bản sao khi phát sinh giao dịch trả chéo chi nhánh.
- **Công nghệ phần mềm:** Mô hình kiến trúc phần mềm nhiều lớp (N-Tier Architecture), kỹ thuật lập trình hướng đối tượng với C#, công nghệ truy xuất dữ liệu ORM Entity Framework Core và kỹ thuật thiết kế giao diện hiện đại (Modern UI/UX).

#### 1.1.3.2. Phạm vi nghiên cứu
- **Phạm vi nghiệp vụ:** Hệ thống phục vụ hoạt động quản trị nội bộ dành cho nhân viên quầy và ban điều hành chuỗi cửa hàng cho thuê băng video. Không bao gồm cổng thông tin thương mại điện tử dành cho khách hàng tự đặt trực tuyến tại nhà.
- **Phạm vi kỹ thuật:** Ứng dụng Desktop Windows Forms (.NET 8.0) kiến trúc 3 lớp (Presentation Layer – Business Logic Layer – Data Access Layer), hoạt động trên hệ điều hành Windows 10/11, sử dụng hệ quản trị cơ sở dữ liệu Microsoft SQL Server (Hỗ trợ LocalDB tự động khởi tạo).

---

### 1.1.4. Phương pháp nghiên cứu
Để hoàn thành mục tiêu đề tài, nhóm nghiên cứu kết hợp hài hòa các phương pháp sau:
1. **Phương pháp nghiên cứu lý thuyết (Theoretical Research):**
   - Nghiên cứu lý thuyết chuẩn hóa cơ sở dữ liệu quan hệ (1NF, 2NF, 3NF), phân tích mô hình Entity-Relationship (ERD) cho bài toán quản lý tài sản có bản sao 1-N.
   - Nghiên cứu mô hình CDIO (Conceive – Design – Implement – Operate) trong phát triển sản phẩm kỹ thuật phần mềm.
2. **Phương pháp phân tích nghiệp vụ & mô hình hóa (System Analysis & Modeling):**
   - Phỏng vấn và khảo sát yêu cầu thực tế của quy trình cho thuê băng video truyền thống và chuỗi liên kết.
   - Xây dựng sơ đồ luồng dữ liệu (DFD), sơ đồ ca sử dụng (Use Case) và mô hình dữ liệu thực thể.
3. **Phương pháp thực nghiệm và phát triển ứng dụng (Empirical & Development Method):**
   - Áp dụng phương pháp phát triển phần mềm lặp (Iterative Development).
   - Xây dựng phần mềm thực tế bằng C# .NET 8, viết các lớp dịch vụ nghiệp vụ (BLL) độc lập, kiểm thử giao dịch (Database Transaction) để đảm bảo toàn vẹn dữ liệu khi Lập Phiếu Mượn và Chốt Nhận Trả.

---

## 1.2. HÌNH THÀNH Ý TƯỞNG SẢN PHẨM (CONCEIVE)

Giai đoạn **Conceive (Hình thành ý tưởng)** trong mô hình CDIO đóng vai trò định vị triết lý sản phẩm, xác định chân dung người dùng mục tiêu và hoạch định lộ trình giải pháp kỹ thuật.

### 1.2.1. Xây dựng ý tưởng giải pháp cá nhân
Từ việc phân tích những trở ngại của mô hình cho thuê băng đĩa cũ, ý tưởng giải pháp phần mềm được định hình dựa trên **4 trụ cột thiết kế chính**:

```
+-----------------------------------------------------------------------------------+
|                        4 TRỤ CỘT Ý TƯỞNG SẢN PHẨM (CONCEIVE)                      |
+---------------------+---------------------+-------------------+-------------------+
|  1. ĐỊNH DANH BẢN SAO |  2. LUÂN CHUYỂN KHO |   3. TRỰC QUAN HÓA| 4. BẢO MẬT & PHÂN |
|   (Master vs Copy)  |    (Cross-Store)    |     (Dashboard)   |    QUYỀN (RBAC)   |
+---------------------+---------------------+-------------------+-------------------+
| Mỗi băng gốc (Phim) | Khách mượn chi nhánh| Tích hợp biểu đồ  | Phân chia quyền   |
| sinh ra N bản sao   | A, trả chi nhánh B. | ScottPlot 5 theo  | Admin, Quản lý    |
| định danh duy nhất  | Hệ thống tự cập nhật| dõi doanh thu và  | chi nhánh, Nhân   |
| theo số 1, 2, 3...  | kho về chi nhánh B. | kho hàng realtime.| viên quầy rõ ràng.|
+---------------------+---------------------+-------------------+-------------------+
```

1. **Ý tưởng phân tách mô hình dữ liệu "Băng gốc – Bản sao" chuẩn xác:**
   - Thay vì quản lý số lượng chung chung, hệ thống coi **Phim** là danh mục tựa đề (Master). Mỗi cuốn băng nhập kho được cấp một mã `MaBanSao` duy nhất (ví dụ: `PHIM001-BS01`, `PHIM001-BS02`...) đi kèm mã thẻ RFID, số thứ tự bản sao (`SoThuTuBanSao`), tình trạng và giá thuê riêng.
2. **Ý tưởng "Một chuỗi – Đa điểm kho" (Seamless Cross-Store Workflow):**
   - Mọi bản sao đều gắn liền với trường `MaCuaHangHienTai`. Khi làm thủ tục trả băng tại bất kỳ quầy nào trong hệ thống, giao dịch lập Phiếu Trả sẽ đồng thời kích hoạt luồng nghiệp vụ ngầm: chuyển `TrangThai` về *"Sẵn sàng"* và cập nhật `MaCuaHangHienTai` về địa điểm tiếp nhận mới.
3. **Ý tưởng Giao diện tinh tế & Trải nghiệm người dùng cao cấp (Modern Enterprise UI):**
   - Loại bỏ giao diện xám nguyên bản thô cứng của WinForms truyền thống, ứng dụng áp dụng bảng màu Pastel ấm áp, nút bấm bo góc phẳng (`FlatStyle.Flat`), phông chữ `Segoe UI Semibold` mượt mà và các bảng dữ liệu bố trí khoa học, giúp nhân viên quầy thao tác nhanh chóng với độ chính xác cao.

---

### 1.2.2. Đánh giá khả năng công nghệ và thống nhất ý tưởng sản phẩm trong nhóm
Sau quá trình thảo luận và phân tích tính khả thi kỹ thuật, nhóm phát triển đã thống nhất lựa chọn hệ sinh thái công nghệ và kiến trúc hệ thống như sau:

#### 1. Lựa chọn nền tảng & công nghệ phát triển
- **Ngôn ngữ lập trình & Framework:** **C# trên nền tảng .NET 8.0 Windows Forms**. .NET 8 mang lại hiệu năng cao, cơ chế quản lý bộ nhớ vượt trội cùng khả năng tương thích hiện đại trên Windows.
- **ORM & Truy xuất dữ liệu:** **Entity Framework Core 8.0 (EF Core 8)**. Việc sử dụng ORM giúp quản lý mô hình cơ sở dữ liệu theo hướng đối tượng (Code-First Migration / Schema Mapping), hỗ trợ quản lý giao dịch toàn vẹn (`IDbContextTransaction`) giúp đảm bảo tính nguyên tử (Atomicity) khi thực hiện lập phiếu mượn/trả gồm nhiều bản ghi cùng lúc.
- **Cơ sở dữ liệu:** **Microsoft SQL Server / SQL Server LocalDB**. Đảm bảo khả năng xử lý truy vấn quan hệ phức tạp, hỗ trợ khóa chính kép (Composite Key) cho bảng chi tiết và dễ dàng đóng gói triển khai trên các máy trạm.
- **Thư viện Biểu đồ:** **ScottPlot.WinForms 5.1.59**. Thay vì tự vẽ đồ họa GDI+ dễ vỡ hình trên màn hình độ phân giải cao, ScottPlot 5 cung cấp cơ chế render biểu đồ vector (Bar Chart, Pie Chart) sắc nét, tự động tương thích responsive khi phóng to/thu nhỏ cửa sổ.

#### 2. Thống nhất Kiến trúc Phần mềm N-Tier (3 Lớp)
Dự án được cấu trúc phân lớp chặt chẽ nhằm đảm bảo tính dễ bảo trì, dễ mở rộng và tách biệt trách nhiệm (Separation of Concerns):
- **Lớp Dữ Liệu (DAL - Data Access Layer):** Chứa `QuanLyThueBangContext`, ánh xạ các thực thể cơ sở dữ liệu (`Phim`, `BanSaoBang`, `PhieuMuon`, `ChiTietPhieuMuon`, `CuaHang`, `NhanVien`, `KhachHang`, `VaiTro`...) và quản lý kết nối CSDL qua `AppConfig`.
- **Lớp Nghiệp Vụ (BLL - Business Logic Layer):** Chứa các dịch vụ chuyên biệt (`MuonTraService`, `PhimService`, `BanSaoBangService`, `AuthService`, `CuaHangService`...). Toàn bộ quy tắc kiểm tra nghiệp vụ (kiểm tra băng hết hạn, kiểm tra băng đang cho mượn, luân chuyển kho tự động, tính tiền phạt) đều được cô lập tại lớp này.
- **Lớp Trình Diễn (Presentation Layer):** Chứa các giao diện cửa sổ (`LoginForm`, `MainShellForm`) và các User Control chuyên đề (`MuonTraBangControl`, `QuanLyPhimControl`, `QuanLyBanSaoControl`, `QuanLyKhachHangControl`...). Lớp này chỉ chịu trách nhiệm thu nhận thao tác người dùng và hiển thị kết quả từ BLL trả về.

Sự thống nhất cao về ý tưởng (Conceive) và công nghệ đã tạo tiền đề vững chắc để dự án bước sang các giai đoạn Thiết kế (Design), Triển khai (Implement) và Vận hành (Operate) đạt chất lượng vượt trội.


---

# CHƯƠNG 2: KHẢO SÁT HIỆN TRẠNG VÀ PHÂN TÍCH YÊU CẦU HỆ THỐNG

---

## 2.1. KHẢO SÁT HIỆN TRẠNG NGHIỆP VỤ THỰC TẾ

Để xây dựng một hệ thống phần mềm quản lý sát với thực tiễn vận hành của chuỗi cửa hàng cho thuê băng đĩa, quá trình khảo sát nghiệp vụ thực tế được tiến hành toàn diện qua các bước tiếp nhận khách hàng, quản lý kho băng vật lý, xử lý mượn trả chéo chi nhánh và báo cáo thu hồi nợ phạt.

### 2.1.1. Quy trình định danh khách hàng và cấp mã số nhân viên
1. **Định danh Khách hàng thành viên (Customer Identification):**
   - Khi một khách hàng đến quầy giao dịch lần đầu, nhân viên quầy tiến hành thu thập các thông tin pháp lý bắt buộc: **Họ và tên, Số CMND/CCCD/Hộ chiếu, Số điện thoại liên lạc**.
   - Mỗi khách hàng được cấp một mã định danh duy nhất (`MaKhachHang`, ví dụ: `KH001`, `KH002`...) gắn liền với hồ sơ cá nhân. Khách hàng sử dụng số điện thoại hoặc mã thành viên để tra cứu lịch sử mượn trả tại tất cả các điểm giao dịch trong chuỗi.
2. **Cấp mã số và quản lý Nhân sự vận hành (Employee Identification):**
   - Nhân viên tuyển dụng vào hệ thống được phân công vào một chi nhánh cụ thể (`MaCuaHang`, ví dụ: `CH01`, `CH02`) hoặc thuộc khối điều hành trung tâm.
   - Mỗi nhân viên sở hữu một mã định danh (`MaNhanVien`, ví dụ: `ADM01`, `QL01`, `NV001`), đi kèm thông tin đăng nhập (`TenDangNhap`, `MatKhau`) và mã vai trò nghiệp vụ (`MaVaiTro`).

---

### 2.1.2. Phương thức phân loại phim gốc và quản lý số thứ tự các bản sao vật lý
Đặc thù lớn nhất trong nghiệp vụ kinh doanh băng đĩa là sự phân định rõ ràng giữa **Tựa phim (Master Catalog)** và **Cuốn băng thực tế trên kệ (Physical Item Copy)**:

```
+-----------------------------------------------------------------------------------+
|               MÔ HÌNH QUẢN LÝ BĂNG GỐC VÀ BẢN SAO VẬT LÝ (1 - N)                  |
+-----------------------------------------------------------------------------------+
|                                 BẢNG PHIM (MASTER)                                |
|  + MaPhim: PHIM001 | TuaDe: Doraemon | Nam: 2024 | TheLoai: Hoạt hình | DoDai: 90 |
+-----------------------------------------+-----------------------------------------+
                                          | (Quan hệ 1 - N)
                                          v
+-----------------------------------------------------------------------------------+
|                        BẢNG BANSAOBANG (PHYSICAL COPIES)                          |
+-------------------+-------------------+-------------------+-----------------------+
| MaBanSao          | SoThuTuBanSao     | MaCuaHangHienTai  | TrangThai             |
+-------------------+-------------------+-------------------+-----------------------+
| PHIM001-BS01      | 1                 | CH01              | Sẵn sàng              |
| PHIM001-BS02      | 2                 | CH01              | Đang cho mượn         |
| PHIM001-BS03      | 3                 | CH02              | Sẵn sàng              |
+-------------------+-------------------+-------------------+-----------------------+
```

1. **Phân loại Phim gốc (Tựa phim lý thuyết):**
   - Được lưu tại bảng `Phim`, đại diện cho thông tin chung về bộ phim: *Mã phim (`MaPhim`), Tựa đề (`TuaDe`), Năm phát hành (`NamPhatHanh`), Thể loại (`MaTheLoai`), Độ dài phút (`DoDaiPhut`)*.
2. **Quản lý số thứ tự các Bản sao vật lý (`BanSaoBang`):**
   - Khi cửa hàng mua về 3 cuốn băng vật lý của phim `PHIM001`, hệ thống tự động sinh mã định danh duy nhất cho từng cuốn theo cú pháp `[MaPhim]-BS[STT]` (ví dụ: `PHIM001-BS01`, `PHIM001-BS02`, `PHIM001-BS03`) tương ứng với `SoThuTuBanSao = 1, 2, 3`.
   - Mỗi bản sao có các thuộc tính độc lập: **Loại băng** (`PAL` / `NTSC`), **Đơn giá thuê** (`DonGiaThue`), **Ngày hết hạn sử dụng** (`NgayHetHan`), **Chi nhánh lưu kho hiện tại** (`MaCuaHangHienTai`) và **Trạng thái vật lý** (`Sẵn sàng`, `Đang cho mượn`, `Bảo trì`, `Thất lạc`).

---

### 2.1.3. Quy trình lập hồ sơ mượn băng video liên chi nhánh
Quy trình cho mượn băng được thiết kế hỗ trợ khả năng mượn liên chi nhánh mượt mà:
1. **Tiếp nhận khách hàng:** Nhân viên quầy chọn Khách hàng thành viên từ danh sách hoặc tra cứu nhanh theo số điện thoại/CMND.
2. **Quét mã băng vào giỏ mượn:** Nhân viên nhập hoặc dùng đầu đọc quét mã vạch/RFID (`MaBanSao`) của các cuốn băng khách chọn. Hệ thống kiểm tra tức thì:
   - Cuốn băng phải ở trạng thái **"Sẵn sàng"** (`Constants.TrangThaiBang_SanSang`).
   - Cuốn băng phải đang nằm tại đúng kho của chi nhánh thực hiện giao dịch (`MaCuaHangHienTai`).
   - Cuốn băng chưa vượt quá hạn sử dụng (`NgayHetHan >= DateTime.Today`).
3. **Chốt Phiếu Mượn (Database Transaction):**
   - Hệ thống mở một giao dịch cơ sở dữ liệu (`BEGIN TRANSACTION`), ghi nhận phiếu mượn mới (`PhieuMuon`) gồm: *Mã phiếu mượn (`MaPhieuMuon`), Khách hàng, Cửa hàng mượn, Nhân viên lập phiếu, Ngày mượn, Ngày dự kiến trả*.
   - Đồng thời ghi danh sách chi tiết (`ChiTietPhieuMuon`) và lập tức chuyển trạng thái các cuốn băng trong giỏ sang **"Đang cho mượn"** (`Constants.TrangThaiBang_DangChoMuon`).

---

### 2.1.4. Quy trình xử lý sự kiện trả băng, nhập phạt thủ công và in biên lai thu tiền
Quy trình nhận trả băng là điểm nhấn nghiệp vụ vượt trội của chuỗi liên kết (Cross-Store Return & Inventory Relocation):
1. **Quét mã băng nhận trả (Không phụ thuộc chi nhánh xuất đi):**
   - Khách hàng có thể mang băng trả tại bất kỳ chi nhánh nào trong chuỗi. Nhân viên chỉ cần quét mã bản sao (`MaBanSao`).
   - Hệ thống tự động truy tìm Phiếu Mượn đang mở gần nhất chứa cuốn băng này (`ChiTietPhieuMuon.TrangThaiTra == false`), hiển thị thông tin phim, ngày mượn, ngày hẹn trả.
2. **Tính toán trễ hạn & Nhập tình trạng phạt:**
   - Nếu `Ngày trả thực tế > Ngày dự kiến trả`, hệ thống tự động cảnh báo số ngày trễ hạn.
   - Nhân viên kiểm tra tình trạng vật lý vỏ băng/dây băng, ghi nhận mô tả tình trạng trả (`TinhTrangKhiTra`: *Bình thường, Hỏng vỏ, Đứt băng...*) và nhập số tiền phạt vi phạm (`TienPhat`).
3. **Chốt Phiếu Trả & Tự động luân chuyển kho chéo chi nhánh:**
   - Hệ thống tạo Phiếu Trả (`PhieuTra`) và Chi tiết Phiếu Trả (`ChiTietPhieuTra`).
   - **Đồng bộ kho tức thì:** Thuộc tính `BanSaoBang.TrangThai` được cập nhật về **"Sẵn sàng"** và thuộc tính `BanSaoBang.MaCuaHangHienTai` tự động chuyển về **chi nhánh vừa tiếp nhận trả băng**.

---

### 2.1.5. Quy trình tổng hợp hồ sơ và in thông báo nhắc trả băng hằng ngày
Để giảm thiểu nợ xấu và thất thoát tài sản, hệ thống duy trì quy trình kiểm tra tự động hằng ngày:
- Hệ thống quét toàn bộ bảng `ChiTietPhieuMuon` có `TrangThaiTra = false` kèm theo điều kiện `PhieuMuon.NgayDuKienTra < DateTime.Today`.
- Danh sách kết quả được tổng hợp thành bảng **Băng Quá Hạn**, hiển thị rõ: *Tên khách hàng, Số điện thoại, Tựa đề phim, Mã bản sao, Ngày hẹn trả và Số ngày đã quá hạn*.
- Nhân viên quản lý có thể xuất danh sách này ra file Excel hoặc in danh sách để gọi điện thông báo, nhắc nhở khách hàng mang trả băng.

---

## 2.2. XÁC ĐỊNH YÊU CẦU PHẦN MỀM

### 2.2.1. Tóm tắt tổng quan hệ thống phần mềm quản lý chuỗi cho thuê băng
Hệ thống phần mềm **Quản Lý Thuê Băng Đĩa (Rental Manager Enterprise)** là giải pháp phần mềm quản trị tập trung trên nền tảng Desktop Windows Forms (.NET 8.0), được thiết kế theo mô hình 3 lớp chuyên nghiệp. Phần mềm cung cấp trọn bộ công cụ từ quản lý danh mục phim, kiểm soát kho bản sao, lập phiếu mượn/trả, giám sát nợ quá hạn đến báo cáo thống kê đa chiều.

---

### 2.2.2. Xác định các đối tượng sử dụng và phân quyền nghiệp vụ (RBAC)
Hệ thống phân chia người dùng thành **3 vai trò cốt lõi** tuân thủ mô hình Role-Based Access Control (RBAC):

```
+-----------------------------------------------------------------------------------+
|                        MA TRẬN PHÂN QUYỀN HỆ THỐNG (RBAC)                         |
+-----------------------------------+-------------------+-------------------+-------+
| Nhóm chức năng / Module           | Admin Cấp Cao     | Quản Lý Chi Nhánh | NV Quầy|
+-----------------------------------+-------------------+-------------------+-------+
| 1. Quản lý Phim gốc               | Toàn quyền        | Xem, Thêm, Sửa    | Xem   |
| 2. Quản lý Bản sao Băng           | Toàn quyền        | Theo kho chi nhánh| Xem   |
| 3. Quản lý Khách hàng             | Toàn quyền        | Xem, Thêm, Sửa    | Thêm  |
| 4. Nghiệp vụ Mượn - Trả Băng      | Toàn quyền        | Toàn quyền        | Mượn/Trả|
| 5. Quản lý Nhân viên              | Toàn quyền        | NV thuộc chi nhánh| Cấm   |
| 6. Quản lý Cửa hàng / Chi nhánh   | Toàn quyền        | Xem chi nhánh mình| Cấm   |
| 7. Dashboard Thống kê Biểu đồ     | Toàn chuỗi        | Theo chi nhánh    | Cấm   |
| 8. Hành động XÓA dữ liệu          | Được phép         | Cấm               | Cấm   |
+-----------------------------------+-------------------+-------------------+-------+
```

1. **Admin Cấp Cao (`Admin_CapCao`):**
   - Có toàn quyền kiểm soát toàn bộ hệ thống ở tất cả các chi nhánh.
   - Được quyền quản lý danh sách cửa hàng, quản lý tất cả nhân viên, cấu hình phân quyền và là vai trò duy nhất được thực hiện thao tác **XÓA (`DELETE`)** hồ sơ nhạy cảm.
2. **Quản Lý Chi Nhánh (`QuanLy_ChiNhanh`):**
   - Phụ trách điều hành tại chi nhánh được phân công.
   - Được xem thống kê doanh thu/kho hàng của chi nhánh mình, quản lý nhân viên thuộc chi nhánh, theo dõi băng quá hạn. **Bị ẩn/khóa toàn bộ nút Xóa dữ liệu**.
3. **Nhân Viên Quầy (`NhanVien_Quay`):**
   - Người trực tiếp thực hiện giao dịch tại quầy.
   - Quyền hạn tập trung vào 2 tab nghiệp vụ chính: **Lập Phiếu Mượn/Trả** và **Đăng ký Khách hàng mới**. Không có quyền truy cập tab Báo cáo Dashboard, Quản lý Cửa hàng hay Quản lý Nhân viên.

---

### 2.2.3. Yêu cầu chức năng (Functional Requirements) cho 8 module cốt lõi

```
+-----------------------------------------------------------------------------------+
|                    KIẾN TRÚC 8 MODULE CHỨC NĂNG CỐT LÕI                           |
+-----------------------------------------------------------------------------------+
|  [Module 1] Quản lý Phim Gốc        |  [Module 5] Quản lý Cửa Hàng / Chi Nhánh    |
|  [Module 2] Quản lý Bản Sao Băng    |  [Module 6] Quản lý Danh Mục Thể Loại       |
|  [Module 3] Quản lý Khách Hàng      |  [Module 7] Nghiệp vụ Mượn - Trả (Cross)    |
|  [Module 4] Quản lý Nhân Sự & RBAC  |  [Module 8] Dashboard Thống Kê & Biểu Đồ    |
+-----------------------------------------------------------------------------------+
```

#### 1. Module Quản trị Phim gốc (Master Movie Management)
- Hiển thị danh sách tựa phim với bộ lọc nhanh theo từ khóa, thể loại và năm phát hành.
- Cho phép thêm mới, cập nhật thông tin tựa phim (Tựa đề, Thể loại, Năm phát hành, Thời lượng phút).
- Thống kê tự động tổng số lượng bản sao hiện có và số bản sao sẵn sàng cho từng tựa phim.

#### 2. Module Quản lý Bản sao Băng vật lý (Physical Tape Copy Management)
- Quản lý từng bản sao cụ thể với mã vạch duy nhất `MaBanSao` (`[MaPhim]-BS[STT]`).
- Hỗ trợ nhập kho bản sao mới, chỉ định kho chi nhánh (`MaCuaHangHienTai`), định giá cho thuê (`DonGiaThue`), loại băng (`PAL`/`NTSC`) và ngày hết hạn.
- Cung cấp bộ lọc theo chi nhánh và trạng thái (*Sẵn sàng, Đang cho mượn, Hư hỏng...*), hỗ trợ xuất dữ liệu ra tệp Excel.

#### 3. Module Quản lý Khách hàng thành viên (Customer Management)
- Quản lý hồ sơ thành viên: Họ tên, CMND/CCCD, Số điện thoại và địa chỉ.
- Tra cứu lịch sử thuê băng của khách hàng, đếm tổng lượt giao dịch đã thực hiện.
- Cảnh báo khách hàng đang giữ băng trễ hạn trước khi cho phép lập phiếu mượn mới.

#### 4. Module Quản lý Nhân sự & Phân quyền (Employee & RBAC Management)
- Quản lý danh sách nhân viên, tài khoản đăng nhập và mật khẩu truy cập.
- Gán vai trò (`Admin`, `Quản lý`, `Nhân viên`) và chi nhánh công tác (`MaCuaHang`).
- Cơ chế kiểm tra vai trò khi đăng nhập để tự động ẩn/hiện các nút chức năng tương ứng trên giao diện.

#### 5. Module Quản lý Cửa hàng / Chi nhánh (Store / Branch Management)
- Thêm mới, cập nhật thông tin mạng lưới chi nhánh (Mã cửa hàng, Địa chỉ, Số điện thoại).
- Hiển thị bảng tổng hợp số lượng nhân viên và tổng số lượng bản sao băng đang lưu trữ tại từng chi nhánh.

#### 6. Module Quản lý Danh mục Thể loại (Genre Management)
- Chuẩn hóa danh mục thể loại phim (*Hành động, Hoạt hình, Khoa học, Hài hước...*) nhằm tránh lỗi nhập dữ liệu tự do.

#### 7. Module Nghiệp vụ Mượn - Trả Băng & Luân chuyển kho (Cross-Store Workflow)
- **Tab Lập Phiếu Mượn:** Quét mã vạch/RFID vào giỏ mượn, tự động tính tổng tiền thuê, chọn khách hàng/nhân viên và chốt phiếu mượn với giao dịch an toàn (`BeginTransaction`).
- **Tab Nhận Trả Băng:** Quét mã băng trả, tự động tra cứu phiếu mượn tương ứng, cảnh báo trễ hạn, hỗ trợ nhập tiền phạt và tự động luân chuyển kho về chi nhánh nhận trả.
- **Tab Quét Băng Quá Hạn:** Hiển thị danh sách khách hàng giữ băng quá hạn hẹn trả để liên hệ nhắc nhở.

#### 8. Module Dashboard Báo cáo & Thống kê Trực quan (Executive Analytics)
- Hiển thị các thẻ chỉ số KPI tổng quan: **Tổng doanh thu thực thu, Tổng lượng phim, Tổng lượng bản sao, Lượt thuê active**.
- **Biểu đồ Cột (Bar Chart):** Thống kê doanh thu theo thời gian, vẽ bằng framework **ScottPlot 5**.
- **Biểu đồ Tròn (Pie Chart):** Phân bổ tỷ lệ tình trạng kho (Sẵn sàng / Đang cho mượn / Hư hỏng).
- **Bảng Top Trending:** Danh sách 5 bộ phim thịnh hành được mượn nhiều nhất trong tháng.

---

### 2.2.4. Yêu cầu phi chức năng (Non-Functional Requirements)

1. **Yêu cầu về Bảo mật & Phân quyền (Security & Access Control):**
   - Thông tin xác thực phải được kiểm tra qua cơ chế kiểm soát vai trò chặt chẽ.
   - Các hành động nhạy cảm như xóa dữ liệu hay truy cập báo cáo tài chính phải được vô hiệu hóa ở tầng UI và chặn ở tầng BLL nếu tài khoản không đủ thẩm quyền.
2. **Yêu cầu về Toàn vẹn dữ liệu & Tốc độ đồng bộ (Data Integrity & Performance):**
   - Mọi thao tác chốt phiếu mượn hoặc trả nhiều cuốn băng đồng thời phải được bọc trong **Database Transaction** (`IDbContextTransaction`). Nếu xảy ra lỗi ở bất kỳ cuốn băng nào, toàn bộ giao dịch phải được `Rollback` hoàn toàn để ngăn tình trạng lệch dữ liệu kho.
   - Thời gian phản hồi cho các thao tác tra cứu, quét mã vạch không quá 500ms đối với kho dữ liệu dưới 100.000 bản ghi.
3. **Yêu cầu về Khả năng lưu trữ & Triển khai (Storage & Deployment):**
   - Hệ thống tương thích tốt với SQL Server LocalDB, không đòi hỏi máy trạm cấu hình cao (chỉ cần RAM tối thiểu 4GB, hệ điều hành Windows 10/11 x64).
   - Biểu đồ và giao diện hỗ trợ tự động co giãn (Responsive Layout) đảm bảo hiển thị sắc nét trên cả màn hình laptop tiêu chuẩn lẫn màn hình độ phân giải Full HD / 4K.

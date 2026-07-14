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

Để xây dựng một hệ thống phần mềm quản lý sát với thực tiễn vận hành của chuỗi cửa hàng cho thuê băng đĩa, quá trình khảo sát nghiệp vụ thực tế được tiến hành toàn diện tại quầy giao dịch và văn phòng điều hành chuỗi. Dưới đây là phân tích chi tiết từng quy trình nghiệp vụ cốt lõi đang diễn ra trong thực tế.

### 2.1.1. Quy trình định danh khách hàng và cấp mã số nhân viên

#### 1. Quy trình tiếp nhận, định danh và cấp thẻ Khách hàng thành viên
Trong mô hình dịch vụ cho thuê tài sản có giá trị (băng video gốc/bản sao bản quyền), việc quản lý chính xác danh tính khách hàng là yêu cầu tiên quyết nhằm ngăn chặn thất thoát tài sản. Quy trình định danh khách hàng tại quầy được thực hiện theo 5 bước chuẩn hóa:

```
[Khách hàng đến quầy] 
       │
       ▼
[Nhân viên kiểm tra giấy tờ pháp lý: CMND/CCCD/SĐT]
       │
       ▼
[Tra cứu trùng lặp trên Hệ thống toàn chuỗi] ──(Đã có)──► [Cập nhật hồ sơ / Tiến hành mượn ngay]
       │
   (Chưa có)
       ▼
[Tạo hồ sơ mới & Cấp mã định danh KHxxx (ví dụ: KH001)]
       │
       ▼
[Cấp tài khoản thành viên liên thông toàn chuỗi]
```

- **Bước 1: Tiếp nhận yêu cầu đăng ký thành viên:** Khách hàng có nhu cầu thuê băng lần đầu xuất trình giấy tờ tùy thân hợp lệ (Căn cước công dân, Chứng minh nhân dân hoặc Hộ chiếu) kèm số điện thoại liên lạc chính chủ.
- **Bước 2: Kiểm tra dữ liệu trùng lặp trên toàn chuỗi:** Nhân viên nhập số điện thoại hoặc số CMND vào hệ thống để tra cứu. Nếu khách hàng đã từng đăng ký tại một chi nhánh khác (ví dụ đã đăng ký tại Chi nhánh 1 - `CH01`), hệ thống hiển thị ngay hồ sơ thành viên, cho phép khách hàng thực hiện giao dịch tại Chi nhánh 2 (`CH02`) mà không cần đăng ký lại.
- **Bước 3: Khởi tạo hồ sơ khách hàng mới:** Nếu là khách hàng mới hoàn toàn, nhân viên nhập các thông tin bắt buộc gồm: **Họ và tên (`HoTen`), Số CMND/CCCD (`CCCD`), Số điện thoại (`SoDienThoai`) và Địa chỉ (`DiaChi`)**.
- **Bước 4: Cấp mã định danh duy nhất (`MaKhachHang`):** Hệ thống tự động phát sinh hoặc cho phép cấp mã định danh thành viên duy nhất toàn chuỗi theo định dạng `KHxxx` (ví dụ: `KH001`, `KH002`...).
- **Bước 5: Kích hoạt quyền giao dịch:** Khách hàng chính thức được gia nhập hệ thống thành viên và có thể lập phiếu mượn băng tại bất kỳ chi nhánh nào thuộc chuỗi.

#### 2. Quy trình tuyển dụng, phân bổ chi nhánh và cấp mã Nhân viên vận hành
Đối với nội bộ doanh nghiệp, nhân sự vận hành được quản lý tập trung và phân cấp theo từng điểm bán:
- **Cấp mã định danh nhân viên (`MaNhanVien`):** Mỗi nhân viên khi gia nhập chuỗi được cấp một mã định danh duy nhất theo tiền tố vai trò nghiệp vụ:
  - `ADMxx` (ví dụ: `ADM01`): Quản trị viên hệ thống cấp cao.
  - `QLxx` (ví dụ: `QL01`): Quản lý phụ trách chi nhánh.
  - `NVxx` (ví dụ: `NV001`, `NVQ01`): Nhân viên nghiệp vụ trực quầy.
- **Gán chi nhánh trực thuộc (`MaCuaHang`):** Nhân viên được gắn định danh vào một chi nhánh công tác cố định (ví dụ `CH01` - Trụ sở chính, `CH02` - Chi nhánh Cầu Giấy). Quyền truy xuất dữ liệu kho hàng của nhân viên quầy và quản lý sẽ được giới hạn theo chi nhánh trực thuộc này.
- **Cấp thông tin xác thực an toàn:** Mỗi hồ sơ nhân viên đi kèm tài khoản đăng nhập (`TenDangNhap`) và mật khẩu (`MatKhau`) được mã hóa bảo mật, bảo đảm truy vết trách nhiệm (Audit Trail) cho từng phiếu mượn/trả do nhân viên đó lập ra.

---

### 2.1.2. Phương thức phân loại phim gốc và quản lý số thứ tự các bản sao vật lý

#### 1. Phân tích bài toán quản lý tài sản theo mô hình 1 - N (Master vs. Physical Copies)
Trong quản lý bán lẻ thông thường, các sản phẩm cùng loại thường chỉ cần theo dõi bằng thuộc tính "Số lượng tồn kho" (Quantity). Tuy nhiên, đối với dịch vụ cho thuê băng đĩa, **việc quản lý gộp số lượng là hoàn toàn không khả thi**, bởi lẽ:
- Hai cuốn băng của cùng một bộ phim có thể được mua vào ở hai thời điểm khác nhau, có độ mới/cũ khác nhau và hạn sử dụng (`NgayHetHan`) khác nhau.
- Khi khách hàng làm hỏng vỏ hoặc làm đứt dây băng của một cuốn cụ thể, cửa hàng phải định danh chính xác cuốn băng đó để tính phí bồi thường và chuyển trạng thái sang "Hư hỏng", trong khi các cuốn băng còn lại của bộ phim đó vẫn cho thuê bình thường.

Do đó, hệ thống chuẩn hóa CSDL theo mô hình quan hệ một - nhiều (**1 - N**) chặt chẽ:

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

#### 2. Đặc tả thông tin Tựa phim gốc (Master Movie Catalog)
Bảng `Phim` đóng vai trò là Danh mục mục lục trung tâm, lưu trữ thông tin trí tuệ và thuộc tính nội dung của bộ phim:
- **Mã phim (`MaPhim`):** Khóa chính định danh tựa phim (ví dụ: `PHIM001`, `PHIM002`).
- **Tựa đề (`TuaDe`):** Tên đầy đủ của tác phẩm điện ảnh/video.
- **Năm phát hành (`NamPhatHanh`) & Thời lượng (`DoDaiPhut`):** Cung cấp thông tin tham khảo cho khách hàng khi lựa chọn.
- **Mã thể loại (`MaTheLoai`):** Liên kết khóa ngoại đến bảng Danh mục Thể loại (`TheLoai`) nhằm phục vụ phân loại và báo cáo thống kê thị hiếu người xem.

#### 3. Đặc tả thông tin từng Bản sao vật lý (`BanSaoBang`) và vòng đời lưu hành
Mỗi cuốn băng nhập kho được khởi tạo một bản ghi độc lập trong bảng `BanSaoBang`:
- **Mã định danh duy nhất (`MaBanSao`):** Được tổ chức theo quy tắc kết hợp mã phim gốc và số thứ tự bản sao: `[MaPhim]-BS[STT]` (ví dụ: `PHIM001-BS01`, `PHIM001-BS02`). Mã này được in thành tem mã vạch (Barcode) hoặc gắn chip RFID dán lên vỏ băng.
- **Số thứ tự bản sao (`SoThuTuBanSao`):** Số nguyên (`1, 2, 3...`) phân biệt các bản sao thuộc cùng một tựa phim.
- **Định dạng kỹ thuật (`LoaiBang`):** Phân loại hệ màu/chuẩn băng như `PAL`, `NTSC` hoặc chuẩn đĩa `HD`, `Blu-ray`.
- **Đơn giá cho thuê (`DonGiaThue`):** Giá tiền cho thuê tiêu chuẩn trên một chu kỳ giao dịch (tính bằng VNĐ).
- **Chi nhánh lưu trữ hiện tại (`MaCuaHangHienTai`):** Trường khóa ngoại quan trọng nhất phục vụ nghiệp vụ luân chuyển liên chi nhánh. Trường này xác định cuốn băng hiện đang nằm trên kệ của chi nhánh nào.
- **Vòng đời trạng thái vật lý (`TrangThai`):** Cuốn băng luân chuyển qua 4 trạng thái nghiệp vụ chuẩn:
  1. *Sẵn sàng (`SanSang`):* Băng đang ở trên kệ tại kho `MaCuaHangHienTai`, sẵn sàng cho thuê.
  2. *Đang cho mượn (`DangChoMuon`):* Băng đang nằm trong tay khách hàng thông qua một Phiếu Mượn chưa hoàn tất trả.
  3. *Hư hỏng (`HuHong`):* Băng bị trầy xước, hỏng dây màng, không đạt tiêu chuẩn cho thuê.
  4. *Thất lạc (`ThatLac`):* Băng bị khách hàng làm mất hoặc thất lạc trong quá trình luân chuyển.

---

### 2.1.3. Quy trình lập hồ sơ mượn băng video liên chi nhánh

Quy trình cho mượn băng được thực hiện hằng ngày tại quầy giao dịch của từng chi nhánh. Nhờ kiến trúc dữ liệu tập trung, khách hàng có thể đến bất kỳ chi nhánh nào trong chuỗi để mượn băng.

```
+-----------------------------------------------------------------------------------+
|               LƯU ĐỒ NGHIỆP VỤ LẬP PHIẾU MƯỢN BĂNG LIÊN CHI NHÁNH                 |
+-----------------------------------------------------------------------------------+
|  1. Nhận diện Khách hàng  ──► 2. Quét mã vạch Bản sao (MaBanSao) vào Giỏ mượn     |
|                                        │                                          |
|                                        ▼                                          |
|                               3. Kiểm tra Hợp lệ Nghiệp vụ:                       |
|                                  - TrangThai == "Sẵn sàng"?                       |
|                                  - MaCuaHangHienTai == Chi nhánh đang đứng?       |
|                                  - NgayHetHan >= Today?                           |
|                                        │                                          |
|                               (Hợp lệ) ▼                                          |
|  5. In Phiếu Mượn giao KH ◄── 4. Chốt Giao Dịch (BEGIN DATABASE TRANSACTION):     |
|                                  - INSERT PhieuMuon (MaKH, MaCH, MaNV, NgayMuon)  |
|                                  - INSERT ChiTietPhieuMuon cho từng bản sao       |
|                                  - UPDATE BanSaoBang.TrangThai = "Đang cho mượn"  |
+-----------------------------------------------------------------------------------+
```

#### 1. Các bước thực hiện chi tiết tại quầy
- **Bước 1: Chọn hoặc tra cứu Khách hàng:** Nhân viên chọn khách hàng từ danh sách thành viên hoặc nhập số điện thoại để tra cứu nhanh. Hệ thống kiểm tra xem khách hàng có đang giữ cuốn băng nào bị quá hạn nghiêm trọng hay không trước khi cho phép mượn mới.
- **Bước 2: Quét mã vạch các cuốn băng vào Giỏ mượn:** Nhân viên sử dụng máy đọc mã vạch quét tem `MaBanSao` trên vỏ băng.
- **Bước 3: Kiểm tra điều kiện ràng buộc toàn vẹn nghiệp vụ:** Trước khi đưa cuốn băng vào giỏ mượn, tầng Business Logic Layer (BLL) kiểm tra đồng thời 3 điều kiện tiên quyết:
  - *Kiểm tra trạng thái:* Cuốn băng bắt buộc phải có `TrangThai == "Sẵn sàng"`. Nếu băng đang cho mượn hoặc hư hỏng, lập tức thông báo lỗi.
  - *Kiểm tra đúng vị trí chi nhánh:* Thuộc tính `MaCuaHangHienTai` của cuốn băng phải khớp với `MaCuaHang` của chi nhánh đang thực hiện lập phiếu. Điều này ngăn chặn lỗi nhân viên chi nhánh A quét nhầm mã của cuốn băng đang ở chi nhánh B.
  - *Kiểm tra hạn sử dụng:* `NgayHetHan` của cuốn băng phải lớn hơn hoặc bằng ngày hiện tại.
- **Bước 4: Xác định thời gian hẹn trả (`NgayDuKienTra`):** Nhân viên chọn ngày dự kiến trả băng thỏa thuận với khách hàng (mặc định cộng thêm 3 ngày kể từ ngày lập phiếu).

#### 2. Cơ chế bảo đảm tính nguyên tử giao dịch (Atomic Database Transaction)
Khi nhân viên nhấn nút **"Chốt Phiếu Mượn"**, một giao dịch cơ sở dữ liệu (`IDbContextTransaction`) được kích hoạt nhằm đảm bảo tính toàn vẹn ACID:
1. Ghi bản ghi mới vào bảng `PhieuMuon`: Mã phiếu mượn (`MaPhieuMuon`), Mã khách hàng, Mã cửa hàng mượn, Mã nhân viên cho mượn, Ngày mượn (`DateTime.Now`) và Ngày dự kiến trả.
2. Duyệt qua từng cuốn băng trong giỏ mượn:
   - Ghi bản ghi chi tiết vào bảng `ChiTietPhieuMuon` (`MaPhieuMuon`, `MaBanSao`, `TrangThaiTra = false`).
   - Cập nhật trạng thái cuốn băng trong bảng `BanSaoBang`: `TrangThai = Constants.TrangThaiBang_DangChoMuon`.
3. Nếu tất cả thao tác thành công, gọi `transaction.Commit()` để lưu vĩnh viễn vào CSDL. Nếu phát sinh bất kỳ ngoại lệ nào (lỗi ngắt kết nối, khóa ngoại), gọi `transaction.Rollback()` để trả lại nguyên trạng CSDL, tuyệt đối không để xảy ra tình trạng phiếu mượn đã tạo nhưng trạng thái băng chưa cập nhật.

---

### 2.1.4. Quy trình xử lý sự kiện trả băng, nhập phạt thủ công và in biên lai thu tiền

Nghiệp vụ trả băng trong chuỗi liên kết mang tính đột phá nhờ cơ chế **"Trả chéo chi nhánh & Luân chuyển kho tự động" (Cross-Store Return & Automatic Inventory Relocation)**.

```
+-----------------------------------------------------------------------------------+
|               LƯU ĐỒ NGHIỆP VỤ NHẬN TRẢ BĂNG CHÉO CHI NHÁNH                       |
+-----------------------------------------------------------------------------------+
|  Khách hàng mang băng đến trả tại Chi nhánh B (Dù trước đó mượn tại Chi nhánh A)  |
|                                        │                                          |
|                                        ▼                                          |
|  1. Quét mã bản sao (MaBanSao) ──► 2. Tự động tìm ChiTietPhieuMuon đang mở gần nhất|
|                                        │                                          |
|                                        ▼                                          |
|  3. Kiểm tra & Tính tiền phạt:                                                    |
|     - Kiểm tra trễ hạn: Nếu Today > NgayDuKienTra ──► Cảnh báo số ngày trễ        |
|     - Kiểm tra vật lý: Nhập mô tả TinhTrangKhiTra & Tiền phạt vi phạm (TienPhat)  |
|                                        │                                          |
|                                        ▼                                          |
|  4. Chốt Nhận Trả & LUÂN CHUYỂN KHO TỰ ĐỘNG (BEGIN TRANSACTION):                  |
|     - INSERT PhieuTra & ChiTietPhieuTra (lưu tiền thuê, tiền phạt, tình trạng)    |
|     - UPDATE ChiTietPhieuMuon.TrangThaiTra = true                                 |
|     - UPDATE BanSaoBang.TrangThai = "Sẵn sàng"                                    |
|     - [QUAN TRỌNG] UPDATE BanSaoBang.MaCuaHangHienTai = Chi nhánh B vừa nhận trả! |
+-----------------------------------------------------------------------------------+
```

#### 1. Quy trình tiếp nhận trả băng không phụ thuộc địa điểm xuất phát
- **Bước 1: Quét mã băng nhận trả:** Khách hàng mang băng đến quầy của chi nhánh bất kỳ (ví dụ Chi nhánh 2 - `CH02`). Nhân viên quét mã `MaBanSao` trên cuốn băng.
- **Bước 2: Tự động đối soát giao dịch đang mở:** Hệ thống tự động tìm kiếm bản ghi `ChiTietPhieuMuon` có `MaBanSao` tương ứng và `TrangThaiTra == false`. Hệ thống lập tức hiển thị thông tin: Tên khách hàng mượn, Tựa đề phim, Chi nhánh xuất cho mượn ban đầu, Ngày mượn và Ngày hẹn trả.

#### 2. Quy tắc kiểm tra trễ hạn và tính toán phí bồi thường vi phạm
- **Kiểm tra thời gian trễ hạn:** Hệ thống tự động so sánh ngày trả thực tế (`DateTime.Today`) với `NgayDuKienTra`. Nếu phát hiện quá hạn, hệ thống hiển thị hộp thoại cảnh báo: *Số ngày trễ hạn = Ngày hiện tại - Ngày hẹn trả*.
- **Kiểm tra tình trạng vật lý và ghi nhận phạt:** Nhân viên kiểm tra ngoại quan cuốn băng (vỏ hộp, nhãn mác, dây màng từ). Nếu băng có hao tổn vật lý hoặc vi phạm trễ hạn, nhân viên nhập:
  - *Mô tả tình trạng khi trả (`TinhTrangKhiTra`):* Ví dụ *"Bình thường"*, *"Trễ hạn 2 ngày"*, *"Hỏng vỏ nhựa"*, *"Đứt dây băng"*.
  - *Số tiền phạt vi phạm (`TienPhat`):* Số tiền phạt do nhân viên nhập hoặc thỏa thuận theo quy chế cửa hàng.

#### 3. Chốt Phiếu Trả và tự động chuyển dịch vị trí kho hàng
Khi nhấn **"Chốt Nhận Trả"**, giao dịch CSDL thực hiện посл tiếp các thay đổi:
1. Tạo Phiếu Trả (`PhieuTra`) và Chi tiết Phiếu Trả (`ChiTietPhieuTra`), ghi nhận chính xác tiền thuê (`TienThue`) và tiền phạt (`TienPhat`).
2. Cập nhật `ChiTietPhieuMuon.TrangThaiTra = true` (đóng giao dịch mượn).
3. **Cập nhật luân chuyển kho tự động cho Bản sao băng (`BanSaoBang`):**
   - Đưa `TrangThai` trở lại **"Sẵn sàng"** (`Constants.TrangThaiBang_SanSang`).
   - Cập nhật `MaCuaHangHienTai` chuyển về **Mã chi nhánh đang tiếp nhận trả băng**.
   - *Ý nghĩa thực tiễn:* Ngay sau khi khách trả băng tại Chi nhánh B, cuốn băng lập tức xuất hiện trong kho "Sẵn sàng" của Chi nhánh B và có thể cho vị khách hàng tiếp theo tại Chi nhánh B mượn ngay mà không cần chờ xe vận chuyển vật lý về Chi nhánh A.

---

### 2.1.5. Quy trình tổng hợp hồ sơ và in thông báo nhắc trả băng hằng ngày

Nhằm duy trì kỷ luật thu hồi tài sản và quản lý nợ xấu, hệ thống cung cấp quy trình quét tự động danh sách các cuốn băng đang quá hạn:

```
+-----------------------------------------------------------------------------------+
|               QUY TRÌNH QUÉT VÀ NHẮC NHỞ BĂNG QUÁ HẠN HẰNG NGÀY                   |
+-----------------------------------------------------------------------------------+
|  Duyệt toàn bộ ChiTietPhieuMuon có TrangThaiTra == false & NgayDuKienTra < Today  |
|                                        │                                          |
|                                        ▼                                          |
|  Tổng hợp danh sách Băng Quá Hạn:                                                 |
|  + Khách hàng | SĐT liên hệ | Tựa phim | Mã bản sao | Ngày hẹn trả | Số ngày trễ  |
|                                        │                                          |
|                                        ▼                                          |
|  Nhân viên Xuất danh sách Excel / Gọi điện thông báo nhắc trả / Khóa quyền mượn   |
+-----------------------------------------------------------------------------------+
```

1. **Quét dữ liệu tự động theo thời gian thực:** Mỗi ngày, quản lý chi nhánh truy cập tab **"Quét Băng Quá Hạn"**. Hệ thống thực hiện câu lệnh LINQ kết nối 4 bảng (`ChiTietPhieuMuon`, `PhieuMuon`, `KhachHang`, `BanSaoBang`), lọc ra tất cả giao dịch chưa trả có `NgayDuKienTra < DateTime.Today`.
2. **Hiển thị thông tin tổng hợp trực quan:** Bảng kết quả hiển thị đầy đủ: *Tên khách hàng, Số điện thoại liên hệ, Tựa đề phim, Mã cuốn băng đang giữ, Ngày hẹn trả ban đầu và Số ngày đã trễ hạn*.
3. **Hành động nghiệp vụ thu hồi:** Nhân viên có thể xuất bảng dữ liệu này ra tệp Excel để gửi báo cáo điều hành, hoặc trực tiếp gọi điện thoại theo số liên lạc hiển thị trên màn hình để nhắc nhở khách hàng mang băng đến chi nhánh gần nhất hoàn trả.

---

## 2.2. XÁC ĐỊNH YÊU CẦU PHẦN MỀM

### 2.2.1. Tóm tắt tổng quan hệ thống phần mềm quản lý chuỗi cho thuê băng
Hệ thống phần mềm **Quản Lý Thuê Băng Đĩa Chuỗi Cửa Hàng (Enterprise Video Rental Management System)** được thiết kế và xây dựng trên nền tảng công nghệ Microsoft .NET 8.0 Windows Forms kết hợp hệ quản trị CSDL SQL Server (EF Core 8). Phần mềm mang sứ mệnh thay thế hoàn toàn sổ sách thủ công, tự động hóa từ khâu kiểm soát kho bản sao đa chi nhánh, thanh toán mượn/trả, giám sát vi phạm đến phân tích biểu đồ doanh thu theo chuẩn mực doanh nghiệp hiện đại.

---

### 2.2.2. Xác định các đối tượng sử dụng và phân quyền nghiệp vụ (RBAC)

Để đảm bảo tính bảo mật và sự tuân thủ kỷ luật vận hành trong mạng lưới nhiều cửa hàng, hệ thống thiết lập ma trận phân quyền dựa theo vai trò (**RBAC - Role-Based Access Control**) với 3 nhóm đối tượng người dùng chính:

```
+-----------------------------------------------------------------------------------+
|                        MA TRẬN PHÂN QUYỀN HỆ THỐNG (RBAC)                         |
+-----------------------------------+-------------------+-------------------+-------+
| Nhóm chức năng / Module           | Admin Cấp Cao     | Quản Lý Chi Nhánh | NV Quầy|
+-----------------------------------+-------------------+-------------------+-------+
| 1. Quản lý Phim gốc               | Toàn quyền        | Toàn quyền        | Xem   |
| 2. Quản lý Bản sao Băng           | Toàn quyền        | Theo kho chi nhánh| Xem   |
| 3. Quản lý Khách hàng             | Toàn quyền        | Toàn quyền        | Thêm  |
| 4. Nghiệp vụ Mượn - Trả Băng      | Toàn quyền        | Toàn quyền        | Mượn/Trả|
| 5. Quản lý Nhân viên              | Toàn quyền        | NV thuộc chi nhánh| Cấm   |
| 6. Quản lý Cửa hàng / Chi nhánh   | Toàn quyền        | Xem chi nhánh mình| Cấm   |
| 7. Dashboard Thống kê Biểu đồ     | Toàn chuỗi        | Theo chi nhánh    | Cấm   |
| 8. Hành động XÓA dữ liệu          | Được phép         | Được phép         | Cấm   |
+-----------------------------------+-------------------+-------------------+-------+
```

#### 1. Quản trị viên Hệ thống Cấp cao (`Admin_CapCao`)
- **Vai trò & Trách nhiệm:** Là người điều hành cao nhất của toàn chuỗi cửa hàng, chịu trách nhiệm quản trị cấu hình hệ thống, kiểm soát tài sản toàn diện và quản lý mạng lưới chi nhánh.
- **Phạm vi quyền hạn:**
  - Truy cập toàn bộ 8 module chức năng của phần mềm.
  - Xem và quản lý dữ liệu kho hàng, nhân viên, doanh thu của **tất cả các chi nhánh** trong hệ thống.
  - Có toàn quyền thực hiện thao tác **XÓA (`DELETE`)** hồ sơ tựa phim, bản sao băng, khách hàng, nhân viên hoặc cửa hàng khỏi cơ sở dữ liệu.

#### 2. Quản lý Cửa hàng / Chi nhánh (`QuanLy_ChiNhanh`)
- **Vai trò & Trách nhiệm:** Là người đứng đầu chịu trách nhiệm điều hành hoạt động kinh doanh tại một điểm bán (Chi nhánh cụ thể được phân công).
- **Phạm vi quyền hạn:**
  - Được quyền **Thêm mới, Cập nhật và Xóa (`DELETE`)** hồ sơ Phim, Bản sao băng, Khách hàng và Nhân viên trong phạm vi chi nhánh được phân công.
  - Quản lý danh sách nhân viên trực thuộc chi nhánh của mình.
  - Xem Dashboard thống kê biểu đồ doanh thu và tình trạng kho hàng thuộc chi nhánh công tác.
  - **Giới hạn bảo mật:** Không được truy cập hoặc xóa dữ liệu của các chi nhánh khác, không được quyền quản lý/xóa Cửa hàng trong toàn hệ thống.

#### 3. Nhân viên Quầy giao dịch (`NhanVien_Quay`)
- **Vai trò & Trách nhiệm:** Là nhân viên nghiệp vụ trực tiếp làm việc với khách hàng tại quầy, thực hiện các thủ tục cho mượn, nhận trả băng và ghi nhận khách hàng mới.
- **Phạm vi quyền hạn:**
  - Chỉ được quyền thao tác trên 3 nghiệp vụ chính: **Lập Phiếu Mượn Băng**, **Chốt Nhận Trả Băng** và **Thêm hồ sơ Khách hàng mới**.
  - Được xem tra cứu danh mục Phim và danh sách Bản sao đang sẵn sàng tại cửa hàng để tư vấn cho khách.
  - **Giới hạn bảo mật:** Giao diện tự động ẩn hoàn toàn các tab Quản lý Nhân viên, Quản lý Cửa hàng và Dashboard Báo cáo Biểu đồ. **Cấm thực hiện hành động Xóa dữ liệu** trên tất cả các danh mục.

---

### 2.2.3. Yêu cầu chức năng (Functional Requirements) cho 8 module cốt lõi

Phần mềm được chia thành 8 module nghiệp vụ độc lập nhưng liên thông chặt chẽ về mặt dữ liệu:

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

#### Module 1: Quản trị Phim gốc (Master Movie Management)
- **Mục đích:** Quản lý danh mục Tựa phim gốc (Master Catalog) của chuỗi cửa hàng.
- **Yêu cầu chức năng chi tiết:**
  - Hiển thị danh sách tựa phim dưới dạng DataGridView với đầy đủ cột thông tin: Mã phim, Tựa đề, Năm phát hành, Tên thể loại, Thời lượng phút.
  - Hỗ trợ tìm kiếm tức thì (Real-time Search) theo từ khóa tựa phim, lọc theo Thể loại hoặc Năm phát hành.
  - Thêm mới một bộ phim vào hệ thống với kiểm tra ràng buộc không được để trống mã phim và tựa đề.
  - Cập nhật chỉnh sửa thông tin bộ phim hiện có.
  - Tự động thống kê số lượng: Hiển thị tổng số bản sao hiện sở hữu và số bản sao đang sẵn sàng cho thuê đối với từng tựa phim.

#### Module 2: Quản lý Bản sao Băng vật lý (Physical Tape Copy Management)
- **Mục đích:** Quản lý từng cuốn băng thực tế nhập kho và luân chuyển giữa các chi nhánh.
- **Yêu cầu chức năng chi tiết:**
  - Nhập kho bản sao mới: Gán tự động mã `MaBanSao` (`[MaPhim]-BS[STT]`), chỉ định chi nhánh lưu kho ban đầu, chọn loại định dạng (`PAL`, `NTSC`), đơn giá thuê tiêu chuẩn và hạn sử dụng.
  - Cập nhật thông tin bản sao và thay đổi trạng thái vật lý (`Sẵn sàng`, `Bảo trì`, `Hư hỏng`, `Thất lạc`).
  - Lọc danh sách bản sao theo chi nhánh cụ thể và theo trạng thái lưu kho, giúp quản lý kiểm kê kho hàng nhanh chóng.

#### Module 3: Quản lý Khách hàng thành viên (Customer Management)
- **Mục đích:** Quản lý cơ sở dữ liệu khách hàng thân thiết toàn chuỗi.
- **Yêu cầu chức năng chi tiết:**
  - Đăng ký hồ sơ thành viên mới với mã `KHxxx`, lưu trữ đầy đủ Họ tên, CMND/CCCD, Số điện thoại và Địa chỉ.
  - Tìm kiếm nhanh khách hàng theo Số điện thoại hoặc Họ tên để hỗ trợ lập phiếu mượn/trả.
  - Thống kê lịch sử: Đếm tổng số lượt giao dịch mượn/trả mà khách hàng đã thực hiện trong hệ thống.

#### Module 4: Quản lý Nhân sự & Phân quyền (Employee & RBAC Management)
- **Mục đích:** Quản lý danh sách nhân sự vận hành và kiểm soát quyền hạn truy cập.
- **Yêu cầu chức năng chi tiết:**
  - Thêm mới hồ sơ nhân viên kèm thông tin tài khoản (`TenDangNhap`, `MatKhau`), phân bổ vào một chi nhánh cụ thể (`MaCuaHang`) và cấp mã vai trò (`Admin`, `Quản lý`, `Nhân viên`).
  - Hỗ trợ lọc danh sách nhân viên theo từng chi nhánh hoặc theo chức vụ.
  - Khóa/mở khóa tài khoản nhân viên khi có biến động nhân sự.

#### Module 5: Quản lý Cửa hàng / Chi nhánh (Store / Branch Management)
- **Mục đích:** Quản lý cấu trúc mạng lưới điểm kinh doanh của toàn chuỗi.
- **Yêu cầu chức năng chi tiết:**
  - Thêm mới hoặc cập nhật thông tin chi nhánh: Mã chi nhánh (`CHxx`), Tên/Địa chỉ chi nhánh, Số điện thoại liên hệ.
  - Hiển thị bảng tổng hợp toàn cảnh: Thống kê số lượng nhân viên đang trực thuộc và tổng số lượng cuốn băng đang lưu kho tại từng chi nhánh.

#### Module 6: Quản lý Danh mục Thể loại (Genre Management)
- **Mục đích:** Chuẩn hóa từ điển thể loại phim phục vụ phân loại và báo cáo.
- **Yêu cầu chức năng chi tiết:**
  - Thêm, sửa danh mục các thể loại chuẩn (*Hành động, Hoạt hình, Tâm lý, Viễn tưởng, Hài hước...*), đảm bảo tính nhất quán dữ liệu cho toàn bộ danh mục phim gốc.

#### Module 7: Nghiệp vụ Mượn - Trả Băng & Luân chuyển kho liên chi nhánh (Cross-Store Rental & Return Workflow)
- **Mục đích:** Xử lý giao dịch mượn, trả băng hằng ngày tại quầy giao dịch.
- **Yêu cầu chức năng chi tiết:**
  - *Tab 1 - Lập Phiếu Mượn Băng:*
    - Chọn Khách hàng mượn, Cửa hàng mượn và Nhân viên lập phiếu.
    - Quét mã vạch `MaBanSao` vào giỏ mượn (đảm bảo kiểm tra băng sẵn sàng, đúng kho và còn hạn).
    - Tự động cộng dồn tổng tiền thuê dự kiến hiển thị trực quan cho khách hàng.
    - Chốt phiếu mượn với giao dịch an toàn (`BeginTransaction`), tự động chuyển trạng thái băng sang *"Đang cho mượn"*.
  - *Tab 2 - Chốt Nhận Trả Băng (Cross-Store Return):*
    - Quét mã `MaBanSao` bất kỳ để tự động tra cứu Phiếu Mượn đang mở gần nhất.
    - Tự động cảnh báo nếu băng trả muộn hơn ngày hẹn trả (`SoNgayTreHan`).
    - Hỗ trợ nhập tiền phạt vi phạm (`TienPhat`) và ghi nhận tình trạng vật lý khi trả (`TinhTrangKhiTra`).
    - Chốt Phiếu Trả, tự động đồng bộ trạng thái băng về *"Sẵn sàng"* và cập nhật `MaCuaHangHienTai` về địa điểm chi nhánh tiếp nhận trả.
  - *Tab 3 - Quét Danh Sách Quá Hạn:*
    - Quét và hiển thị danh sách toàn bộ khách hàng đang giữ băng quá hạn hẹn trả để tiến hành gọi điện thu hồi.

#### Module 8: Dashboard Báo cáo & Thống kê Trực quan (Executive Analytics & Charting)
- **Mục đích:** Cung cấp tầm nhìn quản trị toàn cảnh và biểu đồ phân tích cho lãnh đạo doanh nghiệp.
- **Yêu cầu chức năng chi tiết:**
  - *Thẻ chỉ số KPI tổng quan (KPI Metric Cards):* Hiển thị tức thì 4 chỉ số cốt lõi: **Tổng doanh thu thực thu (VNĐ)**, **Tổng số tựa phim**, **Tổng số bản sao vật lý** và **Số giao dịch đang cho mượn active**.
  - *Biểu đồ Cột Doanh Thu (Revenue Bar Chart):* Sử dụng thư viện **ScottPlot 5** vẽ biểu đồ cột thể hiện sự tăng trưởng doanh thu theo thời gian, giúp nhà quản lý đánh giá hiệu quả kinh doanh.
  - *Biểu đồ Tròn Tình Trạng Kho (Inventory Status Pie Chart):* Vẽ biểu đồ tròn trực quan hóa tỷ lệ phần trăm kho hàng: *Tỷ lệ Sẵn sàng vs. Tỷ lệ Đang cho mượn vs. Tỷ lệ Hư hỏng*.
  - *Bảng xếp hạng Phim Thịnh Hành (Top 5 Trending Movies):* Liệt kê Top 5 bộ phim có lượt mượn cao nhất trong kỳ, hỗ trợ ra quyết định nhập thêm bản sao mới cho các phim hot.

---

### 2.2.4. Yêu cầu phi chức năng (Non-Functional Requirements)

Bên cạnh các chức năng nghiệp vụ, phần mềm phải đáp ứng nghiêm ngặt 4 nhóm yêu cầu phi chức năng nhằm bảo đảm tính chuyên nghiệp và ổn định vận hành:

```
+-----------------------------------------------------------------------------------+
|                  4 TRỤ CỘT YÊU CẦU PHI CHỨC NĂNG CỦA HỆ THỐNG                     |
+---------------------+---------------------+-------------------+-------------------+
| 1. BẢO MẬT & PHÂN   | 2. TOÀN VẸN GIAO    | 3. HIỆU NĂNG &    | 4. GIAO DIỆN HIỆN |
|    QUYỀN (SECURITY) |    DỊCH (ACID)      |    TỐC ĐỘ (SPEED) |    ĐẠI (UI/UX)    |
+---------------------+---------------------+-------------------+-------------------+
| Kiểm tra vai trò    | Mọi thao tác Lập    | Tốc độ tra cứu và | Bảng màu Pastel   |
| RBAC trên từng form | Phiếu Mượn/Trả đều  | quét mã vạch phản | sang trọng, nút   |
| Tự động khóa nút    | bọc trong Database  | hồi dưới 500ms    | Flat bo góc, phông|
| Xóa với Nhân viên   | Transaction, tự động| với CSDL 100.000  | Segoe UI Semibold |
| quầy giao dịch.     | Rollback khi có lỗi.| bản ghi.          | chuẩn Enterprise. |
+---------------------+---------------------+-------------------+-------------------+
```

1. **Yêu cầu về Bảo mật & Phân quyền (Security & Access Control):**
   - Mọi phiên truy cập hệ thống đều bắt buộc phải xác thực qua màn hình Đăng nhập (`LoginForm`).
   - Mật khẩu người dùng phải được bảo vệ an toàn.
   - Phân quyền RBAC được thực thi đa tầng: Kiểm soát hiển thị ở tầng UI (tự động ẩn các tab/nút không hợp lệ) và kiểm soát thực thi ở tầng Business Logic Layer (từ chối giao dịch nếu tài khoản không đủ thẩm quyền).
2. **Yêu cầu về Toàn vẹn dữ liệu & Giao dịch an toàn (Data Integrity & ACID Compliance):**
   - Mọi giao dịch Lập Phiếu Mượn và Chốt Nhận Trả liên quan đến nhiều bảng dữ liệu (`PhieuMuon`, `ChiTietPhieuMuon`, `BanSaoBang`) phải tuân thủ chuẩn nguyên tử ACID.
   - Nếu xảy ra lỗi ngắt kết nối hoặc vi phạm khóa ngoại trong quá trình ghi dữ liệu, giao dịch phải tự động `Rollback` hoàn toàn 100%, tuyệt đối không để xảy ra tình trạng lệch dữ liệu kho.
3. **Yêu cầu về Hiệu năng & Tốc độ đồng bộ (Performance & Speed):**
   - Tốc độ phản hồi cho thao tác tra cứu mã vạch, quét băng vào giỏ mượn không vượt quá 500ms.
   - Các truy vấn thống kê Dashboard phức tạp sử dụng LINQ được tối ưu hóa qua `AsNoTracking()` và chỉ chọn lọc các cột cần thiết (`Select DTO`), đảm bảo thời gian tải biểu đồ dưới 1 giây.
4. **Yêu cầu về Trải nghiệm Người dùng & Giao diện (Modern UI/UX & Responsive Design):**
   - Giao diện người dùng tuân thủ nguyên tắc thiết kế phẳng (Flat Design), sử dụng bảng màu Pastel sang trọng, phông chữ `Segoe UI Semibold` mượt mà và nút bấm phẳng bo góc.
   - Biểu đồ ScottPlot 5 và các bảng dữ liệu DataGridView hỗ trợ cơ chế tự động co giãn (`Dock = DockStyle.Fill`, `Anchor`), bảo đảm hiển thị sắc nét, không vỡ layout trên mọi kích thước cửa sổ màn hình từ Laptop tiêu chuẩn đến màn hình Full HD / 4K.

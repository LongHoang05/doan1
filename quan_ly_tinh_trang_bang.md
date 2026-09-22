# Quản lý tình trạng băng đĩa

Nội dung dưới đây được viết để bổ sung vào báo cáo đồ án, giúp làm rõ nghiệp vụ và cách xử lý liên quan đến "Tình trạng vật lý" của băng đĩa.

---

## 1. Phân tích yêu cầu nghiệp vụ (Nghiệp vụ quản lý tình trạng)

Tình trạng vật lý của băng đĩa là một yếu tố quan trọng ảnh hưởng trực tiếp đến chất lượng dịch vụ và doanh thu của cửa hàng. Hệ thống cần quản lý và theo dõi chặt chẽ tình trạng của từng cuốn băng/đĩa cụ thể trong suốt vòng đời sử dụng (từ lúc nhập kho đến khi thanh lý).

Các trạng thái chính của băng đĩa bao gồm:
*   **Tốt (Sẵn sàng cho thuê):** Băng đĩa hoạt động tốt, chất lượng hình/âm thanh đảm bảo, không có hư hại vật lý đáng kể.
*   **Trầy xước nhẹ:** Có dấu hiệu sử dụng, xước nhẹ nhưng vẫn có thể phát bình thường. Vẫn tiếp tục cho thuê nhưng cần lưu ý theo dõi.
*   **Hỏng (Không thể sử dụng):** Băng đĩa bị nứt, gãy, hỏng từ, xước nặng hoặc không thể đọc được. Cần ngưng cho thuê và đưa vào danh sách chờ thanh lý hoặc tiêu hủy.
*   **Mất mát:** Băng đĩa bị mất trong quá trình khách hàng thuê hoặc thất thoát tại kho.

## 2. Quy trình xử lý khi trả băng đĩa

Nhân viên cửa hàng có trách nhiệm kiểm tra tình trạng vật lý của băng đĩa mỗi khi khách hàng đến trả:

*   **Trường hợp băng đĩa ở tình trạng bình thường:** Nhân viên tiến hành nhận trả băng, hệ thống tự động cập nhật trạng thái cuốn băng đó thành "Sẵn sàng cho thuê".
*   **Trường hợp phát sinh hư hỏng hoặc mất mát (do lỗi của khách hàng):**
    *   Hệ thống phải cho phép nhân viên ghi nhận tình trạng thực tế của băng đĩa ngay tại thời điểm lập phiếu trả (Chọn tình trạng: "Hỏng" hoặc "Mất").
    *   Hệ thống cung cấp chức năng tính toán hoặc cho phép nhập tay khoản phí phạt bù trừ dựa trên quy định của cửa hàng (ví dụ: bồi thường 100% giá trị nếu mất băng đĩa, hoặc thu phí bồi thường hư hại tùy vào mức độ).
    *   Sau khi hoàn tất phiếu trả, hệ thống tự động cập nhật trạng thái cuốn băng đĩa thành "Hỏng" hoặc "Mất", đồng thời **khóa** cuốn băng này lại, không cho phép hiển thị trong các giao dịch cho thuê tiếp theo.

## 3. Cập nhật thiết kế Cơ sở dữ liệu

Để đáp ứng yêu cầu quản lý trên, cơ sở dữ liệu của hệ thống cần được bổ sung và điều chỉnh một số thuộc tính sau:

*   **Tại bảng lưu trữ chi tiết cuốn băng đĩa (`CuonBang` hoặc `ChiTietBangDia`):**
    *   Bổ sung trường `TinhTrang` (Kiểu dữ liệu: `NVARCHAR` hoặc `INT` nếu dùng bảng danh mục riêng). Các giá trị cơ bản: Tốt, Trầy xước, Hỏng, Mất.
    *   *Ghi chú quan trọng:* Trạng thái này phải được gắn với từng **mã vạch (barcode/ID) của cuốn băng vật lý cụ thể**, không gắn chung vào bảng thông tin tựa phim/album, vì cùng một tựa phim có thể có nhiều cuốn với tình trạng khác nhau.
*   **Tại bảng lưu trữ chi tiết phiếu trả (`ChiTietPhieuTra`):**
    *   Bổ sung trường `TinhTrangLucTra` (Kiểu `NVARCHAR`) để lưu lại vết/lịch sử tình trạng khi khách đem trả.
    *   Bổ sung trường `PhiPhatHuHong` (Kiểu `MONEY` hoặc `DECIMAL`) để lưu số tiền phạt phát sinh do làm hỏng hoặc mất băng đĩa trong giao dịch đó.

## 4. Cập nhật thiết kế Giao diện (UI)

*   **Tại Giao diện Quản lý kho / Danh sách Băng đĩa:** Thêm cột "Tình trạng" vào lưới dữ liệu (Grid) để người quản lý dễ dàng lọc, tìm kiếm và thống kê các băng đĩa bị hỏng cần mang đi thanh lý.
*   **Tại Giao diện Lập phiếu trả băng đĩa:** Bổ sung một cột thả xuống (Dropdown list/ComboBox) ở phần chi tiết băng đĩa khách trả để chọn "Tình trạng thực tế". Nếu nhân viên chọn "Hỏng" hoặc "Mất", phần mềm sẽ tự động hiển thị thêm một ô nhập liệu (TextBox) để điền "Tiền phạt". Tiền phạt này sẽ được cộng dồn vào tổng tiền thanh toán của hóa đơn trả.

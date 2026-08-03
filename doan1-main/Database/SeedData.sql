USE QuanLyBangVideo;
GO

-- Xóa dữ liệu cũ
DELETE FROM NHATKY_HETHONG;
DELETE FROM CHITIETTRA;
DELETE FROM HOSOTRA;
DELETE FROM CHITIETMUON;
DELETE FROM HOSOMUON;
DELETE FROM BANGVIDEO;
DELETE FROM BANGVIDEOGOC;
DELETE FROM PHIM;
DELETE FROM NHANVIEN;
DELETE FROM KHACHHANG;
DELETE FROM CUAHANG;
DELETE FROM CAUHINH_HETHONG;

GO

-- Thêm cấu hình hệ thống
INSERT INTO CAUHINH_HETHONG (MaCauHinh, GiaTri, MoTa, KieuDuLieu) VALUES 
('MAX_RENT_DAYS', '7', N'Số ngày mượn băng tối đa mặc định', 'INT'),
('LATE_FEE_MULTIPLIER', '1.5', N'Hệ số phạt khi quá hạn (phạt x1.5)', 'DECIMAL'),
('COMPANY_NAME', N'Công Ty Cho Thuê Băng Đĩa VideoRental Pro', N'Tên công ty in trên biên lai', 'STRING');

-- CUA HANG
SET IDENTITY_INSERT CUAHANG ON;
INSERT INTO CUAHANG (MaCH, DiaChi, SDT) VALUES
(1, N'123 Nguyễn Huệ, Quận 1, TP.HCM', '028-1234-5678'),
(2, N'456 Lê Lợi, Quận 3, TP.HCM', '028-2345-6789'),
(3, N'789 Trần Hưng Đạo, Quận 5, TP.HCM', '028-3456-7890');
SET IDENTITY_INSERT CUAHANG OFF;

-- KHACH HANG
SET IDENTITY_INSERT KHACHHANG ON;
INSERT INTO KHACHHANG (MaKH, CMND, HoTen, DiaChi) VALUES
(1, '079123456789', N'Nguyễn Văn An', N'12 Lý Tự Trọng, Q.1'),
(2, '079234567890', N'Trần Thị Bình', N'34 Hai Bà Trưng, Q.3'),
(3, '079345678901', N'Lê Minh Cường', N'56 Pasteur, Q.1'),
(4, '079456789012', N'Phạm Quang Dũng', N'78 Nguyễn Đình Chiểu, Q.3'),
(5, '079567890123', N'Vũ Bích Hạnh', N'90 Điện Biên Phủ, Q.Bình Thạnh');
SET IDENTITY_INSERT KHACHHANG OFF;

-- NHAN VIEN
SET IDENTITY_INSERT NHANVIEN ON;
INSERT INTO NHANVIEN (MaNV, CMND, HoTen, DiaChi, MaCuaHang, TenDangNhap, MatKhau, VaiTro) VALUES
(1, '079111222333', N'Quản Trị Viên (Admin)', N'Quận 1', 1, 'admin', '123456', 'Admin'),
(2, '079222333444', N'Võ Thanh Phong', N'Quận 3', 1, 'vophong', '123456', 'Staff'),
(3, '079333444555', N'Đặng Thúy Quỳnh', N'Quận 1', 1, 'thuyquynh', '123456', 'Staff'),
(4, '079444555666', N'Trương Minh Tuấn', N'Quận 5', 2, 'minhtuan', '123456', 'Staff');
SET IDENTITY_INSERT NHANVIEN OFF;

-- PHIM
SET IDENTITY_INSERT PHIM ON;
INSERT INTO PHIM (MaPhim, TuaDe, NamPhatHanh, TheLoai, DoDai) VALUES
(1, N'Inception', 2010, N'Khoa học viễn tưởng', 148),
(2, N'The Dark Knight', 2008, N'Hành động', 152),
(3, N'Interstellar', 2014, N'Khoa học viễn tưởng', 169),
(4, N'Parasite', 2019, N'Tâm lý', 132),
(5, N'Avengers: Endgame', 2019, N'Hành động', 181),
(6, N'Spirited Away', 2001, N'Hoạt hình', 125),
(7, N'The Conjuring', 2013, N'Kinh dị', 112);
SET IDENTITY_INSERT PHIM OFF;

-- BANG VIDEOGOC
SET IDENTITY_INSERT BANGVIDEOGOC ON;
INSERT INTO BANGVIDEOGOC (MaBangGoc, MaPhim) VALUES
(1, 1), (2, 2), (3, 3), (4, 4), (5, 5), (6, 6), (7, 7);
SET IDENTITY_INSERT BANGVIDEOGOC OFF;

-- BANG VIDEO
INSERT INTO BANGVIDEO (MaBangGoc, SoThuTu, MaCuaHang, LoaiBang, DonGia, NgayHetHan, TrangThai) VALUES
(1, 1, 1, 'PAL', 15000, '2026-12-31', 'CoSan'),
(1, 2, 1, 'NTSC', 15000, '2026-12-31', 'CoSan'),
(1, 3, 2, 'PAL', 15000, '2026-12-31', 'CoSan'),
(2, 1, 1, 'PAL', 12000, '2027-06-30', 'CoSan'),
(2, 2, 3, 'PAL', 12000, '2027-06-30', 'CoSan'),
(5, 1, 1, 'PAL', 20000, '2025-12-31', 'CoSan'),
(5, 2, 1, 'PAL', 20000, '2025-12-31', 'CoSan'),
(5, 3, 2, 'NTSC', 20000, '2025-12-31', 'CoSan'),
(5, 4, 3, 'PAL', 20000, '2025-12-31', 'CoSan');

-- HO SO MUON
SET IDENTITY_INSERT HOSOMUON ON;
INSERT INTO HOSOMUON (MaMuon, MaKH, MaCuaHang, MaNV, NgayMuon, NgayDuKienTra) VALUES
(1, 1, 1, 2, '2026-07-01', '2026-07-08'); 
SET IDENTITY_INSERT HOSOMUON OFF;

INSERT INTO CHITIETMUON (MaMuon, MaBangGoc, SoThuTu) VALUES
(1, 1, 1),
(1, 2, 1);

UPDATE BANGVIDEO SET TrangThai = 'DangMuon' WHERE (MaBangGoc=1 AND SoThuTu=1) OR (MaBangGoc=2 AND SoThuTu=1);

-- NHAT KY
INSERT INTO NHATKY_HETHONG (ThoiGian, MaNV, HanhDong, ChiTiet) VALUES
('2026-07-01 08:00:00', 1, N'ĐĂNG NHẬP', N'Admin đăng nhập thành công'),
('2026-07-01 09:15:00', 2, N'THÊM KHÁCH HÀNG', N'Thêm khách hàng Nguyễn Văn An (079123456789)'),
('2026-07-01 10:30:00', 2, N'LẬP HỒ SƠ MƯỢN', N'Khách Nguyễn Văn An mượn 2 cuốn băng (Hồ sơ #1)');

PRINT N'Thêm dữ liệu mẫu thành công!';

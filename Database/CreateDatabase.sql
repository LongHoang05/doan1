-- =============================================
-- TẠO DATABASE QUẢN LÝ CHO THUÊ BĂNG VIDEO
-- Đồ Án 1 - Hệ Thống Quản Lý Cho Thuê Băng Video
-- (Bản nâng cấp có Xóa Mềm, Log, Phân quyền)
-- =============================================

CREATE DATABASE QuanLyBangVideo;
GO
USE QuanLyBangVideo;
GO

-- =============================================
-- 1. BẢNG CỬA HÀNG
-- =============================================
CREATE TABLE CUAHANG (
    MaCH        INT IDENTITY(1,1) PRIMARY KEY,
    DiaChi      NVARCHAR(200)   NOT NULL,
    SDT         VARCHAR(15)     NOT NULL,
    DaXoa       BIT             NOT NULL DEFAULT 0
);

-- =============================================
-- 2. BẢNG KHÁCH HÀNG
-- =============================================
CREATE TABLE KHACHHANG (
    MaKH        INT IDENTITY(1,1) PRIMARY KEY,
    CMND        VARCHAR(20)     NOT NULL UNIQUE,
    HoTen       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NOT NULL,
    DaXoa       BIT             NOT NULL DEFAULT 0
);

-- =============================================
-- 3. BẢNG NHÂN VIÊN (Tích hợp Phân quyền)
-- =============================================
CREATE TABLE NHANVIEN (
    MaNV        INT IDENTITY(1,1) PRIMARY KEY,
    CMND        VARCHAR(20)     NOT NULL UNIQUE,
    HoTen       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NOT NULL,
    MaCuaHang   INT             NOT NULL,
    
    -- Tài khoản đăng nhập
    TenDangNhap VARCHAR(50)     NULL UNIQUE,
    MatKhau     VARCHAR(255)    NULL,
    VaiTro      VARCHAR(20)     NOT NULL DEFAULT 'Staff', -- 'Admin', 'Staff'
    DaXoa       BIT             NOT NULL DEFAULT 0,

    CONSTRAINT FK_NV_CuaHang FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH)
);

-- =============================================
-- 4. BẢNG PHIM
-- =============================================
CREATE TABLE PHIM (
    MaPhim      INT IDENTITY(1,1) PRIMARY KEY,
    TuaDe       NVARCHAR(200)   NOT NULL,
    NamPhatHanh INT             NOT NULL,
    TheLoai     NVARCHAR(50)    NOT NULL,
    DoDai       INT             NOT NULL,
    DaXoa       BIT             NOT NULL DEFAULT 0
);

-- =============================================
-- 5. BẢNG BĂNG VIDEO GỐC
-- =============================================
CREATE TABLE BANGVIDEOGOC (
    MaBangGoc   INT IDENTITY(1,1) PRIMARY KEY,
    MaPhim      INT             NOT NULL,
    DaXoa       BIT             NOT NULL DEFAULT 0,
    CONSTRAINT FK_BVG_Phim FOREIGN KEY (MaPhim) REFERENCES PHIM(MaPhim)
);

-- =============================================
-- 6. BẢNG BĂNG VIDEO (bản sao) — Khóa phức hợp
-- =============================================
CREATE TABLE BANGVIDEO (
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,
    MaCuaHang   INT             NOT NULL,
    LoaiBang    VARCHAR(10)     NOT NULL,  
    DonGia      DECIMAL(10,2)   NOT NULL,
    NgayHetHan  DATE            NOT NULL,
    TrangThai   VARCHAR(20)     NOT NULL DEFAULT 'CoSan', 
    DaXoa       BIT             NOT NULL DEFAULT 0,

    CONSTRAINT PK_BangVideo PRIMARY KEY (MaBangGoc, SoThuTu),
    CONSTRAINT FK_BV_BangGoc FOREIGN KEY (MaBangGoc) REFERENCES BANGVIDEOGOC(MaBangGoc),
    CONSTRAINT FK_BV_CuaHang FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH),
    CONSTRAINT CK_TrangThai CHECK (TrangThai IN ('CoSan', 'DangMuon', 'HetHan'))
);

-- =============================================
-- 7. BẢNG HỒ SƠ MƯỢN
-- =============================================
CREATE TABLE HOSOMUON (
    MaMuon      INT IDENTITY(1,1) PRIMARY KEY,
    MaKH        INT             NOT NULL,
    MaCuaHang   INT             NOT NULL,
    MaNV        INT             NOT NULL,
    NgayMuon    DATE            NOT NULL,
    NgayDuKienTra DATE          NOT NULL,

    CONSTRAINT FK_HSM_KH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
    CONSTRAINT FK_HSM_CH FOREIGN KEY (MaCuaHang) REFERENCES CUAHANG(MaCH),
    CONSTRAINT FK_HSM_NV FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV),
    CONSTRAINT CK_NgayTra CHECK (NgayDuKienTra >= NgayMuon)
);

-- =============================================
-- 8. BẢNG CHI TIẾT MƯỢN (Cascade Delete)
-- =============================================
CREATE TABLE CHITIETMUON (
    MaMuon      INT             NOT NULL,
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,

    CONSTRAINT PK_ChiTietMuon PRIMARY KEY (MaMuon, MaBangGoc, SoThuTu),
    CONSTRAINT FK_CTM_HSM FOREIGN KEY (MaMuon) REFERENCES HOSOMUON(MaMuon) ON DELETE CASCADE,
    CONSTRAINT FK_CTM_BV FOREIGN KEY (MaBangGoc, SoThuTu) REFERENCES BANGVIDEO(MaBangGoc, SoThuTu)
);

-- =============================================
-- 9. BẢNG HỒ SƠ TRẢ
-- =============================================
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

-- =============================================
-- 10. BẢNG CHI TIẾT TRẢ (FK chặt chẽ trỏ về CHITIETMUON)
-- =============================================
CREATE TABLE CHITIETTRA (
    MaTra       INT             NOT NULL,
    MaBangGoc   INT             NOT NULL,
    SoThuTu     INT             NOT NULL,
    MaMuon      INT             NOT NULL,
    TienThue    DECIMAL(10,2)   NOT NULL,

    CONSTRAINT PK_ChiTietTra PRIMARY KEY (MaTra, MaBangGoc, SoThuTu),
    CONSTRAINT FK_CTT_HST FOREIGN KEY (MaTra) REFERENCES HOSOTRA(MaTra) ON DELETE CASCADE,
    
    -- Đảm bảo chỉ được trả đúng cuốn băng đã mượn trong hồ sơ mượn đó
    CONSTRAINT FK_CTT_CTM FOREIGN KEY (MaMuon, MaBangGoc, SoThuTu) 
        REFERENCES CHITIETMUON(MaMuon, MaBangGoc, SoThuTu)
);

-- =============================================
-- 11. BẢNG NHẬT KÝ HỆ THỐNG
-- =============================================
CREATE TABLE NHATKY_HETHONG (
    MaLog       INT IDENTITY(1,1) PRIMARY KEY,
    ThoiGian    DATETIME        NOT NULL DEFAULT GETDATE(),
    MaNV        INT             NOT NULL,
    HanhDong    NVARCHAR(100)   NOT NULL,
    ChiTiet     NVARCHAR(500)   NOT NULL,

    CONSTRAINT FK_Log_NV FOREIGN KEY (MaNV) REFERENCES NHANVIEN(MaNV)
);

-- =============================================
-- 12. BẢNG CẤU HÌNH HỆ THỐNG
-- =============================================
CREATE TABLE CAUHINH_HETHONG (
    MaCauHinh   VARCHAR(50)     PRIMARY KEY,
    GiaTri      NVARCHAR(100)   NOT NULL,
    MoTa        NVARCHAR(200)   NOT NULL,
    KieuDuLieu  VARCHAR(20)     NOT NULL DEFAULT 'STRING'
);

-- =============================================
-- INDEX PHỤ TRỢ (Lọc các bản ghi chưa xóa mềm)
-- =============================================
CREATE INDEX IX_BangVideo_TrangThai ON BANGVIDEO(TrangThai);
CREATE INDEX IX_HoSoMuon_NgayDuKienTra ON HOSOMUON(NgayDuKienTra);
CREATE INDEX IX_NhanVien_DaXoa ON NHANVIEN(DaXoa);
CREATE INDEX IX_KhachHang_DaXoa ON KHACHHANG(DaXoa);
CREATE INDEX IX_Phim_DaXoa ON PHIM(DaXoa);
CREATE INDEX IX_BangVideo_DaXoa ON BANGVIDEO(DaXoa);
CREATE INDEX IX_Log_ThoiGian ON NHATKY_HETHONG(ThoiGian DESC);

PRINT N'Tạo database QuanLyBangVideo thành công (Bản nâng cấp Xóa Mềm, Phân Quyền, Log)!';
GO

using System;
using Xunit;

namespace QuanLyThueBang.Tests
{
    public class PhieuTraBLL
    {
        public decimal TinhTienPhat(int soNgayTre, decimal donGia)
        {
            if (soNgayTre > 0) return soNgayTre * (donGia * 0.5m);
            return 0m;
        }

        public decimal TinhTienDenBu(string tinhTrang, decimal donGia)
        {
            if (tinhTrang == "Hư hỏng") return 50000m;
            if (tinhTrang == "Thất lạc") return donGia * 3;
            return 0m;
        }
    }

    public class PhieuMuonBLL
    {
        public bool KiemTraDieuKienMuon(decimal noPhat, int soBangDangGiu)
        {
            if (noPhat > 0) return false;
            if (soBangDangGiu >= 5) return false;
            return true;
        }
    }

    // ================================
    // 1. UNIT TEST HÀM TÍNH TIỀN PHẠT
    // ================================
    public class Test_HamTinhTienPhat
    {
        [Fact]
        public void Test_TinhTienPhat_TraTre3Ngay_TraVeTienPhatDung()
        {
            int soNgayTre = 3;
            decimal donGia = 15000m;
            decimal expectedPhạt = 22500m;
            PhieuTraBLL bll = new PhieuTraBLL();

            // Act (Thực thi hàm)
            decimal actualPhạt = bll.TinhTienPhat(soNgayTre, donGia);

            // Assert (Kiểm tra kết quả)
            Assert.Equal(expectedPhạt, actualPhạt);
        }

        [Fact]
        public void Test_TinhTienPhat_TraDungHan_TraVeKhongDong()
        {
            // Arrange
            int soNgayTre = 0;
            decimal donGia = 20000m;
            decimal expectedPhạt = 0m;
            PhieuTraBLL bll = new PhieuTraBLL();

            // Act
            decimal actualPhạt = bll.TinhTienPhat(soNgayTre, donGia);

            // Assert
            Assert.Equal(expectedPhạt, actualPhạt);
        }
    }

    // =========================================
    // 2. UNIT TEST HÀM KIỂM TRA ĐIỀU KIỆN MƯỢN 
    // =========================================
    public class Test_HamKiemTraDieuKienMuon
    {
        [Fact]
        public void Test_KiemTraDieuKienMuon_KhachDangNoTien_TraVeFalse()
        {
            // Arrange
            decimal noPhat = 50000m;
            int soBangDangGiu = 2;
            PhieuMuonBLL bll = new PhieuMuonBLL();

            // Act
            bool ketQua = bll.KiemTraDieuKienMuon(noPhat, soBangDangGiu);

            // Assert
            Assert.False(ketQua, "Phải trả về False vì khách đang nợ tiền.");
        }

        [Fact]
        public void Test_KiemTraDieuKienMuon_KhachGiu5Cuon_TraVeFalse()
        {
            // Arrange
            decimal noPhat = 0m;
            int soBangDangGiu = 5; // Max limit
            PhieuMuonBLL bll = new PhieuMuonBLL();

            // Act
            bool ketQua = bll.KiemTraDieuKienMuon(noPhat, soBangDangGiu);

            // Assert
            Assert.False(ketQua, "Phải trả về False vì khách đã mượn đủ 5 cuốn.");
        }

        [Fact]
        public void Test_KiemTraDieuKienMuon_HopLe_TraVeTrue()
        {
            // Arrange
            decimal noPhat = 0m;
            int soBangDangGiu = 3;
            PhieuMuonBLL bll = new PhieuMuonBLL();

            // Act
            bool ketQua = bll.KiemTraDieuKienMuon(noPhat, soBangDangGiu);

            // Assert
            Assert.True(ketQua, "Phải trả về True vì khách thỏa mãn mọi điều kiện.");
        }
    }

    // ==================================
    // 3. UNIT TEST HÀM TÍNH TIỀN ĐỀN BÙ
    // ==================================
    public class Test_HamTinhTienDenBu
    {
        [Fact]
        public void Test_TinhTienDenBu_HuHong_Phat50K()
        {
            // Arrange
            string tinhTrang = "Hư hỏng";
            decimal donGia = 15000m;
            PhieuTraBLL bll = new PhieuTraBLL();

            // Act
            decimal tienDenBu = bll.TinhTienDenBu(tinhTrang, donGia);

            // Assert
            Assert.Equal(50000m, tienDenBu);
        }

        [Fact]
        public void Test_TinhTienDenBu_ThatLac_PhatGap3Lan()
        {
            // Arrange
            string tinhTrang = "Thất lạc";
            decimal donGia = 15000m;
            PhieuTraBLL bll = new PhieuTraBLL();

            // Act
            decimal tienDenBu = bll.TinhTienDenBu(tinhTrang, donGia);

            // Assert
            Assert.Equal(45000m, tienDenBu); // 15k * 3
        }

        [Fact]
        public void Test_TinhTienDenBu_BinhThuong_KhongPhat()
        {
            // Arrange
            string tinhTrang = "Có sẵn";
            decimal donGia = 15000m;
            PhieuTraBLL bll = new PhieuTraBLL();

            // Act
            decimal tienDenBu = bll.TinhTienDenBu(tinhTrang, donGia);

            // Assert
            Assert.Equal(0m, tienDenBu);
        }
    }
}

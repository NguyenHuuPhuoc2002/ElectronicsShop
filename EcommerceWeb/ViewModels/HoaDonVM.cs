using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.ViewModels
{
    public class HoaDonVM
    {
        public int MaHd { get; set; }

        public string MaKh { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayDat { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayGiao { get; set; }

        public string HoTen { get; set; }

        public string DiaChi { get; set; }

        public string DienThoai { get; set; }

        public string CachThanhToan { get; set; }

        public double PhiVanChuyen { get; set; }

        public string TrangThai { get; set; }
        public int MaTrangThai { get; set; }

        public string MaNv { get; set; }

        public string GhiChu { get; set; }
    }
}

namespace EcommerceWeb.ViewModels
{
    public class ChiTietHoaDonVM
    {
        public int MaCT {  get; set; }
        public int MaHD { get; set; }
        public int MaHH {  get; set; }
        public string TenHH {  get; set; }
        public double DonGia    { get; set; }
        public int SoLuong { get; set; }
        public double GiamGia { get; set; }
        public string Anh { get; set; }
        public string ThanhTien
        {
            get
            {
                return (DonGia * SoLuong).ToString("#,##0");
            }
        }

    }
}

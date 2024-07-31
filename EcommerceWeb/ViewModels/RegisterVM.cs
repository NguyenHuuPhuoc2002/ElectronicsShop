using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string MaKh { get; set; }


        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; } = true;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày Sinh")]
        public DateTime? NgaySinh { get; set; }

        [Display(Name ="Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 ký tự")]
        public string DiaChi { get; set; }


        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"0[0975]\d{8}", ErrorMessage = "Chưa đúng định dạng di động Việt Nam")]
        public string DienThoai { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng")]
        public string Email { get; set; } = null!;

        public string? Hinh { get; set; }
    }
}

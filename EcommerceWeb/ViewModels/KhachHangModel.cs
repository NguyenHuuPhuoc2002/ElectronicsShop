using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.ViewModels
{
    public class KhachHangModel
    {
        [Display(Name = "Tên đăng nhập")]
        public string MaKh { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        public string? MatKhau { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        public string HoTen { get; set; }

        [Display(Name = "Nam?")]
        [Required(ErrorMessage = "*")]
        public bool GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "*")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? DienThoai { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        public string Hinh { get; set; }

		[Display(Name = "Ảnh")]
		public IFormFile? ImageUpload { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class NhaCungCapAdminModel
    {
        [Display(Name = "Mã nhà cung cấp")]
        [Required(ErrorMessage = "*")]
        public string MaNcc { get; set; }
        [Display(Name = "Tên công ty")]
        [Required(ErrorMessage = "*")]
        public string TenCongTy { get; set; }
        [Display(Name = "Tên người liên lạc")]
        [Required(ErrorMessage = "*")]
        public string NguoiLienLac { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "*")]
        public string DienThoai { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        public string DiaChi { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "*")]
        public string MoTa { get; set; }

    }
}

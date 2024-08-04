using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class LoginAdminModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        public string MatKhau { get; set; }
    }
}

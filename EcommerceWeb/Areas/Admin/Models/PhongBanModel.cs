using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class PhongBanModel
    {
        [Display(Name = "Mã phòng ban")]
        public string MaPb { get; set; }
        [Display(Name = "Tên phòng ban")]
        [Required(ErrorMessage = "*")]
        public string TenPb { get; set; }
        [Display(Name = "Thông tin phòng ban")]
        public string ThongTin { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class PhongBanModel
    {
        public string MaPb { get; set; }
        [Display(Name = "Tên phòng ban")]
        [Required(ErrorMessage = "*")]
        public string TenPb { get; set; }

        public string ThongTin { get; set; }

    }
}

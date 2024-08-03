using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class LoaiAdminModel
    {
        public int MaLoai { get; set; }
        [Display(Name = "Tên loại")]
        [Required(ErrorMessage = "*")]
        public string TenLoai { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "*")]
        public string MoTa { get; set; }
    }
}

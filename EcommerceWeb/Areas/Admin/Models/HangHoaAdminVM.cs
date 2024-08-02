using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class HangHoaAdminVM
    {

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "*")]
        public string TenHh { get; set; }
        [Display(Name = "Mã loại")]
        public int MaLoai { get; set; }
        [Display(Name = "Mô tả ngắn")]
        [Required(ErrorMessage = "*")]
        public string MoTaDonVi { get; set; }
        [Display(Name = "Giá")]
        [Required(ErrorMessage = "*")]
        public double DonGia { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Hinh { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime NgaySx { get; set; }

        [Display(Name = "Giảm giá")]
        [Required(ErrorMessage = "*")]
        public double GiamGia { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        [Required(ErrorMessage = "*")]
        public string MoTa { get; set; }
        [Display(Name = "Mã nhà cung cấp")]
        public string MaNcc { get; set; }
    }
}

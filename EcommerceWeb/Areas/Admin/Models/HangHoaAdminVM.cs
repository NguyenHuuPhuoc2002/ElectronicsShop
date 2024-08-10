using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ShoppingCart.Models.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class HangHoaAdminVM
    {
        public int MaHh { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "*")]
        public string TenHh { get; set; }
        [Display(Name = "Mã loại")]
        public int MaLoai { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string MoTaDonVi { get; set; }
        [Display(Name = "Giá")]
        public double DonGia { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Hinh { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime NgaySx { get; set; }

        [Display(Name = "Giảm giá")]
        [Required(ErrorMessage = "*")]
        public double GiamGia { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        public string MoTa { get; set; }
        [Display(Name = "Mã nhà cung cấp")]
        public string MaNcc { get; set; }
        [NotMapped]
        [FileExtention(ErrorMessage = "Allowed extensions are .jpg, .png, .jpeg")]

        [Display(Name = "Hình ảnh")]

        public IFormFile? ImageUpload { get; set; }
    }
}

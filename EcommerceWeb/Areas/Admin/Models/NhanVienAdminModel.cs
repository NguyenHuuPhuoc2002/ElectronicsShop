using EcommerceWeb.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Areas.Admin.Models
{
    public class NhanVienAdminModel
    {
        [Display (Name = "Mã nhân viên")]
        [Required(ErrorMessage = "*")]
        public string MaNv {  get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        public string HoTen { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        public string Email {  get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        public string MatKhau {  get; set; }
        public string MaPb {  get; set; }
       
    }
}

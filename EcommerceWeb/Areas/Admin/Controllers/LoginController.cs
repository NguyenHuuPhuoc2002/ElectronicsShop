using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class LoginController : Controller
    {
        private readonly ILoginRepository<NhanVien> _nhanVien;

        public LoginController(ILoginRepository<NhanVien> nhanVien)
        {
            _nhanVien = nhanVien;
        }
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        #region LogIn
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginAdminModel model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var nhanVien = await _nhanVien.GetNhanVienAsync(model);
                if (nhanVien == null)
                {
                    ModelState.AddModelError("loi", "Tài khoản Không tồn tại !");
                }
                else
                {
                    if (nhanVien.MatKhau != model.MatKhau)
                    {
                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                    }
                    else
                    {
                        var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, nhanVien.Email),
                                new Claim(ClaimTypes.Name, nhanVien.HoTen),
                                new Claim(MySetting.CLAIM_EMPLOYEE_ID, nhanVien.MaNv),

								//claim - role động
								new Claim(ClaimTypes.Role, MySetting.ROLE_ADMIN)
                            };
                        /* Tạo một danh tính cho người dùng dựa trên danh sách các claim và scheme xác thực cookie. */
                        var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");
                        /*  Tạo một đối tượng đại diện cho người dùng hiện tại bằng cách sử dụng danh
                              tính (ClaimsIdentity) đã tạo.  */
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("AdminScheme", claimsPrincipal);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Product");
                        }
                    }
                }
            }
            return View();
        }
        #endregion
        #region LogOut
  
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync("AdminScheme");
            return RedirectToAction("DangNhap");
        }
        #endregion
    }
}

using AutoMapper;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace EcommerceWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangRepository<KhachHang> _context;
        private readonly IMapper _mapper;

        public KhachHangController(IKhachHangRepository<KhachHang> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Register
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    //ramdomKey sinh mã ngẫu nhiên
                    khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;// xử lí khi dùng mail để active
                    khachHang.VaiTro = 0;

                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "khachHang");
                    }
                    _context.Register(khachHang);

                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        #endregion

        #region LogIn
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LogInVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                // Lấy thông tin khách hàng từ cơ sở dữ liệu
                var khachHang = await _context.GetKhachHang(model);

                if (khachHang == null)
                {
                    // Thông báo lỗi nếu không tìm thấy khách hàng
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else if (!khachHang.HieuLuc)
                {
                    // Thông báo lỗi nếu tài khoản bị khóa
                    ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                }
                else if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                {
                    // Thông báo lỗi nếu mật khẩu không chính xác
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                }
                else
                {
                    // Tạo danh sách các claim cho khách hàng
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, khachHang.Email),
                        new Claim(ClaimTypes.Name, khachHang.HoTen),
                        new Claim(ClaimTypes.StreetAddress, khachHang.DiaChi),
                        new Claim(MySetting.CLAIM_CUSTOMER_ID, khachHang.MaKh),
                        new Claim(ClaimTypes.Role, MySetting.ROLE_CUSTOMER) // Role động
                    };

                    // Tạo một danh tính cho người dùng dựa trên danh sách các claim và scheme xác thực cookie
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Tạo một đối tượng đại diện cho người dùng hiện tại bằng cách sử dụng danh tính (ClaimsIdentity) đã tạo
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Đăng nhập người dùng
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Chuyển hướng đến URL gốc hoặc URL được chỉ định
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }
            }

            // Trả về View nếu có lỗi hoặc không hợp lệ
            return View();
        }

        #endregion

        #region Profile
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
        #endregion

        #region LogOut
        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion
    }
}

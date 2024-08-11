using AutoMapper;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace EcommerceWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly IKhachHangRepository<KhachHang> _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KhachHangController(IKhachHangRepository<KhachHang> context, IMapper mapper
                                    , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Profile()
        {
            var claimMaKh = User.FindFirst(MySetting.CLAIM_CUSTOMER_ID)?.Value;
            var khachHang = await _context.GetKhachHangByIdAsync(claimMaKh);
            var result = _mapper.Map<KhachHangModel>(khachHang);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KhachHangModel model)
        {
            var existed_khachHang = await _context.GetKhachHangByIdAsync(model.MaKh);
            if (ModelState.IsValid)
            {
                if (model.ImageUpload != null)
                {
                    //upload new image
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "Hinh/KhachHang");
                    string imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    if (!string.IsNullOrEmpty(existed_khachHang.Hinh))
                    {
                        //delete old anh
                        string oldfilePath = Path.Combine(uploadsDir, existed_khachHang.Hinh);
                        try
                        {
                            if (System.IO.File.Exists(oldfilePath))
                            {
                                System.IO.File.Delete(oldfilePath);
                            }
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Loi Delete");
                        }
                    }
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_khachHang.Hinh = imageName;

                }
                existed_khachHang.MaKh = model.MaKh;
                existed_khachHang.MatKhau = model.MatKhau;
                existed_khachHang.NgaySinh = model.NgaySinh;
                existed_khachHang.DiaChi = model.DiaChi;
                existed_khachHang.DienThoai = model.DienThoai;
                existed_khachHang.Email = model.Email;
                existed_khachHang.HieuLuc = true;
                existed_khachHang.GioiTinh = model.GioiTinh;
                existed_khachHang.HoTen = model.HoTen;
                existed_khachHang.VaiTro = 0;
                await _context.UpdateAsync(existed_khachHang);
                TempData["MessageUpdateInfo"] = "Chỉnh sửa thành công !";
                return RedirectToAction("Index", "HangHoa");
            }
            return RedirectToAction("Profile", model);
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

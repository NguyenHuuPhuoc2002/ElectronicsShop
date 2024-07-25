using AutoMapper;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public KhachHangController(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
                    _context.Add(khachHang);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                } catch (Exception ex)
                {

                }
            }
            return View();
        }
    }
}

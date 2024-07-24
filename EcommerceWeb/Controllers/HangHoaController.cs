using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly HshopContext _context;

        public HangHoaController(HshopContext context) 
        {
            _context = context;
        }
        
        public IActionResult Index(int? loai)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (loai.HasValue) { 
                hangHoas = _context.HangHoas.Where(p => p.MaLoai == loai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai    
            });
            return View(result);
        }

     
        public IActionResult Search(string? query)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = _context.HangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
    }
}

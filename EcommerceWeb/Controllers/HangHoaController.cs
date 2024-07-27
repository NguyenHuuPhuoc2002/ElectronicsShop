using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly HshopContext _context;

        public HangHoaController(HshopContext context) 
        {
            _context = context;
        }
        
        //Hiển thị toàn bộ sản phẩm
        //xử lí cả sidebar tìm sản phẩm theo loại (nếu có)
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

        public IActionResult Detail(int? id)
        {
            var data = _context.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);
           
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id} !";
                return Redirect("/404");
            }

            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                Hinh = data.Hinh ?? "",
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                DiemDanhGia = 5,
                SoLuongTon = 10
            };
            return View(result);
        }
    }
}

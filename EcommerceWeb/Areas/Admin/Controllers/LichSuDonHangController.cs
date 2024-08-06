using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class LichSuDonHangController : Controller
    {
        private readonly IDonHangRepository<HoaDonVM> _hoaDon;
        private readonly IChiTietHoaDonRepository<ChiTietHoaDonVM> _chiTiet;

        public LichSuDonHangController(IDonHangRepository<HoaDonVM> donHang, IChiTietHoaDonRepository<ChiTietHoaDonVM> chiTiet)
        {
            _hoaDon = donHang;
            _chiTiet = chiTiet;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var _donHangs = await _hoaDon.GetAllAsync(_page, _pageSize);
            return View(_donHangs);
        }
        [Authorize]
        public async Task<IActionResult> Search(string currentFilter, string keyword, int page, int? pageSize)
        {
            IEnumerable<HoaDonVM> hoaDons;
            int pSize = pageSize ?? 10;

            if (!string.IsNullOrEmpty(keyword))
            {
                page = 1;
            }
            else
            {
                keyword = currentFilter;
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                hoaDons = await _hoaDon.GetSearchAsync(keyword, page, pSize);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentFilter = keyword;
            return View(hoaDons);
        }

        [Authorize]
        public async Task<IActionResult> ChiTietDonHang(int id)
        {
            var cTHoaDons = await _chiTiet.GetOderDetailByIdAsync(id);
            var cTHoaDonsVM = new ChiTietHoaDonViewModel
            {
                chiTietHangHoaVMs = cTHoaDons,
                TongTien = (cTHoaDons.Sum(p => Convert.ToDouble(p.ThanhTien)) + MySetting.SHIPPING_FEE),

            };
            ViewBag.MaHD = id;
            return View(cTHoaDonsVM);
        }
    }
}

using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class OdersController : Controller
    {
        private readonly IHoaDonRepository<HoaDonVM> _hoaDon;
        private readonly IChiTietHoaDonRepository<ChiTietHoaDonVM> _chiTietHoaDon;

        public OdersController(IHoaDonRepository<HoaDonVM> hoaDon, IChiTietHoaDonRepository<ChiTietHoaDonVM> chiTietHoaDon) 
        {
            _hoaDon = hoaDon;
            _chiTietHoaDon = chiTietHoaDon;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int size = pageSize ?? 3;

            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;
            var hoaDons = await _hoaDon.GetAllByIdAsync(customerId, pageNumber, size);

            return View(hoaDons);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var cTHoaDons = await _chiTietHoaDon.GetOderDetailByIdAsync(id);
            var cTHoaDonsVM = new ChiTietHoaDonViewModel
            {
                chiTietHangHoaVMs = cTHoaDons,
                TongTien = (cTHoaDons.Sum(p => Convert.ToDouble(p.ThanhTien)) + MySetting.SHIPPING_FEE),

            };
            var hoaDon = await _hoaDon.GetOderByIdAsync(id);
            var state = hoaDon.MaTrangThai;

            if (state != -1)
            {
                TempData["State"] = state;
            }
            else
            {
                TempData["State"] = null;
            }
            ViewBag.MaHD = id;
            return View(cTHoaDonsVM);
        }

        public async Task<IActionResult> CancelOder(int id)
        {
            await _hoaDon.UpdateStateAsync(id);
            
            return RedirectToAction("Index");
        }
    }
}

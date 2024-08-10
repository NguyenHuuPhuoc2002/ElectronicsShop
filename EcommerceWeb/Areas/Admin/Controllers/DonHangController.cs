using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class DonHangController : Controller
    {
        private readonly IDonHangRepository<HoaDonVM> _donHang;
        private readonly IChiTietHoaDonRepository<ChiTietHoaDonVM> _chiTiet;

        public DonHangController(IDonHangRepository<HoaDonVM> donHang, IChiTietHoaDonRepository<ChiTietHoaDonVM> chiTiet)
        {
            _donHang = donHang;
            _chiTiet = chiTiet;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;

            var oders = await _donHang.GetOderConfirmAsync(_page, _pageSize);
            return View(oders);
        }
        [Authorize]
        [Authorize(Policy = "BusinessOrDirectors")]
        public async Task<IActionResult> XacNhan(int id)
        {
            int state = 3;
            if(id == null)
            {
                return Redirect("/404");
            }
            await _donHang.UpdateStateAsync(id, state);
            return RedirectToAction("Index");
        }
        [Authorize]
        [Authorize(Policy = "BusinessOrDirectors")]
        public async Task<IActionResult> HuyDonHang(int id)
        {
            int state = -1;
            if (id == null)
            {
                return Redirect("/404");
            }
            await _donHang.UpdateStateAsync(id, state);
            TempData["Message"] = "Hủy đơn thành công !";
            return RedirectToAction("Index");
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
                hoaDons = await _donHang.GetSearchAsync(keyword, page, pSize);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentFilter = keyword;
            return View(hoaDons);
        }
    }
}

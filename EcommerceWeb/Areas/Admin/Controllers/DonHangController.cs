using EcommerceWeb.Areas.Admin.Repositories;
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

        public DonHangController(IDonHangRepository<HoaDonVM> donHang)
        {
            _donHang = donHang;
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
        public async Task<IActionResult> LichSu(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var _donHangs = await _donHang.GetAllAsync(_page, _pageSize);
            return View(_donHangs);
        }
        [Authorize]
        public async Task<IActionResult> HuyDonHang(int id)
        {
            int state = -1;
            if (id == null)
            {
                return Redirect("/404");
            }
            await _donHang.UpdateStateAsync(id, state);
            return RedirectToAction("Index");
        }
    }
}

using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IHangHoaRepository<HangHoaVM> _hangHoa;

        public ProductController(IHangHoaRepository<HangHoaVM> hangHoa)
        {
            _hangHoa = hangHoa;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? loai, int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var hangHoas = await _hangHoa.GetAllOrById(loai, _page, _pageSize);
            return View(hangHoas);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0) {
                return Redirect("/404");
            }
            else
            {
                await _hangHoa.DeleteAsync(id);
                TempData["Message"] = "Xóa thành công !";
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        public async Task<IActionResult> Search(string currentFilter, string keyword, int page, int? pageSize)
        {
            IEnumerable<HangHoaVM> hangHoas;
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
                hangHoas = await _hangHoa.GetSearch(keyword, page, pSize);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentFilter = keyword;
            return View(hangHoas);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(HangHoaAdminVM hangHoa)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}

using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly IHangHoaRepository<HangHoaVM> _context;

        public HangHoaController(IHangHoaRepository<HangHoaVM> context) 
        {
            _context = context;
        }
        public async Task<IActionResult> Search(string currentFilter, string? query, int page, int? pageSize)
        {
            IEnumerable<HangHoaVM> hangHoas;
            int pSize = pageSize ?? 9;

            if (!string.IsNullOrEmpty(query))
            {
                page = 1;
            }
            else
            {
                query = currentFilter;
            }
            if (!string.IsNullOrEmpty(query))
            {
                hangHoas = await _context.GetSearch(query, page, pSize);
            }
            else
            {
                return RedirectToAction("Index", "HangHoa");
            }
            ViewBag.CurrentFilter = query;
            return View(hangHoas);
        }
        public async Task<IActionResult> Index(int? loai, int? page, int? pageSize)
        {
            HttpContext.Session.Remove("Query");
            int pageNumber = page ?? 1;
            int size = pageSize ?? 9;
            if (loai.HasValue)
            {
                HttpContext.Session.SetJson("loai", loai.Value);
            }
            else
            {
                loai = HttpContext.Session.GetJson<int?>("loai");
            }
            var hangHoas = await _context.GetAllOrById(loai, pageNumber, size);
            return View(hangHoas);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var hangHoas = await _context.GetDetail(id);
            return View(hangHoas);
        }
    }
}

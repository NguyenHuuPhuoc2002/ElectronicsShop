using EcommerceWeb.Data;
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
        
        public async Task<IActionResult> Index(int? loai)
        {
            var hangHoas = await _context.GetAllOrById(loai);
            
            return View(hangHoas);
        }

     
        public async Task<IActionResult> Search(string? query)
        {
            var hangHoas = await _context.GetSearch(query);
            return View(hangHoas);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var hangHoas = await _context.GetDetail(id);
            return View(hangHoas);
        }
    }
}

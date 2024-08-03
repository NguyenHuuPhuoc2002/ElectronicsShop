using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhaCungCapController : Controller
    {
        private readonly INhaCungCapRepository<NhaCungCapAdminModel> _nhaCungCap;

        public NhaCungCapController(INhaCungCapRepository<NhaCungCapAdminModel> nhaCungCap)
        {
            _nhaCungCap = nhaCungCap;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var data = await _nhaCungCap.GetAllAsync(_page, _pageSize);
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhaCungCapAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var data = await _nhaCungCap.GetByIdAsync(model.MaNcc);
                if(data != null)
                {
                    ViewBag.Message = $"Đã tồn tại nhà cung cấp {model.TenCongTy} !";
                    return View();
                }
                else
                {
                    await _nhaCungCap.AddAsync(model);
                    TempData["Message"] = $"Thêm nhà cung cấp {model.TenCongTy} thành công !";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}

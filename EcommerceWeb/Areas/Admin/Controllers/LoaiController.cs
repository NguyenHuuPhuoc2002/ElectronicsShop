using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiController : Controller
    {
        private readonly ILoaiRepository<LoaiAdminModel> _loai;

        public LoaiController(ILoaiRepository<LoaiAdminModel> loai)
        {
            _loai = loai;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var _page = page ?? 1;
            var _pageSize = pageSize ?? 10;
            var loais = await _loai.GetAllAsync(_page, _pageSize);
            return View(loais);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LoaiAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var loai = await _loai.GetByNameAsync(model.TenLoai);
                if(loai != null)
                {
                    ViewBag.Message = $"Đã tồn tại loại \"{model.TenLoai}\" !";
                    return View();
                }
                else
                {
                    await _loai.AddAsync(model);
                    TempData["Message"] = $"Thêm loại \"{model.TenLoai}\" thành công !";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null)
            {
                return Redirect("/404");
            }
            await _loai.DeleteAsync(id);
            TempData["Message"] = "Xóa loại thành công !";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var loai = await _loai.GetByIdAsync(id);
            if(loai == null)
            {
                return Redirect("/404");
            }
            else
            {
                return View(loai);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LoaiAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var exist_loai = await _loai.GetByIdAsync(id);
                if (exist_loai == null) 
                {
                    return Redirect("/404");
                }
                else
                {
                    exist_loai.TenLoai = model.TenLoai;
                    exist_loai.MoTa = model.MoTa;
                    await _loai.UpdateAsync(id, exist_loai);
                }
                TempData["Message"] = $"Chỉnh sửa loại \"{model.TenLoai}\" thành công !";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

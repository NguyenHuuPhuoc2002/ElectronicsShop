using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class PhongBanController : Controller
    {
        private readonly IPhongBanRepository<PhongBanModel> _phongBan;

        public PhongBanController(IPhongBanRepository<PhongBanModel> phongBan)
        {
            _phongBan = phongBan;
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public async Task<IActionResult> Index()
        {
            var phongBans = await _phongBan.GetAllAsync();
            return View(phongBans);
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhongBanModel model)
        {
            if (ModelState.IsValid)
            {
                var loai = await _phongBan.GetByIdAsync(model.MaPb);
                if (loai != null)
                {
                    ViewBag.Message = $"Đã tồn tại phòng ban này !";
                    return View(model);
                }
                else
                {
                    await _phongBan.AddAsync(model);
                    TempData["Message"] = "Thêm phòng ban thành công !";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _phongBan.DeleteAsync(id);
                TempData["Message"] = "Xóa phòng ban thành công !";
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var phongBan = await _phongBan.GetByIdAsync(id);
                return View(phongBan);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PhongBanModel phongBan)
        {
            var tenPhongBan = await _phongBan.GetByNameAsync(phongBan.TenPb);
            if (tenPhongBan != null)
            {
                ViewBag.Message = "Đã tồn tại phòng ban này !";
                return View(phongBan);
            }
            else
            {
                await _phongBan.UpdateAsync(id, phongBan);
                TempData["Message"] = "Cập nhật phòng ban thành công !";
                return RedirectToAction("Index");
            }
        }
    }
}

﻿using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class LoaiController : Controller
    {
        private readonly ILoaiRepository<LoaiAdminModel> _loai;
        private readonly HshopContext _context;

        public LoaiController(ILoaiRepository<LoaiAdminModel> loai, HshopContext context)
        {
            _loai = loai;
            _context = context;
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
        [Authorize(Policy = "BusinessOrDirectors")]
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
                if (loai != null)
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
            var role = User.FindFirst(MySetting.DEPARTMENT)?.Value;
            if (role == MySetting.ROLE_BUSINESS || role == MySetting.ROLE_DIRECTORS)
            {
                if (id == null)
                {
                    return Redirect("/404");
                }
                await _loai.DeleteAsync(id);
                TempData["Message"] = "Xóa loại thành công !";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Bạn không có quyền thực hiện hành động này !";
            return RedirectToAction("Index", "Product");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var role = User.FindFirst(MySetting.DEPARTMENT)?.Value;
            if (role == MySetting.ROLE_BUSINESS || role == MySetting.ROLE_DIRECTORS)
            {
                var loai = await _loai.GetByIdAsync(id);
                if (loai == null)
                {
                    return Redirect("/404");
                }
                else
                {
                    return View(loai);
                }
            }
            TempData["Error"] = "Bạn không có quyền thực hiện hành động này !";
            return RedirectToAction("Index", "Product");
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

        [Authorize]
        public async Task<IActionResult> Search(string currentFilter, string keyword, int page, int? pageSize)
        {
            IEnumerable<LoaiAdminModel> loais;
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
                loais = await _loai.GetSearch(keyword, page, pSize);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentFilter = keyword;
            return View(loais);
        }
    }
}

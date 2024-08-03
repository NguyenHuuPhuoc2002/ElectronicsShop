﻿using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IHangHoaRepository<HangHoaVM> _hangHoa;
        private readonly HshopContext _context;
        private readonly IHangHoaAdminRepository<HangHoaAdminVM> _admin;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IHangHoaRepository<HangHoaVM> hangHoa, HshopContext context, 
                IWebHostEnvironment webHostEnvironment, IHangHoaAdminRepository<HangHoaAdminVM> admin)
        {
            _hangHoa = hangHoa;
            _context = context;
            _admin = admin;
            _webHostEnvironment = webHostEnvironment;
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
            var hangHoa = await _admin.GetById(id);
            if (id == 0)
            {
                return Redirect("/404");
            }
            else
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "Hinh/HangHoa");
                string oldfilePath = Path.Combine(uploadsDir, hangHoa.Hinh);
                try
                {
                    if (System.IO.File.Exists(oldfilePath))
                    {
                        System.IO.File.Delete(oldfilePath);
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Xóa sản phẩm không thành công !");
                }
                await _admin.DeleteAsync(id);
                TempData["Message"] = "Xóa sản phẩm thành công !";
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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy");
            ViewBag.Loais = new SelectList(_context.Loais, "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(HangHoaAdminVM hangHoa)
        {
            ViewBag.Categories = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy", hangHoa.MaNcc);
            ViewBag.Brands = new SelectList(_context.Loais, "MaLoai", "TenLoai", hangHoa.MaLoai);

            if (ModelState.IsValid)
            {
                var product = await _admin.GetByName(hangHoa.TenHh);
                if (product != null)
                {
                    ViewBag.Message = "Đã tồn tại sản phẩm này !";
                    ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps, "MaNcc", "TenCongTy");
                    ViewBag.Loais = new SelectList(_context.Loais, "MaLoai", "TenLoai");
                    return View();
                }
                if (hangHoa.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Hinh/HangHoa");
                    string imageName = Guid.NewGuid().ToString() + "_" + hangHoa.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await hangHoa.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    hangHoa.Hinh = imageName;
                }
                await _admin.AddAsync(hangHoa);
                TempData["Message"] = "Thêm sản phẩm thành công !";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang bị lỗi";
                var errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                var errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);

            }
        }
    }
}

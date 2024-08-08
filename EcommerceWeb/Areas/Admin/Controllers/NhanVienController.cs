using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class NhanVienController : Controller
    {
        private readonly INhanVienRepository<NhanVienAdminModel> _nhanVien;
        private readonly HshopContext _context;

        public NhanVienController(INhanVienRepository<NhanVienAdminModel> nhanVien, HshopContext context)
        {
            _nhanVien = nhanVien;
            _context = context;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var _email = User.FindFirst(ClaimTypes.Email)?.Value;
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var data = await _nhanVien.GetAllAsync(_email, _page, _pageSize);
            return View(data);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Role = new SelectList(_context.PhongBans, "MaPb", "TenPb");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVienAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var nv = await _nhanVien.GetByIdAsync(model.MaNv);
                var nvEmail = await _nhanVien.GetByEmailAsync(model.Email);
                if(nv != null)
                {
                    ViewBag.Message = $"Đã tồn tại mã nhân viên \"{model.MaNv}\" !";
                    return View(model);
                }
                if(nvEmail != null)
                {
                    ViewBag.Message = $"Đã tồn tại email \"{model.Email}\" !";
                    return View(model);
                }
                else
                {
                    await _nhanVien.AddAsync(model);
                    TempData["Message"] = $"Thêm nhân viên \"{model.HoTen}\" thành công !";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _nhanVien.DeleteAsync(id);
            TempData["Message"] = $"Xóa nhân viên có mã \"{id}\" thành công !";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = await _nhanVien.GetByIdAsync(id);
                return View(nhanVien);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, NhanVienAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = await _nhanVien.GetByIdAsync(id);
                
                if (nhanVien == null)
                {
                    return Redirect("/404");
                }
                else
                {
                    nhanVien.MaNv = id;
                    nhanVien.HoTen = model.HoTen;
                    nhanVien.Email = model.Email;
                    nhanVien.MatKhau = model.MatKhau;
                };
                await _nhanVien.UpdateAsync(id, nhanVien);
                TempData["Message"] = "Chỉnh sửa thành công !";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Search(string currentFilter, string keyword, int page, int? pageSize)
        {
            IEnumerable<NhanVienAdminModel> loais;
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
                loais = await _nhanVien.GetSearch(keyword, page, pSize);
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

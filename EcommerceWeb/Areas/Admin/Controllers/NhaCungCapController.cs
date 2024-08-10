using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class NhaCungCapController : Controller
    {
        private readonly INhaCungCapRepository<NhaCungCapAdminModel> _nhaCungCap;

        public NhaCungCapController(INhaCungCapRepository<NhaCungCapAdminModel> nhaCungCap)
        {
            _nhaCungCap = nhaCungCap;
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var data = await _nhaCungCap.GetAllAsync(_page, _pageSize);
            return View(data);
        }

        [Authorize]
        [Authorize(Policy = "BusinessOrDirectors")]
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
                    ViewBag.Message = $"Đã tồn tại nhà cung cấp \"{model.TenCongTy}\" !";
                    return View();
                }
                else
                {
                    await _nhaCungCap.AddAsync(model);
                    TempData["Message"] = $"Thêm nhà cung cấp \"{model.TenCongTy}\" thành công !";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize]
        [Authorize(Policy = "BusinessOrDirectors")]
        public async Task<IActionResult> Edit(string id)
        {
            var nhaCungCap = await _nhaCungCap.GetByIdAsync(id);
            if (nhaCungCap == null)
            {
                return Redirect("/404");
            }
            return View(nhaCungCap);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, NhaCungCapAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var nhaCungCap = await _nhaCungCap.GetByIdAsync(id);
                if(nhaCungCap == null)
                {
                    return Redirect("/404");
                }
                else
                {
                    nhaCungCap.MaNcc = model.MaNcc;
                    nhaCungCap.TenCongTy = model.TenCongTy;
                    nhaCungCap.NguoiLienLac = model.NguoiLienLac;
                    nhaCungCap.DienThoai = model.DienThoai;
                    nhaCungCap.DiaChi = model.DiaChi;
                    nhaCungCap.MoTa = model.MoTa;
                    nhaCungCap.Email = model.Email;

                    await _nhaCungCap.UpdateAsync(id, nhaCungCap);
                    TempData["Message"] = $"Chỉnh sửa nhà cung cấp có mã \"{model.MaNcc}\" thành công !";
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        [Authorize(Policy = "BusinessOrDirectors")]
        public async Task<IActionResult> Delete(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return Redirect("/404");
            }
            await _nhaCungCap.DeleteAsync(id);
            TempData["Message"] = $"Xóa nhà cung cấp có mã \"{id}\" thành công !";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Search(string currentFilter, string keyword, int page, int? pageSize)
        {
            IEnumerable<NhaCungCapAdminModel> nhaCungCaps;
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
                nhaCungCaps = await _nhaCungCap.GetSearch(keyword, page, pSize);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentFilter = keyword;
            return View(nhaCungCaps);
        }
    }
}

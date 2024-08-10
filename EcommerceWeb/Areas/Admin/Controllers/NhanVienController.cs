using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Areas.Admin.ViewModels;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
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
        private readonly IPhongBanRepository<PhongBanModel> _phongBan;
        private readonly IPhanCongRepository<PhanCongModel> _phanCong;

        public NhanVienController(INhanVienRepository<NhanVienAdminModel> nhanVien,
                                IPhongBanRepository<PhongBanModel> phongBan, IPhanCongRepository<PhanCongModel> phanCong)
        {
            _nhanVien = nhanVien;
            _phongBan = phongBan;
            _phanCong = phanCong;
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var data = await _nhanVien.GetAllAsync(_page, _pageSize);
            return View(data);
        }

        [Authorize]
        [Authorize(Policy = "Directors")]
        public async Task<IActionResult> Create()
        {

            ViewBag.MaPhongBan = new SelectList(await _phongBan.GetAllAsync(), "MaPb", "TenPb");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVienAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var nv = await _nhanVien.GetByIdAsync(model.MaNv);
                var nvEmail = await _nhanVien.GetByEmailAsync(model.Email);
                if (nv != null)
                {
                    ViewBag.Message = $"Đã tồn tại mã nhân viên \"{model.MaNv}\" !";
                    return View(model);
                }
                if (nvEmail != null)
                {
                    ViewBag.Message = $"Đã tồn tại email \"{model.Email}\" !";
                    return View(model);
                }
                else
                {
                    try
                    {
                        await _nhanVien.AddAsync(model);
                        var findNhanVien = await _nhanVien.GetByEmailAsync(model.Email);
                        var maNv = findNhanVien.MaNv;
                        var phongBan = await _phongBan.GetById(model.MaPb);
                        var maPb = phongBan.MaPb;
                        var result = new PhanCongModel
                        {
                            MaNv = maNv,
                            MaPb = maPb,
                            NgayPc = DateTime.Now,
                            HieuLuc = true
                        };
                        await _phanCong.AddAsync(result);

                        TempData["Message"] = $"Thêm nhân viên \"{model.HoTen}\" thành công!";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Ghi log hoặc hiển thị thông báo lỗi cho người dùng
                        Console.WriteLine(ex.Message);
                        TempData["Error"] = "Đã xảy ra lỗi khi thêm dữ liệu.";
                        return RedirectToAction("Index");
                    }

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
            ViewBag.MaPhongBan = new SelectList(await _phongBan.GetAllAsync(), "MaPb", "TenPb");
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
                    nhanVien.MaPb = model.MaPb;
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

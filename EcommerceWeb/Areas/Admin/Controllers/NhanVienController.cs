using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Areas.Admin.Repositories;
using EcommerceWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class NhanVienController : Controller
    {
        private readonly INhanVienRepository<NhanVienAdminModel> _nhanVien;

        public NhanVienController(INhanVienRepository<NhanVienAdminModel> nhanVien)
        {
            _nhanVien = nhanVien;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var _email = User.FindFirst(ClaimTypes.Email)?.Value;
            int _page = page ?? 1;
            int _pageSize = pageSize ?? 10;
            var data = await _nhanVien.GetAllAsync(_email, _page, _pageSize);
            return View(data);
        }
    }
}

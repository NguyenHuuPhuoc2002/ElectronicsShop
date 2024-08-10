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
    }
}

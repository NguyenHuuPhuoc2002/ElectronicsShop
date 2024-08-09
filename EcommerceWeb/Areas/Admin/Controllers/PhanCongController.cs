using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    public class PhanCongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

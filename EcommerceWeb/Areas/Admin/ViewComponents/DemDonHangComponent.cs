using EcommerceWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.ViewComponents
{
    public class DemDonHangViewComponent : ViewComponent
    {
        private readonly HshopContext _context;

        public DemDonHangViewComponent(HshopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = await _context.HoaDons.CountAsync(p => p.MaTrangThai == 0);
            return View(count);
        }
    }
}

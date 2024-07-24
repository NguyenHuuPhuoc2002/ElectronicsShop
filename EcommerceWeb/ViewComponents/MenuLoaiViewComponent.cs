using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly HshopContext _context;

        public MenuLoaiViewComponent(HshopContext context) 
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = _context.Loais.Select( lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count(),
            });

            return View(data); //Defaut.cshtml
        }
    }
}

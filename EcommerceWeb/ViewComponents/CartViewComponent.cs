using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var carts = HttpContext.Session.GetJson<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

            return View("CartPanel", new CartModel
            {
                Quantity = carts.Sum(p => p.SoLuong),
                Total = carts.Sum(p => p.ThanhTien)
            });
        }
    }
}

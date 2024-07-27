using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository<HangHoa> _context;

        public CartController(ICartRepository<HangHoa> context) {
            _context = context;
        }

        public List<CartItem> Cart => HttpContext.Session.GetJson<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = await _context.GetHangHoa(id); // Sử dụng await ở đây
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                cart.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.SetJson(MySetting.CART_KEY, cart);
            return RedirectToAction("Index");
        }


        public IActionResult RemoveCart(int id)
        {
            var carts = Cart;
            var item = carts.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                carts.Remove(item);
            }
            else
            {
                Redirect("/404");
            }

            HttpContext.Session.SetJson(MySetting.CART_KEY, carts);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var carts = Cart;
            var item = carts.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                if(quantity < 1)
                {
                    carts.Remove(item);
                }
                else
                {
                    item.SoLuong = quantity;
                }
                
            }
            HttpContext.Session.SetJson(MySetting.CART_KEY, carts);
            return RedirectToAction("Index");
        } 
    }
}

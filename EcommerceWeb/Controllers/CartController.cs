using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository<HangHoa> _context;
        private readonly HshopContext _db;

        public CartController(ICartRepository<HangHoa> context, HshopContext db) {
            _context = context;
            _db = db;
        }

        public List<CartItem> Carts => HttpContext.Session.GetJson<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Carts);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var cart = Carts;
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
            var carts = Carts;
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
            var carts = Carts;
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

		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			if (Carts.Count == 0)
			{
				return Redirect("/");
			}

			return View(Carts);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Checkout(CheckoutVM model)
		{
			if (ModelState.IsValid)
			{
				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;
				var khachHang = new KhachHang();
				if (model.GiongKhachHang)
				{
					khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
				}

				var hoadon = new HoaDon
				{
					MaKh = customerId,
					HoTen = model.HoTen ?? khachHang.HoTen,
					DiaChi = model.DiaChi ?? khachHang.DiaChi,
					DienThoai = model.DienThoai ?? khachHang.DienThoai,
					NgayDat = DateTime.Now,
					CachThanhToan = "COD",
					CachVanChuyen = "GRAB",
					MaTrangThai = 0,
					GhiChu = model.GhiChu
				};

				_db.Database.BeginTransaction();
				try
				{
					_db.Database.CommitTransaction();
					_db.Add(hoadon);
					_db.SaveChanges();

					var cthds = new List<ChiTietHd>();
					foreach (var item in Carts)
					{
						cthds.Add(new ChiTietHd
						{
							MaHd = hoadon.MaHd,
							SoLuong = item.SoLuong,
							DonGia = item.DonGia,
							MaHh = item.MaHh,
							GiamGia = 0
						});
					}
					_db.AddRange(cthds);
					_db.SaveChanges();

					HttpContext.Session.SetJson(MySetting.CART_KEY, new List<CartItem>());

					return View("Success");
				}
				catch
				{
					_db.Database.RollbackTransaction();
				}
			}

			return View(Carts);
		}
	}
}

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
        private readonly ICartRepository _context;
		private readonly PaypalClient _paypalClient;

		public CartController(ICartRepository context, PaypalClient paypalClient) {
            _context = context;
			_paypalClient = paypalClient;
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

            // Pass the PayPal Client ID to the view
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Carts);
        }


        [Authorize]
		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutVM model)
		{
			if (ModelState.IsValid)
			{
				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;
				var khachHang = new KhachHang();
				if (model.GiongKhachHang)
				{
					khachHang = await _context.GetKhachHangByIdAsync(customerId);
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

				await _context.BeginTransaction();
				try
				{
					await _context.CommitTransaction();
					await _context.AddHoaDonAsync(hoadon);

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
					await _context.AddRangeChiTietHdAsync(cthds);
					
					HttpContext.Session.SetJson(MySetting.CART_KEY, new List<CartItem>());

					return View("Success");
				}
				catch
				{
					await _context.RollbackTransaction();
				}
			}

			return View(Carts);
		}

		[Authorize]
		public IActionResult PaymentSuccess()
		{
			return View("Success");
		}

		#region Paypal payment
		[Authorize]
		[HttpPost("/Cart/create-paypal-order")] //định nghĩa URL
		public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
		{
			// Thông tin đơn hàng gửi qua Paypal
			var tongTien = Carts.Sum(p => p.ThanhTien).ToString();
			var donViTienTe = "USD";
			var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		[Authorize]
		[HttpPost("/Cart/capture-paypal-order")]
		public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderID);

				// Lưu database đơn hàng của mình

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		#endregion
	}
}



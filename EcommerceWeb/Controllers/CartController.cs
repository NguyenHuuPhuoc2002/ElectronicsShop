using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.Services;
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
        private readonly IVnPayService _vnPayService;

        public CartController(ICartRepository context, PaypalClient paypalClient, IVnPayService vnPayService) {
            _context = context;
			_paypalClient = paypalClient;
            _vnPayService = vnPayService;
        }

        public List<CartItem> Carts => HttpContext.Session.GetJson<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        private static CheckoutVM _model { get; set; }
        public IActionResult Index()
        {
            return View(Carts);
        }

        #region Add - Remove -Update
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
        #endregion

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


        #region COD payment

        [Authorize]
		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutVM model, string payment = "COD")
		{
            
            if (ModelState.IsValid)
			{
                _model = model;
                //Thanh toan VnPay
                if (payment == "Thanh toán VNPay")
                {
                    var vnPayModel = new VnPaymentRequestMode
                    {
                        Amount = Carts.Sum(p => p.ThanhTien),
                        CreatedDate = DateTime.Now,
                        Description = $"{model.HoTen} {model.DienThoai}",
                        FullName = model.HoTen,
                        OrderId = new Random().Next(1000, 10000)
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                #region Tao don hang
                //Tao don hang
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
					NgayCan = null,
					NgayGiao = DateTime.Now.AddDays(3),
					CachThanhToan = MySetting.COD,
					CachVanChuyen = MySetting.SHIPPING_COD,
					PhiVanChuyen = MySetting.SHIPPING_FEE,
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
                #endregion
            }

            return View(Carts);
		}
        #endregion

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
		public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken, [FromBody] CheckoutVM model)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderID);

                // Lưu database đơn hàng của mình
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
                        NgayCan = null,
                        NgayGiao = DateTime.Now.AddDays(3),
                        CachThanhToan = MySetting.PAYPAL,
                        CachVanChuyen = MySetting.SHIPPING_PAYPAL,
                        PhiVanChuyen = MySetting.SHIPPING_FEE,
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

                    return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

        #endregion

        [Authorize]
        public  IActionResult PaymentFail()
        {
            return View();
        }

        #region Payment VNPay
        [Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {
            var respone = _vnPayService.PaymentExecute(Request.Query);
            if(respone == null || respone.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VnPay! :{respone.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            #region Tao don hang
            //Tao don hang
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;
            var khachHang = new KhachHang();
            if (_model.GiongKhachHang)
            {
                khachHang = await _context.GetKhachHangByIdAsync(customerId);
            }

            var hoadon = new HoaDon
            {
                MaKh = customerId,
                HoTen = _model.HoTen ?? khachHang.HoTen,
                DiaChi = _model.DiaChi ?? khachHang.DiaChi,
                DienThoai = _model.DienThoai ?? khachHang.DienThoai,
                NgayDat = DateTime.Now,
                NgayGiao = DateTime.Now.AddDays(3),
                CachThanhToan = MySetting.VNPAY,
                CachVanChuyen = MySetting.SHIPPING_VNPAY,
                PhiVanChuyen = MySetting.SHIPPING_FEE,
                MaTrangThai = 0,
                GhiChu = _model.GhiChu
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
            #endregion

            TempData["Message"] = "Thanh toán VnPay thành công!";
            return View("Success");
        }
        #endregion


    }


}



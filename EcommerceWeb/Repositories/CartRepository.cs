using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EcommerceWeb.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly HshopContext _context;

        public CartRepository(HshopContext context)
        {
            _context = context;
        }

        public async Task<HangHoa> GetHangHoa(int id)
        {
            var hangHoa = await _context.HangHoas
                .SingleOrDefaultAsync(p => p.MaHh == id);
            return hangHoa;
        }

		public async Task AddHoaDonAsync(HoaDon hoadon)
		{
			await _context.HoaDons.AddAsync(hoadon);
			await _context.SaveChangesAsync();
		}

		public async Task<HoaDon> GetHoaDonByIdAsync(int id)
		{
			return await _context.HoaDons.SingleOrDefaultAsync(hd => hd.MaHd == id);
		}

		public async Task AddRangeChiTietHdAsync(IEnumerable<ChiTietHd> cthds)
		{
			await _context.ChiTietHds.AddRangeAsync(cthds);
			await _context.SaveChangesAsync();
		}

		public async Task<KhachHang> GetKhachHangByIdAsync(string id)
		{
			return await _context.KhachHangs.SingleOrDefaultAsync(kh => kh.MaKh == id);
		}


		public async Task BeginTransaction()
		{
			await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitTransaction()
		{
			await _context.Database.RollbackTransactionAsync();
		}

		public async Task RollbackTransaction()
		{
			await _context.Database.RollbackTransactionAsync();
		}
	}
}

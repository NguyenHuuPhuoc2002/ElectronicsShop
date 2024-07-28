using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace EcommerceWeb.Repositories
{
    public interface ICartRepository
    {
       Task<HangHoa> GetHangHoa(int id);
		Task AddHoaDonAsync(HoaDon hoadon);
		Task<HoaDon> GetHoaDonByIdAsync(int id);
		Task AddRangeChiTietHdAsync(IEnumerable<ChiTietHd> cthds);
		Task<KhachHang> GetKhachHangByIdAsync(string id);
		Task BeginTransaction();
		Task CommitTransaction();
		Task RollbackTransaction();
	}

}

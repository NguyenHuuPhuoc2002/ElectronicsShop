using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Repositories
{
    public class KhachHangRepository : IKhachHangRepository<KhachHang>
    {
        private readonly HshopContext _context;

        public KhachHangRepository(HshopContext context)
        {
            _context = context;

        }
        public async Task<KhachHang> GetKhachHang(LogInVM model)
        {
            var khachHang = await _context.KhachHangs
                .SingleOrDefaultAsync(kh => kh.MaKh == model.UserName); 
            return khachHang;
        }


        public async Task Register(KhachHang kh)
        {
            _context.Add(kh);
            await _context.SaveChangesAsync();
        }

    }
}

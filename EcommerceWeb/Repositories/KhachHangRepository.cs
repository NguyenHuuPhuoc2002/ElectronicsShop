using AutoMapper;
using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Repositories
{
    public class KhachHangRepository : IKhachHangRepository<KhachHang>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public KhachHangRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<KhachHang> GetKhachHang(LogInVM model)
        {
            var khachHang = await _context.KhachHangs
                .SingleOrDefaultAsync(kh => kh.MaKh == model.UserName);
            return khachHang;
        }

        public async Task<KhachHang> GetKhachHangByIdAsync(string maKh)
        {
            var khachHang = await _context.KhachHangs
               .SingleOrDefaultAsync(kh => kh.MaKh == maKh);
            return khachHang;
        }

        public async Task Register(KhachHang kh)
        {
            _context.Add(kh);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang khachHang)
        {
            _context.Update(khachHang);
            await _context.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class HangHoaAdminRepository : IHangHoaAdminRepository<HangHoaAdminVM>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public HangHoaAdminRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(HangHoaAdminVM product)
        {
            var hangHoa = _mapper.Map<HangHoa>(product);
            await _context.AddAsync(hangHoa);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            var hangHoa = await _context.HangHoas.Include(h => h.ChiTietHds).FirstOrDefaultAsync(h => h.MaHh == id);
            if (hangHoa != null)
            {
                _context.Remove(hangHoa);
                _context.SaveChanges();
            }
        }

        public async Task<HangHoaAdminVM> GetById(int id)
        {
            var data = await _context.HangHoas.FirstOrDefaultAsync(p => p.MaHh == id);
            var hangHoa = _mapper.Map<HangHoaAdminVM>(data);
            return hangHoa;
        }

        public async Task<HangHoaAdminVM> GetByName(string name)
        {
            var data = await _context.HangHoas.FirstOrDefaultAsync(p => p.TenHh.Trim().ToLower().Contains(name.Trim().ToLower()));
            var hangHoa = _mapper.Map<HangHoaAdminVM>(data);
            return hangHoa;
        }

    }
}

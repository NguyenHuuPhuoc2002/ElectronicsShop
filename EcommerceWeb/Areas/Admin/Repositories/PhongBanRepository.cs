using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class PhongBanRepository : IPhongBanRepository<PhongBanModel>
    {
        private readonly IMapper _mapper;
        private readonly HshopContext _context;

        public PhongBanRepository(HshopContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task AddAsync(PhongBanModel phongBan)
        {
            var _pb = await _context.PhongBans.FirstOrDefaultAsync(p => p.TenPb.ToLower().Trim().Contains(phongBan.TenPb.ToLower()));
            if(_pb == null)
            {
                var result = _mapper.Map<PhongBan>(phongBan);
                await _context.AddAsync(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var _pb = await _context.PhongBans.FirstOrDefaultAsync(p => p.MaPb == id);
            if(_pb != null)
            {
                _context.Remove(_pb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhongBanModel>> GetAllAsync()
        {
            var phongBans = await _context.PhongBans.ToListAsync();
            var result = phongBans.Select(p => new PhongBanModel
            {
                MaPb = p.MaPb,
                TenPb = p.TenPb,
                ThongTin = p.ThongTin,
            });
            return result;
        }

        public async Task<PhongBanModel> GetByIdAsync(string id)
        {
            var _pb = await _context.PhongBans.FirstOrDefaultAsync(p => p.MaPb == id);
            var result = _mapper.Map<PhongBanModel>(_pb);
            return result;
        }

        public async Task<PhongBanModel> GetByNameAsync(string name)
        {
            var _pb = await _context.PhongBans.FirstOrDefaultAsync(p => p.TenPb.ToLower() == name.ToLower().Trim());
            var result = _mapper.Map<PhongBanModel>(_pb);
            return result;
        }

        public async Task UpdateAsync(string id, PhongBanModel phongBan)
        {
            var _pb = await _context.PhongBans.FirstOrDefaultAsync(p => p.MaPb == id);
            if(_pb != null)
            {
                _mapper.Map(phongBan, _pb);
                _context.Update(_pb);
                await _context.SaveChangesAsync();
            }
        }
    }
}

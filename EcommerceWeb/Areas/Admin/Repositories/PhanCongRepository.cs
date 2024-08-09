using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class PhanCongRepository : IPhanCongRepository<PhanCongModel>
    {
        private readonly IMapper _mapper;
        private readonly HshopContext _context;

        public PhanCongRepository(HshopContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task AddAsync(PhanCongModel phanCong)
        {
            var _pb = await _context.PhanCongs.FirstOrDefaultAsync(p => (p.MaPc == phanCong.MaPc) 
                                                                      && p.MaNv == phanCong.MaNv);
            var result = new PhanCong
            {
                MaNv = phanCong.MaNv,
                MaPb = phanCong.MaPb,
                NgayPc = phanCong.NgayPc,
                HieuLuc = phanCong.HieuLuc,
            };
            if (_pb == null)
            {
                await _context.AddAsync(result);
                await _context.SaveChangesAsync();
            }
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PhanCongModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PhanCongModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, PhanCongModel phanCong)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class LoaiRepository : ILoaiRepository<LoaiAdminModel>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public LoaiRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(LoaiAdminModel loai)
        {
            if(loai != null)
            {
                var _loai = _mapper.Map<Loai>(loai);
                await _context.AddAsync(_loai);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var loai = await _context.Loais.SingleOrDefaultAsync(p => p.MaLoai == id);
            if (loai != null)
            {
                _context.Remove(loai);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LoaiAdminModel>> GetAllAsync(int page, int pageSize)
        {
            var loais = await _context.Loais.Select(p => new LoaiAdminModel
            {
                MaLoai = p.MaLoai,
                TenLoai = p.TenLoai,
                MoTa = p.MoTa,
            }).ToListAsync();

            return loais.ToPagedList(page, pageSize);
        }

        public async Task<LoaiAdminModel> GetByIdAsync(int id)
        {
            var loai = await _context.Loais.SingleOrDefaultAsync(p => p.MaLoai == id);
            var _loai = _mapper.Map<LoaiAdminModel>(loai);
            return _loai;
        }

        public async Task<LoaiAdminModel> GetByNameAsync(string name)
        {
            var loai = await _context.Loais.FirstOrDefaultAsync(p => p.TenLoai.Trim().ToLower() == name.Trim().ToLower());
            var _loai = _mapper.Map<LoaiAdminModel>(loai);
            return _loai;
        }

        public async Task UpdateAsync(int id, LoaiAdminModel loai)
        {
            var _loai = await _context.Loais.SingleOrDefaultAsync(p => p.MaLoai == id);
            if(_loai != null)
            {
                _mapper.Map(loai, _loai);
                _context.Update(_loai);
                await _context.SaveChangesAsync();
            }
        }
    }
}

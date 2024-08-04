using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class NhaCungCapRepository : INhaCungCapRepository<NhaCungCapAdminModel>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public NhaCungCapRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(NhaCungCapAdminModel nhaCungCap)
        {
            if(nhaCungCap != null)
            {
                var _nhaCungCap = _mapper.Map<NhaCungCap>(nhaCungCap);
                await _context.AddAsync(_nhaCungCap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var nhaCungCap = await _context.NhaCungCaps.SingleOrDefaultAsync(p => p.MaNcc.Trim().ToLower() == id.Trim().ToLower());
            if(nhaCungCap != null)
            {
                _context.Remove(nhaCungCap);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NhaCungCapAdminModel>> GetAllAsync(int page, int pageSize)
        {
            var nhaCungCaps = await _context.NhaCungCaps.Select(p => new NhaCungCapAdminModel
            {
                MaNcc = p.MaNcc,
                TenCongTy = p.TenCongTy,
                NguoiLienLac = p.NguoiLienLac,
                Email = p.Email,
                DienThoai = p.DienThoai,
                DiaChi = p.DiaChi,
                MoTa = p.MoTa
            }).ToListAsync();

            return nhaCungCaps.ToPagedList(page, pageSize);
        }


        public async Task<NhaCungCapAdminModel> GetByIdAsync(string id)
        {
            var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(p => p.MaNcc.Trim().ToLower() == (id.ToLower().Trim()));
            var _nhaCungCap = _mapper.Map<NhaCungCapAdminModel>(nhaCungCap);
            return _nhaCungCap;
        }

        public async Task UpdateAsync(string id, NhaCungCapAdminModel model)
        {
            var nhaCungCap = await _context.NhaCungCaps.SingleOrDefaultAsync(p => p.MaNcc.Trim().ToLower() == id.Trim().ToLower());
            if (nhaCungCap != null)
            {
                _mapper.Map(model, nhaCungCap);
                _context.Update(nhaCungCap);
                await _context.SaveChangesAsync();
            }
        }
    }
}

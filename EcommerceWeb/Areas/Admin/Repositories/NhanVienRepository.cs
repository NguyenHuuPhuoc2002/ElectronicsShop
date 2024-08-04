using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class NhanVienRepository : INhanVienRepository<NhanVienAdminModel>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public NhanVienRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task AddAsync(NhanVienAdminModel nhanVien)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NhanVienAdminModel>> GetAllAsync(int page, int pageSize)
        {
            var nhanViens = await _context.NhanViens.Select(p => new NhanVienAdminModel
            {
                MaNv = p.MaNv,
                HoTen = p.HoTen,
                Email = p.Email,
                MatKhau = p.MatKhau
            }).ToListAsync();

            return nhanViens.ToPagedList(page, pageSize);
        }

        public Task<NhanVienAdminModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<NhanVienAdminModel>> GetSearch(string query, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, NhanVienAdminModel nhanVien)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;
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

        public async Task<IEnumerable<NhanVienAdminModel>> GetAllAsync(string email, int page, int pageSize)
        {
            var nhanViens = await (from nv in _context.NhanViens
                                   join pc in _context.PhanCongs on nv.MaNv equals pc.MaNv
                                   join pb in _context.PhongBans on pc.MaPb equals pb.MaPb
                                   join pq in _context.PhanQuyens on pb.MaPb equals pq.MaPb
                                   where nv.Email == email
                                   select new NhanVienAdminModel
                                   {
                                       MaNv = nv.MaNv,
                                       HoTen = nv.HoTen,
                                       Email = nv.Email,
                                       MatKhau = nv.MatKhau,
                                       Xem = pq.Xem,
                                       Sua = pq.Sua,
                                       Xoa = pq.Xoa,
                                       Them = pq.Them,
                                   }).ToListAsync();
            var distinctNhanViens = nhanViens
    .GroupBy(nv => nv.MaNv)
    .Select(g => g.First())
    .ToList();
            /*var nhanViens = await _context.NhanViens.Select(p => new NhanVienAdminModel
            {
                MaNv = p.MaNv,
                HoTen = p.HoTen,
                Email = p.Email,
                MatKhau = p.MatKhau
            }).ToListAsync();*/

            return distinctNhanViens.ToPagedList(page, pageSize);
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

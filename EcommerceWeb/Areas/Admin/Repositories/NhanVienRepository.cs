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
        public async Task AddAsync(NhanVienAdminModel nhanVien)
        {
            if (nhanVien != null)
            {
                var _nhanVien = _mapper.Map<NhanVien>(nhanVien);
                await _context.NhanViens.AddAsync(_nhanVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var _nhanVien = await _context.NhanViens.FirstOrDefaultAsync(p => p.MaNv.ToLower() == id.ToLower());
            if (_nhanVien != null)
            {
                _context.Remove(_nhanVien);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<NhanVienAdminModel>> GetAllAsync(string email, int page, int pageSize)
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

        public async Task<NhanVienAdminModel> GetByEmailAsync(string email)
        {
            var _nhanvien = await _context.NhanViens.FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower().Trim());
            var result = _mapper.Map<NhanVienAdminModel>(_nhanvien);
            return result;
        }

        public async Task<NhanVienAdminModel> GetByIdAsync(string id)
        {
            var _nhanvien = await _context.NhanViens.FirstOrDefaultAsync(p => p.MaNv.ToLower() == id.ToLower().Trim());
            var result = _mapper.Map<NhanVienAdminModel>(_nhanvien);
            return result;
        }


        public async Task<IEnumerable<NhanVienAdminModel>> GetSearch(string query, int page, int pageSize)
        {
            var nhanViens = await _context.NhanViens.Where(p => p.HoTen.ToLower().Contains(query.ToLower().Trim())
                                                || p.MaNv.ToLower().Contains(query.ToLower().Trim())).ToListAsync();
            var result = nhanViens.Select(p => new NhanVienAdminModel
            {
                MaNv = p.MaNv,
                HoTen = p.HoTen,
                Email = p.Email,
                MatKhau = p.MatKhau,
            });
            return result.ToPagedList(page, pageSize);
        }

        public async Task UpdateAsync(string id, NhanVienAdminModel nhanVien)
        {
            var _nhanVien = await _context.NhanViens.FirstOrDefaultAsync(p => p.MaNv.ToLower() == id.ToLower().Trim());
            _mapper.Map(nhanVien, _nhanVien);
            _context.Update(_nhanVien);
            _context.SaveChanges();
        }
    }
}

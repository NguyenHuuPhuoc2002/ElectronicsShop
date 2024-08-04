using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class LoginRepository : ILoginRepository<NhanVien>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public LoginRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<NhanVien> GetNhanVienAsync(LoginAdminModel nhanVien)
        {
            var _nhanVien = await _context.NhanViens
                .SingleOrDefaultAsync(kh => kh.Email == nhanVien.Email);
            return _nhanVien;
        }

        public Task RegisterAsync(NhanVien nhanVien)
        {
            throw new NotImplementedException();
        }
    }
}

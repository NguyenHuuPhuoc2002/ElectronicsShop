using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Repositories
{
    public class HangHoaRepository : IHangHoaRepository<HangHoaVM>
    {
        private readonly HshopContext _context;

        public HangHoaRepository(HshopContext context) 
        {
            _context = context;
        }

        //Hiển thị toàn bộ sản phẩm
        //xử lí cả sidebar tìm sản phẩm theo loại (nếu có)
        public async Task<IEnumerable<HangHoaVM>> GetAllOrById(int? loai)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = _context.HangHoas.Where(p => p.MaLoai == loai.Value);
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return await result.ToListAsync();
        }

        public async Task<HangHoaVM> GetDetail(int? id)
        {
            var data = _context.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);
            var result = new HangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                Hinh = data.Hinh ?? "",
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                DiemDanhGia = 5,
                SoLuongTon = 10
            };
            return result;
        }

        public async Task<IEnumerable<HangHoaVM>> GetSearch(string query)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = _context.HangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return await result.ToListAsync();
        }
    }
}

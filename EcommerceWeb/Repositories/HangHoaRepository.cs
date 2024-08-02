using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using X.PagedList.EF;

namespace EcommerceWeb.Repositories
{
    public class HangHoaRepository : IHangHoaRepository<HangHoaVM>
    {
        private readonly HshopContext _context;

        public HangHoaRepository(HshopContext context) 
        {
            _context = context;
        }

        public Task AddAsync(HangHoaVM product)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int? id)
        {
            var hangHoa = _context.HangHoas.Include(h => h.ChiTietHds).FirstOrDefault(h => h.MaHh == id);
            if(hangHoa != null)
            {
                _context.Remove(hangHoa);
                _context.SaveChanges();
            }
            
        }

        //Hiển thị toàn bộ sản phẩm
        //xử lí cả sidebar tìm sản phẩm theo loại (nếu có)
        public async Task<IEnumerable<HangHoaVM>> GetAllOrById(int? loai, int page, int pageSize)
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
            return await result.ToPagedListAsync(page, pageSize);
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

        public async Task<IEnumerable<HangHoaVM>> GetSearch( string query, int page, int pageSize)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (!string.IsNullOrEmpty(query))
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
            return await result.ToPagedListAsync(page, pageSize);
        }

        public async Task<IEnumerable<HangHoaVM>> SortAsync(string sort, int page, int pageSize)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if (sort == MySetting.NAME_SORT_ASCENDING)
            {
                hangHoas = _context.HangHoas.OrderBy(p => p.TenHh);
            }
            else if (sort == MySetting.NAME_SORT_DESCENDING)
            {
                hangHoas = _context.HangHoas.OrderByDescending(p => p.TenHh);
            }
            else if (sort == MySetting.PRICE_SORT_DESCENDING)
            {
                hangHoas = _context.HangHoas.OrderByDescending(p => p.DonGia);
            }
            else if (sort == MySetting.PRICE_SORT_ASCENDING)
            {
                hangHoas = _context.HangHoas.OrderBy(p => p.DonGia);
            }
            else
            {
                hangHoas = _context.HangHoas;
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
            return await result.ToPagedListAsync(page, pageSize);
        }
    }
}

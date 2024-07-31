using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace EcommerceWeb.Repositories
{
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository<ChiTietHoaDonVM>
    {
        private readonly HshopContext _context;

        public ChiTietHoaDonRepository(HshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietHoaDonVM>> GetOderDetailByIdAsync(int id)
        {

            var data = _context.HoaDons.Where(p => p.MaHd == id);
            var listCtHoaDon = await(from hd in _context.HoaDons
                                     join ct in _context.ChiTietHds on hd.MaHd equals ct.MaHd
                                     join hh in _context.HangHoas on ct.MaHh equals hh.MaHh
                                     where ct.MaHd == id
                                     select new ChiTietHoaDonVM
                                     {
                                         MaCT = ct.MaCt,
                                         MaHD = hd.MaHd,
                                         MaHH = ct.MaHd,
                                         TenHH = hh.TenHh,
                                         DonGia = ct.DonGia,
                                         SoLuong = ct.SoLuong,
                                         GiamGia = ct.GiamGia,
                                         Anh = hh.Hinh

                                     })
                           .ToListAsync();

            return listCtHoaDon;
        }
    }
}

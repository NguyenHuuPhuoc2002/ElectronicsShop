using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.EF;

namespace EcommerceWeb.Repositories
{
    public class HoaDonRepository : IHoaDonRepository<HoaDonVM>
    {
        private readonly HshopContext _context;

        public HoaDonRepository(HshopContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HoaDonVM>> GetAllByIdAsync(string id, int page, int pageSize)
        {
            var listHoaDon = await (from hd in _context.HoaDons
                                    join tt in _context.TrangThais on hd.MaTrangThai equals tt.MaTrangThai
                                    where hd.MaKh == id
                                    select new HoaDonVM
                                    {
                                        MaNv = hd.MaNv,
                                        MaKh = hd.MaKh,
                                        NgayDat = hd.NgayDat,
                                        NgayGiao = hd.NgayDat.AddDays(3),
                                        HoTen = hd.HoTen,
                                        DiaChi = hd.DiaChi,
                                        DienThoai = hd.DienThoai,
                                        CachThanhToan = hd.CachThanhToan,
                                        PhiVanChuyen = hd.PhiVanChuyen,
                                        TrangThai = tt.TenTrangThai,
                                        MaHd = hd.MaHd,
                                        GhiChu = hd.GhiChu
                                    })
                           .ToPagedListAsync(page, pageSize);

            return listHoaDon;
        }
    }
}

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
                                        MaTrangThai = hd.MaTrangThai,
                                        MaHd = hd.MaHd,
                                        GhiChu = hd.GhiChu
                                    })
                           .ToPagedListAsync(page, pageSize);

            return listHoaDon;
        }

        public async Task<HoaDonVM> GetOderByIdAsync(int id)
        {
            var data = await _context.HoaDons.SingleOrDefaultAsync(p => p.MaHd == id);
            var hoaDon = new HoaDonVM
            {
                MaHd = data.MaHd,
                MaKh = data.MaKh,
                NgayDat = data.NgayDat,
                NgayGiao = data.NgayGiao,
                HoTen = data.HoTen,
                DiaChi = data.DiaChi,
                DienThoai = data.DienThoai,
                CachThanhToan = data.CachThanhToan,
                PhiVanChuyen = data.PhiVanChuyen,
                TrangThai = "",
                MaTrangThai = data.MaTrangThai,
                MaNv = data.MaNv,
                GhiChu = data.GhiChu
            };
            return hoaDon;
        }

        public async Task UpdateStateAsync(int id)
        {
            var data = await _context.HoaDons.SingleOrDefaultAsync(p => p.MaHd == id);
            data.MaTrangThai = -1;
            _context.HoaDons.Update(data);
            _context.SaveChanges();
        }
    }
}

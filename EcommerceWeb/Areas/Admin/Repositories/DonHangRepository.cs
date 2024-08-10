using AutoMapper;
using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public class DonHangRepository : IDonHangRepository<HoaDonVM>
    {
        private readonly HshopContext _context;
        private readonly IMapper _mapper;

        public DonHangRepository(HshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HoaDonVM>> GetAllAsync(int page, int pageSize)
        {
            var data = _context.HoaDons.OrderByDescending(p => p.NgayDat).AsQueryable();
            var listHoaDon = await (from hd in data
                                    join tt in _context.TrangThais on hd.MaTrangThai equals tt.MaTrangThai
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

        public async Task<IEnumerable<HoaDonVM>> GetOderConfirmAsync(int page, int pageSize)
        {
            var data = _context.HoaDons.OrderByDescending(p => p.NgayDat).AsQueryable();
            var listHoaDon = await (from hd in data
                                    join tt in _context.TrangThais on hd.MaTrangThai equals tt.MaTrangThai
                                    where hd.MaTrangThai == 0
                                    select new HoaDonVM
                                    {
                                        MaHd = hd.MaHd,
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
                                        GhiChu = hd.GhiChu
                                    }).ToListAsync();

            return listHoaDon.ToPagedList(page, pageSize);

        }
        public async Task UpdateStateAsync(int id, int state, string maNv)
        {
            var data = await _context.HoaDons.SingleOrDefaultAsync(p => p.MaHd == id);
            data.MaNv = maNv;
            data.MaTrangThai = state;
            _context.HoaDons.Update(data);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<HoaDonVM>> GetSearchAsync(string keyWord, int page, int pageSize)
        {
            var data = _context.HoaDons.OrderByDescending(p => p.NgayDat).AsQueryable();
            var listHoaDon = await (from hd in data
                                    join tt in _context.TrangThais on hd.MaTrangThai equals tt.MaTrangThai
                                    where (hd.HoTen.ToLower().Contains(keyWord.ToLower().Trim()) || hd.DiaChi.ToLower().Contains(keyWord.ToLower().Trim()) 
                                            || hd.CachThanhToan.ToLower().Contains(keyWord.Trim()) || hd.DienThoai.Contains(keyWord.Trim()))
                                            && (hd.MaTrangThai == 0)
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

    }
}

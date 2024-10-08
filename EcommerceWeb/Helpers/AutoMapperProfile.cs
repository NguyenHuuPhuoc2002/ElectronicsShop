﻿using AutoMapper;
using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<RegisterVM, KhachHang>();/*.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM =>
            RegisterVM.HoTen)).ReverseMap();*/
            CreateMap<HangHoa, HangHoaVM>().ReverseMap();
            CreateMap<HangHoa, HangHoaAdminVM>().ReverseMap();
            CreateMap<Loai, LoaiAdminModel>().ReverseMap();
            CreateMap<NhaCungCapAdminModel, NhaCungCap>().ReverseMap();
            CreateMap<NhanVien, NhanVienAdminModel>().ReverseMap();
            CreateMap<PhongBan, PhongBanModel>().ReverseMap();
            CreateMap<KhachHang, KhachHangModel>().ReverseMap();
        }
    }
}

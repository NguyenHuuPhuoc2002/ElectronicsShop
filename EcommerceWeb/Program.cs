using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.Repositories;
using EcommerceWeb.Services;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //connect database
            builder.Services.AddDbContext<HshopContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

            //Repository
            builder.Services.AddScoped<IHangHoaRepository<HangHoaVM>, HangHoaRepository>();
            builder.Services.AddScoped<IHoaDonRepository<HoaDonVM>, HoaDonRepository>();
            builder.Services.AddScoped<IChiTietHoaDonRepository<ChiTietHoaDonVM>, ChiTietHoaDonRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IKhachHangRepository<KhachHang>, KhachHangRepository>();

            //session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/KhachHang/DangNhap";
                options.AccessDeniedPath = "/AccessDenied";

            });

            //đăng kí PaypalClient dạng SingleTon
            builder.Services.AddSingleton(x => new PaypalClient(
                builder.Configuration["PaypalOptions:AppId"],
                builder.Configuration["PaypalOptions:AppSecret"],
                builder.Configuration["PaypalOptions:Mode"]
            ));

            //đăng kí VNPay dạng SingleTon
            builder.Services.AddSingleton<IVnPayService, VnPayService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

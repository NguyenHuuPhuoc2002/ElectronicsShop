using EcommerceWeb.Areas.Admin.Models;
using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface ILoginRepository<T>
    {
        Task RegisterAsync(NhanVien nhanVien);
        Task<T> GetNhanVienAsync(LoginAdminModel nhanVien);
    }
}

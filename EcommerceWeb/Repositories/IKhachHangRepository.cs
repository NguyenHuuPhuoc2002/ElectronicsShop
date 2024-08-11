using EcommerceWeb.Data;
using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Repositories
{
    public interface IKhachHangRepository<T>
    {
        Task Register(KhachHang kh);
        Task<T> GetKhachHang(LogInVM model);
        Task<T> GetKhachHangByIdAsync(string maKh);
        Task UpdateAsync(T khachHang);
    }
}

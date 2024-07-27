using EcommerceWeb.ViewModels;

namespace EcommerceWeb.Repositories
{
    public interface ICartRepository<T>
    {
       Task<T> GetHangHoa(int id);  
    }
}

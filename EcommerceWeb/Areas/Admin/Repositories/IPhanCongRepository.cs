namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface IPhanCongRepository <T>
    {
        Task AddAsync(T phanCong);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<T> GetById(int id);
        Task UpdateAsync(int id, T phanCong);
    }
}

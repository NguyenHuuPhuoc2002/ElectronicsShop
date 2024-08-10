namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface IPhongBanRepository<T>
    {
        Task AddAsync(T phongBan);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(string id);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByNameAsync(string name);
        Task UpdateAsync(string id, T phongBan);
    }
}

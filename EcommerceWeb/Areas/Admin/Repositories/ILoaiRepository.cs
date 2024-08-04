namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface ILoaiRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByNameAsync(string name);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, T loai);
        Task AddAsync(T loai);
        Task<IEnumerable<T>> GetSearch(string? query, int page, int pageSize);
    }
}

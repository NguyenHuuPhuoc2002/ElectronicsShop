namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface INhaCungCapRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task DeleteAsync(string id);
        Task UpdateAsync(string id, T nhaCungCap);
        Task AddAsync(T nhaCungCaps);
        Task<IEnumerable<T>> GetSearch(string? query, int page, int pageSize);
    }
}

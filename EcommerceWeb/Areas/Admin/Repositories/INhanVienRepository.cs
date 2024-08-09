namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface INhanVienRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<T> GetByEmailAsync(string email);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task DeleteAsync(string id);
        Task UpdateAsync(string id, T nhanVien);
        Task AddAsync(T nhanVien);
        Task<IEnumerable<T>> GetSearch(string? query, int page, int pageSize);

    }
}

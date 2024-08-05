namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface INhanVienRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(string email, int page, int pageSize);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, T nhanVien);
        Task AddAsync(T nhanVien);
        Task<IEnumerable<T>> GetSearch(string? query, int page, int pageSize);
    }
}

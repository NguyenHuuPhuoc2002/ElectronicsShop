namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface INhaCungCapRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, T nhaCungCap);
        Task AddAsync(T nhaCungCaps);
    }
}

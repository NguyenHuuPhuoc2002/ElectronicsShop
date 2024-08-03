namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface IHangHoaAdminRepository<T>
    {
        Task AddAsync(T product);
        Task<T> GetByName(string name);
        Task DeleteAsync(int? id);
        Task<T> GetById(int id);
        Task UpdateAsync(int id, T product);
    }
}

namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface IPhongBanRepository<T>
    {
        Task AddAsync(T phongBan);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(string id);
        Task<T> GetById(string id);
        Task UpdateAsync(string id, T phongBan);
    }
}

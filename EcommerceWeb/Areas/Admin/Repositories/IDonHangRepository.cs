namespace EcommerceWeb.Areas.Admin.Repositories
{
    public interface IDonHangRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
        Task<T> GetOderByIdAsync(int id);
        Task<IEnumerable<T>> GetOderConfirmAsync(int page, int pageSize);
        Task UpdateStateAsync(int id, int state);
    }
}

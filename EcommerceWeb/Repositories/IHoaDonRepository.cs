namespace EcommerceWeb.Repositories
{
    public interface IHoaDonRepository<T>
    {
        Task<IEnumerable<T>> GetAllByIdAsync(string id, int page, int pageSize);

        Task UpdateStateAsync(int id);
        Task<T> GetOderByIdAsync(int id);

    }
}

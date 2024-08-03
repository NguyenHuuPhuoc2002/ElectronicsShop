namespace EcommerceWeb.Repositories
{
    public interface IHangHoaRepository<T>
    {
        Task<IEnumerable<T>> GetAllOrById(int? loai, int page, int pageSize);
        Task<IEnumerable<T>> GetSearch(string? query, int page, int pageSize);
        Task<T> GetDetail(int? id);
        Task<IEnumerable<T>> SortAsync(string sort, int page, int pageSize);
        Task AddAsync(T product);
    }
}

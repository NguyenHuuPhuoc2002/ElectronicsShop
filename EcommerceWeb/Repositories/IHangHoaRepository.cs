namespace EcommerceWeb.Repositories
{
    public interface IHangHoaRepository<T>
    {
        Task<IEnumerable<T>> GetAllOrById(int? loai);
        Task<IEnumerable<T>> GetSearch(string? query);
        Task<T> GetDetail(int? id);
    }
}

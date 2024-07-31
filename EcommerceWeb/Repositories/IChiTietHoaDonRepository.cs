namespace EcommerceWeb.Repositories
{
    public interface IChiTietHoaDonRepository<T>
    {
        Task<IEnumerable<T>> GetOderDetailByIdAsync(int id);
    }
}

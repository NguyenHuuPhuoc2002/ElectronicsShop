using EcommerceWeb.Data;
using EcommerceWeb.Helpers;
using EcommerceWeb.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWeb.Repositories
{
    public class CartRepository : ICartRepository<HangHoa>
    {
        private readonly HshopContext _context;

        public CartRepository(HshopContext context)
        {
            _context = context;
        }

        public async Task<HangHoa> GetHangHoa(int id)
        {
            var hangHoa = await _context.HangHoas
                .SingleOrDefaultAsync(p => p.MaHh == id);
            return hangHoa;
        }

    }
}

using BTL_WEBNC.Models;
using System.Threading.Tasks;

namespace BTL_WEBNC.Repositories
{
    public interface ICartRepository
    {
        Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity);
        Task<int> GetCartCountAsync(string userId);
    }
}

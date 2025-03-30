using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;

namespace BTL_WEBNC.Services
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity);
        Task<int> GetCartCountAsync(string userId);
    }
}

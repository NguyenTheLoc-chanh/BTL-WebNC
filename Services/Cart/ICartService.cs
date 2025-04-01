using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;

namespace BTL_WEBNC.Services
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity);
        Task<int> GetCartCountAsync(string userId);
        Task<List<CartDetailModel>> GetCartDetailsByUserId(string userId);

        Task<bool> RemoveFromCart(int cartDetailId);
        Task<bool> IncreaseQuantityAsync(int cartDetailId);
        Task<bool> DecreaseQuantityAsync(int cartDetailId);
        Task<List<Product>> GetProductsAsync();
    }
}

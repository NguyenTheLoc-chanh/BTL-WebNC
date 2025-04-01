using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using System.Threading.Tasks;

namespace BTL_WEBNC.Repositories
{
    public interface ICartRepository
    {
        Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity);
        Task<int> GetCartCountAsync(string userId);
        Task<List<CartDetailModel>> GetCartDetailsByUserId(string userId);
        Task<bool> RemoveFromCartAsync(int cartDetailId);

        Task<bool> IncreaseQuantityAsync(int cartDetailId);
        Task<bool> DecreaseQuantityAsync(int cartDetailId);
        Task<List<Product>> GetProductsAsync();
    }
}

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
        Task<List<CartDetailModel>> GetSelectedCartDetailsAsync(string userId, List<int> cartDetailIds);
        Task<float> CalculateTotalAmountAsync(List<int> cartDetailIds);
        Task<List<Address>> GetUserAddressesAsync(string userId);

        Task<bool> AddAddressAsync(string userId, string FullName, string PhoneNumber, string StreetAddress, bool IsDefault);
        Task<bool> UpdateAddressAsync(Address updatedAddress);
        Task<(bool Success, string Message, int OrderId)> ConfirmCheckoutAsync(string userId, CheckoutRequest request);
    }
}

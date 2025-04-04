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
        Task<List<CartDetailModel>> GetSelectedCartDetailsAsync(string userId, List<int> cartDetailIds);
        Task<float> CalculateTotalAmountAsync(List<int> cartDetailIds);
        Task<List<Address>> GetUserAddressesAsync(string userId);

        Task<bool> AddAddressAsync(string userId, string FullName, string PhoneNumber, string StreetAddress, bool IsDefault);
        Task<bool> UpdateAddressAsync(Address updatedAddress);
    }
}

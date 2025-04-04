
using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using BTL_WEBNC.Repositories;

namespace BTL_WEBNC.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity)
        {
            return await _cartRepository.AddToCartAsync(userId,productId, SizeId, quantity);
        }

        public async Task<bool> DecreaseQuantityAsync(int cartDetailId)
        {
            return await _cartRepository.DecreaseQuantityAsync(cartDetailId);
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _cartRepository.GetCartCountAsync(userId);
        }

        public async Task<List<CartDetailModel>> GetCartDetailsByUserId(string userId)
        {
            return await _cartRepository.GetCartDetailsByUserId(userId);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _cartRepository.GetProductsAsync();
        }

        public async Task<float> CalculateTotalAmountAsync(List<int> cartDetailIds)
        {
            return await _cartRepository.CalculateTotalAmountAsync(cartDetailIds);
        }
        public async Task<List<CartDetailModel>> GetSelectedCartDetailsAsync(string userId, List<int> cartDetailIds)
        {
            return await _cartRepository.GetSelectedCartDetailsAsync(userId, cartDetailIds);
        }

        public async Task<bool> IncreaseQuantityAsync(int cartDetailId)
        {
            return await _cartRepository.IncreaseQuantityAsync(cartDetailId);
        }

        public async Task<bool> RemoveFromCart(int cartDetailId)
        {
            return await _cartRepository.RemoveFromCartAsync(cartDetailId);
        }

        public async Task<List<Address>> GetUserAddressesAsync(string userId)
        {
            return await _cartRepository.GetUserAddressesAsync(userId);
        }

        public async Task<bool> AddAddressAsync(string userId, string FullName, string PhoneNumber, string StreetAddress, bool IsDefault)
        {
            return await _cartRepository.AddAddressAsync(userId,FullName,PhoneNumber,StreetAddress,IsDefault);
        }

        public async Task<bool> UpdateAddressAsync(Address updatedAddress)
        {
            return await _cartRepository.UpdateAddressAsync(updatedAddress);
        }
    }
}
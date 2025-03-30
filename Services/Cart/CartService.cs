
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

        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _cartRepository.GetCartCountAsync(userId);
        }
    }
}
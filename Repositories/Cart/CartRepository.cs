using BTL_WEBNC.Models;
using BTL_WEBNC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDBContext _context;

        public CartRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToCartAsync(string userId, int productId, int SizeId, int quantity)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == userId);
            if (cart == null)
            {
                cart = new CartModel 
                {
                    Id = userId,
                    CreatedAt = DateTime.UtcNow 
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartDetail = await _context.CartDetails.FirstOrDefaultAsync(cd => cd.CartId == cart.CartId && cd.Product_Id == productId);
            if (cartDetail != null)
            {
                cartDetail.Quantity += quantity;
            }
            else
            {
                cartDetail = new CartDetails
                {
                    CartId = cart.CartId,
                    Product_Id = productId,
                    Size_Id = SizeId,
                    Quantity = quantity
                };
                _context.CartDetails.Add(cartDetail);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCartCountAsync(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == userId);

            return cart != null 
                ? await _context.CartDetails
                    .Where(cd => cd.CartId == cart.CartId)
                    .Select(cd => cd.Product_Id)
                    .Distinct()
                    .CountAsync()
                : 0;
        }
    }
}

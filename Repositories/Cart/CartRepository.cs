using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
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

        public async Task<bool> DecreaseQuantityAsync(int cartDetailId)
        {
            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail == null || cartDetail.Quantity <= 1) return false;

            cartDetail.Quantity -= 1;
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
        public async Task<List<CartDetailModel>> GetCartDetailsByUserId(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == userId);
            if (cart == null)
            {
                return new List<CartDetailModel>();
            }

            return await _context.CartDetails
                        .Where(cd => cd.CartId == cart.CartId)
                        .Select(cd => new CartDetailModel
                        {
                            CartDetailId = cd.CartDetailId,
                            ProductID = cd.Product_Id,
                            Quantity = cd.Quantity,
                            ProductName = cd.Product.Name,
                            ProductPrice = cd.Product.Price,
                            ImageUrl = cd.Product.Images
                        })
                        .ToListAsync();
        }

        public async Task<bool> IncreaseQuantityAsync(int cartDetailId)
        {
            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail == null) return false;

            cartDetail.Quantity += 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailId)
        {
             var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail != null)
            {
                _context.CartDetails.Remove(cartDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

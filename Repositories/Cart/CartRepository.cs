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
                            ImageUrl = cd.Product.Images,
                        })
                        .ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.CreatedAt) // Sắp xếp sản phẩm mới nhất
                .Take(10)
                .ToListAsync();
        }

        // Chuyển sang thanh toán
        public async Task<float> CalculateTotalAmountAsync(List<int> cartDetailIds)
        {
            return await _context.CartDetails
                .Where(cd => cartDetailIds.Contains(cd.CartDetailId))
                .SumAsync(cd => cd.Product.Price * cd.Quantity);
        }

        public async Task<List<CartDetailModel>> GetSelectedCartDetailsAsync(string userId, List<int> cartDetailIds)
        {
            return await _context.CartDetails
            .Include(cd => cd.Product)
            .Where(cd => cd.Cart.Id == userId && cartDetailIds.Contains(cd.CartDetailId))
            .Select(cd => new CartDetailModel
            {
                CartDetailId = cd.CartDetailId,
                ProductName = cd.Product.Name,
                ProductPrice = cd.Product.Price,
                Quantity = cd.Quantity,
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

        public async Task<List<Address>> GetUserAddressesAsync(string userId)
        {
            return await _context.AddressUser
            .Where(a => a.user_Id == userId)
            .ToListAsync();
        }

        public async Task<bool> AddAddressAsync(string userId, string FullName, string PhoneNumber, string StreetAddress, bool IsDefault)
        {
            try
            {
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrWhiteSpace(FullName))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(StreetAddress))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return false;
                }

                // Nếu đây là địa chỉ mặc định, hủy mặc định của các địa chỉ khác
                if (IsDefault)
                {
                    var defaultAddresses = await _context.AddressUser
                        .Where(a => a.user_Id == userId && a.IsDefault)
                        .ToListAsync();

                    foreach (var address in defaultAddresses)
                    {
                        address.IsDefault = false;
                    }
                }

                // Thêm địa chỉ mới
                var addressUser = new Address{
                    FullName = FullName,
                    PhoneNumber = PhoneNumber,
                    StreetAddress = StreetAddress,
                    IsDefault = IsDefault,
                    user_Id = userId,
                };
                _context.AddressUser.Add(addressUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                // Log lỗi tổng quát
                return false;
            }
        }

        public async Task<bool> UpdateAddressAsync(Address updatedAddress)
        {
            var address = await _context.AddressUser.FindAsync(updatedAddress.Id);
            if (address != null)
            {
                address.FullName = updatedAddress.FullName;
                address.PhoneNumber = updatedAddress.PhoneNumber;
                address.StreetAddress = updatedAddress.StreetAddress;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

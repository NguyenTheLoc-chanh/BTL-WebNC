using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace BTL_WEBNC.ProductManagement.Sellers
{
    [Authorize(Roles = "Seller")]
    public class CreateModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ProductInputModel Product { get; set; }

        public List<Category> Categories { get; set; }
        public List<Size> Sizes { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy danh sách danh mục từ cơ sở dữ liệu
            Categories = await _context.Categories.ToListAsync();
            Sizes = await _context.Sizes.ToListAsync();
            if (Categories == null)
            {
                Categories = new List<Category>();
            }

            if (Sizes == null)
            {
                Sizes = new List<Size>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Any())
                    .Select(kvp => new
                    {
                        Field = kvp.Key,
                        Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    });

                return new JsonResult(new { success = false, message = "Dữ liệu không hợp lệ.", errors });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy ID người dùng đang đăng nhập
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return new JsonResult(new { success = false, message = "Không xác định được người dùng." });
                }

                // Tìm seller theo userId
                var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.user_Id == userId);
                if (seller == null)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy người bán." });
                }
                Product.seller_Id = seller.seller_Id;
                // Tạo sản phẩm mới
                var newProduct = new Product
                {
                    Name = Product.Name,
                    Price = (float)Product.Price,
                    Description = Product.Description,
                    Stock = Product.Stock,
                    Images = Product.Images,
                    category_id = Product.category_id,
                    seller_Id = Product.seller_Id,
                    CreatedAt = DateTime.Now,
                    Status = ProductStatus.Active
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync(); // Để lấy Product_Id

                // Thêm các size sản phẩm nếu có
                if (Product.Sizes != null && Product.Sizes.Any())
                {
                    foreach (var size in Product.Sizes)
                    {
                        var productSize = new ProductSize
                        {
                            Product_Id = newProduct.Product_Id,
                            Size_Id = size.Size_Id,
                            Stock = size.Stock
                        };
                        _context.ProductSizes.Add(productSize);
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToPage("./Index"); // Redirect to the product list page after successful creation
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Ghi log lỗi nếu cần
                return new JsonResult(new { success = false, message = "Đã xảy ra lỗi khi thêm sản phẩm.", error = ex.Message });
            }
        }


        public class ProductInputModel
        {
            [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
            public string Name { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
            public decimal Price { get; set; }

            public string Description { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không hợp lệ.")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "Hình ảnh là bắt buộc.")]
            public string Images { get; set; } // JSON string of image URLs

            [Required(ErrorMessage = "Danh mục là bắt buộc.")]
            public int category_id { get; set; }

            public string? seller_Id { get; set; }

            public List<ProductSizeInputModel> Sizes { get; set; } = new List<ProductSizeInputModel>();
        }

        public class ProductSizeInputModel
        {
            public int Product_Id { get; set; }
            public int Size_Id { get; set; }
            public int Stock { get; set; }
        }
    }
}
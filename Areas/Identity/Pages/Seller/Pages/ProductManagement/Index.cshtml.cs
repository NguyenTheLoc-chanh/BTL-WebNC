using BTL_WEBNC.Models;
using BTL_WEBNC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBNC.ProductManagement.Sellers
{
    [Authorize(Roles = "Seller")]
    public class IndexModel : PageModel
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ProductManagementViewModel> Products { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var seller = _context.Sellers.FirstOrDefault(s => s.user_Id == userId);
            if (seller == null)
            {
            Products = new List<ProductManagementViewModel>();
            }
            else
            {
            Products = _context.Products
                .Where(p => p.seller_Id == seller.seller_Id)
                .Select(p => new ProductManagementViewModel
                {
                    Product_Id = p.Product_Id,
                    Name = p.Name,
                    Price = p.Price,
                    Images = p.Images,
                    Stock = p.Stock,
                    Description = p.Description,
                    Status = p.Status,
                    ApprovalStatus = p.ApprovalStatus ?? ProductApprovalStatus.Pending,
                    seller_Id = p.seller_Id,
                    category_id = p.category_id,
                    CategoryName = _context.Categories
                        .Where(c => c.category_id == p.category_id)
                        .Select(c => c.category_Name)
                        .FirstOrDefault() ?? string.Empty
                    })
                .ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteRequest request)
        {   
            Console.WriteLine($"Deleting product with ID: {request.id}");
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Product_Id == request.id);
            if (product == null)
            {
                return new JsonResult(new { success = false, message = "Sản phẩm không tồn tại hoặc bạn không có quyền xóa." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = "Xóa sản phẩm thành công!" });
        }
        public class DeleteRequest
        {
            public int id { get; set; }
        }
        
    }
}
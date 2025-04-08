using BTL_WEBNC.ViewModels;
using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Controllers
{
    
    
  
    public class ProductController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDBContext _dbContext;

        public ProductController(UserManager<User> userManager, AppDBContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET: /Seller/CreateProduct
        
        [HttpGet]
        public IActionResult CreateProduct()
        {
            // Giả sử _dbContext.Categories là DbSet chứa danh mục sản phẩm
            ViewBag.Categories = _dbContext.Categories.ToList();
            return View();
        }

        // POST: /Seller/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            // Log giá trị các thuộc tính của model để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Name: {model.Name}, Price: {model.Price}, Stock: {model.Stock}, CategoryId: {model.CategoryId}");

            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                    return View();
                }

                var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.user_Id == currentUser.Id);
                if (seller == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin người bán. Vui lòng liên hệ admin.";
                    return View();
                }

                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = (float)model.Price,
                    Stock = model.Stock,
                    seller_Id = seller.seller_Id,
                    category_id = model.CategoryId,

                    // Dùng giá trị mặc định "Pending" để chỉ ra trạng thái "chưa duyệt"
                    ApprovalStatus = ProductApprovalStatus.Pending,

                    CreatedAt = DateTime.UtcNow
                };

                // Xử lý upload ảnh nếu có
                if (model.ImageFile != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine("wwwroot/images/products", uniqueFileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }
                        product.Images = "/images/products/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Lỗi khi tải ảnh: {ex.Message}");
                        return View();
                    }
                }

                try
                {
                    await _dbContext.Products.AddAsync(product);
                    await _dbContext.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Sản phẩm đã được gửi để duyệt.";
                    return RedirectToAction("Index","Home");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu sản phẩm: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi lưu sản phẩm. Vui lòng thử lại.";
                }
            }

            return View();
        }


    }
}

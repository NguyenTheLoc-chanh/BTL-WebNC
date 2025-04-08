using BTL_WEBNC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_WEBNC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product/[action]")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;

        public ProductController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Admin/Product/PendingProducts
        [HttpGet]
        public async Task<IActionResult> PendingProducts()
        {
            var pendingProducts = await _dbContext.Products
                .Where(p => p.ApprovalStatus == ProductApprovalStatus.Pending)
                .ToListAsync();
            return View(pendingProducts);
        }

        // POST: /Admin/Product/ApproveProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveProduct(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            product.ApprovalStatus = ProductApprovalStatus.Approved;
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sản phẩm đã được duyệt.";
            return RedirectToAction(nameof(PendingProducts));
        }

        // POST: /Admin/Product/RejectProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectProduct(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            product.ApprovalStatus = ProductApprovalStatus.Rejected;
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sản phẩm đã bị từ chối.";
            return RedirectToAction(nameof(PendingProducts));
        }
    }
}
